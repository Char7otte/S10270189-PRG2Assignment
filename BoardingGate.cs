﻿//==========================================================
// Student Number	: S10270189
// Student Name	: Huang Yangmile
// Partner Name	: Larry Chia
//==========================================================

class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    public BoardingGate() { }

    public BoardingGate(string name, bool supportsCFFT, bool supportsDDJB, bool supportsWTT, Flight flight)
    {
        GateName = name;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsWTT;
        Flight = flight;
    }

    public double CalculateFees()
    {
        return 0.0;
    }

    public override string ToString()
    {
        return $"Boarding Gate Name: {GateName}\n" +
               $"Supports DDJB: {SupportsDDJB}\n" +
               $"Supports CFFT: {SupportsCFFT}\n" +
               $"Supports LWTT: {SupportsLWTT}";
    }

}