using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000293 RID: 659
	public class Utility
	{
		// Token: 0x06000C23 RID: 3107 RVA: 0x0003EDD9 File Offset: 0x0003D1D9
		public static string GetPlatformName()
		{
			return Utility.GetPlatformForAssetBundles(Application.platform);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003EDE8 File Offset: 0x0003D1E8
		private static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			default:
				if (platform == RuntimePlatform.OSXPlayer)
				{
					return "OSX";
				}
				if (platform == RuntimePlatform.WindowsPlayer)
				{
					return "Windows";
				}
				switch (platform)
				{
				case RuntimePlatform.PS4:
					return "PS4";
				default:
					if (platform == RuntimePlatform.WebGLPlayer)
					{
						return "WebGL";
					}
					if (platform != RuntimePlatform.Switch)
					{
						return null;
					}
					return "Switch";
				case RuntimePlatform.XboxOne:
					return "XboxOne";
				}
				break;
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.LinuxPlayer:
				return "Linux";
			}
		}

		// Token: 0x0400092F RID: 2351
		public const string AssetBundlesOutputPath = "AssetBundles";
	}
}
