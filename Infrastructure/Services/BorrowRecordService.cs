using System;
using System.Collections;
using Domain.DTOs.BorrowRecord;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BorrowRecordService : IBorrowRecordServices
{
    private readonly DataContext _context;
    public BorrowRecordService(DataContext context)
    {
        _context = context;
    }
    public async Task<Response<string>> CreateItem(BorrowRecordCreateDto request)
    {
        try
        {
            if (request.BookId == 0 || request.MemberId == 0)
                return Response<string>.Fail(400, "");

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);
            if (book == null)
                return Response<string>.Fail(404, "Not Found");
            var borrowNew = new BorrowRecord()
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                ReturnDate = request.ReturnDate
            };
            await _context.BorrowRecords.AddAsync(borrowNew);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Borrow record created successfully");
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

            if (id < 1)
                return Response<string>.Fail(400, "Id must be greater than 0");

            var item = await _context.BorrowRecords.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Borrow record not found");

            _context.BorrowRecords.Remove(item);
            await _context.SaveChangesAsync();
            return Response<string>.Done("Borrow record deleted successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }

    }

    public async Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByBook(int bookId)
    {
        try
        {

            if (bookId < 1)
                return Response<BorrowRecordGetDto>.Fail(400, "Bad request");
            var find = await _context.BorrowRecords.AnyAsync(b => b.BookId == bookId);
            if (!find)
                return Response<BorrowRecordGetDto>.Fail(404, "Not Found");
            var item = await _context.BorrowRecords
                .Where(b => b.BookId == bookId)
                .Select(b => new BorrowRecordGetDto
                {
                    Id = b.Id,
                    BookId = b.BookId,
                    MemberId = b.MemberId,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate
                }).FirstOrDefaultAsync();
            return Response<BorrowRecordGetDto>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<BorrowRecordGetDto>.Fail(500, "Internal Server Error");
        }
    }

    public async Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByMember(int memberId)
    {
        try
        {
            if (memberId == 0)
                return Response<BorrowRecordGetDto>.Fail(400, "MemberId is required");
            var find = await _context.BorrowRecords.AnyAsync(b => b.MemberId == memberId);
            if (!find)
                return Response<BorrowRecordGetDto>.Fail(404, $"No borrow records found for your given memberId: '{memberId}'");
            var item = await _context.BorrowRecords
                .Where(b => b.MemberId == memberId)
                .Select(b => new BorrowRecordGetDto
                {
                    Id = b.Id,
                    BookId = b.BookId,
                    MemberId = b.MemberId,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate
                }).FirstOrDefaultAsync();
            return Response<BorrowRecordGetDto>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<BorrowRecordGetDto>.Fail(500,"Internal Server Error");
        }
    }

    public async Task<Response<BorrowRecordGetDto>> GetBorrowRecordById(int id)
    {
        try
        {
            if (id < 1)
                return Response<BorrowRecordGetDto>.Fail(400, "Id must be greater than 0");
            var item = await _context.BorrowRecords.FindAsync(id);
            if (item == null)
                return Response<BorrowRecordGetDto>.Fail(404, "Borrow record not found");
            var borrowRecord = await _context.BorrowRecords
                .Where(b => b.Id == id)
                .Select(b => new BorrowRecordGetDto
                {
                    Id = b.Id,
                    BookId = b.BookId,
                    MemberId = b.MemberId,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate
                }).FirstOrDefaultAsync();
            return Response<BorrowRecordGetDto>.Success(borrowRecord, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<BorrowRecordGetDto>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<IEnumerable<BorrowRecordGetDto>>> GetItems()
    {
        try
        {

            var borrowRecords = await _context.BorrowRecords
                 .Select(b => new BorrowRecordGetDto
                 {
                     Id = b.Id,
                     BookId = b.BookId,
                     MemberId = b.MemberId,
                     BorrowDate = b.BorrowDate,
                     ReturnDate = b.ReturnDate
                 }).ToListAsync();
                 return Response<IEnumerable<BorrowRecordGetDto>>.Success(borrowRecords, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<BorrowRecordGetDto>>.Fail(500, "Internal server error:");
        }

    }

    public async Task<Response<IEnumerable<BorrowRecordGetDto>>> GetOverdueBorrows()
    {
        try
        {
            var item = await _context.BorrowRecords
            .Where(b => b.ReturnDate < DateTime.Now)
            .Select(b => new BorrowRecordGetDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            }).ToListAsync();
            return Response<IEnumerable<BorrowRecordGetDto>>.Success(item, "Success");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<IEnumerable<BorrowRecordGetDto>>.Fail(500, "Internal server error:");
        }
    }

    public async Task<Response<string>> UpdateItem(BorrowRecordUpdateDto request, int id)
    {
        try
        {
            if (id < 1)
                return Response<string>.Fail(400, "Id must be greater than 0");
            if (request.BookId == 0 || request.MemberId == 0)
                return Response<string>.Fail(400, "BookId and MemberId are required");
            var item = await _context.BorrowRecords.FindAsync(id);
            if (item == null)
                return Response<string>.Fail(404, "Borrow record not found");
            item.BookId = request.BookId;
            item.MemberId = request.MemberId;
            item.ReturnDate = request.ReturnDate;
            await _context.SaveChangesAsync();
            return Response<string>.Done("Borrow record updated successfully");
        }
       catch(Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return Response<string>.Fail(500, "Internal server error:");
        }
    }
}
