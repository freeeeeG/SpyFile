using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000BB7 RID: 2999
public class OutfitDesignerScreen_OutfitState
{
	// Token: 0x06005DC3 RID: 24003 RVA: 0x00224FA8 File Offset: 0x002231A8
	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		this.name = sourceTarget.ReadName();
		this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			this.ApplyItem(item);
		}
	}

	// Token: 0x06005DC4 RID: 24004 RVA: 0x0022502C File Offset: 0x0022322C
	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		global::Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	// Token: 0x06005DC5 RID: 24005 RVA: 0x00225048 File Offset: 0x00223248
	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	// Token: 0x06005DC6 RID: 24006 RVA: 0x00225064 File Offset: 0x00223264
	public unsafe void ApplyItem(ClothingItemResource item)
	{
		*this.slots.GetItemSlotForCategory(item.Category) = item;
	}

	// Token: 0x06005DC7 RID: 24007 RVA: 0x00225082 File Offset: 0x00223282
	public unsafe Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return *this.slots.GetItemSlotForCategory(category);
	}

	// Token: 0x06005DC8 RID: 24008 RVA: 0x00225098 File Offset: 0x00223298
	public unsafe void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, string.Format("Tried to set clothing item with outfit type \"{0}\" to outfit of type \"{1}\"", item.Unwrap().outfitType, this.outfitType), null);
			DebugUtil.DevAssert(item.Unwrap().Category == category, string.Format("Tried to set clothing item with category \"{0}\" to slot with type \"{1}\"", item.Unwrap().Category, category), null);
		}
		*this.slots.GetItemSlotForCategory(category) = item;
	}

	// Token: 0x06005DC9 RID: 24009 RVA: 0x00225138 File Offset: 0x00223338
	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				clothingItems.Add(ptr.Unwrap());
			}
		}
	}

	// Token: 0x06005DCA RID: 24010 RVA: 0x00225184 File Offset: 0x00223384
	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				itemIds.Add(ptr.Unwrap().Id);
			}
		}
	}

	// Token: 0x06005DCB RID: 24011 RVA: 0x002251D4 File Offset: 0x002233D4
	public string[] GetItems()
	{
		List<string> list = new List<string>();
		this.AddItemsTo(list);
		return list.ToArray();
	}

	// Token: 0x06005DCC RID: 24012 RVA: 0x002251F4 File Offset: 0x002233F4
	public bool DoesContainNonOwnedItems()
	{
		bool result;
		using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList pooledList = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
		{
			this.AddItemsTo(pooledList);
			result = ClothingOutfitTarget.DoesContainNonOwnedItems(pooledList);
		}
		return result;
	}

	// Token: 0x06005DCD RID: 24013 RVA: 0x00225234 File Offset: 0x00223434
	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			this.AddItemsTo(pooledHashSet);
			string[] array = this.destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			foreach (string item in array)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04003F24 RID: 16164
	public string name;

	// Token: 0x04003F25 RID: 16165
	private OutfitDesignerScreen_OutfitState.Slots slots;

	// Token: 0x04003F26 RID: 16166
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x04003F27 RID: 16167
	public ClothingOutfitTarget sourceTarget;

	// Token: 0x04003F28 RID: 16168
	public ClothingOutfitTarget destinationTarget;

	// Token: 0x02001AF6 RID: 6902
	public abstract class Slots
	{
		// Token: 0x060098A8 RID: 39080 RVA: 0x0034288C File Offset: 0x00340A8C
		private Slots(int slotsCount)
		{
			this.array = new Option<ClothingItemResource>[slotsCount];
		}

		// Token: 0x060098A9 RID: 39081 RVA: 0x003428A0 File Offset: 0x00340AA0
		public static OutfitDesignerScreen_OutfitState.Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				return new OutfitDesignerScreen_OutfitState.Slots.Clothing();
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060098AA RID: 39082
		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		// Token: 0x060098AB RID: 39083 RVA: 0x003428D4 File Offset: 0x00340AD4
		private ref Option<ClothingItemResource> FallbackSlot(OutfitDesignerScreen_OutfitState.Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", new object[]
			{
				"Option",
				"ClothingItemResource",
				"PermitCategory",
				category,
				"Slots",
				self.GetType().Name
			}), null);
			return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
		}

		// Token: 0x04007B2F RID: 31535
		public Option<ClothingItemResource>[] array;

		// Token: 0x04007B30 RID: 31536
		private static Option<ClothingItemResource> dummySlot;

		// Token: 0x02002235 RID: 8757
		public class Clothing : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600AD20 RID: 44320 RVA: 0x003790B3 File Offset: 0x003772B3
			public Clothing() : base(6)
			{
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x0600AD21 RID: 44321 RVA: 0x003790BC File Offset: 0x003772BC
			public ref Option<ClothingItemResource> hatSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x0600AD22 RID: 44322 RVA: 0x003790CA File Offset: 0x003772CA
			public ref Option<ClothingItemResource> topSlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000A85 RID: 2693
			// (get) Token: 0x0600AD23 RID: 44323 RVA: 0x003790D8 File Offset: 0x003772D8
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000A86 RID: 2694
			// (get) Token: 0x0600AD24 RID: 44324 RVA: 0x003790E6 File Offset: 0x003772E6
			public ref Option<ClothingItemResource> bottomSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000A87 RID: 2695
			// (get) Token: 0x0600AD25 RID: 44325 RVA: 0x003790F4 File Offset: 0x003772F4
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x17000A88 RID: 2696
			// (get) Token: 0x0600AD26 RID: 44326 RVA: 0x00379102 File Offset: 0x00377302
			public ref Option<ClothingItemResource> accessorySlot
			{
				get
				{
					return ref this.array[5];
				}
			}

			// Token: 0x0600AD27 RID: 44327 RVA: 0x00379110 File Offset: 0x00377310
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.DupeHats)
				{
					return this.hatSlot;
				}
				if (category == PermitCategory.DupeTops)
				{
					return this.topSlot;
				}
				if (category == PermitCategory.DupeGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.DupeBottoms)
				{
					return this.bottomSlot;
				}
				if (category == PermitCategory.DupeShoes)
				{
					return this.shoesSlot;
				}
				if (category == PermitCategory.DupeAccessories)
				{
					return this.accessorySlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x02002236 RID: 8758
		public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600AD28 RID: 44328 RVA: 0x00379167 File Offset: 0x00377367
			public Atmosuit() : base(5)
			{
			}

			// Token: 0x17000A89 RID: 2697
			// (get) Token: 0x0600AD29 RID: 44329 RVA: 0x00379170 File Offset: 0x00377370
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000A8A RID: 2698
			// (get) Token: 0x0600AD2A RID: 44330 RVA: 0x0037917E File Offset: 0x0037737E
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000A8B RID: 2699
			// (get) Token: 0x0600AD2B RID: 44331 RVA: 0x0037918C File Offset: 0x0037738C
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000A8C RID: 2700
			// (get) Token: 0x0600AD2C RID: 44332 RVA: 0x0037919A File Offset: 0x0037739A
			public ref Option<ClothingItemResource> beltSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000A8D RID: 2701
			// (get) Token: 0x0600AD2D RID: 44333 RVA: 0x003791A8 File Offset: 0x003773A8
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x0600AD2E RID: 44334 RVA: 0x003791B8 File Offset: 0x003773B8
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.AtmoSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.AtmoSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.AtmoSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.AtmoSuitBelt)
				{
					return this.beltSlot;
				}
				if (category == PermitCategory.AtmoSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}
	}
}
