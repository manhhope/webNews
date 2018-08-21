using System.Collections.Generic;
using webNews.Domain.Entities;
using webNews.Models;
using webNews.Models.News;

namespace webNews.Domain.Repositories.News
{
    public interface INewsRepository : IRepository<Entities.News>
    {
        PagingObject<Entities.News> GetListNews(NewsSearchModel model, int pageIndex, int pageSize);
    }
}
