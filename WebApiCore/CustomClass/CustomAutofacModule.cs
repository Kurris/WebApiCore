using Autofac;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ligy.Project.WebApi.CustomClass
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //string localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Assembly service = Assembly.LoadFrom(Path.Combine(localPath, "WebApiCore.IOC.Service.dll"));

            //Assembly iservice = Assembly.Load("WebApiCore.IOC.Interface");

            //builder.RegisterAssemblyTypes(service, iservice)
            //    .InstancePerLifetimeScope()
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired();

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => x.Name.EndsWith("Controller")).PropertiesAutowired();
        }
    }
}
