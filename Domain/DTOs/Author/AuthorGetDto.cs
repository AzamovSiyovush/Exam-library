using System;

namespace Domain.DTOs.Author;

public class AuthorGetDto
{
public int Id { get; set; }
public string Name { get; set; }
public DateTime BirthDate { get; set; }
}
