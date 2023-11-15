using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200036D RID: 877
	internal class TriggerArray : MonoBehaviour
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x00030310 File Offset: 0x0002E510
		public Trigger trigger
		{
			get
			{
				return this._trigger;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00030318 File Offset: 0x0002E518
		public override string ToString()
		{
			return this.trigger.GetAutoName() ?? "";
		}

		// Token: 0x04000D3F RID: 3391
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x0200036E RID: 878
		[Serializable]
		internal class Subcomponents : SubcomponentArray<TriggerArray>
		{
			// Token: 0x0600102F RID: 4143 RVA: 0x0003032E File Offset: 0x0002E52E
			internal IEnumerable<bool> CCheckNext()
			{
				int num;
				for (int operationIndex = 0; operationIndex < base.components.Length; operationIndex = num + 1)
				{
					yield return base.components[operationIndex].trigger.IsSatisfied();
					num = operationIndex;
				}
				yield break;
			}
		}
	}
}
