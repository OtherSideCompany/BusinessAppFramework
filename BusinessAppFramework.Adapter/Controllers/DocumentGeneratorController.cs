using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.DocumentGenerator}")]
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
            var htmlDocument = await _documentGenerator.GetHtmlDocumentAsync(StringKey.From(documentKey), domainObjectId);

            return Ok(new DocumentHtmlResponse() { HtmlContent = htmlDocument });
        }

        [HttpGet($"{DocumentRouteSegments.DownloadPdf}/{{{ApiRouteParams.Key}}}/{{{ApiRouteParams.DomainObjectId}:int}}")]
        public virtual async Task<IActionResult> DownloadPdfDocumentAsync(
            [FromRoute(Name = ApiRouteParams.Key)] string documentKey,
            [FromRoute(Name = ApiRouteParams.DomainObjectId)] int domainObjectId)
        {
            var pdfDocumentBytes = await _documentGenerator.GetPdfDocumentAsync(StringKey.From(documentKey), domainObjectId);

            if (pdfDocumentBytes is null || pdfDocumentBytes.Length == 0)
                return NotFound();

            return File(pdfDocumentBytes, "application/pdf", $"{documentKey}-{domainObjectId}.pdf");
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
