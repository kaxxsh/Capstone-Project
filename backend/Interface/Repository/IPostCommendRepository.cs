using backend.Model.Domain.Post;

namespace backend.Interface.Repository
{
    public interface IPostCommendRepository: IRepository<Guid,PostComment>
    {
        Task<IEnumerable<PostComment>> GetCommentByPost(Guid PostId);
    }
}
