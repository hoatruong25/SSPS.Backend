using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Paypal.Requests
{
    public class CreatePlanRequest
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public string product_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public List<BillingCycle> billing_cycles { get; set; }
        public PaymentPreferences payment_preferences { get; set; }
        public Taxes taxes { get; set; }

    }

    public class BillingCycle
    {
        public Frequency frequency { get; set; }
        public string tenure_type { get; set; }
        public int sequence { get; set; }
        public int total_cycles { get; set; }
        public PricingScheme pricing_scheme { get; set; }
    }

    public class FixedPrice
    {
        public string value { get; set; }
        public string currency_code { get; set; }
    }

    public class Frequency
    {
        public string interval_unit { get; set; }
        public int interval_count { get; set; }
    }

    public class PaymentPreferences
    {
        public bool auto_bill_outstanding { get; set; }
        public SetupFee setup_fee { get; set; }
        public string setup_fee_failure_action { get; set; }
        public int payment_failure_threshold { get; set; }
    }

    public class PricingScheme
    {
        public FixedPrice fixed_price { get; set; }
    }

    public class SetupFee
    {
        public string value { get; set; }
        public string currency_code { get; set; }
    }

    public class Taxes
    {
        public string percentage { get; set; }
        public bool inclusive { get; set; }
    }
}
