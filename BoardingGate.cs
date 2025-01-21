//==========================================================
// Student Number	: S0270189
// Student Name	: Huang Yangmile
// Partner Name	: Larry Chia
//==========================================================

class BoardingGate
{
    public string GetName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsWTT { get; set; }
    public Flight Flight { get; set; }

    public BoardingGate() { }

    public BoardingGate(string name, bool supportsCFFT, bool supportsDDJB, bool supportsWTT, Flight flight)
    {
        GetName = name;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsWTT = supportsWTT;
        Flight = flight;
    }

    public double CalculateFees()
    {
        return 0.0;
    }

    public override string ToString()
    {
        return base.ToString();
    }

}