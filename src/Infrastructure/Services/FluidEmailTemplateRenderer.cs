using Domain.Interfaces.Services;
using Fluid;
using System.Collections.Concurrent;
using System.Reflection;

namespace Infrastructure.Services;

public class FluidEmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly FluidParser _parser = new();
    private readonly Assembly _assembly = typeof(FluidEmailTemplateRenderer).Assembly;
    private readonly ConcurrentDictionary<string, IFluidTemplate> _cache = new();

    public FluidEmailTemplateRenderer() { }

    public async Task<string> Render<TModel>(string templateName, TModel model)
    {
        var template = _cache.GetOrAdd(templateName, name =>
        {
            var resourceName = _assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith($"{name}.liquid", StringComparison.OrdinalIgnoreCase));

            if (resourceName is null)
                throw new FileNotFoundException($"Email template '{name}' not found in embedded resources");

            using var stream = _assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Could not load resource {resourceName}");

            using var reader = new StreamReader(stream);
            var templateContent = reader.ReadToEnd();

            if (!_parser.TryParse(templateContent, out var parsed, out var error))
                throw new InvalidOperationException($"Failed to parse template '{name}': {error}");

            return parsed;
        });

        var context = new TemplateContext(model);

        return await template.RenderAsync(context);
    }
}