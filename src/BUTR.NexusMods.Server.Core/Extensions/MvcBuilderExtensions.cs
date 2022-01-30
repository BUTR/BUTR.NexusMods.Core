using Microsoft.Extensions.DependencyInjection;

namespace BUTR.NexusMods.Server.Core.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddServerCore(this IMvcBuilder builder)
        {
            builder.AddApplicationPart(typeof(MvcBuilderExtensions).Assembly);
            return builder;
        }
    }
}