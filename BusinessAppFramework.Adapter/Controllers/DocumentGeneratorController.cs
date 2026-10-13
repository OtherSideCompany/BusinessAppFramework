using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class DocumentGeneratorController : ControllerBase
    {
        #region Fields

        private IDocumentGenerator _documentGenerator;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DocumentGeneratorController(IDocumentGenerator documentGenerator)
        {
            _documentGenerator = documentGenerator;
        }

        #endregion

        #region Public Methods

        [HttpGet($"{DocumentRouteSegments.GetHtml}/{{{ApiRouteParams.Key}}}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<ActionResult<DocumentHtmlResponse>> GetHtmlDocumentAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string documentKey,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var htmlDocument = await _documentGenerator.GetHtmlDocumentAsync(documentKey, domainObjectId);

            return Ok(new DocumentHtmlResponse() { HtmlContent = htmlDocument });
        }

        [HttpGet($"{DocumentRouteSegments.DownloadPdf}/{{{ApiRouteParams.Key}}}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<IActionResult> DownloadPdfDocumentAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string documentKey,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var pdfDocument = await _documentGenerator.GetPdfDocumentAsync(documentKey, domainObjectId);

            if (pdfDocument is null || pdfDocument.Content.Length == 0)
                return NotFound();

            var documentNumber = string.IsNullOrWhiteSpace(pdfDocument.DocumentNumber) ? domainObjectId.ToString() : pdfDocument.DocumentNumber;

            return File(pdfDocument.Content, "application/pdf", $"{documentKey}-{documentNumber}.pdf");
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
