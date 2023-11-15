using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x02000CF0 RID: 3312
	public class BuildingFacadeResource : PermitResource
	{
		// Token: 0x06006962 RID: 26978 RVA: 0x0027F811 File Offset: 0x0027DA11
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : base(Id, Name, Description, PermitCategory.Building, Rarity)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0027F840 File Offset: 0x0027DA40
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, List<FacadeInfo.workable> workables = null) : base(Id, Name, Description, PermitCategory.Building, Rarity)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = new Dictionary<string, string>();
			if (workables != null)
			{
				foreach (FacadeInfo.workable workable in workables)
				{
					this.InteractFile.Add(workable.workableName, workable.workableAnim);
				}
			}
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x0027F8D4 File Offset: 0x0027DAD4
		public void Init()
		{
			GameObject prefab = Assets.GetPrefab(this.PrefabID);
			if (prefab == null)
			{
				global::Debug.LogWarning("Missing prefab id " + this.PrefabID + " for facade " + this.Name);
				return;
			}
			prefab.AddOrGet<BuildingFacade>();
			BuildingDef def = prefab.GetComponent<Building>().Def;
			if (def != null)
			{
				def.AddFacade(this.Id);
			}
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x0027F944 File Offset: 0x0027DB44
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		// Token: 0x0400490C RID: 18700
		public string PrefabID;

		// Token: 0x0400490D RID: 18701
		public string AnimFile;

		// Token: 0x0400490E RID: 18702
		public Dictionary<string, string> InteractFile;
	}
}
