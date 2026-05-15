namespace BusinessAppFramework.Application.Workflows
{
    public class Step
    {
        #region Fields



        #endregion

        #region Properties

        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public string Key { get; set; }
        public Transition? OutgoingTransition { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public Step(string key)
        {
            Key = key;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
