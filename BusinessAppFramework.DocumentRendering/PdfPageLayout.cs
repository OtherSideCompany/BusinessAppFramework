namespace BusinessAppFramework.DocumentRendering
{
    public class PdfPageLayout
    {
        #region Properties

        public string MarginTop { get; set; } = "14mm";
        public string MarginBottom { get; set; } = "12mm";
        public string MarginLeft { get; set; } = "18mm";
        public string MarginRight { get; set; } = "18mm";
        public string? FooterTemplate { get; set; }

        #endregion
    }
}
