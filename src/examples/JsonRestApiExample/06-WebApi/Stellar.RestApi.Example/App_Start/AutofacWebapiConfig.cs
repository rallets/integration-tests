using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;

namespace Stellar.RestApi.Example.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Repository>()
                   .As<IRepository>()
                   .SingleInstance();

            //builder.RegisterType<DbTestEntities>()
            //       .As<DbContext>()
            //       .InstancePerRequest();

            //builder.RegisterType<DbFactory>()
            //       .As<IDbFactory>()
            //       .InstancePerRequest();

            //builder.RegisterType<OrderService>()
            //      .As<IOrderService>()
            //      .InstancePerRequest();

            //builder.RegisterType<DbFactory>()
            //      .As<IDbFactory>()
            //      .InstancePerRequest();

            //builder.RegisterGeneric(typeof(EfRepository<>))
            //       .As(typeof(IRepository<>))
            //       .InstancePerRequest();

            //RegisterAutomapper(builder);

            Container = builder.Build();

            return Container;
        }

        //private static void RegisterAutomapper(ContainerBuilder builder)
        //{
        //    var profiles = from t in typeof(AutofacWebapiConfig).Assembly.GetTypes()
        //                   where typeof(Profile).IsAssignableFrom(t)
        //                   select (Profile)Activator.CreateInstance(t);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        foreach (var profile in profiles)
        //        {
        //            cfg.AddProfile(profile);
        //        }
        //    });

        //    var mapper = config.CreateMapper();

        //    builder.RegisterInstance(mapper).As<IMapper>();
        //}
    }
}