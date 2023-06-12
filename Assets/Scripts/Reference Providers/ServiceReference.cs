namespace Skills.Core
{
    public struct ServiceReference<TService> where TService : ApplicationService, new()
    {
        public TService Reference => ReferenceProvider.GetWithGeneration<TService>();
    }
}