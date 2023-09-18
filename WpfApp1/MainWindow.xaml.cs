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
            //MessageBox.Show("Hello World","訊息");
            int number;
            List<int> primes = new List<int>();
            bool success =int.TryParse(textbox1.Text, out  number); 
            //int n = Convert.ToInt32(textbox1.Text);

            if (!success) {
                MessageBox.Show("請輸入整數", "輸入錯誤");
            }else if (number < 2) {
                MessageBox.Show($"輸入的值為{number} ，小於2，請重新輸入", "輸入錯誤");
            }
            else
            {
                for(int i = 2; i < number; i++) { 
                    if(IsPrime(i)) {
                        primes.Add(i);
                    }
                }
            }

            ListResult(primes, number);
        }
        private void ListResult(List<int> myPrimes, int n)
        {
            string primeList = $"小於{n}的質數為：";
            string primeMultiple = "";
            foreach (int p in myPrimes)
            {
                primeList += $"{p}  ";
                primeMultiple += $"{p}的倍數為： ";
                int i = 1;
                while (p * i <= n)
                {
                    primeMultiple += $"{p * i}  ";
                    i++;
                }
                primeMultiple += "\n";
            }
            block1.Text = primeList;
            block2.Text = primeMultiple;

        }

        private bool IsPrime(int p)
        {
            for(int i = 2; i < p; i++)
            {
                if (p % i == 0) return false;
            }
            return true;
        }


    }
}



