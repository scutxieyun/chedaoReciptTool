using System;
using System.Configuration;

namespace AutoUpdate
{
	/// <summary>
	/// AppConst ��ժҪ˵����
	/// </summary>
	public class AppConst
	{
		public static string TempFilePath;

		public static bool GetConfig()
		{
			
			
			TempFilePath=ConfigurationSettings.AppSettings["TempFilePath"];
			return true;
		}

	}
}
