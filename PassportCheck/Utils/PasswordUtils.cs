using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace CheckPass
{
    // Клас із допоміжними функціями для роботи з паролем
    public class PasswordUtils
    {
        public static string imgHidePath = "../../Resources/hide.png";
        public static string imgShowPath = "../../Resources/show.png";

        //Конвертація об'єкта SecureString в текст для подальшої обробки пароля
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                // Конвертація SecureString в неуправляючу пам'ять
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                // Повернення рядка, отриманого з неуправляючої пам'яті
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Очищення неуправляючої пам'яті після виконання операції
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        //Зміна зображення кнопки
        public static void ChangeImageOfHideShowPasswordButton(string imgPath, Button button)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imgPath, UriKind.Relative));
            button.OpacityMask = imageBrush;
        }
    }
}
