using System;
using Domain.DTOs.Book;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookService : IBookServices
{
    private readonly DataContext _context;
    public BookService(DataContext context)
    {
        _context = context;
    }
    public async Task<Response<string>> CreateItem(BookCreateDto request)
    {
        try
        {           // Validations
            if (string.IsNullOrWhiteSpace(request.Genre))
                return Response<string>.Fail(400, "Genre is required");
            if (request.Genre.Length < 3)
                return Response<string>.Fail(400, "Genre must be at least 3 characters long");

            if (request.AuthorId == 0)
                return Response<string>.Fail(400, "AuthorId is required");

            if (string.IsNullOrWhiteSpace(request.Title))
                return Response<string>.Fail(400, "Title is required");

            var newBook = new Book()
            {
                Title = request.Title,
                Genre = request.Genre,
                PublishedDate = request.PublishedDate,
                AuthorId = request.AuthorId
            };
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Book created successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
        
    }

    public async Task<Response<string>> DeleteItem(int id)
    {
        try
        {
            var item = await _context.Books.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Book not found");
            _context.Books.Remove(item);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Book deleted successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<IEnumerable<BookGetDto>>> GetBooksByAuthor(int authorId)
    {
        try
        {
            if(authorId == 0)
                return Response<IEnumerable<BookGetDto>>.Fail(400, "AuthorId is required");
            var find = await _context.Books.AnyAsync(b => b.AuthorId == authorId);
            if(!find)
                return Response<IEnumerable<BookGetDto>>.Fail(404, $"No books found for your given authorId: '{authorId}'");
            var item = await _context.Books
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookGetDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedDate = b.PublishedDate,
                    AuthorId = b.AuthorId,
                }).ToListAsync();
            return Response<IEnumerable<BookGetDto>>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<BookGetDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<IEnumerable<BookGetDto>>> GetBooksByGenre(string genre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(genre))
                return Response<IEnumerable<BookGetDto>>.Fail(400, "Genre is required");
            var find = await _context.Books.AnyAsync(b => b.Genre == genre);
            if(!find)
            return Response<IEnumerable<BookGetDto>>.Fail(404, $"No books found for your given genre: '{genre}'");
            var item = await _context.Books
            .Where(b => b.Genre.ToLower() == genre.ToLower())
            .Select(b => new BookGetDto
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                PublishedDate = b.PublishedDate,
                AuthorId = b.AuthorId,
            }).ToListAsync();
            return Response<IEnumerable<BookGetDto>>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<BookGetDto>>.Fail(500, "Internal server error:");
        }   
    }

    public async Task<Response<IEnumerable<BookGetDto>>> GetItems()
    {
        var books = await _context.Books
             .Select(b => new BookGetDto
             {
                 Id = b.Id,
                 Title = b.Title,
                 Genre = b.Genre,
                 PublishedDate = b.PublishedDate,
                 AuthorId = b.AuthorId,
             }).ToListAsync();

        return Response<IEnumerable<BookGetDto>>.Success(books, "Success");
    }

    public async Task<Response<IEnumerable<BookGetDto>>> GetRecentlyPublishedBooks(int years)
    {
        try
        {
            if (years < 1)
                return Response<IEnumerable<BookGetDto>>.Fail(400, "Invalid input");
            var currentDate = DateTime.Now;
            var minus = currentDate.AddYears(-years);
            var item = await _context.Books
            .Where(b => b.PublishedDate >= minus)
            .Select(b => new BookGetDto
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                PublishedDate = b.PublishedDate,
                AuthorId = b.AuthorId,
            }).ToListAsync();
            return Response<IEnumerable<BookGetDto>>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<BookGetDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<string>> UpdateItem(BookUpdateDto request, int id)
    {
        try
        {
            var item = await _context.Books.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Book not found");
            if (string.IsNullOrWhiteSpace(request.Genre))
                return Response<string>.Fail(400, "Genre is required");
            if (request.Genre.Length < 3)
                return Response<string>.Fail(400, "Genre must be at least 3 characters long");
            if (request.AuthorId == 0)
                return Response<string>.Fail(400, "AuthorId is required");
            if (string.IsNullOrWhiteSpace(request.Title))
                return Response<string>.Fail(400, "Title is required");

            item.Title = request.Title;
            item.Genre = request.Genre;
            item.PublishedDate = request.PublishedDate;
            item.AuthorId = request.AuthorId;
            await _context.SaveChangesAsync();
            return Response<string>.Done("Book updated successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }
}
