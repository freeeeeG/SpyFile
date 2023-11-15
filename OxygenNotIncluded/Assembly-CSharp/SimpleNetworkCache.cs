using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000B9A RID: 2970
public static class SimpleNetworkCache
{
	// Token: 0x06005C8F RID: 23695 RVA: 0x0021E7FC File Offset: 0x0021C9FC
	public static void LoadFromCacheOrDownload(string cache_id, string url, int version, UnityWebRequest data_wr, Action<UnityWebRequest> callback)
	{
		string cache_folder = Util.CacheFolder();
		string cache_prefix = Path.Combine(cache_folder, cache_id);
		string version_filepath = cache_prefix + "_version";
		string data_filepath = cache_prefix + "_data";
		UnityWebRequest version_wr = new UnityWebRequest(new Uri(version_filepath, UriKind.Absolute), "GET", new DownloadHandlerBuffer(), null);
		Action<AsyncOperation> <>9__1;
		Action<AsyncOperation> <>9__2;
		version_wr.SendWebRequest().completed += delegate(AsyncOperation op)
		{
			if (SimpleNetworkCache.GetVersionFromWebRequest(version_wr) == version)
			{
				data_wr.uri = new Uri(data_filepath, UriKind.Absolute);
				AsyncOperation asyncOperation = data_wr.SendWebRequest();
				Action<AsyncOperation> value;
				if ((value = <>9__1) == null)
				{
					value = (<>9__1 = delegate(AsyncOperation fileOp)
					{
						if (!string.IsNullOrEmpty(data_wr.error))
						{
							global::Debug.LogWarning("Failure to read cached file: " + data_filepath);
							try
							{
								File.Delete(version_filepath);
								File.Delete(data_filepath);
							}
							catch
							{
								global::Debug.LogWarning("Failed to delete cached files");
							}
						}
						callback(data_wr);
					});
				}
				asyncOperation.completed += value;
			}
			else
			{
				data_wr.url = url;
				AsyncOperation asyncOperation2 = data_wr.SendWebRequest();
				Action<AsyncOperation> value2;
				if ((value2 = <>9__2) == null)
				{
					value2 = (<>9__2 = delegate(AsyncOperation webOp)
					{
						if (string.IsNullOrEmpty(data_wr.error))
						{
							try
							{
								Directory.CreateDirectory(cache_folder);
								File.WriteAllBytes(data_filepath, data_wr.downloadHandler.data);
								File.WriteAllText(version_filepath, version.ToString());
							}
							catch
							{
								global::Debug.LogWarning("Failed to write cache files to: " + cache_prefix);
							}
						}
						callback(data_wr);
					});
				}
				asyncOperation2.completed += value2;
			}
			version_wr.Dispose();
		};
	}

	// Token: 0x06005C90 RID: 23696 RVA: 0x0021E8B4 File Offset: 0x0021CAB4
	private static int GetVersionFromWebRequest(UnityWebRequest version_wr)
	{
		if (!string.IsNullOrEmpty(version_wr.error))
		{
			return -1;
		}
		int result;
		if (version_wr.downloadHandler != null && int.TryParse(version_wr.downloadHandler.text, out result))
		{
			return result;
		}
		return -1;
	}
}
