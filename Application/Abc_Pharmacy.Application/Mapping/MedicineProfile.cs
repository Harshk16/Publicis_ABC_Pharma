using Abc_Pharmacy.Application.Dto;
using AutoMapper;

namespace Abc_Pharmacy.Application.Mapping
{
    public class MedicineProfile : Profile
    {
        public MedicineProfile()
        {
            CreateMap<AddMedicineDto, MedicineDto>();
        }
    }
}
