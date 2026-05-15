using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.Application.Workflows
{
    public class Condition
    {
        #region Fields



        #endregion

        #region Properties

        public string Key { get; set; }
        public bool IsCompleted { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public Condition(string key)
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
