using System.Configuration;

using Autofac;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace webNews.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            OrmLiteConfig.CommandTimeout = 360;
            OrmLiteConfig.DialectProvider = SqlServer2014Dialect.Provider;
            OrmLiteConfig.DialectProvider.GetStringConverter().UseUnicode = true;

            builder.Register<IWebNewsDbConnectionFactory>(
                p =>
                    new WebNewsDbConnectionFactory(ConfigurationManager.ConnectionStrings["WebNews"].ConnectionString, SqlServer2014OrmLiteDialectProvider.Instance));

            base.Load(builder);
        }
    }
}