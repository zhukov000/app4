using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Security;

namespace App3.Web
{
    public class DatabaseUser : IUserMapper
    {
        Nancy.Security.IUserIdentity IUserMapper.GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            throw new NotImplementedException();
        }
    }
}
