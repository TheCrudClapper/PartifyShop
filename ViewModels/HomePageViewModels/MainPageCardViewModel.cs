namespace ComputerServiceOnlineShop.ViewModels.IndexPageViewModel
{
    public class MainPageCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }

        public string ImagePath { get; set; } = null!;
    }
}
