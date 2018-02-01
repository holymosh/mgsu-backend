using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;

namespace MGSUCore.Authentification.Handlers
{

    public class IsAuthentificatedAuthHandler : AuthorizationHandler<IsAuthentificated>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthentificated requirement)
        {
            ObjectId currentId;
            if(context.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && 
                                            ObjectId.TryParse(c.Value, out currentId)))
            {
                context.Succeed(requirement);
            } 

            return Task.CompletedTask;
        }
    }
}