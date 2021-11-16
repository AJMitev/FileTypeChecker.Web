namespace FileTypeChecker.Web.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFileTypesValidation(this IServiceCollection services, params Assembly[] assemblies)
        {
            FileTypeValidator.RegisterCustomTypes(assemblies);

            return services;
        }
    }
}
