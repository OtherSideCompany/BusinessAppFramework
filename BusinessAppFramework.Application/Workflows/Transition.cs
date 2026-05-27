using BusinessAppFramework.Application.Actions;

namespace BusinessAppFramework.Application.Workflows
{
    public class Transition
    {
        #region Fields



        #endregion

        #region Properties

        public DomainObjectHttpApplicationAction? DomainObjectHttpApplicationAction { get; set; }
        public OpenDialogApplicationAction? OpenDialogApplicationAction { get; set; }
        public bool IsExecutable { get; set; }
        public List<Condition> Conditions { get; set; } = new();     

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public Transition()
        {
            
        }

        #endregion

        #region Public Methods

        public string GetActionKey()
        {
            return DomainObjectHttpApplicationAction != null ? DomainObjectHttpApplicationAction.ActionKey :
                    OpenDialogApplicationAction != null ? OpenDialogApplicationAction.ActionKey : "";
        }

        public bool AreConditionsVerified()
        {
            return Conditions.Where(c => c.IsBlocking).All(c => c.IsCompleted);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
