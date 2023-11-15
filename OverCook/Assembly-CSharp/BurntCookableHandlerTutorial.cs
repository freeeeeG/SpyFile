using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class BurntCookableHandlerTutorial : ButtonHoverIcon, ICarryNotified
{
	// Token: 0x06001D0F RID: 7439 RVA: 0x0008F35D File Offset: 0x0008D75D
	protected override void Awake()
	{
		base.Awake();
		ClientCookingHandler.OnBurntCookableAdded += this.OnBurntCookableAdded;
		ClientCookingHandler.OnBurntCookableRemoved += this.OnBurntCookableRemoved;
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x0008F387 File Offset: 0x0008D787
	public void OnCarryBegun(ICarrier _carrier)
	{
		this.m_beingCarried = true;
		this.UpdateVisiblity();
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x0008F396 File Offset: 0x0008D796
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.m_beingCarried = false;
		this.UpdateVisiblity();
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x0008F3A5 File Offset: 0x0008D7A5
	private void OnBurntCookableAdded(ClientCookingHandler _cookable)
	{
		if (MaskUtils.HasFlag<CookingStationType>(this.m_typeMask, _cookable.GetRequiredStationType()))
		{
			this.m_burntCookables++;
			this.UpdateVisiblity();
		}
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x0008F3D1 File Offset: 0x0008D7D1
	private void OnBurntCookableRemoved(ClientCookingHandler _cookable)
	{
		if (MaskUtils.HasFlag<CookingStationType>(this.m_typeMask, _cookable.GetRequiredStationType()))
		{
			this.m_burntCookables--;
			this.UpdateVisiblity();
		}
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x0008F3FD File Offset: 0x0008D7FD
	private void UpdateVisiblity()
	{
		base.SetVisibility(this.m_burntCookables > 0 && !this.m_beingCarried);
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0008F41D File Offset: 0x0008D81D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ClientCookingHandler.OnBurntCookableAdded -= this.OnBurntCookableAdded;
		ClientCookingHandler.OnBurntCookableRemoved -= this.OnBurntCookableRemoved;
	}

	// Token: 0x040016A0 RID: 5792
	[SerializeField]
	[Mask(typeof(CookingStationType))]
	private int m_typeMask;

	// Token: 0x040016A1 RID: 5793
	private int m_burntCookables;

	// Token: 0x040016A2 RID: 5794
	private bool m_beingCarried;
}
