using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001089 RID: 4233
	public sealed class AlchemistAI : AIController
	{
		// Token: 0x060051EC RID: 20972 RVA: 0x000F5EB8 File Offset: 0x000F40B8
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._idle,
				this._attack,
				this._chase
			};
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x000F5F0C File Offset: 0x000F410C
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x000F5F34 File Offset: 0x000F4134
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x000F5F43 File Offset: 0x000F4143
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
				{
					if (base.target != null && base.target.movement != null && base.target.movement.isGrounded)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return null;
					}
				}
				else
				{
					yield return this._chase.CRun(this);
					if (base.target != null && base.target.movement != null && base.target.movement.isGrounded)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return null;
					}
				}
			}
			yield break;
		}

		// Token: 0x040041C6 RID: 16838
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040041C7 RID: 16839
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040041C8 RID: 16840
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x040041C9 RID: 16841
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private Attack _attack;

		// Token: 0x040041CA RID: 16842
		[Chase.SubcomponentAttribute(true)]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040041CB RID: 16843
		[SerializeField]
		private Collider2D _attackTrigger;
	}
}
