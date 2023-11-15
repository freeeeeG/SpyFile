using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010B4 RID: 4276
	public sealed class AgedFanaticAI : AIController
	{
		// Token: 0x060052E4 RID: 21220 RVA: 0x000F8840 File Offset: 0x000F6A40
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._attack,
				this._keepDistance,
				this._sacrifice
			};
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x000F88A0 File Offset: 0x000F6AA0
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x000F88C8 File Offset: 0x000F6AC8
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x000F88D7 File Offset: 0x000F6AD7
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			while (!base.dead)
			{
				if (this._sacrifice.result.Equals(Characters.AI.Behaviours.Behaviour.Result.Doing))
				{
					yield return null;
				}
				else
				{
					if (base.FindClosestPlayerBody(this._keepDistanceTrigger) != null)
					{
						yield return this._keepDistance.CRun(this);
					}
					if (this._sacrifice.result.Equals(Characters.AI.Behaviours.Behaviour.Result.Doing))
					{
						yield return null;
					}
					else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
					{
						yield return this._attack.CRun(this);
					}
					else
					{
						yield return this._chase.CRun(this);
						if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
						{
							yield return this._attack.CRun(this);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0400428C RID: 17036
		[Header("Behaviours")]
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400428D RID: 17037
		[SerializeField]
		[Subcomponent(typeof(Wander))]
		private Wander _wander;

		// Token: 0x0400428E RID: 17038
		[Subcomponent(typeof(Chase))]
		[SerializeField]
		private Chase _chase;

		// Token: 0x0400428F RID: 17039
		[SerializeField]
		[Subcomponent(typeof(CircularProjectileAttack))]
		private CircularProjectileAttack _attack;

		// Token: 0x04004290 RID: 17040
		[SerializeField]
		[Subcomponent(typeof(KeepDistance))]
		private KeepDistance _keepDistance;

		// Token: 0x04004291 RID: 17041
		[SerializeField]
		[Subcomponent(typeof(Sacrifice))]
		private Sacrifice _sacrifice;

		// Token: 0x04004292 RID: 17042
		[Space]
		[SerializeField]
		[Header("Tools")]
		private Collider2D _attackTrigger;

		// Token: 0x04004293 RID: 17043
		[SerializeField]
		private Collider2D _keepDistanceTrigger;
	}
}
