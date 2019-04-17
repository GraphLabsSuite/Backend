using System;
using System.Linq;
using System.Security.Claims;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace GraphLabs.Backend.Api.Controllers
{
    public class UsersController : ODataController
    {
        private readonly GraphLabsContext _db;

        public UsersController(GraphLabsContext context)
        {
            _db = context;
        }
        
        [HttpGet]
        [ODataRoute("currentUser")]
        [EnableQuery]
        public SingleResult<User> CurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return SingleResult.Create(_db.Users.Where(u => u.Email == email));
        }
    }
}