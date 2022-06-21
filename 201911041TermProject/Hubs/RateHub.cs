using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace _201911041TermProject.Hubs
{
    public class RateHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public RateHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task UpdateLike(string postId, string userId)
        {

            // JS'den gelen değer string olarak geliyor. Dolayısıyla TryParse metotunu kullan.
            int postIdInt = int.TryParse(postId, out int x) == true ? x : 0;

            if (postIdInt == 0)
            {
                return;
            }

            var post = await _context.Posts.FindAsync(postIdInt);
            var user = await _context.Users.FindAsync(userId);

            if (post == null || user == null)
            {
                return;
            }

            // Kullanıcı daha önce post'u beğendiyse, like'ı azalt.
            var userInteraction = await _context.UsersPosts.FindAsync(userId, postIdInt);

            if (userInteraction == null)
            {
                // User, post ile herhangi bir etkileşim kurmamış.
                // Yeni etkileşim oluştur ve uygun data ile doldurup db'ye kaydet.


                var newInteraction = new UserPost()
                {
                    PostId = postIdInt,
                    Post = post,
                    UserId = userId,
                    User = user,
                };

                post.LikeCount++;
                newInteraction.Interaction = Interaction.Like;
                await _context.UsersPosts.AddAsync(newInteraction);

            }

            else
            {
                // User, daha önce posta like atmış.
                // Bulunan userInteraction'u db'den sil.

                post.LikeCount--;
                _context.UsersPosts.Remove(userInteraction);

            }

            await _context.SaveChangesAsync();
            // Send Async Yazılabilir.

            await Clients.All.SendAsync("UpdateRating", true, post.LikeCount);

        }


        public async Task UpdateDislike(string postId, string userId)
        {

        }
    }
}
