using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000CDC RID: 3292
	public class FertilityModifiers : ResourceSet<FertilityModifier>
	{
		// Token: 0x06006920 RID: 26912 RVA: 0x00279C5C File Offset: 0x00277E5C
		public List<FertilityModifier> GetForTag(Tag searchTag)
		{
			List<FertilityModifier> list = new List<FertilityModifier>();
			foreach (FertilityModifier fertilityModifier in this.resources)
			{
				if (fertilityModifier.TargetTag == searchTag)
				{
					list.Add(fertilityModifier);
				}
			}
			return list;
		}
	}
}
