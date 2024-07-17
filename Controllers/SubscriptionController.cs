using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinnanciaCSharp.Extensions;
using DotNetEnv;
using Stripe;

namespace FinnanciaCSharp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly string _apiKey;
        private readonly string _returnUrl;
        public SubscriptionController(UserManager<User> userManager)
        {
            Env.Load();
            _userManager = userManager;
            _apiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY")!;
            _returnUrl = $"{Environment.GetEnvironmentVariable("CLIENT_URL")!}/plans";
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptionUrl()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userManager.FindByIdAsync(userId == null ? "" : userId);

                if (user == null || userId == null)
                {
                    return Unauthorized(new { error = "Não autorizado" });
                }

                //TODO: User subscription
                // const userSubscription = await getUserSubscription(user.id);
                // if (userSubscription && userSubscription.stripeCustomerId) {
                // 	const stripeSession = await stripe.billingPortal.sessions.create({
                // 		customer: userSubscription.stripeCustomerId,
                // 		return_url: returnUrl,
                // 	});

                // 	return NextResponse.json({ url: stripeSession.url });
                // }

                StripeConfiguration.ApiKey = _apiKey;
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    Mode = "subscription",
                    PaymentMethodTypes = new List<string> { "card" },
                    CustomerEmail = user.Email,
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            Quantity = 1,
                            PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                            {
                                Currency = "BRL",
                                UnitAmount = 990,
                                ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Finnancia Pro",
                                    Description = "Tenha acesso à todo o potencial do Finnancia!",
                                },
                                Recurring = new Stripe.Checkout.SessionLineItemPriceDataRecurringOptions
                                {
                                    Interval = "month"
                                }
                            }
                        },
                    },
                    Metadata = new Dictionary<string, string>{
                        { "userId", user.Id }
                    },
                    SuccessUrl = _returnUrl,
                    CancelUrl = _returnUrl
                };
                var service = new Stripe.Checkout.SessionService();

                var res = await service.CreateAsync(options);

                return Ok(new { url = res.Url });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}