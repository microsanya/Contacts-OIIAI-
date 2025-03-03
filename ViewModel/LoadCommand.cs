using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Класс, хранящий в себе реализацию подгрузки контакта.
    /// </summary>
    class LoadCommand : ICommand
    {
        /// <summary>
        /// Делегат, который устанавливает подгруженный контакт.
        /// </summary>
        private readonly Action<Contact> _setContact;

        /// <summary>
        /// Инициализирует новый экземпляр команды LoadCommand<see cref="LoadCommand"/>.
        /// </summary>
        /// <param name="setContact">Делегат, передает подгруженный контакт.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если передан null.</exception>
        public LoadCommand(Action<Contact> setContact)
        {
            if (setContact == null)
            {
                throw new ArgumentNullException(nameof(setContact));
            }
            _setContact = setContact;
        }

        /// <summary>
        ///  Событие, вызывается когда изменяется состояние выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Метод, определяющий, можно ли выполнить команду загрузки.
        /// </summary>
        /// <param name="parameter">Любой параметр.</param>
        /// <returns>Возвращает true.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполняет команду загрузки контакта из файла.
        /// </summary>
        /// <param name="parameter">Любой параметр.</param>
        public void Execute(object parameter)
        {
            var contact = ContactSerializer.LoadContact();
            if (contact != null)
            {
                _setContact(contact);
            }
        }
    }
}
