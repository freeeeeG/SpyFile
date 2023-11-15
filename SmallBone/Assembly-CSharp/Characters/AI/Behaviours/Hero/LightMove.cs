using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x02001399 RID: 5017
	public abstract class LightMove : Behaviour
	{
		// Token: 0x06006302 RID: 25346 RVA: 0x001205A4 File Offset: 0x0011E7A4
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			LightSword sword = this.GetDestination();
			if (sword == null)
			{
				Debug.LogError("Sword is Null in LightMove");
			}
			else
			{
				this._destination.position = sword.GetStuckPosition();
				sword.Sign();
				this._move.TryStart();
				while (this._move.running)
				{
					yield return null;
				}
				sword.Despawn();
				base.result = Behaviour.Result.Success;
			}
			yield break;
		}

		// Token: 0x06006303 RID: 25347
		protected abstract LightSword GetDestination();

		// Token: 0x04004FDA RID: 20442
		[SerializeField]
		private Transform _destination;

		// Token: 0x04004FDB RID: 20443
		[SerializeField]
		private Characters.Actions.Action _move;
	}
}
