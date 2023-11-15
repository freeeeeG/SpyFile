using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem
{
	// Token: 0x0200138D RID: 5005
	public class Range : Behaviour, IPattern
	{
		// Token: 0x060062C4 RID: 25284 RVA: 0x00099F2B File Offset: 0x0009812B
		public bool CanUse()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060062C5 RID: 25285 RVA: 0x0011F8A7 File Offset: 0x0011DAA7
		public bool CanUse(AIController controller)
		{
			return controller.FindClosestPlayerBody(this.trigger) != null;
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x0011F8BB File Offset: 0x0011DABB
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._attack.CRun(controller);
			yield break;
		}

		// Token: 0x04004FA0 RID: 20384
		[SerializeField]
		internal Collider2D trigger;

		// Token: 0x04004FA1 RID: 20385
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;
	}
}
