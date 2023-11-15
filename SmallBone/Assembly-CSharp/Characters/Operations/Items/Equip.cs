using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E7E RID: 3710
	public class Equip : CharacterOperation
	{
		// Token: 0x06004985 RID: 18821 RVA: 0x000D6F09 File Offset: 0x000D5109
		public override void Run(Character owner)
		{
			Debug.Log(owner.playerComponents.inventory.item.TryEquip(this._item.Instantiate()));
		}

		// Token: 0x040038C2 RID: 14530
		[SerializeField]
		private Item _item;
	}
}
