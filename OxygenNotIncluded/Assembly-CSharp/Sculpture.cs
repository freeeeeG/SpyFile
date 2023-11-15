using System;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class Sculpture : Artable
{
	// Token: 0x06001E26 RID: 7718 RVA: 0x000A1048 File Offset: 0x0009F248
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (Sculpture.sculptureOverrides == null)
		{
			Sculpture.sculptureOverrides = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};
		}
		this.overrideAnims = Sculpture.sculptureOverrides;
		this.synchronizeAnims = false;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000A1088 File Offset: 0x0009F288
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		if (!skip_effect && base.CurrentStage != "Default")
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("sculpture_fx_kanim", base.transform.GetPosition(), base.transform, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.destroyOnAnimComplete = true;
			kbatchedAnimController.transform.SetLocalPosition(Vector3.zero);
			kbatchedAnimController.Play("poof", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040010E4 RID: 4324
	private static KAnimFile[] sculptureOverrides;
}
