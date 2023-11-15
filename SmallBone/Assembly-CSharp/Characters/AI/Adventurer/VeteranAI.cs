using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Adventurer
{
	// Token: 0x020013EC RID: 5100
	public sealed class VeteranAI : AIController
	{
		// Token: 0x06006469 RID: 25705 RVA: 0x000F0D27 File Offset: 0x000EEF27
		public void StartCombat()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x00123793 File Offset: 0x00121993
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._behaviours.CRun(this);
			yield break;
		}

		// Token: 0x04005102 RID: 20738
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _behaviours;
	}
}
