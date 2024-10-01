using FluentValidation;
using Web.Api.Base.Enums;
using Web.Api.Base.Message;
using Web.Api.Schema;

namespace Web.Api.Business.Validation
{
    public class VpExpenseFormValidator : AbstractValidator<ExpenseFormRequest>
    {
        public VpExpenseFormValidator()
        {
            // Total amount 
            RuleFor(request => request.TotalAmount)
                .GreaterThan(0)
                .WithMessage(ErrorMessage.ValidationErrorMessage.TotalAmountError);

            // Currency 
            RuleFor(request => request.Currency)
                 .NotEmpty()
                 .WithMessage(ErrorMessage.ValidationErrorMessage.CurrencyRequieredError)
                 .Must(c => Enum.TryParse(typeof(CurrencyEnum), c, true, out _))
                 .WithMessage(ErrorMessage.ValidationErrorMessage.CurrencyEnumError);

          
            RuleForEach(request => request.Expenses).ChildRules(expense =>
            {
                expense.RuleFor(e => e.Amount)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(5000)
                    .WithMessage(ErrorMessage.ValidationErrorMessage.ExpenseAmountError);

                expense.RuleFor(e => e.Location)
                    .NotEmpty()
                    .WithMessage(ErrorMessage.ValidationErrorMessage.LocationError);


            });
        }
    }
}
