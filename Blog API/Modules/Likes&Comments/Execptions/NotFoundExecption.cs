using Blog_API.EexceptionMiddleware;

namespace Blog_API.Modules.Likes_Comments.Execptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message = "Not Found", string? localizerKey = null) : base(message)
        {
            Data.Add(AbstractExceptionHandlerMiddleware.LocalizationKey, localizerKey);

        }
    }
}
