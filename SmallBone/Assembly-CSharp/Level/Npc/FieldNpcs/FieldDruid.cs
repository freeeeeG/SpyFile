using System;
using System.Collections;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005D4 RID: 1492
	public sealed class FieldDruid : FieldNpc
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x0005AB7F File Offset: 0x00058D7F
		protected override NpcType _type
		{
			get
			{
				return NpcType.FieldDruid;
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0005AB84 File Offset: 0x00058D84
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0005ABE8 File Offset: 0x00058DE8
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGreetingAndConfirm(character, null));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0005AC29 File Offset: 0x00058E29
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			int lastIndex = 3;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			this.DropWeapon();
			this._phase = FieldNpc.Phase.Gave;
			for (int i = lastIndex; i < base._greeting.Length; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0005AC38 File Offset: 0x00058E38
		private void Load()
		{
			ItemInventory item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
			int num = 7;
			for (int i = 0; i < num; i++)
			{
				this._selected = this._items.Random(this._random);
				if (!item.Has(this._selected))
				{
					break;
				}
			}
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0005AC98 File Offset: 0x00058E98
		private void DropWeapon()
		{
			this.Load();
			Singleton<Service>.Instance.levelManager.DropItem(this._selected, this._dropPosition.position);
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, base.transform.position);
		}

		// Token: 0x04001930 RID: 6448
		private const int _randomSeed = 2028506624;

		// Token: 0x04001931 RID: 6449
		[SerializeField]
		private Item[] _items;

		// Token: 0x04001932 RID: 6450
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x04001933 RID: 6451
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x04001934 RID: 6452
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x04001935 RID: 6453
		private Item _selected;

		// Token: 0x04001936 RID: 6454
		private System.Random _random;
	}
}
