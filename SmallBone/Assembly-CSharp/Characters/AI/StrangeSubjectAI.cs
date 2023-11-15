using System;
using System.Collections;
using Characters.Abilities;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010AC RID: 4268
	public sealed class StrangeSubjectAI : AIController
	{
		// Token: 0x060052B5 RID: 21173 RVA: 0x000F810A File Offset: 0x000F630A
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x000F8132 File Offset: 0x000F6332
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			this.character.ability.Add(this._wanderSpeedAbilityComponent.ability);
			yield return this._wander.CRun(this);
			this.character.ability.Remove(this._wanderSpeedAbilityComponent.ability);
			yield return this._idle.CRun(this);
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x000F8141 File Offset: 0x000F6341
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else if (base.FindClosestPlayerBody(this._attackCollider))
				{
					yield return this.CAttack();
				}
				else
				{
					this.character.ability.Add(this._chaseSpeedAbilityComponent.ability);
					yield return this._chase.CRun(this);
					this.character.ability.Remove(this._chaseSpeedAbilityComponent.ability);
					if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
					{
						yield return this.CAttack();
					}
				}
			}
			yield break;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x000F8150 File Offset: 0x000F6350
		private IEnumerator CAttack()
		{
			yield return this._attack.CRun(this);
			this.character.ability.Add(this._confusingSpeedAbilityComponent.ability);
			yield return this._confusing.CRun(this);
			this.character.ability.Remove(this._confusingSpeedAbilityComponent.ability);
			yield break;
		}

		// Token: 0x04004265 RID: 16997
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004266 RID: 16998
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x04004267 RID: 16999
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004268 RID: 17000
		[SerializeField]
		[Subcomponent(typeof(Confusing))]
		private Confusing _confusing;

		// Token: 0x04004269 RID: 17001
		[Chase.SubcomponentAttribute(true)]
		[SerializeField]
		private Chase _chase;

		// Token: 0x0400426A RID: 17002
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400426B RID: 17003
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x0400426C RID: 17004
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _wanderSpeedAbilityComponent;

		// Token: 0x0400426D RID: 17005
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _chaseSpeedAbilityComponent;

		// Token: 0x0400426E RID: 17006
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _confusingSpeedAbilityComponent;
	}
}
