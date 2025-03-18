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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using WPF_L6.Properties;
using System.IO;
using System.Windows.Threading;
using System.Text.Unicode;
using System.Text.Encodings.Web;




namespace WPF_L6
{
    public class MainViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _surname;
        private string _firstname;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                OnPropertyChanged(nameof(Firstname));
            }
        }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Surname):
                        if (string.IsNullOrEmpty(Surname) || !Surname.All(char.IsLetter) || !char.IsUpper(Surname[0]) || Surname[0] == 'Ь' || Surname[0] == 'Ъ' || Surname[0] == 'Ы')
                        {
                            return "Неверно введены данные!";
                        }
                        break;

                    case nameof(Firstname):
                        if (string.IsNullOrEmpty(Firstname) || !Firstname.All(char.IsLetter) || !char.IsUpper(Firstname[0]) || Firstname[0] == 'Ь' || Firstname[0] == 'Ъ' || Firstname[0] == 'Ы')
                        {
                            return "Неверно введены данные!";
                        }
                        break;
                }
                return null;
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _year_flag = false;
        private bool _day_flag = false;
        public MainWindow()
        {
            InitializeComponent();
            MonthsComboBox.ItemsSource = new Months[]
            {
                new Months{month = "Январь"},
                new Months{month = "Февраль"},
                new Months{month = "Март"},
                new Months{month = "Апрель"},
                new Months{month = "Май"},
                new Months{month = "Июнь"},
                new Months{month = "Июль"},
                new Months{month = "Август"},
                new Months{month = "Сентябрь"},
                new Months{month = "Октябрь"},
                new Months{month = "Ноябрь"},
                new Months{month = "Декабрь"}
            };
            Check_Item();
            Day_Check();
            Year_Check();
            DataContext = new MainViewModel();
        }

        
        public void Check_Item()
        {
            if (Country.SelectedItem != null)
            {
                switch (Country.SelectedItem.ToString())
                {
                    case "Россия":
                        {
                            if (PhoneType.SelectedItem.ToString() == "Мобильный")
                            {
                                PhoneCode.Text = "+7";
                                PhoneCode.Margin = new Thickness(356, PhoneCode.Margin.Top, 0, 0);
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(000)-000-00-00";
                            }
                            else if (PhoneType.SelectedItem.ToString() == "Домашний")
                            {
                                PhoneCode.Text = "+7 (4012)";
                                PhoneCode.Margin = new Thickness(PhoneCode.Margin.Left - 50, PhoneCode.Margin.Top, 0, 0);
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "00-00-00";
                            }
                            break;
                        }
                    case "Григория":
                        {
                            if (PhoneType.SelectedItem.ToString() == "Мобильный")
                            {
                                PhoneCode.Text = "+26";
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(000)-000-0";
                                PhoneCode.Margin = new Thickness(350, PhoneCode.Margin.Top, 0, 0);
                            }
                            else if (PhoneType.SelectedItem.ToString() == "Домашний")
                            {
                                PhoneCode.Text = "+1 (2005)";
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(0000)-0000-0000";
                                PhoneCode.Margin = new Thickness(PhoneCode.Margin.Left - 40, PhoneCode.Margin.Top, 0, 0);
                            }
                            break;
                        }
                    case "Финляндия":
                        {
                            if (PhoneType.SelectedItem.ToString() == "Мобильный")
                            {
                                PhoneCode.Text = "+358";
                                PhoneCode.Margin = new Thickness(345, PhoneCode.Margin.Top, 0, 0);
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(000)-000-00-00";
                            }
                            else if (PhoneType.SelectedItem.ToString() == "Домашний")
                            {
                                PhoneCode.Text = "+358";
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(0)-000-0000";
                            }
                            break;
                        }
                    case "Франция":
                        {
                            if (PhoneType.SelectedItem.ToString() == "Мобильный")
                            {
                                PhoneCode.Text = "+33";
                                PhoneCode.Margin = new Thickness(350, PhoneCode.Margin.Top, 0, 0);
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(0)-00-00-00-00";
                            }
                            else if (PhoneType.SelectedItem.ToString() == "Домашний")
                            {
                                PhoneCode.Text = "+33";
                                PhoneNumber.Text = "";
                                PhoneNumber.Mask = "(0)-00-00-00-00";
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private void PhoneType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Check_Item();
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PhoneType.SelectedIndex = 0;
            Check_Item();
        }

        public void Day_Check()
        {
            if (MonthsComboBox.SelectedIndex == 0 || MonthsComboBox.SelectedIndex == 2 || MonthsComboBox.SelectedIndex == 4 ||
                MonthsComboBox.SelectedIndex == 6 || MonthsComboBox.SelectedIndex == 7 || MonthsComboBox.SelectedIndex == 9 ||
                MonthsComboBox.SelectedIndex == 11 || MonthsComboBox.SelectedItem == null)
            {
                if (int.TryParse(day.Text, out int Day))
                {
                    if (Day < 1 || Day > 31)
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Red;
                        _day_flag = false;
                    }
                    else
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Black;
                        _day_flag = true;
                    }
                }
            }
            else if (MonthsComboBox.SelectedIndex == 1)
            {
                if (int.TryParse(day.Text, out int Day))
                {
                    if (Day < 1 || Day > 28)
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Red;
                        _day_flag = false;
                    }
                    else
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Black;
                        _day_flag = true;
                    }
                }
            }
            
            else
            {
                if (int.TryParse(day.Text, out int Day))
                {
                    if (Day < 1 || Day > 30)
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Red;
                        _day_flag = false;
                    }
                    else
                    {
                        day.BorderThickness = new Thickness(1);
                        day.BorderBrush = Brushes.Black;
                        _day_flag = true;
                    }
                }
            }
        }

        public void Year_Check()
        {
            if (MonthsComboBox.SelectedIndex == 2)
            {
                if (int.TryParse(day.Text, out int Day) && int.TryParse(year.Text, out int Year))
                {
                    if (Day <= 19 && (Year <= 1935 || Year > 2007))
                    {
                        year.BorderThickness = new Thickness(1);
                        year.BorderBrush = Brushes.Red;
                        _year_flag = false;
                    }
                    else if (Year <= 1934 || Year > 2006)
                    {
                        year.BorderThickness = new Thickness(1);
                        year.BorderBrush = Brushes.Red;
                        _year_flag = false;
                    }
                    else
                    {
                        year.BorderBrush = Brushes.Black;
                        _year_flag = true;
                    }
                }
                
            }
            else if (MonthsComboBox.SelectedIndex == 0 || MonthsComboBox.SelectedIndex == 1) {
                if (int.TryParse(year.Text, out int Year))
                {
                    if (Year <= 1935 || Year > 2007)
                    {
                        year.BorderThickness = new Thickness(1);
                        year.BorderBrush = Brushes.Red;
                        _year_flag = false;
                    }
                    else
                    {
                        year.BorderBrush = Brushes.Black;
                        _year_flag = true;
                    }
                }
            }
            else
            {
                if (int.TryParse(year.Text, out int Year))
                {
                    if (Year <= 1934 || Year > 2006)
                    {
                        year.BorderThickness = new Thickness(1);
                        year.BorderBrush = Brushes.Red;
                        _year_flag = false;
                    }
                    else
                    {
                        year.BorderBrush = Brushes.Black;
                        _year_flag = true;
                    }
                }
            }
        }
        private void MonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Day_Check();
            Year_Check();
        }

        private void day_KeyUp(object sender, KeyEventArgs e)
        {
            Day_Check();
            Year_Check();
        }

        public class Person
        {
            public string Country { get; set; }
            public string Phone { get; set; }
            public string SurName { get; set; }
            public string FirstName { get; set; }
            public string BirthDate { get; set; }

            

        }
        private void SaveToJson(string filename, string country, string phone, string Surname, string Firstname, string birthdate)
        {
            var data = new Person
            {
                Country = country,
                Phone = phone,
                SurName = Surname,
                FirstName = Firstname,
                BirthDate = birthdate,

            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filename, json, Encoding.UTF8);
        }
        private void Sender_Click(object sender, RoutedEventArgs e)
        {
            string cleanednums = PhoneNumber.Text.Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "")
                .Replace("(", "").Replace(")", "").Replace("-", "");
            if (_day_flag == false || _year_flag == false || Validation.GetHasError(Surname) || Validation.GetHasError(Firstname) ||
                MonthsComboBox.SelectedItem == null || Country.SelectedItem == null || PhoneType.SelectedItem == null || !string.IsNullOrEmpty(cleanednums))
            {
                MessageBox.Show("Неверно введены некоторые данные!");
            }
            else
            {
                string country = Country.SelectedItem.ToString();
                string phone = PhoneCode.Text + "-" + PhoneNumber.Text;
                string SurName = Surname.Text;
                string FirstName = Firstname.Text;
                string BirthDay = day.Text + "/" + MonthsComboBox.SelectedItem.ToString() + "/" + year.Text;

                SaveToJson("anketa.json", country, phone, SurName, FirstName, BirthDay);
                MessageBox.Show("Данные сохранены успешно!");
            }
        }

        private void year_KeyUp(object sender, KeyEventArgs e)
        {
            Day_Check();
            Year_Check();
        }
    }

    public class Months
    {
        public string month { get; set; } = "";
        public override string ToString() => $"{month}";
    }
}
