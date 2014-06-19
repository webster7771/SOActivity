using Ccai.NewRoam.SOActivity.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Ccai.NewRoam.SOActivity.Web.Controllers
{
    [Authorize]
    public class RolesController : ApiController
    {
        // GET api/accountapi
        public IEnumerable<RoleModel> Get()
        {
            return Roles.GetAllRoles().Select( roleName => 
                {
                    return new RoleModel
                    {
                        RoleName = roleName
                    };
                });
        }

        // GET api/accountapi/5
        public RoleModel Get(string role)
        {
            return Roles.GetAllRoles().
                Where(roleName => roleName == role).
                Select(roleName => new RoleModel { RoleName = roleName}).
                FirstOrDefault();
        }

        // POST api/accountapi
        public void Post(RoleModel role)
        {
            
            SetRole(role);
            
            if (ModelState.IsValid)
            {
                Roles.CreateRole(role.RoleName);
            }
            else
            {
                throw new ApplicationException("Role name is required.");
            }
        }

        // PUT api/accountapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/accountapi/5
        public void Delete(int id)
        {
        }

        private void SetRole(RoleModel role)
        {
        }

        private RoleModel GetRole()
        {
            return new RoleModel { 
                RoleName = "Administrator"
            };
        }

        private void SetRoleObj(Object role)
        {
        }

        private Object GetRoleObj()
        {
            return GetRole();
        }
    }
}