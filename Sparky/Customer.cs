using System;

namespace Sparky
{
    public class Customer
    {
        public int Discount { get; set; } = 10;
        public int OrderTotal { get; set; }
        public string GreetMessage { get; set; }
        public bool IsPlatinum { get; set; }
        public Customer()
        {
            IsPlatinum = false;
        }
        public string GreetAndCombineNames(string firstname, string lastname)
        {
            if (string.IsNullOrEmpty(firstname))
                throw new ArgumentException("Empty firstname");

            GreetMessage = $"Hello, {firstname} {lastname}";
            return GreetMessage;
        }

        public CustomerType GetCustomerDetails()
        {
            if (OrderTotal < 100)
                return new BasicCustomer();

            return new PlatinumCustomer();
        }
    }

    public class CustomerType { }
    public class BasicCustomer : CustomerType { }
    public class PlatinumCustomer : CustomerType { }
}
