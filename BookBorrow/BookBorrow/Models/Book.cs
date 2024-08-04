using System.ComponentModel.DataAnnotations;

namespace BookBorrow.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string? BookImageUrl { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Number of copies cannot be negative.")]
        public int NumberOfCopies { get; set; }
        public int NumberOfCopiesAfterBorrowed { get; set; }


        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

        public Book()
        {
            NumberOfCopiesAfterBorrowed = NumberOfCopies;
        }

        public Book(int numberOfCopies)
        {
            NumberOfCopies = numberOfCopies;
            NumberOfCopiesAfterBorrowed = numberOfCopies;
        }

        public void Borrow()
        {
            if (NumberOfCopiesAfterBorrowed > 0)
            {
                NumberOfCopiesAfterBorrowed--;
            }
        }

        public void Return()
        {
            if (NumberOfCopiesAfterBorrowed < NumberOfCopies)
            {
                NumberOfCopiesAfterBorrowed++;
            }
        }

        public bool CanBorrow()
        {
            return NumberOfCopiesAfterBorrowed > 0;
        }

        public bool CanReturn()
        {
            return NumberOfCopiesAfterBorrowed < NumberOfCopies;
        }

        public void UpdateNumberOfCopies(int newNumberOfCopies)
        {
            NumberOfCopies = newNumberOfCopies;

            // Reset NumberOfCopiesAfterBorrowed if there are no borrowed books
            if (BorrowRecords.All(br => br.ReturnDate != null))
            {
                NumberOfCopiesAfterBorrowed = newNumberOfCopies;
            }
        }
    }
}
