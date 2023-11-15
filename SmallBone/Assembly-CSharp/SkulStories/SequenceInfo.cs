using System;
using System.Collections;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000120 RID: 288
	public class SequenceInfo : MonoBehaviour
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00010E84 File Offset: 0x0000F084
		public Sequence sequence
		{
			get
			{
				return this._sequence;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00010E8C File Offset: 0x0000F08C
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this._tag))
			{
				return this._tag;
			}
			return this.GetAutoName();
		}

		// Token: 0x0400044B RID: 1099
		[SerializeField]
		private string _tag;

		// Token: 0x0400044C RID: 1100
		[SerializeField]
		[Sequence.SubcomponentAttribute]
		private Sequence _sequence;

		// Token: 0x02000121 RID: 289
		[Serializable]
		internal class Subcomponents : SubcomponentArray<SequenceInfo>
		{
			// Token: 0x060005A7 RID: 1447 RVA: 0x00010EA8 File Offset: 0x0000F0A8
			internal IEnumerator CRun()
			{
				int num;
				for (int operationIndex = 0; operationIndex < base.components.Length; operationIndex = num + 1)
				{
					yield return base.components[operationIndex].sequence.CCheckWait();
					num = operationIndex;
				}
				yield break;
			}
		}
	}
}
