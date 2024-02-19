using ECommerce.Models.DTOs.Stripe;

namespace ECommerce.Services.StripeService;

public interface IStripeService
{
    Task<CustomerDto> CreateCustomer(CreateCustomerDto dto, CancellationToken cancellationToken);
    Task<ChargeDto> CreateCharge(CreateChargeDto dto, CancellationToken cancellationToken);
}