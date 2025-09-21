using System;
using Domain.DTOs.BorrowRecord;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IBorrowRecordServices
{
    Task<Response<string>> CreateItem(BorrowRecordCreateDto request);
    Task<Response<IEnumerable<BorrowRecordGetDto>>> GetItems();
    Task<Response<string>> UpdateItem(BorrowRecordUpdateDto request, int id);
    Task<Response<string>> DeleteItem(int id);
    Task<Response<BorrowRecordGetDto>> GetBorrowRecordById(int id);
    Task<Response<IEnumerable<BorrowRecordGetDto>>> GetOverdueBorrows();
    Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByMember(int memberId);
    Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByBook(int bookId);
}
