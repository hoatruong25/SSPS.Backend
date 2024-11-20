using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helper.Paypal.Requests.CreateSubscriptionRequest;

namespace Helper.Paypal.Requests
{
    public class CreateSubscriptionRequest
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public string plan_id { get; set; }
        public string start_time { get; set; }
        public ShippingAmount shipping_amount { get; set; }
        public Subscriber subscriber { get; set; }
        public ApplicationContext application_context { get; set; }
    }

    public class Address
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class ApplicationContext
    {
        public string brand_name { get; set; }
        public string locale { get; set; }
        public string shipping_preference { get; set; }
        public string user_action { get; set; }
        public PaymentMethod payment_method { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }

    public class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
        public string full_name { get; set; }
    }

    public class PaymentMethod
    {
        public string payer_selected { get; set; }
        public string payee_preferred { get; set; }
    }
    public class ShippingAddress
    {
        public Name name { get; set; }
        public Address address { get; set; }
    }

    public class ShippingAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Subscriber
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
    }
}
