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
using L9_Table;

namespace L9_Table
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StudentManager _studentManager = new StudentManager();
        public MainWindow()
        {
            InitializeComponent();
            CityComboBox.ItemsSource = new Cities[]
            {
                new Cities {City = "Калининград", DistanceFromMoscow = 1259},
                new Cities {City = "Москва", DistanceFromMoscow = 0},
                new Cities {City = "Санкт-Петербург", DistanceFromMoscow = 704},
                new Cities {City = "Новосибирск", DistanceFromMoscow = 3380},
                new Cities {City = "Иркутск", DistanceFromMoscow = 5251}
            };
        }
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedCity = (Cities)CityComboBox.SelectedItem;

                var student = new Student
                {
                    SurName = LastNameTextBox.Text,
                    FirstName = FirstNameTextBox.Text,
                    Patronymic = PatronymicTextBox.Text,
                    BirthDate = BirthDatePicker.SelectedDate ?? DateTime.Now,
                    Height = double.Parse(HeightTextBox.Text),
                    City = selectedCity.City,
                    DistanceFromMoscow = selectedCity.DistanceFromMoscow
                };

                _studentManager.AddStudent(student);
                

                // Очистка полей
                LastNameTextBox.Clear();
                FirstNameTextBox.Clear();
                PatronymicTextBox.Clear();
                BirthDatePicker.SelectedDate = DateTime.Now;
                HeightTextBox.Clear();
                CityComboBox.SelectedItem = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении студента: {ex.Message}");
            }
        }
    }
}
