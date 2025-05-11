namespace Kurs_Tareeva_A_A_23VP2
{
    public class Course
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string DifficultyLevel { get; set; }
        public string ProgrammingLanguage { get; set; }
        public int StudentCount { get; set; }


        public Course(string courseName, string teacherName, string difficultyLevel, string programmingLanguage)
        {
            CourseName = courseName;
            TeacherName = teacherName;
            DifficultyLevel = difficultyLevel;
            ProgrammingLanguage = programmingLanguage;
            StudentCount = 0;
        }

        public Course() { }
    }

}

    