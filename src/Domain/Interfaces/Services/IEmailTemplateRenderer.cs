namespace Domain.Interfaces.Services;

public interface IEmailTemplateRenderer
{
    Task<string> Render<TModel>(string template, TModel model);
}