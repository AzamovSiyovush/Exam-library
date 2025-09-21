using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
           modelBuilder.Entity<Book>()
        .HasOne(b => b.Author)
        .WithMany(a => a.Books)
        .HasForeignKey(b => b.AuthorId);
        
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.Book)
            .WithMany(b => b.BorrowRecords)
            .HasForeignKey(br => br.BookId);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.Member)
            .WithMany(m => m.BorrowRecords)
            .HasForeignKey(br => br.MemberId);

        modelBuilder.Entity<BorrowRecord>()
            .HasKey(p => p.MemberId);

        modelBuilder.Entity<BorrowRecord>()
            .HasKey(p => p.BookId);

            
    }

}
