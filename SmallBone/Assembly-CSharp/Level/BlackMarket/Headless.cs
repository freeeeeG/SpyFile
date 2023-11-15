using System;
using System.Collections;
using System.Linq;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x0200061A RID: 1562
	public class Headless : Npc
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0005F2C8 File Offset: 0x0005D4C8
		public string submitLine
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/headless/submit/line").Random<string>();
			}
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0005F2DC File Offset: 0x0005D4DC
		private void OnDisable()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player != null)
			{
				player.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChanged;
			}
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0005F324 File Offset: 0x0005D524
		private void OnWeaponChanged(Weapon old, Weapon @new)
		{
			if (@new != this._displayedGear)
			{
				return;
			}
			@new.destructible = true;
			this._lineText.Run(this.submitLine);
			if (old == null)
			{
				WeaponInventory weapon = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
				old = weapon.next;
				weapon.Unequip(old);
			}
			this._headSlot.enabled = true;
			this._headSlot.sprite = old.dropped.spriteRenderer.sprite;
			this._animator.Play(Headless._activateWithHeadHash, 0, 0f);
			this._headSlotAnimator.Play(Npc._activateHash, 0, 0f);
			old.destructible = false;
			UnityEngine.Object.Destroy(old.gameObject);
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChanged;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0005F424 File Offset: 0x0005D624
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 548136436 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			if (MMMaths.PercentChance(this._random, currentChapter.currentStage.marketSettings.headlessPossibility))
			{
				base.Activate();
				return;
			}
			base.Deactivate();
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0005F497 File Offset: 0x0005D697
		private IEnumerator CDropGear()
		{
			Rarity rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings.headlessHeadPossibilities.Evaluate(this._random);
			WeaponReference weaponToTake = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, rarity);
			WeaponInventory weapon2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
			for (;;)
			{
				if ((from weapon in weapon2.weapons
				where weapon != null
				select weapon).Count<Weapon>() > 1 || !weaponToTake.name.Equals("BombSkul", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				weaponToTake = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, rarity);
			}
			WeaponRequest request = weaponToTake.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			this._displayedGear = levelManager.DropWeapon(request, this._slot.position);
			this._displayedGear.destructible = false;
			this._displayedGear.dropped.dropMovement.Stop();
			this._displayedGear.dropped.dropMovement.Float();
			yield break;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0005F4A8 File Offset: 0x0005D6A8
		protected override void OnActivate()
		{
			this._lineText.gameObject.SetActive(true);
			this._talk.SetActive(true);
			this._headSlot.enabled = false;
			base.StartCoroutine(this.CDropGear());
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.onChanged += this.OnWeaponChanged;
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0005F51A File Offset: 0x0005D71A
		protected override void OnDeactivate()
		{
			this._lineText.gameObject.SetActive(false);
			this._headSlot.enabled = false;
		}

		// Token: 0x04001A75 RID: 6773
		private const int _randomSeed = 548136436;

		// Token: 0x04001A76 RID: 6774
		protected static readonly int _activateWithHeadHash = Animator.StringToHash("ActivateWithHead");

		// Token: 0x04001A77 RID: 6775
		[SerializeField]
		private Transform _slot;

		// Token: 0x04001A78 RID: 6776
		[SerializeField]
		private SpriteRenderer _headSlot;

		// Token: 0x04001A79 RID: 6777
		[SerializeField]
		private Animator _headSlotAnimator;

		// Token: 0x04001A7A RID: 6778
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001A7B RID: 6779
		[SerializeField]
		private GameObject _talk;

		// Token: 0x04001A7C RID: 6780
		private Weapon _displayedGear;

		// Token: 0x04001A7D RID: 6781
		private System.Random _random;
	}
}
