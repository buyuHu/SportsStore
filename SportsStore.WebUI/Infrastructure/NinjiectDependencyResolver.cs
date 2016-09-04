using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Web.Mvc;
using Ninject;
using Moq;
using SportsStore.Domain.Concrete;
using System.Configuration;


namespace SportsStore.WebUI.Infractructure
{
    public class NinjectDependcyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependcyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddiBindings();
        }
        private void AddiBindings()
        {
            kernel.Bind<IProductsRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")

            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting", emailSettings);


            //Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product {Name="Football",Price=25 },
            //    new Product {Name="Surf board",Price=179 },
            //    new Product {Name="Running shoes",Price=95 }
            //});
            //kernel.Bind<IProductsRepository>().ToConstant(mock.Object);

        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

    }
}