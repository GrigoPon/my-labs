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
using System.Diagnostics.PerformanceData;

namespace Lab2_wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _count = 0;
        
        public string CounterText => $"Счет: {_count}";

        public void Increment()
        {
            int previousCount = _count;
            _count += 1;
            CheckSignChange(previousCount);
            OnPropertyChanged(nameof(CounterText));
        }

        public void Decrement()
        {
            int previousCount = _count;
            _count -= 2;
            CheckSignChange(previousCount);
            OnPropertyChanged(nameof(CounterText));
        }

        private void CheckSignChange(int previousCount)
        {
            if (previousCount >= 0 && _count < 0)
            {
                MessageBox.Show("Значение стало отрицательным!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (_count == 3)
            {
                MessageBox.Show("Значение совпавдает с номером твоего варианта!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            ChangeLabelContent();
        }

        public void ChangeLabelContent()
        {
            l1.Content = "Содержимое";
            l2.Content = "Content";
            l3.Content = "Contenuto";
        }

        private void L2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _viewModel.Increment();
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                _viewModel.Decrement();
            }
        }
    }


    
}

