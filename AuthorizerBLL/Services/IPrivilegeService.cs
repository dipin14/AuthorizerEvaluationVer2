using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizerBLL.Services
{
    public interface IPrivilegeService
    {
        int setPrivilege(int roleId);
        int updatePrivilege();
    }
}
