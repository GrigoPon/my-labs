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
using System.Reflection.Emit;
using System.Drawing;

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
            ChangeLabelStyle();
            ChangeLabelFont();
            ChangeLabelFontSize();
            ChangeLabelSize();
            ChangeLabelColor();
        }

        public void ChangeLabelContent()
        {
            l1.Content = "231";
            l2.Content = "Content";
            l3.Content = "Contenuto";
        }

        public void ChangeLabelStyle()
        {
            l3.FontStyle = FontStyles.Italic;
        }

        public void ChangeLabelFont()
        {
            l1.FontFamily = new FontFamily("Times New Roman");
            l2.FontFamily = new FontFamily("Arial");
            l3.FontFamily = new FontFamily("Calibri");
        }

        public void ChangeLabelFontSize()
        {
            l1.FontSize = 40;
            l2.FontSize = 15;
            l3.FontSize = 25;
        }

        public void ChangeLabelSize()
        {
            l1.Width = 200;
            l1.Height = 50;

            l2.Width = 100;
            l2.Height = 90;

            l3.Width = 150;
            l3.Height = 70;
        }

        public void ChangeLabelColor()
        {
            l1.Background = Brushes.Aquamarine;
            l1.Foreground = Brushes.Black;
            l2.Background = Brushes.AliceBlue;
            l3.Background = Brushes.Beige;
            l3.Foreground = Brushes.Purple;
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

