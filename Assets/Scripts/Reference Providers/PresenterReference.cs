namespace Skills.Core
{
    public struct PresenterReference<TPresenter> where TPresenter : BaseMainPresenter
    {
        public TPresenter Reference => ReferenceProvider.GetReference<TPresenter>();
    }
}