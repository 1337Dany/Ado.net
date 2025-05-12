using System.Data.SqlClient;
using APBD_08.dtos;
using APBD_08.models;

namespace APBD_08.repositories;

public class PotatoTeacherQuizRepository(string connectionString) : IPotatoTeacherQuizRepository
{
    private readonly string _connectionString = connectionString;

    public async Task<List<Quiz>> GetAllQuizzesAsync()
    {
        var quizzes = new List<Quiz>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT * FROM Quiz", connection);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            quizzes.Add(new Quiz
            {
                id = reader.GetInt32(0),
                name = reader.GetString(1)
            });
        }

        return quizzes;
    }

    public async Task<Quiz> GetQuizByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT * FROM Quiz WHERE id = @id", connection);

        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Quiz
            {
                id = reader.GetInt32(0),
                name = reader.GetString(1)
            };
        }

        return null;
    }

    public async Task CreateTestAsync(TestDto dto)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            // Check if teacher exists
            int teacherId;
            using (var command = new SqlCommand(
                       "SELECT Id FROM PotatoTeacher WHERE Name = @Name", 
                       connection, 
                       transaction))
            {
                command.Parameters.AddWithValue("@Name", dto.potatoTeacherName);
                var result = await command.ExecuteScalarAsync();
            
                if (result != null)
                {
                    teacherId = (int)result;
                }
                else
                {
                    // Create new teacher
                    using var insertTeacherCommand = new SqlCommand(
                        "INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)", 
                        connection, 
                        transaction);
                    insertTeacherCommand.Parameters.AddWithValue("@Name", dto.potatoTeacherName);
                    teacherId = (int)await insertTeacherCommand.ExecuteScalarAsync();
                }
            }

            // Create new quiz
            using var insertQuizCommand = new SqlCommand(
                "INSERT INTO Quiz (Name, PotatoTeacherId, PathFile) VALUES (@Name, @PotatoTeacherId, @PathFile)", 
                connection, 
                transaction);
            insertQuizCommand.Parameters.AddWithValue("@Name", dto.name);
            insertQuizCommand.Parameters.AddWithValue("@PotatoTeacherId", teacherId);
            insertQuizCommand.Parameters.AddWithValue("@PathFile", dto.path);
        
            await insertQuizCommand.ExecuteNonQueryAsync();
        
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw; // Re-throw the exception after rollback
        }
    }}