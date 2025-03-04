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
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Threading;


namespace WPF_L5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    [Serializable] // Для возможности сериализации
    public class AppSettings
    {
        public bool italic { get; set; }
        public bool bold { get; set; }
        public bool underline { get; set; }
        public bool lower { get; set; }
        public bool upper { get; set; }
        public bool now_check { get; set; }
        public bool button_check { get; set; }
    }
    public partial class MainWindow : Window
    {
        private Random _random;
        private const string SettingsFilePath = "appsettings.json";
        private DispatcherTimer _autoSave;
        public MainWindow()
        {
            InitializeComponent();
            _random = new Random();
            LoadSettings();


            _autoSave = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60) // Интервал 30 секунд
            };
            _autoSave.Tick += AutoSave_Tick;
            _autoSave.Start();

            it_check.Checked += CheckBox_Changed;
            it_check.Unchecked += CheckBox_Changed;

            b_check.Checked += CheckBox_Changed;
            b_check.Unchecked += CheckBox_Changed;

            u_check.Checked += CheckBox_Changed;
            u_check.Unchecked += CheckBox_Changed;
            this.Closing += MainWindow_Closing;
        }

        private void AutoSave_Tick(object sender, EventArgs e)
        {
            SaveSettings();
        }
        private void LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                try
                {
                    // Читаем файл настроек
                    string json = File.ReadAllText(SettingsFilePath);
                    AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json);

                    // Применяем настройки
                    it_check.IsChecked = settings.italic;
                    b_check.IsChecked = settings.bold;
                    u_check.IsChecked = settings.underline;

                    lower.IsChecked = settings.lower;
                    upper.IsChecked = settings.upper;

                    now_check.IsChecked = settings.now_check;
                    button_check.IsChecked = settings.button_check;

                    // Обновляем текст
                    updating();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}");
                }
            }
            else
            {
                // Если файла нет, устанавливаем случайные настройки
                randomset();
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Создаем объект настроек
                var settings = new AppSettings
                {
                    italic = it_check.IsChecked == true,
                    bold = b_check.IsChecked == true,
                    underline = u_check.IsChecked == true,
                    lower = lower.IsChecked == true,
                    upper = upper.IsChecked == true,
                    now_check = now_check.IsChecked == true,
                    button_check = button_check.IsChecked == true
                };

                // Сериализуем настройки в JSON
                string json = JsonSerializer.Serialize(settings);

                // Сохраняем в файл
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения настроек: {ex.Message}");
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Сохраняем настройки при закрытии программы
            SaveSettings();
        }
        private void randomset()
        {
            
            it_check.IsChecked = _random.Next(2) == 1; 
            b_check.IsChecked = _random.Next(2) == 1;
            u_check.IsChecked = _random.Next(2) == 1;

            
            if (_random.Next(2) == 1)
            {
                lower.IsChecked = true;
            }
            else
            {
                upper.IsChecked = true;
            }

            
            if (_random.Next(2) == 1)
            {
                now_check.IsChecked = true;
            }
            else
            {
                button_check.IsChecked = true;
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (now_check.IsChecked == true)
            {
                updating();
            }
        }

        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            
            updating();
            
        }

        private void updating()
        {
            checker.FontStyle = it_check.IsChecked == true ? FontStyles.Italic : FontStyles.Normal;
            checker.FontWeight = b_check.IsChecked == true ? FontWeights.Bold : FontWeights.Normal;
            checker.TextDecorations = u_check.IsChecked == true ? TextDecorations.Underline : null;
            updating_reg();
        }

        private void button_check_Checked(object sender, RoutedEventArgs e)
        {
            upd.IsEnabled = true;
        }

        private void button_check_Unchecked(object sender, RoutedEventArgs e)
        {
            upd.IsEnabled = false;
        }

        private void reg_checked(object sender, RoutedEventArgs e)
        {
            if (now_check.IsChecked == true)
            {
                updating();
            }
        }
        private void updating_reg()
        {
            if (lower.IsChecked == true)
            {
                checker.Text = checker.Text.ToLower();
            }
            else if (upper.IsChecked == true)
            {
                checker.Text = checker.Text.ToUpper();
            }
        }
    }
}
