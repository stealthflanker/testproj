using Autofac;

namespace ReferenceService.WebApi.Repositories
{
    internal class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReferenceRepository>().AsImplementedInterfaces();
        }
    }
}
