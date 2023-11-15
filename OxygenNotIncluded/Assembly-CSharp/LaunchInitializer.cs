using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x0200083C RID: 2108
public class LaunchInitializer : MonoBehaviour
{
	// Token: 0x06003D58 RID: 15704 RVA: 0x001545F6 File Offset: 0x001527F6
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x001545FD File Offset: 0x001527FD
	public static int UpdateNumber()
	{
		return 49;
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00154604 File Offset: 0x00152804
	private void Update()
	{
		if (this.numWaitFrames > Time.renderedFrameCount)
		{
			return;
		}
		if (!DistributionPlatform.Initialized)
		{
			if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				global::Debug.LogError("Machine does not support RGBAFloat32");
			}
			GraphicsOptionsScreen.SetSettingsFromPrefs();
			Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
			global::Debug.Log("Current date: " + System.DateTime.Now.ToString());
			global::Debug.Log("release Build: " + BuildWatermark.GetBuildText());
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			KPlayerPrefs.instance.Load();
			DistributionPlatform.Initialize();
		}
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		global::Debug.Log("DistributionPlatform initialized.");
		global::Debug.Log("release Build: " + BuildWatermark.GetBuildText());
		global::Debug.Log(string.Format("EXPANSION1 installed: {0}  active: {1}", DlcManager.IsExpansion1Installed(), DlcManager.IsExpansion1Active()));
		KFMOD.Initialize();
		for (int i = 0; i < this.SpawnPrefabs.Length; i++)
		{
			GameObject gameObject = this.SpawnPrefabs[i];
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
		LaunchInitializer.DeleteLingeringFiles();
		base.enabled = false;
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x00154724 File Offset: 0x00152924
	private static void DeleteLingeringFiles()
	{
		string[] array = new string[]
		{
			"fmod.log",
			"load_stats_0.json",
			"OxygenNotIncluded_Data/output_log.txt"
		};
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		foreach (string path in array)
		{
			string path2 = Path.Combine(directoryName, path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
			}
		}
	}

	// Token: 0x04002800 RID: 10240
	private const string PREFIX = "U";

	// Token: 0x04002801 RID: 10241
	private const int UPDATE_NUMBER = 49;

	// Token: 0x04002802 RID: 10242
	private static readonly string BUILD_PREFIX = "U" + 49.ToString();

	// Token: 0x04002803 RID: 10243
	public GameObject[] SpawnPrefabs;

	// Token: 0x04002804 RID: 10244
	[SerializeField]
	private int numWaitFrames = 1;
}
