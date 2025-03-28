using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Net.Mail;

namespace RegTest.Pages
{
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        public bool RegisterUser(string firstName, string lastName, string username,
                                 string email, string phone, string password)
        {
            if (!ValidateUserInput(firstName, lastName, username, email, phone, password))
                return false;

            try
            {
                return TryRegisterUser(firstName, lastName, username, email, phone, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool ValidateUserInput(string firstName, string lastName, string username,
                                       string email, string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный адрес электронной почты", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Номер должен начинаться с +7 и содержать 12 символов", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть минимум 6 символов", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool TryRegisterUser(string firstName, string lastName, string username,
                                     string email, string phone, string password)
        {
            using (var db = new Artem134Entities())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
                if (existingUser != null)
                {
                    MessageBox.Show("Такой пользователь или email уже существует", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                var user = new Users
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Email = email,
                    Phone = phone,
                    PasswordHash = HashPassword(password)
                };

                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashString = BitConverter.ToString(hashedBytes);
                return hashString.Replace("-", "").ToLower();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            if (phone.Length != 12)
                return false;

            if (!phone.StartsWith("+7"))
                return false;

            string digits = phone.Substring(1);
            foreach (char c in digits)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = RegisterUser(
                FirstNameTextBox.Text.Trim(),
                LastNameTextBox.Text.Trim(),
                UsernameTextBox.Text.Trim(),
                EmailTextBox.Text.Trim(),
                PhoneTextBox.Text.Trim(),
                PasswordBox.Password);

            if (success)
            {
                MessageBox.Show("Регистрация успешно завершена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (NavigationService != null)
                    NavigationService.Navigate(new Login());
            }
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
                NavigationService.Navigate(new Login());
        }
    }
}
