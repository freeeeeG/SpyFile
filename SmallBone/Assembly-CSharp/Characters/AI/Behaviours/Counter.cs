using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012DE RID: 4830
	public sealed class Counter : Decorator
	{
		// Token: 0x06005F88 RID: 24456 RVA: 0x00117CAA File Offset: 0x00115EAA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._ownerHealth = controller.character.health;
			float duration = UnityEngine.Random.Range(this._duration.x, this._duration.y);
			float elapsed = 0f;
			this._success = false;
			this._ownerHealth.onTookDamage -= new TookDamageDelegate(this.RunCounter);
			this._ownerHealth.onTookDamage += new TookDamageDelegate(this.RunCounter);
			while (elapsed < duration && !this._success)
			{
				elapsed += controller.character.chronometer.master.deltaTime;
				yield return null;
			}
			if (this._success)
			{
				this._ownerHealth.onTookDamage -= new TookDamageDelegate(this.RunCounter);
				yield return this._behaviour.CRun(controller);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005F89 RID: 24457 RVA: 0x00117CC0 File Offset: 0x00115EC0
		private void RunCounter(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._success = true;
			this._ownerHealth.onTookDamage -= new TookDamageDelegate(this.RunCounter);
		}

		// Token: 0x04004CC7 RID: 19655
		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2 _duration;

		// Token: 0x04004CC8 RID: 19656
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004CC9 RID: 19657
		private Health _ownerHealth;

		// Token: 0x04004CCA RID: 19658
		private bool _success;
	}
}
