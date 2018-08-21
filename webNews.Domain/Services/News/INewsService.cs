using System.Collections.Generic;
using webNews.Domain.Entities;
using webNews.Domain.Repositories.News;
using webNews.Models;
using webNews.Models.News;

namespace webNews.Domain.Services.News
{
    public interface INewsService : IService<Entities.News>
    {
        PagingObject<Entities.News> GetListNews(NewsSearchModel model, int pageIndex, int pageSize);
    }
}
