namespace MedicalResearch.Api.DTO
{
    public class ClinicDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AddressOne { get; set; } = string.Empty;
        public string? AddressTwo { get; set; }
        public string? Phone { get; set; }
    }
}
