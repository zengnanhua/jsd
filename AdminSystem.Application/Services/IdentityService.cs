using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Application.Services
{
    public class IdentityService:IIdentityService
    {
        private IHttpContextAccessor _context;
        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetMobile()
        {
            return _context.HttpContext.User.FindFirst("Mobile").Value;
        }
        public string GetUserId()
        {
            return _context.HttpContext.User.FindFirst("UserId").Value;
        }
        public string GetDeptCode()
        {
            return _context.HttpContext.User.FindFirst("DeptCode").Value;
        }
        public string GetTrueName()
        {
            return _context.HttpContext.User.FindFirst("TrueName").Value;
        }
    }
}
