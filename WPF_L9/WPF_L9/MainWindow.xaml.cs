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
using System.IO;
using L9_Table;

namespace WPF_L9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StudentManager _studentManager;

        public MainWindow()
        {
            InitializeComponent();
            _studentManager = new StudentManager();
            var students = _studentManager.GetAllStudents().ToList();
            Console.WriteLine($"Загружено студентов: {students.Count}");
            ShowAllStudents();
        }

        private void ShowAllStudents()
        {
            StudentsDataGrid.ItemsSource = _studentManager.GetAllStudents().ToList();
            StudentsDataGrid.AutoGenerateColumns = true;
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAllStudents();
        }

        private void ShowMoscow_Click(object sender, RoutedEventArgs e)
        {
            StudentsDataGrid.ItemsSource = _studentManager.GetStudentsFromCity("Москва");
        }

        private void ShowAdults_Click(object sender, RoutedEventArgs e)
        {
            StudentsDataGrid.ItemsSource = _studentManager.GetAdultStudents();
        }

        private void ShowByYear_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(YearTextBox.Text, out int year))
            {
                StudentsDataGrid.ItemsSource = _studentManager.GetStudentsByBirthYear(year);
            }
            else
            {
                MessageBox.Show("Введите корректный год");
            }
        }

        private void ShowIntroduction_Click(object sender, RoutedEventArgs e)
        {
            StudentsDataGrid.ItemsSource = _studentManager.GetStudentsIntroduction();
        }

        private void ShowByDistance_Click(object sender, RoutedEventArgs e)
        {
            StudentsDataGrid.ItemsSource = _studentManager.GetStudentsByDistance();
        }
    }
}
