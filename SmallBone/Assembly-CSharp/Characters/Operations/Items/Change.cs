using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E79 RID: 3705
	public class Change : Operation
	{
		// Token: 0x06004972 RID: 18802 RVA: 0x000D6CBF File Offset: 0x000D4EBF
		public override void Run()
		{
			this._item.ChangeOnInventory(this._itemToChange.Instantiate());
		}

		// Token: 0x040038B4 RID: 14516
		[SerializeField]
		private Item _item;

		// Token: 0x040038B5 RID: 14517
		[SerializeField]
		private Item _itemToChange;
	}
}
