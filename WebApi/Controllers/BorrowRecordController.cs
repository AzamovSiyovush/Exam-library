using System;
using System.Collections;
using Domain.DTOs.BorrowRecord;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/borrowRecord")]
public class BorrowRecordController
{
    private readonly IBorrowRecordServices borrowRecordServices;
    public BorrowRecordController(IBorrowRecordServices services)
    {
        borrowRecordServices = services;
    }
    [HttpPost]
    public async Task<Response<string>> CreateItem([FromForm]BorrowRecordCreateDto request)
    {
        return await borrowRecordServices.CreateItem(request);
    }
    [HttpPut("{id:int}")]
    public async Task<Response<string>> UpdateItem([FromForm]BorrowRecordUpdateDto request, [FromRoute]int id)
    {
        return await borrowRecordServices.UpdateItem(request, id);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteItem(int id)
    {
        return await borrowRecordServices.DeleteItem(id);
    }
    [HttpGet("getall")]
    public async Task<Response<IEnumerable<BorrowRecordGetDto>>> GetItems()
    {
        return await borrowRecordServices.GetItems();
    }
    [HttpGet("get/overdue")]
    public async Task<Response<IEnumerable<BorrowRecordGetDto>>> GetOverDueBorrows()
    {
        return await borrowRecordServices.GetOverdueBorrows();
    }
    [HttpGet("{bookId:int}")]
    public async Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByBook(int bookId)
    {
        return await borrowRecordServices.GetBorrowHistoryByBook(bookId);
    }
    [HttpGet("get/{memberId:int}")]
    public async Task<Response<BorrowRecordGetDto>> GetBorrowHistoryByMember(int memberId)
    {
        return await borrowRecordServices.GetBorrowHistoryByMember(memberId);
    }
}
