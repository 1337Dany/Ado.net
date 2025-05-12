using APBD_08.dtos;

namespace APBD_08.Services;

public interface IDbService
{
    Task<List<QuizDto>> GetAllQuizesAsync();
}