using System.Globalization;

bool loopContinue = true; //TO BE FOR USE IN WHILE (TRUE) LOOPS IF THERE IS A
                          //NESTED SWITCH STATEMENT OR WTV THAT PREVENTS CONTINUE FROM BEING USED



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

// Feature #4: List Boarding Gates
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

//Feature #5: Assign boarding gate to flight
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


//Feature #6: Create a new flight
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

//Feautre #8 : Modify flight details













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
