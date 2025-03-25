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

namespace Wpf_lab7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public int _step = 25;
        public int _count = 0;
        public int _attempts = 10;
        public MainWindow()
        {
            InitializeComponent();
            Update_vals();
            check_step();
            rect_fill();
            invert_label();
            Locker();
            Attempt();
        }

        public void Update_vals()
        {
            red_val.Text = red_balance.Value.ToString();
            green_val.Text = green_balance.Value.ToString();
            blue_val.Text = blue_balance.Value.ToString();
        }

        public void check_step()
        {
            if(red_balance.Value % 25 != 0)
            {
                red_balance.Value += 25 - red_balance.Value % 25;
            }
        }
        
        public void rect_fill()
        {
            byte red = (byte)red_balance.Value;
            byte green = (byte)green_balance.Value;
            byte blue = (byte)blue_balance.Value;
            rectangle.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));
        }

        public void invert_label()
        {
            byte L_red = (byte)(255 - red_balance.Value);
            byte L_green = (byte)(255 - green_balance.Value);
            byte L_blue = (byte)(255 -blue_balance.Value);
            hello.Foreground = new SolidColorBrush(Color.FromRgb(L_red, L_green, L_blue));
        }

        public void Random_Settings()
        {
            var rnd = new Random();
            red_balance.Value = rnd.Next(0, 11) * 25;
            green_balance.Value = rnd.Next(0, 256);
            blue_balance.Value = rnd.Next(0, 256);
            Update_vals();
            invert_label();
            rect_fill();
        }

        public void Locker()
        {
            if (_count >= 10)
            {
                Randomizer.IsEnabled = false;
            }
            else
            {
                Randomizer.IsEnabled = true;
            }
        }

        public void Attempt()
        {
            attempt.Text = $"осталось тыков: {(_attempts - _count).ToString()}";
        }

        private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            rectangle.Opacity = 1 - scroll.Value;
        }

        private void rectangle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scroll.Value += e.Delta < 0 ? 0.1 : -0.1;
            rectangle.Opacity = 1 - scroll.Value;
        }

        private void red_balance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            check_step();
            red_val.Text = red_balance.Value.ToString();
            rect_fill();
            invert_label();
        }

        private void green_balance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rect_fill();
            invert_label();
            green_val.Text = green_balance.Value.ToString();
        }

        private void blue_balance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rect_fill();
            invert_label();
            blue_val.Text = blue_balance.Value.ToString();
        }

        private void Randomizer_Click(object sender, RoutedEventArgs e)
        {
            Random_Settings();
            _count++;
            Locker();
            Attempt();
        }
    }
}
