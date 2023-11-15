using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000D68 RID: 3432
	public class SkillAttributePerk : SkillPerk
	{
		// Token: 0x06006B36 RID: 27446 RVA: 0x0029CE28 File Offset: 0x0029B028
		public SkillAttributePerk(string id, string attributeId, float modifierBonus, string modifierDesc) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, false)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
			this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc, false, false, true);
			this.Name = string.Format(UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, this.modifier.GetFormattedString(), attribute.Name);
			base.OnApply = delegate(MinionResume identity)
			{
				if (identity.GetAttributes().Get(this.modifier.AttributeId).Modifiers.FindIndex((AttributeModifier mod) => mod == this.modifier) == -1)
				{
					identity.GetAttributes().Add(this.modifier);
				}
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				identity.GetAttributes().Remove(this.modifier);
			};
		}

		// Token: 0x04004E09 RID: 19977
		public AttributeModifier modifier;
	}
}
