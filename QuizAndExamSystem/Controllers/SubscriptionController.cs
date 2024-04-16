using ExamSystem.Data.Interface;
using ExamSystem.Data;
using ExamSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Data.Services;

namespace ExamSystem.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionService _subscriptionService;
        
        //constructor
        public SubscriptionController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, ISubscriptionService subscriptionService)
        {
            _logger = logger;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
        }

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(int planId)
        {
            try
            {
                var user = new ApplicationUser()
                { UserName = _userManager.GetUserId(User) }; // Get the current user's ID
                var plan =  _subscriptionService.GetPlanById(planId);  // Implement a method in SubscriptionService to get a Plan by Id

                if (user == null || plan == null)
                {
                    return NotFound("User or plan not found.");
                }

                var result = await _subscriptionService.SubscribeUser(user, planId);

                if (result)
                {
                    return Ok("User subscribed successfully.");
                }
                else
                {
                    return BadRequest("Error subscribing user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing user.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("UnSubscribeUser")]
        public async Task<IActionResult> CancelSubscription(string userId, int subscriptionId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var subscription = await _subscriptionService.GetSubscriptionById(subscriptionId);  // Implement a method in SubscriptionService to get a Subscription by Id

                if (user == null || subscription == null)
                {
                    return NotFound("User or subscription not found.");
                }

                var result = await _subscriptionService.UnSubscribeUser(user, subscription);

                if (result)
                {
                    return Ok("Subscription canceled successfully.");
                }
                else
                {
                    return BadRequest("Error canceling subscription.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling subscription.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("RenewSubscription")]
        public async Task<IActionResult> RenewSubscription(string userId, int subscriptionId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var subscription = await _subscriptionService.GetSubscriptionById(subscriptionId);  // Implement a method in SubscriptionService to get a Subscription by Id

                if (user == null || subscription == null)
                {
                    return NotFound("User or subscription not found.");
                }

                var result = await _subscriptionService.RenewSubscription(user, subscription);

                if (result)
                {
                    return Ok("Subscription renewed successfully.");
                }
                else
                {
                    return BadRequest("Error renewing subscription.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renewing subscription.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("SuspendSubscription")]
        public async Task<IActionResult> SuspendSubscription(string userId, int subscriptionId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var subscription = await _subscriptionService.GetSubscriptionById(subscriptionId);  // Implement a method in SubscriptionService to get a Subscription by Id

                if (user == null || subscription == null)
                {
                    return NotFound("User or subscription not found.");
                }

                var result = await _subscriptionService.SuspendSubscription(user, subscription);

                if (result)
                {
                    return Ok("Subscription suspended successfully.");
                }
                else
                {
                    return BadRequest("Error suspending subscription.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suspending subscription.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("AllSubscriptions")]
        public async Task<IActionResult> AllSubscriptions(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var subscriptions = await _subscriptionService.AllSubscriptions(user);

                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subscriptions.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("AllActiveSubscriptions")]
        public async Task<IActionResult> AllActiveSubscriptions(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var activeSubscriptions = await _subscriptionService.AllActiveSubscriptions(user);

                return Ok(activeSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active subscriptions.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("AllExpiredSubscriptions")]
        public async Task<IActionResult> AllExpiredSubscriptions(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var expiredSubscriptions = await _subscriptionService.AllExpiredSubscriptions(user);

                return Ok(expiredSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expired subscriptions.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("AllCanceledSubscriptions")]
        public async Task<IActionResult> AllCanceledSubscriptions(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var canceledSubscriptions = await _subscriptionService.AllCanceledSubscriptions(user);

                return Ok(canceledSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving canceled subscriptions.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
