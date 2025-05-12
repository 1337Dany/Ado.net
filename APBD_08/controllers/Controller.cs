using APBD_08.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_08.controllers;

[ApiController]
[Route ("api/[controller]")]
public class Controller(IDbService dbService)
{
    private readonly IDbService _dbService = dbService;

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var result = await _dbService.GetAllQuizesAsync();
        return Results.Ok(result);
    }

    [HttpGet("/{id}")]
    //[Route ("api/[controller]/{id}")]
    public async Task<IResult> GetById(int id)
    {
        var result = await _dbService.GetQuizByIdAsync(id);
        return result == null ? Results.NotFound() : Results.Ok(result);
    }
}