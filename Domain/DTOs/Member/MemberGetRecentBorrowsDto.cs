using System;

namespace Domain.DTOs.Member;

public class MemberGetRecentBorrowsDto
{
public int Id { get; set; }
public string Name { get; set; }
public string Email { get; set; }
public DateTime MembershipDate { get; set; }
}
