using Web.Api.Base.Enums;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;

namespace Web.Api.Business.Helper
{
    public class ExpenseFormHistoryHelper
    {
      

        public async Task AddHistoryLog(int expenseFormId, string madeBy, HistoryActionEnum action, CancellationToken cancellationToken,AppDbContext appDbContext)
        {
            var historyLog = new VpExpenseFormHistory
            {
                ExpenseFormId = expenseFormId,
                MadeBy = madeBy,
                Action = action,
                Date = DateTime.Now
            };

            appDbContext.VpExpenseFormHistories.Add(historyLog);
            await appDbContext.SaveChangesAsync();
        }
    }

}
