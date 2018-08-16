using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using webNews.Domain;
using webNews.Domain.Entities;
using static System.String;

namespace webNews.Security
{
    public class Authentication
    {
        public static void ClearAllSession()
        {
        }

        public static HomePageInfo GetHomePageInfo()
        {
            var defaultInfo = new HomePageInfo
            {
                Branches = new List<BasicInfo>()
            };

            if (HttpContext.Current == null) return defaultInfo;

            if (HttpContext.Current.Session["###HOME_PAGE_INFO###"] == null)
            {
                //defaultInfo = 

                HttpContext.Current.Session["###HOME_PAGE_INFO###"] = defaultInfo;
            }

            return HttpContext.Current.Session["###HOME_PAGE_INFO###"] as HomePageInfo;
        }

        public static string BuildMenuFE(List<MenuFE> menus)
        {
            var menuString = "";

            foreach(MenuFE menu in menus)
            {
                menuString += "<li>";
                if(menu.Slug == "trang-chu")
                    menuString += $"<a class=\"active\" title=\"{menu.Name}\" href=\"{menu.Slug}\">{menu.Name}</a>";
                menuString += $"<a href=\"{menu.Slug}\">{menu.Name}</a>";

                //Check menu has childs
                if (menu.MenuChilds != null && menu.MenuChilds.Count > 0)
                    menuString += $"<ul>{BuildMenuFE(menu.MenuChilds)}</ul>";

                menuString += "</li>";
            }

            return menuString;
        }

        public static void MarkMenuFE(List<MenuFE> menus)
        {
            HttpContext.Current.Session["###MENU_FE###"] = BuildMenuFE(menus);
        }

        public static string GetMenuFE()
        {
            if (HttpContext.Current == null) return string.Empty;

            if (HttpContext.Current.Session["###MENU_FE###"] == null)
            {
                HttpContext.Current.Session["###MENU_FE###"] = string.Empty;
            }

            return HttpContext.Current.Session["###MENU_FE###"].ToString();
        }

        public static bool Authenticate(string username, string password)
        {
            HttpContext.Current.Session.Timeout = 600;
            if (username == null || password == null) return false;

            if (username.Equals("superadmin") && password.Equals("password"))
            {
                HttpContext.Current.Session["username"] = username;
                return true;
            }
            return false;
        }
        public static bool Logout()
        {
            if (GetUserName() == null) return true;
            if (HttpContext.Current == null) return true;
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            SessionIDManager manager = new SessionIDManager();
            manager.RemoveSessionID(HttpContext.Current);
            var newId = manager.CreateSessionID(HttpContext.Current);
            bool isRedirected;
            bool isAdded;
            manager.SaveSessionID(HttpContext.Current, newId, out isRedirected, out isAdded);
            return true;
        }

