using System;
using Characters;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004D5 RID: 1237
	public class DroppedWeapon : InteractiveObject
	{
		// Token: 0x0600181B RID: 6171 RVA: 0x0004BABB File Offset: 0x00049CBB
		protected override void Awake()
		{
			base.Awake();
			this._dropMovement.onGround += this.Activate;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00049CE5 File Offset: 0x00047EE5
		private void OnEnable()
		{
			base.Deactivate();
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0004BADC File Offset: 0x00049CDC
		public override void InteractWith(Character character)
		{
			character.playerComponents.inventory.weapon.Equip(this.weapon);
			this._effect.Spawn(base.transform.position, true);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001504 RID: 5380
		[NonSerialized]
		public Weapon weapon;

		// Token: 0x04001505 RID: 5381
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x04001506 RID: 5382
		[GetComponent]
		[SerializeField]
		private DropMovement _dropMovement;
	}
}
