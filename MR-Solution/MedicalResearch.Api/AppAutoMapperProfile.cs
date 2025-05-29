using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api
{
    public class AppAutoMapperProfile : Profile
    {
        public AppAutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserCreateDTO, User>();

            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
            CreateMap<RoleCreateDTO, Role>();

            CreateMap<Clinic, ClinicDTO>();
            CreateMap<ClinicDTO, Clinic>();
            CreateMap<ClinicCreateDTO, Clinic>();

            CreateMap<DosageForm, DosageFormDTO>();
            CreateMap<DosageFormDTO, DosageForm>();
            CreateMap<DosageFormCreateDTO, DosageForm>();

            CreateMap<Medicine, MedicineDTO>();
            CreateMap<MedicineDTO, Medicine>();
            CreateMap<MedicineCreateDTO, Medicine>();

            CreateMap<Supply, SupplyDTO>();
            CreateMap<SupplyDTO, Supply>();
            CreateMap<SupplyCreateDTO, Supply>();

            CreateMap<Patient, PatientDTO>();
            CreateMap<PatientDTO, Patient>();
            CreateMap<PatientCreateDTO, Patient>();

            CreateMap<MedicineContainer, MedicineContainerDTO>();
            CreateMap<MedicineContainerDTO, MedicineContainer>();
            CreateMap<MedicineContainerCreateDTO, MedicineContainer>();

            CreateMap<MedicineType, MedicineTypeDTO>();
            CreateMap<MedicineTypeDTO, MedicineType>();
            CreateMap<MedicineTypeCreateDTO, MedicineType>();

            CreateMap<Visit, VisitDTO>();
            CreateMap<VisitDTO, Visit>();
            CreateMap<VisitCreateDTO, Visit>();
        }
    }   
}
