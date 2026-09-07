using Application;
using Domain.Interfaces.Services;
using Domain.Options;
using FluentEmail.MailKitSmtp;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Web.BackgroundServices;

namespace Web.Extensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Options
        services.Configure<CloudinaryOptions>(
            configuration.GetSection(CloudinaryOptions.SectionName)
        );
        services.Configure<EmailSenderOptions>(
            configuration.GetSection(EmailSenderOptions.SectionName)
        );

        // Razor Components
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Logging
        services.AddLogging();
        
        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly)
        );

        // Services
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddSingleton<IEmailQueueService, EmailQueueService>();
        services.AddSingleton<IEmailTemplateRenderer, FluidEmailTemplateRenderer>();

        // Background Services
        services.AddHostedService<EmailBackgroundService>();
        
        // Email Configuration
        var emailSection = configuration.GetSection("Email");
        services
            .AddFluentEmail(emailSection["From"])
            .AddMailKitSender(new SmtpClientOptions
            {
                Server = emailSection["SmtpServer"],
                Port = int.Parse(emailSection["Port"]!),
                UseSsl = true,
                RequiresAuthentication = true,
                User = emailSection["From"],
                Password = emailSection["Password"]
            });

        return services;
    }
}