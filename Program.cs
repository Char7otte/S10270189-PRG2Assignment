﻿/////////////////////////////////////////////////////////////////////////
/// S10270189J Huang Yangmile: Features 2, 3, 5, 6, 9, Advanced (a) ///
/// S10259006 Larry Chia: Features 1, 4, 7, 8, Advanced (b)         ///
/////////////////////////////////////////////////////////////////////////

using System.Globalization;

bool loopContinue = true; //TO BE FOR USE IN WHILE (TRUE) LOOPS IF THERE IS A
                          //NESTED SWITCH STATEMENT OR WTV THAT PREVENTS CONTINUE FROM BEING USED



//Feature #1: Load airline and boarding gate data from file (LARRY CHIA)
Dictionary<string, Airline> airlinesDict = new();
Dictionary<string, BoardingGate> boardingGatesDict = new();
void LoadAirlineAndBoardingGateData(
    Dictionary<string, Airline> airlinesDict,
    Dictionary<string, BoardingGate> boardingGatesDict,
    StreamReader sr,
    StreamReader sr2)
{
    using (sr)
    {
        Console.WriteLine("Loading Airlines...");

        sr.ReadLine(); //Skip header line

        string line = "";
        int lineCount = 0;

        while ((line = sr.ReadLine()) != null)
        {
            lineCount++;

            //Read the line
            string airlineName = line.Split(',')[0];
            string airlineCode = line.Split(',')[1];

            //Create a new airline object
            Airline airline = new(airlineName, airlineCode);
            airlinesDict.Add(airlineCode, airline);

        }
        Console.WriteLine($"{lineCount} Airlines Loaded!");
    }
    using (sr2)
    {
        Console.WriteLine("Loading Boarding Gates...");

        sr2.ReadLine(); //Skip header line

        string line = "";
        int lineCount = 0;

        while ((line = sr2.ReadLine()) != null)
        {
            lineCount++;
            //Read the line
            string boardingGate = line.Split(',')[0];
            bool DDJBBool = bool.Parse(line.Split(',')[1]);
            bool CFFTBool = bool.Parse(line.Split(',')[2]);
            bool LWTTBool = bool.Parse(line.Split(',')[3]);

            //Create a new boarding gate object
            BoardingGate boardingGateObj = new(boardingGate, DDJBBool, CFFTBool, LWTTBool);
            boardingGatesDict.Add(boardingGate, boardingGateObj);
        }
        Console.WriteLine($"{lineCount} Boarding Gates Loaded!");
    }
}


//Feature #2: Load Flights from file (HUANG YANGMILE)
Dictionary<string, Flight> flightsDict = new();
void LoadFlights(Dictionary<string, Flight> flightsDict, StreamReader sr)
{
    using (sr)
    {
        Console.WriteLine("Loading Flights...");

        sr.ReadLine(); //Skip header line

        string line;
        int lineCount = 0;

        while ((line = sr.ReadLine()) != null)
        {
            lineCount++;

            //Read the line
            string[] spitLine = line.Split(',');
            string flightNumber = spitLine[0];
            string origin = spitLine[1];
            string destination = spitLine[2];
            DateTime expectedTime = DateTime.Parse(spitLine[3]);
            string requestCode = spitLine[4];

            //Create a new flight object
            //Request Codes: LWTT, DDJB, CFFT
            if (requestCode == "")
            {
                NORMFlight newNormFlight = new(flightNumber, origin, destination, expectedTime);
                flightsDict.Add(flightNumber, newNormFlight);
            }
            else if (requestCode == "LWTT")
            {
                LWTTFlight newLWTTFlight = new(flightNumber, origin, destination, expectedTime);
                flightsDict.Add(flightNumber, newLWTTFlight);
            }
            else if (requestCode == "DDJB")
            {
                DDJBFlight newDDJBFlight = new(flightNumber, origin, destination, expectedTime);
                flightsDict.Add(flightNumber, newDDJBFlight);
            }
            else if (requestCode == "CFFT")
            {
                CFFTFlight newCFFTFlight = new(flightNumber, origin, destination, expectedTime);
                flightsDict.Add(flightNumber, newCFFTFlight);
            }
            else
            {
                Console.WriteLine($"ERROR WHILE CREATING FLIGHT OBJECT, SOMETHING WENT WRONG WITH FLIGHT {flightNumber} WITH REQUEST CODE {requestCode}");
            }
        }

        Console.WriteLine($"{lineCount} Flights Loaded!");
    }
}

