namespace StudentWebService.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public int GrupID { get; set;}

        public int OtrasID { get; set; }

        public required string Nume { get; set; }

        public required string Prenume { get; set; }
    }
}
