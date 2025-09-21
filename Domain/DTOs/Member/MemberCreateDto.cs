using System;

namespace Domain.DTOs.Member;

public class MemberCreateDto
{
public string Name { get; set; }
public string Email { get; set; }
public DateTime MembershipDate { get; set; } = DateTime.Now;
}
