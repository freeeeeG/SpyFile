using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x02001217 RID: 4631
	public abstract class LadderPolicy : MonoBehaviour
	{
		// Token: 0x06005AD7 RID: 23255 RVA: 0x0010D140 File Offset: 0x0010B340
		public List<FanaticLadder> GetLadders(List<FanaticFactory.SummonType> summonTypes)
		{
			if (this._results == null)
			{
				this._results = new List<FanaticLadder>(summonTypes.Count);
			}
			this.GetLadders(ref this._results, summonTypes.Count);
			for (int i = 0; i < summonTypes.Count; i++)
			{
				this._results[i].SetFanatic(summonTypes[i]);
			}
			return this._results;
		}

		// Token: 0x06005AD8 RID: 23256
		protected abstract void GetLadders(ref List<FanaticLadder> results, int count);

		// Token: 0x0400494C RID: 18764
		private List<FanaticLadder> _results;

		// Token: 0x02001218 RID: 4632
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005ADA RID: 23258 RVA: 0x0010D1A7 File Offset: 0x0010B3A7
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, LadderPolicy.SubcomponentAttribute.types)
			{
			}

			// Token: 0x0400494D RID: 18765
			public new static readonly Type[] types = new Type[]
			{
				typeof(RandomLadderPolicy)
			};
		}
	}
}
