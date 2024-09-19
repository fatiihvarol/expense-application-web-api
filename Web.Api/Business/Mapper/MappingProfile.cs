using AutoMapper;
using Web.Api.Data.Entities;
using Web.Api.Schema;

namespace Web.Api.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map entities to view models
            CreateMap<VpExpenseForm, ExpenseFormResponse>()
                .ForMember(dest => dest.CurrencyEnum, opt => opt.MapFrom(src => src.CurrencyEnum.ToString()))
                .ForMember(dest => dest.ExpenseStatusEnum, opt => opt.MapFrom(src => src.ExpenseStatusEnum));

            CreateMap<VpExpense, ExpenseFormResponse>();
        }
    }

}