//Feature #3: List Flights (HUANG YANGMILE)
Dictionary<string, string> specialRequestFlightsDict = new();
void ListFlights(Dictionary<string, Flight> flightsDict, Dictionary<string, Airline> airlinesDict)
{
    Console.WriteLine("" +
                      "=============================================\r\n" +
                      "List of Flights for Changi Airport Terminal 5\r\n" +
                      "=============================================");


    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}{4, -20}";

    Console.WriteLine(stringFormat, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        Flight flight = kvp.Value;
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateTime expectedTime = flight.ExpectedTime;

        string airlineCode = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "ERROR";

        if (airlinesDict.ContainsKey(airlineCode))
        {
            airlineName = airlinesDict[airlineCode].Name;
        }

        Console.WriteLine(stringFormat, flightNumber, airlineName, origin, destination, expectedTime);
    }
}

// Feature #4: List Boarding Gates (LARRY CHIA)
void ListBoardingGates(Dictionary<string, BoardingGate> boardingGatesDict)
{
    Console.WriteLine("=============================================\n" +
                      "List of Boarding Gates\n" +
                      "=============================================");
    
    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20} {4,-20}";
    Console.WriteLine(stringFormat, "Boarding Gate", "DDJB", "CFFT", "LWTT", "Flight Number");
    foreach (var kvp in boardingGatesDict)
    {
        BoardingGate gate = kvp.Value;

        if (gate.Flight == null)
        {
            Console.WriteLine(stringFormat, gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT, "");
        }
        else
        {
            Console.WriteLine(stringFormat, gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT,
                gate.Flight.FlightNumber);
        }
    }
}

//Feature #5: Assign boarding gate to flight (HUANG YANGMILE)
void AssignGateToFlight(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGatesDict)
{
    Console.WriteLine("=============================================\n" +
                      "Assign a Boarding Gate to a Flight\n" +
                      "=============================================");

    string flightNumber = "";
    string boardingGateName = "";

    while (true)
    {
        flightNumber = InputForString("Enter Flight Number:").ToUpper();

        if (!flightsDict.ContainsKey(flightNumber))
        {
            Console.WriteLine("FLIGHT NUMBER NOT FOUND!");
            Console.ReadLine();
            continue;
        }

        break;
    }

    while (true) 
    {
        boardingGateName = InputForString("Enter Boarding Gate Name:").ToUpper();
        
        if (!boardingGatesDict.ContainsKey(boardingGateName))
        {
            Console.WriteLine("BOARDING GATE NOT FOUND!");
            Console.ReadLine();
            continue;
        }

        break;
    }
    
    Flight flight = flightsDict[flightNumber];
    BoardingGate boardingGate = boardingGatesDict[boardingGateName];
    boardingGate.Flight = flight;
    
    Console.WriteLine(flight);
    Console.WriteLine(boardingGate);
    
    loopContinue = true;
    while (loopContinue)
    {
        string stringInput = InputForString("Would you like to update the status of the flight? (Y/N)").ToUpper();

        switch (stringInput)
        {
            case "N":
                Console.WriteLine($"Flight {flightNumber} has been assigned to Boarding Gate {boardingGate.GateName}!");
                return;
            case "Y":
                loopContinue = false;
                continue;
            default:
                Console.WriteLine("Please choose Y or N.");
                Console.ReadLine();
                break;
        }
    }

    loopContinue = true;
    while (loopContinue)
    {
        Console.WriteLine("1. Delayed\n" +
                          "2. Boarding\n" +
                          "3. On Time");
        int intInput = InputForInt("Please select the new status of the flight:", "how");
        
        switch (intInput)
        {
            case 1:
                loopContinue = false;
                flight.Status = "Delayed";
                break;
            case 2:
                loopContinue = false;
                flight.Status = "Boarding";
                break;
            case 3:
                loopContinue = false;
                flight.Status = "On Time";
                break;
            default:
                Console.WriteLine("Please select one of the 3 options.");
                Console.ReadLine();
                break;
        }
    }
    
    Console.WriteLine($"Flight {flightNumber} has been set to '{flight.Status}'.");
}


