using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Model
{
    /// <summary>
    /// Класс, хранящий в себе контакт пользователя (имя, телефон, почта).
    /// </summary>
    class Contact
    {
        /// <summary>
        /// Приватное поле для имени пользователя.
        /// </summary>
        private string _name;

        /// <summary>
        /// Приватное поле для номера телефона.
        /// </summary>
        private int _phoneNumber;

        /// <summary>
        /// Приватное поле для почтового адреса.
        /// </summary>
        private string _email;


        /// <summary>
        /// Принимает и задаёт имя пользователя.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Принимает и задает номер телефона пользователя.
        /// </summary>
        public int PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
            }
        }

        /// <summary>
        /// Принимает и задаёт почту пользователя.
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        /// <summary>
        /// Конструктор Contact c параметрами.
        /// Инициализирует новый класс с указанными значениями.
        /// </summary>
        /// <param name="name">Имя контакта</param>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <param name="email">Почта контакта</param>
        public Contact(string name, int phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        /// <summary>
        /// Конструктор класса Contact. 
        /// Инициализирует поля значениями по умолчанию.
        /// </summary>
        public Contact()
        {
            Name = "Имя Фамилия Отчество";
            PhoneNumber = 0;
            Email = "examplemail@gmail.com";
        }
    }
}
