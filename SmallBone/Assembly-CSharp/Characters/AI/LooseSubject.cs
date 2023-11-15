using System;
using System.Collections;
using Characters.Abilities;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010A2 RID: 4258
	public sealed class LooseSubject : AIController
	{
		// Token: 0x0600527C RID: 21116 RVA: 0x000F78BD File Offset: 0x000F5ABD
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x000F78E5 File Offset: 0x000F5AE5
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			this.character.collider.enabled = true;
			this.character.ability.Add(this._wanderSpeedAbilityComponent.ability);
			yield return this._wander.CRun(this);
			this.character.ability.Remove(this._wanderSpeedAbilityComponent.ability);
			yield return this._idle.CRun(this);
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x000F78F4 File Offset: 0x000F5AF4
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				if (base.target == null || (this._groggy != null && this._groggy.running))
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

		// Token: 0x0600527F RID: 21119 RVA: 0x000F7903 File Offset: 0x000F5B03
		private IEnumerator CAttack()
		{
			yield return this._attack.CRun(this);
			this._landing.TryStart();
			while (this._landing.running)
			{
				yield return null;
			}
			this.character.ability.Add(this._confusingSpeedAbilityComponent.ability);
			yield return this._confusing.CRun(this);
			this.character.ability.Remove(this._confusingSpeedAbilityComponent.ability);
			yield break;
		}

		// Token: 0x04004236 RID: 16950
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004237 RID: 16951
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x04004238 RID: 16952
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004239 RID: 16953
		[SerializeField]
		[Subcomponent(typeof(Confusing))]
		private Confusing _confusing;

		// Token: 0x0400423A RID: 16954
		[SerializeField]
		[Chase.SubcomponentAttribute(true)]
		private Chase _chase;

		// Token: 0x0400423B RID: 16955
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400423C RID: 16956
		[SerializeField]
		private Collider2D _attackCollider;

		// Token: 0x0400423D RID: 16957
		[SerializeField]
		private Collider2D _dontChseCollider;

		// Token: 0x0400423E RID: 16958
		[SerializeField]
		private Characters.Actions.Action _landing;

		// Token: 0x0400423F RID: 16959
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _wanderSpeedAbilityComponent;

		// Token: 0x04004240 RID: 16960
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _chaseSpeedAbilityComponent;

		// Token: 0x04004241 RID: 16961
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _confusingSpeedAbilityComponent;

		// Token: 0x04004242 RID: 16962
		[SerializeField]
		private Characters.Actions.Action _groggy;
	}
}
