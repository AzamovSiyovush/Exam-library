using System;

namespace Domain.DTOs.BorrowRecord;

public class BorrowRecordCreateDto
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; }
}
