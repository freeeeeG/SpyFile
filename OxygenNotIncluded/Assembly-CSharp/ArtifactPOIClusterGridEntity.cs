using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000988 RID: 2440
[SerializationConfig(MemberSerialization.OptIn)]
public class ArtifactPOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x060047FF RID: 18431 RVA: 0x0019665D File Offset: 0x0019485D
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06004800 RID: 18432 RVA: 0x00196665 File Offset: 0x00194865
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06004801 RID: 18433 RVA: 0x00196668 File Offset: 0x00194868
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("gravitas_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "station_1" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06004802 RID: 18434 RVA: 0x001966C0 File Offset: 0x001948C0
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06004803 RID: 18435 RVA: 0x001966C3 File Offset: 0x001948C3
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x001966C6 File Offset: 0x001948C6
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x04002FBC RID: 12220
	public string m_name;

	// Token: 0x04002FBD RID: 12221
	public string m_Anim;
}
