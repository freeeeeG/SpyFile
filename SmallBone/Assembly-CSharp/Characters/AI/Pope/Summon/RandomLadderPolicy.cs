using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x0200121E RID: 4638
	public class RandomLadderPolicy : LadderPolicy
	{
		// Token: 0x06005AE7 RID: 23271 RVA: 0x0010D374 File Offset: 0x0010B574
		private void Awake()
		{
			this._ladders = new FanaticLadder[this._ladderContainer.childCount];
			int num = 0;
			foreach (object obj in this._ladderContainer)
			{
				Transform transform = (Transform)obj;
				this._ladders[num++] = transform.GetComponent<FanaticLadder>();
			}
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x0010D3F0 File Offset: 0x0010B5F0
		protected override void GetLadders(ref List<FanaticLadder> results, int count)
		{
			results.Clear();
			this._ladders.Shuffle<FanaticLadder>();
			FanaticLadder[] array = this._ladders.Take(count).ToArray<FanaticLadder>();
			for (int i = 0; i < count; i++)
			{
				results.Add(array[i]);
			}
		}

		// Token: 0x04004956 RID: 18774
		[SerializeField]
		private Transform _ladderContainer;

		// Token: 0x04004957 RID: 18775
		private FanaticLadder[] _ladders;
	}
}
