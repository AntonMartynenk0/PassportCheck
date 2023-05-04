using CheckPass.Entities;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CheckPass
{
    public class ImportData
    {
        // Функція, яка імпортує дані з CSV файлу та повертає список паспортів
        public List<Passport> ImportFromCSV(string fileName)
        {
            List<Passport> passports = new List<Passport>();
            string line;
            string[] passport;

            // Відкрити файл для читання та пропустити перший рядок
            using (StreamReader sr = new StreamReader(fileName))
            {
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null) // Перебір рядків у файлі
                {
                    passport = line.Split(';'); // Розбити рядок на частини
                    passports.Add(new Passport(passport[0], passport[1], passport[2], passport[3], Convert.ToDateTime(passport[4]))); // Додати новий паспорт до списку, використовуючи розбиті дані та конвертування дати
                }
            }
            return passports; // Повернути список паспортів
        }


        // Функція, яка імпортує користувачів з CSV файлу та повертає список користувачів
        public List<User> ImportUsersFromCSV(string fileName)
        {
            List<User> users = new List<User>();
            string line;
            string[] user;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    sr.ReadLine(); // Пропустити перший рядок з заголовками
                    while ((line = sr.ReadLine()) != null) // Перебір рядків у файлі
                    {
                        user = line.Split(';'); // Розбити рядок на частини
                        Role role;
                        role = user[4] == "ADMIN" ? Role.ADMIN : Role.USER; // Визначити роль користувача
                        users.Add(new User(user[0], user[1], user[2], user[3], role, user[5])); // Додати нового користувача до списку, використовуючи розбиті дані та визначену роль
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Вивести повідомлення про помилку в консоль
            }
            return users; // Повернути список користувачів
        }

        // Функція, яка імпортує дані з бінарного файлу та повертає список об'єктів заданого типу
        public List<T> ImportFromBinary<T>(string fileName, T obj)
        {
            List<T> objects = null;
            var formatter = new BinaryFormatter(); // Створити форматер для серіалізації об'єктів

            // Відкрити файл для читання
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                // Десеріалізувати двійкові дані з файлу та перетворити їх у список об'єктів заданого типу
                objects = (List<T>)formatter.Deserialize(fileStream);
            }
            return objects; // Повернути список об'єктів
        }
    }
}
