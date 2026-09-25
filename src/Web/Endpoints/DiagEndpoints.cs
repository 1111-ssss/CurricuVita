// TEMPORARY diagnostics for the missing _framework/blazor.web.js on hosting.
// TODO: remove after diagnosis.
namespace Web.Endpoints;

public static class DiagEndpoints
{
    public static IEndpointRouteBuilder MapDiagEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/diag", (IWebHostEnvironment env) =>
        {
            var webRoot = env.WebRootPath;
            string[] entries;
            try
            {
                entries = Directory.Exists(webRoot)
                    ? Directory.GetFileSystemEntries(webRoot).Select(Path.GetFileName).OfType<string>().ToArray()
                    : ["<webroot-missing>"];
            }
            catch (Exception ex)
            {
                entries = [$"<error: {ex.Message}>"];
            }

            var fwDir = Path.Combine(webRoot, "_framework");
            var fwFile = new FileInfo(Path.Combine(fwDir, "blazor.web.js"));

            string[] fwEntries;
            try
            {
                fwEntries = Directory.Exists(fwDir)
                    ? Directory.GetFileSystemEntries(fwDir).Select(Path.GetFileName).OfType<string>().ToArray()
                    : ["<framework-dir-missing>"];
            }
            catch (Exception ex)
            {
                fwEntries = [$"<error: {ex.Message}>"];
            }

            return Results.Json(new
            {
                environment = env.EnvironmentName,
                contentRoot = env.ContentRootPath,
                webRoot,
                webRootExists = Directory.Exists(webRoot),
                webRootEntries = entries,
                frameworkFileExists = fwFile.Exists,
                frameworkFileLength = fwFile.Exists ? fwFile.Length : 0,
                frameworkDirEntries = fwEntries,
                runtime = Environment.Version.ToString(),
            });
        });

        return endpoints;
    }
}
