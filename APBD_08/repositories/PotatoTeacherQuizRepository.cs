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
}