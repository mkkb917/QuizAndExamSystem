using ExamSystem.Controllers;
using ExamSystem.Data.Interface;
using ExamSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ExamSystem.Data.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailservice;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;    // for including WC constant paths

        //constructor
        public SubscriptionService(ILogger<AccountController> logger, IEmailService emailservice, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _emailservice = emailservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Subscription>> AllActiveSubscriptions(ApplicationUser user)
        {
            try
            {
                // Get all active subscriptions for the user
                var activeSubscriptions = await _context.Subscriptions
                    .Where(s => s.UserId == user.Id && s.IsActive)
                    .ToListAsync();

                // Handle the active subscriptions (e.g., send notifications)
                // ...
                return activeSubscriptions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active subscriptions.");
                return null;
            }
        }

        public async Task<List<Subscription>> AllCanceledSubscriptions(ApplicationUser user)
        {
            try
            {
                // Get all canceled subscriptions for the user
                var canceledSubscriptions = await _context.Subscriptions
                    .Where(s => s.UserId == user.Id && !s.IsActive)
                    .ToListAsync();

                // Handle the canceled subscriptions (e.g., send notifications)
                // ...
                return canceledSubscriptions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving canceled subscriptions.");
                return null;
            }
        }

        public async Task<List<Subscription>> AllExpiredSubscriptions(ApplicationUser user)
        {
            try
            {
                // Get all expired subscriptions for the user
                var expiredSubscriptions = await _context.Subscriptions
                    .Where(s => s.UserId == user.Id && !s.IsActive && s.EndDate < DateTime.UtcNow)
                    .ToListAsync();

                // Handle the expired subscriptions (e.g., send notifications)
                // ...
                return expiredSubscriptions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expired subscriptions.");
                return null;
            }
        }

        public async Task<List<Subscription>> AllSubscriptions(ApplicationUser user)
        {
            try
            {
                // Get all subscriptions for the user
                var subscriptions = await _context.Subscriptions
                    .Where(s => s.UserId == user.Id)
                    .ToListAsync();

                // Handle the subscriptions (e.g., send notifications)
                // ...
                return subscriptions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subscriptions.");
                return null;
            }
        }




        #region Subscriptions

        public async Task<Subscription> GetSubscriptionById(int subscriptionId)
        {
            try
            {
                var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
                if (subscription != null)
                {
                    return subscription;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subscription by ID.");
                return null;
            }
        }

        public async Task<bool> RenewSubscription(ApplicationUser user, Subscription subscription)
        {
            try
            {
                // Renew the subscription
                subscription.StartDate = DateTime.UtcNow;
                subscription.EndDate = DateTime.UtcNow.AddMonths(12);
                subscription.IsActive = true;

                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                return true; // Subscription renewed successfully
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renewing subscription.");
                return false;
            }
        }

        public async Task<bool> SubscribeUser(ApplicationUser user, int planId)
        {
            try
            {
                // Create a new subscription record
                var newSubscription = new Subscription
                {
                    User = user,
                    UserId = user.Id,
                    PlanId = planId,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(12),
                    IsActive = true,
                };

                // Add the new subscription to the database
                _context.Subscriptions.Add(newSubscription);
                await _context.SaveChangesAsync();

                return true; // Subscription created successfully
            }
            catch (Exception ex)
            {
                // Handle the exception
                _logger.LogError(ex, "Error subscribing user.");
                return false;
            }
        }

        public async Task<bool> SuspendSubscription(ApplicationUser user, Subscription subscription)
        {
            try
            {
                // Suspend the subscription
                subscription.IsActive = false;

                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                return true; // Subscription suspended successfully
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending subscription.");
                return false;
            }
        }


        public async Task<bool> UnSubscribeUser(ApplicationUser user, Subscription subscription)
        {
            try
            {
                // Cancel the subscription
                subscription.IsActive = false;

                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                return true; // Subscription canceled successfully
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling subscription.");
                return false;
            }
        }


        #endregion



        #region Plans
        public Task CreatePlan(Plan plan)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Plan>> GetPlans()
        {
            try
            {
                var plans = await _context.Plans.ToListAsync();
                if (plans != null)
                {
                    return plans;
                }
                else
                {
                    _logger.LogWarning($"Plans not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all plans");
                return null;
            }
        }
        public async Task<Plan?> GetPlanById(int planId)
        {
            try
            {
                var plan = await _context.Plans.FindAsync(planId);
                if (plan != null)
                {
                    return plan;
                }
                else
                {
                    _logger.LogWarning($"Plan with ID {planId} not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plan by ID.");
                return null;
            }
        }
        public async Task<bool> UpdatePlan(int planId, Plan updatedPlan)
        {
            try
            {
                var existingPlan = await _context.Plans.FindAsync(planId);

                if (existingPlan == null)
                {
                    _logger.LogWarning($"Plan with ID {planId} not found.");
                    return false;
                }

                existingPlan.Name = updatedPlan.Name;
                existingPlan.Price = updatedPlan.Price;
                existingPlan.Description = updatedPlan.Description;

                _context.Plans.Update(existingPlan);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating plan.");
                return false;
            }
        }

        
        public async Task<bool> DeletePlan(int planId)
        {
            try
            {
                var plan = await _context.Plans.FindAsync(planId);

                if (plan == null)
                {
                    _logger.LogWarning($"Plan with ID {planId} not found.");
                    return false;
                }

                _context.Plans.Remove(plan);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting plan.");
                return false;
            }
        }
        
        #endregion
    }
}
