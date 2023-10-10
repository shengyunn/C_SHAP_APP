﻿using System;
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
        Dictionary<string,int> drinks = new Dictionary<string,int>();
        Dictionary<string,int> orders = new Dictionary<string,int>();
        public MainWindow()
        {
            InitializeComponent();
            //add all drinks list
            AddNewDrinks(drinks);

        }

        private void AddNewDrinks(Dictionary<string, int> mydrinks)
        {
            mydrinks.Add("紅茶大杯", 60);
            mydrinks.Add("紅茶小杯", 40);
            mydrinks.Add("綠茶大杯", 60);
            mydrinks.Add("綠茶小杯", 40);
            mydrinks.Add("咖啡大杯", 80);
            mydrinks.Add("咖啡小杯", 50);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox targetTexBox = sender as TextBox;

            bool succes = int.TryParse(targetTexBox.Text, out int amount);

            if(!succes)MessageBox.Show("請輸入整數", "輸入錯誤");
            else if(amount <= 0)  MessageBox.Show("請輸入正整數", "輸入錯誤");
            else
            {
                StackPanel targetStackPanel = targetTexBox.Parent as StackPanel;
                Label targetLabel = targetStackPanel.Children[0] as Label;
                string drinkName = targetLabel.Content.ToString();
                if(orders.ContainsKey(drinkName)) orders.Remove(drinkName);
                orders.Add(drinkName, amount);
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            double total = 0.0;
            double sellPrice = 0.0;
            string displayString = "訂購清單如下:\n";
            string message = "";

            foreach(KeyValuePair<string, int> item in orders)
            {
                string drinkName = item.Key;
                int amount = orders[drinkName];
                int price = drinks[drinkName];
                total += price * amount;
                displayString += $"{drinkName} X {amount}杯，每杯 {price}元，總共 {price * amount}元\n";
            }

            if (total >= 500)
            {
                message = "訂購滿500元以上者8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                message = "訂購滿300元以上者85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                message = "訂購滿200元以上者9折";
                sellPrice = total * 0.9;
            }
            else
            {
                message = "訂購未滿200元以上不打折";
                sellPrice = total;
            }
            displayString += $"本次訂購總共{orders.Count}項，{message}，售價{sellPrice}元";
            textblock1.Text = displayString;
        }
    }
}
