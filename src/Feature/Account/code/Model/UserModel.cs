using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CGP.Feature.Account.Model
{
    public class UserModel
    {

        public string Name { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}