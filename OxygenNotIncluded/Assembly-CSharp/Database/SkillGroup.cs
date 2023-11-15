using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D6B RID: 3435
	public class SkillGroup : Resource, IListableOption
	{
		// Token: 0x06006B3C RID: 27452 RVA: 0x0029DCDE File Offset: 0x0029BEDE
		string IListableOption.GetProperName()
		{
			return Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x0029DD04 File Offset: 0x0029BF04
		public SkillGroup(string id, string choreGroupID, string name, string icon, string archetype_icon) : base(id, name)
		{
			this.choreGroupID = choreGroupID;
			this.choreGroupIcon = icon;
			this.archetypeIcon = archetype_icon;
		}

		// Token: 0x04004E59 RID: 20057
		public string choreGroupID;

		// Token: 0x04004E5A RID: 20058
		public List<Klei.AI.Attribute> relevantAttributes;

		// Token: 0x04004E5B RID: 20059
		public List<string> requiredChoreGroups;

		// Token: 0x04004E5C RID: 20060
		public string choreGroupIcon;

		// Token: 0x04004E5D RID: 20061
		public string archetypeIcon;
	}
}
