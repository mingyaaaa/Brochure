using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Brochure.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brochure.DI.AspectCore
{
    public class AspectCoreDI : IDIServiceManager
    {
        private IServiceContainer _container;

        public AspectCoreDI(IServiceCollection serviceCollection)
        {
            _container = serviceCollection.ToServiceContainer();
        }
        public IDIServiceManager AddScoped(Type serviceType, Type implementationType)
        {
            _container.AddType(serviceType, implementationType, Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddScoped(Type serviceType)
        {
            _container.AddType(serviceType, Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddScoped<TService>() where TService : class
        {
            _container.AddType<TService>();
            return this;
        }

        public IDIServiceManager AddScoped<TService>(Func<object, TService> implementationFactory) where TService : class
        {
            _container.AddType(typeof(TService), implementationFactory.GetType(), Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddScoped<TService, TImpService>(TImpService implementationInstance)
            where TService : class
            where TImpService : TService
        {
            _container.AddType(typeof(TService), implementationInstance.GetType(), Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
        {
            _container.AddType<TService, TImplementation>(Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddScoped<TService>(TService serviceType) where TService : class
        {
            _container.AddType(serviceType.GetType(), Lifetime.Scoped);
            return this;
        }

        public IDIServiceManager AddSingleton(Type serviceType, Type implementationType)
        {
            _container.AddType(serviceType, implementationType, Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton(Type serviceType)
        {
            _container.AddType(serviceType, Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton<TService>() where TService : class
        {
            _container.AddType<TService>(Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton<TService>(Func<object, TService> implementationFactory) where TService : class
        {
            _container.AddType(typeof(TService), implementationFactory.GetType(), Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton<TService, TImpService>(TImpService implementationInstance)
            where TService : class
            where TImpService : TService
        {
            _container.AddType(typeof(TService), implementationInstance.GetType(), Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
        {
            _container.AddType<TService, TImplementation>(Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddSingleton<TService>(TService serviceType) where TService : class
        {
            _container.AddType(serviceType.GetType(), Lifetime.Singleton);
            return this;
        }

        public IDIServiceManager AddTransient(Type serviceType, Type implementationType)
        {
            _container.AddType(serviceType, implementationType, Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient(Type serviceType)
        {
            _container.AddType(serviceType, Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient<TService>() where TService : class
        {
            _container.AddType<TService>(Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient<TService>(Func<object, TService> implementationFactory) where TService : class
        {
            _container.AddType(typeof(TService), implementationFactory.GetType(), Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient<TService, TImpService>(TImpService implementationInstance)
            where TService : class
            where TImpService : TService
        {
            _container.AddType(typeof(TService), implementationInstance.GetType(), Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
        {
            _container.AddType<TService, TImplementation>(Lifetime.Transient);
            return this;
        }

        public IDIServiceManager AddTransient<TService>(TService serviceType) where TService : class
        {
            _container.AddType(serviceType.GetType(), Lifetime.Transient);
            return this;
        }

        public IServiceProvider BuildServiceProvider()
        {
            return _container.Build();
        }
    }
}
