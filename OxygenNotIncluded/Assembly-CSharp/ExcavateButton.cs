using System;
using STRINGS;

// Token: 0x0200023C RID: 572
public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00040A85 File Offset: 0x0003EC85
	public string SidescreenButtonText
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00040AB1 File Offset: 0x0003ECB1
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00040ADD File Offset: 0x0003ECDD
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00040AE0 File Offset: 0x0003ECE0
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00040AE7 File Offset: 0x0003ECE7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00040AEA File Offset: 0x0003ECEA
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00040AED File Offset: 0x0003ECED
	public void OnSidescreenButtonPressed()
	{
		System.Action onButtonPressed = this.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00040AFF File Offset: 0x0003ECFF
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040006BA RID: 1722
	public Func<bool> isMarkedForDig;

	// Token: 0x040006BB RID: 1723
	public System.Action OnButtonPressed;
}
