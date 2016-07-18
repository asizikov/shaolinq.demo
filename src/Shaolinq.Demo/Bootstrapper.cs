using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using Shaolinq.Demo.DataAccess.Mapping;
using Shaolinq.Demo.DataAccess.Repository;
using Shaolinq.Demo.Domain.Repository;
using Shaolinq.Demo.Model;
using Shaolinq.SqlServer;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Shaolinq.Demo
{
    [UsedImplicitly]
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override NancyInternalConfiguration InternalConfiguration
            => NancyInternalConfiguration.WithOverrides(c => { c.ResponseProcessors.Remove(typeof(ViewProcessor)); });

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            var configuration = LoadConfiguration();
            container.Register<IConfiguration>(configuration);
            BuildDbIfNeeded(configuration, true);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(ConfigureAutomapper());
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IAuthorRepository, AuthorRepository>();
            container.Register<IPostRepository, PostRepository>();
            container.Register((c, n) => BuildDbIfNeeded(c.Resolve<IConfiguration>(), false));
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configurationRoot = configurationBuilder.Build();
            return configurationRoot;
        }

        private static IMapper ConfigureAutomapper()
        {
            var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<DataAccessProfile>(); });
            return new Mapper(mapperConfiguration);
        }

        private static BlogModel BuildDbIfNeeded(IConfiguration configurationRoot, bool seed)
        {
            var dbName = configurationRoot.GetSection("Database:Name").Value;
            var server = configurationRoot.GetSection("Database:Server").Value;
            var configuration = SqlServerConfiguration.Create(dbName, server);
            var model = DataAccessModel.BuildDataAccessModel<BlogModel>(configuration);
            if (seed)
            {
                model.Create(DatabaseCreationOptions.IfDatabaseNotExist);
            }
            return model;
        }
    }
}