﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public class AuthenticatedUserSession
    {
        public SysApplication Application { get; set; }
        public SysUser User { get; set; }
        public DateTime SessionStart { get; set; }
        public AuthenticatedUserSession(SysUser sysUser)
        {
            User = sysUser;
            SessionStart = DateTime.Now;
        }
    }
}