using System;
using Domain.DTOs.Author;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController
{
    private readonly IAuthorServices _authorServices;
    public AuthorController(IAuthorServices services)
    {
        _authorServices = services;
    }
    [HttpPost]
    public async Task<Response<string>> CreateItem([FromForm] AuthorCreateDto request)
    {
        return await _authorServices.CreateItem(request);
    }
    [HttpPut("{id:int}")]
    public async Task<Response<string>> UpdateItem([FromRoute] int id, [FromForm] AuthorUpdateDto request)
    {
        return await _authorServices.UpdateItem(request, id);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteItem(int id)
    {
        return await _authorServices.DeleteItem(id);
    }
    [HttpGet("getall")]
    public async Task<Response<IEnumerable<AuthorGetDto>>> GetItems()
    {
        return await _authorServices.GetItems();
    }
    [HttpGet("{id:int}")]
    public async Task<Response<AuthorGetDto>> GetAuthorById(int id)
    {
        return await _authorServices.GetAuthorById(id);
    }



}
