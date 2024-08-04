namespace BookBorrow.Models
{
    public class Client
    {
        public int ClientId { get; set; }   
        public string ClientName { get; set; }
        public int ClientPhoneNumber { get; set; }  

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}
