using System;
using System.Collections.Generic;
using System.Web.Mvc;
using webNews.Domain;
using webNews.Language.Language;
using webNews.Areas.Admin.Models;
using webNews.Controllers;
using webNews.Domain.Services.RoleManage;
using NLog;
using webNews.Security;
using webNews.Domain.Entities;

namespace webNews.Areas.Admin.Controllers
{
    public class RoleManageController : BaseController
    {
        #region khởi tạo
        private readonly Logger _log = LogManager.GetLogger("RoleManageController");
        private readonly IRoleManageService _roleManageService;
        public RoleManageController(IRoleManageService roleManageService)
        {
            _roleManageService = roleManageService;
        }

        #endregion

        #region trang index
        public ActionResult Index()
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            if (!CheckAuthorizer.Authorize(Permission.VIEW))
                return RedirectToAction("Permission", "Error");
            return View();
        }
        #endregion

        #region tìm kiếm 
        [HttpPost]
        public ActionResult Search(string name, int offset, int limit)
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            try
            {
                int pageIndex;

                if (offset == 0 || offset < limit)
                    pageIndex = 0;
                else
                    pageIndex = (offset / limit);

                var list = _roleManageService.GetListSecurity_Role(name, pageIndex, limit);
                var total = 0;
                if (list == null)
                {
                    total = 0;
                    return Json(new
                    {
                        total
                    }, JsonRequestBehavior.AllowGet);
                }
                var data = list.DataList;

                total = list.Total;
                return Json(new
                {
                    data,
                    total
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error("GetData in RoleManageController error : " + ex);
                return null;
            }
        }

        #endregion

        #region Bind popup Thêm - xác nhận
        [HttpPost]
        public ActionResult BindPupop(int code, string action)
        {
            var model = new RoleManagerCreateModel
            {
                Action = action,
                ListFunctions = _roleManageService.GetListFunctions(),
                ListPermissions = _roleManageService.GetListPermissions()
            };
            #region thông tin
            if (code > 0)
            {
                model.Role = _roleManageService.GetRole(code);
                model.ListMarked = _roleManageService.GetListRoleMark(code);
            }
            else
            {
                model.Role = new Security_Role();
                model.ListMarked = new List<Security_VwRoleService>();
            }

            #endregion
            #region return    
            return PartialView("Add", model);
            #endregion
        }
        #endregion
        #region Thêm sửa xóa
        public ActionResult UpdateRole(int Id, string RoleName, string functionAndPermission)
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            if (!CheckAuthorizer.Authorize(Id == 0 ? Permission.ADD : Permission.EDIT))
                return Json(new
                {
                    Status = "00",
                    Message = string.Format(Resource.PermissionContent_Lang)
                }, JsonRequestBehavior.AllowGet);

            var rs = _roleManageService.UpdateRole(new Security_Role() { Id = Id, RoleName = RoleName }, functionAndPermission);
            if (rs)
            {
                return Json(new
                {
                    Status = rs,
                    Message = rs ? "Thêm mới nhóm quyền thành công" : "Có lỗi trong quá trình xử lý"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Status = rs,
                Message = rs ? "Cập nhật thành công" : "Có lỗi trong quá trình xử lý"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRole(int id)
        {
            if (!CheckAuthorizer.IsAuthenticated())
                return RedirectToAction("Index", "Login");
            if (!CheckAuthorizer.Authorize(Permission.DELETE))
                return Json(new
                {
                    Status = "00",
                    Message = string.Format(Resource.PermissionContent_Lang)
                }, JsonRequestBehavior.AllowGet);

            var rs = _roleManageService.Delete(id);
            return Json(new
            {
                Status = rs,
                Message = rs > 0 ? "Xóa thành công" : "Có lỗi trong quá trình xử lý"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
