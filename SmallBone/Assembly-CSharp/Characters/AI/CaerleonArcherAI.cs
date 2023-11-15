using System;
using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001042 RID: 4162
	public sealed class CaerleonArcherAI : AIController
	{
		// Token: 0x0600504C RID: 20556 RVA: 0x000F1FB5 File Offset: 0x000F01B5
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x000F1FDD File Offset: 0x000F01DD
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this._wander.CRun(this);
			yield return this._idle.CRun(this);
			yield return this.Combat();
			yield break;
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x000F1FEC File Offset: 0x000F01EC
		private IEnumerator Combat()
		{
			while (!base.dead)
			{
				yield return null;
				if (!(base.target == null))
				{
					if (base.FindClosestPlayerBody(this._minimumCollider) != null)
					{
						yield return this._keepDistance.CRun(this);
					}
					if (base.FindClosestPlayerBody(this._attackCollider) != null)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._attack.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this._attack.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x04004090 RID: 16528
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004091 RID: 16529
		[SerializeField]
		[Wander.SubcomponentAttribute(true)]
		private Wander _wander;

		// Token: 0x04004092 RID: 16530
		[SerializeField]
		[Subcomponent(typeof(KeepDistance))]
		private KeepDistance _keepDistance;

		// Token: 0x04004093 RID: 16531
		[SerializeField]
		[Subcomponent(typeof(HorizontalProjectileAttack))]
		private HorizontalProjectileAttack _attack;

		// Token: 0x04004094 RID: 16532
		[Chase.SubcomponentAttribute(true)]
		[SerializeField]
		private Chase _chase;

		// Token: 0x04004095 RID: 16533
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04004096 RID: 16534
		[SerializeField]
		private Collider2D _minimumCollider;

		// Token: 0x04004097 RID: 16535
		[SerializeField]
		private Collider2D _attackCollider;
	}
}
