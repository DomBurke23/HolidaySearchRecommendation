namespace TFP.HolidaySearchRecommendation.Common.UseCases
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}
