using System;

namespace Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }

    // Navigation property
    public IEnumerable<Book> Books { get; set; }
    public IEnumerable<BorrowRecord> BorrowRecords { get; set; }
}
