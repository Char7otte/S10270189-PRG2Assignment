//Feature #1: Load airline and boarding gate data from file
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
            bool CFFTBool = bool.Parse(line.Split(',')[1]);
            bool DDJBBool = bool.Parse(line.Split(',')[2]);
            bool LWTTBool = bool.Parse(line.Split(',')[3]);

            //Create a new boarding gate object
            BoardingGate boardingGateObj = new(boardingGate, CFFTBool, DDJBBool, LWTTBool, null);
            boardingGatesDict.Add(boardingGate, boardingGateObj);
        }
        Console.WriteLine($"{lineCount} Boarding Gates Loaded!");
    }
}


//Feature #2: Load Flights from file
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

//Feature #3: List Flights
Dictionary<string, string> specialRequestFlightsDict = new();
void ListFlights(Dictionary<string, Flight> flightsDict, Dictionary<string, Airline> airlinesDict)
{
    Console.WriteLine("" +
                      "=============================================\r\n" +
                      "List of Flights for Changi Airport Terminal 5\r\n" +
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

        string airlineCode = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "ERROR";

        if (airlinesDict.ContainsKey(airlineCode))
        {
            airlineName = airlinesDict[airlineCode].Name;
        }

        Console.WriteLine(stringFormat, flightNumber, airlineName, origin, destination, date, time);
    }
}

// Feature #4: List Boarding Gates
void ListBoardingGates(Dictionary<string, BoardingGate> boardingGatesDict)
{
    Console.WriteLine("=============================================\n" +
                      "List of Boarding Gates\n" +
                      "=============================================");
    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}";
    Console.WriteLine(stringFormat, "Boarding Gate", "CFFT", "DDJB", "LWTT");
    foreach (var kvp in boardingGatesDict)
    {
        BoardingGate gate = kvp.Value;
        Console.WriteLine(stringFormat, gate.GateName, gate.SupportsCFFT, gate.SupportsDDJB, gate.SupportsLWTT);
    }
}


//Feature #5: Assign boarding gate to flight
void AssignGateToFlight(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGatesDict)
{
    Console.WriteLine("=============================================\n" +
                      "Assign a Boarding Gate to a Flight\n" +
                      "=============================================");


    string flightNumber = InputForString("Enter Flight Number:", "how did you mess up the flight number");
    string boardingGate = InputForString("Enter Boarding Gate Name:", "Boarding gate input broke");

    if (!flightsDict.ContainsKey(flightNumber))
    {
        Console.WriteLine("FLIGHT NUMBER NOT FOUND!");
        Console.ReadLine();
        return;
    }

    if (!boardingGatesDict.ContainsKey(boardingGate))
    {
        Console.WriteLine("BOARDING GATE NOT FOUND!");
        Console.ReadLine();
        return;
    }

    Console.WriteLine(flightsDict[flightNumber].ToString());
    Console.WriteLine(boardingGatesDict[boardingGate].ToString());

    boardingGatesDict[boardingGate].Flight = flightsDict[flightNumber];

    string stringInput = InputForString("Would you like to update the status of the flight? (Y/N)",
        "How did you break this");
    stringInput = stringInput.ToUpper();

    if (stringInput == "N")
    {
        return;
    }
    else if (stringInput != "Y")
    {
        Console.WriteLine("Invalid input");
        return;
    }

    Console.WriteLine("1. Delayed\n" +
                      "2. Boarding\n" +
                      "3. On Time");
    int intInput = InputForInt("Please select the new status of the flight:", "how");

    Console.WriteLine($"Flight {flightNumber} has been assigned to Boarding Gate {boardingGate}!");
}

//Feature #7: Display full flight details from an airline
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
    string AirlineName = airlinesDict[airlineCode].Name;
    Console.WriteLine("=============================================\n" +
                      $"List of Flights for {AirlineName}\n" +
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

//Feature #8 : Modify flight details (LARRY CHIA)
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

    string airlineCode = InputForString("Enter Airline Code:").ToUpper();
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
                      "2. Delete Flight\n"   +
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
                    DateTime newExpectedTime = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
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

    }
    else if (userInput == 0)
    {
        Console.Write("Goodbye!");
        Console.ReadLine();
        Environment.Exit(0);
    }

    Console.ReadLine();
}

string InputForString(string request, string errorMessage)
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

int InputForInt(string request, string errorMessage)
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
