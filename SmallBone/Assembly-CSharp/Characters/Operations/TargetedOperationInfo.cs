using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E48 RID: 3656
	internal class TargetedOperationInfo : MonoBehaviour
	{
		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x060048B3 RID: 18611 RVA: 0x000D41B7 File Offset: 0x000D23B7
		public TargetedCharacterOperation operation
		{
			get
			{
				return this._operation;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x000D41BF File Offset: 0x000D23BF
		public float timeToTrigger
		{
			get
			{
				return this._timeToTrigger;
			}
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x000D41C7 File Offset: 0x000D23C7
		public override string ToString()
		{
			return string.Format("{0:0.##}s({1:0.##}f), {2}", this._timeToTrigger, this._timeToTrigger * 60f, this._operation.GetAutoName());
		}

		// Token: 0x040037C4 RID: 14276
		[SerializeField]
		[FrameTime]
		private float _timeToTrigger;

		// Token: 0x040037C5 RID: 14277
		[TargetedCharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private TargetedCharacterOperation _operation;

		// Token: 0x02000E49 RID: 3657
		[Serializable]
		internal class Subcomponents : SubcomponentArray<TargetedOperationInfo>
		{
			// Token: 0x17000EF0 RID: 3824
			// (get) Token: 0x060048B7 RID: 18615 RVA: 0x000D41FA File Offset: 0x000D23FA
			public float speed
			{
				get
				{
					return this._speed;
				}
			}

			// Token: 0x060048B8 RID: 18616 RVA: 0x000D4204 File Offset: 0x000D2404
			internal void Initialize()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i]._operation.Initialize();
				}
			}

			// Token: 0x060048B9 RID: 18617 RVA: 0x000D4236 File Offset: 0x000D2436
			internal IEnumerator CRun(Character owner, Character target)
			{
				int operationIndex = 0;
				float time = 0f;
				while (operationIndex < base.components.Length)
				{
					while (operationIndex < base.components.Length && time >= base.components[operationIndex].timeToTrigger)
					{
						base.components[operationIndex].operation.Run(owner, target);
						int num = operationIndex;
						operationIndex = num + 1;
					}
					yield return null;
					time += owner.chronometer.animation.deltaTime * this.speed;
				}
				yield break;
			}

			// Token: 0x040037C6 RID: 14278
			[SerializeField]
			private float _speed = 1f;
		}
	}
}
