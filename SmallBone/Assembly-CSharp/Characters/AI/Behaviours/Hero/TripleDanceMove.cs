using System;
using System.Collections.Generic;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A6 RID: 5030
	public sealed class TripleDanceMove : LightMove
	{
		// Token: 0x06006336 RID: 25398 RVA: 0x00120BAF File Offset: 0x0011EDAF
		private void Awake()
		{
			this._swords = new Queue<LightSword>(3);
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x00120BBD File Offset: 0x0011EDBD
		protected override LightSword GetDestination()
		{
			if (this._swords.Count <= 0)
			{
				this.UpdateSwords();
			}
			return this._swords.Dequeue();
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x00120BE0 File Offset: 0x0011EDE0
		private void UpdateSwords()
		{
			ValueTuple<LightSword, LightSword, LightSword> stuck = this._helper.GetStuck();
			LightSword item = stuck.Item1;
			LightSword item2 = stuck.Item2;
			LightSword item3 = stuck.Item3;
			this._swords.Enqueue(item);
			this._swords.Enqueue(item3);
			this._swords.Enqueue(item2);
		}

		// Token: 0x04005005 RID: 20485
		[SerializeField]
		private TripleDanceHelper _helper;

		// Token: 0x04005006 RID: 20486
		private Queue<LightSword> _swords;
	}
}
