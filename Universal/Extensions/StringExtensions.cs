namespace Universal.Extensions
{
    public static class StringExtensions
    {
        public static string ToLength(this string str, int length)
        {
            var newString = str;
            for (int i = 0; i < length - str.Length; i++)
            {
                newString += '\0';
            }

            return newString;
        }
    }
}
