using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Triangle> triangles = new List<Triangle>();
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.099);
            timer.Tick += Timer_Tick;

            originalBitmap = new BitmapImage();
            originalBitmap.BeginInit();
            originalBitmap.UriSource = new Uri(originalImagePath, UriKind.Relative);
            originalBitmap.EndInit();
            cat.Source = originalBitmap;
        }

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Double numberA,numberB,numberC;

            bool A1 = Double.TryParse(TextBox01.Text, out numberA);
            bool B2 = Double.TryParse(TextBox02.Text, out numberB);
            bool C3 = Double.TryParse(TextBox03.Text, out numberC);
            
            if ( !A1 || !B2 || !C3 || numberA <=0 || numberB <=0 || numberC<=0)
            {
                MessageBox.Show("請輸入正確數值不可小於0或是空白", "輸入錯誤");
                return;
            }
            Triangle triangle = new Triangle(numberA, numberB, numberC);
            if (triangle.IsValid)
            {
                ltest.Content = $"邊長 {numberA}, {numberB}, {numberC} 可以構成三角形";
                ltest.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5FEB64"));

            }
            else
            {
                ltest.Content = $"邊長 {numberA}, {numberB}, {numberC} 不可構成三角形";
                ltest.Background = Brushes.Red;
            }
            Cout.Text += $"{ltest.Content}\n";
            TextboxReset();
            string newImgagePath = "cat2.jpeg";
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(newImgagePath, UriKind.Relative);
            bitmap.EndInit();
            cat.Source = bitmap;
            timer.Start();
            

        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 防止換行
                e.Handled = true;

                // 尋找下一個 TextBox
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
            }
        }
        private DispatcherTimer timer = new DispatcherTimer();

        private string originalImagePath = "cat1.jpeg"; 
     
        private BitmapImage originalBitmap;

        private void Timer_Tick(object? sender, EventArgs e)
        {

            // 停止計時器
            timer.Stop();

            // 切換回原始圖片
            cat.Source = originalBitmap;

        }
        private void TextboxReset()
        {
            TextBox01.Text = "";
            TextBox02.Text = "";
            TextBox03.Text = "";

        }
    }
}
