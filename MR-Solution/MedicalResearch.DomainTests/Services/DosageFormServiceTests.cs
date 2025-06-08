using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Assert = Xunit.Assert;



namespace MedicalResearch.DomainTests.Services;

public class DosageFormServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DosageForm> _validator;
    private readonly ILogger<DosageFormService> _logger;
    private readonly DosageFormService _service;

    public DosageFormServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = Substitute.For<IValidator<DosageForm>>();
        _logger = Substitute.For<ILogger<DosageFormService>>();
        _service = new DosageFormService(_unitOfWork, _validator, _logger);
    }

    [Fact]
    public async Task AddDosageFormAsync_ShouldAdd_WhenValid()
    {
        // Arrange  
        var dosageForm = new DosageForm { Name = "Tablet" };
        _validator.ValidateAsync(dosageForm).Returns(new FluentValidation.Results.ValidationResult());
        _unitOfWork.DosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name).Returns((DosageForm?)null);
        _unitOfWork.DosageFormRepository.AddAsync(dosageForm).Returns(dosageForm);
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _service.AddDosageFormAsync(dosageForm);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(dosageForm.Name, result.Name);
    }

    [Fact]
    public async Task AddDosageFormAsync_ShouldThrow_WhenInvalid()
    {
        // Arrange  
        var dosageForm = new DosageForm { Name = "" };
        _validator.ValidateAsync(dosageForm).Returns(new FluentValidation.Results.ValidationResult
        {
            Errors = { new FluentValidation.Results.ValidationFailure("Name", "Name is required") }
        });

        // Act & Assert  
        var exception = await Assert.ThrowsAsync<DomainException>(() => _service.AddDosageFormAsync(dosageForm));
        Assert.Equal("Name is required", exception.Message);
    }

    [Fact]
    public async Task DeleteDosageFormAsync_ShouldDelete_WhenExists()
    {
        // Arrange  
        var dosageForm = new DosageForm { Id = 1, Name = "Tablet" };
        _unitOfWork.DosageFormRepository.GetByIdAsync(dosageForm.Id).Returns(dosageForm);
        _unitOfWork.DosageFormRepository.Delete(dosageForm).Returns(true);
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _service.DeleteDosageFormAsync(dosageForm.Id);

        // Assert  
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteDosageFormAsync_ShouldThrow_WhenNotFound()
    {
        // Arrange  
        _unitOfWork.DosageFormRepository.GetByIdAsync(Arg.Any<int>()).Returns((DosageForm?)null);

        // Act & Assert  
        var exception = await Assert.ThrowsAsync<DomainException>(() => _service.DeleteDosageFormAsync(1));
        Assert.Equal("Dosage form not found", exception.Message);
    }

    [Fact]
    public async Task GetDosageFormAsync_ShouldReturn_WhenExists()
    {
        // Arrange  
        var dosageForm = new DosageForm { Id = 1, Name = "Tablet" };
        _unitOfWork.DosageFormRepository.GetByIdAsync(dosageForm.Id).Returns(dosageForm);

        // Act  
        var result = await _service.GetDosageFormAsync(dosageForm.Id);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(dosageForm.Name, result?.Name);
    }

    [Fact]
    public async Task GetDosageFormAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange  
        _unitOfWork.DosageFormRepository.GetByIdAsync(Arg.Any<int>()).Returns((DosageForm?)null);

        // Act  
        var result = await _service.GetDosageFormAsync(1);

        // Assert  
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateDosageFormAsync_ShouldUpdate_WhenValid()
    {
        // Arrange  
        var dosageForm = new DosageForm { Id = 1, Name = "Tablet" };
        _validator.ValidateAsync(dosageForm).Returns(new FluentValidation.Results.ValidationResult());
        _unitOfWork.DosageFormRepository.GetByIdAsync(dosageForm.Id).Returns(dosageForm);
        _unitOfWork.DosageFormRepository.GetDosageFormByNameAsync(dosageForm.Name).Returns((DosageForm?)null);
        _unitOfWork.DosageFormRepository.Update(dosageForm).Returns(dosageForm);
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _service.UpdateDosageFormAsync(dosageForm);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(dosageForm.Name, result.Name);
    }

    [Fact]
    public async Task UpdateDosageFormAsync_ShouldThrow_WhenInvalid()
    {
        // Arrange  
        var dosageForm = new DosageForm { Id = 1, Name = "" };
        _validator.ValidateAsync(dosageForm).Returns(new FluentValidation.Results.ValidationResult
        {
            Errors = { new FluentValidation.Results.ValidationFailure("Name", "Name is required") }
        });

        // Act & Assert  
        var exception = await Assert.ThrowsAsync<DomainException>(() => _service.UpdateDosageFormAsync(dosageForm));
        Assert.Equal("Name is required", exception.Message);
    }
}
