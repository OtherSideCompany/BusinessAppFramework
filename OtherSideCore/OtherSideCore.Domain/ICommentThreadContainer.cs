using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Domain
{
   public interface ICommentThreadContainer
   {
      CommentThread CommentThread { get; set; }
   }
}
