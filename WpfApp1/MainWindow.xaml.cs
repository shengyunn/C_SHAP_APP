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
        String shapeType;
        Color strokeColor = Colors.Red;
        int strokeThickness = 1;
        Brush strokeBrush = new SolidColorBrush(Colors.Red);
        Point start, dest;

        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            //MessageBox.Show(shapeType);
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;
            switch (shapeType)
            {
                case "Line":
                    break;
                case "Rectangle":
                    break;
                case "Ellipse":
                    break;
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void strokeThicknessSider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(strokeThicknessSider.Value);
        }
    }
}
