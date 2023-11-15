using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000B99 RID: 2969
public class MotdServerClient
{
	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06005C80 RID: 23680 RVA: 0x0021E568 File Offset: 0x0021C768
	private static string MotdServerUrl
	{
		get
		{
			return "https://klei-motd.s3.amazonaws.com/oni/" + MotdServerClient.GetLocalePathSuffix();
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06005C81 RID: 23681 RVA: 0x0021E579 File Offset: 0x0021C779
	private static string MotdLocalPath
	{
		get
		{
			return "motd_local/" + MotdServerClient.GetLocalePathSuffix();
		}
	}

	// Token: 0x06005C82 RID: 23682 RVA: 0x0021E58A File Offset: 0x0021C78A
	private static string MotdLocalImagePath(int imageVersion)
	{
		return MotdServerClient.MotdLocalImagePath(imageVersion, Localization.GetLocale());
	}

	// Token: 0x06005C83 RID: 23683 RVA: 0x0021E597 File Offset: 0x0021C797
	private static string FallbackMotdLocalImagePath(int imageVersion)
	{
		return MotdServerClient.MotdLocalImagePath(imageVersion, null);
	}

	// Token: 0x06005C84 RID: 23684 RVA: 0x0021E5A0 File Offset: 0x0021C7A0
	private static string MotdLocalImagePath(int imageVersion, Localization.Locale locale)
	{
		return "motd_local/" + MotdServerClient.GetLocalePathModifier(locale) + "image_" + imageVersion.ToString();
	}

	// Token: 0x06005C85 RID: 23685 RVA: 0x0021E5BE File Offset: 0x0021C7BE
	private static string GetLocalePathModifier()
	{
		return MotdServerClient.GetLocalePathModifier(Localization.GetLocale());
	}

	// Token: 0x06005C86 RID: 23686 RVA: 0x0021E5CC File Offset: 0x0021C7CC
	private static string GetLocalePathModifier(Localization.Locale locale)
	{
		string result = "";
		if (locale != null)
		{
			Localization.Language lang = locale.Lang;
			if (lang == Localization.Language.Chinese || lang - Localization.Language.Korean <= 1)
			{
				result = locale.Code + "/";
			}
		}
		return result;
	}

	// Token: 0x06005C87 RID: 23687 RVA: 0x0021E604 File Offset: 0x0021C804
	private static string GetLocalePathSuffix()
	{
		return MotdServerClient.GetLocalePathModifier() + "motd.json";
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x0021E618 File Offset: 0x0021C818
	public void GetMotd(Action<MotdServerClient.MotdResponse, string> cb)
	{
		this.m_callback = cb;
		MotdServerClient.MotdResponse localResponse = this.GetLocalMotd(MotdServerClient.MotdLocalPath);
		this.GetWebMotd(MotdServerClient.MotdServerUrl, localResponse, delegate(MotdServerClient.MotdResponse response, string err)
		{
			MotdServerClient.MotdResponse motdResponse;
			if (err == null)
			{
				global::Debug.Assert(response.image_texture != null, "Attempting to return response with no image texture");
				motdResponse = response;
			}
			else
			{
				global::Debug.LogWarning("Could not retrieve web motd from " + MotdServerClient.MotdServerUrl + ", falling back to local - err: " + err);
				motdResponse = localResponse;
			}
			if (Localization.GetSelectedLanguageType() == Localization.SelectedLanguageType.UGC)
			{
				global::Debug.Log("Language Mod detected, MOTD strings falling back to local file");
				motdResponse.image_header_text = UI.FRONTEND.MOTD.IMAGE_HEADER;
				motdResponse.news_header_text = UI.FRONTEND.MOTD.NEWS_HEADER;
				motdResponse.news_body_text = UI.FRONTEND.MOTD.NEWS_BODY;
				motdResponse.patch_notes_summary = UI.FRONTEND.MOTD.PATCH_NOTES_SUMMARY;
				motdResponse.vanilla_update_data.update_text_override = UI.FRONTEND.MOTD.UPDATE_TEXT;
				motdResponse.expansion1_update_data.update_text_override = UI.FRONTEND.MOTD.UPDATE_TEXT_EXPANSION1;
			}
			this.doCallback(motdResponse, null);
		});
	}

	// Token: 0x06005C89 RID: 23689 RVA: 0x0021E668 File Offset: 0x0021C868
	private MotdServerClient.MotdResponse GetLocalMotd(string filePath)
	{
		TextAsset textAsset = Resources.Load<TextAsset>(filePath.Replace(".json", ""));
		this.m_localMotd = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(textAsset.ToString());
		string text = MotdServerClient.MotdLocalImagePath(this.m_localMotd.image_version);
		this.m_localMotd.image_texture = Resources.Load<Texture2D>(text);
		if (this.m_localMotd.image_texture == null)
		{
			string text2 = MotdServerClient.FallbackMotdLocalImagePath(this.m_localMotd.image_version);
			if (text2 != text)
			{
				global::Debug.Log("Could not load " + text + ", falling back to " + text2);
				text = text2;
				this.m_localMotd.image_texture = Resources.Load<Texture2D>(text);
			}
		}
		global::Debug.Assert(this.m_localMotd.image_texture != null, "Failed to load " + text);
		return this.m_localMotd;
	}

	// Token: 0x06005C8A RID: 23690 RVA: 0x0021E73C File Offset: 0x0021C93C
	private void GetWebMotd(string url, MotdServerClient.MotdResponse localMotd, Action<MotdServerClient.MotdResponse, string> cb)
	{
		MotdServerClient.<>c__DisplayClass16_0 CS$<>8__locals1 = new MotdServerClient.<>c__DisplayClass16_0();
		CS$<>8__locals1.localMotd = localMotd;
		CS$<>8__locals1.cb = cb;
		Action<string, string> cb2 = delegate(string response, string err)
		{
			MotdServerClient.<>c__DisplayClass16_1 CS$<>8__locals2 = new MotdServerClient.<>c__DisplayClass16_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			DebugUtil.DevAssert(CS$<>8__locals1.localMotd.image_texture != null, "Local MOTD image_texture is no longer loaded", null);
			if (CS$<>8__locals1.localMotd.image_texture == null)
			{
				CS$<>8__locals1.cb(null, "Local image_texture has been unloaded since we requested the MOTD");
				return;
			}
			if (err != null)
			{
				CS$<>8__locals1.cb(null, err);
				return;
			}
			MotdServerClient.<>c__DisplayClass16_1 CS$<>8__locals3 = CS$<>8__locals2;
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.Error = delegate(object sender, ErrorEventArgs args)
			{
				args.ErrorContext.Handled = true;
			};
			CS$<>8__locals3.responseStruct = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(response, jsonSerializerSettings);
			if (CS$<>8__locals2.responseStruct == null)
			{
				CS$<>8__locals1.cb(null, "Invalid json from server:" + response);
				return;
			}
			if (CS$<>8__locals2.responseStruct.version <= CS$<>8__locals1.localMotd.version)
			{
				global::Debug.Log("Using local MOTD at version: " + CS$<>8__locals1.localMotd.version.ToString() + ", web version at " + CS$<>8__locals2.responseStruct.version.ToString());
				CS$<>8__locals1.cb(CS$<>8__locals1.localMotd, null);
				return;
			}
			UnityWebRequest unityWebRequest = new UnityWebRequest();
			unityWebRequest.downloadHandler = new DownloadHandlerTexture();
			SimpleNetworkCache.LoadFromCacheOrDownload("motd_image", CS$<>8__locals2.responseStruct.image_url, CS$<>8__locals2.responseStruct.image_version, unityWebRequest, delegate(UnityWebRequest wr)
			{
				string arg = null;
				if (string.IsNullOrEmpty(wr.error))
				{
					global::Debug.Log("Using web MOTD at version: " + CS$<>8__locals2.responseStruct.version.ToString() + ", local version at " + CS$<>8__locals2.CS$<>8__locals1.localMotd.version.ToString());
					CS$<>8__locals2.responseStruct.image_texture = DownloadHandlerTexture.GetContent(wr);
				}
				else
				{
					arg = "Failed to load image: " + CS$<>8__locals2.responseStruct.image_url + " SimpleNetworkCache - " + wr.error;
				}
				CS$<>8__locals2.CS$<>8__locals1.cb(CS$<>8__locals2.responseStruct, arg);
				wr.Dispose();
			});
		};
		this.getAsyncRequest(url, cb2);
	}

	// Token: 0x06005C8B RID: 23691 RVA: 0x0021E770 File Offset: 0x0021C970
	private void getAsyncRequest(string url, Action<string, string> cb)
	{
		UnityWebRequest motdRequest = UnityWebRequest.Get(url);
		motdRequest.SetRequestHeader("Content-Type", "application/json");
		motdRequest.SendWebRequest().completed += delegate(AsyncOperation operation)
		{
			cb(motdRequest.downloadHandler.text, motdRequest.error);
			motdRequest.Dispose();
		};
	}

	// Token: 0x06005C8C RID: 23692 RVA: 0x0021E7C7 File Offset: 0x0021C9C7
	public void UnregisterCallback()
	{
		this.m_callback = null;
	}

	// Token: 0x06005C8D RID: 23693 RVA: 0x0021E7D0 File Offset: 0x0021C9D0
	private void doCallback(MotdServerClient.MotdResponse response, string error)
	{
		if (this.m_callback != null)
		{
			this.m_callback(response, error);
			return;
		}
		global::Debug.Log("Motd Response receieved, but callback was unregistered");
	}

	// Token: 0x04003E3D RID: 15933
	private Action<MotdServerClient.MotdResponse, string> m_callback;

	// Token: 0x04003E3E RID: 15934
	private MotdServerClient.MotdResponse m_localMotd;

	// Token: 0x02001AC6 RID: 6854
	public class MotdUpdateData
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06009805 RID: 38917 RVA: 0x00340643 File Offset: 0x0033E843
		// (set) Token: 0x06009806 RID: 38918 RVA: 0x0034064B File Offset: 0x0033E84B
		public string last_update_time { get; set; }

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06009807 RID: 38919 RVA: 0x00340654 File Offset: 0x0033E854
		// (set) Token: 0x06009808 RID: 38920 RVA: 0x0034065C File Offset: 0x0033E85C
		public string next_update_time { get; set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06009809 RID: 38921 RVA: 0x00340665 File Offset: 0x0033E865
		// (set) Token: 0x0600980A RID: 38922 RVA: 0x0034066D File Offset: 0x0033E86D
		public string update_text_override { get; set; }
	}

	// Token: 0x02001AC7 RID: 6855
	public class MotdResponse
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x0600980C RID: 38924 RVA: 0x0034067E File Offset: 0x0033E87E
		// (set) Token: 0x0600980D RID: 38925 RVA: 0x00340686 File Offset: 0x0033E886
		public int version { get; set; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x0600980E RID: 38926 RVA: 0x0034068F File Offset: 0x0033E88F
		// (set) Token: 0x0600980F RID: 38927 RVA: 0x00340697 File Offset: 0x0033E897
		public string image_header_text { get; set; }

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06009810 RID: 38928 RVA: 0x003406A0 File Offset: 0x0033E8A0
		// (set) Token: 0x06009811 RID: 38929 RVA: 0x003406A8 File Offset: 0x0033E8A8
		public int image_version { get; set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06009812 RID: 38930 RVA: 0x003406B1 File Offset: 0x0033E8B1
		// (set) Token: 0x06009813 RID: 38931 RVA: 0x003406B9 File Offset: 0x0033E8B9
		public string image_url { get; set; }

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06009814 RID: 38932 RVA: 0x003406C2 File Offset: 0x0033E8C2
		// (set) Token: 0x06009815 RID: 38933 RVA: 0x003406CA File Offset: 0x0033E8CA
		public string image_link_url { get; set; }

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06009816 RID: 38934 RVA: 0x003406D3 File Offset: 0x0033E8D3
		// (set) Token: 0x06009817 RID: 38935 RVA: 0x003406DB File Offset: 0x0033E8DB
		public string image_rail_link_url { get; set; }

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06009818 RID: 38936 RVA: 0x003406E4 File Offset: 0x0033E8E4
		// (set) Token: 0x06009819 RID: 38937 RVA: 0x003406EC File Offset: 0x0033E8EC
		public string news_header_text { get; set; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x0600981A RID: 38938 RVA: 0x003406F5 File Offset: 0x0033E8F5
		// (set) Token: 0x0600981B RID: 38939 RVA: 0x003406FD File Offset: 0x0033E8FD
		public string news_body_text { get; set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600981C RID: 38940 RVA: 0x00340706 File Offset: 0x0033E906
		// (set) Token: 0x0600981D RID: 38941 RVA: 0x0034070E File Offset: 0x0033E90E
		public string patch_notes_summary { get; set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x0600981E RID: 38942 RVA: 0x00340717 File Offset: 0x0033E917
		// (set) Token: 0x0600981F RID: 38943 RVA: 0x0034071F File Offset: 0x0033E91F
		public string patch_notes_link_url { get; set; }

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06009820 RID: 38944 RVA: 0x00340728 File Offset: 0x0033E928
		// (set) Token: 0x06009821 RID: 38945 RVA: 0x00340730 File Offset: 0x0033E930
		public string patch_notes_rail_link_url { get; set; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06009822 RID: 38946 RVA: 0x00340739 File Offset: 0x0033E939
		// (set) Token: 0x06009823 RID: 38947 RVA: 0x00340741 File Offset: 0x0033E941
		public MotdServerClient.MotdUpdateData vanilla_update_data { get; set; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06009824 RID: 38948 RVA: 0x0034074A File Offset: 0x0033E94A
		// (set) Token: 0x06009825 RID: 38949 RVA: 0x00340752 File Offset: 0x0033E952
		public MotdServerClient.MotdUpdateData expansion1_update_data { get; set; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06009826 RID: 38950 RVA: 0x0034075B File Offset: 0x0033E95B
		// (set) Token: 0x06009827 RID: 38951 RVA: 0x00340763 File Offset: 0x0033E963
		public string latest_update_build { get; set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06009828 RID: 38952 RVA: 0x0034076C File Offset: 0x0033E96C
		// (set) Token: 0x06009829 RID: 38953 RVA: 0x00340774 File Offset: 0x0033E974
		[JsonIgnore]
		public Texture2D image_texture { get; set; }
	}
}
