using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Kurs_Tareeva_A_A_23VP2
{
    public class DatabaseManager
    {
        private DataSet dataSet = new DataSet("ProgrammingCourses");
        private DataTable coursesTable;
        private DataTable studentsTable;

        public DataTable Courses => coursesTable;
        public DataTable Students => studentsTable;

        public DatabaseManager()
        {
            InitializeTables();
        }

        private void InitializeTables()
        {
            // Создаем таблицу "Courses", если её нет
            if (!dataSet.Tables.Contains("Courses"))
            {
                coursesTable = new DataTable("Courses");
                coursesTable.Columns.Add("CourseName", typeof(string));
                coursesTable.Columns.Add("TeacherName", typeof(string));
                coursesTable.Columns.Add("DifficultyLevel", typeof(string));
                coursesTable.Columns.Add("ProgrammingLanguage", typeof(string));
                coursesTable.Columns.Add("StudentCount", typeof(int));
                dataSet.Tables.Add(coursesTable);
            }
            else
            {
                coursesTable = dataSet.Tables["Courses"];
            }

            // Создаем таблицу "Students", если её нет
            if (!dataSet.Tables.Contains("Students"))
            {
                studentsTable = new DataTable("Students");
                studentsTable.Columns.Add("LastName", typeof(string));
                studentsTable.Columns.Add("FirstName", typeof(string));
                studentsTable.Columns.Add("MiddleName", typeof(string));
                studentsTable.Columns.Add("CourseName", typeof(string));
                studentsTable.Columns.Add("AccessLevel", typeof(string));
                dataSet.Tables.Add(studentsTable);
            }
            else
            {
                studentsTable = dataSet.Tables["Students"];
            }
        }

        /// <summary>
        /// Очищает данные, но оставляет структуру таблиц
        /// </summary>
        public void ClearData()
        {
            dataSet.Clear();
        }

        /// <summary>
        /// Полностью пересоздаёт таблицы (полезно при создании новой БД)
        /// </summary>
        public void ResetData()
        {
            if (dataSet.Tables.Contains("Students"))
                dataSet.Tables.Remove("Students");

            if (dataSet.Tables.Contains("Courses"))
                dataSet.Tables.Remove("Courses");

            InitializeTables();
        }

        /// <summary>
        /// Сохраняет данные в XML
        /// </summary>
        public void SaveToXml(string filePath)
        {
            try
            {
                dataSet.WriteXml(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает данные из XML
        /// </summary>
        public void LoadFromXml(string filePath)
        {
            try
            {
                dataSet.ReadXml(filePath);
                coursesTable = dataSet.Tables["Courses"];
                studentsTable = dataSet.Tables["Students"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Проверяет, существует ли курс с указанным названием
        /// </summary>
        public bool CourseExists(string courseName)
        {
            return coursesTable.AsEnumerable()
                .Any(r => r.Field<string>("CourseName")?.Trim().Equals(courseName.Trim(), StringComparison.OrdinalIgnoreCase) ?? false);
        }

        /// <summary>
        /// Добавляет новый курс
        /// </summary>
        public void AddCourse(Course course)
        {
            DataRow row = coursesTable.NewRow();
            row["CourseName"] = course.CourseName;
            row["TeacherName"] = course.TeacherName;
            row["DifficultyLevel"] = course.DifficultyLevel;
            row["ProgrammingLanguage"] = course.ProgrammingLanguage;
            row["StudentCount"] = course.StudentCount;
            coursesTable.Rows.Add(row);
        }

        /// <summary>
        /// Добавляет нового студента
        /// </summary>
        public void AddStudent(Student student)
        {
            if (!CourseExists(student.CourseName))
            {
                throw new Exception("Курс не найден. Невозможно добавить студента.");
            }

            DataRow row = studentsTable.NewRow();
            row["LastName"] = student.LastName;
            row["FirstName"] = student.FirstName;
            row["MiddleName"] = student.MiddleName;
            row["CourseName"] = student.CourseName;
            row["AccessLevel"] = student.AccessLevel;
            studentsTable.Rows.Add(row);
        }

        /// <summary>
        /// Обновляет количество студентов на курсе
        /// </summary>
        public void UpdateStudentCount(string courseName, int change)
        {
            foreach (DataRow row in coursesTable.Rows)
            {
                if (row["CourseName"].ToString() == courseName)
                {
                    int currentCount = Convert.ToInt32(row["StudentCount"]);
                    row["StudentCount"] = currentCount + change;
                    break;
                }
            }
        }

        /// <summary>
        /// Удаляет курс по названию
        /// </summary>
        public void DeleteCourse(string courseName)
        {
            var rows = coursesTable.Select($"CourseName = '{courseName.Replace("'", "''")}'");
            if (rows.Length > 0)
            {
                coursesTable.Rows.Remove(rows[0]);
            }
        }


        /// <summary>
        /// Удаляет всех студентов, связанных с курсом
        /// </summary>
        public void DeleteStudentsByCourse(string courseName)
        {
            var rows = studentsTable.Select($"CourseName = '{courseName}'");
            foreach (var row in rows)
            {
                studentsTable.Rows.Remove(row);
            }
        }

        /// <summary>
        /// Обновляет информацию о курсе
        /// </summary>
        public void UpdateCourse(string courseName, string teacherName, string difficultyLevel, string language)
        {
            var rows = coursesTable.Select($"CourseName = '{courseName}'");
            if (rows.Length > 0)
            {
                var row = rows[0];
                row["TeacherName"] = teacherName;
                row["DifficultyLevel"] = difficultyLevel;
                row["ProgrammingLanguage"] = language;
            }
        }
    }
}