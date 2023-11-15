using System;
using Characters;
using Characters.Player;

namespace FX
{
	// Token: 0x0200024C RID: 588
	public class PlayerSpriteEffectStack : SpriteEffectStack, ISpriteEffectStack
	{
		// Token: 0x06000B93 RID: 2963 RVA: 0x0001FE30 File Offset: 0x0001E030
		protected override void Awake()
		{
			base.Awake();
			Character component = base.GetComponent<Character>();
			this._chronometer = component.chronometer.animation;
			this._weaponInventory = component.playerComponents.inventory.weapon;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0001FE71 File Offset: 0x0001E071
		protected override void LateUpdate()
		{
			this._spriteRenderer = this._weaponInventory.polymorphOrCurrent.characterAnimation.spriteRenderer;
			base.LateUpdate();
		}

		// Token: 0x0400099C RID: 2460
		private WeaponInventory _weaponInventory;
	}
}
