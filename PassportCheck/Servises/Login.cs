using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckPass
{
    public class Login
    {
        public Login() { }
        // Функція, яка виконує авторизацію користувача за заданими іменем користувача та паролем
        public User PerfomLogin(string username, SecureString password)
        {
            ImportData importData = new ImportData(); // Створити об'єкт для імпорту даних
            List<User> users = importData.ImportUsersFromCSV(@"..\..\Data\users.csv"); // Імпортувати користувачів з CSV-файлу
            User user = users.Where(u => u.Username == username && u.Password == PasswordUtils.ConvertToUnsecureString(password)).FirstOrDefault(); // Знайти користувача за іменем користувача та паролем
            if (user == null)
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error); // Повідомити про невірне ім'я користувача або пароль
                return null;
            }
            return user; // Повернути знайденого користувача
        }
    }
}
