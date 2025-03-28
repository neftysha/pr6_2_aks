using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegTest.Pages
{
    public partial class Login : Page
    {
        private int failedAttempts = 0;
        private string currentCaptchaText = "";

        public Login()
        {
            InitializeComponent();
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        public bool Authenticate(string login, string password, string captcha = "")
        {
            if (CaptchaPanel.Visibility == Visibility.Visible && !string.IsNullOrEmpty(currentCaptchaText))
            {
                if (string.IsNullOrWhiteSpace(captcha) || captcha != currentCaptchaText)
                {
                    MessageBox.Show("Неверная CAPTCHA! Попробуйте снова.");
                    GenerateCaptcha();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return false;
            }

            try
            {
                using (var db = new Artem134Entities())
                {
                    var hashedPassword = GetHash(password);
                    var user = db.Users.AsNoTracking()
                        .FirstOrDefault(u => u.Username == login && u.PasswordHash == hashedPassword);

                    if (user == null)
                    {
                        failedAttempts++;
                        if (failedAttempts >= 3)
                        {
                            CaptchaPanel.Visibility = Visibility.Visible;
                            GenerateCaptcha();
                        }
                        MessageBox.Show("Неверный логин или пароль");
                        return false;
                    }

                    ResetForm();
                    MessageBox.Show("Вы успешно вошли");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}");
                return false;
            }
        }

        private void ResetForm()
        {
            failedAttempts = 0;
            CaptchaPanel.Visibility = Visibility.Collapsed;
            LoginTextBox.Clear();
            PasswordBox.Clear();
            CaptchaInput.Clear();
        }

        private void GenerateCaptcha()
        {
            CaptchaCanvas.Children.Clear();
            currentCaptchaText = GenerateRandomText(5);
            var rand = new Random();

            for (int i = 0; i < currentCaptchaText.Length; i++)
            {
                var textBlock = new TextBlock
                {
                    Text = currentCaptchaText[i].ToString(),
                    FontSize = rand.Next(20, 26),
                    FontWeight = FontWeights.Bold,
                    RenderTransform = new RotateTransform(rand.Next(-15, 15))
                };
                Canvas.SetLeft(textBlock, 10 + i * 30);
                Canvas.SetTop(textBlock, 10 + rand.Next(-5, 5));
                CaptchaCanvas.Children.Add(textBlock);
            }

            for (int i = 0; i < 5; i++)
            {
                CaptchaCanvas.Children.Add(new Line
                {
                    X1 = rand.Next(0, 100),
                    Y1 = rand.Next(0, 50),
                    X2 = rand.Next(100, 200),
                    Y2 = rand.Next(0, 50),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
            }

            for (int i = 0; i < 30; i++)
            {
                var ellipse = new Ellipse
                {
                    Width = rand.Next(1, 3),
                    Height = rand.Next(1, 3),
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(ellipse, rand.Next(0, 200));
                Canvas.SetTop(ellipse, rand.Next(0, 50));
                CaptchaCanvas.Children.Add(ellipse);
            }
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            Authenticate(LoginTextBox.Text, PasswordBox.Password, CaptchaInput.Text);
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register());
        }
    }
}
