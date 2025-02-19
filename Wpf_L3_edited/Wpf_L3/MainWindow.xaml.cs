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
using System.Xml;

namespace Wpf_L3
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _startPosX = 200;
        private int _startPosY = 200;
        
        
        private int currX;
        private int currY;

        public MainWindow()
        {
            InitializeComponent();
            LabelPos(_startPosX, _startPosY);
            UpdatingButton();
            Loaded += L3_Loaded;
        }

        public void LabelPos(int x, int y)
        {
            Krest.Margin = new Thickness(x, y, 0, 0);
            currX = _startPosX;
            currY = _startPosY;
        }

        private void UpdatingPos()
        {
          
            Krest.Margin = new Thickness(currX, currY, 0, 0);
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            int currentX = (int)Krest.Margin.Left;
            int currentY = (int)Krest.Margin.Top;
            
            int dX = currX - _startPosX;
            int dY = currY - _startPosY;
            MessageBox.Show($"Текущее положение: ({currX}, {currY})\n\n" +
                $"Отклонение: ({dX}, {dY})");
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            currY -= (int)Krest.ActualHeight - 26;
            UpdatingPos();
            UpdatingButton();

        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            currY += (int)Krest.ActualHeight - 26;
            UpdatingPos();
            UpdatingButton();
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            currX -= (int)Krest.ActualWidth - 12;
            UpdatingPos();
            UpdatingButton();

        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            currX += (int)Krest.ActualWidth - 12;
            UpdatingPos();
            UpdatingButton();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currX = _startPosX;
            currY = _startPosY;
            UpdatingPos();
            UpdatingButton();
        }

        
        private void UpdatingButton()
        {
            up.IsEnabled = Krest.Margin.Top > 0;
            down.IsEnabled = Krest.Margin.Top < Grid.ActualHeight - Krest.ActualHeight - (int)Krest.ActualHeight + 26;
            left.IsEnabled = Krest.Margin.Left > 12;
            right.IsEnabled = Krest.Margin.Left < Grid.ActualWidth - Krest.ActualWidth - (int)Krest.ActualWidth + 12;
        }

        private void L3_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatingButton();
        }
    }
}
