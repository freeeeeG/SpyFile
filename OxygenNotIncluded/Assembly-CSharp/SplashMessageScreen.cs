using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6E RID: 3182
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class SplashMessageScreen : KMonoBehaviour
{
	// Token: 0x06006554 RID: 25940 RVA: 0x0025A4DC File Offset: 0x002586DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/118-oxygen-not-included/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.bodyText.text = UI.DEVELOPMENTBUILDS.ALPHA.LOADING.BODY;
	}

	// Token: 0x06006555 RID: 25941 RVA: 0x0025A545 File Offset: 0x00258745
	private void OnEnable()
	{
		this.confirmButton.GetComponent<LayoutElement>();
		this.confirmButton.GetComponentInChildren<LocText>();
	}

	// Token: 0x06006556 RID: 25942 RVA: 0x0025A55F File Offset: 0x0025875F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.IsExpansion1Active())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04004586 RID: 17798
	public KButton forumButton;

	// Token: 0x04004587 RID: 17799
	public KButton confirmButton;

	// Token: 0x04004588 RID: 17800
	public LocText bodyText;

	// Token: 0x04004589 RID: 17801
	public bool previewInEditor;
}
