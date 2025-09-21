using System;

namespace Domain.DTOs.BorrowRecord;

public class BorrowRecordGetDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; }
}
