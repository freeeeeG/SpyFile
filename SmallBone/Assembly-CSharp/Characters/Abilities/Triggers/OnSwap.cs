using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B5C RID: 2908
	[Serializable]
	public class OnSwap : Trigger
	{
		// Token: 0x06003A48 RID: 14920 RVA: 0x000AC5A1 File Offset: 0x000AA7A1
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.playerComponents.inventory.weapon.onSwap += this.Check;
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x000AC5D0 File Offset: 0x000AA7D0
		public override void Detach()
		{
			this._character.playerComponents.inventory.weapon.onSwap -= this.Check;
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x000AC5F8 File Offset: 0x000AA7F8
		private void Check()
		{
			if (!this._types[this._character.playerComponents.inventory.weapon.polymorphOrCurrent.category])
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x04002E5C RID: 11868
		[SerializeField]
		private WeaponTypeBoolArray _types;

		// Token: 0x04002E5D RID: 11869
		private Character _character;
	}
}
