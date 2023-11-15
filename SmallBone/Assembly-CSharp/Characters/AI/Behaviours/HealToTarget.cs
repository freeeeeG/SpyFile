using System;
using System.Collections;
using Characters.Actions;
using FX;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012E4 RID: 4836
	public class HealToTarget : Behaviour
	{
		// Token: 0x06005FA3 RID: 24483 RVA: 0x00118198 File Offset: 0x00116398
		public override IEnumerator CRun(AIController controller)
		{
			if (this._target == null)
			{
				yield break;
			}
			base.result = Behaviour.Result.Doing;
			this._healMotion.TryStart();
			int elapsed = 0;
			while (this._healMotion.running)
			{
				yield return null;
				this._time += controller.character.chronometer.master.deltaTime;
				if (this._time >= (float)elapsed)
				{
					break;
				}
			}
			this._info.Spawn(this._target.transform.position, 0f, 1f);
			int num2;
			for (int i = 0; i < this._count; i = num2 + 1)
			{
				float num = UnityEngine.Random.Range(this._amount.x, this._amount.y);
				this._target.health.Heal((double)num, true);
				yield return Chronometer.global.WaitForSeconds(this._delay);
				num2 = i;
			}
			while (this._healMotion.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x001181AE File Offset: 0x001163AE
		public void SetTarget(Character target)
		{
			this._target = target;
		}

		// Token: 0x04004CE2 RID: 19682
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _amount;

		// Token: 0x04004CE3 RID: 19683
		[SerializeField]
		private Characters.Actions.Action _healMotion;

		// Token: 0x04004CE4 RID: 19684
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x04004CE5 RID: 19685
		[SerializeField]
		[Range(1f, 10f)]
		private int _count = 1;

		// Token: 0x04004CE6 RID: 19686
		[SerializeField]
		[FrameTime]
		private float _time;

		// Token: 0x04004CE7 RID: 19687
		[SerializeField]
		[FrameTime]
		private float _delay = 0.1f;

		// Token: 0x04004CE8 RID: 19688
		private Character _target;
	}
}
