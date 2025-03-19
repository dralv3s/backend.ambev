using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        foreach (var item in command.Items)
        {
            if (item.Quantity > 20)
                throw new ValidationException($"Cannot sell more than 20 identical items for product {item.Product}");

            item.CalculateTotalAmount();
        }

        var sale = _mapper.Map<Sale>(command);
        sale.CalculateTotalSaleAmount();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        PublishEvent("SaleModified", sale);

        return result;
    }

    private void PublishEvent(string eventName, Sale sale)
    {
        // Placeholder method for publishing events to some message broker
        _logger.Information("SaleModified event logged for Sale: {SaleNumber}", sale.SaleNumber);
    }
}
