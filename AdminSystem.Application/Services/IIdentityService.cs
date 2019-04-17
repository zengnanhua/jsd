using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Services
{
    public interface IIdentityService
    {
        string GetMobile();
        string GetUserId();
        string GetDeptCode();
        string GetTrueName();
    }
}
