namespace MedicalResearch.Api.DTO
{
    public class ClinicCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AddressOne { get; set; } = string.Empty;
        public string? AddressTwo { get; set; }
        public string? Phone { get; set; }
    }
}
