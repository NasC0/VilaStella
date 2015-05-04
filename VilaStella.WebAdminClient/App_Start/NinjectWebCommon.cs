[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VilaStella.WebAdminClient.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(VilaStella.WebAdminClient.App_Start.NinjectWebCommon), "Stop")]

namespace VilaStella.WebAdminClient.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using VilaStella.Data;
    using VilaStella.Data.Common.Repositories;
    using VilaStella.Models;
    using VilaStella.Web.Common.Contracts;
    using VilaStella.Web.Common.Factories;
    using VilaStella.Web.Common.Helpers;
    using VilaStella.WebAdminClient.Infrastructure;
    using VilaStella.WebAdminClient.Infrastructure.Contracts;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDbContext>().To<DefaultDbContext>();
            kernel.Bind<DbContext>().To<DefaultDbContext>();

            kernel.Bind(typeof(IDeletableRepository<>))
                .To(typeof(DeletableEntityRepository<>));

            kernel.Bind(typeof(IGenericRepositoy<>)).To(typeof(GenericRepository<>));

            kernel.Bind<IRandomReservationGenerator>().To<RandomReservationGenerator>();

            kernel.Bind<IFilterFactory>().To<FilterFactory>();

            kernel.Bind<ICapparoFactory>().To<CapparoFactory>();

            kernel.Bind<ICalculatePricing>().To<CalculatePricing>();

            kernel.Bind<IReservationManager>().To<ReservationManager>();

            kernel.Bind<IOverlapDatesManager>().To<OverlapDatesManager>();
        }        
    }
}
