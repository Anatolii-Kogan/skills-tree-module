namespace Skills.Core
{
    public class ApplicationService
    {
        protected ApplicationService()
        {
            ReferenceProvider.Register(this );
        }
    }
}
