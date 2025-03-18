using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Класс VM для главного окна.
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Текущий контакт.
        /// </summary>
        private Contact _currentContact;

        /// <summary>
        /// Значение, указывающее, находятся ли поля доступными только для чтения.
        /// </summary>
        private bool _isReadOnlyMode = true;

        /// <summary>
        /// Событие, срабатывает когда происходят изменения в свойствах.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Команда для добавления нового контакта.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Команда для редактирования выбранного контакта.
        /// </summary>
        public ICommand EditCommand { get; }

        /// <summary>
        /// Команда для удаления выбранного контакта.
        /// </summary>
        public ICommand RemoveCommand { get; }

        /// <summary>
        /// Команда для применения изменений в выбранном контакте.
        /// </summary>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Список контактов.
        /// </summary>
        public ObservableCollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            ContactSerializer.CreateDirectory();
            Contacts = new ObservableCollection<Contact>(ContactSerializer.LoadContact());
            AddCommand = new RelayCommand(AddContact, CanAddContact);
            EditCommand = new RelayCommand(EditContact, CanEditContact);
            RemoveCommand = new RelayCommand(RemoveContact, CanRemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact, CanApplyContact);
        }

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для обновления интерфейса.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Возвращает и задает текущий контакт.
        /// </summary>
        public Contact CurrentContact
        {
            get => _currentContact;
            set
            {
                if (_currentContact == value)
                {
                    return;
                }

                CancelEdit();
                _currentContact = value;
                OnPropertyChanged(nameof(IsAddOrEditMode));
                OnPropertyChanged(nameof(CurrentContact));
                OnPropertyChanged(nameof(IsContactSelected));
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее, редактируется ли контакт.
        /// </summary>
        public bool IsAddOrEditMode => !IsReadOnlyMode;

        /// <summary>
        /// Получает или задает значение, указывающее, находится ли приложение в режиме редактирования.
        /// </summary>
        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set
            {
                _isReadOnlyMode = value;
                OnPropertyChanged(nameof(IsReadOnlyMode));
                OnPropertyChanged(nameof(IsAddOrEditMode));
            }
        }

        /// <summary>
        /// Проверяет, выбран ли контакт.
        /// </summary>
        public bool IsContactSelected => _currentContact != null;

        /// <summary>
        /// Редактирует выбранный контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void EditContact(object parameter)
        {
            IsReadOnlyMode = false;
        }

        /// <summary>
        /// Удаляет выбранный контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void RemoveContact(object parameter)
        {
            if (CurrentContact == null)
            {
                return;
            }

            int index = Contacts.IndexOf(CurrentContact);
            Contacts.Remove(CurrentContact);

            if (Contacts.Any())
            {
                CurrentContact = index < Contacts.Count ? Contacts[index] : Contacts.Last();
            }
            else
            {
                CurrentContact = null;
            }

            ContactSerializer.SaveContact(Contacts);
        }

        /// <summary>
        /// Добавляет новый контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void AddContact(object parameter)
        {
            CurrentContact = null;
            CurrentContact = new Contact();
            IsReadOnlyMode = false;
        }

        /// <summary>
        /// Применяет изменения к выбранному контакту.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void ApplyContact(object parameter)
        {
            if (parameter is not BindingGroup bindingGroup)
            {
                return;
            }

            bindingGroup.CommitEdit();
            if (CurrentContact == null)
            {
                return;
            }

            if (!Contacts.Contains(CurrentContact))
            {
                Contacts.Add(CurrentContact);
            }

            IsReadOnlyMode = true;
            ContactSerializer.SaveContact(Contacts);
        }

        /// <summary>
        /// Отменяет редактирование контакта.
        /// </summary>
        private void CancelEdit()
        {
            IsReadOnlyMode = true;
            OnPropertyChanged(nameof(IsReadOnlyMode));
            OnPropertyChanged(nameof(IsAddOrEditMode));
        }

        /// <summary>
        /// Проверяет, можно ли добавить контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Возвращает <c>true</c>, если контакт можно добавить; иначе <c>false</c>.</returns>
        private bool CanAddContact(object parameter) => !IsAddOrEditMode;

        /// <summary>
        /// Проверяет, можно ли редактировать выбранный контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Возвращает <c>true</c>, если контакт можно редактировать; иначе <c>false</c>.</returns>
        private bool CanEditContact(object parameter) => IsContactSelected && !IsAddOrEditMode;

        /// <summary>
        /// Проверяет, можно ли удалить выбранный контакт.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Возвращает <c>true</c>, если контакт можно удалить; иначе <c>false</c>.</returns>
        private bool CanRemoveContact(object parameter) => IsContactSelected && !IsAddOrEditMode;

        /// <summary>
        /// Проверяет, можно ли применить изменения для выбранного контакта.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>Возвращает <c>true</c>, если изменения можно применить; иначе <c>false</c>.</returns>
        private bool CanApplyContact(object parameter) => IsAddOrEditMode;
    }
}
