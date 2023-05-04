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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        // Змінна, що містить об'єкт користувача
        private User user;
        // Вікно, на якому розташована сторінка логінування
        private Window currentWindow;

        // Конструктор класу LoginPage
        public LoginPage()
        {
            InitializeComponent(); // Ініціалізує компоненти сторінки
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgHidePath, showPassword); // Зміна зображення кнопки для відображення паролю
        }

        // Обробник події для кнопки "Увійти"
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(); // Створюємо об'єкт класу Login
            user = login.PerfomLogin(textBoxUsername.Text, textBoxPassword.SecurePassword); // Викликаємо метод для авторизації користувача
            if (user != null) // Якщо користувач був успішно авторизований
            {
                MainWindow mainWindow = new MainWindow(user); // Створюємо об'єкт головного вікна з передачею об'єкта користувача
                mainWindow.Show(); // Показуємо головне вікно
                currentWindow = Window.GetWindow(this); // Отримуємо вікно сторінки логінування
                currentWindow.Close(); // Закриваємо вікно сторінки логінування
            }
        }

        // Обробник події для кнопки "Зареєструватися"
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegPage()); // Переходимо на сторінку реєстрації
        }

        // Обробник події при завантаженні сторінки
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            currentWindow = Window.GetWindow(this); // Отримуємо вікно сторінки логінування
            currentWindow.Title = "Login"; // Встановлюємо заголовок вікна
        }

        // Обробник події при натисканні кнопки відображення паролю
        private void showPassword_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgShowPath, sender as Button); // Зміна зображення кнопки для відображення паролю
            textBoxPasswordVisible.Text = PasswordUtils.ConvertToUnsecureString(textBoxPassword.SecurePassword); // Відображення, конвертованого з захищеного, паролю в textBox
            textBoxPassword.Visibility = Visibility.Collapsed; // Ховаємо захищений textBoxPassword з паролем
            textBoxPasswordVisible.Visibility = Visibility.Visible; // Показуємо  незахищенийй textBox з паролем
        }

        // Обробник події при відтисканні кнопки відображення паролю
        private void showPassword_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PasswordUtils.ChangeImageOfHideShowPasswordButton(PasswordUtils.imgHidePath, sender as Button); // Зміна зображення кнопки для відображення паролю
            textBoxPassword.Visibility = Visibility.Visible; // Показуємо PasswordBox
            textBoxPasswordVisible.Visibility = Visibility.Collapsed; // Ховаємо textBox з паролем
        }
    }
}
