using Common.Entities;

namespace MGSUCore.Models.Mappers
{
    public static class PostMapper
    {
        public static PostModel PostToPostModel(Post post)
        {
            if (post == null) return null;

            return new PostModel
            {
                Id = post.Id,
                Title = post.Title,
                Category = post.Category,
                Content = post.Content,
                Date = post.Date,
                Description = post.Description,
                Img = post.Img == null
                    ? null
                    : new ImageModel
                    {
                        Original = post.Img.Original,
                        Small = post.Img.Small,
                        Role = post.Img.Role
                    },
                CreatingDate = post.CreatingDate
            };
        }

        public static Post PostModelToPost(PostModel postModel)
        {
            if (postModel == null) return null;

            return new Post
            {
                Title = postModel.Title,
                Category = postModel.Category,
                Content = postModel.Content,
                Date = postModel.Date,
                Description = postModel.Description,
                Img = postModel.Img == null
                    ? null
                    : new Image
                    {
                        Original = postModel.Img.Original,
                        Small = postModel.Img.Small,
                        Role = postModel.Img.Role
                    }
            };
        }

        public static PartnerModel PostToPartnerModel(Post post)
        {
            if(post == null)
            {
                return null;
            }

            return new PartnerModel
            {
                Title = post.Title,
                Img = post.Img == null ? null :
                    new ImageModel
                    {
                        Original = post.Img.Original,
                        Small = post.Img.Small,
                        Role = post.Img.Role
                    },
                CreatingDate = post.CreatingDate
            };
        }
    }
}