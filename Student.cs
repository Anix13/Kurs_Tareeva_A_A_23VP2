namespace Kurs_Tareeva_A_A_23VP2
{
    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string CourseName { get; set; }
        public string AccessLevel { get; set; }

        public Student(string lastName, string firstName, string middleName, string courseName, string accessLevel)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            CourseName = courseName;
            AccessLevel = accessLevel;
        }

        public Student() { }
    }
}