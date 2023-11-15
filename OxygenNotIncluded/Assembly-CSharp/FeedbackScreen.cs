using System;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AFF RID: 2815
public class FeedbackScreen : KModalScreen
{
	// Token: 0x060056DC RID: 22236 RVA: 0x001FC070 File Offset: 0x001FA270
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.bugForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		};
		this.suggestionForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/");
		};
		this.logsDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(Util.LogsFolder());
		};
		this.saveFilesDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		};
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			this.saveFilesDirectoryButton.gameObject.SetActive(false);
			this.logsDirectoryButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x04003A8B RID: 14987
	public LocText title;

	// Token: 0x04003A8C RID: 14988
	public KButton dismissButton;

	// Token: 0x04003A8D RID: 14989
	public KButton closeButton;

	// Token: 0x04003A8E RID: 14990
	public KButton bugForumsButton;

	// Token: 0x04003A8F RID: 14991
	public KButton suggestionForumsButton;

	// Token: 0x04003A90 RID: 14992
	public KButton logsDirectoryButton;

	// Token: 0x04003A91 RID: 14993
	public KButton saveFilesDirectoryButton;
}
