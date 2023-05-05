using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsGrid2Mvc.Configuration
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class ConfigurationExtensions
    {
        const string UI_JSGRID = "UIDefaults:Grid";

        public static IConfigurationSection GetGridSection(this IConfiguration config)
        {
            return config.GetSection(UI_JSGRID);
		}
        /// <summary>
        /// Reads appasettings section to retreive shared gridsettings
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static GridSettings GetGridSettings(this IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var gridSettings = new GridSettings();
            config.GetGridSection().Bind(gridSettings); 
            return gridSettings;
        }
    }
}
