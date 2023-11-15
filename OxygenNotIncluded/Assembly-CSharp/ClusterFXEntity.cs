using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000991 RID: 2449
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x0600484C RID: 18508 RVA: 0x00197CB2 File Offset: 0x00195EB2
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x0600484D RID: 18509 RVA: 0x00197CBE File Offset: 0x00195EBE
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.FX;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x0600484E RID: 18510 RVA: 0x00197CC4 File Offset: 0x00195EC4
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.kAnimName),
					initialAnim = this.animName,
					playMode = this.animPlayMode,
					animOffset = this.animOffset
				}
			};
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x0600484F RID: 18511 RVA: 0x00197D23 File Offset: 0x00195F23
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06004850 RID: 18512 RVA: 0x00197D26 File Offset: 0x00195F26
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x00197D29 File Offset: 0x00195F29
	public void Init(AxialI location, Vector3 animOffset)
	{
		base.Location = location;
		this.animOffset = animOffset;
	}

	// Token: 0x04002FE8 RID: 12264
	[SerializeField]
	public string kAnimName;

	// Token: 0x04002FE9 RID: 12265
	[SerializeField]
	public string animName;

	// Token: 0x04002FEA RID: 12266
	public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;

	// Token: 0x04002FEB RID: 12267
	public Vector3 animOffset;
}
