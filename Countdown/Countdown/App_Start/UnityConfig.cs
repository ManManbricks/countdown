using Countdown.Controllers;
using Countdown.Models;
using Countdown.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace Countdown
{
    public static class UnityConfig
    {
        [System.Obsolete]
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ITodoRepository, CountDownTodoRepository>();
            container.RegisterType<DbContext, ApplicationDbContext>(
    new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new HierarchicalLifetimeManager());

            container.RegisterType<AccountController>(
                new InjectionConstructor());

            container.RegisterType<IAuthenticationManager>(
    new InjectionFactory(
        o => System.Web.HttpContext.Current.GetOwinContext().Authentication
    )
);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}