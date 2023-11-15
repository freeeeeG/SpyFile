using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000CFB RID: 3323
	public class Diseases : ResourceSet<Disease>
	{
		// Token: 0x06006989 RID: 27017 RVA: 0x0028B064 File Offset: 0x00289264
		public Diseases(ResourceSet parent, bool statsOnly = false) : base("Diseases", parent)
		{
			this.FoodGerms = base.Add(new FoodGerms(statsOnly));
			this.SlimeGerms = base.Add(new SlimeGerms(statsOnly));
			this.PollenGerms = base.Add(new PollenGerms(statsOnly));
			this.ZombieSpores = base.Add(new ZombieSpores(statsOnly));
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationPoisoning = base.Add(new RadiationPoisoning(statsOnly));
			}
		}

		// Token: 0x0600698A RID: 27018 RVA: 0x0028B0E0 File Offset: 0x002892E0
		public bool IsValidID(string id)
		{
			bool result = false;
			using (List<Disease>.Enumerator enumerator = this.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x0028B140 File Offset: 0x00289340
		public byte GetIndex(int hash)
		{
			byte b = 0;
			while ((int)b < this.resources.Count)
			{
				Disease disease = this.resources[(int)b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x0028B18C File Offset: 0x0028938C
		public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}

		// Token: 0x04004B13 RID: 19219
		public Disease FoodGerms;

		// Token: 0x04004B14 RID: 19220
		public Disease SlimeGerms;

		// Token: 0x04004B15 RID: 19221
		public Disease PollenGerms;

		// Token: 0x04004B16 RID: 19222
		public Disease ZombieSpores;

		// Token: 0x04004B17 RID: 19223
		public Disease RadiationPoisoning;
	}
}
