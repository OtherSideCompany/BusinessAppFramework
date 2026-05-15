using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BusinessAppFramework.Application.Workflows
{
    public class Transition
    {
        #region Fields



        #endregion

        #region Properties

        public string Key { get; set; }
        public DomainObjectHttpApplicationAction? DomainObjectHttpApplicationAction { get; set; }
        public OpenDialogApplicationAction? OpenDialogApplicationAction { get; set; }
        public bool IsExecuted { get; set; }
        public bool IsExecutable { get; set; }
        public List<Condition> Conditions { get; set; } = new();     
        public string? ExecutionConfirmationMessageKey { get; set; }
        public string ExecutionErrorMessageKey { get; set; } = BusinessAppFramework.Contracts.MessageKeys.CannotExecuteAction;

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public Transition(string key)
        {
            Key = key;
        }

        #endregion

        #region Public Methods

        public string GetActionKey()
        {
            return DomainObjectHttpApplicationAction != null ? DomainObjectHttpApplicationAction.ActionKey :
                    OpenDialogApplicationAction != null ? OpenDialogApplicationAction.ActionKey : "";
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
