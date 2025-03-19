using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required.");

        RuleFor(sale => sale.Customer)
            .NotEmpty()
            .WithMessage("Customer is required.");

        RuleFor(sale => sale.Branch)
            .NotEmpty()
            .WithMessage("Branch is required.");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("At least one sale item is required.");

        RuleForEach(sale => sale.Items).ChildRules(items =>
        {
            items.RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            items.RuleFor(item => item.Quantity)
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot sell more than 20 identical items.");

            items.RuleFor(item => item)
                .Must(item => !(item.Quantity < 4) || (item.Quantity < 4 && item.Discount == 0))
                .WithMessage("Purchases below 4 items cannot have a discount.");


            items.RuleFor(item => item)
                .Must(item => !(item.Quantity >= 4 && item.Quantity <= 10) || (item.Quantity >= 4 && item.Quantity <= 10 && item.Discount == 10))
                .WithMessage("Purchases between 4 and 10 identical items must have a 10% discount.");

            items.RuleFor(item => item)
                .Must(item => !(item.Quantity >= 11 && item.Quantity <= 20) || (item.Quantity >= 11 && item.Quantity <= 20 && item.Discount == 20))
                .WithMessage("Purchases between 10 and 20 identical items must have a 20% discount.");

        });
    }
}
