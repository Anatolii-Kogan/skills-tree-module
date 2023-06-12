namespace Skills.Core
{
    public abstract class ApplicationService
    {
        protected ApplicationService()
        {
            ReferenceProvider.Register(this );
        }
    }
}
