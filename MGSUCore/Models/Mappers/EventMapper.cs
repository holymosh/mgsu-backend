using Common.Entities;

namespace MGSUCore.Models.Mappers
{
    public class EventMapper
    {
        public static EventModel EventToEventModel(Event post)
        {
            if (post == null) return null;

            return new EventModel
            {
                Id = post.Id,
                Title = post.Title,
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

        public static Event EventModelToEvent(EventSavingModel postModel)
        {
            if (postModel == null) return null;

            return new Event
            {
                Title = postModel.Title,
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
        
    }
}