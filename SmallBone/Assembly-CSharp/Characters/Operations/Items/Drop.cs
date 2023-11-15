using System;
using Characters.Gear.Items;
using Level;
using UnityEngine;

namespace Characters.Operations.Items
{
	// Token: 0x02000E7B RID: 3707
	public class Drop : CharacterOperation
	{
		// Token: 0x06004976 RID: 18806 RVA: 0x000D6CE4 File Offset: 0x000D4EE4
		public override void Run(Character owner)
		{
			Item item = UnityEngine.Object.Instantiate<Item>(this._item, owner.transform.position, Quaternion.identity);
			item.name = this._item.name;
			item.transform.parent = Map.Instance.transform;
			item.Initialize();
		}

		// Token: 0x040038B7 RID: 14519
		[SerializeField]
		private Item _item;
	}
}
