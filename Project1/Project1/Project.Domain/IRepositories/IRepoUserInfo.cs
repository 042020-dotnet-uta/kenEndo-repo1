using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain.IRepositories
{
    public interface IRepoUserInfo
    {
        //add new user info to database
        void AddUserInfo(UserInfo userInfo);
        //return all user infos
        IEnumerable<UserInfo> GetAllUserInfo();
        //check user info to db and returns the user info
        UserInfo CheckUserInfoToDb(UserInfo userInfo);

    }
}
