using System;
using Finix.Code;
using Finix.Domain.Entity.SystemManage;
using Finix.Domain.IRepository.SystemManage;
using Finix.Repository.SystemManage;

namespace Finix.Application.SystemManage
{
    public class UserLogOnApp
    {
        private IUserLogOnRepository service = new UserLogOnRepository();

        public UserLogOnEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void UpdateForm(UserLogOnEntity userLogOnEntity)
        {
            service.Update(userLogOnEntity);
        }
        public void RevisePassword(string userPassword,string keyValue)
        {
            UserLogOnEntity userLogOnEntity = new UserLogOnEntity();
            userLogOnEntity.F_Id = keyValue;
            userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
            userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
            service.Update(userLogOnEntity);
        }

        public string ModifyPassword(string oldPwd, string newPwd1)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            UserEntity userEntity = new UserApp().CheckLogin(userInfo.UserCode, oldPwd);
            if (userEntity != null)
            {
                UserLogOnEntity userLogOnEntity = new UserLogOnEntity();
                userLogOnEntity.F_Id = userInfo.UserId;
                userLogOnEntity.F_UserId = userInfo.UserId;
                userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
                userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(newPwd1, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                service.Update(userLogOnEntity);
                return "success";
            }
            else
            {
                return "errorPwd";
            }
        }
    }
}
