namespace Blog_API.EexceptionMiddleware
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException (string message = "Not Found", string? localizerKey = null) : base(message)
        {
            Data.Add(AbstractExceptionHandlerMiddleware.LocalizationKey, localizerKey);

        }
    }
    public sealed class BadRequestException : Exception
    {
        public BadRequestException(string message = "Bad Request", string? localizerKey = null) : base(message)
        {
            Data.Add(AbstractExceptionHandlerMiddleware.LocalizationKey, localizerKey);

        }
    }

    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized", string? localizerKey = null) : base(message)
        {
            Data.Add(AbstractExceptionHandlerMiddleware.LocalizationKey, localizerKey);

        }
    }
}
