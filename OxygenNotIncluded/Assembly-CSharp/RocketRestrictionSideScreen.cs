using System;
using UnityEngine;

// Token: 0x02000C42 RID: 3138
public class RocketRestrictionSideScreen : SideScreenContent
{
	// Token: 0x0600636A RID: 25450 RVA: 0x0024BEFB File Offset: 0x0024A0FB
	protected override void OnSpawn()
	{
		this.unrestrictedButton.onClick += this.ClickNone;
		this.spaceRestrictedButton.onClick += this.ClickSpace;
	}

	// Token: 0x0600636B RID: 25451 RVA: 0x0024BF2B File Offset: 0x0024A12B
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x0600636C RID: 25452 RVA: 0x0024BF2E File Offset: 0x0024A12E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<RocketControlStation.StatesInstance>() != null;
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x0024BF39 File Offset: 0x0024A139
	public override void SetTarget(GameObject new_target)
	{
		this.controlStation = new_target.GetComponent<RocketControlStation>();
		this.controlStationLogicSubHandle = this.controlStation.Subscribe(1861523068, new Action<object>(this.UpdateButtonStates));
		this.UpdateButtonStates(null);
	}

	// Token: 0x0600636E RID: 25454 RVA: 0x0024BF70 File Offset: 0x0024A170
	public override void ClearTarget()
	{
		if (this.controlStationLogicSubHandle != -1 && this.controlStation != null)
		{
			this.controlStation.Unsubscribe(this.controlStationLogicSubHandle);
			this.controlStationLogicSubHandle = -1;
		}
		this.controlStation = null;
	}

	// Token: 0x0600636F RID: 25455 RVA: 0x0024BFA8 File Offset: 0x0024A1A8
	private void UpdateButtonStates(object data = null)
	{
		bool flag = this.controlStation.IsLogicInputConnected();
		if (!flag)
		{
			this.unrestrictedButton.isOn = !this.controlStation.RestrictWhenGrounded;
			this.spaceRestrictedButton.isOn = this.controlStation.RestrictWhenGrounded;
		}
		this.unrestrictedButton.gameObject.SetActive(!flag);
		this.spaceRestrictedButton.gameObject.SetActive(!flag);
		this.automationControlled.gameObject.SetActive(flag);
	}

	// Token: 0x06006370 RID: 25456 RVA: 0x0024C02C File Offset: 0x0024A22C
	private void ClickNone()
	{
		this.controlStation.RestrictWhenGrounded = false;
		this.UpdateButtonStates(null);
	}

	// Token: 0x06006371 RID: 25457 RVA: 0x0024C041 File Offset: 0x0024A241
	private void ClickSpace()
	{
		this.controlStation.RestrictWhenGrounded = true;
		this.UpdateButtonStates(null);
	}

	// Token: 0x040043D9 RID: 17369
	private RocketControlStation controlStation;

	// Token: 0x040043DA RID: 17370
	[Header("Buttons")]
	public KToggle unrestrictedButton;

	// Token: 0x040043DB RID: 17371
	public KToggle spaceRestrictedButton;

	// Token: 0x040043DC RID: 17372
	public GameObject automationControlled;

	// Token: 0x040043DD RID: 17373
	private int controlStationLogicSubHandle = -1;
}
