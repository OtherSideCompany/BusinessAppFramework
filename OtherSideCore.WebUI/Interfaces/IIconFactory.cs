using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IIconFactory
    {
        void RegisterIcon(StringKey key, string icon);
        string CreateIcon(StringKey key);
    }
}
