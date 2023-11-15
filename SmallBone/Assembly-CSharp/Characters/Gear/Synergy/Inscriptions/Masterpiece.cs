using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Items;
using Characters.Operations;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200089C RID: 2204
	public sealed class Masterpiece : InscriptionInstance
	{
		// Token: 0x06002EBD RID: 11965 RVA: 0x0008C928 File Offset: 0x0008AB28
		protected override void Initialize()
		{
			this._statBonus.Initialize();
			this._canUse = true;
			this._itemIndics = new Queue<ValueTuple<int, int>>(9);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x0008C949 File Offset: 0x0008AB49
		public override void Attach()
		{
			base.character.playerComponents.inventory.item.onChanged += this.EnhanceItems;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x0008C974 File Offset: 0x0008AB74
		public override void Detach()
		{
			base.character.playerComponents.inventory.item.onChanged -= this.EnhanceItems;
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamageDelegate));
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x0008C9C4 File Offset: 0x0008ABC4
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			if (this._upgrading)
			{
				return;
			}
			if (this.keyword.step >= 1)
			{
				if (!base.character.onGiveDamage.Contains(new GiveDamageDelegate(this.OnGiveDamageDelegate)))
				{
					base.character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamageDelegate));
					return;
				}
			}
			else
			{
				base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamageDelegate));
			}
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x0008CA45 File Offset: 0x0008AC45
		private bool OnGiveDamageDelegate(ITarget target, ref Damage damage)
		{
			if (!this._canUse || damage.attribute != Damage.Attribute.Physical)
			{
				return false;
			}
			base.character.ability.Add(this._statBonus.ability);
			this._remainCooldownTime = this._cooldownTime;
			return false;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x0008CA84 File Offset: 0x0008AC84
		private void Update()
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			this._remainCooldownTime -= base.character.chronometer.master.deltaTime;
			if (this._remainCooldownTime < 0f)
			{
				this._canUse = true;
				return;
			}
			this._canUse = false;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x0008CAE0 File Offset: 0x0008ACE0
		private void EnhanceItems()
		{
			if (this._upgrading)
			{
				return;
			}
			if (this.keyword.step < this.keyword.steps.Count - 1)
			{
				return;
			}
			if (!base.character.playerComponents.inventory.item.items.Any((Item item) => !(item == null) && (item.keyword1 == this.keyword.key || item.keyword2 == this.keyword.key)))
			{
				return;
			}
			this._upgrading = true;
			this.UpdateItemIndex();
			this.ChangeItems();
			this._upgrading = false;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x0008CB60 File Offset: 0x0008AD60
		private void UpdateItemIndex()
		{
			ItemInventory item = base.character.playerComponents.inventory.item;
			for (int i = 0; i < item.items.Count; i++)
			{
				Item item2 = item.items[i];
				if (!(item2 == null) && (item2.keyword1 == this.keyword.key || item2.keyword2 == this.keyword.key))
				{
					for (int j = 0; j < this._enhancementMaps.Length; j++)
					{
						ItemReference itemReference;
						if (GearResource.instance.TryGetItemReferenceByGuid(this._enhancementMaps[j].from.AssetGUID, out itemReference) && item2.name.Equals(itemReference.name, StringComparison.OrdinalIgnoreCase))
						{
							this._itemIndics.Enqueue(new ValueTuple<int, int>(i, j));
							break;
						}
					}
				}
			}
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x0008CC3C File Offset: 0x0008AE3C
		private void ChangeItems()
		{
			ItemInventory item = base.character.playerComponents.inventory.item;
			if (this._itemIndics.Count > 0)
			{
				base.StartCoroutine(this._onEnhanced.CRun(base.character));
			}
			while (this._itemIndics.Count > 0)
			{
				ValueTuple<int, int> valueTuple = this._itemIndics.Dequeue();
				int item2 = valueTuple.Item1;
				int item3 = valueTuple.Item2;
				Item item4 = item.items[item2];
				ItemReference itemReference;
				if (GearResource.instance.TryGetItemReferenceByGuid(this._enhancementMaps[item3].to.AssetGUID, out itemReference))
				{
					ItemRequest itemRequest = itemReference.LoadAsync();
					itemRequest.WaitForCompletion();
					Item item5 = Singleton<Service>.Instance.levelManager.DropItem(itemRequest, Vector3.zero);
					item5.keyword1 = item4.keyword1;
					item5.keyword2 = item4.keyword2;
					item4.ChangeOnInventory(item5);
				}
			}
		}

		// Token: 0x040026D3 RID: 9939
		[Header("2세트 효과")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x040026D4 RID: 9940
		[SerializeField]
		[Subcomponent(typeof(StatBonusComponent))]
		private StatBonusComponent _statBonus;

		// Token: 0x040026D5 RID: 9941
		[Header("4세트 효과")]
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onEnhanced;

		// Token: 0x040026D6 RID: 9942
		[SerializeField]
		private Masterpiece.EnhancementMap[] _enhancementMaps;

		// Token: 0x040026D7 RID: 9943
		private bool _canUse;

		// Token: 0x040026D8 RID: 9944
		private bool _upgrading;

		// Token: 0x040026D9 RID: 9945
		private Queue<ValueTuple<int, int>> _itemIndics;

		// Token: 0x040026DA RID: 9946
		private float _remainCooldownTime;

		// Token: 0x0200089D RID: 2205
		[Serializable]
		private class EnhancementMap
		{
			// Token: 0x170009FD RID: 2557
			// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x0008CD62 File Offset: 0x0008AF62
			public AssetReference from
			{
				get
				{
					return this._from;
				}
			}

			// Token: 0x170009FE RID: 2558
			// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x0008CD6A File Offset: 0x0008AF6A
			public AssetReference to
			{
				get
				{
					return this._to;
				}
			}

			// Token: 0x040026DB RID: 9947
			[SerializeField]
			private AssetReference _from;

			// Token: 0x040026DC RID: 9948
			[SerializeField]
			private AssetReference _to;
		}
	}
}
