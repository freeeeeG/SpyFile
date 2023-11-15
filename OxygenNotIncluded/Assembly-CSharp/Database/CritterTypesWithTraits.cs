using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000D41 RID: 3393
	public class CritterTypesWithTraits : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AA1 RID: 27297 RVA: 0x002991F8 File Offset: 0x002973F8
		public CritterTypesWithTraits(List<Tag> critterTypes)
		{
			foreach (Tag key in critterTypes)
			{
				if (!this.critterTypesToCheck.ContainsKey(key))
				{
					this.critterTypesToCheck.Add(key, false);
				}
			}
			this.hasTrait = false;
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x00299288 File Offset: 0x00297488
		public override bool Success()
		{
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().tamedCritterTypes;
			bool flag = true;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				flag = (flag && tamedCritterTypes.Contains(keyValuePair.Key));
			}
			this.UpdateSavedState();
			return flag;
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x00299304 File Offset: 0x00297504
		public void UpdateSavedState()
		{
			this.revisedCritterTypesToCheckState.Clear();
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().tamedCritterTypes;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				this.revisedCritterTypesToCheckState.Add(keyValuePair.Key, tamedCritterTypes.Contains(keyValuePair.Key));
			}
			foreach (KeyValuePair<Tag, bool> keyValuePair2 in this.revisedCritterTypesToCheckState)
			{
				this.critterTypesToCheck[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x002993E0 File Offset: 0x002975E0
		public void Deserialize(IReader reader)
		{
			this.critterTypesToCheck = new Dictionary<Tag, bool>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				bool value = reader.ReadByte() > 0;
				this.critterTypesToCheck.Add(new Tag(name), value);
			}
			this.hasTrait = (reader.ReadByte() > 0);
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x04004DA4 RID: 19876
		public Dictionary<Tag, bool> critterTypesToCheck = new Dictionary<Tag, bool>();

		// Token: 0x04004DA5 RID: 19877
		private Tag trait;

		// Token: 0x04004DA6 RID: 19878
		private bool hasTrait;

		// Token: 0x04004DA7 RID: 19879
		private Dictionary<Tag, bool> revisedCritterTypesToCheckState = new Dictionary<Tag, bool>();
	}
}
