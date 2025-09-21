using System;
using Domain.DTOs.Author;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IAuthorServices
{
    Task<Response<string>> CreateItem(AuthorCreateDto request);
    Task<Response<IEnumerable<AuthorGetDto>>> GetItems();
    Task<Response<string>> UpdateItem(AuthorUpdateDto request, int id);
    Task<Response<string>> DeleteItem(int id);
    Task<Response<AuthorGetDto>> GetAuthorById(int id);
}
