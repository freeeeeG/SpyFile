using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D1C RID: 3356
	public class Sicknesses : ResourceSet<Sickness>
	{
		// Token: 0x060069FA RID: 27130 RVA: 0x00293794 File Offset: 0x00291994
		public Sicknesses(ResourceSet parent) : base("Sicknesses", parent)
		{
			this.FoodSickness = base.Add(new FoodSickness());
			this.SlimeSickness = base.Add(new SlimeSickness());
			this.ZombieSickness = base.Add(new ZombieSickness());
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationSickness = base.Add(new RadiationSickness());
			}
			this.Allergies = base.Add(new Allergies());
			this.ColdBrain = base.Add(new ColdBrain());
			this.HeatRash = base.Add(new HeatRash());
			this.Sunburn = base.Add(new Sunburn());
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x0029383C File Offset: 0x00291A3C
		public static bool IsValidID(string id)
		{
			bool result = false;
			using (List<Sickness>.Enumerator enumerator = Db.Get().Sicknesses.resources.GetEnumerator())
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

		// Token: 0x04004CE6 RID: 19686
		public Sickness FoodSickness;

		// Token: 0x04004CE7 RID: 19687
		public Sickness SlimeSickness;

		// Token: 0x04004CE8 RID: 19688
		public Sickness ZombieSickness;

		// Token: 0x04004CE9 RID: 19689
		public Sickness Allergies;

		// Token: 0x04004CEA RID: 19690
		public Sickness RadiationSickness;

		// Token: 0x04004CEB RID: 19691
		public Sickness ColdBrain;

		// Token: 0x04004CEC RID: 19692
		public Sickness HeatRash;

		// Token: 0x04004CED RID: 19693
		public Sickness Sunburn;
	}
}
