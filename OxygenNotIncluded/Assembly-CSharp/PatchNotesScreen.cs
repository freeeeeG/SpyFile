using System;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
public class PatchNotesScreen : KModalScreen
{
	// Token: 0x06005158 RID: 20824 RVA: 0x001CEC74 File Offset: 0x001CCE74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		this.closeButton.onClick += this.MarkAsReadAndClose;
		this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
		this.okButton.onClick += this.MarkAsReadAndClose;
		this.previousVersion.onClick += delegate()
		{
			App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
		};
		this.fullPatchNotes.onClick += this.OnPatchNotesClick;
		PatchNotesScreen.instance = this;
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x001CED2C File Offset: 0x001CCF2C
	protected override void OnCleanUp()
	{
		PatchNotesScreen.instance = null;
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001CED34 File Offset: 0x001CCF34
	public static bool ShouldShowScreen()
	{
		return KPlayerPrefs.GetInt("PatchNotesVersion") < PatchNotesScreen.PatchNotesVersion;
	}

	// Token: 0x0600515B RID: 20827 RVA: 0x001CED47 File Offset: 0x001CCF47
	private void MarkAsReadAndClose()
	{
		KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
		this.Deactivate();
	}

	// Token: 0x0600515C RID: 20828 RVA: 0x001CED5E File Offset: 0x001CCF5E
	public static void UpdatePatchNotes(string patchNotesSummary, string url)
	{
		PatchNotesScreen.m_patchNotesUrl = url;
		PatchNotesScreen.m_patchNotesText = patchNotesSummary;
		if (PatchNotesScreen.instance != null)
		{
			PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		}
	}

	// Token: 0x0600515D RID: 20829 RVA: 0x001CED8D File Offset: 0x001CCF8D
	private void OnPatchNotesClick()
	{
		App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);
	}

	// Token: 0x0600515E RID: 20830 RVA: 0x001CED99 File Offset: 0x001CCF99
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.MarkAsReadAndClose();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003566 RID: 13670
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003567 RID: 13671
	[SerializeField]
	private KButton okButton;

	// Token: 0x04003568 RID: 13672
	[SerializeField]
	private KButton fullPatchNotes;

	// Token: 0x04003569 RID: 13673
	[SerializeField]
	private KButton previousVersion;

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	private LocText changesLabel;

	// Token: 0x0400356B RID: 13675
	private static string m_patchNotesUrl;

	// Token: 0x0400356C RID: 13676
	private static string m_patchNotesText;

	// Token: 0x0400356D RID: 13677
	private static int PatchNotesVersion = 9;

	// Token: 0x0400356E RID: 13678
	private static PatchNotesScreen instance;
}
