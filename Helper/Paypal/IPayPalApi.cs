using Helper.Paypal.Requests;
using Helper.Paypal.Responses;

namespace Helper.Paypal
{
    public interface IPayPalApi
    {
        Task<CreatePlanPayPalResponse?> CreatePlan(CreatePlanRequest request);
        Task<GetPlanDetailsResponse?> GetPlanDetails(string planId);
        Task<CreateSubscriptionResponse?> CreateSubscrition(CreateSubscriptionRequest request);
    }
}
