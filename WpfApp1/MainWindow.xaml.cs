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
        List<Student> students = new List<Student>();
        Student selectedStudent = null;

        List<Course> courses = new List<Course>();
        Course selectedCourse = null;
        List<Teacher> teachers = new List<Teacher>();
        Teacher selectedTeacher = null;

        List<Record> records = new List<Record>();
        Record selectedRecord = null;
        public MainWindow()
        {
            InitializeComponent();

            //InitializeStudent();

            // 修改這裡，提供 JSON 學生資料檔案的路徑
            InitializeStudentFromJson("C:\\Users\\ryan0\\Downloads\\2023student (1).json");

            InititalizeCourse();
        }

        private void InititalizeCourse()
        {
            Teacher teacher1 = new Teacher() { TeacherName = "陳定宏" };
            teacher1.TeachingCourses.Add(new Course(teacher1) { CourseName = "視窗程式設計", OpeningClass = "五專三甲", Point = 3, Type = "必修" });
            teacher1.TeachingCourses.Add(new Course(teacher1) { CourseName = "視窗程式設計", OpeningClass = "四技二甲", Point = 3, Type = "選修" });
            teacher1.TeachingCourses.Add(new Course(teacher1) { CourseName = "視窗程式設計", OpeningClass = "四技二乙", Point = 3, Type = "選修" });
            teacher1.TeachingCourses.Add(new Course(teacher1) { CourseName = "視窗程式設計", OpeningClass = "四技二丙", Point = 3, Type = "選修" });

            Teacher teacher2 = new Teacher() { TeacherName = "陳福坤" };
            teacher2.TeachingCourses.Add(new Course(teacher2) { CourseName = "計算機概論", OpeningClass = "四技一丙", Point = 2, Type = "必修" });
            teacher2.TeachingCourses.Add(new Course(teacher2) { CourseName = "計算機概論", OpeningClass = "四技一甲一乙", Point = 2, Type = "必修" });
            teacher2.TeachingCourses.Add(new Course(teacher2) { CourseName = "數位系統導論", OpeningClass = "四技一乙", Point = 3, Type = "必修" });

            Teacher teacher3 = new Teacher() { TeacherName = "許子衡" };
            teacher3.TeachingCourses.Add(new Course(teacher3) { CourseName = "Android程式設計", OpeningClass = "四技資工三甲等合開", Point = 3, Type = "選修" });
            teacher3.TeachingCourses.Add(new Course(teacher3) { CourseName = "人工智慧與雲端運算", OpeningClass = "四技資工四甲等合開", Point = 3, Type = "選修" });
            teacher3.TeachingCourses.Add(new Course(teacher3) { CourseName = "動態程式語言", OpeningClass = "五專資工三甲", Point = 3, Type = "系定選修" });

            teachers.Add(teacher1);
            teachers.Add(teacher2);
            teachers.Add(teacher3);

            tvTeacher.ItemsSource = teachers;

            foreach (Teacher teacher in teachers)
            {
                foreach (Course course in teacher.TeachingCourses)
                {
                    courses.Add(course);
                }
            }
            lbCourse.ItemsSource = courses;
        }

        private void InitializeStudent()
        {
            students.Add(new Student { StudentId = "A10001", StudentName = "陳小明" });
            students.Add(new Student { StudentId = "A10002", StudentName = "王小美" });
            students.Add(new Student { StudentId = "A10003", StudentName = "林小英" });
            students.Add(new Student { StudentId = "A10004", StudentName = "黃大山" });


            cmbStudent.ItemsSource = students;
            cmbStudent.SelectedIndex = 0;
        }

        private void cmbStudent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedStudent = (Student)cmbStudent.SelectedItem;
            labelStatus.Content = $"選取學生：{selectedStudent.ToString()}";
        }

        private void tvTeacher_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvTeacher.SelectedItem is Teacher)
            {
                selectedTeacher = (Teacher)tvTeacher.SelectedItem;
                labelStatus.Content = $"選取教師：{selectedTeacher.ToString()}";
            }
            else if (tvTeacher.SelectedItem is Course)
            {
                selectedCourse = (Course)tvTeacher.SelectedItem;
                labelStatus.Content = selectedCourse.ToString();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent == null || selectedCourse == null)
            {
                MessageBox.Show("請選取學生或課程");
                return;
            }
            else
            {
                Record newRecord = new Record() { SelectedStudent = selectedStudent, SelectedCourse = selectedCourse };

                foreach (Record r in records)
                {
                    if (r.Equals(newRecord))
                    {
                        MessageBox.Show($"{selectedStudent.StudentName}已選取 {selectedCourse.CourseName}");
                        return;
                    }
                }

                records.Add(newRecord);
                lvRecord.ItemsSource = records;
                lvRecord.Items.Refresh();
            }
        }

        private void lbCourse_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedCourse = (Course)lbCourse.SelectedItem;
            labelStatus.Content = selectedCourse.ToString();
        }

        private void lvRecord_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedRecord = (Record)lvRecord.SelectedItem;
            if (selectedRecord != null) labelStatus.Content = selectedRecord.ToString();
        }

        private void btnWithdrawl_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRecord != null)
            {
                records.Remove(selectedRecord);
                lvRecord.ItemsSource = records;
                lvRecord.Items.Refresh();
            }
        }

        private void InitializeStudentFromJson(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(jsonString);

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json文件 (*.json)|*.json|All Files (*.*)|*.*";
            saveFileDialog.Title = "儲存學生選課紀錄";

            if (saveFileDialog.ShowDialog() == true)
            {
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