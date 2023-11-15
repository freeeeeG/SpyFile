using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Token: 0x02000BAF RID: 2991
[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
	// Token: 0x06005D4E RID: 23886 RVA: 0x0022213C File Offset: 0x0022033C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		for (int i = 0; i < this.buttonData.Count; i++)
		{
			OpenURLButtons.URLButtonData data = this.buttonData[i];
			GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, base.gameObject, true);
			string text = Strings.Get(data.stringKey);
			gameObject.GetComponentInChildren<LocText>().SetText(text);
			switch (data.urlType)
			{
			case OpenURLButtons.URLButtonType.url:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.platformUrl:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPlatformURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.patchNotes:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPatchNotes();
				};
				break;
			case OpenURLButtons.URLButtonType.feedbackScreen:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenFeedbackScreen();
				};
				break;
			}
		}
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x00222247 File Offset: 0x00220447
	public void OpenPatchNotes()
	{
		Util.KInstantiateUI(this.patchNotesScreenPrefab, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x00222260 File Offset: 0x00220460
	public void OpenFeedbackScreen()
	{
		Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x0022227E File Offset: 0x0022047E
	public void OpenURL(string URL)
	{
		App.OpenWebURL(URL);
	}

	// Token: 0x06005D52 RID: 23890 RVA: 0x00222288 File Offset: 0x00220488
	public void OpenPlatformURL(string URL)
	{
		if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
		{
			DistributionPlatform.Inst.GetAuthTicket(delegate(byte[] ticket)
			{
				string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (byte x) => x.ToString("X2")));
				App.OpenWebURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
			});
			return;
		}
		string value = URL.Replace("{SteamID}", "").Replace("{SteamTicket}", "");
		App.OpenWebURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(value)));
	}

	// Token: 0x04003ECB RID: 16075
	public GameObject buttonPrefab;

	// Token: 0x04003ECC RID: 16076
	public List<OpenURLButtons.URLButtonData> buttonData;

	// Token: 0x04003ECD RID: 16077
	[SerializeField]
	private GameObject patchNotesScreenPrefab;

	// Token: 0x04003ECE RID: 16078
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x02001AD8 RID: 6872
	public enum URLButtonType
	{
		// Token: 0x04007ABA RID: 31418
		url,
		// Token: 0x04007ABB RID: 31419
		platformUrl,
		// Token: 0x04007ABC RID: 31420
		patchNotes,
		// Token: 0x04007ABD RID: 31421
		feedbackScreen
	}

	// Token: 0x02001AD9 RID: 6873
	[Serializable]
	public class URLButtonData
	{
		// Token: 0x04007ABE RID: 31422
		public string stringKey;

		// Token: 0x04007ABF RID: 31423
		public OpenURLButtons.URLButtonType urlType;

		// Token: 0x04007AC0 RID: 31424
		public string url;
	}
}
