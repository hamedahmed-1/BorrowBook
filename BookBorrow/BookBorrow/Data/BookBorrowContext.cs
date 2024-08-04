using BookBorrow.Models;
using Microsoft.EntityFrameworkCore;

namespace BookBorrow.Data
{
    public class BookBorrowContext : DbContext
    {
        public BookBorrowContext(DbContextOptions<BookBorrowContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
    }
}
