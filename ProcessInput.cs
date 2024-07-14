namespace CodingTracker
{
    public class ProcessInput
    {
        public static bool CheckDate(string date)
        {
            string[] formats = { "MM/dd/yyyy" }; // Add more formats if needed
            return DateTime.TryParseExact(date, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _);
        }

        public static bool CheckHours(string hours)
        {
            if(Int32.TryParse(hours, out int hoursInt))
            {
                if(hoursInt <= 24 && hoursInt >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
