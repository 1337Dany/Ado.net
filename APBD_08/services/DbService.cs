using APBD_08.dtos;
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
}