using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckPass
{
    [Serializable]
    public class Passport
    {
        //Ідентифікатор
        public string Identifier { get; set; }
        //Статус
        public string Status { get; set; }
        //Серія
        public string Series { get; set; }
        //Номер паспорту
        public string Number { get; set; }
        //Дата останнього редагування або створення запису
        public DateTime DateOfEditing { get; set; }

        public Passport() { }

        public Passport(string identifier, string status, string series, string number, DateTime dateOfEditing)
        {
            Identifier = identifier;
            Status = status;
            Series = series;
            Number = number;
            DateOfEditing = dateOfEditing;
        }

        //Перевизначений метод для порівняння об'єктів класу
        public override bool Equals(object obj)
        {
            return obj is Passport passport &&
                   Identifier == passport.Identifier &&
                   Status == passport.Status &&
                   Series == passport.Series &&
                   Number == passport.Number &&
                   DateOfEditing.Day == passport.DateOfEditing.Day &&
                   DateOfEditing.Month == passport.DateOfEditing.Month &&
                   DateOfEditing.Year == passport.DateOfEditing.Year &&
                   DateOfEditing.Hour == passport.DateOfEditing.Hour &&
                   DateOfEditing.Minute == passport.DateOfEditing.Minute &&
                   DateOfEditing.Second == passport.DateOfEditing.Second;
        }

        //Перевизначений метод для порівняння об'єктів класу
        public override int GetHashCode()
        {
            int hashCode = 538950389;
            hashCode = hashCode * -1521134295 + Identifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Series);
            hashCode = hashCode * -1521134295 + Number.GetHashCode();
            hashCode = hashCode * -1521134295 + DateOfEditing.GetHashCode();
            return hashCode;
        }
    }
}
