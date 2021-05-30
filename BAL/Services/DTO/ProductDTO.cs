namespace Services.DTO
{
    public class ProductDTO : DTO
    {
        public ProductDTO()
        {

        }
        public ProductDTO(long id, string code, string name, byte[] photo, decimal price)
        {
            Id = id;
            Code = code;
            Name = name;
            Photo = photo;
            Price = price;
        }
        public string Code { get; set; }

        public string Name { get; set; }

        public byte[] Photo { get; set; }

        public decimal Price { get; set; }
    }
}
