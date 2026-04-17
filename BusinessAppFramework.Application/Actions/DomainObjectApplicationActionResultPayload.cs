namespace BusinessAppFramework.Application.Actions
{
    public class DomainObjectApplicationActionResultPayload
    {
        #region Fields



        #endregion

        #region Properties

        public List<DomainObjectChange> Changes { get; set; } = new();
        public string? ErrorMessageKey { get; set; }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectApplicationActionResultPayload()
        {

        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
