using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x020009E5 RID: 2533
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
	// Token: 0x06004BA3 RID: 19363 RVA: 0x001A877E File Offset: 0x001A697E
	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x001A87AE File Offset: 0x001A69AE
	public WorldContainer GetWorld()
	{
		if (this.world == null)
		{
			this.world = ClusterManager.Instance.GetWorld(this.worldId);
		}
		return this.world;
	}

	// Token: 0x04003162 RID: 12642
	[NonSerialized]
	private WorldContainer world;

	// Token: 0x04003163 RID: 12643
	[Serialize]
	public int worldId;

	// Token: 0x04003164 RID: 12644
	[Serialize]
	public float networkQuality01;

	// Token: 0x04003165 RID: 12645
	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	// Token: 0x04003166 RID: 12646
	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	// Token: 0x04003167 RID: 12647
	[NonSerialized]
	public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

	// Token: 0x02001866 RID: 6246
	public class Scratchpad
	{
		// Token: 0x040071DF RID: 29151
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		// Token: 0x040071E0 RID: 29152
		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		// Token: 0x040071E1 RID: 29153
		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		// Token: 0x040071E2 RID: 29154
		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}
}
