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
    public static class ContactSerializer
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
        /// <param name="contacts">Список контактов для сохранения.</param>
        public static void SaveContact(IEnumerable<Contact> contacts)
        {
            if (contacts == null)
            {
                throw new ArgumentNullException(nameof(contacts), "Контакты не могут быть null.");
            }

            CreateDirectory();
            var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Подгружает контакт из файла.
        /// </summary>
        /// <returns>Возвращает список контактов, если файл существует.</returns>
        public static List<Contact> LoadContact()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Contact>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Contact>>(json);
        }
    }
}
