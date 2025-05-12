using APBD_08.dtos;
using APBD_08.models;
using APBD_08.repositories;

namespace APBD_08.Services;

public class DbService(IPotatoTeacherQuizRepository repo) : IDbService
{
    public async Task<List<QuizDto>> GetAllQuizesAsync()
    {
        var quizzes = await repo.GetAllQuizzesAsync();

        var dtoList = quizzes.Select(q => new QuizDto
        {
            id = q.id,
            name = q.name
        }).ToList();

        return dtoList;
    }

    public async Task<QuizDto> GetQuizByIdAsync(int id)
    {
        var quiz = await repo.GetQuizByIdAsync(id);

        var dto = new QuizDto
        {
            id = quiz.id,
            name = quiz.name
        };

        return await Task.FromResult(dto);
    }

    public async Task CreatePotatoTeacherAsync(TestDto dto)
    {
        await repo.CreateTestAsync(dto);

    }
}