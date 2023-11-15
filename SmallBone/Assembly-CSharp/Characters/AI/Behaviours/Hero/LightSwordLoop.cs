using System;
using System.Collections;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A0 RID: 5024
	public sealed class LightSwordLoop : Decorator
	{
		// Token: 0x0600631D RID: 25373 RVA: 0x0012084A File Offset: 0x0011EA4A
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			while (this._helper.GetActivatedSwordCount() > 0)
			{
				yield return this._behaviour.CRun(controller);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004FEF RID: 20463
		[SerializeField]
		private LightSwordFieldHelper _helper;

		// Token: 0x04004FF0 RID: 20464
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _behaviour;
	}
}
