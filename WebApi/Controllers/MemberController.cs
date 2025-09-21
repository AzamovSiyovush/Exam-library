using System;
using Domain.DTOs.Member;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/members")]
public class MemberController
{
    private readonly IMemberServices _memberService;
    public MemberController(IMemberServices memberServices)
    {
        _memberService = memberServices;
    }

    [HttpPost]
    public async Task<Response<string>> CreateItem([FromForm]MemberCreateDto request)
    {
        return await _memberService.CreateItem(request);
    }
    [HttpPut("{id:int}")]
    public async Task<Response<string>> UpdateItem([FromForm]MemberUpdateDto request, [FromRoute]int id)
    {
        return await _memberService.UpdateItem(request, id);
    }
    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteItem(int id)
    {
        return await _memberService.DeleteItem(id);
    }
    [HttpGet("all")]
    public async Task<Response<IEnumerable<MemberGetDto>>> GetItems()
    {
        return await _memberService.GetItems();
    }
    [HttpGet("{id:int}")]
    public async Task<Response<MemberGetDto>> GetMemberGetById(int id)
    {
        return await _memberService.GetMemberById(id);
    }
    [HttpGet("get/days{days:int}")]
    public async Task<Response<IEnumerable<MemberGetRecentBorrowsDto>>> GetMembersWithRecentBorrows(int days)
    {
        return await _memberService.GetMemberWithRecentBorrows(days);
    }
}
