using System;
using Domain.DTOs.Member;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IMemberServices
{
    Task<Response<string>> CreateItem(MemberCreateDto request);
    Task<Response<IEnumerable<MemberGetDto>>> GetItems();
    Task<Response<string>> UpdateItem(MemberUpdateDto request, int id);
    Task<Response<string>> DeleteItem(int id);
    Task<Response<MemberGetDto>> GetMemberById(int id);
    Task<Response<IEnumerable<MemberGetRecentBorrowsDto>>> GetMemberWithRecentBorrows(int days);
    
}
