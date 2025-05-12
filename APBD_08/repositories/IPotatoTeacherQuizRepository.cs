using APBD_08.dtos;
using APBD_08.models;

namespace APBD_08.repositories;

public interface IPotatoTeacherQuizRepository
{
    Task<List<Quiz>> GetAllQuizzesAsync();
}