using System;
using UnityEngine;

namespace BT
{
	// Token: 0x0200140D RID: 5133
	public class Random : Composite
	{
		// Token: 0x060064FF RID: 25855 RVA: 0x00124439 File Offset: 0x00122639
		protected virtual Node GetChild(int i)
		{
			if (i >= this._child.components.Length || i < 0)
			{
				throw new ArgumentException(string.Format("{0} : invalid child index", i));
			}
			return this._child.components[i].node;
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x0012473E File Offset: 0x0012293E
		protected override void OnInitialize()
		{
			this._index = UnityEngine.Random.Range(0, this._child.components.Length);
			base.OnInitialize();
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x0012475F File Offset: 0x0012295F
		protected override NodeState UpdateDeltatime(Context context)
		{
			return this.GetChild(this._index).Tick(context);
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x00124773 File Offset: 0x00122973
		protected override void DoReset(NodeState state)
		{
			this._index = UnityEngine.Random.Range(0, this._child.components.Length);
			base.DoReset(state);
		}

		// Token: 0x04005159 RID: 20825
		private int _index;
	}
}
