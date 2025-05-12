using APBD_08.dtos;
using APBD_08.models;
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
    public async Task<IResult> GetById(int id)
    {
        var result = await _dbService.GetQuizByIdAsync(id);
        return result == null ? Results.NotFound() : Results.Ok(result);
    }

    [HttpPost("/{id}")]
    public async Task<IResult> Save(TestDto dto)
    {
        if (dto == null)
        {
            return Results.BadRequest("Invalid data.");
        }

        try
        {
            await _dbService.CreatePotatoTeacherAsync(dto);
            return Results.Ok("Quiz and Potato Teacher have been successfully created.");
        }
        catch (Exception ex)
        {
            return Results.StatusCode(500);
        }
    }
}