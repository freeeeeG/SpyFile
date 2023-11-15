using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010E3 RID: 4323
	public sealed class WisdomAI : AIController
	{
		// Token: 0x060053FE RID: 21502 RVA: 0x000FBA6E File Offset: 0x000F9C6E
		private void Awake()
		{
			this.character.status.unstoppable.Attach(this);
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._attack
			};
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x000FBAA9 File Offset: 0x000F9CA9
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x000FBAD1 File Offset: 0x000F9CD1
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x000FBAE0 File Offset: 0x000F9CE0
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null) && this._attack.CanUse() && base.FindClosestPlayerBody(this._attackTrigger) != null)
				{
					yield return this._attack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x04004377 RID: 17271
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004378 RID: 17272
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004379 RID: 17273
		[SerializeField]
		[Space]
		[Header("Tools")]
		private Collider2D _attackTrigger;
	}
}
