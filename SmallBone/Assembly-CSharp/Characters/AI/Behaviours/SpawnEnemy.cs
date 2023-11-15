using System;
using System.Collections;
using Characters.Actions;
using Characters.Actions.Constraints;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001323 RID: 4899
	public class SpawnEnemy : Behaviour
	{
		// Token: 0x060060BA RID: 24762 RVA: 0x0011B844 File Offset: 0x00119A44
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (!this.isSatisfy())
			{
				base.result = Behaviour.Result.Done;
				yield break;
			}
			this._spawnAction.TryStart();
			while (this._spawnAction.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x0011B854 File Offset: 0x00119A54
		private bool isSatisfy()
		{
			SpawnEnemy.Condition condition = this._condition;
			if (condition != SpawnEnemy.Condition.CooldownConstraint)
			{
				return condition != SpawnEnemy.Condition.Cleared || this._master.isCleared();
			}
			return this._cooldownConstraint.canUse;
		}

		// Token: 0x04004DF6 RID: 19958
		private Enum a;

		// Token: 0x04004DF7 RID: 19959
		[SerializeField]
		private Characters.Actions.Action _spawnAction;

		// Token: 0x04004DF8 RID: 19960
		[SerializeField]
		private CooldownConstraint _cooldownConstraint;

		// Token: 0x04004DF9 RID: 19961
		[SerializeField]
		private Master _master;

		// Token: 0x04004DFA RID: 19962
		[SerializeField]
		private SpawnEnemy.Condition _condition;

		// Token: 0x02001324 RID: 4900
		private enum Condition
		{
			// Token: 0x04004DFC RID: 19964
			CooldownConstraint,
			// Token: 0x04004DFD RID: 19965
			Cleared
		}
	}
}
