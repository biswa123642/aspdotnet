using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Security.Accounts;

namespace CGP.Foundation.SitecoreExtensions.CustomField.UsersMultilist
{
    public interface IUsersField
    {
        IEnumerable<User> GetSelectedUsers();

        IEnumerable<User> GetUnselectedUsers();

        string GetProviderUserKey(User user);
    }
}