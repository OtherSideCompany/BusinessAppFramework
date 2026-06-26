namespace BusinessAppFramework.WebUI.Components.Editor
{
    public class PropertyEdition
    {
        public string PropertyName { get; set; } = default!;
        public int Md { get; set; } = 3;
        public string? AvailableValuesRelationKey { get; set; }
        public int? AvailableValuesParentId { get; set; }

    }
}