//Feature #6: Create a new flight (HUANG YANGMILE)
void CreateNewFlight(Dictionary<string, Flight> flightsDict)
{
    loopContinue = true;
    while (loopContinue)
    {
        string flightNumber = "";
        while (true)
        {
            try
            {
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine().ToUpper();

                Console.Write("Enter Origin: ");
                string origin = Console.ReadLine();

                Console.Write("Enter Destination: ");
                string destination = Console.ReadLine();

                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string input = Console.ReadLine();
                string format = "d/M/yyyy H:mm";
                DateTime departureTime = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);

                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string specialRequestCode = Console.ReadLine().ToUpper();

                if (specialRequestCode == "NONE")
                {
                    NORMFlight newNORMFlight = new NORMFlight(flightNumber, origin, destination, departureTime);
                    flightsDict.Add(flightNumber, newNORMFlight);
                    break;
                }
                if (specialRequestCode == "DDJB")
                {
                    DDJBFlight newDDJBFlight = new DDJBFlight(flightNumber, origin, destination, departureTime);
                    flightsDict.Add(flightNumber, newDDJBFlight);
                    break;
                }
                if (specialRequestCode == "CFTT")
                {
                    CFFTFlight newCFFTFlight = new CFFTFlight(flightNumber, origin, destination, departureTime);
                    flightsDict.Add(flightNumber, newCFFTFlight);
                    break;
                }
                if (specialRequestCode == "LWTT")
                {
                    LWTTFlight newLWTTFlight = new LWTTFlight(flightNumber, origin, destination, departureTime);
                    flightsDict.Add(flightNumber, newLWTTFlight);
                    break;
                }

                throw new Exception("Invalid request code!");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "Please try again!");
                Console.ReadLine();
            }
        }
        
        Console.WriteLine($"Flight {flightNumber} has been added!");
        
        while (true)
        {
            string addAnotherFlight = InputForString("Would you like to add another flight? (Y/N)").ToUpper();

            if (addAnotherFlight == "Y")
            {
                break;
            }
            if (addAnotherFlight == "N")
            {
                loopContinue = false;
                break;
            }
        
            Console.WriteLine("Invalid input. Please try again.");
            Console.ReadLine();
        }
        
    }
}


//Feature #7: Display full flight details from an airline (LARRY CHIA)
void DisplayFullDetailsFromAirline()
{
    Console.WriteLine("=============================================\n" +
                      "Display Full Flight Details from an Airline\n" +
                      "=============================================");

    Console.WriteLine("Airline Code     Airline Name");
    foreach (KeyValuePair<string, Airline> keyValuePair in airlinesDict) 
    {
        Airline airline = keyValuePair.Value;
        Console.WriteLine($"{airline.Code,-17}{airline.Name}");
    }

    string airlineCode = InputForString("Enter Airline Code:", "how did you mess up the airline code").ToUpper();
    if (!airlinesDict.ContainsKey(airlineCode))
    {
        Console.WriteLine("AIRLINE CODE NOT FOUND!");
        Console.ReadLine();
        return;
    }
    Console.WriteLine("=============================================\n" +
                      "List of Flights for Changi Airport Terminal 5\n" +
                      "=============================================");
    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}{4, -20}\n{5, -20}";
    Console.WriteLine(stringFormat, "Flight Number", "Airline Name", "Origin", "Destination", "Expected",
        "Departure/Arrival Time");
    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        Flight flight = kvp.Value;
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateOnly date = DateOnly.FromDateTime(flight.ExpectedTime);
        TimeOnly time = TimeOnly.FromDateTime(flight.ExpectedTime);
        string airlineCode2 = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "ERROR";
        if (airlinesDict.ContainsKey(airlineCode2))
        {
            airlineName = airlinesDict[airlineCode2].Name;
        }
        if (airlineCode2 == airlineCode)
        {
            string formattedTime = time.ToString("hh:mm:ss tt");
            Console.WriteLine(stringFormat, flightNumber, airlineName, origin, destination, date, formattedTime);
        }
    }
}

