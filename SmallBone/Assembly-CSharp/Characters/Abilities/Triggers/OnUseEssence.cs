using System;
using Characters.Player;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B62 RID: 2914
	[Serializable]
	public sealed class OnUseEssence : Trigger
	{
		// Token: 0x06003A59 RID: 14937 RVA: 0x000AC88C File Offset: 0x000AAA8C
		public override void Attach(Character character)
		{
			this._character = character;
			QuintessenceInventory quintessence = character.playerComponents.inventory.quintessence;
			quintessence.onChanged += this.ObserveUsing;
			if (quintessence.items[0] == null)
			{
				return;
			}
			quintessence.items[0].onUse -= base.Invoke;
			quintessence.items[0].onUse += base.Invoke;
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x000AC914 File Offset: 0x000AAB14
		private void ObserveUsing()
		{
			QuintessenceInventory quintessence = this._character.playerComponents.inventory.quintessence;
			if (quintessence.items[0] == null)
			{
				return;
			}
			quintessence.items[0].onUse -= base.Invoke;
			quintessence.items[0].onUse += base.Invoke;
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x000AC986 File Offset: 0x000AAB86
		public override void Detach()
		{
			this._character.playerComponents.inventory.quintessence.onChanged -= this.ObserveUsing;
		}

		// Token: 0x04002E6A RID: 11882
		private Character _character;
	}
}
