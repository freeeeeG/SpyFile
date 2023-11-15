using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000FC32F File Offset: 0x000FA52F
	public string OutfitId
	{
		get
		{
			return this.impl.OutfitId;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000FC33C File Offset: 0x000FA53C
	public ClothingOutfitUtility.OutfitType OutfitType
	{
		get
		{
			return this.impl.OutfitType;
		}
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x000FC349 File Offset: 0x000FA549
	public string[] ReadItems()
	{
		return this.impl.ReadItems(this.OutfitType).Where(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x000FC372 File Offset: 0x000FA572
	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		this.impl.WriteItems(outfitType, items);
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000FC381 File Offset: 0x000FA581
	public bool CanWriteItems
	{
		get
		{
			return this.impl.CanWriteItems;
		}
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000FC38E File Offset: 0x000FA58E
	public string ReadName()
	{
		return this.impl.ReadName();
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x000FC39B File Offset: 0x000FA59B
	public void WriteName(string name)
	{
		this.impl.WriteName(name);
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06002FBB RID: 12219 RVA: 0x000FC3A9 File Offset: 0x000FA5A9
	public bool CanWriteName
	{
		get
		{
			return this.impl.CanWriteName;
		}
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x000FC3B6 File Offset: 0x000FA5B6
	public void Delete()
	{
		this.impl.Delete();
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000FC3C3 File Offset: 0x000FA5C3
	public bool CanDelete
	{
		get
		{
			return this.impl.CanDelete;
		}
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x000FC3D0 File Offset: 0x000FA5D0
	public bool DoesExist()
	{
		return this.impl.DoesExist();
	}

	// Token: 0x06002FBF RID: 12223 RVA: 0x000FC3DD File Offset: 0x000FA5DD
	public ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x000FC3E6 File Offset: 0x000FA5E6
	public bool DoesContainNonOwnedItems()
	{
		return ClothingOutfitTarget.DoesContainNonOwnedItems(this.ReadItems());
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x000FC3F4 File Offset: 0x000FA5F4
	public static bool DoesContainNonOwnedItems(IList<string> itemIds)
	{
		foreach (string id in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(id);
			if (permitResource != null && permitResource.IsOwnable() && PermitItems.GetOwnedCount(permitResource) <= 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x000FC464 File Offset: 0x000FA664
	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in this.ReadItems()
		select Db.Get().Permits.ClothingItems.Get(i);
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x000FC490 File Offset: 0x000FA690
	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x000FC4AF File Offset: 0x000FA6AF
	public bool Is<T>() where T : ClothingOutfitTarget.Implementation
	{
		return this.impl is T;
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x000FC4C0 File Offset: 0x000FA6C0
	public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
	{
		ClothingOutfitTarget.Implementation implementation = this.impl;
		if (implementation is T)
		{
			T t = (T)((object)implementation);
			value = t;
			return true;
		}
		value = default(T);
		return false;
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000FC4F4 File Offset: 0x000FA6F4
	public bool IsTemplateOutfit()
	{
		return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000FC506 File Offset: 0x000FA706
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000FC527 File Offset: 0x000FA727
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (ClothingOutfitTarget.DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id));
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000FC558 File Offset: 0x000FA758
	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000FC58B File Offset: 0x000FA78B
	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x000FC5A0 File Offset: 0x000FA7A0
	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x000FC5BC File Offset: 0x000FA7BC
	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		ClothingOutfitUtility.OutfitType outfitType;
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out customTemplateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x000FC655 File Offset: 0x000FA855
	public static bool DoesTemplateExist(string outfitId)
	{
		return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x000FC68A File Offset: 0x000FA88A
	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource outfit in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit));
		}
		List<ClothingOutfitResource>.Enumerator enumerator = default(List<ClothingOutfitResource>.Enumerator);
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> self in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			self.Deconstruct(out text, out customTemplateOutfitEntry);
			string outfitId = text;
			ClothingOutfitUtility.OutfitType outfitType;
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				yield return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
			}
		}
		Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator enumerator2 = default(Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x000FC693 File Offset: 0x000FA893
	public static ClothingOutfitTarget GetRandom()
	{
		return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x000FC6A0 File Offset: 0x000FA8A0
	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in ClothingOutfitTarget.GetAllTemplates()
		where t.OutfitType == onlyOfType
		select t;
		if (enumerable == null || enumerable.Count<ClothingOutfitTarget>() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x000FC6F4 File Offset: 0x000FA8F4
	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string a = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string b = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text;
		if (a != b)
		{
			text = UI.OUTFIT_NAME.RESOLVE_CONFLICT;
		}
		else
		{
			text = "{OutfitName} ({ConflictNumber})";
		}
		for (int i = 1; i < 10000; i++)
		{
			string text2 = text.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!ClothingOutfitTarget.DoesTemplateExist(text2))
			{
				return text2;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x000FC7C2 File Offset: 0x000FA9C2
	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x000FC7CC File Offset: 0x000FA9CC
	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x000FC7DC File Offset: 0x000FA9DC
	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget)
		{
			ClothingOutfitTarget other = (ClothingOutfitTarget)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06002FD5 RID: 12245 RVA: 0x000FC801 File Offset: 0x000FAA01
	public bool Equals(ClothingOutfitTarget other)
	{
		if (this.impl == null || other.impl == null)
		{
			return this.impl == null == (other.impl == null);
		}
		return this.OutfitId == other.OutfitId;
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x000FC83A File Offset: 0x000FAA3A
	public override int GetHashCode()
	{
		return Hash.SDBMLower(this.impl.OutfitId);
	}

	// Token: 0x04001C49 RID: 7241
	public readonly ClothingOutfitTarget.Implementation impl;

	// Token: 0x04001C4A RID: 7242
	public static readonly string[] NO_ITEMS = new string[0];

	// Token: 0x04001C4B RID: 7243
	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	// Token: 0x02001414 RID: 5140
	public interface Implementation
	{
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06008347 RID: 33607
		string OutfitId { get; }

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06008348 RID: 33608
		ClothingOutfitUtility.OutfitType OutfitType { get; }

		// Token: 0x06008349 RID: 33609
		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600834A RID: 33610
		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x0600834B RID: 33611
		bool CanWriteItems { get; }

		// Token: 0x0600834C RID: 33612
		string ReadName();

		// Token: 0x0600834D RID: 33613
		void WriteName(string name);

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600834E RID: 33614
		bool CanWriteName { get; }

		// Token: 0x0600834F RID: 33615
		void Delete();

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06008350 RID: 33616
		bool CanDelete { get; }

		// Token: 0x06008351 RID: 33617
		bool DoesExist();
	}

	// Token: 0x02001415 RID: 5141
	public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06008352 RID: 33618 RVA: 0x002FD3A2 File Offset: 0x002FB5A2
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06008353 RID: 33619 RVA: 0x002FD3A5 File Offset: 0x002FB5A5
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06008354 RID: 33620 RVA: 0x002FD3A8 File Offset: 0x002FB5A8
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008355 RID: 33621 RVA: 0x002FD3AB File Offset: 0x002FB5AB
		public bool DoesExist()
		{
			return !this.minionInstance.IsNullOrDestroyed();
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06008356 RID: 33622 RVA: 0x002FD3BC File Offset: 0x002FB5BC
		public string OutfitId
		{
			get
			{
				return this.minionInstance.GetInstanceID().ToString() + "_outfit";
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06008357 RID: 33623 RVA: 0x002FD3E6 File Offset: 0x002FB5E6
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06008358 RID: 33624 RVA: 0x002FD3EE File Offset: 0x002FB5EE
		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.m_outfitType = outfitType;
			this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06008359 RID: 33625 RVA: 0x002FD40A File Offset: 0x002FB60A
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.accessorizer.GetClothingItemsIds(outfitType);
		}

		// Token: 0x0600835A RID: 33626 RVA: 0x002FD418 File Offset: 0x002FB618
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			this.accessorizer.ClearAllOutfitItems(new ClothingOutfitUtility.OutfitType?(outfitType));
			this.accessorizer.AddCustomClothingOutfit(outfitType, from i in items
			select Db.Get().Permits.ClothingItems.Get(i));
		}

		// Token: 0x0600835B RID: 33627 RVA: 0x002FD467 File Offset: 0x002FB667
		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
		}

		// Token: 0x0600835C RID: 33628 RVA: 0x002FD483 File Offset: 0x002FB683
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		// Token: 0x0600835D RID: 33629 RVA: 0x002FD48F File Offset: 0x002FB68F
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}

		// Token: 0x04006448 RID: 25672
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		// Token: 0x04006449 RID: 25673
		public readonly GameObject minionInstance;

		// Token: 0x0400644A RID: 25674
		public readonly WearableAccessorizer accessorizer;
	}

	// Token: 0x02001416 RID: 5142
	public readonly struct UserAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x0600835E RID: 33630 RVA: 0x002FD49B File Offset: 0x002FB69B
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x0600835F RID: 33631 RVA: 0x002FD49E File Offset: 0x002FB69E
		public bool CanWriteName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06008360 RID: 33632 RVA: 0x002FD4A1 File Offset: 0x002FB6A1
		public bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008361 RID: 33633 RVA: 0x002FD4A4 File Offset: 0x002FB6A4
		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06008362 RID: 33634 RVA: 0x002FD4C0 File Offset: 0x002FB6C0
		public string OutfitId
		{
			get
			{
				return this.m_outfitId[0];
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06008363 RID: 33635 RVA: 0x002FD4CA File Offset: 0x002FB6CA
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x002FD4D2 File Offset: 0x002FB6D2
		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			this.m_outfitId = new string[]
			{
				outfitId
			};
			this.m_outfitType = outfitType;
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x002FD4EC File Offset: 0x002FB6EC
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out customTemplateOutfitEntry))
			{
				ClothingOutfitUtility.OutfitType outfitType2;
				global::Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType2) && outfitType2 == this.m_outfitType);
				return customTemplateOutfitEntry.itemIds;
			}
			return ClothingOutfitTarget.NO_ITEMS;
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x002FD544 File Offset: 0x002FB744
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x002FD558 File Offset: 0x002FB758
		public string ReadName()
		{
			return this.OutfitId;
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x002FD560 File Offset: 0x002FB760
		public void WriteName(string name)
		{
			if (this.OutfitId == name)
			{
				return;
			}
			if (ClothingOutfitTarget.DoesTemplateExist(name))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Can not change outfit name from \"",
					this.OutfitId,
					"\" to \"",
					name,
					"\", \"",
					name,
					"\" already exists"
				}));
			}
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
			{
				CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
			}
			else
			{
				CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
			}
			this.m_outfitId[0] = name;
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x002FD61A File Offset: 0x002FB81A
		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
		}

		// Token: 0x0400644B RID: 25675
		private readonly string[] m_outfitId;

		// Token: 0x0400644C RID: 25676
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}

	// Token: 0x02001417 RID: 5143
	public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x0600836A RID: 33642 RVA: 0x002FD632 File Offset: 0x002FB832
		public bool CanWriteItems
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600836B RID: 33643 RVA: 0x002FD635 File Offset: 0x002FB835
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x0600836C RID: 33644 RVA: 0x002FD638 File Offset: 0x002FB838
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x002FD63B File Offset: 0x002FB83B
		public bool DoesExist()
		{
			return true;
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x0600836E RID: 33646 RVA: 0x002FD63E File Offset: 0x002FB83E
		public string OutfitId
		{
			get
			{
				return this.m_outfitId;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600836F RID: 33647 RVA: 0x002FD646 File Offset: 0x002FB846
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06008370 RID: 33648 RVA: 0x002FD64E File Offset: 0x002FB84E
		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			this.m_outfitId = outfit.Id;
			this.m_outfitType = outfit.outfitType;
			this.resource = outfit;
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x002FD66F File Offset: 0x002FB86F
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.resource.itemsInOutfit;
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x002FD67C File Offset: 0x002FB87C
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x002FD688 File Offset: 0x002FB888
		public string ReadName()
		{
			return this.resource.Name;
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x002FD695 File Offset: 0x002FB895
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		// Token: 0x06008375 RID: 33653 RVA: 0x002FD6A1 File Offset: 0x002FB8A1
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}

		// Token: 0x0400644D RID: 25677
		public readonly ClothingOutfitResource resource;

		// Token: 0x0400644E RID: 25678
		private readonly string m_outfitId;

		// Token: 0x0400644F RID: 25679
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}
}
