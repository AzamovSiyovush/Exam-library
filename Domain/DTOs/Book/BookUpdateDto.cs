using System;

namespace Domain.DTOs.Book;

public class BookUpdateDto
{
public string Title { get; set; }
public string Genre { get; set; }
public DateTime PublishedDate { get; set; }
public int AuthorId { get; set; }
}
