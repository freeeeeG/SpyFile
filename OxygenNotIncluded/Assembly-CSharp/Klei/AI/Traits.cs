using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E0E RID: 3598
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
	public class Traits : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x06006E59 RID: 28249 RVA: 0x002B6E1E File Offset: 0x002B501E
		public List<string> GetTraitIds()
		{
			return this.TraitIds;
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x002B6E26 File Offset: 0x002B5026
		public void SetTraitIds(List<string> traits)
		{
			this.TraitIds = traits;
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x002B6E30 File Offset: 0x002B5030
		protected override void OnSpawn()
		{
			foreach (string id in this.TraitIds)
			{
				if (Db.Get().traits.Exists(id))
				{
					Trait trait = Db.Get().traits.Get(id);
					this.AddInternal(trait);
				}
			}
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
			{
				List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
				if (base.GetComponent<MinionIdentity>())
				{
					bool flag = true;
					foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
					{
						if (this.HasTrait(traitVal.id))
						{
							flag = false;
						}
					}
					if (flag)
					{
						DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
						Trait trait2 = Db.Get().traits.Get(random.id);
						this.Add(trait2);
					}
				}
			}
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x002B6F50 File Offset: 0x002B5150
		private void AddInternal(Trait trait)
		{
			if (!this.HasTrait(trait))
			{
				this.TraitList.Add(trait);
				trait.AddTo(this.GetAttributes());
				if (trait.OnAddTrait != null)
				{
					trait.OnAddTrait(base.gameObject);
				}
			}
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x002B6F8C File Offset: 0x002B518C
		public void Add(Trait trait)
		{
			DebugUtil.Assert(base.IsInitialized() || base.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, base.gameObject.name);
			if (trait.ShouldSave)
			{
				this.TraitIds.Add(trait.Id);
			}
			this.AddInternal(trait);
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x002B6FEC File Offset: 0x002B51EC
		public bool HasTrait(string trait_id)
		{
			bool result = false;
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == trait_id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x002B704C File Offset: 0x002B524C
		public bool HasTrait(Trait trait)
		{
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == trait)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x002B70A4 File Offset: 0x002B52A4
		public void Clear()
		{
			while (this.TraitList.Count > 0)
			{
				this.Remove(this.TraitList[0]);
			}
		}

		// Token: 0x06006E61 RID: 28257 RVA: 0x002B70C8 File Offset: 0x002B52C8
		public void Remove(Trait trait)
		{
			for (int i = 0; i < this.TraitList.Count; i++)
			{
				if (this.TraitList[i] == trait)
				{
					this.TraitList.RemoveAt(i);
					this.TraitIds.Remove(trait.Id);
					trait.RemoveFrom(this.GetAttributes());
					return;
				}
			}
		}

		// Token: 0x06006E62 RID: 28258 RVA: 0x002B7128 File Offset: 0x002B5328
		public bool IsEffectIgnored(Effect effect)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E63 RID: 28259 RVA: 0x002B7198 File Offset: 0x002B5398
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroup, out trait);
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x002B71AE File Offset: 0x002B53AE
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
		{
			return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x002B71C0 File Offset: 0x002B53C0
		public bool IsChoreGroupDisabled(HashedString choreGroupId)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroupId, out trait);
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x002B71D8 File Offset: 0x002B53D8
		public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == choreGroupId)
						{
							disablingTrait = trait;
							return true;
						}
					}
				}
			}
			disablingTrait = null;
			return false;
		}

		// Token: 0x040052A3 RID: 21155
		public List<Trait> TraitList = new List<Trait>();

		// Token: 0x040052A4 RID: 21156
		[Serialize]
		private List<string> TraitIds = new List<string>();
	}
}
