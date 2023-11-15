using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012B8 RID: 4792
	public class Repeat : Decorator
	{
		// Token: 0x06005EE4 RID: 24292 RVA: 0x001163C7 File Offset: 0x001145C7
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			int count = UnityEngine.Random.Range(this._countRange.x, this._countRange.y + 1);
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this._behaviour.CRun(controller);
				num = i;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004C38 RID: 19512
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2Int _countRange;

		// Token: 0x04004C39 RID: 19513
		[SerializeField]
		[Behaviour.SubcomponentAttribute(true)]
		private Behaviour _behaviour;
	}
}
