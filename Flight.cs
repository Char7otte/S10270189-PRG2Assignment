
abstract class Flight
{
    public string FlightNumber { get; set; }

    public string Origin { get; set; }

    public string Destination { get; set; }

    public DateTime ExpectedTime { get; set; }

    public string Status { get; set; } = "On Time";

    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        Status = status;
    }

    public virtual double CalculateFees()
    {
        double totalfee = 0;
        if (Destination == "Singapore (SIN)")
        {
            return totalfee += 500;
        }
        else
        {
            return totalfee += 800;
        }
    }
    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}\nOrigin: {Origin}\nDestination: {Destination}\nExpected Time: {ExpectedTime}\nStatus: {Status}";
    }
}

