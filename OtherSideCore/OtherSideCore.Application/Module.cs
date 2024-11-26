namespace OtherSideCore.Application
{
    public abstract class Module : IDisposable
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public Module()
        {

        }

        #endregion

        #region Public Methods

        public abstract Task InitializeAsync();

        public abstract void Dispose();

        #endregion

        #region Private Methods



        #endregion
    }
}
