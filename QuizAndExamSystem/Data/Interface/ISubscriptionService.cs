using ExamSystem.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ExamSystem.Data.Interface
{
    public interface ISubscriptionService
    {
        
        //get all subscriptions
        Task<List<Subscription>> AllSubscriptions(ApplicationUser user);
        //get all active subscriptions
        Task<List<Subscription>> AllActiveSubscriptions(ApplicationUser user);
        //get all expired subscriptions
        Task<List<Subscription>> AllExpiredSubscriptions(ApplicationUser user);
        //get all canceled subscriptions
        Task<List<Subscription>> AllCanceledSubscriptions(ApplicationUser user);

        #region Subscription
        //create the new  subscription
        Task<bool> SubscribeUser(ApplicationUser user, int planId);
        //Renew he subscription
        Task<bool> RenewSubscription(ApplicationUser user, Subscription subscription);
        //suspend the subscription
        Task<bool> SuspendSubscription(ApplicationUser user, Subscription subscription);
        //cancel the subscription
        Task<bool> UnSubscribeUser(ApplicationUser user, Subscription subscription);
        Task<Subscription> GetSubscriptionById(int subscriptionId);
        #endregion

        #region Plan
        Task<Plan?> GetPlanById(int planId);
        Task<List<Plan>> GetPlans();
        Task CreatePlan(Plan plan);
        Task<bool> UpdatePlan(int planId, Plan updatedPlan);
        Task<bool> DeletePlan(int planId);

        #endregion
    }
}