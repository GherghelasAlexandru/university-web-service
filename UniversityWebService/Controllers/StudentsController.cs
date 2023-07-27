using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace StudentWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(BasicAuthentication))]

    public class StudentsController : Controller
    {
        private readonly string connectionString = "Server=DESKTOP-ALEX;Database=Universitate;Trusted_Connection=true;Encrypt=False";

        [HttpGet("{id}/Courses and grades")]
        public IActionResult GetMateriiSiNote(int id)
        {
            try
            {
                string query = "SELECT CONCAT(Student.Nume, ' ', Student.Prenume) AS [Nume], Materie.Nume AS [Materie], Note.Nota AS [Nota] FROM Note JOIN Materie ON Note.MaterieID = Materie.MaterieID JOIN Student ON Note.StudentID = Student.StudentID WHERE Student.StudentID = @StudentID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        var note = new List<object>();

                        while (reader.Read())
                        {
                            var nota = new
                            {
                                NumeStudent = reader["Nume"],
                                NumeMaterie = reader["Materie"],
                                NotaObtinuta = reader["Nota"]
                            };

                            note.Add(nota);
                        }
                        return new JsonResult(note);
                    }
                    else
                    {
                        return NotFound("Student not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}/Final Grade")]
        public IActionResult GetMediaGenerala(int id)
        {
            try
            {
                string query = "SELECT CAST(AVG(CAST(Nota AS DECIMAL(10, 2))) AS DECIMAL(10, 2)) AS [MediaGenerala] FROM (SELECT MAX(Nota) AS Nota FROM Note WHERE StudentID = @StudentID GROUP BY MaterieID) AS UltimaNota";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var mediaGenerala = reader["MediaGenerala"];

                        if (mediaGenerala != DBNull.Value)
                        {
                            var result = new
                            {
                                MediaGenerala = mediaGenerala
                            };

                            return new JsonResult(result);
                        }
                        else
                        {
                            return NotFound("Final grade not found");
                        }
                    }
                    else
                    {
                        return NotFound("Student not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("Return all student")]
        public IActionResult GetAllStudents()
        {
            try
            {
                string query = "SELECT StudentID, Nume, Prenume FROM Student";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    var students = new List<object>();

                    while (reader.Read())
                    {
                        var student = new
                        {
                            StudentId = reader["StudentID"],
                            FirstName = reader["Nume"],
                            LastName = reader["Prenume"]
                        };

                        students.Add(student);
                    }

                    return new JsonResult(students);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}/Return a student by ID")]
        public IActionResult GetStudent(int id)
        {
            try
            {
                string query = "SELECT Nume, Prenume FROM Student WHERE StudentID = @StudentID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        var firstName = reader["Nume"].ToString();
                        var lastName = reader["Prenume"].ToString();

                        var student = new
                        {
                            StudentId = id,
                            FirstName = firstName,
                            LastName = lastName
                        };

                        return new JsonResult(student);
                    }
                    else
                    {
                        return NotFound("Student not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
