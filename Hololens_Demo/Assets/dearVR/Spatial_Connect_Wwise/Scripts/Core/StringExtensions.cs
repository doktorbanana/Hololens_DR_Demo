using System;

public static class StringExtensions
{
    public static bool CaseInsensitiveContains(this string sourceText, string toCheck, 
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    {
        return sourceText?.IndexOf(toCheck, stringComparison) >= 0;
    }
}
