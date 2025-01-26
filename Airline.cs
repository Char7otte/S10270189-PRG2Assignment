//==========================================================
// Student Number	: S10270189
// Student Name	: Huang Yangmile
// Partner Name	: Larry Chia
//==========================================================

class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline() { }
    public Airline(string airlineName)
    {
        Flights = new Dictionary<string, Flight>();
    }

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public Airline(string name, string code, Dictionary<string, Flight> flights)
    {
        Name = name;
        Code = code;
        Flights = flights;
    }

    public bool AddFlight(Flight flight)
    {
        return true;
    }

    public double CalculateFees()
    {
        return 0.0;
    }

    public bool RemoveFlight(Flight flight)
    {
        return true;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
