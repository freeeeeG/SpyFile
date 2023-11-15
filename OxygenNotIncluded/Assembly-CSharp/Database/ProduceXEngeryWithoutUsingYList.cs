using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000D42 RID: 3394
	public class ProduceXEngeryWithoutUsingYList : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AA5 RID: 27301 RVA: 0x00299448 File Offset: 0x00297648
		public ProduceXEngeryWithoutUsingYList(float amountToProduce, List<Tag> disallowedBuildings)
		{
			this.disallowedBuildings = disallowedBuildings;
			this.amountToProduce = amountToProduce;
			this.usedDisallowedBuilding = false;
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x00299470 File Offset: 0x00297670
		public override bool Success()
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num / 1000f > this.amountToProduce;
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x002994F8 File Offset: 0x002976F8
		public override bool Fail()
		{
			foreach (Tag key in this.disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x00299564 File Offset: 0x00297764
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.disallowedBuildings = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.disallowedBuildings.Add(new Tag(name));
			}
			this.amountProduced = (float)reader.ReadDouble();
			this.amountToProduce = (float)reader.ReadDouble();
			this.usedDisallowedBuilding = (reader.ReadByte() > 0);
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x002995D4 File Offset: 0x002977D4
		public float GetProductionAmount(bool complete)
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			if (!complete)
			{
				return num;
			}
			return this.amountToProduce;
		}

		// Token: 0x04004DA8 RID: 19880
		public List<Tag> disallowedBuildings = new List<Tag>();

		// Token: 0x04004DA9 RID: 19881
		public float amountToProduce;

		// Token: 0x04004DAA RID: 19882
		private float amountProduced;

		// Token: 0x04004DAB RID: 19883
		private bool usedDisallowedBuilding;
	}
}
