using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WTelegram.Enums;

namespace WTelegram
{
    public class ConfigParameters
    {
        private readonly Dictionary<Enums.ConfigEnum, Func<string>> config;
		private readonly Dictionary<Enums.ConfigEnum, Func<string>> defaultConfigValue = DefaultConfig();

		public ConfigParameters(Dictionary<Enums.ConfigEnum, Func<string>> _config)
        {
			this.config = _config;
        }

        internal string Get(ConfigEnum configEnum)
        {
            if (config.ContainsKey(configEnum) && config[configEnum] != null)
            {
                return config[configEnum].Invoke();
            }

			if (defaultConfigValue.ContainsKey(configEnum) && defaultConfigValue[configEnum] != null)
            {
				return defaultConfigValue[configEnum].Invoke();
			}

			return null;
        }

		public static Dictionary<Enums.ConfigEnum, Func<string>> DefaultConfig()
		{
			Dictionary<Enums.ConfigEnum, Func<string>> r = new()
			{
				[Enums.ConfigEnum.session_pathname] = () =>
				{
					return Path.Combine(
						Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar)))
						?? AppDomain.CurrentDomain.BaseDirectory, "WTelegram.session");
				},
				[Enums.ConfigEnum.device_model] = () => { return Environment.Is64BitOperatingSystem ? "PC 64bit" : "PC 32bit"; },
				[Enums.ConfigEnum.server_address] = () => {

#if DEBUG
					return "149.154.167.40:443";  // Test DC 2
#else
					return ""149.154.167.50:443";
#endif

				},
				[Enums.ConfigEnum.system_version] = () => { return Helpers.GetSystemVersion(); },
				[Enums.ConfigEnum.app_version] = () => { return Helpers.GetAppVersion(); },
				[Enums.ConfigEnum.system_lang_code] = () => { return CultureInfo.InstalledUICulture.TwoLetterISOLanguageName; },
				[Enums.ConfigEnum.lang_pack] = () => { return ""; },
				[Enums.ConfigEnum.lang_code] = () => { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; },
				[Enums.ConfigEnum.user_id] = () => { return "-1"; },
				[Enums.ConfigEnum.verification_code] = () => { return Console.ReadLine(); },
				[Enums.ConfigEnum.password] = () => { return Console.ReadLine(); }
			};

			return r;
		}


	}
}