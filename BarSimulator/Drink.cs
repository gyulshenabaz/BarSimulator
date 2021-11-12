namespace BarSimulator
{
    class Drink
    {
        public Drink(string name, double price, int quantity)
        {
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.SoldDrinksQuantity = 0;
        }

        public string Name { get; }
        
        public double Price { get; }

        public int Quantity { get; set; }
        
        public int SoldDrinksQuantity { get; set; }
        
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}