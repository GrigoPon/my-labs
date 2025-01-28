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

namespace Lab1WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            L1.Background = Brushes.Coral;
           
        }
        int k = 0;
        private void L1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            k++;
            if (k % 2 == 0)
            {
                L1.Background = Brushes.Coral;
            }
            else
            {
                L1.Background = Brushes.Aquamarine;
            }
        }
        int i = 0;
        private void L1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            i++;
            if (i % 2 == 0) { 
            
                L1.WindowState = WindowState.Normal;
            }
            else
            {
                L1.WindowState = WindowState.Maximized;
            }
                
        }

        
        private void L1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                double AvW = (L1.MaxWidth + L1.MinWidth) / 2;
                double AvH = (L1.MaxHeight + L1.MinHeight) / 2;
                if (L1.Width < AvW)
                {
                    L1.Width = AvW + (AvW - L1.Width);
                    
                }
                else if (L1.Width > AvW)
                {
                    L1.Width = AvW - (L1.Width - AvW);
                    
                }
                else
                {
                    L1.Width = AvW;

                }

                if (L1.Height < AvH) { L1.Height = AvH + (AvH - L1.Height); }
                else if (L1.Height > AvH) { L1.Height = AvH - (L1.Height - AvH); }
                else { L1.Height = AvH; }
            }

            if (e.Key == Key.Escape)
            {
                L1.Close();
            }

            if (e.Key == Key.G)
            {
                L1.Title = "Ponyaev_Grigory_L1";
            }
            if (e.Key == Key.P)
            {
                L1.Title = "Поняев_Григорий_Л1";
            }
        }
    }
}
