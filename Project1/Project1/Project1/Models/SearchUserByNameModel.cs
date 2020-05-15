using Project1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class SearchUserByNameModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<UserInfo> userInfos { get; set; }
    }
}
