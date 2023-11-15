using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200109F RID: 4255
	public sealed class HighAlchemistAI : AIController
	{
		// Token: 0x0600526B RID: 21099 RVA: 0x000F75A8 File Offset: 0x000F57A8
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

		// Token: 0x0600526C RID: 21100 RVA: 0x000F7608 File Offset: 0x000F5808
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x000F7630 File Offset: 0x000F5830
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x000F763F File Offset: 0x000F583F
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						if (this._flaskAttack.CanUse())
						{
							if (!this.character.movement.controller.isGrounded)
							{
								continue;
							}
							yield return this._flaskAttack.CRun(this);
						}
						else
						{
							yield return this._summonAttack.CRun(this);
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

		// Token: 0x04004228 RID: 16936
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004229 RID: 16937
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x0400422A RID: 16938
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x0400422B RID: 16939
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x0400422C RID: 16940
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private ActionAttack _flaskAttack;

		// Token: 0x0400422D RID: 16941
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _summonAttack;

		// Token: 0x0400422E RID: 16942
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wanderAfterAttack;

		// Token: 0x0400422F RID: 16943
		[SerializeField]
		private Collider2D _attackTrigger;
	}
}
