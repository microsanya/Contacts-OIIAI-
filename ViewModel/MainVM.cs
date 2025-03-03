﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using View.Model;

namespace View.ViewModel
{
    /// <summary>
    /// Класс VM для главного окна.
    /// </summary>
    class MainVM : INotifyPropertyChanged
    {

        /// <summary>
        /// Текущий контакт.
        /// </summary>
        private Contact _currentContact;

        /// <summary>
        /// Команда для сохранения контакта.
        /// </summary>
        public SaveCommand SaveCommand { get; }

        /// <summary>
        /// Команда для загрузки контакта.
        /// </summary>
        public LoadCommand LoadCommand { get; }

        /// <summary>
        /// Событие, срабатывает когда происходят изменения в свойствах.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MainVM"/>.
        /// </summary>
        public MainVM()
        {
            _currentContact = new Contact();

            SaveCommand = new SaveCommand(() => CurrentContact);
            LoadCommand = new LoadCommand(loadedContact => UpdateContact(loadedContact));
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
        /// Возвращает и задаёт текущее имя контакта, если оно отличается.
        /// </summary>
        public string Name
        {
            get => _currentContact.Name;
            set
            {
                if (_currentContact.Name == value)
                {
                    return;
                }

                _currentContact.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Возвращает и задает текущий номер телефона контакта.
        /// </summary>
        public string PhoneNumber
        {
            get => _currentContact.PhoneNumber;
            set
            {
                if (_currentContact.PhoneNumber == value)
                {
                    return;
                }

                _currentContact.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Email
        {
            get => _currentContact.Email;
            set
            {
                if (_currentContact.Email == value)
                {
                    return;
                }

                _currentContact.Email = value;
                OnPropertyChanged(nameof(Email));
            }
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

                _currentContact = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Обновляет текущий контакт новыми данными.
        /// </summary>
        /// <param name="contact">Загруженный контакт.</param>
        private void UpdateContact(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            CurrentContact = contact;
        }
    }
}
