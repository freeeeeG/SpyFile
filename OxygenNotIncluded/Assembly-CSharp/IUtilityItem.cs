using System;

// Token: 0x02000A14 RID: 2580
public interface IUtilityItem
{
	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06004D3D RID: 19773
	// (set) Token: 0x06004D3E RID: 19774
	UtilityConnections Connections { get; set; }

	// Token: 0x06004D3F RID: 19775
	void UpdateConnections(UtilityConnections Connections);

	// Token: 0x06004D40 RID: 19776
	int GetNetworkID();

	// Token: 0x06004D41 RID: 19777
	UtilityNetwork GetNetworkForDirection(Direction d);
}
