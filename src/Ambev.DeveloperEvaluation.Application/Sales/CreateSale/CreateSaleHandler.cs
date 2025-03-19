using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        foreach (var item in command.Items)
        {
            if (item.Quantity > 20)
                throw new ValidationException($"Cannot sell more than 20 identical items for product {item.Product}");

            item.CalculateTotalAmount();
        }

        var sale = _mapper.Map<Sale>(command);
        sale.CalculateTotalSaleAmount();
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);

        PublishEvent("SaleCreated", sale);

        return result;
    }

    private void PublishEvent(string eventName, Sale sale)
    {
        // Placeholder method for publishing events to some message broker
        _logger.Information("SaleCreated event logged for Sale: {SaleNumber}", sale.SaleNumber);
    }
}
