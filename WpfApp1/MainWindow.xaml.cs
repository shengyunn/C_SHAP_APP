using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 學生列表
        List<Student> students = new List<Student>();
        Student selectedStudent = null;

        // 課程列表
        List<Course> courses = new List<Course>();
        Course selectedCourse = null;

        // 教師列表
        List<Teacher> teachers = new List<Teacher>();
        Teacher selectedTeacher = null;

        // 選課紀錄列表
        List<Record> records = new List<Record>();
        Record selectedRecord = null;

        public MainWindow()
        {
            InitializeComponent();

            // 初始化學生資料
            InitializeStudentFromJson("C:\\Users\\ryan0\\Downloads\\2023student (1).json");

            // 初始化課程資料
            InititalizeCourse();
        }

        private void InititalizeCourse()
        {
            // 初始化教師及其授課課程
            Teacher teacher1 = new Teacher() { TeacherName = "陳定宏" };
            //... 簡化課程初始化 ...
            Teacher teacher2 = new Teacher() { TeacherName = "陳福坤" };
            //... 簡化課程初始化 ...
            Teacher teacher3 = new Teacher() { TeacherName = "許子衡" };
            //... 簡化課程初始化 ...

            // 將教師加入教師列表
            teachers.Add(teacher1);
            teachers.Add(teacher2);
            teachers.Add(teacher3);

            // 設定 TreeView 的資料來源
            tvTeacher.ItemsSource = teachers;

            // 將教師的授課課程加入課程列表
            foreach (Teacher teacher in teachers)
            {
                foreach (Course course in teacher.TeachingCourses)
                {
                    courses.Add(course);
                }
            }

            // 設定 ListBox 的資料來源
            lbCourse.ItemsSource = courses;
        }

        private void InitializeStudent()
        {
            // 初始化學生列表
            students.Add(new Student { StudentId = "A10001", StudentName = "陳小明" });
            students.Add(new Student { StudentId = "A10002", StudentName = "王小美" });
            students.Add(new Student { StudentId = "A10003", StudentName = "林小英" });
            students.Add(new Student { StudentId = "A10004", StudentName = "黃大山" });

            // 設定 ComboBox 的資料來源
            cmbStudent.ItemsSource = students;
            cmbStudent.SelectedIndex = 0;
        }

        private void cmbStudent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // 當使用者改變 ComboBox 的選擇時，將選取的學生設定為 selectedStudent
            selectedStudent = (Student)cmbStudent.SelectedItem;
            labelStatus.Content = $"選取學生：{selectedStudent?.ToString()}";
        }

        private void tvTeacher_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 當使用者改變 TreeView 選取項目時，根據選取的項目類型進行相應的處理
            if (tvTeacher.SelectedItem is Teacher)
            {
                selectedTeacher = (Teacher)tvTeacher.SelectedItem;
                labelStatus.Content = $"選取教師：{selectedTeacher?.ToString()}";
            }
            else if (tvTeacher.SelectedItem is Course)
            {
                selectedCourse = (Course)tvTeacher.SelectedItem;
                labelStatus.Content = selectedCourse?.ToString();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // 學生選課按鈕的點擊事件處理
            if (selectedStudent == null || selectedCourse == null)
            {
                MessageBox.Show("請選取學生或課程");
                return;
            }
            else
            {
                // 建立新的選課紀錄
                Record newRecord = new Record() { SelectedStudent = selectedStudent, SelectedCourse = selectedCourse };

                // 檢查是否已經存在相同的選課紀錄
                foreach (Record r in records)
                {
                    if (r.Equals(newRecord))
                    {
                        MessageBox.Show($"{selectedStudent.StudentName}已選取 {selectedCourse.CourseName}");
                        return;
                    }
                }

                // 加入新的選課紀錄，並更新列表
                records.Add(newRecord);
                lvRecord.ItemsSource = records;
                lvRecord.Items.Refresh();
            }
        }

        private void lbCourse_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // 當使用者改變 ListBox 的選擇時，將選取的課程設定為 selectedCourse
            selectedCourse = (Course)lbCourse.SelectedItem;
            labelStatus.Content = selectedCourse?.ToString();
        }

        private void lvRecord_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // 當使用者改變 ListView 的選擇時，將選取的選課紀錄設定為 selectedRecord
            selectedRecord = (Record)lvRecord.SelectedItem;
            if (selectedRecord != null) labelStatus.Content = selectedRecord.ToString();
        }

        private void btnWithdrawl_Click(object sender, RoutedEventArgs e)
        {
            // 按下退選按鈕的事件處理
            if (selectedRecord != null)
            {
                // 移除選課紀錄，並更新列表
                records.Remove(selectedRecord);
                lvRecord.ItemsSource = records;
                lvRecord.Items.Refresh();
            }
        }

        private void InitializeStudentFromJson(string filePath)
        {
            // 從 JSON 檔案初始化學生列表
            try
            {
                string jsonString = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(jsonString);

                // 設定 ComboBox 的資料來源
                cmbStudent.ItemsSource = students;
                cmbStudent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法載入學生資料: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 儲存學生選課紀錄按鈕的點擊事件處理
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json文件 (*.json)|*.json|All Files (*.*)|*.*";
            saveFileDialog.Title = "儲存學生選課紀錄";

            if (saveFileDialog.ShowDialog() == true)
            {
                // 使用 JsonSerializer 將選課紀錄轉換成 JSON 格式，並儲存到指定的檔案
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(records, options);
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
        }
    }
}
