using PostManagment;
using UserManagment;
using UserManagment.Application;
using DataAccess.Application;
using DataAccess;
using FileManagment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using ProjectManagment.Application;
using ProjectManagment;
using DonationManagment.Application;
using DonationManagment;
using Common;
using EventManagment;
using MGSUCore.Authentification.Handlers;

namespace MGSUCore
{
    public class Bootstraper
    {
        private readonly IServiceCollection _services;
        private readonly IConfigurationRoot _configuration;

        public Bootstraper(IServiceCollection services, IConfigurationRoot configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Configure()
        {
            _services //DI Work
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddSingleton<IUserManager, UserManager>()
                .AddSingleton<IPostManager, PostManager>()
                .AddSingleton<IContactManager, ContactManager>()
                .AddSingleton<IProjectManager, ProjectManager>()
                .AddSingleton<IDonationManager, DonationManager>()
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IImageResizer, ImageResizer>()
                .AddSingleton<IAttendanceManager, AttendanceManager>()
                .AddSingleton<IEventManager, EventManager>()

                //Register auth middleware
                .AddSingleton<IAuthorizationHandler, IsAuthentificatedAuthHandler>()
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>();

            //Register Settings
            _services.Configure<DeploySettings>(options => _configuration.GetSection("DeploySettings").Bind(options));
            _services.Configure<FileStorageSettings>(options => _configuration.GetSection("FileStorageSettings")
                .Bind(options));

            var sessionProvider = new SessionProvider(_configuration["ConnectionStrings:Mongo"],new EntityMapper());
            _services.AddSingleton<ISessionProvider, SessionProvider>(
                (arg) => sessionProvider);
            //await DataConstraintsProvider.CreateConstraints(sessionProvider);
        }
    }
}