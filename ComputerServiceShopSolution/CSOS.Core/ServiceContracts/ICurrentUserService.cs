using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ServiceContracts
{
    public interface ICurrentUserService

    {  /// <summary>
       /// Method serches for currently logged user
       /// </summary>
       /// <returns>Returns GUID of currently logged in user</returns>
        Guid GetUserId();
    }
}
