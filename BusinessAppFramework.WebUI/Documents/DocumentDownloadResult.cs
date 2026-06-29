using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Documents
{
    public sealed record DocumentDownloadResult(byte[] Content, string ContentType, string FileName);
}
