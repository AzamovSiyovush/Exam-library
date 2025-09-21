using System;
using System.Collections;
using Domain.DTOs.Book;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IBookServices
{
    Task<Response<string>> CreateItem(BookCreateDto request);
    Task<Response<IEnumerable<BookGetDto>>> GetItems();
    Task<Response<string>> UpdateItem(BookUpdateDto request, int id);
    Task<Response<string>> DeleteItem(int id);
    Task<Response<IEnumerable<BookGetDto>>> GetBooksByAuthor(int authorId);
    Task<Response<IEnumerable<BookGetDto>>> GetBooksByGenre(string genre);
    Task<Response<IEnumerable<BookGetDto>>> GetRecentlyPublishedBooks(int years);
}
