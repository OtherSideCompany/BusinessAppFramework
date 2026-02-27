using BaseData.Domain.DomainObjects;
using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Services;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessAppFramework.Adapter.Controllers
{
   [ApiController]
   [Authorize]
   public abstract class DomainObjectDocumentController<TDomainObject> : ControllerBase where TDomainObject : DomainObject
   {
      #region Fields

      private readonly IDomainObjectDocumentService<TDomainObject> _domainObjectDocumentService;

      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectDocumentController(
         IDomainObjectDocumentService<TDomainObject> domainObjectDocumentService)
      {
         _domainObjectDocumentService = domainObjectDocumentService;
      }

      #endregion

      #region Public Methods

      [HttpPost(Routes.UploadDocumentTemplate)]
      [RequestSizeLimit(50_000_000)]
      public async Task<ActionResult<int>> UploadAsync(int domainObjectId, [FromForm] string relationKey, [FromForm] IFormFile file, CancellationToken ct)
      {
         await using var stream = file.OpenReadStream();

         var documentId = await _domainObjectDocumentService.AttachAsync(domainObjectId, relationKey, file.FileName, file.ContentType, file.Length, stream);

         return Ok(documentId);
      }

      [HttpDelete(Routes.DeleteDocumentTemplate)]
      public async Task<ActionResult> DeleteAsync(int domainObjectId, CancellationToken ct)
      {
         await _domainObjectDocumentService.DeleteAsync(domainObjectId, ct);

         return Ok();
      }

      [HttpGet(Routes.DocumentExistsTemplate)]
      public async Task<ActionResult<bool>> ExistsAsync(int domainObjectId, CancellationToken ct)
      {
         var exists = await _domainObjectDocumentService.ExistsAsync(domainObjectId, ct);

         return Ok(exists);
      }

      [HttpGet(Routes.DownloadDocumentTemplate)]
      public async Task<ActionResult> DownloadAsync(int domainObjectId, CancellationToken ct)
      {
         var readResult = await _domainObjectDocumentService.OpenReadAsync(domainObjectId, ct);

         if (!readResult.HasValue)
            return NotFound();

         return File(readResult.Value.stream, readResult.Value.contentType ?? "application/octet-stream", readResult.Value.fileName);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
