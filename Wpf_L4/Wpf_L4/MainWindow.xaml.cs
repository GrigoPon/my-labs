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

namespace Wpf_L4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public class Quantity
    {
        public int? N1 { get; set; }
        public int? N2 { get; set; }
        public int? K1 { get; set; }
        public int? K2 { get; set; }
    }

    public partial class MainWindow : Window
    {
        private const string _myName = "Гриша";
        private const string _neighbourName = "Влад";
        private List<TextBox> _textBoxes;
        Quantity N;
        public MainWindow()
        {
            InitializeComponent();
            SetNames();
            N = new Quantity();
            
            this.DataContext = N;
            _textBoxes = new List<TextBox> { t1, t2, t3, t4 };
        }



        public void SetNames()
        {
            MyName.Text = _myName;
            NeighbourName.Text = _neighbourName;
        }

        private void Count_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(t1) || Validation.GetHasError(t2) || Validation.GetHasError(t3) || Validation.GetHasError(t4) || string.IsNullOrEmpty(t1.Text) || string.IsNullOrEmpty(t2.Text) || string.IsNullOrEmpty(t3.Text) || string.IsNullOrEmpty(t4.Text))
            {
                foreach (var TextBox in _textBoxes)
                {
                    TextBox.BorderThickness = new Thickness(0);
                    
                    if (string.IsNullOrEmpty(TextBox.Text))
                        TextBox.BorderThickness = new Thickness(1);
                        TextBox.BorderBrush = Brushes.Red;
                }
                MessageBox.Show("Введены некорректные данные!\nУ тебя либо не все поля заполненны, либо яблок 'абвгдеёж'!");
            }
            else
            {

                string N1 = t1.Text;
                string N2 = t3.Text;
                string K1 = t2.Text;
                string K2 = t3.Text;

                Output.Visibility = Visibility.Visible;
                Output.IsEnabled = true;
                if (int.TryParse(N1, out int Apple1) && int.TryParse(N2, out int Apple2) && int.TryParse(K1, out int Pear1) && int.TryParse(K2, out int Pear2))
                {
                    int Apples = Apple1 + Apple2;
                    int Pears = Pear1 + Pear2;
                    Output.Text = $"Всего яблок {Apples}, а груш - {Pears}";
                    foreach (var Textbox in _textBoxes)
                    {
                        Textbox.BorderThickness = new Thickness(0);
                        Textbox.BorderBrush = Brushes.Transparent;
                        Textbox.IsEnabled = false;
                    }
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach(var TextBox in _textBoxes)
            {
                TextBox.BorderBrush = Brushes.Transparent;
                TextBox.IsEnabled = true;
                TextBox.Text = "";
            }
            Output.Visibility = Visibility.Hidden;
            Output.IsEnabled = false;
            Output.Text = "";
        }
    }
}
