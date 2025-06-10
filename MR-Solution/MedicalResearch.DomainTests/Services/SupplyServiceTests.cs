using Xunit;
using FluentValidation;
using MedicalResearch.DAL.UnitOfWork;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Assert = Xunit.Assert;



namespace MedicalResearch.Domain.Services.Tests;

public class SupplyServiceTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IValidator<Supply> _validator = Substitute.For<IValidator<Supply>>();
    private readonly ILogger<SupplyService> _logger = Substitute.For<ILogger<SupplyService>>();
    private readonly SupplyService _supplyService;

    public SupplyServiceTests()
    {
        _supplyService = new SupplyService(_unitOfWork, _validator, _logger);
    }

    [Fact]
    public async Task AddSupplyAsync_ShouldAddSupplies_WhenValid()
    {
        // Arrange  
        var supplies = new List<Supply>
           {
               new Supply { Id = 1, MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = false }
           };
        var userId = 1;
        var medicine = new Medicine { Id = 1, Amount = 20 };

        _unitOfWork.SupplyRepository.GetByIdAsync(1).Returns(supplies[0]);
        _unitOfWork.MedicineRepository.GetByIdAsync(1).Returns(medicine);
        _unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(1, 1).Returns((ClinicStockMedicine?)null);
        _unitOfWork.SupplyRepository.Update(Arg.Any<Supply>()).Returns(supplies[0]);

        // Act  
        var result = await _supplyService.AddSupplyAsync(supplies, userId);

        // Assert  
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result[0].IsActive);
        await _unitOfWork.Received(1).SaveAsync();
    }

    [Fact]
    public async Task AddSupplyAsync_ShouldThrowException_WhenSupplyNotFound()
    {
        // Arrange  
        var supplies = new List<Supply>
           {
               new Supply { Id = 1, MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = false }
           };
        var userId = 1;

        _unitOfWork.SupplyRepository.GetByIdAsync(1).Returns((Supply?)null);

        // Act & Assert  
        await Assert.ThrowsAsync<DomainException>(() => _supplyService.AddSupplyAsync(supplies, userId));
    }

    [Fact]
    public async Task AddToSupply_ShouldAddSupply_WhenValid()
    {
        // Arrange  
        var medicine = new Medicine { Id = 1, Description = "Aspirin", CreatedAt = DateTime.UtcNow, Amount = 20 };
        var amount = 10;
        var clinicId = 1;
        var userId = 1;
        var supply = new Supply { MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = false };

        _unitOfWork.MedicineRepository.GetByIdAsync(1).Returns(medicine);
        _validator.ValidateAsync(Arg.Any<Supply>()).Returns(new FluentValidation.Results.ValidationResult());
        _unitOfWork.SupplyRepository.AddAsync(Arg.Any<Supply>()).Returns(supply);
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _supplyService.AddToSupply(medicine, amount, clinicId, userId);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(supply.MedicineId, result.MedicineId);
        await _unitOfWork.Received(1).SaveAsync();
    }

    [Fact]
    public async Task AddToSupply_ShouldThrowException_WhenNotEnoughMedicineInStock()
    {
        // Arrange  
        var medicine = new Medicine { Id = 1, Amount = 5 };
        var amount = 10;
        var clinicId = 1;
        var userId = 1;

        _unitOfWork.MedicineRepository.GetByIdAsync(1).Returns(medicine);

        // Act & Assert  
        await Assert.ThrowsAsync<DomainException>(() => _supplyService.AddToSupply(medicine, amount, clinicId, userId));
    }



    [Fact]
    public async Task DeleteSupplyAsync_ShouldDeleteSupply_WhenValid()
    {
        // Arrange  
        var supplyId = 1;
        var supply = new Supply { Id = supplyId, MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = true };
        var medicine = new Medicine { Id = 1, Amount = 20 };
        var clinicStockMedicine = new ClinicStockMedicine { MedicineId = 1, ClinicId = 1, Amount = 10 };

        _unitOfWork.SupplyRepository.GetByIdAsync(supplyId).Returns(supply);
        _unitOfWork.MedicineRepository.GetByIdAsync(1).Returns(medicine);
        _unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(1, 1).Returns(clinicStockMedicine);
        _unitOfWork.SupplyRepository.Delete(supply).Returns(true);
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _supplyService.DeleteSupplyAsync(supplyId);

        // Assert  
        Assert.True(result);
        Assert.Equal(30, medicine.Amount);
        Assert.Equal(0, clinicStockMedicine.Amount);
        await _unitOfWork.Received(1).SaveAsync();
    }

    [Fact]
    public async Task DeleteSupplyAsync_ShouldThrowException_WhenSupplyNotFound()
    {
        // Arrange  
        var supplyId = 1;
        _unitOfWork.SupplyRepository.GetByIdAsync(supplyId).Returns((Supply?)null);

        // Act & Assert  
        await Assert.ThrowsAsync<DomainException>(() => _supplyService.DeleteSupplyAsync(supplyId));
    }

    [Fact]
    public async Task UpdateSupplyAsync_ShouldUpdateSupply_WhenValid()
    {
        // Arrange  
        var supply = new Supply { Id = 1, MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = true };
        var updatedSupply = new Supply { Id = 1, MedicineId = 1, Amount = 15, ClinicId = 1, UserId = 1, IsActive = true };
        var clinicStockMedicine = new ClinicStockMedicine { MedicineId = 1, ClinicId = 1, Amount = 10 };

        _unitOfWork.SupplyRepository.GetByIdAsync(1).Returns(supply);
        _unitOfWork.ClinicStockMedicineRepository.GetClinicStockMedicineAsync(1, 1).Returns(clinicStockMedicine);
        _unitOfWork.SupplyRepository.Update(supply).Returns(updatedSupply);
        _validator.ValidateAsync(Arg.Any<Supply>()).Returns(new FluentValidation.Results.ValidationResult());
        _unitOfWork.SaveAsync().Returns(1);

        // Act  
        var result = await _supplyService.UpdateSupplyAsync(updatedSupply);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(15, result.Amount);
        Assert.Equal(15, clinicStockMedicine.Amount);
        await _unitOfWork.Received(1).SaveAsync();
    }

    [Fact]
    public async Task UpdateSupplyAsync_ShouldThrowException_WhenSupplyNotFound()
    {
        // Arrange  
        var supply = new Supply { Id = 1, MedicineId = 1, Amount = 10, ClinicId = 1, UserId = 1, IsActive = true };
        _unitOfWork.SupplyRepository.GetByIdAsync(1).Returns((Supply?)null);
        _validator.ValidateAsync(Arg.Any<Supply>()).Returns(new FluentValidation.Results.ValidationResult());

        // Act & Assert  
        await Assert.ThrowsAsync<DomainException>(() => _supplyService.UpdateSupplyAsync(supply));
    }
}