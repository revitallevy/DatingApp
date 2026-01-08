using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository(AppDbContext context) : ILikesRepository
    {
        public void AddLike(MemberLike like)
        {
            context.Likes.Add(like);
        }

        public void DeleteLike(MemberLike like)
        {
            context.Likes.Remove(like);
        }

        public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
        {
            return await context.Likes
                .Where(x => x.SourceMemberId == memberId)
                .Select(x => x.TargetMemberId)
                .ToListAsync();
        }

        public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
        {
            return await context.Likes.FindAsync(sourceMemberId, targetMemberId);
        }

        public async Task<PaginatedResult<Member>> GetMemberLikes(LikesParams likesParams)
        {
            var query = context.Likes.AsQueryable();
            IQueryable<Member> result;

            switch (likesParams.Predicate)
            {
                case "liked":
                    result = query
                        .Where(x => x.SourceMemberId == likesParams.MemberId)
                        .Select(x => x.TargetMember);
                    break;
                case "likedBy":
                    result = query
                        .Where(x => x.TargetMemberId == likesParams.MemberId)
                        .Select(x => x.SourceMember);
                    break;
                default: // mutual
                    var likeIds = await GetCurrentMemberLikeIds(likesParams.MemberId);

                    result = query
                        //keeps only likes where someone (SourceMemberId) liked MemberId AND that someone’s ID is in the list that MemberId liked — that intersection is the mutual like.
                        .Where(x => x.TargetMemberId == likesParams.MemberId && likeIds.Contains(x.SourceMemberId))
                        .Select(x => x.SourceMember);
                    break;
                    // 1.	GetCurrentMemberLikeIds("A") returns IDs A liked (["B"]).
                    // 2.	The query filters likes where TargetMemberId == "A" (people who liked A: B and C).
                    // 3.	It then requires likeIds.Contains(x.SourceMemberId) → keeps only those whose id is in ["B"].
                    // 4.	The .Select(x => x.SourceMember) returns those other Member objects (B). It does not include A itself.
            }

            return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
        }
    }
}
