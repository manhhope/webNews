using System.Collections.Generic;

namespace webNews.Models
{
    public class PagingObject<T>
    {
        public List<T> DataList { get; set; }
        public int Total { get; set; }
        public string ExtendInfo { get; set; }
    }
}
