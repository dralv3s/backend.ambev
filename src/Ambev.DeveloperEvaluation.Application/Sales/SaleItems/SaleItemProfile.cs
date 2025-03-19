using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleItems;

public class SaleItemProfile : Profile
{
    public SaleItemProfile()
    {
        CreateMap<SaleItem, SaleItemResult>().ReverseMap();
    }
}
