using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace WPF_L10._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string password_text;
        private string login_text;
        private string FilePath = "C:\\Users\\ponya\\source\\repos\\WPF_L10\\WPF_L10\\bin\\Debug\\users.txt";
        public MainWindow()
        {
            InitializeComponent();
            Set();
        }

        public void Set()
        {
            Authorize.Visibility = Visibility.Visible;
            Panel.SetZIndex(Authorize, 2);
            Hello.Visibility = Visibility.Hidden;
            Panel.SetZIndex(Hello, 1);
        }

        public string RePass()
        {
            password_text = password.Password;
            byte[] bytes = Encoding.UTF8.GetBytes(password_text);
            return Convert.ToBase64String(bytes);
        }

        private Dictionary<string, string> ReadDb()
        {
            var dict = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(FilePath))
            {
                var parts = line.Split(';');
                if (parts.Length == 2)
                {
                    dict[parts[0]] = parts[1];
                }
            }
            return dict;
        }

        private async Task<bool> AuthenticateAsync(string username, string password)
        {
            try
            {
                Bttn.IsEnabled = false;
                UpdateProgress(1, "Поиск логина в базе...");

                // Полноценная проверка существования логина
                bool loginExists = false;
                if (File.Exists(FilePath))
                {
                    foreach (string line in File.ReadLines(FilePath))
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 1 && parts[0] == username)
                        {
                            loginExists = true;
                            break;
                        }
                    }
                }

                if (!loginExists)
                {
                    UpdateProgress(100, "Ошибка загрузки!");
                    AuthProgress.Foreground = Brushes.Red;
                    MessageBox.Show("Логин не найден!");
                    Bttn.IsEnabled = true;
                    await Task.Delay(500);
                    return false;
                }

                UpdateProgress(10, "Шифрование пароля...");
                string encryptedPass = RePass();
                await Task.Delay(1000);

                UpdateProgress(30, "Проверка соответствия...");
                var users = File.ReadAllLines(FilePath);
                await Task.Delay(1000);

                bool found = false;
                foreach (var line in users)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2 && parts[1] == encryptedPass)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    UpdateProgress(100, "Ошибка загрузки!");
                    AuthProgress.Foreground = Brushes.Red;
                    MessageBox.Show("Пароль неверный!");
                    Bttn.IsEnabled = true;
                    await Task.Delay(500);
                    return false;
                }

                UpdateProgress(68, "Загрузка данных...");
                await Task.Delay(3000);

                UpdateProgress(100, "Успешный вход!");
                Bttn.IsEnabled = true;
                
                await Task.Delay(500);
                Hello.Visibility = Visibility.Visible;
                Panel.SetZIndex(Authorize, 0);
                Panel.SetZIndex(Hello, 2);
                Authorize.Visibility = Visibility.Hidden;
                profile.Text = $"профиль: {username}";

                return true;
            }
            catch (Exception ex)
            {
                UpdateProgress(100, $"Ошибка: {ex.Message}");
                AuthProgress.Foreground = Brushes.Red;
                Bttn.IsEnabled = true;
                await Task.Delay(1000);
                return false;
            }
        }

        private void UpdateProgress(int value, string status)
        {
            AuthProgress.Value = value;
            Status.Text = status;
            AuthProgress.Foreground = Brushes.Green;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthProgress.Visibility = Visibility.Visible;
            AuthenticateAsync(login.Text, password.Password);
            
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            AuthProgress.Visibility= Visibility.Hidden;
            Set();
        }

        private void Bttn_reg_Click(object sender, RoutedEventArgs e)
        {
            string RegPath = @"C:\Users\ponya\source\repos\WPF_L10\WPF_L10\bin\Debug\WPF_L10.exe";

            try
            {
                Process.Start(RegPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска: {ex.Message}");
            }
        }
    }
}
