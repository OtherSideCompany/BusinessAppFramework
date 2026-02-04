using Application.Relations;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.Controllers
{
    [ApiController]
    [Authorize]
    public class RelationServiceController : ControllerBase
    {
        #region Fields

        private IRelationService _relationService;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public RelationServiceController(IRelationService relationService)
        {
            _relationService = relationService;
        }

        #endregion

        #region Public Methods

        [HttpPut(Routes.SetParentTemplate)]
        public virtual async Task<ActionResult> SetParent(int parentDomainObjectId, int domainObjectId, string key)
        {
            await _relationService.SetParentAsync(parentDomainObjectId, domainObjectId, key);

            return Ok();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
