using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CC RID: 460
	public class RepeatAction : Action
	{
		// Token: 0x06000A45 RID: 2629 RVA: 0x00028061 File Offset: 0x00026261
		public override void Init()
		{
			this.action.Init();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002806E File Offset: 0x0002626E
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.StartCoroutine(this.RepeatActivateCR(target));
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00028082 File Offset: 0x00026282
		private IEnumerator RepeatActivateCR(GameObject target)
		{
			int num;
			for (int i = 0; i < this.numOfActivations; i = num + 1)
			{
				this.action.Activate(target);
				yield return new WaitForSeconds(this.delayBetweenActivations);
				num = i;
			}
			yield break;
		}

		// Token: 0x04000744 RID: 1860
		[SerializeField]
		private int numOfActivations;

		// Token: 0x04000745 RID: 1861
		[SerializeField]
		private float delayBetweenActivations;

		// Token: 0x04000746 RID: 1862
		[SerializeReference]
		private Action action;
	}
}
