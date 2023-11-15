using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200109C RID: 4252
	public sealed class GrandAlchemistAI : AIController
	{
		// Token: 0x0600525A RID: 21082 RVA: 0x000F72B8 File Offset: 0x000F54B8
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._idle,
				this._chase,
				this._flaskAttack,
				this._summonAttack
			};
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x000F7318 File Offset: 0x000F5518
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x000F7340 File Offset: 0x000F5540
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x000F734F File Offset: 0x000F554F
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						if (this._summonAttack.CanUse())
						{
							yield return this._summonAttack.CRun(this);
						}
						else
						{
							if (!this.character.movement.controller.isGrounded)
							{
								continue;
							}
							yield return this._flaskAttack.CRun(this);
						}
						if (base.FindClosestPlayerBody(this._attackTrigger) != null)
						{
							yield return this._wanderAfterAttack.CRun(this);
						}
						else
						{
							yield return this._idle.CRun(this);
						}
					}
					else
					{
						yield return this._chase.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400421A RID: 16922
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400421B RID: 16923
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x0400421C RID: 16924
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400421D RID: 16925
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x0400421E RID: 16926
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _flaskAttack;

		// Token: 0x0400421F RID: 16927
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _summonAttack;

		// Token: 0x04004220 RID: 16928
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wanderAfterAttack;

		// Token: 0x04004221 RID: 16929
		[SerializeField]
		private Collider2D _attackTrigger;
	}
}
