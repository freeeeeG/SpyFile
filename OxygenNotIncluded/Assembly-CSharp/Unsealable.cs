using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000539 RID: 1337
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
	// Token: 0x06001FFB RID: 8187 RVA: 0x000AB70C File Offset: 0x000A990C
	private Unsealable()
	{
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000AB714 File Offset: 0x000A9914
	public override CellOffset[] GetOffsets(int cell)
	{
		if (this.facingRight)
		{
			return OffsetGroups.RightOnly;
		}
		return OffsetGroups.LeftOnly;
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000AB729 File Offset: 0x000A9929
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_door_poi_kanim")
		};
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000AB758 File Offset: 0x000A9958
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
		if (this.unsealed)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				component.allowDeconstruction = true;
			}
		}
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000AB795 File Offset: 0x000A9995
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000AB7A0 File Offset: 0x000A99A0
	protected override void OnCompleteWork(Worker worker)
	{
		this.unsealed = true;
		base.OnCompleteWork(worker);
		Deconstructable component = base.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.allowDeconstruction = true;
			Game.Instance.Trigger(1980521255, base.gameObject);
		}
	}

	// Token: 0x040011D8 RID: 4568
	[Serialize]
	public bool facingRight;

	// Token: 0x040011D9 RID: 4569
	[Serialize]
	public bool unsealed;
}
