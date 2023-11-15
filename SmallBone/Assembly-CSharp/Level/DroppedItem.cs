using System;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004BD RID: 1213
	public class DroppedItem : InteractiveObject
	{
		// Token: 0x0600177B RID: 6011 RVA: 0x00049CC5 File Offset: 0x00047EC5
		protected override void Awake()
		{
			base.Awake();
			this._dropMovement.onGround += this.Activate;
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00049CE5 File Offset: 0x00047EE5
		private void OnEnable()
		{
			base.Deactivate();
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00049CF0 File Offset: 0x00047EF0
		public override void InteractWith(Character character)
		{
			if (this._clip != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._clip, base.transform.position, 0.05f);
			}
			ItemInventory itemInventory = character.playerComponents.inventory.item;
			if (!itemInventory.TryEquip(this.item))
			{
				itemInventory.EquipAt(this.item, 0);
			}
			this._effect.Spawn(base.transform.position, true);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001486 RID: 5254
		[NonSerialized]
		public Item item;

		// Token: 0x04001487 RID: 5255
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x04001488 RID: 5256
		[SerializeField]
		[GetComponent]
		private DropMovement _dropMovement;

		// Token: 0x04001489 RID: 5257
		[SerializeField]
		private AudioClip _clip;
	}
}
