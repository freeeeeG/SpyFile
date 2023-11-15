using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012C1 RID: 4801
	public class WeightedSelector : Decorator
	{
		// Token: 0x06005F09 RID: 24329 RVA: 0x00116758 File Offset: 0x00114958
		private void Awake()
		{
			List<ValueTuple<Behaviour, float>> list = new List<ValueTuple<Behaviour, float>>();
			foreach (Weight weight in this._weights.components)
			{
				list.Add(new ValueTuple<Behaviour, float>(weight.key, (float)weight.value));
			}
			this._weightedRandomizer = new WeightedRandomizer<Behaviour>(list);
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x001167AD File Offset: 0x001149AD
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._weightedRandomizer.TakeOne().CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004C53 RID: 19539
		private WeightedRandomizer<Behaviour> _weightedRandomizer;

		// Token: 0x04004C54 RID: 19540
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Weight))]
		private Weight.Subcomponents _weights;
	}
}
