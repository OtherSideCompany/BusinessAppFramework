using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
   [ApiController]
   [Authorize]
   public class DocumentController : ControllerBase
   {
      #region Fields

      private IDocumentGenerator _documentGenerator;

      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DocumentController(IDocumentGenerator documentGenerator)
      {
         _documentGenerator = documentGenerator;
      }

      #endregion

      #region Public Methods

      [HttpGet(Routes.GetHtmlDocumentTemplate)]
      public virtual async Task<ActionResult<DocumentHtmlResponse>> GetHtmlDocumentAsync(string key, int domainObjectId)
      {
         var htmlDocument = await _documentGenerator.GetHtmlDocumentAsync(StringKey.From(key), domainObjectId);

         return Ok(new DocumentHtmlResponse() { HtmlContent = htmlDocument });
      }

      [HttpGet(Routes.DownloadPdfDocumentTemplate)]
      public virtual async Task<IActionResult> DownloadPdfDocumentAsync(string key, int domainObjectId)
      {
         var pdfDocumentBytes = await _documentGenerator.GetPdfDocumentAsync(StringKey.From(key), domainObjectId);

         if (pdfDocumentBytes is null || pdfDocumentBytes.Length == 0)
            return NotFound();

         return File(pdfDocumentBytes, "application/pdf", $"{key}-{domainObjectId}.pdf");
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
