using System;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class GroundTile : TileBase
{
	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0001EEB4 File Offset: 0x0001D0B4
	// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0001EEBC File Offset: 0x0001D0BC
	public override bool IsLanded
	{
		get
		{
			return base.IsLanded;
		}
		set
		{
			base.IsLanded = value;
			base.gameObject.layer = (value ? LayerMask.NameToLayer(StaticData.GroundTileMask) : LayerMask.NameToLayer(StaticData.TempGroundMask));
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0001EEE9 File Offset: 0x0001D0E9
	public override void OnTileSelected(bool value)
	{
		base.OnTileSelected(value);
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowBuyGroundTips(StaticData.LeftTipsPos);
		}
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0001EF04 File Offset: 0x0001D104
	public override void OnSpawn()
	{
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0001EF06 File Offset: 0x0001D106
	public override void OnUnSpawn()
	{
	}
}
