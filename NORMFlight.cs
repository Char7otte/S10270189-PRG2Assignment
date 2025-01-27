﻿//==========================================================
// Student Number	: S10259006
// Student Name	: Larry Chia
// Partner Name	: Huang Yangmile
//==========================================================

class NORMFlight : Flight
{
    public NORMFlight() : base() { }

    public NORMFlight(
    string flightNumber,
    string origin,
    string destination,
    DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
    {
    }

    public NORMFlight(
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
            return totalfee += 500;
        }
        else
        {
            return totalfee += 800;
        }
    }

    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}\n" +
               $"Origin: {Origin}\n" +
               $"Destination: {Destination}\n" +
               $"Expected Time: {ExpectedTime}\n" +
               $"Special Request Code: None";
    }
}


