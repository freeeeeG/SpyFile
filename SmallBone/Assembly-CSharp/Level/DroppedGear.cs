using System;
using System.Linq;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Data;
using Scenes;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004B8 RID: 1208
	public class DroppedGear : InteractiveObject
	{
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001758 RID: 5976 RVA: 0x00049658 File Offset: 0x00047858
		// (remove) Token: 0x06001759 RID: 5977 RVA: 0x00049690 File Offset: 0x00047890
		public event Action<Character> onLoot;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x0600175A RID: 5978 RVA: 0x000496C8 File Offset: 0x000478C8
		// (remove) Token: 0x0600175B RID: 5979 RVA: 0x00049700 File Offset: 0x00047900
		public event Action<Character> onDestroy;

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x00049735 File Offset: 0x00047935
		public override CharacterInteraction.InteractionType interactionType
		{
			get
			{
				if (!(this.gear == null) && this.gear.destructible)
				{
					return CharacterInteraction.InteractionType.Pressing;
				}
				return CharacterInteraction.InteractionType.Normal;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00049755 File Offset: 0x00047955
		// (set) Token: 0x0600175E RID: 5982 RVA: 0x0004975D File Offset: 0x0004795D
		public Gear gear { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x00049766 File Offset: 0x00047966
		public SpriteRenderer spriteRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0004976E File Offset: 0x0004796E
		public DropMovement dropMovement
		{
			get
			{
				return this._dropMovement;
			}
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00049778 File Offset: 0x00047978
		protected override void Awake()
		{
			base.Awake();
			if (this._dropMovement == null)
			{
				base.Activate();
			}
			else
			{
				this._dropMovement.onGround += this.OnGround;
			}
			this._initialPosition = base.transform.localPosition;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000497C9 File Offset: 0x000479C9
		public void Initialize(Gear gear)
		{
			this.gear = gear;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000497D4 File Offset: 0x000479D4
		private void OnGround()
		{
			base.Activate();
			if (this.gear != null && this._droppedEffect != null)
			{
				if (this.gear.rarity == Rarity.Legendary)
				{
					this._droppedEffect.SpawnLegendaryEffect();
					return;
				}
				if (this.gear.gearTag.HasFlag(Gear.Tag.Omen))
				{
					this._droppedEffect.SpawnOmenEffect();
				}
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00049848 File Offset: 0x00047A48
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y + this.additionalPopupUIOffsetY;
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.gearPopup.Set(this.gear);
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x000498F8 File Offset: 0x00047AF8
		public override void InteractWith(Character character)
		{
			if (this.gear != null && !this.gear.lootable)
			{
				return;
			}
			GameData.Currency currency = GameData.Currency.currencies[this.priceCurrency];
			if (!currency.Has(this.price))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactFailSound, base.transform.position);
				return;
			}
			Item item = this.gear as Item;
			if (item != null)
			{
				if (!character.playerComponents.inventory.item.items.Any((Item i) => i == null))
				{
					Scene<GameBase>.instance.uiManager.itemSelect.Open(item);
					return;
				}
			}
			if (!currency.Consume(this.price))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactFailSound, base.transform.position);
				return;
			}
			this.ClosePopup();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this.price = 0;
			base.transform.localPosition = this._initialPosition;
			if (this.gear != null && this.gear.rarity == Rarity.Legendary && this._droppedEffect != null)
			{
				this._droppedEffect.Despawn();
			}
			Action<Character> action = this.onLoot;
			if (action == null)
			{
				return;
			}
			action(character);
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00049A68 File Offset: 0x00047C68
		public override void InteractWithByPressing(Character character)
		{
			this.ClosePopup();
			if (this.gear != null && this.gear.rarity == Rarity.Legendary && this._droppedEffect != null)
			{
				this._droppedEffect.Despawn();
			}
			Action<Character> action = this.onDestroy;
			if (action != null)
			{
				action(character);
			}
			UnityEngine.Object.Destroy(this.gear.gameObject);
		}

		// Token: 0x04001471 RID: 5233
		[SerializeField]
		private DropMovement _dropMovement;

		// Token: 0x04001472 RID: 5234
		[SerializeField]
		private DroppedEffect _droppedEffect;

		// Token: 0x04001473 RID: 5235
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001474 RID: 5236
		[NonSerialized]
		public GameData.Currency.Type priceCurrency;

		// Token: 0x04001475 RID: 5237
		[NonSerialized]
		public int price;

		// Token: 0x04001476 RID: 5238
		[NonSerialized]
		public float additionalPopupUIOffsetY;

		// Token: 0x04001477 RID: 5239
		private Vector3 _initialPosition;
	}
}
