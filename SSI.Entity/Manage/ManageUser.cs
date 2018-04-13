using SSI.Entity.SystemManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;

namespace SSI.Entity.Manage
{
    public class ManageUser
    {
        public T_User User { get; set; }
        public List<T_Org> Orgs { get; set; }
        public List<T_Role> Roles { get; set; }
        public List<T_Menu> Menus { get; set; }
        public List<T_Action> Actions { get; set; }
    }
}
