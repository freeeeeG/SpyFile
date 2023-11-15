using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DED RID: 3565
	public class CommonSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x06006D90 RID: 28048 RVA: 0x002B38B0 File Offset: 0x002B1AB0
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx_loop", KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x002B3910 File Offset: 0x002B1B10
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}
	}
}
