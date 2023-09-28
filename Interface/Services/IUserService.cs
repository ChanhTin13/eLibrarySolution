using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.Services.DomainServices;
using Request.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IUserService: IDomainService<tbl_Users, UserSearch>
    {
        Task<bool> Verify(string userName, string password);
        Task<bool> UpdateUserPassword(Guid userId, string newPassword);
        Task<bool> HasPermission(Guid userId, string controller, string action);
        Task ValidatePassword(Guid userId, string password);
        Task ForgotPassword(ForgotPasswordModel model);
        Task ResetPassword(ResetPasswordModel model);
        Task<List<AccountModel>> GetAccount();
        Task ValidateUser(tbl_Users model);
    }
}
