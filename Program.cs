// S10270189J Huang Yangmile: Features 2, 3, 5, 6, 9, Advanced (a)
// S10259006 Larry Chia: Features 1, 4, 7, 8, Advanced (b)


using System.Globalization;

bool loopContinue = true; //For use in while (true) loops with nested loops where calling the while loop is difficult.



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
            Flight newFlight = requestCode switch //God bless these things
            {
                "" => new NORMFlight(flightNumber, origin, destination, expectedTime),
                "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime),
                "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime),
                "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime)
            };
            
            flightsDict.Add(flightNumber, newFlight);
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

    //Print Header
    Console.WriteLine(stringFormat, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    //Print each Flight
    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        Flight flight = kvp.Value;
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateTime expectedTime = flight.ExpectedTime;

        string airlineCode = $"{flightNumber[0]}{flightNumber[1]}";
        string airlineName = "NOT FOUND";

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
    
    //Print header
    Console.WriteLine(stringFormat, "Boarding Gate", "DDJB", "CFFT", "LWTT", "Flight Number");
    
    //Print each boarding gate
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
    loopContinue = true;
    while (loopContinue)
    {
        Console.WriteLine("=============================================\n" +
                          "Assign a Boarding Gate to a Flight\n" +
                          "=============================================");

        string flightNumber = "";
        string boardingGateName = "";

        //Flight number input
        while (true)
        {
            flightNumber = InputForString("Enter Flight Number:").ToUpper();

            if (flightsDict.ContainsKey(flightNumber))
            {
                Console.WriteLine("FLIGHT NUMBER NOT FOUND!");
                Console.ReadLine();
                continue;
            }

            break;
        }

        //Boarding Gate input
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

        //Get flight & boarding gate from input
        Flight flight = flightsDict[flightNumber];
        BoardingGate boardingGate = boardingGatesDict[boardingGateName];

        boardingGate.Flight = flight;

        Console.WriteLine(flight);
        Console.WriteLine(boardingGate);

        while (true)
        {
            string stringInput = InputForString("Would you like to update the status of the flight? (Y/N)").ToUpper();
            
            if (stringInput == "N")
            {
                loopContinue = false;
                Console.WriteLine($"{flight.FlightNumber} has been assigned to Boarding Gate {boardingGate.GateName}!");
                break;
            }

            if (stringInput == "Y")
            {
                while (true)
                {
                    Console.WriteLine("1. Delayed\n" +
                                      "2. Boarding\n" +
                                      "3. On Time");
                    int intInput = InputForInt("Please select the new status of the flight:");

                    if (intInput == 1)
                    {
                        flight.Status = "Delayed";
                    }
                    else if (intInput == 2)
                    {
                        flight.Status = "Boarding";
                    }
                    else if (intInput == 3)
                    {
                        flight.Status = "On Time";
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number between 1 and 3.");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine($"{flight.FlightNumber} has been assigned to Boarding Gate {boardingGate.GateName}!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Please enter Y or N.");
            }
        }
    }
}


//Feature #6: Create a new flight (HUANG YANGMILE)
void CreateNewFlight(Dictionary<string, Flight> flightsDict, Dictionary<string, Airline> airlinesDict)
{
    loopContinue = true;
    while (loopContinue)
    {
        string flightNumber = "";
        //Get flight number & make sure it's valid by checking if it's found in airlinesDict
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            flightNumber = Console.ReadLine().ToUpper();

            string airlineCode = $"{flightNumber[0]}{flightNumber[1]}";

            if (!airlinesDict.ContainsKey(airlineCode))
            {
                Console.WriteLine("Airline code not found. Please try again.");
                Console.ReadLine();
                continue;
            }

            break;
        }

        //Get the rest of the inputs.
        string origin = InputForStringNoNewLine("Enter Origin: ");
        string destination = InputForStringNoNewLine("Enter Destination: ");
        DateTime departureTime;
        string specialRequestCode;

        while (true)
        {
            try
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string input = Console.ReadLine();
                string format = "d/M/yyyy H:mm";
                departureTime = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
                break;
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a valid date and time. For example, '13/1/2025 15:40'.");
            }
        }

        while (true)
        {
            specialRequestCode = InputForStringNoNewLine("Enter Special Request Code (CFFT/DDJB/LWTT/None): ").ToUpper();
            if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" ||
                specialRequestCode == "NONE")
            {
                break;
            }
            
            Console.WriteLine("Invalid Special Request Code. Please try again.");
        }

        //Surely, there won't be any errors here since we've checked all the inputs already <========== Clueless
        Flight newFlight = specialRequestCode switch //have you heard about our lord and saviour, arrow expressions?
        {
            "NONE" => new NORMFlight(flightNumber, origin, destination, departureTime),
            "DDJB" => new DDJBFlight(flightNumber, origin, destination, departureTime),
            "CFFT" => new CFFTFlight(flightNumber, origin, destination, departureTime),
            "LWTT" => new LWTTFlight(flightNumber, origin, destination, departureTime),
        };

        flightsDict.Add(newFlight.FlightNumber, newFlight);
        Console.WriteLine($"Flight {flightNumber} has been added!");

        while (true)
        {
            string addAnotherFlight = InputForString("Would you like to add another flight? (Y/N)").ToUpper();

            if (addAnotherFlight == "N") return;
            if (addAnotherFlight == "Y") break;
            Console.WriteLine("Please enter Y or N.");
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










//Feature #9: Display scheduled flights in chronological order (HUANG YANGMILE)
void DisplayFlightSchedule(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGatesDict, Dictionary<string, Airline> airlinesDict)
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

//Advanced Feature A: Process all unassigned flights to boarding gates in bulk (HUANG YANGMILE)
void AutoAssignFlights(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGatesDict)
{
    List<Flight> flightList = flightsDict.Values.ToList();
    Queue<Flight> filteredFlightQueue = new();
    
    List<BoardingGate> gateList = boardingGatesDict.Values.ToList();
    List<BoardingGate> filteredGateList = new();

    foreach (BoardingGate boardingGate in gateList)
    {
        if (boardingGate.Flight != null)
        {
            flightList.Remove(boardingGate.Flight);
            continue;
        }
        
        filteredGateList.Add(boardingGate);
    }

    foreach (Flight flight in flightList)
    {
        filteredFlightQueue.Enqueue(flight);
    }
    
    Console.WriteLine($"There are {filteredFlightQueue.Count} flights yet to be assigned.");
    Console.WriteLine($"There are {filteredGateList.Count} boarding gates yet to be assigned.");
    
    
    
    while (filteredFlightQueue.Count > 0)
    {
        Flight flightToAssign = filteredFlightQueue.Dequeue();
        BoardingGate gateToAssign = new();

        if (flightToAssign is NORMFlight)
        {
            gateToAssign = filteredGateList.Find(gate => gate.SupportsDDJB && gate.SupportsCFFT && gate.SupportsLWTT); //God bless arrow functions
        }
        else if (flightToAssign is DDJBFlight)
        {
            gateToAssign = filteredGateList.Find(gate => gate.SupportsDDJB);
        }
        else if (flightToAssign is CFFTFlight)
        {
            gateToAssign = filteredGateList.Find(gate => gate.SupportsCFFT);
        }
        else if (flightToAssign is LWTTFlight)
        {
            gateToAssign = filteredGateList.Find(gate => gate.SupportsLWTT);
        }

        if (gateToAssign == null) continue; //If there is no suitable gate found, this will be null. 
        
        gateToAssign.Flight = flightToAssign;
        filteredGateList.Remove(gateToAssign);
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
                      "8. Automatically assign flights to boarding gates\n" +
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
        CreateNewFlight(flightsDict, airlinesDict);
    }
    else if (userInput == 5)
    {
        DisplayFullDetailsFromAirline();
    }
    else if (userInput == 6)
    {

    }
    else if (userInput == 7)
    {
        DisplayFlightSchedule(flightsDict, boardingGatesDict, airlinesDict);
    }
    else if (userInput == 8)
    {
        AutoAssignFlights(flightsDict, boardingGatesDict);
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

string InputForStringNoNewLine(string request, string errorMessage = "Invalid input.")
{
    while (true)
    {
        try
        {
            Console.Write(request);
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
