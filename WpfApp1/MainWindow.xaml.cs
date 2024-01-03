using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _2023_WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 宣告一些成員變數，用來存儲畫布的狀態
        String shapeType = "Line";
        String actionType = "Draw";
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Yellow;
        int strokeThickness = 1;
        // 起始點和終點座標
        Point start, dest;

        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }
        // 圖形按鈕點擊事件處理
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            actionType = "Draw";
            DisplayStatus();
            //MessageBox.Show(shapeType);
        }
        // 畫筆粗細滑塊值變更事件處理
        private void strokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(strokeThicknessSlider.Value);
        }
        // 畫布滑鼠移動事件處理
        private void myCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            DisplayStatus();

            switch (actionType)
            {
                case "Draw": //繪圖模式
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point origin = new Point
                        {
                            X = Math.Min(start.X, dest.X),
                            Y = Math.Min(start.Y, dest.Y)
                        };
                        double width = Math.Abs(dest.X - start.X);
                        double height = Math.Abs(dest.Y - start.Y);

                        switch (shapeType)
                        {
                            case "Line":
                                var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                                line.X2 = dest.X;
                                line.Y2 = dest.Y;
                                break;
                            case "Rectangle":
                                var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width;
                                rect.Height = height;
                                rect.SetValue(Canvas.LeftProperty, origin.X);
                                rect.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Ellipse":
                                var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width;
                                ellipse.Height = height;
                                ellipse.SetValue(Canvas.LeftProperty, origin.X);
                                ellipse.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Polyline":
                                var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(dest);
                                break;
                        }
                    }
                    break;
                case "Erase": //橡皮擦模式
                    var shape = e.OriginalSource as Shape;
                    myCanvas.Children.Remove(shape);
                    if (myCanvas.Children.Count == 0) myCanvas.Cursor = Cursors.Arrow;
                    break;
            }
        }
        // 畫布滑鼠左鍵點擊事件處理
        private void myCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;

            if (actionType == "Draw")
            {
                switch (shapeType)
                {
                    case "Line":
                        var line = new Line
                        {
                            Stroke = Brushes.Gray,
                            StrokeThickness = 1,
                            X1 = start.X,
                            Y1 = start.Y,
                            X2 = dest.X,
                            Y2 = dest.Y
                        };
                        myCanvas.Children.Add(line);
                        break;
                    case "Rectangle":
                        var rect = new Rectangle
                        {
                            Stroke = Brushes.Gray,
                            StrokeThickness = 1,
                            Fill = Brushes.LightGray,
                        };
                        myCanvas.Children.Add(rect);
                        rect.SetValue(Canvas.LeftProperty, start.X);
                        rect.SetValue(Canvas.TopProperty, start.Y);
                        break;
                    case "Ellipse":
                        var ellipse = new Ellipse
                        {
                            Stroke = Brushes.Gray,
                            StrokeThickness = 1,
                            Fill = Brushes.LightGray,
                        };
                        myCanvas.Children.Add(ellipse);
                        ellipse.SetValue(Canvas.LeftProperty, start.X);
                        ellipse.SetValue(Canvas.TopProperty, start.Y);
                        break;
                    case "Polyline":
                        var polyline = new Polyline
                        {
                            Stroke = Brushes.Gray,
                            StrokeThickness = 1,
                            Fill = Brushes.LightGray,
                        };
                        myCanvas.Children.Add(polyline);
                        break;
                }
            }

            DisplayStatus();
        }
        // 顯示目前畫布狀態的方法
        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();
            int polylineCount = myCanvas.Children.OfType<Polyline>().Count();
            coordinateLabel.Content = $"{actionType}模式 || 座標點: ({Math.Round(start.X)}, {Math.Round(start.Y)}) : ({Math.Round(dest.X)}, {Math.Round(dest.Y)})";
            shapeLabel.Content = $"Line: {lineCount}, Rectangle: {rectCount}, Ellipse: {ellipseCount}, Polyline: {polylineCount}";
        }
        // 畫筆顏色選擇器值變更事件處理
        private void strokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)strokeColorPicker.SelectedColor;
        }
        // 填充顏色選擇器值變更事件處理
        private void fillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)fillColorPicker.SelectedColor;
        }
        // 清空畫布選單項目點擊事件處理
        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }
        // 橡皮擦按鈕點擊事件處理
        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            actionType = "Erase";
            myCanvas.Cursor = Cursors.Hand;
            DisplayStatus();
        }

        private void saveCanvas_Click(object sender, RoutedEventArgs e)
        {
            // Show the SaveFileDialog to allow the user to choose the file name and location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "儲存畫布";
            saveFileDialog.Filter = "Png檔案|*.png|所有檔案|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Create a RenderTargetBitmap to capture the canvas content
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)myCanvas.ActualWidth,
                    (int)myCanvas.ActualHeight,
                    64d, 64d, PixelFormats.Default);

                // Render the canvas to the RenderTargetBitmap
                renderBitmap.Render(myCanvas);

                // Create a BitmapEncoder (e.g., PNGEncoder) to save the image
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                // Create a file stream using the user-selected file name
                string fileName = saveFileDialog.FileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                //MessageBox.Show($"Canvas content saved as {fileName}");
            }
        }
        // 畫布滑鼠左鍵釋放事件處理
        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (actionType == "Draw")
            {
                switch (shapeType)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.Stroke = new SolidColorBrush(strokeColor);
                        line.StrokeThickness = strokeThickness;
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        rect.Stroke = new SolidColorBrush(strokeColor);
                        rect.Fill = new SolidColorBrush(fillColor);
                        rect.StrokeThickness = strokeThickness;
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        ellipse.Stroke = new SolidColorBrush(strokeColor);
                        ellipse.Fill = new SolidColorBrush(fillColor);
                        ellipse.StrokeThickness = strokeThickness;
                        break;
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        polyline.Stroke = new SolidColorBrush(strokeColor);
                        polyline.Fill = new SolidColorBrush(fillColor);
                        polyline.StrokeThickness = strokeThickness;
                        break;
                }
                myCanvas.Cursor = Cursors.Arrow;
            }
        }
    }
}