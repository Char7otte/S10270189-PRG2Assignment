class DDJBFlight : Flight
{
    public double RequestFee { get; set; } = 300;

    public DDJBFlight(
    string flightNumber,
    string origin,
    string destination,
    DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
    {
    }

    public DDJBFlight(
        string flightNumber, 
        string origin, 
        string destination, 
        DateTime expectedTime, 
        string status) : base(flightNumber, origin, destination, expectedTime, status)
    {
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

