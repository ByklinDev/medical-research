using MedicalResearch.DAL.DataContext;
using MedicalResearch.DAL.Repositories;
using MedicalResearch.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly MedicalResearchDbContext _context;
        private IClinicRepository? clinicRepository;
        private IClinicStockMedicineRepository? clinicStockMedicineRepository;
        private IDosageFormRepository? dosageFormRepository;
        private IMedicineContainerRepository? medicineContainerRepository;
        private IMedicineRepository? medicineRepository;
        private IMedicineTypeRepository? medicineTypeRepository;
        private IPatientRepository? patientRepository;
        private IRoleRepository? roleRepository;
        private ISupplyRepository? supplyRepository;
        private IUserRepository? userRepository;
        private IVisitRepository? visitRepository;

        public UnitOfWork(MedicalResearchDbContext context)
        {
            _context = context;
        }

        public IClinicRepository ClinicRepository
        {
            get
            {

                if (this.clinicRepository == null)
                {
                    this.clinicRepository = new ClinicRepository(_context);
                }
                return clinicRepository;
            }
        }

        public IClinicStockMedicineRepository ClinicStockMedicineRepository
        {
            get
            {

                if (this.clinicStockMedicineRepository == null)
                {
                    this.clinicStockMedicineRepository = new ClinicStockMedicineRepository(_context);
                }
                return clinicStockMedicineRepository;
            }
        }

        public IDosageFormRepository DosageFormRepository
        {
            get
            {

                if (this.dosageFormRepository == null)
                {
                    this.dosageFormRepository = new DosageFormRepository(_context);
                }
                return dosageFormRepository;
            }
        }

        public IMedicineContainerRepository MedicineContainerRepository
        {
            get
            {

                if (this.medicineContainerRepository == null)
                {
                    this.medicineContainerRepository = new MedicineContainerRepository(_context);
                }
                return medicineContainerRepository;
            }
        }

        public IMedicineRepository MedicineRepository
        {
            get
            {
                if (this.medicineRepository == null)
                {
                    this.medicineRepository = new MedicineRepository(_context);
                }
                return medicineRepository;
            }
        }
        public IMedicineTypeRepository MedicineTypeRepository
        {
            get
            {
                if (this.medicineTypeRepository == null)
                {
                    this.medicineTypeRepository = new MedicineTypeRepository(_context);
                }
                return medicineTypeRepository;
            }
        }
        public IPatientRepository PatientRepository
        {
            get
            {
                if (this.patientRepository == null)
                {
                    this.patientRepository = new PatientRepository(_context);
                }
                return patientRepository;
            }
        }
        public IRoleRepository RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new RoleRepository(_context);
                }
                return roleRepository;
            }
        }
        public ISupplyRepository SupplyRepository
        {
            get
            {
                if (this.supplyRepository == null)
                {
                    this.supplyRepository = new SupplyRepository(_context);
                }
                return supplyRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(_context);
                }
                return userRepository;
            }
        }
        public IVisitRepository VisitRepository
        {
            get
            {
                if (this.visitRepository == null)
                {
                    this.visitRepository = new VisitRepository(_context);
                }
                return visitRepository;
            }
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
