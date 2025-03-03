using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace View.Model.Services
{
    /// <summary>
    /// Класс, содержащий реализацию сохранения и подгрузки контактов (статический).
    /// </summary>
    static class ContactSerializer
    {
        /// <summary>
        /// Полный путь к файлу, в котором хранятся контакты.
        /// </summary>
        private static string _filePath = Path.Combine(@"D:\Документы", "Contacts", "contacts.json");

        /// <summary>
        /// Метод, который устанавливает нужную папку, куда сохранять, и если её нет, создаёт её.
        /// </summary>
        public static void CreateDirectory()
        {
            _filePath = Path.Combine(@"D:\Документы", "Contacts", "contacts.json");

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Сериализует данные и сохраняет.
        /// </summary>
        /// <param name="contact">Объект контакта для сохранения.</param>
        public static void SaveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "Контакт не может быть null.");
            }

            CreateDirectory();
            var json = JsonConvert.SerializeObject(contact, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Подгружает контакт из файла.
        /// </summary>
        /// <returns>Объект <see cref="Contact"/>, если файл существует, иначе null.</returns>
        public static Contact LoadContact()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<Contact>(json);
        }
    }
}
