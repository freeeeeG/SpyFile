using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x020009BD RID: 2493
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06004A6C RID: 19052 RVA: 0x001A3985 File Offset: 0x001A1B85
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06004A6D RID: 19053 RVA: 0x001A3991 File Offset: 0x001A1B91
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Telescope;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06004A6E RID: 19054 RVA: 0x001A3994 File Offset: 0x001A1B94
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("telescope_target_kanim"),
					initialAnim = "idle"
				}
			};
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06004A6F RID: 19055 RVA: 0x001A39D7 File Offset: 0x001A1BD7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06004A70 RID: 19056 RVA: 0x001A39DA File Offset: 0x001A1BDA
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x001A39DD File Offset: 0x001A1BDD
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x001A39E6 File Offset: 0x001A1BE6
	public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
	{
		this.targetMeteorShower = meteorShower;
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x001A39EF File Offset: 0x001A1BEF
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x001A39F2 File Offset: 0x001A1BF2
	public override bool ShowProgressBar()
	{
		return true;
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x001A39F5 File Offset: 0x001A1BF5
	public override float GetProgress()
	{
		if (this.targetMeteorShower != null)
		{
			return this.targetMeteorShower.IdentifyingProgress;
		}
		return SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(base.Location);
	}

	// Token: 0x040030EC RID: 12524
	private ClusterMapMeteorShower.Instance targetMeteorShower;
}
