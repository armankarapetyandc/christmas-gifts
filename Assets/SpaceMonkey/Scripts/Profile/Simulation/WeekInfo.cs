namespace SpaceMonkey.Scripts.Profile.Simulation
{
    public struct WeekInfo
    {
        public string Id { get; set; }
        public int Week { get; set; }
        public int NeededCap { get; set; }
        public OrderInfo[] Orders { get; set; }
        
    }
    public struct OrderInfo
    {
        public string CharacterId { get; set; }
        public int Mood { get; set; }
        public bool Shipped { get; set; }
        public ProductOrderInfo[]  Products { get; set; }
    }

    public struct ProductOrderInfo
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}

