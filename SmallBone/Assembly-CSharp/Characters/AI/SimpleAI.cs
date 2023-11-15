using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001115 RID: 4373
	public sealed class SimpleAI : AIController
	{
		// Token: 0x06005517 RID: 21783 RVA: 0x000F0D27 File Offset: 0x000EEF27
		private new void OnEnable()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x000FE403 File Offset: 0x000FC603
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._behaviours.CRun(this);
			yield break;
		}

		// Token: 0x04004435 RID: 17461
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _behaviours;
	}
}
