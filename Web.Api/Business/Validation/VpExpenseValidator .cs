using FluentValidation;
using Web.Api.Data.Entities;

namespace Web.Api.Business.Validation
{
    public class VpExpenseValidator : AbstractValidator<VpExpense>
    {
        public VpExpenseValidator()
        {
            RuleFor(expense => expense.Amount)
                .GreaterThan(0)
                .LessThanOrEqualTo(5000)
                .WithMessage("Amount must be between 0 and 5000.");
        }
    }

}
