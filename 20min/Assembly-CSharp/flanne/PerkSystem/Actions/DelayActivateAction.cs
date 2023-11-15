using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AD RID: 429
	public class DelayActivateAction : Action
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x0002779C File Offset: 0x0002599C
		public override void Init()
		{
			this.action.Init();
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000277A9 File Offset: 0x000259A9
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.StartCoroutine(this.DelayActivateCR(target));
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x000277BD File Offset: 0x000259BD
		private IEnumerator DelayActivateCR(GameObject target)
		{
			yield return new WaitForSeconds(this.delayTime);
			this.action.Activate(target);
			yield break;
		}

		// Token: 0x04000710 RID: 1808
		[SerializeField]
		private float delayTime;

		// Token: 0x04000711 RID: 1809
		[SerializeReference]
		private Action action;
	}
}
