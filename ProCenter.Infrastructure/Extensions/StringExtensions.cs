namespace ProCenter.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstLetterUpper(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }
            if (s.Length == 1)
            {
                return s.ToUpper();
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}