namespace StarbucksModels.ViewModels
{
    public class OrderHistoryVM
    {
        public string ProductName { get; set; }

        public string FirstName { get; set; }

        public decimal Price { get; set; }

        public string? CroppedUrl { get; set; }

        public int OrderHeaderId { get; set; }
        public int OrderDetailId { get; set; }

        public DateTime OrderDate { get; set; }


    }
}
