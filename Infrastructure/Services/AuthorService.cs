using System;
using Domain.DTOs.Author;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorService : IAuthorServices
{
    private readonly DataContext _context;
    public AuthorService(DataContext context)
    {
        _context = context;
    }
    public async Task<Response<string>> CreateItem(AuthorCreateDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Response<string>.Fail(400, "Author name is required");
            if (request.Name.Length < 3)
                return Response<string>.Fail(400, "Author name is too short");
            var authorNew = new Author()
            {
                Name = request.Name,
                BirthDate = request.BirthDate
            };
            await _context.Authors.AddAsync(authorNew);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Author created successfully");
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

            var item = await _context.Authors.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Author not found");
            _context.Authors.Remove(item);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Author deleted successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<AuthorGetDto>> GetAuthorById(int id)
    {
        try
        {
            var item = await _context.Authors.FindAsync(id);
            if (item == null)
                return Response<AuthorGetDto>.Fail(404, "Author not found");

            // if (id < 1)
            //     return Response<AuthorGetDto>.Fail(400, "Id must be greater than 0");
            var author = await _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorGetDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthDate = a.BirthDate
                }).FirstOrDefaultAsync();
            return Response<AuthorGetDto>.Success(author, "Success");
        }
        catch( Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<AuthorGetDto>.Fail(500, "Internal server error:");
        }
    }

    

    public async Task<Response<IEnumerable<AuthorGetDto>>> GetItems()
    {
        try
        {

            var authors = await _context.Authors
           .Select(a => new AuthorGetDto
           {
               Id = a.Id,
               Name = a.Name,
               BirthDate = a.BirthDate
           }).ToListAsync();
        return Response<IEnumerable<AuthorGetDto>>.Success(authors,"Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<AuthorGetDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<string>> UpdateItem(AuthorUpdateDto request, int id)
    {
        try
        {
            var item = await _context.Authors.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Author not found");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Response<string>.Fail(400, "Author name is required");

            if (request.Name.Length < 3)
                return Response<string>.Fail(400, "Author name is too short");
            item.Name = request.Name;
            item.BirthDate = request.BirthDate;
            await _context.SaveChangesAsync();
            return Response<string>.Done("Author updated successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }
}
