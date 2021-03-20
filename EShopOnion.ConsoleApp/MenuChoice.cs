using EShopOnion.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.ConsoleApp
{
    public class MenuChoice
    {
        public string Title { get; private set; }
        public Action Action { get; private set; }
        public UserRoles UserRole { get; private set; }
        public MenuChoice(string title, Action action, UserRoles userRole)
        {
            Title = title;
            Action = action;
            UserRole = userRole;
        }
    }
}
