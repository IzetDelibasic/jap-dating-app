using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;

namespace DatingApp.Services
{
    public class LikesService(IUnitOfWork unitOfWork) : ILikesService
    {
        public async Task<bool> ToggleLike(int sourceUserId, int targetUserId)
        {
            if (sourceUserId == targetUserId) throw new ArgumentException("You cannot like yourself");

            var existingLike = await unitOfWork.LikesRepository.GetUserLike(sourceUserId, targetUserId);

            if (existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId,
                };

                unitOfWork.LikesRepository.Add(like);
            }
            else
            {
                unitOfWork.LikesRepository.Delete(existingLike);
            }

            return await unitOfWork.Complete();
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int userId)
        {
            return await unitOfWork.LikesRepository.GetCurrentUserLikeIds(userId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
        {
            return await unitOfWork.LikesRepository.GetUserLikes(likesParams);
        }
    }
}