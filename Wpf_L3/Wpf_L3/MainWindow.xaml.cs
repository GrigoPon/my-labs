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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Wpf_L3
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _startPosX = 200;
        private int _startPosY = 200;
        private const int _step = 20;
        public MainWindow()
        {
            InitializeComponent();
            LabelPos(_startPosX, _startPosY);
        }

        public void LabelPos(int x, int y)
        {
            Krest.Margin = new Thickness(x, y, 0, 0);
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            int currentX = (int)Krest.Margin.Left;
            int currentY = (int)Krest.Margin.Top;
            
            int dX = currentX - _startPosX;
            int dY = currentY - _startPosY;
            MessageBox.Show($"Текущее положение: ({currentX}, {currentY})\n\n" +
                $"Отклонение: ({dX}, {dY})");
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            if (Krest.Margin.Top > 0)
            {
                ThicknessAnimation animation = new ThicknessAnimation
                {
                    From = new Thickness(Krest.Margin.Left, Krest.Margin.Top, 0, 0),  // Начальное значение Margin
                    To = new Thickness(Krest.Margin.Left, Krest.Margin.Top - _step, 0, 0),  // Конечное значение Margin
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)),  // Длительность анимации
                    AutoReverse = false,  // Автоматическое возвращение к начальному значению
                    //RepeatBehavior = RepeatBehavior.Forever  // Бесконечное повторение
                };
                Krest.Margin = new Thickness(Krest.Margin.Left, Krest.Margin.Top - _step, 0, 0);
                Krest.BeginAnimation(FrameworkElement.MarginProperty, animation);
            }
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            if (Krest.Margin.Top < this.Height - Krest.ActualHeight - _step)
            {
                ThicknessAnimation animation = new ThicknessAnimation
                {
                    From = new Thickness(Krest.Margin.Left, Krest.Margin.Top, 0, 0),  // Начальное значение Margin
                    To = new Thickness(Krest.Margin.Left, Krest.Margin.Top + _step, 0, 0),  // Конечное значение Margin
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)),  // Длительность анимации
                    AutoReverse = false,  // Автоматическое возвращение к начальному значению
                    //RepeatBehavior = RepeatBehavior.Forever  // Бесконечное повторение
                };

                Krest.Margin = new Thickness(Krest.Margin.Left, Krest.Margin.Top + _step, 0, 0);
                Krest.BeginAnimation(FrameworkElement.MarginProperty, animation);
            }
            //MessageBox.Show($"{L3.Height}");
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            if (Krest.Margin.Left > 0)
            {
                ThicknessAnimation animation = new ThicknessAnimation
                {
                    From = new Thickness(Krest.Margin.Left, Krest.Margin.Top, 0, 0),  // Начальное значение Margin
                    To = new Thickness(Krest.Margin.Left - _step, Krest.Margin.Top, 0, 0),  // Конечное значение Margin
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)),  // Длительность анимации
                    AutoReverse = false,  // Автоматическое возвращение к начальному значению
                    //RepeatBehavior = RepeatBehavior.Forever  // Бесконечное повторение
                };

                Krest.Margin = new Thickness(Krest.Margin.Left - _step, Krest.Margin.Top, 0, 0);
                Krest.BeginAnimation(FrameworkElement.MarginProperty, animation);
            }
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            if (Krest.Margin.Left < this.Width - Krest.ActualWidth)
            {
                ThicknessAnimation animation = new ThicknessAnimation
                {
                    From = new Thickness(Krest.Margin.Left, Krest.Margin.Top, 0, 0),  // Начальное значение Margin
                    To = new Thickness(Krest.Margin.Left + _step, Krest.Margin.Top, 0, 0),  // Конечное значение Margin
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)),  // Длительность анимации
                    AutoReverse = false,  // Автоматическое возвращение к начальному значению
                    //RepeatBehavior = RepeatBehavior.Forever  // Бесконечное повторение
                };

                Krest.Margin = new Thickness(Krest.Margin.Left + _step, Krest.Margin.Top, 0, 0);
                Krest.BeginAnimation(FrameworkElement.MarginProperty, animation);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Krest.Margin = new Thickness(_startPosX, _startPosY, 0, 0);
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = new Thickness(Krest.Margin.Left, Krest.Margin.Top, 0, 0),  // Начальное значение Margin
                To = new Thickness(_startPosX, _startPosY, 0, 0),  // Конечное значение Margin
                Duration = new Duration(TimeSpan.FromSeconds(0.1)),  // Длительность анимации
                AutoReverse = false,  // Автоматическое возвращение к начальному значению
                                      //RepeatBehavior = RepeatBehavior.Forever  // Бесконечное повторение
            };
            Krest.Margin = new Thickness(_startPosX, _startPosY, 0, 0);
            Krest.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }
    }
}
