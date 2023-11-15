using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E7A RID: 3706
	public class Discard : Operation
	{
		// Token: 0x06004974 RID: 18804 RVA: 0x000D6CD7 File Offset: 0x000D4ED7
		public override void Run()
		{
			this._item.DiscardOnInventory();
		}

		// Token: 0x040038B6 RID: 14518
		[SerializeField]
		private Item _item;
	}
}
