using CheckPass;
using CheckPass.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace PassportCheck.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Passport> passports = new List<Passport>(); // Колекція  паспортів
        private Passport selectedPassport = null; // Вибраний паспорт
        private User user; // Залогінений користувач
        private bool isAddNewElementCollapsed = true; // Стан відображення панелі з додаванням нового запису
        private bool isEditEnable = false; // Чи можна редагувати записи
        private int selectedIndex = -1; // Індекс поточного вибраного запису (якщо не вибрано = -1)
        private Dictionary<string, ListSortDirection> sortDirections = new Dictionary<string, ListSortDirection>(); // Словний для зберігання напряму сортування паспортів

        public MainPage(User user)
        {
            InitializeComponent();
            this.user = user;
            UpdateUser();
            dataGrid.ItemsSource = passports;
        }

        //Оновлення допуску користувача до функцій редагування, та додавання записів
        private void UpdateUser()
        {
            labelWelcome.Content = new StringBuilder().AppendFormat("Welcome, {0} {1}", user.Name, user.Surname).ToString();
            if (user.Role == Role.ADMIN) //Якщо роль Адмін, то можна редагувати, кнопка додавання нового користувача видима
            {
                buttonAdd.Visibility = Visibility.Visible;
                isEditEnable = true;
            }
            else //Якщо роль не адмін, то не можна редагувати, кнопка додавання нового користувача невидима
            {
                editElementPanel.Visibility = Visibility.Collapsed;
                buttonAdd.Visibility = Visibility.Collapsed;
                isEditEnable = false;
            }
        }

        //Обробка події виходу користувача з облікового запису
        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginRegisterWindow loginWindow = new LoginRegisterWindow();
            loginWindow.Show(); // Відкриває вікно Логіну
            Window currentWindow = Window.GetWindow(this); // Отримуємо поточне вікно
            currentWindow.Close(); // Закриваємо поточне вікно
        }

        //Обробка події імпорту
        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Comma-separated values (*.csv)|*.csv|Binary file (*.bin)|*.bin"; // Встановлення можливих типів
            bool? result = openFileDialog.ShowDialog(); //Відображення вікна відкриття файлу
            if (result == true)
            {
                ImportData importData = new ImportData();
                string selectedFileName = openFileDialog.FileName; //Шлях обраного файлу
                string selectedFileExtension = Path.GetExtension(selectedFileName); //Тип вибраного файлу

                switch (selectedFileExtension.ToLower())
                {
                    case ".bin":
                        {
                            passports = importData.ImportFromBinary(selectedFileName, new Passport());
                            break;
                        }
                    case ".csv":
                        {
                            passports = importData.ImportFromCSV(selectedFileName);
                            break;
                        }
                }
                dataGrid.ItemsSource = passports; //Прив'язка отриманих паспортів до dataGrid
            }
        }

        //Обробка події експорту
        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0) //Якщо в таблиці нема елементів, виходимо з фінкції
            {
                return;
            }
            ExportData exportData = new ExportData();
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Comma-separated values (*.csv)|*.csv|Binary file (*.bin)|*.bin|PDF file (*.pdf)|*.pdf"; // Встановлення можливих типів
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string selectedFileName = saveFileDialog.FileName; //Шлях до файла куди зберігати
                string selectedFileExtension = Path.GetExtension(selectedFileName); //Тип файлу
                switch (selectedFileExtension.ToLower())
                {
                    case ".bin":
                        {
                            exportData.ExportToBinaryFile(selectedFileName, passports);
                            break;
                        }
                    case ".csv":
                        {
                            exportData.ExportToCSVFile(selectedFileName, passports);
                            break;
                        }
                    case ".pdf":
                        {
                            exportData.ExportToPDFFile(selectedFileName, passports);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        //Обробка події видалення вибраного елементу
        private void buttonDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            //Якщо є вибраний елемент
            if (selectedIndex >= 0)
            {
                passports.RemoveAt(selectedIndex); //Видалення за індексом
                dataGrid.Items.Refresh(); //Оновлення елементів таблиці
            }
        }

        //Обробка події видалення всіх елементів
        private void buttonDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count != 0)
            {
                passports.Clear();
                dataGrid.Items.Refresh();
            }
        }

        //Обробка події Додати елемент
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (isAddNewElementCollapsed) //Якщо панель додавання скрита
            {
                addNewElementPanel.Visibility = Visibility.Visible; // Показуємо панель додавання
                isAddNewElementCollapsed = false;
            }
            else //Якщо панель додавання видима
            {
                addNewElementPanel.Visibility = Visibility.Collapsed; //Скриваємо панель додавання
                isAddNewElementCollapsed = true;
            }
        }

        //Обробка події Підтверждення додавання елементу
        private void buttonAddConfirm_Click(object sender, RoutedEventArgs e)
        {
            //Створення та додавання паспорту з новими данними
            Passport newPassport = InputValuesValidation(textBoxAddId.Text, textBoxAddNumber.Text, textBoxAddSeries.Text);
            if (newPassport != null)
            {
                passports.Add(newPassport);
            }
            dataGrid.Items.Refresh();
        }

        //Обробка події Підтверждення редагування елементу
        private void buttonEditConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Створення паспорту з новими данними
            Passport newPassport = InputValuesValidation(textBoxEditId.Text, textBoxEditNumber.Text, textBoxEditSeries.Text);
            if (newPassport != null)
            {
                // Перевизначення стараго паспорту новими данними
                selectedPassport.Identifier = newPassport.Identifier;
                selectedPassport.Series = newPassport.Series;
                selectedPassport.Number = newPassport.Number;
                selectedPassport.DateOfEditing = newPassport.DateOfEditing;
                dataGrid.Items.Refresh();
            }
        }

        // Перевірка валідності вхідних параметрів для створення паспорту
        private Passport InputValuesValidation(string identifier, string num, string series)
        {
            series = series.Trim().ToUpper(); //Видалення пробільних знаків та переведеня у верхній регістр
            try
            {
                // Перевірка чи не є порожніми номер та ідентифікатор паспорту
                if (num == string.Empty)
                {
                    MessageBox.Show("Номер обраного паспорту не може бути порожнім!", "Помилка при створенні нового запису", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                if (identifier == string.Empty)
                {
                    MessageBox.Show("Введіть ідентифікатор!", "Помилка при створенні нового запису", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невірний ідентифікатор або номер\n" + ex.Message, "Помилка при створенні нового запису", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // Перевірка серії паспорту за допомогою регулярного виразу (Повинен містити 2 літери, або бути порожнім)
            if (Regex.IsMatch(series, "^([A-ZА-Я]{2})?$") || series == "")
            {
                return new Passport(identifier, "недійсний", series, num, DateTime.Now);
            }
            else
            {
                MessageBox.Show("Серія паспорту має бути порожньою або містити 2 символи!", "Помилка при створенні нового запису", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Скидання значень полів пошуку при натисканні кнопки "Скинути пошук"
        private void buttonResetSearch_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Text = string.Empty; // Скидання текстового поля пошуку
            datePickerSearch.Text = string.Empty; // Скидання поля дати пошуку
        }

        // Виклик функції пошуку паспортів при зміні тексту в полі пошуку
        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPassports(); // Виклик функції пошуку паспортів
        }

        // Виклик функції пошуку при зміні вибраної дати в полі пошуку
        private void datePickerSearch_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchPassports(); // Виклик функції пошуку паспортів
        }

        // Функція пошуку паспортів
        private void SearchPassports()
        {
            List<Passport> filtredPassports = passports; // Список фільтрованих паспортів, початково дорівнює списку всіх паспортів
            string searchText = textBoxSearch.Text.ToUpper().Trim(); // Текст, введений користувачем в поле пошуку, переведений до верхнього регістру та очищений від зайвих пробілів
            DateTime? searchDate = datePickerSearch.SelectedDate; // Вибрана користувачем дата пошуку
            if (searchText != string.Empty)
            {
                // Фільтрація списку паспортів за введеним користувачем текстом (за ідентифікатором, серією або номером) та оновлення джерела даних для таблиці
                filtredPassports = filtredPassports.Where(x => x.Identifier.ToString().StartsWith(searchText) || x.Series.ToString().StartsWith(searchText) || x.Number.ToString().StartsWith(searchText)).ToList();
                dataGrid.ItemsSource = filtredPassports;
            }
            if (searchDate != null)
            {
                // Фільтрація списку паспортів за вибраною користувачем датою та оновлення джерела даних для таблиці
                filtredPassports = filtredPassports.Where(x => x.DateOfEditing.ToString().StartsWith(datePickerSearch.Text)).ToList();
                dataGrid.ItemsSource = filtredPassports;
            }
            if (searchText == string.Empty && searchDate == null)
            {
                // Якщо текст та дата пошуку порожні, то вивести всі паспорти в таблицю
                dataGrid.ItemsSource = passports;
            }
        }

        // Обробник подвійного натискання на рядок таблиці
        private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isEditEnable) // якщо функціонал редагування включений
            {
                var row = sender as DataGridRow;
                Passport passport = row.DataContext as Passport; // отримання даних паспорту з виділеного рядка таблиці
                if (passport != selectedPassport) // якщо виділений паспорт не є вже вибраним для редагування
                {
                    selectedPassport = row.DataContext as Passport; // збереження виділеного паспорту як обраного для редагування
                    selectedPassport = dataGrid.SelectedItem as Passport; // збереження виділеного паспорту як обраного для редагування
                    editElementPanel.Visibility = Visibility.Visible; // відображення панелі редагування елемента
                    dataGrid.SelectedItem = selectedPassport; // встановлення вибраного паспорту як виділеного в таблиці
                    textBoxEditId.Text = selectedPassport.Identifier; // встановлення ідентифікатору обраного паспорту у текстове поле редагування
                    textBoxEditNumber.Text = selectedPassport.Number; // встановлення номеру обраного паспорту у текстове поле редагування
                    textBoxEditSeries.Text = selectedPassport.Series; // встановлення серії обраного паспорту у текстове поле редагування
                }
            }
            dataGrid.SelectedItem = null; // скидання виділення з рядка таблиці
        }

        // Обробник подвійного натискання на рядок таблиці
        private void dataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true; // Відміна стандартної сортування
            string propertyName = e.Column.SortMemberPath; // Отримання назви властивості, по якій буде відбуватись сортування
            if (!sortDirections.ContainsKey(propertyName))
            {
                sortDirections.Add(propertyName, ListSortDirection.Ascending); // Якщо даної властивості ще не було сортовано, то додаємо її в словник з напрямками сортування
            }

            if (propertyName == "Identifier")
            {
                passports = passports.OrderBy(p => long.Parse(Regex.Match(p.Identifier, @"\d+").Value))
                    .ThenBy(p => Regex.Replace(p.Identifier, @"[\d-]", string.Empty)).ToList(); // Сортування паспортів за номером ідентифікатора
            }
            else if (propertyName == "Series")
            {
                passports = passports.OrderBy(p => p.Series).ToList(); // Сортування паспортів за серією
            }
            else if (propertyName == "Number")
            {
                passports = passports.OrderBy(p => long.Parse(Regex.Match(p.Number, @"\d+").Value))
                    .ThenBy(p => Regex.Replace(p.Number, @"[\d-]", string.Empty)).ToList(); // Сортування паспортів за номером
            }
            else if (propertyName == "DateOfEditing")
            {
                passports = passports.OrderBy(p => p.DateOfEditing).ToList(); // Сортування паспортів за датою редагування
            }

            if (sortDirections[propertyName] == ListSortDirection.Descending)
            {
                passports.Reverse(); // Перевернення списку, якщо напрям сортування - у зворотному порядку
            }

            sortDirections[propertyName] = (sortDirections[propertyName] == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending; // Зміна напряму сортування

            dataGrid.ItemsSource = passports; // Оновлення вмісту таблиці
            e.Column.SortDirection = sortDirections[propertyName]; // Встановлення напрямку сортування для даного стовпця
            dataGrid.Items.Refresh(); // Оновлення елементів таблиці
        }

        // Обробник зміни вибору рядка в таблиці
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Якщо було вибрано хоча б один рядок
            if (dataGrid.SelectedItems.Count > 0)
            {
                // Зберегти індекс вибраного рядка
                selectedIndex = dataGrid.SelectedIndex;
            }
            else
            {
                // Якщо нічого не вибрано, встановити індекс в -1
                selectedIndex = -1;
            }
        }
    }
}
