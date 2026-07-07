using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Components.Editor
{
    public class PropertyCategory
    {
        public bool IsExpanded { get; set; }
        public string LabelKey { get; set; } = default!;
        public string? Icon { get; set; }
        public List<PropertyEdition> PropertyEditions { get; set; } = new();
    }
}
