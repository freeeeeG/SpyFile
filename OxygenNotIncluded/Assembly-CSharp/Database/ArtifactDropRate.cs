using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000CE4 RID: 3300
	public class ArtifactDropRate : Resource
	{
		// Token: 0x06006934 RID: 26932 RVA: 0x0027C317 File Offset: 0x0027A517
		public void AddItem(ArtifactTier tier, float weight)
		{
			this.rates.Add(new global::Tuple<ArtifactTier, float>(tier, weight));
			this.totalWeight += weight;
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x0027C33C File Offset: 0x0027A53C
		public float GetTierWeight(ArtifactTier tier)
		{
			float result = 0f;
			foreach (global::Tuple<ArtifactTier, float> tuple in this.rates)
			{
				if (tuple.first == tier)
				{
					result = tuple.second;
				}
			}
			return result;
		}

		// Token: 0x040048A1 RID: 18593
		public List<global::Tuple<ArtifactTier, float>> rates = new List<global::Tuple<ArtifactTier, float>>();

		// Token: 0x040048A2 RID: 18594
		public float totalWeight;
	}
}
