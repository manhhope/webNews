using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webNews.Domain.Repositories;
using webNews.Models;
using NLog;
using ServiceStack.OrmLite;
using webNews.Domain.Entities;

namespace webNews.Domain.Services
{
    public class SystemService : ISystemService
    {
        private readonly ISystemRepository _systemRepository;
        private readonly Logger _logger = LogManager.GetLogger("SystemService");
        public SystemService(ISystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        public bool Security_Permission_Update(Security_Permission request)
        {
            return _systemRepository.Security_Permission_Update(request);
        }

        public int Security_Permission_Delete(int id)
        {
            return _systemRepository.Security_Permission_Delete(id);
        }

        public string ReplaceStringWithToken(Dictionary<string, string> tokens, string input)
        {
            if(string.IsNullOrEmpty(input) || tokens == null || tokens.Count == 0) return input;
            var b = new StringBuilder(input);
            foreach(var token in tokens)
            {
                if(!b.ToString().Contains(token.Key)) continue;
                b.Replace(token.Key, token.Value);
            }
            return b.ToString();
        }

        public Task<PagingObject<T>> PagingAsync<T>(SqlExpression<T> query, int? pageIndex = null, int? pageSize = null)
        {
            return _systemRepository.PagingAsync(query, pageIndex, pageSize);
        }

        public PagingObject<T> Paging<T>(SqlExpression<T> query, int? pageIndex = null, int? pageSize = null)
        {
            return _systemRepository.Paging(query, pageIndex, pageSize);
        }
        
        public PagingObject<T> Paging<T>(List<T> list, int? pageIndex = null, int? pageSize = null)
        {
            return _systemRepository.Paging(list, pageIndex, pageSize);
        }

        public HomePageInfo GetPageInfo()
        {
            return _systemRepository.GetPageInfo();
        }

        public List<System_Menu> GetMenu()
        {
            return _systemRepository.GetMenu();
        }

        public List<News> GetNews(int categoryId = -1)
        {
            return _systemRepository.GetNews(categoryId);
        }

        public List<NewsCategory> GetNewCategories()
        {
            return _systemRepository.GetNewCategories();
        }

        public List<ProjectCategory> GetProjectCategories()
        {
            return _systemRepository.GetProjectCategories();
        }

        public List<Project> GetProjects(int categoryId = -1)
        {
            return _systemRepository.GetProjects(categoryId = 1);
        }
    }
}
