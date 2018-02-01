using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using Common.Entities;

namespace PostManagment
{
    public interface IPostManager
    {
        Post GetPostById(ObjectId postId);
        IEnumerable<Post> GetPostsByPredicate(Expression<Func<Post, bool>> predicate = null);
        IEnumerable<Post> GetPostsByCategory(string categoryName);

        ObjectId CreatePost(Post postToCreate);
        void UpdatePost(Post postToUpdate);
        void DeletePost(ObjectId postId);
    }
}