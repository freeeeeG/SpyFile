using System;
using System.Collections.Generic;

// Token: 0x02000A1C RID: 2588
public interface IUtilityNetworkMgr
{
	// Token: 0x06004D68 RID: 19816
	bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason);

	// Token: 0x06004D69 RID: 19817
	void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

	// Token: 0x06004D6A RID: 19818
	void StashVisualGrids();

	// Token: 0x06004D6B RID: 19819
	void UnstashVisualGrids();

	// Token: 0x06004D6C RID: 19820
	string GetVisualizerString(int cell);

	// Token: 0x06004D6D RID: 19821
	string GetVisualizerString(UtilityConnections connections);

	// Token: 0x06004D6E RID: 19822
	UtilityConnections GetConnections(int cell, bool is_physical_building);

	// Token: 0x06004D6F RID: 19823
	UtilityConnections GetDisplayConnections(int cell);

	// Token: 0x06004D70 RID: 19824
	void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

	// Token: 0x06004D71 RID: 19825
	void ClearCell(int cell, bool is_physical_building);

	// Token: 0x06004D72 RID: 19826
	void ForceRebuildNetworks();

	// Token: 0x06004D73 RID: 19827
	void AddToNetworks(int cell, object item, bool is_endpoint);

	// Token: 0x06004D74 RID: 19828
	void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

	// Token: 0x06004D75 RID: 19829
	object GetEndpoint(int cell);

	// Token: 0x06004D76 RID: 19830
	UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

	// Token: 0x06004D77 RID: 19831
	UtilityNetwork GetNetworkForCell(int cell);

	// Token: 0x06004D78 RID: 19832
	void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06004D79 RID: 19833
	void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06004D7A RID: 19834
	IList<UtilityNetwork> GetNetworks();
}
