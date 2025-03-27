using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalamaTravelDAL
{
    public static class CurrentUser
    {
        public static string Token { get; set; }
        public static string Email { get; set; }

        public static int OwnerId { get; set; }
    }
}
