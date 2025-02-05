﻿//==========================================================
// Student Number	: S10259006
// Student Name	: Larry Chia
// Partner Name	: Huang Yangmile
//==========================================================

abstract class Flight :IComparable<Flight>
{
    public string FlightNumber { get; set; }

    public string Origin { get; set; }

    public string Destination { get; set; }

    public DateTime ExpectedTime { get; set; }

    public string Status { get; set; } = "On Time";

    public Flight() { }

    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
    }

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
        return $"Flight Number: {FlightNumber}\n" +
               $"Origin: {Origin}\n" +
               $"Destination: {Destination}\n" +
               $"Expected Time: {ExpectedTime}\n";
    }

    public int CompareTo(Flight other)
    {
        return ExpectedTime.CompareTo(other.ExpectedTime);
    }
}

