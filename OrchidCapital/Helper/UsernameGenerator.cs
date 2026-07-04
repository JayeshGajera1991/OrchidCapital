namespace OrchidCapital.Helper
{
    public class UsernameGenerator
    {
        public static string GenerateUsername(string fName,string lName)
        {
            // Remove spaces and convert to uppercase
            string namePart = fName.ToUpper();

            // Take first 4 characters only
            if (namePart.Length > 4)
            {
                namePart = namePart.Substring(0, 3);
            }
            // Remove spaces and convert to uppercase
            string namePart1 = lName.ToUpper();

            // Take first 4 characters only
            if (namePart1.Length > 4)
            {
                namePart1 = namePart1.Substring(0, 3);
            }

            // Random 4 digit number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);

            // Final username
            string username = $"OC{namePart}{namePart1}{randomNumber}";

            return username;
        }
    }
}