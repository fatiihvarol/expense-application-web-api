using MediatR;
using Web.Api.Base.Response;
using Web.Api.Schema.Authentication;
using Web.Api.Schema.Reports;

namespace Web.Api.Business.Cqrs
{

    public record GetPieChartReportQuery() : IRequest<ApiResponse<List<PieChartReportVM>>>;
    public record GetBarChartReportQuery() : IRequest<ApiResponse<List<BarChartReportVM>>>;
    public record GetStatusReportQuery() : IRequest<ApiResponse<List<StatusReportVM>>>;


}
