using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x02001395 RID: 5013
	public sealed class Groggy : Behaviour
	{
		// Token: 0x060062EF RID: 25327 RVA: 0x001201D2 File Offset: 0x0011E3D2
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._sign.CRun(controller);
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

		// Token: 0x060062F0 RID: 25328 RVA: 0x001201E8 File Offset: 0x0011E3E8
		private void TargetHealth_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._damageAmount += damageDealt;
		}

		// Token: 0x04004FC0 RID: 20416
		[SerializeField]
		private RunAction _sign;

		// Token: 0x04004FC1 RID: 20417
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x04004FC2 RID: 20418
		[SerializeField]
		private RunAction _groggy;

		// Token: 0x04004FC3 RID: 20419
		[SerializeField]
		private double _damageForGroggy;

		// Token: 0x04004FC4 RID: 20420
		private double _damageAmount;
	}
}
