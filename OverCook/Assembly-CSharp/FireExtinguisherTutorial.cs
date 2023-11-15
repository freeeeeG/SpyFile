using System;

// Token: 0x02000604 RID: 1540
public class FireExtinguisherTutorial : ButtonHoverIcon, ICarryNotified
{
	// Token: 0x06001D3C RID: 7484 RVA: 0x0008F9FA File Offset: 0x0008DDFA
	protected override void Awake()
	{
		base.Awake();
		ClientFlammable.OnObjectsOnFireChanged += this.OnObjectsOnFireChanged;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0008FA13 File Offset: 0x0008DE13
	protected override void OnDestroy()
	{
		ClientFlammable.OnObjectsOnFireChanged -= this.OnObjectsOnFireChanged;
		base.OnDestroy();
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0008FA2C File Offset: 0x0008DE2C
	public void OnCarryBegun(ICarrier _carrier)
	{
		this.m_beingCarried = true;
		this.UpdateVisiblity();
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0008FA3B File Offset: 0x0008DE3B
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.m_beingCarried = false;
		this.UpdateVisiblity();
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0008FA4A File Offset: 0x0008DE4A
	private void OnObjectsOnFireChanged(int _count)
	{
		this.m_hasFire = (_count > 0);
		this.UpdateVisiblity();
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x0008FA5C File Offset: 0x0008DE5C
	private void UpdateVisiblity()
	{
		base.SetVisibility(this.m_hasFire && !this.m_beingCarried && base.gameObject.activeInHierarchy);
	}

	// Token: 0x040016B4 RID: 5812
	private bool m_hasFire;

	// Token: 0x040016B5 RID: 5813
	private bool m_beingCarried;
}
