using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using DataAccess.Application;
using MongoDB.Bson;
using Common.Entities;

namespace PostManagment
{
    public class PostManager : IPostManager
    {
        private readonly IRepository<Post> _postRepository;

        public PostManager(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public Post GetPostById(ObjectId postId)
        {
            Require.NotNull(postId, nameof(postId));

            return _postRepository.GetById(postId);
        }

        public IEnumerable<Post> GetPostsByPredicate(Expression<Func<Post, bool>> predicate = null)
        {
            return _postRepository.GetByPredicate(predicate);
        }

        public IEnumerable<Post> GetPostsByCategory(string categoryName)
        {
            Require.NotEmpty(categoryName, nameof(categoryName));

            return _postRepository.GetByPredicate(post => post.Category == categoryName);
        }

        public ObjectId CreatePost(Post postToCreate)
        {
            Require.NotNull(postToCreate, nameof(postToCreate));

            return _postRepository.Create(postToCreate);
        }

        public void UpdatePost(Post postToUpdate)
        {
            Require.NotNull(postToUpdate, nameof(postToUpdate));

            _postRepository.Update(postToUpdate);
        }

        public void DeletePost(ObjectId postId)
        {
            Require.NotNull(postId, nameof(postId));

            _postRepository.Delete(postId);
        }
    }
}