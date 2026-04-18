using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class UserTokenDomain
    {
        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;

        public int? UserId { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }


    }
}
