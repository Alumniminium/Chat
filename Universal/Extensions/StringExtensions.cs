namespace Universal.Extensions
{
    public static class StringExtensions
    {
        public static string ToLength(this string str, int length)
        {
            if (str == null)
                str = string.Empty;
            var newString = str;
            for (var i = 0; i < length - str.Length; i++)
            {
                newString += '\0';
            }

            return newString;
        }
        public static string CenterLength(this string str, int length)
        {
            var newString = str;

            while (newString.Length != length)
            {
                if (newString.Length % 2 == 0)
                {
                    newString = " " + newString;
                }
                else
                {
                    newString = newString + " ";
                }
            }

            return newString;
        }
    }
}
