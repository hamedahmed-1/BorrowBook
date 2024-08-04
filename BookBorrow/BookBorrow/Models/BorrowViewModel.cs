namespace BookBorrow.Models
{
    public class BorrowViewModel
    {
        public Book Book { get; set; }
        public List<Client> Clients { get; set; }
        public int SelectedClientId { get; set; }
    }
}
