using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DEE RID: 3566
	public class CustomSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x06006D93 RID: 28051 RVA: 0x002B392A File Offset: 0x002B1B2A
		public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
		{
			this.kanim = effect_kanim;
			this.animName = effect_anim_name;
		}

		// Token: 0x06006D94 RID: 28052 RVA: 0x002B3940 File Offset: 0x002B1B40
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(this.animName, KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x06006D95 RID: 28053 RVA: 0x002B39A2 File Offset: 0x002B1BA2
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}

		// Token: 0x04005226 RID: 21030
		private string kanim;

		// Token: 0x04005227 RID: 21031
		private string animName;
	}
}
