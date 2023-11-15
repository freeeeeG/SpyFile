using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012AF RID: 4783
	public class Count : Decorator
	{
		// Token: 0x06005EC2 RID: 24258 RVA: 0x001160CD File Offset: 0x001142CD
		private void OnEnable()
		{
			this._current = 0;
			this._max = UnityEngine.Random.Range(this._range.x, this._range.y + 1);
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x001160F9 File Offset: 0x001142F9
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._current >= this._max)
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			this._current++;
			yield return this._behaviour.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004C1F RID: 19487
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2Int _range;

		// Token: 0x04004C20 RID: 19488
		[SerializeField]
		[Behaviour.SubcomponentAttribute(true)]
		private Behaviour _behaviour;

		// Token: 0x04004C21 RID: 19489
		private int _max;

		// Token: 0x04004C22 RID: 19490
		private int _current;
	}
}
