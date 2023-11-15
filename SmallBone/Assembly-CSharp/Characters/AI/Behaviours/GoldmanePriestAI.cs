using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200133A RID: 4922
	public sealed class GoldmanePriestAI : AIController
	{
		// Token: 0x06006116 RID: 24854 RVA: 0x0011C724 File Offset: 0x0011A924
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x0011C74C File Offset: 0x0011A94C
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.Combat();
			yield break;
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x0011C75B File Offset: 0x0011A95B
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
						if (this.CanStartToHeal())
						{
							yield return this._heal.CRun(this);
						}
					}
					else if (this.CanStartToHeal())
					{
						yield return this._heal.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x06006119 RID: 24857 RVA: 0x0011C76C File Offset: 0x0011A96C
		private bool CanStartToHeal()
		{
			foreach (Character character in base.FindEnemiesInRange(this._healRange))
			{
				if (character.gameObject.activeSelf && !character.health.dead && character.health.percent < 1.0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004E4C RID: 20044
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004E4D RID: 20045
		[SerializeField]
		[Subcomponent(typeof(KeepDistance))]
		private KeepDistance _keepDistance;

		// Token: 0x04004E4E RID: 20046
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _heal;

		// Token: 0x04004E4F RID: 20047
		[SerializeField]
		private Collider2D _minimumCollider;

		// Token: 0x04004E50 RID: 20048
		[SerializeField]
		private Collider2D _healRange;
	}
}
