using Helper.Paypal.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Paypal.Responses
{
    public class GetPlanDetailsResponse
    {
        public string id { get; set; }
        public string product_id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string usage_type { get; set; }
        public List<BillingCycle> billing_cycles { get; set; }
        public PaymentPreferences payment_preferences { get; set; }
        public Taxes taxes { get; set; }
        public bool quantity_supported { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public List<Link> links { get; set; }
    }
}
