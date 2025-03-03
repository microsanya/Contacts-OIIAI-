using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    /// <summary>
    /// Класс, хранящий в себе реализацию сохранения контакта.
    /// </summary>
    class SaveCommand : ICommand
    {
        /// <summary>
        /// Делегат, возвращает текущий контакт, чтобы его сохранить.
        /// </summary>
        private readonly Func<Contact> _getContact;

        /// <summary>
        /// Инициализирует новый экземпляр команды SaveCommand <see cref="SaveCommand"/>.
        /// </summary>
        /// <param name="getContact">Делегат, возвращающая текущий контакт.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если передан null.</exception>
        public SaveCommand(Func<Contact> getContact)
        {
            if (getContact == null)
            {
                throw new ArgumentNullException(nameof(getContact));
            }
            _getContact = getContact;
        }

        /// <summary>
        /// Событие, вызывается когда изменяется состояние выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Метод, определяющий, можно ли выполнить команду сохранения.
        /// </summary>
        /// <param name="parameter">Любой параметр.</param>
        /// <returns>Возвращает true.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Выполняет команду сохранения контакта в файл.
        /// </summary>
        /// <param name="parameter">Любой параметр.</param>
        public void Execute(object parameter)
        {
            var contact = _getContact();
            if (contact != null)
            {
                ContactSerializer.SaveContact(contact);
            }
        }
    }
}