//Feautre #8 : Modify flight details (LARRY CHIA)
void ModifyFlightDetails()
{
    Console.WriteLine("=============================================\n" +
                  "Display Full Flight Details from an Airline\n" +
                  "=============================================");
    Console.WriteLine("Airline Code     Airline Name");
    foreach (KeyValuePair<string, Airline> keyValuePair in airlinesDict)
    {
        Airline airline = keyValuePair.Value;
        Console.WriteLine($"{airline.Code,-17}{airline.Name}");
    }

    string airlineCode = InputForString("Enter Airline Code:", "how did you mess up the airline code").ToUpper();
    if (!airlinesDict.ContainsKey(airlineCode))
    {
        Console.WriteLine("AIRLINE CODE NOT FOUND!");
        Console.ReadLine();
        return;
    }

    string AirlineName = airlinesDict[airlineCode].Name;
    Console.WriteLine($"List of Flights for {AirlineName}");

    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}{4, -20}\n{5, -20}";
    Console.WriteLine(stringFormat, "Flight Number", "Airline Name", "Origin", "Destination", "Expected",
        "Departure/Arrival Time");
    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        Flight flight = kvp.Value;
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateOnly date = DateOnly.FromDateTime(flight.ExpectedTime);
        TimeOnly time = TimeOnly.FromDateTime(flight.ExpectedTime);
        string airlineCode2 = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "ERROR";
        if (airlinesDict.ContainsKey(airlineCode2))
        {
            airlineName = airlinesDict[airlineCode2].Name;
        }
        if (airlineCode2 == airlineCode)
        {
            string formattedTime = time.ToString("hh:mm:ss tt");
            Console.WriteLine(stringFormat, flightNumber, airlineName, origin, destination, date, formattedTime);
        }
    }

    Console.Write("Choose an existing Flight to modify or delete: ");
    string FlightNumber = Console.ReadLine().ToUpper();
    if (!flightsDict.ContainsKey(FlightNumber))
    {
        Console.WriteLine("Flight Number not found!");
        return;
    }
    Console.WriteLine("1. Modify Flight\n" +
                      "2. Delete Flight\n" +
                      "Choose an option: ");
    int choice = int.Parse(Console.ReadLine());

    string newRequestCode = string.Empty; //To store the new Special Request Code
    if (choice == 1)
    {
        Flight flight = flightsDict[FlightNumber];
        Console.WriteLine("1. Modify Basic Information\n" +
                          "2. Modify Status\n" +
                          "3. Modify Special Request Code\n" +
                          "4. Modify Boarding Gate\n" +
                          "Choose an option: ");
        int choice2 = int.Parse(Console.ReadLine());
        switch (choice2)
        {
            case 1:
                try
                {
                    Console.Write("Enter new Origin: ");
                    string newOrigin = Console.ReadLine();
                    flight.Origin = newOrigin;

                    Console.Write("Enter new Destination: ");
                    string newDestination = Console.ReadLine();
                    flight.Destination = newDestination;

                    Console.Write("Enter new Expected Time (dd/mm/yyyy hh:mm): ");
                    string input = Console.ReadLine();
                    string format = "d/M/yyyy H:mm";
                    DateTime newExpectedTime = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture); //Tells the parser to use standard formats regardless of the computer's regional settings
                    flight.ExpectedTime = newExpectedTime;

                    Console.WriteLine("Flight updated!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error updating flight information: " + e.Message);
                }
                break;
            case 2:
                try
                {
                    Console.Write("Enter new Status: ");
                    string newStatus = Console.ReadLine();
                    flight.Status = newStatus;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error updating flight status: " + e.Message);
                }
                break;
            case 3:
                Console.Write("Enter new Special Request Code: ");
                newRequestCode = Console.ReadLine().ToUpper();
                if (newRequestCode == "NONE")
                {
                    NORMFlight newNormFlight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                    flightsDict[FlightNumber] = newNormFlight;
                }
                else if (newRequestCode == "LWTT")
                {
                    LWTTFlight newLWTTFlight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                    flightsDict[FlightNumber] = newLWTTFlight;
                }
                else if (newRequestCode == "DDJB")
                {
                    DDJBFlight newDDJBFlight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                    flightsDict[FlightNumber] = newDDJBFlight;
                }
                else if (newRequestCode == "CFFT")
                {
                    CFFTFlight newCFFTFlight = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
                    flightsDict[FlightNumber] = newCFFTFlight;
                }
                else
                {
                    Console.WriteLine("Invalid Special Request Code!");
                }
                break;
            case 4:
                Console.Write("Enter new Boarding Gate: ");
                string newBoardingGate = Console.ReadLine().ToUpper();
                if (!boardingGatesDict.ContainsKey(newBoardingGate))
                {
                    Console.WriteLine("Boarding Gate not found!");
                    break;
                }
                boardingGatesDict[newBoardingGate].Flight = flight;
                Console.WriteLine("Boarding Gate updated!");
                break;
            default:
                Console.WriteLine("Invalid input!");
                break;
        }
        Console.WriteLine("Flight Number: " + FlightNumber);
        Console.WriteLine("Airline Name: " + airlinesDict[airlineCode].Name);
        Console.WriteLine("Origin: " + flightsDict[FlightNumber].Origin);
        Console.WriteLine("Destination: " + flightsDict[FlightNumber].Destination);
        Console.WriteLine("Expected Time: " + flightsDict[FlightNumber].ExpectedTime);
        Console.WriteLine("Status: " + flightsDict[FlightNumber].Status);
        Console.WriteLine("Special Request Code: " + newRequestCode);
        if (boardingGatesDict.FirstOrDefault(x => x.Value.Flight == flightsDict[FlightNumber]).Value != null)
        {
            Console.WriteLine("Boarding Gate: " + boardingGatesDict.FirstOrDefault(x => x.Value.Flight == flightsDict[FlightNumber]).Key);
        }
        else
        {
            Console.WriteLine("Boarding Gate: Unassigned");
        }
    }
    else if (choice == 2)
    {
        Console.WriteLine("Confirmation to delete flight[Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();
        if (confirm == "Y")
        {
            flightsDict.Remove(FlightNumber);
            Console.WriteLine("Flight has been deleted!");
        }
        else if (confirm == "N")
        {
            Console.WriteLine("Flight has not been deleted!");
            return;
        }
        else
        {
            Console.WriteLine("Invalid input!");
        }
    }
    else
    {
        Console.WriteLine("Invalid input!");
    }
}


