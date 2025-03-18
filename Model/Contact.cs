using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Model
{
    /// <summary>
    /// Класс, хранящий в себе контакт пользователя (имя, телефон, почта).
    /// </summary>
    public class Contact : INotifyPropertyChanged
    {
        /// <summary>
        /// Приватное поле для имени пользователя.
        /// </summary>
        private string _name;

        /// <summary>
        /// Приватное поле для номера телефона.
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Приватное поле для почтового адреса.
        /// </summary>
        private string _email;


        /// <summary>
        /// Принимает и задаёт имя пользователя.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Принимает и задает номер телефона пользователя.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber == value)
                {
                    return;
                }

                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        /// <summary>
        /// Принимает и задаёт почту пользователя.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                {
                    return;
                }

                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Конструктор Contact c параметрами.
        /// Инициализирует новый класс <see cref="Contact"/> с указанными значениями.
        /// </summary>
        /// <param name="name">Имя контакта</param>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <param name="email">Почта контакта</param>
        public Contact(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        /// <summary>
        /// Конструктор класса <see cref="Contact"/>. 
        /// Инициализирует поля значениями по умолчанию.
        /// </summary>
        public Contact()
        {
            Name = "Имя Фамилия Отчество";
            PhoneNumber = "89095490417";
            Email = "examplemail@gmail.com";
        }

        /// <summary>
        /// Событие, вызывается когда изменяется состояние объекта.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для обновления интерфейса.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
