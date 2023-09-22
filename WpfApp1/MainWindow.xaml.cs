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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Double numberA,numberB,numberC;

            bool A1 = Double.TryParse(TextBox01.Text, out numberA);
            bool B2 = Double.TryParse(TextBox02.Text, out numberB);
            bool C3 = Double.TryParse(TextBox03.Text, out numberC);
            
            if( !A1 || !B2 || !C3)
            {
                MessageBox.Show("請輸入整數", "輸入錯誤");
            }
            else if (numberA <=0 || numberB <=0 || numberC<=0) 
            {
                MessageBox.Show($"輸入的值不可小於0，請重新輸入","輸入錯誤");
            }
            else
            {
               if(numberA + numberB > numberC && numberB + numberC > numberA && numberA + numberC > numberB) 
                {
                    string success = $"{numberA},{numberB},{numberC}\n可以構成三角形";
                    Cout.Text = success;
                    TextboxReset();
                }
                else
                {
                    string Unable = $"{numberA},{numberB},{numberC}\n無法構成三角形";
                    Cout.Text= Unable;
                    TextboxReset();
                }
            }
        }

        private void TextboxReset()
        {
            TextBox01.Text = "";
            TextBox02.Text = "";
            TextBox03.Text = "";

        }
    }
}
