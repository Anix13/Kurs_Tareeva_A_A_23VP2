using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            SetUIState(false); // Интерфейс заблокирован до создания базы данных
        }

        private void InitializeDataGridViews()
        {
            dataGridView1.DataSource = dbManager.Courses;
            dataGridView2.DataSource = dbManager.Students;

            // --- Заголовки для Курсов ---
            NameColumn.DataPropertyName = "CourseName";
            NameColumn.HeaderText = "Название курса";

            Column1.DataPropertyName = "TeacherName";
            Column1.HeaderText = "ФИО преподавателя";

            Column2.DataPropertyName = "DifficultyLevel";
            Column2.HeaderText = "Уровень сложности";

            Column4.DataPropertyName = "ProgrammingLanguage";
            Column4.HeaderText = "Язык программирования";

            Column3.DataPropertyName = "StudentCount";
            Column3.HeaderText = "Кол-во обучающихся";

            // --- Заголовки для Студентов ---
            dataGridViewTextBoxColumn1.DataPropertyName = "LastName";
            dataGridViewTextBoxColumn1.HeaderText = "Фамилия";

            dataGridViewTextBoxColumn2.DataPropertyName = "FirstName";
            dataGridViewTextBoxColumn2.HeaderText = "Имя";

            dataGridViewTextBoxColumn3.DataPropertyName = "MiddleName";
            dataGridViewTextBoxColumn3.HeaderText = "Отчество";

            dataGridViewTextBoxColumn4.DataPropertyName = "CourseName";
            dataGridViewTextBoxColumn4.HeaderText = "Курс";

            Column5.DataPropertyName = "AccessLevel";
            Column5.HeaderText = "Уровень доступа";
        }


        private void SetUIState(bool isEnabled)
        {
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
            comboBoxy1.Enabled = isEnabled;
            comboBoxy2.Enabled = isEnabled;
            comboBoxc1.Enabled = isEnabled;
            comboBoxc1.Enabled = isEnabled;

            // --- Сохранение и отчёт ---
            сформироватьОтчетToolStripMenuItem.Enabled = isEnabled;
            сохранитьToolStripMenuItem.Enabled = isEnabled ; 
            удалитьToolStripMenuItem.Enabled = isEnabled;

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
            dbManager.ClearData();
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

            if (!string.IsNullOrEmpty(selected5)) comboBox5.SelectedItem = selected5;
            if (!string.IsNullOrEmpty(selected7)) comboBox7.SelectedItem = selected7;
            if (!string.IsNullOrEmpty(selected6)) comboBox6.SelectedItem = selected6;
        }

        private void RefreshStudentComboBox()
        {
            string selected = comboBox8.SelectedItem?.ToString();

            comboBox8.Items.Clear();

            foreach (DataRow row in dbManager.Students.AsEnumerable())
            {
                comboBox8.Items.Add(row.Field<string>("LastName"));
            }

            if (!string.IsNullOrEmpty(selected))
                comboBox8.SelectedItem = selected;
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isExistingData = dbManager.Courses.Rows.Count > 0 || dbManager.Students.Rows.Count > 0;

            if (isExistingData)
            {
                DialogResult result = MessageBox.Show(
                    "База данных уже содержит данные. Сохранить текущую и создать новую?",
                    "Подтверждение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Cancel)
                    return;

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "XML файлы (*.xml)|*.xml";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        dbManager.SaveToXml(saveDialog.FileName);
                    }
                    else
                    {
                        return;
                    }
                }

                dbManager.ResetData(); // Пересоздаем структуру
            }

            SetUIState(true);
            ClearAllInputFields();
            RefreshCourseComboBoxes();
            RefreshStudentComboBox();
            MessageBox.Show("Новая база данных создана", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить текущую базу данных?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbManager.ResetData();
                SetUIState(false);
                ClearAllInputFields();
                RefreshCourseComboBoxes();
                RefreshStudentComboBox();
                currentFilePath = null;
                MessageBox.Show("База данных удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                .Any(r => r.Field<string>("CourseName")?.Trim().Equals(newCourseName, StringComparison.OrdinalIgnoreCase) ?? false);

            if (courseExists)
            {
                MessageBox.Show("Курс с таким названием уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var course = new Course(newCourseName, textBox2.Text, comboBox3.Text, textBox4.Text);

            course.StudentCount = 0;
            dbManager.AddCourse(course);


            textBox1.Clear();
            textBox2.Clear();
            comboBox3.SelectedIndex = -1;
            textBox4.Clear();

            RefreshCourseComboBoxes();
            dataGridView1.DataSource = dbManager.Courses;
            dataGridView1.Refresh();

            MessageBox.Show("Курс успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(comboBox7.Text) ||
                string.IsNullOrWhiteSpace(comboBox11.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var student = new Student(
                textBox8.Text,
                textBox7.Text,
                textBox6.Text,
                comboBox7.Text,
                comboBox11.Text);

            dbManager.AddStudent(student);
            dbManager.UpdateStudentCount(student.CourseName, 1);

            textBox8.Clear();
            textBox7.Clear();
            textBox6.Clear();
            comboBox7.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;

            RefreshStudentComboBox();
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = dbManager.Students;
            dataGridView2.Refresh();

            MessageBox.Show("Ученик успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            RefreshCourseComboBoxes();
            comboBox5.SelectedItem = courseName;

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
            int studentCount = dbManager.Students.AsEnumerable()
                .Count(row => row.Field<string>("CourseName") == courseName);

            string message = $"Вы действительно хотите удалить курс '{courseName}'?";
            if (studentCount > 0)
            {
                message += $"\n\nНа этом курсе обучается {studentCount} студент(ов). Удаление курса приведёт к удалению всех студентов этого курса.";
            }

            if (MessageBox.Show(message, "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dbManager.DeleteStudentsByCourse(courseName);
                dbManager.DeleteCourse(courseName);

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
            comboBox8.SelectedItem = oldName;
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

        private void comboBox5_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox7_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox8_DropDown(object sender, EventArgs e)
        {
            RefreshStudentComboBox();
        }

        private void comboBox6_DropDown(object sender, EventArgs e)
        {
            RefreshCourseComboBoxes();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortCourses();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortCourses();
        }

        private void SortCourses()
        {
            if (comboBoxc1.SelectedIndex == -1 || comboBoxc2.SelectedIndex == -1)
                return;

            string field = "";
            switch (comboBoxc2.SelectedItem.ToString())
            {
                case "Название курса": field = "CourseName"; break;
                case "ФИО преподавателя": field = "TeacherName"; break;
                case "Уровень сложности": field = "DifficultyLevel"; break;
                case "Язык программирования": field = "ProgrammingLanguage"; break;
                case "Кол-во обучающихся": field = "StudentCount"; break;
                default:
                    return;
            }

            string direction = comboBoxc1.SelectedItem.ToString() == "возрастанию" ? "ASC" : "DESC";
            dbManager.Courses.DefaultView.Sort = $"{field} {direction}";
            dataGridView1.DataSource = dbManager.Courses.DefaultView;
        }

        private void SortStudents()
        {
            if (comboBoxy2.SelectedIndex == -1 || comboBoxy1.SelectedIndex == -1)
                return;

            string field = "";
            switch (comboBoxy2.SelectedItem.ToString())
            {
                case "Фамилия": field = "LastName"; break;
                case "Имя": field = "FirstName"; break;
                case "Отчество": field = "MiddleName"; break;
                case "Курс": field = "CourseName"; break;
                case "Уровень доступа": field = "AccessLevel"; break;
                default:
                    return;
            }

            string direction = comboBoxy1.SelectedItem.ToString() == "возрастанию" ? "ASC" : "DESC";
            dbManager.Students.DefaultView.Sort = $"{field} {direction}";
            dataGridView2.DataSource = dbManager.Students.DefaultView;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortStudents();
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortStudents();
        }



        private void сформироватьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbManager.Courses.Rows.Count == 0 || dbManager.Students.Rows.Count == 0)
            {
                MessageBox.Show("Невозможно сформировать отчёт: база данных пустая", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF файлы (*.pdf)|*.pdf";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveDialog.FileName;

                if (!IsPathValid(filePath))
                    return;

                GeneratePdfReportWithTwoTables(filePath);
            }

        }

        private bool IsPathValid(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            char[] invalidChars = Path.GetInvalidPathChars();
            if (filePath.IndexOfAny(invalidChars) >= 0)
            {
                MessageBox.Show("Путь содержит недопустимые символы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (FileStream fs = File.Create(filePath, 1, FileOptions.DeleteOnClose)) { }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка доступа к файлу: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void GeneratePdfReportWithTwoTables(string filePath)
        {
            try
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Шрифт для кириллицы
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);

                // --- Таблица "Курсы" ---
                PdfPTable courseTable = new PdfPTable(dataGridView1.Columns.Count);
                courseTable.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell(new Phrase("Таблица: Курсы", font))
                {
                    Colspan = dataGridView1.Columns.Count,
                    HorizontalAlignment = 1,
                    Border = 0
                };
                courseTable.AddCell(cell);

                // Заголовки из HeaderText
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(dataGridView1.Columns[i].HeaderText, font))
                    {
                        BackgroundColor = new BaseColor(211, 211, 211)
                    };
                    courseTable.AddCell(headerCell);
                }

                // Данные
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cellItem in row.Cells)
                    {
                        if (cellItem.Value != null)
                            courseTable.AddCell(new Phrase(cellItem.Value.ToString(), font));
                    }
                }

                doc.Add(courseTable);

                // --- Таблица "Студенты" ---
                PdfPTable studentTable = new PdfPTable(dataGridView2.Columns.Count);
                studentTable.WidthPercentage = 100;

                cell = new PdfPCell(new Phrase("Таблица: Студенты", font))
                {
                    Colspan = dataGridView2.Columns.Count,
                    HorizontalAlignment = 1,
                    Border = 0
                };
                studentTable.AddCell(cell);

                // Заголовки из HeaderText
                for (int i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(dataGridView2.Columns[i].HeaderText, font))
                    {
                        BackgroundColor = new BaseColor(211, 211, 211)
                    };
                    studentTable.AddCell(headerCell);
                }

                // Данные
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    foreach (DataGridViewCell cellItem in row.Cells)
                    {
                        if (cellItem.Value != null)
                            studentTable.AddCell(new Phrase(cellItem.Value.ToString(), font));
                    }
                }

                doc.Add(studentTable);
                doc.Close();

                MessageBox.Show("PDF-отчет успешно создан", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчёта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxc1.SelectedIndex = 0;

            comboBoxc2.Items.AddRange(new string[] {
        "Название курса",
        "ФИО преподавателя",
        "Уровень сложности",
        "Язык программирования",
        "Кол-во обучающихся"
    });
            comboBoxc2.SelectedIndex = 0;

            comboBoxy1.SelectedIndex = 0;
            comboBoxy2.Items.AddRange(new string[] {
                "Фамилия",
                "Имя",
                "Отчество",
                "Название курса",
                "Уровень доступа"
            });
            comboBoxy2.SelectedIndex = 0;

            comboBox3.Items.AddRange(new string[] { "junior", "middle", "senior" });
            comboBox4.Items.AddRange(new string[] { "junior", "middle", "senior" });

            comboBox11.Items.AddRange(new string[] { "Начальный", "Полный" });
            comboBox12.Items.AddRange(new string[] { "Начальный", "Полный" });

            создатьToolStripMenuItem.Click += создатьToolStripMenuItem_Click;
            удалитьToolStripMenuItem.Click += удалитьToolStripMenuItem_Click;
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            сформироватьОтчетToolStripMenuItem.Click += сформироватьОтчетToolStripMenuItem_Click;

            buttonSort1.Click += buttonSort1_Click;
            buttonSort2.Click += buttonSort2_Click;
        }
        private bool HasUnsavedChanges()
        {
            return dbManager.Courses.Rows.Count > 0 || dbManager.Students.Rows.Count > 0;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSort1_Click(object sender, EventArgs e)
        {
            SortCourses();
            MessageBox.Show("Сортировка успешно выполнена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSort2_Click(object sender, EventArgs e)
        {
            SortStudents();
            MessageBox.Show("Сортировка студентов выполнена успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML файлы (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    dbManager.LoadFromXml(filePath);
                    currentFilePath = filePath;

                    SetUIState(true); // Включаем интерфейс
                    ClearAllInputFields(); // Очищаем поля ввода
                    RefreshCourseComboBoxes(); // Обновляем комбобоксы с курсами
                    RefreshStudentComboBox(); // Обновляем комбобоксы со студентами

                    // Привязываем обновлённые данные к DataGridView
                    dataGridView1.DataSource = dbManager.Courses;
                    dataGridView2.DataSource = dbManager.Students;

                    MessageBox.Show("База данных успешно загружена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}