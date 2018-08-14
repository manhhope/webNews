using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using webNews.Models;
using NLog;
using ServiceStack.OrmLite;
using webNews.Models.Common;
using ServiceStack.Data;
using webNews.Domain.Entities;

namespace webNews.Domain.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly IWebNewsDbConnectionFactory _connectionFactory;
        private readonly Logger _logger = LogManager.GetLogger("SystemRepository");

        public SystemRepository(IWebNewsDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<System_Menu> GetMenu()
        {
            using(var db = _connectionFactory.Open())
            {
                var query = db.From<System_Menu>();
                query = query.Where(x => x.ShowMenu == true && x.Area == "Admin");
                return db.Select(query);
            }
        }

        public List<System_Menu> GetMenuUser()
        {
            using(var db = _connectionFactory.Open())
            {
                var query = db.From<System_Menu>();
                query = query.Where(x => x.ShowMenu == true && x.Area != "Admin");
                return db.Select(query);
            }
        }

        public List<Security_Function> GetListFunctions()
        {
            using(var db = _connectionFactory.Open())
            {
                var query = db.From<Security_Function>();
                return db.Select(query);
            }
        }

        public List<Security_Permission> GetListSecurity_Permission()
        {
            using(var db = _connectionFactory.Open())
            {
                var query = db.From<Security_Permission>();
                return db.Select(query);
            }
        }

        public List<Security_VwRoleService> GetMarkPermission(List<Security_UserRole> listRole)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    var queryRole = listRole.Select(x => x.Id);
                    var queryMark = db.From<Security_VwRoleService>().Where(e => Sql.In(e.RoleID, queryRole));
                    var listper = db.Select(queryMark).ToList();
                    return listper;
                }
            }
            catch(Exception ex)
            {
                _logger.Error("MarkRole fail: " + ex.Message);
                return null;
            }
        }

        public bool Security_Permission_Update(Security_Permission request)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    if(request.Id > 0)
                    {
                        db.Update(request);
                    }
                    else
                    {
                        db.Insert(request);
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Security_Permission_Update fail: " + ex.Message);
                return false;
            }
        }

        public int Security_Permission_Delete(int id)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    return db.Delete<Security_Permission>(new { Id = id });
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Security_Permission_Update fail: " + ex.Message);
                return -1;
            }
        }

        //public string CodeGen(ObjectType objectType, string name = "Z", int number = 10)
        //{
        //    try
        //    {
        //        using(var db = _connectionFactory.Open())
        //        {
        //            var code = db.SingleById<TEMP_CODE>(1);

        //            int id = 0;
        //            var retryCount = 0;
        //            do
        //            {
        //                switch(objectType)
        //                {
        //                    case ObjectType.Staff:
        //                        id = code.Staff++;
        //                        name = PrefixType.Store;
        //                        break;

        //                    default:
        //                        id = code.OtherPerson++;
        //                        name = PrefixType.OtherPerson;
        //                        break;
        //                }

        //                if(id != 0)
        //                {
        //                    try
        //                    {
        //                        number = Constant.LengthCode.LengthCountChar;
        //                        db.Update(code);
        //                        return @"" + name + id.ToString("D" + number);
        //                    }
        //                    catch(OptimisticConcurrencyException ex)
        //                    {
        //                        retryCount++;
        //                        _logger.Info("Get Code error DbUpdateConcurrencyException: " + ex.Message);
        //                        code = db.SingleById<TEMP_CODE>(1);
        //                    }
        //                    catch(Exception ex)
        //                    {
        //                        retryCount++;
        //                        _logger.Info("Get Code error DbUpdateConcurrencyException: " + ex.Message);
        //                        code = db.SingleById<TEMP_CODE>(1);
        //                        _logger.Error(ex);
        //                    }
        //                }
        //            } while(retryCount < 3 || id == 0);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //    }

        //    return "AUTOGEN";
        //}

        public async Task<PagingObject<T>> PagingAsync<T>(SqlExpression<T> query, int? pageIndex = null, int? pageSize = null)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    //Get total items
                    var total = (int)db.Count(query);
                    if(pageIndex == 0 || pageIndex < pageSize)
                        pageIndex = 0;
                    else
                        pageIndex = (pageIndex / pageSize);
                    if(pageIndex != -1 && pageSize != -1)
                    {
                        query.Skip(pageIndex * pageSize).Take(pageSize);
                    }
                    var data = await db.SelectAsync<T>(query);
                    //Get items by current page
                    return new PagingObject<T>
                    {
                        DataList = data,
                        Total = total
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error("StackTrace " + ex.StackTrace + " message: " + ex.Message);
                return null;
            }
        }

        public PagingObject<T> Paging<T>(SqlExpression<T> query, int? pageIndex = null, int? pageSize = null)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    //Get total items
                    var total = (int)db.Count(query);
                    if(pageIndex == 0 || pageIndex < pageSize)
                        pageIndex = 0;
                    else
                        pageIndex = (pageIndex / pageSize);
                    if(pageIndex != -1 && pageSize != -1)
                    {
                        query.Skip(pageIndex * pageSize).Take(pageSize);
                    }
                    var data = db.Select<T>(query);
                    //Get items by current page
                    return new PagingObject<T>
                    {
                        DataList = data,
                        Total = total
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error("StackTrace " + ex.StackTrace + " message: " + ex.Message);
                return null;
            }
        }

        public PagingObject<T> Paging<T>(List<T> list, int? pageIndex = null, int? pageSize = null)
        {
            try
            {
                using(var db = _connectionFactory.Open())
                {
                    //Get total items
                    var total = list.Count;
                    //Get items by current page
                    return new PagingObject<T>
                    {
                        DataList = list.Skip(pageIndex * pageSize ?? 0).Take(pageSize ?? 0).ToList(),
                        Total = total
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error("StackTrace " + ex.StackTrace + " message: " + ex.Message);
                return null;
            }
        }
    }
}