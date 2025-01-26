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
}