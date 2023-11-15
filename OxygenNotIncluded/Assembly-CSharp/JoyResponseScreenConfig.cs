using System;
using Database;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
public readonly struct JoyResponseScreenConfig
{
	// Token: 0x060057E4 RID: 22500 RVA: 0x002038CB File Offset: 0x00201ACB
	private JoyResponseScreenConfig(JoyResponseOutfitTarget target, Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
	{
		this.target = target;
		this.initalSelectedItem = initalSelectedItem;
		this.isValid = true;
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x002038E2 File Offset: 0x00201AE2
	public JoyResponseScreenConfig WithInitialSelection(Option<BalloonArtistFacadeResource> initialSelectedItem)
	{
		return new JoyResponseScreenConfig(this.target, JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x002038FA File Offset: 0x00201AFA
	public static JoyResponseScreenConfig Minion(GameObject minionInstance)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), Option.None);
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x00203911 File Offset: 0x00201B11
	public static JoyResponseScreenConfig Personality(Personality personality)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), Option.None);
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x00203928 File Offset: 0x00201B28
	public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return JoyResponseScreenConfig.Personality(personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return JoyResponseScreenConfig.Minion(minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x00203966 File Offset: 0x00201B66
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen, null);
	}

	// Token: 0x04003B6F RID: 15215
	public readonly JoyResponseOutfitTarget target;

	// Token: 0x04003B70 RID: 15216
	public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;

	// Token: 0x04003B71 RID: 15217
	public readonly bool isValid;
}
