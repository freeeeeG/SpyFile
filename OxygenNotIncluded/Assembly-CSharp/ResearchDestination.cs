using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x020009AF RID: 2479
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x060049D3 RID: 18899 RVA: 0x0019FD25 File Offset: 0x0019DF25
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x060049D4 RID: 18900 RVA: 0x0019FD31 File Offset: 0x0019DF31
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x060049D5 RID: 18901 RVA: 0x0019FD34 File Offset: 0x0019DF34
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>();
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x060049D6 RID: 18902 RVA: 0x0019FD3B File Offset: 0x0019DF3B
	public override bool IsVisible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x060049D7 RID: 18903 RVA: 0x0019FD3E File Offset: 0x0019DF3E
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x0019FD41 File Offset: 0x0019DF41
	public void Init(AxialI location)
	{
		this.m_location = location;
	}
}
