using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000BAE RID: 2990
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
	// Token: 0x06005D4A RID: 23882 RVA: 0x00222030 File Offset: 0x00220230
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x06005D4B RID: 23883 RVA: 0x002220B0 File Offset: 0x002202B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float)Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04003EC5 RID: 16069
	public KButton forumButton;

	// Token: 0x04003EC6 RID: 16070
	public KButton confirmButton;

	// Token: 0x04003EC7 RID: 16071
	public KButton quitButton;

	// Token: 0x04003EC8 RID: 16072
	public LocText bodyText;

	// Token: 0x04003EC9 RID: 16073
	public bool previewInEditor;

	// Token: 0x04003ECA RID: 16074
	public RectTransform messageContainer;
}
