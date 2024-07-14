using System.Data.SQLite;


namespace CodingTracker
{
    class Program
    {
        
        private static void promptUser()
        {
            Console.WriteLine("This is your coding tracker, which of the following would you like to do?");
            Console.WriteLine("1. Read past data");
            Console.WriteLine("2. Log new data");
            Console.WriteLine("3. Update existing data");
            Console.WriteLine("4. Delete past data");
            Console.WriteLine("5. End session");
        }

        static void Main(string[] args)
        {
            string userChoice = string.Empty;
            DatabaseManagement.DatabaseSetup();
            SQLiteConnection connection = DatabaseManagement.ConnectToDB();

            while (userChoice != "5")
            {
                promptUser();
                userChoice = Console.ReadLine();
                string date = string.Empty;
                string hours = string.Empty;
                string[] words = [];

                switch (userChoice)
                {
                    case "1":
                        Console.WriteLine("You have selected to read data, enter the date in format MM/DD/YYYY you would like to see your hours for");
                        date = Console.ReadLine();
                        Console.WriteLine(DatabaseManagement.ReadFromDB(connection, date) + "\n");
                        break;
                    case "2":
                        Console.WriteLine("You have selected to enter new data, enter the date in format MM/DD/YYYY and hours separated by a space");
                        words = Console.ReadLine().Split(' ');
                        date = words[0]; hours = words[1];
                        Console.WriteLine(DatabaseManagement.InsertIntoDB(connection, date, hours) + "\n");
                        break;
                    case "3":
                        Console.WriteLine("You have selected to update data, enter the date in format MM/DD/YYYY and new hour data separated by a space");
                        words = Console.ReadLine().Split(' ');
                        date = words[0]; hours = words[1];
                        Console.WriteLine(DatabaseManagement.UpdateDB(connection, date, hours) + "\n");    
                        break;
                    case "4":
                        Console.WriteLine("You have selected to delete data, enter the date in format MM/DD/YYYY you would like to delete data for");
                        date = Console.ReadLine();
                        Console.WriteLine(DatabaseManagement.DeleteFromDB(connection, date) + "\n");
                        break;
                    case "5":
                        Console.WriteLine("You have selected to end the session");
                        break; 
                    default:
                        Console.WriteLine("Please enter a valid choice" + "\n");
                        break;
                }
            }
            DatabaseManagement.CloseDBConnection(connection);

        }

    }
}
