using Suffer_Travels.Models;

namespace Suffer_Travels.Service
{
    public interface IPaymentService
    {
        Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest);
        Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor);
    }
}
