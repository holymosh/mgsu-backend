using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using PostManagment;
using Common.Entities;
using MGSUCore.Filters;
using MGSUCore.Models;
using Newtonsoft.Json;
using MGSUCore.Models.Convertors;
using MGSUCore.Models.Mappers;

namespace MGSUCore.Controllers
{
    [CustomExceptionFilterAttribute]
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostManager _postManager;
        private const string partnersCategoryName = "partners";

        public PostsController(IPostManager postManager)
        {
            _postManager = postManager;
        }

        // GET: api/Posts?one=value&two=secondValue
        [HttpGet]
        public IActionResult Get()
        {
            if (!Request.Query.TryGetValue("category", out StringValues value))
            {
                return Ok(_postManager.GetPostsByPredicate().Select(PostMapper.PostToPostModel));
            }

            if (value.Count > 1)
            {
                return BadRequest("Query bad");
            }

            var category = value.Single();

            return
                Ok(_postManager.GetPostsByCategory(category).Select(PostMapper.PostToPostModel));


        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var postToReturn = _postManager.GetPostById(objectId);

            if (postToReturn == null)
                return NotFound();

            return Ok(PostMapper.PostToPostModel(postToReturn));
        }

        [HttpGet("~/partners")]
        public IActionResult GetPartners()
        {
            return
                Ok(_postManager.GetPostsByCategory(partnersCategoryName).Select(PostMapper.PostToPartnerModel));
        }

        // POST: api/Posts
        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Post([FromBody] PostModel postModel, string category)
        {
            if(category != null)
            {
                var categoryName = nameof(postModel.Category);
                postModel.Category = category;
                ModelState.ClearError(categoryName);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postToCreate = PostMapper.PostModelToPost(postModel);

            return Ok(_postManager.CreatePost(postToCreate).ToString());
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public IActionResult Put(string id, [FromBody] PostModel postModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldPost = _postManager.GetPostById(objectId);

            if (oldPost == null)
                return NotFound();

            oldPost.Title = postModel.Title ?? oldPost.Title;
            oldPost.Category = postModel.Category ?? oldPost.Category;
            oldPost.Content = postModel.Content ?? oldPost.Content;
            oldPost.Date = postModel.Date;
            oldPost.Description = postModel.Description ?? oldPost.Description;
            oldPost.Img = postModel.Img != null ? new Image(postModel.Img.Original, postModel.Img.Small) : oldPost.Img;

            _postManager.UpdatePost(oldPost);
            return Ok(id);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public IActionResult Delete(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId))
                return BadRequest("'Id' parameter is ivalid ObjectId");

            var oldPost = _postManager.GetPostById(objectId);

            if (oldPost == null)
                return NotFound();

            _postManager.DeletePost(objectId);
            return Ok(id);
        }
    }
}