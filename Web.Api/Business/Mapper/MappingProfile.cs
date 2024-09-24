using AutoMapper;
using Web.Api.Base.Enums;
using Web.Api.Business.Cqrs;
using Web.Api.Data.Entities;
using Web.Api.Schema;

namespace Web.Api.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping from CreateExpenseFormCommand to VpExpenseForm
            CreateMap<CreateExpenseFormCommand, VpExpenseForm>()
                .ForMember(dest => dest.CurrencyEnum, opt => opt.MapFrom(src => src.Model.Currency))
                .ForMember(dest => dest.ExpenseStatusEnum, opt => opt.MapFrom(src => src.Model.ExpenseStatus))
                .ForMember(dest => dest.RejectionDescription, opt => opt.MapFrom(src => src.Model.RejectionDescription))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Model.TotalAmount))
                .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => src.Model.Expenses));

            // Mapping from ExpenseFormRequest to VpExpense
            CreateMap<ExpenseRequest, VpExpense>();

            // Mapping from VpExpenseForm to ExpenseFormResponse
            CreateMap<VpExpenseForm, ExpenseFormResponse>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.CurrencyEnum.ToString()))
                .ForMember(dest => dest.ExpenseStatus, opt => opt.MapFrom(src => src.ExpenseStatusEnum.ToString()))
                .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => src.Expenses)); // Map nested list

            // Mapping from VpExpense to ExpenseResponse
            CreateMap<VpExpense, ExpenseResponse>();
            // Mapping from CreateExpenseFormCommand to VpExpenseForm
            CreateMap<ExpenseFormRequest, VpExpenseForm>()
     .ForMember(dest => dest.CurrencyEnum, opt => opt.MapFrom(src => src.Currency))
     .ForMember(dest => dest.ExpenseStatusEnum, opt => opt.MapFrom(src => src.ExpenseStatus))
     .ForMember(dest => dest.RejectionDescription, opt => opt.MapFrom(src => src.RejectionDescription))
     .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
     .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => src.Expenses)); // Eğer `Expenses` listesi varsa


            CreateMap<VpExpenseFormHistory, ExpenseFormHistoryVM>()
                     .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action.ToString()));

                ;


        }
    }
}
