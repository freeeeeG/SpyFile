using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD7 RID: 3543
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeLevels")]
	public class AttributeLevels : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x06006D15 RID: 27925 RVA: 0x002AF7F8 File Offset: 0x002AD9F8
		public IEnumerator<AttributeLevel> GetEnumerator()
		{
			return this.levels.GetEnumerator();
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06006D16 RID: 27926 RVA: 0x002AF80A File Offset: 0x002ADA0A
		// (set) Token: 0x06006D17 RID: 27927 RVA: 0x002AF812 File Offset: 0x002ADA12
		public AttributeLevels.LevelSaveLoad[] SaveLoadLevels
		{
			get
			{
				return this.saveLoadLevels;
			}
			set
			{
				this.saveLoadLevels = value;
			}
		}

		// Token: 0x06006D18 RID: 27928 RVA: 0x002AF81C File Offset: 0x002ADA1C
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				if (attributeInstance.Attribute.IsTrainable)
				{
					AttributeLevel attributeLevel = new AttributeLevel(attributeInstance);
					this.levels.Add(attributeLevel);
					attributeLevel.Apply(this);
				}
			}
		}

		// Token: 0x06006D19 RID: 27929 RVA: 0x002AF88C File Offset: 0x002ADA8C
		[OnSerializing]
		public void OnSerializing()
		{
			this.saveLoadLevels = new AttributeLevels.LevelSaveLoad[this.levels.Count];
			for (int i = 0; i < this.levels.Count; i++)
			{
				this.saveLoadLevels[i].attributeId = this.levels[i].attribute.Attribute.Id;
				this.saveLoadLevels[i].experience = this.levels[i].experience;
				this.saveLoadLevels[i].level = this.levels[i].level;
			}
		}

		// Token: 0x06006D1A RID: 27930 RVA: 0x002AF938 File Offset: 0x002ADB38
		[OnDeserialized]
		public void OnDeserialized()
		{
			foreach (AttributeLevels.LevelSaveLoad levelSaveLoad in this.saveLoadLevels)
			{
				this.SetExperience(levelSaveLoad.attributeId, levelSaveLoad.experience);
				this.SetLevel(levelSaveLoad.attributeId, levelSaveLoad.level);
			}
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x002AF988 File Offset: 0x002ADB88
		public int GetLevel(Attribute attribute)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attribute == attributeLevel.attribute.Attribute)
				{
					return attributeLevel.GetLevel();
				}
			}
			return 1;
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x002AF9F0 File Offset: 0x002ADBF0
		public AttributeLevel GetAttributeLevel(string attribute_id)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attributeLevel.attribute.Attribute.Id == attribute_id)
				{
					return attributeLevel;
				}
			}
			return null;
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x002AFA5C File Offset: 0x002ADC5C
		public bool AddExperience(string attribute_id, float time_spent, float multiplier)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel == null)
			{
				global::Debug.LogWarning(attribute_id + " has no level.");
				return false;
			}
			time_spent *= multiplier;
			AttributeConverterInstance attributeConverterInstance = Db.Get().AttributeConverters.TrainingSpeed.Lookup(this);
			if (attributeConverterInstance != null)
			{
				float num = attributeConverterInstance.Evaluate();
				time_spent += time_spent * num;
			}
			bool result = attributeLevel.AddExperience(this, time_spent);
			attributeLevel.Apply(this);
			return result;
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x002AFAC4 File Offset: 0x002ADCC4
		public void SetLevel(string attribute_id, int level)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetLevel(level);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x06006D1F RID: 27935 RVA: 0x002AFAEC File Offset: 0x002ADCEC
		public void SetExperience(string attribute_id, float experience)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetExperience(experience);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x002AFB12 File Offset: 0x002ADD12
		public float GetPercentComplete(string attribute_id)
		{
			return this.GetAttributeLevel(attribute_id).GetPercentComplete();
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x002AFB20 File Offset: 0x002ADD20
		public int GetMaxLevel()
		{
			int num = 0;
			foreach (AttributeLevel attributeLevel in this)
			{
				if (attributeLevel.GetLevel() > num)
				{
					num = attributeLevel.GetLevel();
				}
			}
			return num;
		}

		// Token: 0x040051D9 RID: 20953
		private List<AttributeLevel> levels = new List<AttributeLevel>();

		// Token: 0x040051DA RID: 20954
		[Serialize]
		private AttributeLevels.LevelSaveLoad[] saveLoadLevels = new AttributeLevels.LevelSaveLoad[0];

		// Token: 0x02001F5F RID: 8031
		[Serializable]
		public struct LevelSaveLoad
		{
			// Token: 0x04008DEE RID: 36334
			public string attributeId;

			// Token: 0x04008DEF RID: 36335
			public float experience;

			// Token: 0x04008DF0 RID: 36336
			public int level;
		}
	}
}
