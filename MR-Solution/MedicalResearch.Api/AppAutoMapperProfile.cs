using AutoMapper;
using MedicalResearch.Api.DTO;
using MedicalResearch.Domain.DTO;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;

namespace MedicalResearch.Api;

public class AppAutoMapperProfile : Profile
{
    public AppAutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<User, UserRoleDTO>().ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Roles[0].Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Roles[0].Name));
        CreateMap<UserDTO, User>();
        CreateMap<UserCreateDTO, User>();
        CreateMap<UserUpdateDTO, User>();

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
        CreateMap<MedicineUpdateDTO, Medicine>();


        CreateMap<Supply, SupplyDTO>().ForMember(dest=> dest.ClinicName, opt => opt.MapFrom(src=> src.Clinic.Name))
            .ForMember(dest => dest.MedicineDescription, opt => opt.MapFrom(src => src.Medicine.Description));
        CreateMap<SupplyDTO, Supply>();
        CreateMap<SupplyCreateDTO, Supply>();

        CreateMap<Patient, PatientDTO>();
        CreateMap<PatientDTO, Patient>();
        CreateMap<PatientUpdateDTO, Patient>();
        CreateMap<PatientCreateDTO, Patient>();
        CreateMap<PatientResearchDTO, PatientDTO>();

        CreateMap<MedicineContainer, MedicineContainerDTO>();
        CreateMap<MedicineContainerDTO, MedicineContainer>();
        CreateMap<MedicineContainerCreateDTO, MedicineContainer>();

        CreateMap<MedicineType, MedicineTypeDTO>();
        CreateMap<MedicineTypeDTO, MedicineType>();
        CreateMap<MedicineTypeCreateDTO, MedicineType>();

        CreateMap<Visit, VisitDTO>().ForMember(dest => dest.MedicineDescription, opt => opt.MapFrom(src => src.Medicine.Description));
        CreateMap<VisitCreateDTO, Visit>();
        CreateMap<VisitDTO, Visit>();
        CreateMap<VisitCreateDTO, Visit>();

        CreateMap<ClinicStockMedicine, ClinicStockMedicineDTO>();
        CreateMap<ClinicStockMedicineDTO, ClinicStockMedicine>();

        CreateMap<QueryDTO, Query>();
        CreateMap<Query, QueryDTO>();        
    }
}   
