using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // API 的 URL
        string url = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=e8dd42e6-9b8b-43f8-991e-b3dee723a52d&limit=1000&sort=ImportDate%20desc&format=JSON";
        AQIdata aqidata = new AQIdata();
        List<Field> fields = new List<Field>();
        List<Record> records = new List<Record>();
        List<Record> selectedRecords = new List<Record>();
        SeriesCollection seriesCollection = new SeriesCollection();

        public MainWindow()
        {
            InitializeComponent();
            // 將 URL 設定到文字方塊
            UrlTextBox.Text = url;
            selectedRecords.Clear();
        }

        // 按下「抓取網路資料」按鈕的事件處理
        private async void GetWebDataButton_Click(object sender, RoutedEventArgs e)
        {
            // 顯示訊息，表示正在抓取網路資料
            ContentTextBox.Text = "正在抓取網路資料...";

            // 非同步取得 API 回傳的 JSON 資料
            string jsontext = await FetchContentAsync(url);
            // 顯示 JSON 資料在文字方塊中
            ContentTextBox.Text = jsontext;
            // 將 JSON 資料反序列化為 AQIdata 物件
            aqidata = JsonSerializer.Deserialize<AQIdata>(jsontext);
            // 取得欄位清單和 AQI 記錄清單
            fields = aqidata.fields.ToList();
            records = aqidata.records.ToList();
            // 預設選取所有 AQI 記錄
            selectedRecords = records;
            // 顯示 AQI 資料
            DisplayAQIData();
        }

        // 顯示 AQI 資料的方法
        private void DisplayAQIData()
        {
            // 設定資料表格的資料來源
            RecordDataGrid.ItemsSource = records;
            // 清空資料選擇面板的內容
            DataWrapPanel.Children.Clear();

            // 遍歷所有欄位
            foreach (var field in fields)
            {
                // 取得 AQI 記錄中對應欄位的值
                var propertyInfo = typeof(Record).GetProperty(field.id);
                if (propertyInfo != null)
                {
                    // 取得欄位值轉換為字串
                    string value = propertyInfo.GetValue(records[0]) as string;
                    if (double.TryParse(value, out double v))
                    {
                        // 建立 Checkbox 並加入事件處理
                        CheckBox cb = new CheckBox
                        {
                            Content = field.info.label,
                            Tag = field.id,
                            Margin = new Thickness(3),
                            Width = 120,
                            FontSize = 14,
                            FontWeight = FontWeights.Bold,
                        };

                        cb.Checked += UpdateChart;
                        cb.Unchecked += UpdateChart;
                        // 將 Checkbox 加入資料選擇面板
                        DataWrapPanel.Children.Add(cb);
                    }
                }
            }
        }

        // 更新圖表的方法
        private void UpdateChart(object sender, RoutedEventArgs e)
        {
            // 清空圖表的 SeriesCollection
            seriesCollection.Clear();

            // 遍歷所有資料選擇面板的 Checkbox
            foreach (CheckBox cb in DataWrapPanel.Children)
            {
                if (cb.IsChecked == true)
                {
                    // 取得 Checkbox 的 Tag (欄位名稱)
                    var tag = cb.Tag as String;
                    // 建立 ColumnSeries 和 ChartValues
                    ColumnSeries columnSeries = new ColumnSeries();
                    ChartValues<double> values = new ChartValues<double>();
                    List<String> labels = new List<String>();

                    // 遍歷所有選取的 AQI 記錄
                    foreach (var record in selectedRecords)
                    {
                        var propertyInfo = typeof(Record).GetProperty(tag);
                        if (propertyInfo != null)
                        {
                            // 取得對應欄位的值轉換為字串
                            string value = propertyInfo.GetValue(record) as string;
                            if (double.TryParse(value, out double v))
                            {
                                // 將值加入 ChartValues 和標籤清單
                                values.Add(v);
                                labels.Add(record.sitename);
                            }
                        }
                    }
                    // 設定 ColumnSeries 的屬性
                    columnSeries.Values = values;
                    columnSeries.Title = tag;
                    columnSeries.LabelPoint = point => $"{labels[(int)point.X]}: {point.Y.ToString()}";
                    // 加入 ColumnSeries 到 SeriesCollection
                    seriesCollection.Add(columnSeries);
                }
            }
            // 更新圖表的 SeriesCollection
            AQIChart.Series = seriesCollection;
        }

        // 非同步取得網路內容的方法
        private async Task<string> FetchContentAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex)
            {
                return $"發生錯誤: {ex.Message}";
            }
        }

        // 資料表格選取項目改變事件處理
        private void RecordDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 更新選取的 AQI 記錄清單和狀態文字
            selectedRecords = RecordDataGrid.SelectedItems.Cast<Record>().ToList();
            StatusTextBlock.Text = $"共選取 {selectedRecords.Count} 筆資料";
        }

        // 資料表格載入行事件處理
        private void RecordDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // 設定資料表格行的標頭
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}