        public static string GetMenu()
        {
            if (HttpContext.Current == null) return string.Empty;
            if (HttpContext.Current.Session["###Menu###"] == null) return string.Empty;
            return HttpContext.Current.Session["###Menu###"].ToString();

        }
        public static void MarkLanguage(string language)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Session["languagecode"] = language;
        }
        public static void MarkAuthenticate(System_User user, Vw_UserInfo userInfo)
        {
            HttpContext.Current.Session["username"] = user.UserName;
            HttpContext.Current.Session["userid"] = user.Id;
            HttpContext.Current.Session["===user==="] = user;
            HttpContext.Current.Session["===userInfo==="] = userInfo;
        }
        public static void MarkPermission(List<Security_VwRoleService> permission)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Session["permission"] = permission;
        }
        public static void MarkRole(List<Security_UserRole> listRole)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Session["roleUser"] = listRole;
        }
        public static void MarkMennu(List<System_Menu> menus)
        {
            var menu = menus.Where(p => (p.ParentId ?? 0) == 0).OrderBy(p => p.MenuOrder).ToList().Aggregate("", (current, item) => current + BuildMenu(item, menus));
            HttpContext.Current.Session["###Menu###"] = menu;

        }
        public static List<Security_VwRoleService> GetPermission()
        {
            if (HttpContext.Current == null) return new List<Security_VwRoleService>();
            if (HttpContext.Current.Session["permission"] == null) return new List<Security_VwRoleService>();
            return (List<Security_VwRoleService>)HttpContext.Current.Session["permission"];
        }
        public static void MarkCaptchar(string captchar)
        {
            if (HttpContext.Current == null) return;
            HttpContext.Current.Session["Captcha"] = captchar;
        }
        public static int GetUserId()
        {
            if (HttpContext.Current == null) return -1;
            if (HttpContext.Current.Session["userid"] == null) return -1;
            return (int)HttpContext.Current.Session["userid"];

        }
        public static System_User GetUser()
        {
            if (HttpContext.Current == null) return null;
            if (HttpContext.Current.Session["===user==="] == null) return null;
            return (System_User)HttpContext.Current.Session["===user==="];
        }
        public static Vw_UserInfo GetUserInfo()
        {
            if (HttpContext.Current == null) return null;
            if (HttpContext.Current.Session["===userInfo==="] == null) return null;
            return (Vw_UserInfo)HttpContext.Current.Session["===userInfo==="];
        }

        static bool CheckPermissionMenu(System_Menu item, List<System_Menu> listMenus)
        {


            var check = CheckAuthorizer.Authorize(Permission.VIEW, item.Controller ?? "");
            if (check)
                return true;
            var listchild = listMenus.Where(p => p.ParentId == item.Id);

            var systemMenus = listchild as System_Menu[] ?? listchild.ToArray();
            if (systemMenus.Any())
            {
                foreach (var subMenu in systemMenus)
                {
                    check = CheckPermissionMenu(subMenu, listMenus);
                    if (check)
                        return true;
                }
            }
            return false;
        }

        static string BuildMenu(System_Menu item, List<System_Menu> listMenus)
        {


            var menu = "";
            if (CheckPermissionMenu(item, listMenus))
            {
                var listChild = listMenus.Where(p => p.ParentId == item.Id).OrderBy(p => p.MenuOrder);

                if (listChild.Any())
                {
                    menu += "<li> <a href=\"index.html\" class=\"waves-effect\"><i class=\"mdi mdi-av-timer fa-fw\" data-icon=\"v\"></i> <span class=\"hide-menu\">" + 
                        item.Text.ToUpper() +
                        "<span class=\"fa arrow\"></span> </span></a>";
                    if (item.MenuLevel != null)
                        menu += "<ul class='nav nav-" + ((MenuLevel)item.MenuLevel).ToString("G") + "-level'>";
                    menu = listChild.Aggregate(menu, (current, submenu) => current + BuildMenu(submenu, listMenus));
                    menu += "</ul></li>";
                }
                else
                {
                    if(item.Area == "") { 
                        menu += Format("<li><a href='/" + (!string.IsNullOrEmpty(item.AliasUrl) ? item.AliasUrl : item.Controller) + "'>" + "&nbsp;<i class=\"ti-info-alt fa-fw\"></i>&nbsp;<span class=\"hide-menu\">" + item.Text + "</span></a></li>");
                    }
                    else
                    {
                        menu += Format("<li><a href='/" + item.Area + "/" + (!string.IsNullOrEmpty(item.AliasUrl)?item.AliasUrl : item.Controller) + "'>" + "&nbsp;<i class=\"ti-info-alt fa-fw\"></i>&nbsp;<span class=\"hide-menu\">&nbsp;" + item.Text + "</span></a></li>");
                    }
                }
            }
            return menu;
        }
        static string BuildMenu1(System_Menu item, List<System_Menu> listMenus)
        {


            var menu = "";
            if (CheckPermissionMenu(item, listMenus))
            {
                var listChild = listMenus.Where(p => p.ParentId == item.Id).OrderBy(p => p.MenuOrder);

                if (listChild.Any())
                {
                    menu += "<li><a><i class='fa fa-plus - square' aria-hidden='true'></i> " + item.Text.ToUpper() + "<span class='fa arrow'></span></a>";
                    if (item.MenuLevel != null)
                        menu += "<ul class='nav nav-" + ((MenuLevel)item.MenuLevel).ToString("G") + "-level menu-level'>";
                    menu = listChild.Aggregate(menu, (current, submenu) => current + BuildMenu(submenu, listMenus));
                    menu += "</ul></li>";
                }
                else
                {
                    if (item.Area == "")
                    {
                        menu += Format("<li><a href='/" + (!string.IsNullOrEmpty(item.AliasUrl) ? item.AliasUrl : item.Controller) + "'>" + "&nbsp;<i class='fa fa-hand-o-right' aria-hidden='true'></i>&nbsp;" + item.Text + "</a></li>");
                    }
                    else
                    {
                        menu += Format("<li><a href='/" + item.Area + "/" + (!string.IsNullOrEmpty(item.AliasUrl) ? item.AliasUrl : item.Controller) + "'>" + "&nbsp;<i class='fa fa-hand-o-right' aria-hidden='true'></i>&nbsp;" + item.Text + "</a></li>");
                    }
                }
            }
            return menu;
        }
        public static string GetLanguageCode()
        {
            if (HttpContext.Current == null) return "vi";
            if (HttpContext.Current.Session["languagecode"] == null) return "vi";
            return (string)HttpContext.Current.Session["languagecode"];

        }
        public static string GetUserName()
        {
            if (HttpContext.Current == null) return null;
            if (HttpContext.Current.Session["username"] == null) return null;
            return HttpContext.Current.Session["username"].ToString();

        }

        public static bool IsUserBackEnd()
        {
            if (GetUserName() == null) return false;
            return true;
        }
    }
    public enum MenuLevel
    {
        Second = 1,
        Third = 2,
    }
}
