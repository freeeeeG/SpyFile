using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C47 RID: 3143
public class SelfDestructButtonSideScreen : SideScreenContent
{
	// Token: 0x060063AA RID: 25514 RVA: 0x0024E2AD File Offset: 0x0024C4AD
	protected override void OnSpawn()
	{
		this.Refresh();
		this.button.onClick += this.TriggerDestruct;
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x0024E2CC File Offset: 0x0024C4CC
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x0024E2D3 File Offset: 0x0024C4D3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CraftModuleInterface>() != null && target.HasTag(GameTags.RocketInSpace);
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x0024E2F0 File Offset: 0x0024C4F0
	public override void SetTarget(GameObject target)
	{
		this.craftInterface = target.GetComponent<CraftModuleInterface>();
		this.acknowledgeWarnings = false;
		this.craftInterface.Subscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate);
		this.Refresh();
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x0024E321 File Offset: 0x0024C521
	public override void ClearTarget()
	{
		if (this.craftInterface != null)
		{
			this.craftInterface.Unsubscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate, false);
			this.craftInterface = null;
		}
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x0024E34E File Offset: 0x0024C54E
	private void OnTagsChanged(object data)
	{
		if (((TagChangedEventData)data).tag == GameTags.RocketStranded)
		{
			this.Refresh();
		}
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x0024E36D File Offset: 0x0024C56D
	private void TriggerDestruct()
	{
		if (this.acknowledgeWarnings)
		{
			this.craftInterface.gameObject.Trigger(-1061799784, null);
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.acknowledgeWarnings = true;
		}
		this.Refresh();
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x0024E3A4 File Offset: 0x0024C5A4
	private void Refresh()
	{
		if (this.craftInterface == null)
		{
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.MESSAGE_TEXT;
		if (this.acknowledgeWarnings)
		{
			this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT_CONFIRM;
			this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP_CONFIRM;
			return;
		}
		this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT;
		this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP;
	}

	// Token: 0x04004410 RID: 17424
	public KButton button;

	// Token: 0x04004411 RID: 17425
	public LocText statusText;

	// Token: 0x04004412 RID: 17426
	private CraftModuleInterface craftInterface;

	// Token: 0x04004413 RID: 17427
	private bool acknowledgeWarnings;

	// Token: 0x04004414 RID: 17428
	private static readonly EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen> TagsChangedDelegate = new EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen>(delegate(SelfDestructButtonSideScreen cmp, object data)
	{
		cmp.OnTagsChanged(data);
	});
}
