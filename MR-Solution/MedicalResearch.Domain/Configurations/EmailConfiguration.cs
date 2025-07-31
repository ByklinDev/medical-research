namespace MedicalResearch.Domain.Configurations;

public class EmailConfiguration
{

    public string SmtpServer { get; set; } = null!;
    public string Port { get; set; } = null!;
    public string CompanyEmail { get; set; } = null!;
    public string CompanyEmailPassword { get; set; } = null!;

}