//Feature #9: Display scheduled flights in chronological order (HUANG YANGMILE)
void DisplayFlightSchedule(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGatesDict, Dictionary<string, Airline> airlineDict)
{
    List<Flight> flightList = flightsDict.Values.ToList();
    flightList.Sort();
    
    Console.WriteLine("=============================================\n" +
                      "Flight Schedule for Changi Airport Terminal 5\n" +
                      "=============================================");
    
    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}{4, -20}\n{5, -20} {6, -20}";
    Console.WriteLine(stringFormat, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Boarding Gate");
    
    List<BoardingGate> boardingGatesWithFlights = new();
    foreach (BoardingGate boardingGate in boardingGatesDict.Values.ToList())
    {
        if (boardingGate.Flight != null)
        {
            boardingGatesWithFlights.Add(boardingGate);
        }
    }
    
    foreach (Flight flight in flightList)
    {
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateTime expectedTime = flight.ExpectedTime;
        string status = flight.Status;
        string boardingGate = "Unassigned";

        foreach (BoardingGate gate in boardingGatesWithFlights)
        {
            if (flight == gate.Flight)
            {
                boardingGate = gate.GateName;
            }
        }
        
        string airlineCode = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "ERROR";

        if (airlinesDict.ContainsKey(airlineCode))
        {
            airlineName = airlinesDict[airlineCode].Name;
        }
        
        Console.WriteLine(stringFormat, flightNumber, airlineName, origin, destination, expectedTime, status, boardingGate);
    }
}


LoadAirlineAndBoardingGateData(airlinesDict, boardingGatesDict, new("airlines.csv"), new("boardinggates.csv"));
LoadFlights(flightsDict, new("flights.csv"));
Console.WriteLine("\n\n\n\n\n");

while (true)
{
    Console.WriteLine("=============================================\n" +
                      "Welcome to Changi Airport Terminal 5\n" +
                      "=============================================\n" +
                      "1. List All Flights\n" +
                      "2. List Boarding Gates\n" +
                      "3. Assign a Boarding Gate to a Flight\n" +
                      "4. Create Flight\n" +
                      "5. Display Airline Flights\n" +
                      "6. Modify Flight Details\n" +
                      "7. Display Flight Schedule\n" +
                      "0. Exit\n\n" +
                      "Please select your option:");

    int userInput = 0;
    try
    {
        userInput = int.Parse(Console.ReadLine());
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        Console.ReadLine();
        continue;
    }

    if (userInput == 1)
    {
        ListFlights(flightsDict, airlinesDict);
    }
    else if (userInput == 2)
    {
        ListBoardingGates(boardingGatesDict);
    }
    else if (userInput == 3)
    {
        AssignGateToFlight(flightsDict, boardingGatesDict);
    }
    else if (userInput == 4)
    {
        CreateNewFlight(flightsDict);
    }
    else if (userInput == 5)
    {
        DisplayFullDetailsFromAirline();
    }
    else if (userInput == 6)
    {
        ModifyFlightDetails();
    }
    else if (userInput == 7)
    {
        DisplayFlightSchedule(flightsDict, boardingGatesDict, airlinesDict);
    }
    else if (userInput == 0)
    {
        Console.Write("Goodbye!");
        Console.ReadLine();
        Environment.Exit(0);
    }

    Console.ReadLine();
}

string InputForString(string request, string errorMessage = "Invalid input.")
{
    while (true)
    {
        try
        {
            Console.WriteLine(request);
            return Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(errorMessage);
            Console.ReadLine();
        }
    }
}

int InputForInt(string request, string errorMessage = "Please enter a number matching one of the options.")
{
    while (true)
    {
        try
        {
            Console.WriteLine(request);
            return int.Parse(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(errorMessage);
            Console.ReadLine();
        }
    }
}
