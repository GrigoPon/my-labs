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
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace WPF_L10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string login_text;
        private string password_text;
        private bool flag;
        private string pass_encoded;
        public MainWindow()
        {
            InitializeComponent();
            LoginCheck();
            PasswordCheck();
        }

        public bool LoginCheck()
        {
            login_text = login.Text;
            if (login_text == null || login_text.Length < 6)
            {
                return false;
            }

            return Regex.IsMatch(login_text, @"^[a-zA-Z0-9_\s]+$");
        }

        public bool PasswordCheck()
        {
            password_text = password.Password;
            if (password_text == null || password_text.Length < 6)
            {
                return false;
            }

            return !Regex.IsMatch(password_text, @"^[а-яА-ЯёЁ]+$");
        }

        public bool DataCheck()
        {
            if (!LoginCheck())
            {
                Checker.Visibility = Visibility.Visible;
                login.BorderBrush = Brushes.Red;
                flag = false;
            }
            else
            {
                Checker.Visibility = Visibility.Hidden;
                login.BorderBrush = Brushes.White;
                flag = true;
            }


            if (!PasswordCheck())
            {
                Checker_pass.Visibility = Visibility.Visible;
                password.BorderBrush = Brushes.Red;
                flag = false;
            }
            else
            {
                Checker_pass.Visibility = Visibility.Hidden;
                password.BorderBrush = Brushes.White;
                flag = true;
            }

            return flag;
        }

        public string Base64Encode()
        {
            password_text = password.Password;
            byte[] bytes = Encoding.UTF8.GetBytes(password_text);
            return Convert.ToBase64String(bytes);
        }

        public void SaveToFile(string login, string encryptedPassword, string filePath)
        {

            string line = $"{login};{encryptedPassword}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login_text = login.Text;
            

            if(DataCheck())
            {
                pass_encoded = Base64Encode();
                //функция занесения в файл
                string dbFilePath = "users.txt";
                SaveToFile(login_text, pass_encoded, dbFilePath);
                MessageBox.Show("Данные сохранены успешно!");
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string LogPath = @"C:\Users\ponya\source\repos\WPF_L10\WPF_L10.1\bin\Debug\WPF_L10.1.exe";

            try
            {
                Process.Start(LogPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска: {ex.Message}");
            }
        }
    }
}
