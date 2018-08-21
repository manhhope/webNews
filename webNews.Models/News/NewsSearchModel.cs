using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webNews.Models.News
{
    public class NewsSearchModel
    {
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
    }
}
