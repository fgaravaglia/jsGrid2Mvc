using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace jsGrid2Mvc.Configuration
{
	/// <summary>
	/// Extensions to manage services into DI
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Add default services for NDataHub to create POC
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config"></param>
		public static void AddCommonGridViewSettings(this IServiceCollection services, IConfiguration config)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));
			if (config == null)
				throw new ArgumentNullException(nameof(config));
			services.AddOptions<GridSettings>().Bind(config.GetGridSection());
		}
	}
}
