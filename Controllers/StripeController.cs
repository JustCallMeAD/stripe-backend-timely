using Microsoft.AspNetCore.Mvc;
using Stripe.Terminal;

namespace Stripe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Stripe : ControllerBase
    {
        public Stripe()
        {
            StripeConfiguration.ApiKey = "sk_test_mWQrgyxyh3ac7Khv1hy86DAZ00Yv2aXf7q";
        }

        [Route("connection_token")]
        [HttpPost]
        public JsonResult ConnectionToken()
        {
            var options = new ConnectionTokenCreateOptions { };
            var service = new ConnectionTokenService();
            var connectionToken = service.Create(options);
            Console.WriteLine(connectionToken);
            return new JsonResult(new { secret = connectionToken.Secret });
        }

        [Route("create_payment_intent")]
        [HttpPost]
        public JsonResult CreatePaymentIntent()
        {
            StripeConfiguration.ApiKey = "sk_test_mWQrgyxyh3ac7Khv1hy86DAZ00Yv2aXf7q";

            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1000,
                Currency = "nzd",
                PaymentMethodTypes = new List<string> { "card_present" },
                CaptureMethod = "manual",
            };

            var intent = service.Create(options);
            //var intent = // ... Fetch or create the PaymentIntent
            return new JsonResult(intent);
        }

        [Route("confirm_payment_intent")]
        [HttpPost]
        public JsonResult ConfirmPaymentIntent(ConfirmPaymentIntentRequest? paymentIntent)
        {
            var service = new PaymentIntentService();
            service.Capture(paymentIntent?.PaymentIntentId);
            return new JsonResult(new { });
        }
    }

    public class ConfirmPaymentIntentRequest
    {
        public string? PaymentIntentId { get; set; }
    }
}