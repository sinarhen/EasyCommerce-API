using ECommerce.Models.DTOs.Stripe;
using Stripe;

namespace ECommerce.Services.StripeService;

public class StripeService : IStripeService
{
    private readonly TokenService _tokenService;
    private readonly CustomerService _customerService;
    private readonly ChargeService _chargeService;
    
    public StripeService(
        TokenService tokenService, 
        CustomerService customerService, 
        ChargeService chargeService)
    {
        _tokenService = tokenService;
        _customerService = customerService;
        _chargeService = chargeService;
    }

    public async Task<CustomerDto> CreateCustomer(CreateCustomerDto resource, CancellationToken cancellationToken)
    {
        var tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = resource.Card.Name,
                Number = resource.Card.Number,
                ExpYear = resource.Card.ExpiryYear,
                ExpMonth = resource.Card.ExpiryMonth,
                Cvc = resource.Card.Cvc
            }
        };
        var token = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

        var customerOptions = new CustomerCreateOptions
        {
            Email = resource.Email,
            Name = resource.Name,
            Source = token.Id
        };
        var customer = await _customerService.CreateAsync(customerOptions, null, cancellationToken);

        return new CustomerDto(customer.Id, customer.Email, customer.Name);
    }

    public async Task<ChargeDto> CreateCharge(CreateChargeDto resource, CancellationToken cancellationToken)
    {
        var chargeOptions = new ChargeCreateOptions
        {
            Currency = resource.Currency,
            Amount = resource.Amount,
            ReceiptEmail = resource.ReceiptEmail,
            Customer = resource.CustomerId,
            Description = resource.Description
        };

        var charge = await _chargeService.CreateAsync(chargeOptions, null, cancellationToken);

        return new ChargeDto(
            charge.Id, 
            charge.Currency, 
            charge.Amount, 
            charge.CustomerId, 
            charge.ReceiptEmail,
            charge.Description);
    }
}