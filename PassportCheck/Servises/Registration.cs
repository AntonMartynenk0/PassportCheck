using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckPass
{
    public class Registration
    {
        private List<User> users;
        private User user;
        public Registration()
        { }

        // Метод реєстрації нового облікового запису
        public void RegisterNewAccount(string name, string surname, string username, SecureString securedPassword, SecureString securedConfirmPassword)
        {
            // Перевірка на те, чи всі поля заповнені
            if (name == "" || surname == "" || username == "" || securedPassword.Length == 0 || securedConfirmPassword.Length == 0)
            {
                MessageBox.Show("All fields must be filled.", "Empty fields", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ImportData importData = new ImportData();
            users = importData.ImportUsersFromCSV(@"..\..\Data\users.csv");

            // Перевірка на унікальність імені користувача
            if (!IsUsernameUnique(username))
            {
                MessageBox.Show("Account with \"" + username + "\" username already exists. Try different one.", "Incorrect Username", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка на співпадіння пароля та його підтвердження
            string unsecuredPassword = PasswordUtils.ConvertToUnsecureString(securedPassword);
            string unsecuredConfirmPassword = PasswordUtils.ConvertToUnsecureString(securedConfirmPassword);
            if (!IsPasswordMatches(unsecuredPassword, unsecuredConfirmPassword))
            {
                MessageBox.Show("Password and Confirm Password must match", "Passwords does'nt match", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Створення нового користувача з введеними даними та додавання його до списку користувачів
            user = new User(name, surname, unsecuredPassword, Entities.Role.USER, username);
            users.Add(user);

            // Експорт списку користувачів в CSV файл
            ExportData exportData = new ExportData();
            exportData.ExportToCSVFile(@"..\..\Data\users.csv", users);

            MessageBox.Show("Account with \"" + username + "\" username has been created!", "Account Successfully Created", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Функція перевірки того, чи співпадають паролі
        private bool IsPasswordMatches(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }

        // Функція перевірки того, чи є ім'я користувача унікальним
        private bool IsUsernameUnique(string username)
        {
            return users.Where(u => u.Username == username).FirstOrDefault() == null ? true : false;
        }
    }
}
