using APBD_08.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_08.controllers;

[ApiController]
[Route ("api/controller")]
public class Controller(IDbService dbService)
{
    private readonly IDbService _dbService = dbService;

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var result = await _dbService.GetAllQuizesAsync();
        return Results.Ok(result);
    }
}