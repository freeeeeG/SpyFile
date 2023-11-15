using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001274 RID: 4724
	public class FirstHeroAIPhase1 : AIController
	{
		// Token: 0x06005DAE RID: 23982 RVA: 0x000F0D27 File Offset: 0x000EEF27
		private new void OnEnable()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x0011386E File Offset: 0x00111A6E
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return Chronometer.global.WaitForSeconds(1f);
			yield return this._behaviours.CRun(this);
			yield break;
		}

		// Token: 0x04004B32 RID: 19250
		[SerializeField]
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		private Characters.AI.Behaviours.Behaviour _behaviours;
	}
}
