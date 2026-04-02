namespace E_Commerce.Shared.DTOs.OrderDTO
{
    public record AddressDTO
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string Street { get; set; } = default!;
    }


}