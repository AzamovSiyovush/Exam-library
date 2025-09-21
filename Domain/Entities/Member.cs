using System;

namespace Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime MembershipDate { get; set; } = DateTime.Now;

    // Navigation property
    public IEnumerable<BorrowRecord> BorrowRecords { get; set; }    
}
