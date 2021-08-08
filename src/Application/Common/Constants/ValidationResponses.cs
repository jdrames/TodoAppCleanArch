namespace Application.Common.Constants
{
    public static class ValidationResponses
    {
        public static string Required = "This field is required.";
        public static string InvalidEmailFormat = "This field should be an email address.";
        public static string MinimumLength(int length) => $"This field requires at least {length} characters.";
    }
}
