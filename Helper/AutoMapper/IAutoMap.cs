namespace Helper.AutoMapper
{
    public interface IAutoMap
    {
        TOutput Map<TInput, TOutput>(TInput inputObj);
    }
}