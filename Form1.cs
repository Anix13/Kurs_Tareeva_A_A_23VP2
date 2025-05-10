using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Kurs_Tareeva_A_A_23VP2
{
    public partial class Form1 : Form
    {
        private DatabaseManager dbManager = new DatabaseManager();
        private string currentFilePath = null;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridViews();
            SetUIState(false); // По умолчанию всё заблокировано
        }

        private void InitializeDataGridViews()
        {
            dataGridView1.DataSource = dbManager.Courses;
            dataGridView2.DataSource = dbManager.Students;

            NameColumn.DataPropertyName = "CourseName";
            Column1.DataPropertyName = "TeacherName";
            Column2.DataPropertyName = "DifficultyLevel";
            Column4.DataPropertyName = "ProgrammingLanguage";
            Column3.DataPropertyName = "StudentCount";

            dataGridViewTextBoxColumn1.DataPropertyName = "LastName";
            dataGridViewTextBoxColumn2.DataPropertyName = "FirstName";
            dataGridViewTextBoxColumn3.DataPropertyName = "MiddleName";
            dataGridViewTextBoxColumn4.DataPropertyName = "CourseName";
            Column5.DataPropertyName = "AccessLevel";
        }

        private void SetUIState(bool isEnabled)
        {
            if (!isEnabled)
            {
                ClearAllInputFields();
                ClearAllTables();
            }

            // --- Курсы ---
            textBox1.Enabled = isEnabled;
            textBox2.Enabled = isEnabled;
            comboBox3.Enabled = isEnabled;
            textBox4.Enabled = isEnabled;
            button1.Enabled = isEnabled;

            // --- Студенты ---
            textBox8.Enabled = isEnabled;
            textBox7.Enabled = isEnabled;
            textBox6.Enabled = isEnabled;
            comboBox7.Enabled = isEnabled;
            comboBox11.Enabled = isEnabled;
            button4.Enabled = isEnabled;

            // --- Редактирование курсов ---
            comboBox5.Enabled = isEnabled;
            textBox5.Enabled = isEnabled;
            comboBox4.Enabled = isEnabled;
            textBox3.Enabled = isEnabled;
            button2.Enabled = isEnabled;
            button3.Enabled = isEnabled;

            // --- Редактирование студентов ---
            comboBox8.Enabled = isEnabled;
            textBox10.Enabled = isEnabled;
            textBox9.Enabled = isEnabled;
            comboBox6.Enabled = isEnabled;
            comboBox12.Enabled = isEnabled;
            button5.Enabled = isEnabled;
            button6.Enabled = isEnabled;

            // --- Сортировка ---
            comboBox1.Enabled = isEnabled;
            comboBox2.Enabled = isEnabled;
            comboBox9.Enabled = isEnabled;
            comboBox10.Enabled = isEnabled;

            // --- Сохранение ---
            сохранитьToolStripMenuItem.Enabled = isEnabled;

            // --- Видимость таблиц ---
            dataGridView1.Visible = isEnabled;
            dataGridView2.Visible = isEnabled;
            label14.Visible = isEnabled; // Заголовок "Курсы"
            label15.Visible = isEnabled; // Заголовок "Студенты"
        }

        private void ClearAllInputFields()
        {
            textBox1.Clear(); textBox2.Clear(); comboBox3.SelectedIndex = -1; textBox4.Clear();
            textBox5.Clear(); comboBox4.SelectedIndex = -1; textBox3.Clear(); comboBox5.SelectedIndex = -1;
            textBox8.Clear(); textBox7.Clear(); textBox6.Clear(); comboBox7.SelectedIndex = -1; comboBox11.SelectedIndex = -1;
            textBox10.Clear(); textBox9.Clear(); comboBox6.SelectedIndex = -1; comboBox12.SelectedIndex = -1; comboBox8.SelectedIndex = -1;
        }

        private void ClearAllTables()
        {
            dbManager.Courses.Clear();
            dbManager.Students.Clear();
        }

        private void RefreshCourseComboBoxes()
        {
            string selected5 = comboBox5.SelectedItem?.ToString();
            string selected7 = comboBox7.SelectedItem?.ToString();
            string selected6 = comboBox6.SelectedItem?.ToString();

            comboBox5.Items.Clear();
            comboBox7.Items.Clear();
            comboBox6.Items.Clear();

            foreach (DataRow row in dbManager.Courses.Rows)
            {
                string courseName = row["CourseName"].ToString();
                comboBox5.Items.Add(courseName);
                comboBox7.Items.Add(courseName);
                comboBox6.Items.Add(courseName);
            }

            if (!string.IsNullOrEmpty(selected5))
                comboBox5.SelectedItem = selected5;
            if (!string.IsNullOrEmpty(selected7))
                comboBox7.SelectedItem = selected7;
            if (!string.IsNullOrEmpty(selected6))
                comboBox6.SelectedItem = selected6;
        }

        private void RefreshStudentComboBox()
        {
            string selected = comboBox8.SelectedItem?.ToString();

            comboBox8.Items.Clear();

            foreach (DataRow row in dbManager.Students.Rows)
            {
                comboBox8.Items.Add(row["LastName"]);
            }

            if (!string.IsNullOrEmpty(selected))
                comboBox8.SelectedItem = selected;
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dbManager.ClearData();
            SetUIState(true);
            RefreshCourseComboBoxes();
            RefreshStudentComboBox();
            MessageBox.Show("Новая база данных создана", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить текущую базу данных?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbManager.ClearData();
                SetUIState(false);
                currentFilePath = null;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFilePath == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML files (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }
            try
            {
                dbManager.SaveToXml(currentFilePath);
                MessageBox.Show("База данных успешно сохранена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(comboBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newCourseName = textBox1.Text.Trim();

            bool courseExists = dbManager.Courses.AsEnumerable()
                .Any(row => row.Field<string>("CourseName")?.Trim().Equals(newCourseName, StringComparison.OrdinalIgnoreCase) ?? false);

            if (courseExists)
            {
                MessageBox.Show("Курс с таким названием уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var course = new Course(newCourseName, textBox2.Text, comboBox3.Text, textBox4.Text);
            dbManager.AddCourse(course);

            textBox1.Clear();
            textBox2.Clear();
            comboBox3.SelectedIndex = -1;
            textBox4.Clear();

            RefreshCourseComboBoxes();
            MessageBox.Show("Курс успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите курс для изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string courseName = comboBox5.SelectedItem.ToString();
            string teacher = textBox5.Text;
            string difficulty = comboBox4.Text;
            string language = textBox3.Text;

            dbManager.UpdateCourse(courseName, teacher, difficulty, language);

            MessageBox.Show("Курс успешно изменён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите курс для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string courseName = comboBox5.SelectedItem.ToString();

            // Получаем количество студентов на курсе
            int studentCount = dbManager.Students.AsEnumerable()
                .Count(row => row.Field<string>("CourseName") == courseName);

            // Сообщаем пользователю о последствиях
            string message = $"Вы действительно хотите удалить курс '{courseName}'?";
            if (studentCount > 0)
            {
                message += $"\n\nПРЕДУПРЕЖДЕНИЕ: На этом курсе обучается {studentCount} студент(ов). Удаление курса приведёт к удалению всех студентов этого курса.";
            }

            DialogResult result = MessageBox.Show(message, "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                // Удаляем студентов и сам курс
                dbManager.DeleteStudentsByCourse(courseName);
                dbManager.DeleteCourse(courseName);

                // Обновляем комбобоксы и таблицы
                RefreshCourseComboBoxes();
                RefreshStudentComboBox();
                dataGridView1.DataSource = dbManager.Courses;
                dataGridView2.DataSource = dbManager.Students;

                MessageBox.Show("Курс и связанные студенты удалены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите ученика для изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldName = comboBox8.SelectedItem.ToString();
            var row = dbManager.Students.AsEnumerable().FirstOrDefault(r => r.Field<string>("LastName") == oldName);
            if (row == null) return;

            string oldCourse = row["CourseName"].ToString();
            row["FirstName"] = textBox10.Text;
            row["MiddleName"] = textBox9.Text;
            string newCourse = comboBox6.Text;
            row["CourseName"] = newCourse;
            row["AccessLevel"] = comboBox12.Text;

            if (oldCourse != newCourse)
            {
                dbManager.UpdateStudentCount(oldCourse, -1);
                dbManager.UpdateStudentCount(newCourse, 1);
            }

            RefreshStudentComboBox();
            MessageBox.Show("Данные ученика обновлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите ученика для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string lastName = comboBox8.SelectedItem.ToString();
            var row = dbManager.Students.AsEnumerable().FirstOrDefault(r => r.Field<string>("LastName") == lastName);
            if (row == null) return;

            string courseName = row["CourseName"].ToString();

            if (MessageBox.Show($"Удалить ученика '{lastName}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbManager.Students.Rows.Remove(row);
                dbManager.UpdateStudentCount(courseName, -1);
                RefreshStudentComboBox();
                MessageBox.Show("Ученик удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox5_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == -1) return;
            string selectedCourse = comboBox5.SelectedItem.ToString();
            var row = dbManager.Courses.AsEnumerable().FirstOrDefault(r => r.Field<string>("CourseName") == selectedCourse);
            if (row != null)
            {
                textBox5.Text = row["TeacherName"].ToString();
                comboBox4.Text = row["DifficultyLevel"].ToString();
                textBox3.Text = row["ProgrammingLanguage"].ToString();
            }
        }

        private void comboBox7_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox8_DropDown(object sender, EventArgs e)
        {
            RefreshStudentComboBox();
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex == -1) return;
            string lastName = comboBox8.SelectedItem.ToString();
            var row = dbManager.Students.AsEnumerable().FirstOrDefault(r => r.Field<string>("LastName") == lastName);
            if (row != null)
            {
                textBox10.Text = row["FirstName"].ToString();
                textBox9.Text = row["MiddleName"].ToString();
                comboBox6.Text = row["CourseName"].ToString();
                comboBox12.Text = row["AccessLevel"].ToString();
            }
        }

        private void comboBox6_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortCourses();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortCourses();
        }

        private void SortCourses()
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1) return;
            string direction = comboBox1.SelectedItem.ToString() == "возрастанию" ? "ASC" : "DESC";
            string field = "";
            switch (comboBox2.SelectedItem.ToString())
            {
                case "Название курса": field = "CourseName"; break;
                case "ФИО преподавателя": field = "TeacherName"; break;
                case "Уровень сложности": field = "DifficultyLevel"; break;
                case "Язык программирования": field = "ProgrammingLanguage"; break;
                case "Кол-во обучающихся": field = "StudentCount"; break;
            }
            dbManager.Courses.DefaultView.Sort = $"{field} {direction}";
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortStudents();
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortStudents();
        }

        private void SortStudents()
        {
            if (comboBox9.SelectedIndex == -1 || comboBox10.SelectedIndex == -1) return;
            string direction = comboBox10.SelectedItem.ToString() == "возрастанию" ? "ASC" : "DESC";
            string field = "";
            switch (comboBox9.SelectedItem.ToString())
            {
                case "Фамилия": field = "LastName"; break;
                case "Имя": field = "FirstName"; break;
                case "Отчество": field = "MiddleName"; break;
                case "Название курса": field = "CourseName"; break;
                case "Уровень доступа": field = "AccessLevel"; break;
            }
            dbManager.Students.DefaultView.Sort = $"{field} {direction}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.AddRange(new string[] {
                "Название курса",
                "ФИО преподавателя",
                "Уровень сложности",
                "Язык программирования",
                "Кол-во обучающихся"
            });
            comboBox2.SelectedIndex = 0;

            comboBox9.Items.AddRange(new string[] {
                "Фамилия",
                "Имя",
                "Отчество",
                "Название курса",
                "Уровень доступа"
            });
            comboBox9.SelectedIndex = 0;

            comboBox3.Items.AddRange(new string[] { "junior", "middle", "senior" });
            comboBox4.Items.AddRange(new string[] { "junior", "middle", "senior" });

            comboBox11.Items.AddRange(new string[] { "Начальный", "Полный" });
            comboBox12.Items.AddRange(new string[] { "Начальный", "Полный" });

            создатьToolStripMenuItem.Click += создатьToolStripMenuItem_Click;
            удалитьToolStripMenuItem.Click += удалитьToolStripMenuItem_Click;
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Проверка полей
            if (string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                comboBox7.SelectedItem == null ||
                comboBox11.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Создание объекта Student
                var student = new Student(
                    textBox8.Text.Trim(),
                    textBox7.Text.Trim(),
                    textBox6.Text.Trim(),
                    comboBox7.Text,
                    comboBox11.Text
                );

                // Добавление через DatabaseManager
                dbManager.AddStudent(student);

                // Обновление количества студентов на курсе
                dbManager.UpdateStudentCount(student.CourseName, 1);

                // Очистка полей
                textBox8.Clear();
                textBox7.Clear();
                textBox6.Clear();
                comboBox7.SelectedIndex = -1;
                comboBox11.SelectedIndex = -1;

                // Обновление интерфейса
                RefreshStudentComboBox(); // Обновляем comboBox8
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dbManager.Students; // Привязываем заново, чтобы обновить отображение
                dataGridView2.Refresh();

                MessageBox.Show("Ученик успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении ученика: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}