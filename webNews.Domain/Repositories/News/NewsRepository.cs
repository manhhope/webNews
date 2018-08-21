using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using webNews.Models;
using NLog;
using ServiceStack.OrmLite;
using webNews.Domain.Entities;
using webNews.Models.News;

namespace webNews.Domain.Repositories.News
{
    public class NewsRepository : Repository<Entities.News>, INewsRepository
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IWebNewsDbConnectionFactory _connectionFactory;

        public NewsRepository(IWebNewsDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public PagingObject<Entities.News> GetListNews(NewsSearchModel model, int pageIndex, int pageSize)
        {
            try
            {
                using (var db = _connectionFactory.Open())
                {
                    var query = db.From<Entities.News>();
                    if (!string.IsNullOrEmpty(model.Title))
                    {
                        query.Where(e => e.Title.Contains(model.Title));
                    }

                    if (model.CategoryId != -1)
                    {
                        query.Where(x => x.CategoryId == model.CategoryId);
                    }
                    
                    var total = db.Scalar<int>(query.ToCountStatement(), query.Params);
                    query.Skip(pageIndex * pageSize).Take(pageSize);
                    var lst = db.Select(query);
                    var data = new PagingObject<Entities.News>()
                    {
                        Total = total,
                        DataList = lst
                    };
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.Info("GetListNews is error: " + ex);
                return null;
            }
        }
    }
}
