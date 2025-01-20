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
void ListFlights(Dictionary<string, Flight> flightsDict)
{
    Console.WriteLine("" +
        "=============================================\r\n" +
        "List of Flights for Changi Airport Terminal 5\r\n" +
        "=============================================");

    string stringFormat = "{0,-20} {1,-20} {2,-20} {3,-20}\n{4, -20}";

    Console.WriteLine(string.Format(stringFormat, "Flight Number", "Origin", "Destination", "Expected", "Departure/Arrival Time"));

    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        Flight flight = kvp.Value;
        string flightNumber = flight.FlightNumber;
        string origin = flight.Origin;
        string destination = flight.Destination;
        DateOnly date = DateOnly.FromDateTime(flight.ExpectedTime);
        TimeOnly time = TimeOnly.FromDateTime(flight.ExpectedTime);

        Console.WriteLine(string.Format(stringFormat, flightNumber, origin, destination, date, time));

    }
}

LoadFlights(flightsDict, new("flights.csv"));
ListFlights(flightsDict);