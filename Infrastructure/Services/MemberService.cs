using System;
using Domain.DTOs.Member;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class MemberService : IMemberServices
{
    private readonly DataContext _context;
    public MemberService(DataContext context)
    {
        _context = context;
    }
    public async Task<Response<string>> CreateItem(MemberCreateDto request)
    {
        try
        {
            
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            return Response<string>.Fail(400, "Name and Email are required");
            
        if (request.Name.Length < 3)
                return Response<string>.Fail(400, "Name must be at least 3 characters long");
                
        if (request.Email.Length < 5)
                return Response<string>.Fail(400, "Email must be at least 5 characters long");
        if (!request.Email.Contains("@"))
            return Response<string>.Fail(400, "Email is not valid");
        var findEmail = await _context.Members.AnyAsync(m => m.Email.ToLower() == request.Email.ToLower());
        if (findEmail)
            return Response<string>.Fail(409, "Email already exists");
        var newMember = new Member()
        {
            Name = request.Name,
            Email = request.Email
        };
        await _context.Members.AddAsync(newMember);
        await _context.SaveChangesAsync();
        return Response<string>.Done("Member created successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, $"Internal server error: {ex.Message}");
        }
    }

    public async Task<Response<string>> DeleteItem(int id)
    {
        try
        {
            if (id < 1)
                return Response<string>.Fail(400, "Id must be greater than 0");
            var item = await _context.Members.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Member not found");
            _context.Members.Remove(item);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Member deleted successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<IEnumerable<MemberGetDto>>> GetItems()
    {
        try
        {

            var members = await _context.Members
                 .Select(m => new MemberGetDto
                 {
                     Id = m.Id,
                     Name = m.Name,
                     Email = m.Email,
                     MembershipDate = m.MembershipDate
                 }).ToListAsync();
        return Response<IEnumerable<MemberGetDto>>.Success(members, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<MemberGetDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<MemberGetDto>> GetMemberById(int id)
    {
        try
        {
            if (id < 1)
                return Response<MemberGetDto>.Fail(400, "Id must be greater than 0");
            var item = await _context.Members.FindAsync(id);
            if (item == null)
                return Response<MemberGetDto>.Fail(404, "Member not found");
            var member = await _context.Members
                .Where(m => m.Id == id)
                .Select(m => new MemberGetDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    MembershipDate = m.MembershipDate
                }).FirstOrDefaultAsync();
            return Response<MemberGetDto>.Success(member, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<MemberGetDto>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<IEnumerable<MemberGetRecentBorrowsDto>>> GetMemberWithRecentBorrows(int days)
    {
        try
        {
            if (days < 0)
                return Response<IEnumerable<MemberGetRecentBorrowsDto>>.Fail(400, "Bad Request");
            var currentDate = DateTime.Now;
            var minus = currentDate.AddDays(-days);
            var item = await _context.BorrowRecords
            .Where(p => p.BorrowDate >= minus)
            .Select(m => m.Member).ToListAsync();
            var member = item.Select(m => new MemberGetRecentBorrowsDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                MembershipDate = m.MembershipDate
            }).ToList();
            return Response<IEnumerable<MemberGetRecentBorrowsDto>>.Success(member, "Success");
        }
    catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<MemberGetRecentBorrowsDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<string>> UpdateItem(MemberUpdateDto request, int id)
    {
        try
        {
            if (id < 1)
                return Response<string>.Fail(400, "Id must be greater than 0");
            var item = await _context.Members.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Member not found");
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
                return Response<string>.Fail(400, "Name and Email are required");
            if (request.Name.Length < 3)
                return Response<string>.Fail(400, "Name must be at least 3 characters long");
            if (request.Email.Length < 5)
                return Response<string>.Fail(400, "Email must be at least 5 characters long");
            if (!request.Email.Contains("@"))
                return Response<string>.Fail(400, "Email is not valid");
            var findEmail = await _context.Members.AnyAsync(m => m.Email.ToLower() == request.Email.ToLower() && m.Id != id);
            if (findEmail)
                return Response<string>.Fail(409, "Email already exists");
            item.Name = request.Name;
            item.Email = request.Email;
            await _context.SaveChangesAsync();
            return Response<string>.Done("Member updated successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }
}
