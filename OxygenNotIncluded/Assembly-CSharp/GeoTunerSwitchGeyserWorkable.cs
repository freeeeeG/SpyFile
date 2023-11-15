using System;

// Token: 0x0200060C RID: 1548
public class GeoTunerSwitchGeyserWorkable : Workable
{
	// Token: 0x060026FB RID: 9979 RVA: 0x000D3C88 File Offset: 0x000D1E88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000D3CBC File Offset: 0x000D1EBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x0400165B RID: 5723
	private const string animName = "anim_use_remote_kanim";
}
