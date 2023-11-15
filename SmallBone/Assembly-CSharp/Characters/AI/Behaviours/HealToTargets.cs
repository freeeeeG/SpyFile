using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using FX;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012E6 RID: 4838
	public class HealToTargets : Behaviour
	{
		// Token: 0x06005FAC RID: 24492 RVA: 0x00118379 File Offset: 0x00116579
		public override IEnumerator CRun(AIController controller)
		{
			while (this._healMotion.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x00002191 File Offset: 0x00000391
		public void SetTarget(List<Character> targets)
		{
		}

		// Token: 0x04004CEF RID: 19695
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _amount;

		// Token: 0x04004CF0 RID: 19696
		[SerializeField]
		private Characters.Actions.Action _healMotion;

		// Token: 0x04004CF1 RID: 19697
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x04004CF2 RID: 19698
		[SerializeField]
		[Range(1f, 10f)]
		private int _count = 1;

		// Token: 0x04004CF3 RID: 19699
		[SerializeField]
		[FrameTime]
		private float _time;

		// Token: 0x04004CF4 RID: 19700
		[SerializeField]
		[FrameTime]
		private float _delay = 0.1f;
	}
}
