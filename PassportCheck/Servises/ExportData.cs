using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Windows;
using iTextSharp.text.pdf.qrcode;

namespace CheckPass
{
    public class ExportData
    {
        // Функція для експортування колекції в бінарний файл
        public void ExportToBinaryFile<T>(string path, List<T> collection)
        {
            // Створюємо об'єкт BinaryFormatter для серіалізації даних
            var formatter = new BinaryFormatter();
            // Відкриваємо файл для запису
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                // Серіалізуємо список об'єктів у бінарний формат і записуємо в файл
                formatter.Serialize(fileStream, collection);
            }
        }

        // Формує текстовий CSV-файл з даними паспортів
        public void ExportToCSVFile(string path, List<Passport> passports)
        {
            // Створюємо об'єкт StringBuilder для формування тексту CSV-файлу
            StringBuilder csvData = new StringBuilder();
            // Додаємо рядок з назвами колонок
            csvData.AppendLine("Identifier;Status;Series;Number;DateOfEditing");
            // Проходимо по списку паспортів і формуємо рядки з даними кожного паспорта
            foreach (Passport passport in passports)
            {
                // Форматуємо дату редагування паспорту
                string formattedDate = passport.DateOfEditing.ToString();
                // Додаємо рядок з даними паспорту у форматі CSV
                csvData.AppendFormat("{0};{1};{2};{3};{4}\n",
                passport.Identifier, passport.Status, passport.Series, passport.Number, formattedDate);
            }
            // Записуємо згенерований CSV-файл у файлову систему
            File.WriteAllText(path, csvData.ToString());
        }

        // Експорт користувачів до файлу CSV
        public void ExportToCSVFile(string path, List<User> users)
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("Id;Name;Password;Role;Username");

            // Для кожного користувача в списку користувачів формуємо рядок даних у форматі CSV
            foreach (User user in users)
            {
                csvData.AppendFormat("{0};{1};{2};{3};{4};{5}\n",
                    user.Id, user.Name, user.Surname, user.Password, user.Role, user.Username);
            }

            // Записуємо отриманий рядок даних до файлу за вказаним шляхом
            File.WriteAllText(path, csvData.ToString());
        }

        // Експортує список паспортів в PDF-файл за заданим шаблоном
        public void ExportToPDFFile(string path, List<Passport> passports)
        {
            try
            {
                // Створення потоку файлу для запису
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    // Створення документа PDF
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    // Налаштування документа
                    writer.SetFullCompression();
                    writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                    writer.SetEncryption(PdfWriter.STANDARD_ENCRYPTION_128, null, null, PdfWriter.ALLOW_PRINTING);

                    // Встановлення шрифту для кирилиці
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(bf, 12, Font.NORMAL);

                    // Створення таблиці
                    PdfPTable table = new PdfPTable(5);
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 20f, 20f, 20f, 20f, 20f };
                    table.SetWidths(widths);

                    // Додавання заголовків стовпців
                    table.AddCell(new PdfPCell(new Phrase("Ідентифікатор", font)));
                    table.AddCell(new PdfPCell(new Phrase("Статус", font)));
                    table.AddCell(new PdfPCell(new Phrase("Серія", font)));
                    table.AddCell(new PdfPCell(new Phrase("Номер", font)));
                    table.AddCell(new PdfPCell(new Phrase("Дата редагування", font)));

                    // Додавання даних
                    foreach (Passport passport in passports)
                    {
                        table.AddCell(new PdfPCell(new Phrase(passport.Identifier.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(passport.Status, font)));
                        table.AddCell(new PdfPCell(new Phrase(passport.Series, font)));
                        table.AddCell(new PdfPCell(new Phrase(passport.Number.ToString(), font)));
                        table.AddCell(new PdfPCell(new Phrase(passport.DateOfEditing.ToShortDateString(), font)));
                    }

                    // Додавання таблиці до документу
                    pdfDoc.Open();
                    pdfDoc.Add(table);
                    pdfDoc.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
