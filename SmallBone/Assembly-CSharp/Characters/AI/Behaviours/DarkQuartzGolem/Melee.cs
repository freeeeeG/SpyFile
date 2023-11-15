using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem
{
	// Token: 0x0200138B RID: 5003
	public class Melee : Behaviour, IPattern
	{
		// Token: 0x060062BA RID: 25274 RVA: 0x00099F2B File Offset: 0x0009812B
		public bool CanUse()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x0011F811 File Offset: 0x0011DA11
		public bool CanUse(AIController controller)
		{
			return controller.FindClosestPlayerBody(this.trigger) != null;
		}

		// Token: 0x060062BC RID: 25276 RVA: 0x0011F825 File Offset: 0x0011DA25
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._attack.CRun(controller);
			yield break;
		}

		// Token: 0x04004F9A RID: 20378
		[SerializeField]
		internal Collider2D trigger;

		// Token: 0x04004F9B RID: 20379
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;
	}
}
