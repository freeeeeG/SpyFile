using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200099D RID: 2461
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06004948 RID: 18760 RVA: 0x0019D1EB File Offset: 0x0019B3EB
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06004949 RID: 18761 RVA: 0x0019D1F3 File Offset: 0x0019B3F3
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x0600494A RID: 18762 RVA: 0x0019D1F8 File Offset: 0x0019B3F8
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x0600494B RID: 18763 RVA: 0x0019D250 File Offset: 0x0019B450
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x0600494C RID: 18764 RVA: 0x0019D253 File Offset: 0x0019B453
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x0019D256 File Offset: 0x0019B456
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x04003034 RID: 12340
	public string m_name;

	// Token: 0x04003035 RID: 12341
	public string m_Anim;
}
