using System;
using System.Collections;
using Domain.DTOs.Author;
using Domain.DTOs.Book;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
[Authorize]
public class BookController
{
    private readonly IBookServices _bookservice;
    public BookController(IBookServices bookServices)
    {
        _bookservice = bookServices;
    }
    [HttpPost]
    public async Task<Response<string>> CreateItem([FromForm] BookCreateDto request)
    {
        return await _bookservice.CreateItem(request);
    }
    [HttpPut("{id:int}")]
    public async Task<Response<string>> UpdateItem([FromForm] BookUpdateDto request, [FromRoute] int id)
    {
        return await _bookservice.UpdateItem(request, id);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteItem(int id)
    {
        return await _bookservice.DeleteItem(id);
    }
    [HttpGet("getall")]
    public async Task<Response<IEnumerable<BookGetDto>>> GetItems()
    {
        return await _bookservice.GetItems();
    }
    [HttpGet("{authorId:int}")]
    public async Task<Response<IEnumerable<BookGetDto>>> GetBooksByAuthor(int authorId)
    {
        return await _bookservice.GetBooksByAuthor(authorId);
    }
    [HttpGet("{genre}")]
    public async Task<Response<IEnumerable<BookGetDto>>> GetBooksByGenre(string genre)
    {
        return await _bookservice.GetBooksByGenre(genre);
    }
    [HttpGet("years{years:int}")]
    public async Task<Response<IEnumerable<BookGetDto>>> GetRecentlyPublishedBooks(int years)
    {
        return await _bookservice.GetRecentlyPublishedBooks(years);
    }

}
