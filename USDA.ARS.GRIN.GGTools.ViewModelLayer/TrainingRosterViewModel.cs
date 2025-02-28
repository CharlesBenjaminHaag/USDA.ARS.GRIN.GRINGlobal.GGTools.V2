using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;
namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class TrainingRosterViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentCount { get; set; }
        public CooperatorViewModel DefaultCooperatorViewModel { get; set; }
        public SysUserViewModel DefaultSysUserViewModel { get; set; }
        public TrainingRosterViewModel()
        {
            DefaultCooperatorViewModel = new CooperatorViewModel();
            DefaultSysUserViewModel = new SysUserViewModel();
        }
    }
}
