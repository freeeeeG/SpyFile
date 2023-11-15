using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles
{
	// Token: 0x02000290 RID: 656
	public class AssetBundleManager : MonoBehaviour
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0003E1DE File Offset: 0x0003C5DE
		public static string GetStreamingAssetsPath()
		{
			return Path.Combine(Application.streamingAssetsPath, "Windows");
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0003E1EF File Offset: 0x0003C5EF
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x0003E1F6 File Offset: 0x0003C5F6
		public static AssetBundleManager.LogMode logMode
		{
			get
			{
				return AssetBundleManager.m_LogMode;
			}
			set
			{
				AssetBundleManager.m_LogMode = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0003E1FE File Offset: 0x0003C5FE
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x0003E205 File Offset: 0x0003C605
		public static string BaseDownloadingURL
		{
			get
			{
				return AssetBundleManager.m_BaseDownloadingURL;
			}
			set
			{
				AssetBundleManager.m_BaseDownloadingURL = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0003E20D File Offset: 0x0003C60D
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x0003E214 File Offset: 0x0003C614
		public static string[] ActiveVariants
		{
			get
			{
				return AssetBundleManager.m_ActiveVariants;
			}
			set
			{
				AssetBundleManager.m_ActiveVariants = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0003E224 File Offset: 0x0003C624
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0003E21C File Offset: 0x0003C61C
		public static AssetBundleManifest AssetBundleManifestObject
		{
			get
			{
				return AssetBundleManager.m_AssetBundleManifest;
			}
			set
			{
				AssetBundleManager.m_AssetBundleManifest = value;
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0003E22B File Offset: 0x0003C62B
		private static void Log(AssetBundleManager.LogType logType, string text)
		{
			if (logType == AssetBundleManager.LogType.Error)
			{
				Debug.LogError("[AssetBundleManager] " + text);
			}
			else if (AssetBundleManager.m_LogMode == AssetBundleManager.LogMode.All)
			{
				Debug.Log("[AssetBundleManager] " + text);
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0003E263 File Offset: 0x0003C663
		public static void SetSourceAssetBundleDirectory(string relativePath)
		{
			AssetBundleManager.BaseDownloadingURL = AssetBundleManager.GetStreamingAssetsPath() + relativePath;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0003E275 File Offset: 0x0003C675
		public static void SetSourceAssetBundleURL(string absolutePath)
		{
			AssetBundleManager.BaseDownloadingURL = absolutePath + Utility.GetPlatformName() + "/";
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0003E28C File Offset: 0x0003C68C
		public static void SetDevelopmentAssetBundleServer()
		{
			TextAsset textAsset = Resources.Load("AssetBundleServerURL") as TextAsset;
			string text = (!(textAsset != null)) ? null : textAsset.text.Trim();
			if (text == null || text.Length == 0)
			{
				Debug.LogError("Development Server URL could not be found.");
			}
			else
			{
				AssetBundleManager.SetSourceAssetBundleURL(text);
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0003E2F0 File Offset: 0x0003C6F0
		public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			if (AssetBundleManager.m_LoadingErrors.TryGetValue(assetBundleName, out error))
			{
				return null;
			}
			LoadedAssetBundle loadedAssetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
			if (loadedAssetBundle == null)
			{
				return null;
			}
			string[] array = null;
			if (!AssetBundleManager.m_Dependencies.TryGetValue(assetBundleName, out array))
			{
				return loadedAssetBundle;
			}
			foreach (string key in array)
			{
				if (AssetBundleManager.m_LoadingErrors.TryGetValue(assetBundleName, out error))
				{
					return loadedAssetBundle;
				}
				LoadedAssetBundle loadedAssetBundle2;
				AssetBundleManager.m_LoadedAssetBundles.TryGetValue(key, out loadedAssetBundle2);
				if (loadedAssetBundle2 == null)
				{
					return null;
				}
			}
			return loadedAssetBundle;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0003E388 File Offset: 0x0003C788
		public static AssetBundleLoadManifestOperation Initialize()
		{
			return AssetBundleManager.Initialize(Utility.GetPlatformName());
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0003E394 File Offset: 0x0003C794
		public static AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
		{
			GameObject gameObject = new GameObject("AssetBundleManager", new Type[]
			{
				typeof(AssetBundleManager)
			});
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			if (AssetBundleManager.m_Instance != null && AssetBundleManager.m_Instance.gameObject != gameObject)
			{
				AssetBundleManager.m_Instance.gameObject.Destroy();
				AssetBundleManager.m_Instance = null;
			}
			AssetBundleManager.m_Instance = gameObject.GetComponent<AssetBundleManager>();
			AssetBundleManager.LoadAssetBundle(manifestAssetBundleName, true, true);
			AssetBundleLoadManifestOperation assetBundleLoadManifestOperation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadManifestOperation);
			return assetBundleLoadManifestOperation;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0003E434 File Offset: 0x0003C834
		protected static void LoadAssetBundle(string assetBundleName, bool async, bool isLoadingAssetBundleManifest = false)
		{
			AssetBundleManager.Log(AssetBundleManager.LogType.Info, "Loading Asset Bundle " + ((!isLoadingAssetBundleManifest) ? ": " : "Manifest: ") + assetBundleName);
			if (!isLoadingAssetBundleManifest && AssetBundleManager.m_AssetBundleManifest == null)
			{
				Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}
			AssetBundleManager.LoadAssetBundleInternal(assetBundleName, async, isLoadingAssetBundleManifest);
			if (!isLoadingAssetBundleManifest)
			{
				AssetBundleManager.LoadDependencies(assetBundleName, async);
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0003E4A0 File Offset: 0x0003C8A0
		protected static string RemapVariantName(string assetBundleName)
		{
			string[] allAssetBundlesWithVariant = AssetBundleManager.m_AssetBundleManifest.GetAllAssetBundlesWithVariant();
			string[] array = assetBundleName.Split(new char[]
			{
				'.'
			});
			int num = int.MaxValue;
			int num2 = -1;
			for (int i = 0; i < allAssetBundlesWithVariant.Length; i++)
			{
				string[] array2 = allAssetBundlesWithVariant[i].Split(new char[]
				{
					'.'
				});
				if (!(array2[0] != array[0]))
				{
					int num3 = Array.IndexOf<string>(AssetBundleManager.m_ActiveVariants, array2[1]);
					if (num3 == -1)
					{
						num3 = 2147483646;
					}
					if (num3 < num)
					{
						num = num3;
						num2 = i;
					}
				}
			}
			if (num == 2147483646)
			{
				Debug.LogWarning("Ambigious asset bundle variant chosen because there was no matching active variant: " + allAssetBundlesWithVariant[num2]);
			}
			if (num2 != -1)
			{
				return allAssetBundlesWithVariant[num2];
			}
			return assetBundleName;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0003E56C File Offset: 0x0003C96C
		protected static void LoadDependencies(string assetBundleName, bool async)
		{
			if (AssetBundleManager.m_AssetBundleManifest == null)
			{
				Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}
			string[] allDependencies = AssetBundleManager.m_AssetBundleManifest.GetAllDependencies(assetBundleName);
			if (allDependencies.Length == 0)
			{
				return;
			}
			for (int i = 0; i < allDependencies.Length; i++)
			{
				allDependencies[i] = AssetBundleManager.RemapVariantName(allDependencies[i]);
			}
			if (!AssetBundleManager.m_Dependencies.ContainsKey(assetBundleName))
			{
				AssetBundleManager.m_Dependencies.Add(assetBundleName, allDependencies);
			}
			for (int j = 0; j < allDependencies.Length; j++)
			{
				AssetBundleManager.LoadAssetBundleInternal(allDependencies[j], async, false);
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0003E604 File Offset: 0x0003CA04
		protected static bool LoadAssetBundleInternal(string assetBundleName, bool async, bool isLoadingAssetBundleManifest)
		{
			LoadedAssetBundle loadedAssetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
			if (loadedAssetBundle != null)
			{
				loadedAssetBundle.m_ReferencedCount++;
				return true;
			}
			LoadingAssetBundle loadingAssetBundle = null;
			AssetBundleManager.m_LoadingAssetBundles.TryGetValue(assetBundleName, out loadingAssetBundle);
			if (loadingAssetBundle != null)
			{
				if (!async)
				{
					Debug.LogError(assetBundleName + " could not be loaded synchronously as the bundle is currently being async loaded!");
				}
				loadingAssetBundle.m_ReferencedCount++;
				return true;
			}
			if (async)
			{
				Debug.Log(string.Format("Initializing file request with path : \"{0}\"", AssetBundleManager.GetStreamingAssetsPath() + "/" + assetBundleName));
				AssetBundleManager.m_Instance.StartCoroutine(AssetBundleManager.m_Instance.LoadAssetBundleFromFileAsync(assetBundleName));
			}
			else
			{
				Debug.Log(string.Format("Initializing syncronous file request with path : \"{0}\"", AssetBundleManager.GetStreamingAssetsPath() + "/" + assetBundleName));
				AssetBundleManager.m_Instance.LoadAssetBundleFromFile(assetBundleName);
			}
			return false;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0003E6DE File Offset: 0x0003CADE
		public static void UnloadAssetBundle(string assetBundleName)
		{
			AssetBundleManager.UnloadAssetBundleInternal(assetBundleName);
			AssetBundleManager.UnloadDependencies(assetBundleName);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0003E6EC File Offset: 0x0003CAEC
		protected static void UnloadDependencies(string assetBundleName)
		{
			string[] array = null;
			if (!AssetBundleManager.m_Dependencies.TryGetValue(assetBundleName, out array))
			{
				return;
			}
			foreach (string assetBundleName2 in array)
			{
				AssetBundleManager.UnloadAssetBundleInternal(assetBundleName2);
			}
			LoadedAssetBundle loadedAssetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
			if (loadedAssetBundle == null)
			{
				AssetBundleManager.m_Dependencies.Remove(assetBundleName);
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0003E754 File Offset: 0x0003CB54
		protected static void UnloadAssetBundleInternal(string assetBundleName)
		{
			string text;
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(assetBundleName, out text);
			if (loadedAssetBundle == null)
			{
				LoadingAssetBundle loadingAssetBundle = null;
				AssetBundleManager.m_LoadingAssetBundles.TryGetValue(assetBundleName, out loadingAssetBundle);
				if (loadingAssetBundle != null)
				{
					loadingAssetBundle.m_ReferencedCount--;
				}
				return;
			}
			if (--loadedAssetBundle.m_ReferencedCount <= 0)
			{
				loadedAssetBundle.m_AssetBundle.Unload(true);
				AssetBundleManager.m_LoadedAssetBundles.Remove(assetBundleName);
				AssetBundleManager.Log(AssetBundleManager.LogType.Info, assetBundleName + " has been unloaded successfully");
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0003E7D8 File Offset: 0x0003CBD8
		public static AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, Type type)
		{
			AssetBundleManager.Log(AssetBundleManager.LogType.Info, string.Concat(new string[]
			{
				"Loading ",
				assetName,
				" from ",
				assetBundleName,
				" bundle"
			}));
			assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName);
			AssetBundleManager.LoadAssetBundle(assetBundleName, true, false);
			AssetBundleLoadAssetOperation assetBundleLoadAssetOperation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadAssetOperation);
			return assetBundleLoadAssetOperation;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0003E840 File Offset: 0x0003CC40
		public static AssetBundleLoadLevelOperationBase LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
		{
			AssetBundleManager.Log(AssetBundleManager.LogType.Info, string.Concat(new string[]
			{
				"Loading ",
				levelName,
				" from ",
				assetBundleName,
				" bundle"
			}));
			assetBundleName = assetBundleName.ToLowerInvariant();
			assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName);
			AssetBundleManager.LoadAssetBundle(assetBundleName, true, false);
			AssetBundleLoadLevelOperationBase assetBundleLoadLevelOperationBase = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadLevelOperationBase);
			AssetBundleLoadLevelOperationBase assetBundleLoadLevelOperationBase2 = assetBundleLoadLevelOperationBase;
			assetBundleLoadLevelOperationBase2.OperationComplete = (AssetBundleLoadOperation.OperationCompleteDelegate)Delegate.Combine(assetBundleLoadLevelOperationBase2.OperationComplete, new AssetBundleLoadOperation.OperationCompleteDelegate(delegate()
			{
				AssetBundleManager.UpdateLoadedSceneBundles();
			}));
			return assetBundleLoadLevelOperationBase;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003E8E0 File Offset: 0x0003CCE0
		public static void LoadLevel(string assetBundleName, string levelName, bool isAdditive)
		{
			AssetBundleManager.Log(AssetBundleManager.LogType.Info, string.Concat(new string[]
			{
				"Loading ",
				levelName,
				" from ",
				assetBundleName,
				" bundle"
			}));
			assetBundleName = assetBundleName.ToLowerInvariant();
			assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName);
			AssetBundleManager.LoadAssetBundle(assetBundleName, false, false);
			string message;
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(assetBundleName, out message);
			if (loadedAssetBundle != null)
			{
				if (isAdditive)
				{
					SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
				}
				else
				{
					SceneManager.LoadScene(levelName, LoadSceneMode.Single);
				}
			}
			else
			{
				Debug.LogError(message);
			}
			AssetBundleManager.UpdateLoadedSceneBundles();
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0003E970 File Offset: 0x0003CD70
		protected static void UpdateLoadedSceneBundles()
		{
			List<string> list = new List<string>();
			int i = 0;
			int sceneCount = SceneManager.sceneCount;
			while (i < sceneCount)
			{
				list.Add(SceneManager.GetSceneAt(i).name.ToLowerInvariant());
				i++;
			}
			List<string> list2 = new List<string>();
			Dictionary<string, LoadedAssetBundle>.KeyCollection keys = AssetBundleManager.m_LoadedAssetBundles.Keys;
			foreach (string text in keys)
			{
				if (!list.Contains(text))
				{
					LoadedAssetBundle loadedAssetBundle = null;
					AssetBundleManager.m_LoadedAssetBundles.TryGetValue(text, out loadedAssetBundle);
					if (loadedAssetBundle != null && loadedAssetBundle.m_AssetBundle != null && loadedAssetBundle.m_AssetBundle.isStreamedSceneAssetBundle)
					{
						list2.Add(text);
					}
				}
			}
			int j = 0;
			int count = list2.Count;
			while (j < count)
			{
				AssetBundleManager.UnloadAssetBundle(list2[j]);
				j++;
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0003EA74 File Offset: 0x0003CE74
		private IEnumerator LoadAssetBundleFromFileAsync(string assetBundleName)
		{
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(AssetBundleManager.GetStreamingAssetsPath() + "/" + assetBundleName);
			AssetBundleManager.m_LoadingAssetBundles.Add(assetBundleName, new LoadingAssetBundle(request));
			yield return request;
			LoadedAssetBundle assetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out assetBundle);
			if (assetBundle == null || assetBundle.m_AssetBundle == null)
			{
				if (request.isDone)
				{
					AssetBundle assetBundle2 = request.assetBundle;
					if (assetBundle2 == null)
					{
						AssetBundleManager.m_LoadingErrors.SafeAdd(assetBundleName, string.Format("{0} is not a valid asset bundle.", assetBundleName));
					}
					else
					{
						AssetBundleManager.m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(assetBundle2));
					}
				}
				else
				{
					AssetBundleManager.m_LoadingErrors.SafeAdd(assetBundleName, string.Format("Failed to complete request for bundle {0}.", assetBundleName));
				}
			}
			LoadedAssetBundle loadedBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedBundle);
			LoadingAssetBundle loadingBundle = null;
			AssetBundleManager.m_LoadingAssetBundles.TryGetValue(assetBundleName, out loadingBundle);
			if (loadedBundle != null && loadingBundle != null)
			{
				loadedBundle.m_ReferencedCount += loadingBundle.m_ReferencedCount;
				if (loadedBundle.m_ReferencedCount <= 0)
				{
					AssetBundleManager.UnloadAssetBundle(assetBundleName);
				}
			}
			AssetBundleManager.m_LoadingAssetBundles.Remove(assetBundleName);
			yield break;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0003EA90 File Offset: 0x0003CE90
		private void LoadAssetBundleFromFile(string assetBundleName)
		{
			AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetBundleManager.GetStreamingAssetsPath() + "/" + assetBundleName);
			if (assetBundle == null)
			{
				AssetBundleManager.m_LoadingErrors.SafeAdd(assetBundleName, string.Format("{0} is not a valid asset bundle.", assetBundleName));
			}
			else
			{
				AssetBundleManager.m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(assetBundle));
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003EAEC File Offset: 0x0003CEEC
		private void Update()
		{
			int i = 0;
			while (i < AssetBundleManager.m_InProgressOperations.Count)
			{
				if (!AssetBundleManager.m_InProgressOperations[i].Update())
				{
					AssetBundleManager.m_InProgressOperations.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0400091D RID: 2333
		private static AssetBundleManager.LogMode m_LogMode = AssetBundleManager.LogMode.All;

		// Token: 0x0400091E RID: 2334
		private static string m_BaseDownloadingURL = string.Empty;

		// Token: 0x0400091F RID: 2335
		private static string[] m_ActiveVariants = new string[0];

		// Token: 0x04000920 RID: 2336
		private static AssetBundleManifest m_AssetBundleManifest = null;

		// Token: 0x04000921 RID: 2337
		private static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

		// Token: 0x04000922 RID: 2338
		private static Dictionary<string, LoadingAssetBundle> m_LoadingAssetBundles = new Dictionary<string, LoadingAssetBundle>();

		// Token: 0x04000923 RID: 2339
		private static Dictionary<string, string> m_LoadingErrors = new Dictionary<string, string>();

		// Token: 0x04000924 RID: 2340
		private static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

		// Token: 0x04000925 RID: 2341
		private static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();

		// Token: 0x04000926 RID: 2342
		private static AssetBundleManager m_Instance = null;

		// Token: 0x02000291 RID: 657
		public enum LogMode
		{
			// Token: 0x04000929 RID: 2345
			All,
			// Token: 0x0400092A RID: 2346
			JustErrors
		}

		// Token: 0x02000292 RID: 658
		public enum LogType
		{
			// Token: 0x0400092C RID: 2348
			Info,
			// Token: 0x0400092D RID: 2349
			Warning,
			// Token: 0x0400092E RID: 2350
			Error
		}
	}
}
