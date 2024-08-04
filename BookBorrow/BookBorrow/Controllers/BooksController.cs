using BookBorrow.Data;
using BookBorrow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using BookBorrow.Models;

namespace BookBorrow.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookBorrowContext _context;
        public BooksController (BookBorrowContext context)
        {
            _context = context;
        }

        //Get Books
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            ViewBag.Clients = await _context.Clients.ToListAsync();
            return View(books);
        }

        //Get Books Details
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //Get Book Create 
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookImageUrl,NumberOfCopies")] Book book)
        {
            if (ModelState.IsValid)
            {
                // Set NumberOfCopiesAfterBorrowed to NumberOfCopies for new books
                book.NumberOfCopiesAfterBorrowed = book.NumberOfCopies;

                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Book added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,BookImageUrl,NumberOfCopies")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books.FindAsync(id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    // Update the book details
                    existingBook.BookName = book.BookName;
                    existingBook.BookImageUrl = book.BookImageUrl;
                    existingBook.UpdateNumberOfCopies(book.NumberOfCopies);

                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //Post Book Delete 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        // Borrow a book
        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Message"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!book.CanBorrow())
            {
                TempData["Message"] = "Cannot borrow this book. No copies available.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("ClientPage", new { bookId = id });

        }

        public async Task<IActionResult> ClientPage(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                TempData["Message"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new BorrowViewModel
            {
                Book = book,
                Clients = await _context.Clients.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBorrow(int bookId, int selectedClientId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                TempData["Message"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!book.CanBorrow())
            {
                TempData["Message"] = "Cannot borrow this book. No copies available.";
                return RedirectToAction(nameof(Index));
            }

            var client = await _context.Clients.FindAsync(selectedClientId);
            if (client == null)
            {
                TempData["Message"] = "Client not found.";
                return RedirectToAction(nameof(Index));
            }

            book.Borrow();
            _context.BorrowRecords.Add(new BorrowRecord
            {
                BookId = book.BookId,
                ClientId = client.ClientId,
                BorrowDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
            TempData["Message"] = "Book borrowed successfully!";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Return(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Message"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!book.CanReturn())
            {
                TempData["Message"] = "Cannot return this book. All copies are already returned.";
                return RedirectToAction(nameof(Index));
            }

            var borrowRecord = await _context.BorrowRecords
                .FirstOrDefaultAsync(br => br.BookId == id && br.ReturnDate == null);

            if (borrowRecord == null)
            {
                TempData["Message"] = "No active borrow record found for this book.";
                return RedirectToAction(nameof(Index));
            }

            borrowRecord.ReturnDate = DateTime.Now;

            book.Return();

            await _context.SaveChangesAsync();
            TempData["Message"] = "Book returned successfully!";
            return RedirectToAction(nameof(Index));
        }


    }
}
