using System;

// Token: 0x02000A0B RID: 2571
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
	// Token: 0x06004CED RID: 19693 RVA: 0x001AF4EC File Offset: 0x001AD6EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004CEE RID: 19694 RVA: 0x001AF4F4 File Offset: 0x001AD6F4
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
	}

	// Token: 0x06004CEF RID: 19695 RVA: 0x001AF507 File Offset: 0x001AD707
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
	}

	// Token: 0x06004CF0 RID: 19696 RVA: 0x001AF51A File Offset: 0x001AD71A
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}
}
