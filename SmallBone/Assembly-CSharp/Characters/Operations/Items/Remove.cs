using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E7F RID: 3711
	public class Remove : Operation
	{
		// Token: 0x06004987 RID: 18823 RVA: 0x000D6F35 File Offset: 0x000D5135
		public override void Run()
		{
			this._item.RemoveOnInventory();
		}

		// Token: 0x040038C3 RID: 14531
		[SerializeField]
		private Item _item;
	}
}
