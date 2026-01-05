using Helpers.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Web;

public static class DIContainerExtensions
{
    public static IServiceCollection RegisterTranslation(IServiceCollection services, IConfiguration configuration)
        => services.AddSingleton<ITranslationHandler>(
            configuration.GetRequiredSection("Translation").Get<TranslationHandler>()!);
}
