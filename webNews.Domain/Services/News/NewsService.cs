using System.Collections.Generic;
using webNews.Domain.Entities;
using webNews.Domain.Repositories;
using webNews.Domain.Repositories.News;
using webNews.Domain.Repositories.RoleManage;
using webNews.Models;
using webNews.Models.News;

namespace webNews.Domain.Services.News
{
    public class NewsService : Service<Entities.News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(IRepository<Entities.News> repository, INewsRepository newsRepository) : base(repository)
        {
            _newsRepository = newsRepository;
        }


        public PagingObject<Entities.News> GetListNews(NewsSearchModel model, int pageIndex, int pageSize)
        {
            if (pageIndex == 0 || pageIndex < pageSize)
            {
                pageSize = 0;
            }
            else
            {
                pageSize = (pageIndex / pageSize);
            }
            return _newsRepository.GetListNews(model, pageIndex, pageSize);
        }
    }
}
