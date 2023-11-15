using System;
using Characters.Gear.Weapons;
using Characters.Player;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200071E RID: 1822
	public sealed class SilenceMark : MonoBehaviour
	{
		// Token: 0x060024EE RID: 9454 RVA: 0x0006EF48 File Offset: 0x0006D148
		private void Update()
		{
			if (this._character.silence.value)
			{
				WeaponInventory weapon = this._character.playerComponents.inventory.weapon;
				if (!this._image.enabled || this._lastWeapon == null || this._lastWeapon != weapon.current)
				{
					this._image.enabled = true;
					Bounds bounds = this._character.collider.bounds;
					this._lastWeapon = weapon.current;
					this._image.transform.position = new Vector2(bounds.min.x - 0.3f, bounds.max.y + 0.35f);
				}
				return;
			}
			if (!this._image.enabled)
			{
				return;
			}
			this._image.enabled = false;
		}

		// Token: 0x04001F52 RID: 8018
		[SerializeField]
		private Character _character;

		// Token: 0x04001F53 RID: 8019
		[SerializeField]
		private SpriteRenderer _image;

		// Token: 0x04001F54 RID: 8020
		private Weapon _lastWeapon;
	}
}
