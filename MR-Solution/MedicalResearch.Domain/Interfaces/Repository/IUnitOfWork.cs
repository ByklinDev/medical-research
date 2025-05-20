using MedicalResearch.Domain.Interfaces.Repository;

namespace MedicalResearch.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IClinicRepository ClinicRepository { get; }
        IClinicStockMedicineRepository ClinicStockMedicineRepository { get; }
        IDosageFormRepository DosageFormRepository { get; }
        IMedicineContainerRepository MedicineContainerRepository { get; }
        IMedicineRepository MedicineRepository { get; }
        IMedicineTypeRepository MedicineTypeRepository { get; }
        IPatientRepository PatientRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISupplyRepository SupplyRepository { get; }
        IUserRepository UserRepository { get; }
        IVisitRepository VisitRepository { get; }
        Task<int> SaveAsync();
    }
}