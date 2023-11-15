using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001081 RID: 4225
	public sealed class Maid02AI : AIController
	{
		// Token: 0x060051BB RID: 20923 RVA: 0x000F5838 File Offset: 0x000F3A38
		public void Awake()
		{
			this.character.health.onTookDamage += new TookDamageDelegate(this.Health_onTookDamage);
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x000F5856 File Offset: 0x000F3A56
		private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._jumpAttack.StopPropagation();
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x000F5863 File Offset: 0x000F3A63
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x000F588B File Offset: 0x000F3A8B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x000F589A File Offset: 0x000F3A9A
		private IEnumerator CCombat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._attackCollider) != null)
					{
						yield return this._jumpAttack.CRun(this);
						yield return this._confusing.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this._jumpAttack.CRun(this);
							yield return this._confusing.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x040041A5 RID: 16805
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040041A6 RID: 16806
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x040041A7 RID: 16807
		[Subcomponent(typeof(Confusing))]
		[SerializeField]
		private Confusing _confusing;

		// Token: 0x040041A8 RID: 16808
		[SerializeField]
		[Subcomponent(typeof(ActionAttack))]
		private ActionAttack _jumpAttack;

		// Token: 0x040041A9 RID: 16809
		[SerializeField]
		private Collider2D _attackCollider;
	}
}
