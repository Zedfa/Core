namespace Core.Cmn.Resources
{
    #region Localizer Base Class

    public abstract class BaseLocalizer
    {
        #region Abstract Methods

        public abstract string GetLocalizedString(StringID id);

		#endregion
    }

    #endregion
}
