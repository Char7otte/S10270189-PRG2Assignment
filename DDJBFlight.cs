using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270189_PRG2Assignment
{
    internal class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 300;
        }

        public override double CalculateFees()
        {
            double totalfee = 0;
            if (Destination == "Singapore (SIN)")
            {
                return totalfee += 500 + RequestFee;
            }
            else
            {
                return totalfee += 800 + RequestFee;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
