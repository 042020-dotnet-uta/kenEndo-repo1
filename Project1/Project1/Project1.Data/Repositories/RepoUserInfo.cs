using Project1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Project1.Domain.IRepositories;

namespace Project1.Data.Repositories
{
    public class RepoUserInfo : IRepoUserInfo 
    {
        private readonly Project1Context _context;
        public RepoUserInfo(Project1Context context)
        {
            _context = context;
        }
        //add new user info to database
        public void AddUserInfo(UserInfo userInfo)
        {
            //converts username to lowercase for name search purpose
            userInfo.fName = userInfo.fName.ToLower();
            userInfo.lName = userInfo.lName.ToLower();
            if (_context.UserInfos.Any(x => x.userName == userInfo.userName))
            {
                throw new InvalidOperationException("Username already exists");
            }
            _context.Add(userInfo);
            _context.SaveChanges();
        }
        //return all user info
        public IEnumerable<UserInfo> GetAllUserInfo()
        {
            return _context.UserInfos;
        }
        //check user info to db and returns the user info
        public UserInfo CheckUserInfoToDb(UserInfo userInfo)
        {
            var obj = _context.UserInfos.Where(x => x.userName
            .Equals(userInfo.userName) && x.password.Equals(userInfo.password)).FirstOrDefault();
            return obj;
        }

    }
}
