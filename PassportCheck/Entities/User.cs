using CheckPass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPass
{
    [Serializable]
    public class User
    {
        //Ідентифікатор користувача
        public string Id { get; set; }
        //Ім'я користувача
        public string Name { get; set; }
        //Ім'я користувача
        public string Surname { get; set; }
        //Пароль користувача
        public string Password { get; set; }
        //Роль користувача
        public Role Role { get; set; }
        //Логін користувача
        public string Username { get; set; }

        public User() { }

        //Конструктор, для використання при ініціализації з рандомним ідентифікатором
        public User(string name, string surname, string password, Role role, string username)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Surname = surname;
            Password = password;
            Role = role;
            Username = username;
        }

        //Конструктор, для використання при ініціализації із певним ідентифікатором
        public User(string id, string name, string surname, string password, Role role, string username)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Password = password;
            Role = role;
            Username = username;
        }

        //Перевизначений метод для порівняння об'єктів класу
        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Name == user.Name &&
                   Surname == user.Surname &&
                   Password == user.Password &&
                   Role == user.Role &&
                   Username == user.Username;
        }

        //Перевизначений метод для порівняння об'єктів класу
        public override int GetHashCode()
        {
            int hashCode = -1974409362;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = hashCode * -1521134295 + Role.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            return hashCode;
        }
    }
}
