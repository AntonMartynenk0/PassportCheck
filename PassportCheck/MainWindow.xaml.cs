using CheckPass;
using PassportCheck.Pages;
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

namespace PassportCheck
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            //users.Add(new User("Admin", "admin", Entities.Role.ADMIN, "admin"));
            //ExportData exportData = new ExportData();
            //exportData.ExportToBinaryFile(@"..\..\Data\users.bin", users);
            MainContent.NavigationService.Navigate(new MainPage(user));
        }
    }
}
