using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001300 RID: 4864
	public sealed class RunBreakableAction : Behaviour
	{
		// Token: 0x0600602E RID: 24622 RVA: 0x001196F6 File Offset: 0x001178F6
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._damageAmount = 0.0;
			CharacterHealth targetHealth = controller.character.health;
			targetHealth.onTookDamage += new TookDamageDelegate(this.TargetHealth_onTookDamage);
			bool doGroggy = false;
			this._ready.TryStart();
			while (this._ready.running)
			{
				yield return null;
				if (this._damageAmount >= this._damageForGroggy)
				{
					doGroggy = true;
					break;
				}
			}
			if (doGroggy)
			{
				yield return this._groggy.CRun(controller);
			}
			targetHealth.onTookDamage -= new TookDamageDelegate(this.TargetHealth_onTookDamage);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x0011970C File Offset: 0x0011790C
		private void TargetHealth_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._damageAmount += damageDealt;
		}

		// Token: 0x04004D64 RID: 19812
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x04004D65 RID: 19813
		[SerializeField]
		private RunAction _groggy;

		// Token: 0x04004D66 RID: 19814
		[SerializeField]
		private double _damageForGroggy;

		// Token: 0x04004D67 RID: 19815
		private double _damageAmount;
	}
}
