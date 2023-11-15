using System;
using UnityEngine;

// Token: 0x02000C8C RID: 3212
public class VictoryScreen : KModalScreen
{
	// Token: 0x06006657 RID: 26199 RVA: 0x00262D5B File Offset: 0x00260F5B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x06006658 RID: 26200 RVA: 0x00262D69 File Offset: 0x00260F69
	private void Init()
	{
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x06006659 RID: 26201 RVA: 0x00262D8F File Offset: 0x00260F8F
	private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

	// Token: 0x0600665A RID: 26202 RVA: 0x00262D9F File Offset: 0x00260F9F
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x0600665B RID: 26203 RVA: 0x00262DA8 File Offset: 0x00260FA8
	public void SetAchievements(string[] achievementIDs)
	{
		string text = "";
		for (int i = 0; i < achievementIDs.Length; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[i]).Name);
			text = text + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[i]).description;
		}
		this.descriptionText.text = text;
	}

	// Token: 0x04004681 RID: 18049
	[SerializeField]
	private KButton DismissButton;

	// Token: 0x04004682 RID: 18050
	[SerializeField]
	private LocText descriptionText;
}
