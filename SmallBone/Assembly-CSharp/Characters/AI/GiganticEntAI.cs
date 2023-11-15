using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001056 RID: 4182
	public sealed class GiganticEntAI : AIController
	{
		// Token: 0x060050B8 RID: 20664 RVA: 0x000F3128 File Offset: 0x000F1328
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x000F3150 File Offset: 0x000F1350
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x000F315F File Offset: 0x000F135F
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else
				{
					if (this._meleeAttack.CanUse() && base.FindClosestPlayerBody(this._meleeAttackCollider) != null)
					{
						yield return this._meleeAttack.CRun(this);
					}
					yield return this._rangeAttack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x040040E4 RID: 16612
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040040E5 RID: 16613
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _rangeAttack;

		// Token: 0x040040E6 RID: 16614
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _meleeAttack;

		// Token: 0x040040E7 RID: 16615
		[SerializeField]
		private Collider2D _meleeAttackCollider;
	}
}
