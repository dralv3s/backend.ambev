using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

public class GetAllSalesCommand : IRequest<List<GetAllSalesResult>>
{
}
