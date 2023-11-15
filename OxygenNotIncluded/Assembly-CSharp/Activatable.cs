using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200047A RID: 1146
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06001917 RID: 6423 RVA: 0x00083BFF File Offset: 0x00081DFF
	public bool IsActivated
	{
		get
		{
			return this.activated;
		}
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x00083C07 File Offset: 0x00081E07
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x00083C0F File Offset: 0x00081E0F
	protected override void OnSpawn()
	{
		this.UpdateFlag();
		if (this.awaitingActivation && this.activateChore == null)
		{
			this.CreateChore();
		}
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x00083C2D File Offset: 0x00081E2D
	protected override void OnCompleteWork(Worker worker)
	{
		this.activated = true;
		if (this.onActivate != null)
		{
			this.onActivate();
		}
		this.awaitingActivation = false;
		this.UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x00083C68 File Offset: 0x00081E68
	private void UpdateFlag()
	{
		base.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated, null);
		base.Trigger(-1909216579, this.IsActivated);
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x00083CD8 File Offset: 0x00081ED8
	private void CreateChore()
	{
		if (this.activateChore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		this.activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.shouldShowSkillPerkStatusItem = true;
			this.requireMinionToWork = true;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x00083D47 File Offset: 0x00081F47
	private void CancelChore()
	{
		if (this.activateChore == null)
		{
			return;
		}
		this.activateChore.Cancel("User cancelled");
		this.activateChore = null;
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00083D69 File Offset: 0x00081F69
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600191F RID: 6431 RVA: 0x00083D6C File Offset: 0x00081F6C
	public string SidescreenButtonText
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06001920 RID: 6432 RVA: 0x00083DCC File Offset: 0x00081FCC
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x00083E2A File Offset: 0x0008202A
	public bool SidescreenEnabled()
	{
		return !this.activated;
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x00083E35 File Offset: 0x00082035
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		this.textOverride = text;
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x00083E3E File Offset: 0x0008203E
	public void OnSidescreenButtonPressed()
	{
		if (this.activateChore == null)
		{
			this.CreateChore();
		}
		else
		{
			this.CancelChore();
		}
		this.awaitingActivation = (this.activateChore != null);
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x00083E65 File Offset: 0x00082065
	public bool SidescreenButtonInteractable()
	{
		return !this.activated;
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x00083E70 File Offset: 0x00082070
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000DD7 RID: 3543
	public bool Required = true;

	// Token: 0x04000DD8 RID: 3544
	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	// Token: 0x04000DD9 RID: 3545
	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	// Token: 0x04000DDA RID: 3546
	[Serialize]
	private bool activated;

	// Token: 0x04000DDB RID: 3547
	[Serialize]
	private bool awaitingActivation;

	// Token: 0x04000DDC RID: 3548
	private Guid statusItem;

	// Token: 0x04000DDD RID: 3549
	private Chore activateChore;

	// Token: 0x04000DDE RID: 3550
	public System.Action onActivate;

	// Token: 0x04000DDF RID: 3551
	[SerializeField]
	private ButtonMenuTextOverride textOverride;
}
