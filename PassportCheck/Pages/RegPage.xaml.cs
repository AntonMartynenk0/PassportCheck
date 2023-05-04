using CheckPass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PassportCheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        // Вікно, на якому розташована сторінка реєстрації
        private Window currentWindow;

        // Конструктор класу RegPage
        public RegPage()
        {
            InitializeComponent(); // Ініціалізує компоненти 
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgHidePath, showPassword);
        }

        // Обробник події для кнопки "Зареєструватися"
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration(); // Створюємо об'єкт класу Registration
            registration.RegisterNewAccount(
                textBoxName.Text,
                textBoxSurname.Text,
                textBoxUsername.Text,
                textBoxPassword.SecurePassword,
                textBoxConfirmPassword.SecurePassword); // Викликаємо метод для реєстрації нового користувача
        }

        // Обробник події для кнопки "Увійти"
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage()); // Переходимо на сторінку логінування
        }

        // Обробник події при завантаженні сторінки
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            currentWindow = Window.GetWindow(this); // Отримуємо вікно сторінки реєстрації
            currentWindow.Title = "Registration"; // Встановлюємо заголовок вікна
        }

        // Обробник події при натисканні кнопки відображення паролю
        private void showPassword_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgShowPath, sender as Button);
            textBoxPasswordVisible.Text = PasswordUtils.ConvertToUnsecureString(textBoxPassword.SecurePassword);
            textBoxPasswordVisible1.Text = PasswordUtils.ConvertToUnsecureString(textBoxConfirmPassword.SecurePassword);
            textBoxPassword.Visibility = Visibility.Collapsed;
            textBoxConfirmPassword.Visibility = Visibility.Collapsed;
            textBoxPasswordVisible.Visibility = Visibility.Visible;
            textBoxPasswordVisible1.Visibility = Visibility.Visible;
        }

        // Обробник події при відтисканні кнопки відображення паролю
        private void showPassword_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgHidePath, sender as Button);
            textBoxConfirmPassword.Visibility = Visibility.Visible;
            textBoxPassword.Visibility = Visibility.Visible;
            textBoxPasswordVisible.Visibility = Visibility.Collapsed;
            textBoxPasswordVisible1.Visibility = Visibility.Collapsed;
        }
    }
}
