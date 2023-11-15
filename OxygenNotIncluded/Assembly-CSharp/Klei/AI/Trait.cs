using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E0B RID: 3595
	public class Trait : Modifier
	{
		// Token: 0x06006E44 RID: 28228 RVA: 0x002B67FA File Offset: 0x002B49FA
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x002B683C File Offset: 0x002B4A3C
		public void AddIgnoredEffects(string[] effects)
		{
			List<string> list = new List<string>(this.ignoredEffects);
			list.AddRange(effects);
			this.ignoredEffects = list.ToArray();
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x002B6868 File Offset: 0x002B4A68
		public string GetTooltip()
		{
			string text;
			if (this.TooltipCB != null)
			{
				text = this.TooltipCB();
			}
			else
			{
				text = this.description;
				text += this.GetAttributeModifiersString(true);
				text += this.GetDisabledChoresString(true);
				text += this.GetIgnoredEffectsString(true);
				text += this.GetExtendedTooltipStr();
			}
			return text;
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x002B68CC File Offset: 0x002B4ACC
		public string GetAttributeModifiersString(bool list_entry)
		{
			string text = "";
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString());
			}
			return text;
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x002B696C File Offset: 0x002B4B6C
		public string GetDisabledChoresString(bool list_entry)
		{
			string text = "";
			if (this.disabledChoreGroups != null)
			{
				string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
				if (this.isTaskBeingRefused)
				{
					format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
				}
				foreach (ChoreGroup choreGroup in this.disabledChoreGroups)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					text += string.Format(format, choreGroup.Name);
				}
			}
			return text;
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x002B69E8 File Offset: 0x002B4BE8
		public string GetIgnoredEffectsString(bool list_entry)
		{
			string text = "";
			if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
			{
				foreach (string text2 in this.ignoredEffects)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
					text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
				}
			}
			return text;
		}

		// Token: 0x06006E4A RID: 28234 RVA: 0x002B6A74 File Offset: 0x002B4C74
		public string GetExtendedTooltipStr()
		{
			string text = "";
			if (this.ExtendedTooltip != null)
			{
				foreach (Func<string> func in this.ExtendedTooltip.GetInvocationList())
				{
					text = text + "\n" + func();
				}
			}
			return text;
		}

		// Token: 0x06006E4B RID: 28235 RVA: 0x002B6AC8 File Offset: 0x002B4CC8
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, false);
				}
			}
		}

		// Token: 0x06006E4C RID: 28236 RVA: 0x002B6B1C File Offset: 0x002B4D1C
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, true);
				}
			}
		}

		// Token: 0x04005298 RID: 21144
		public float Rating;

		// Token: 0x04005299 RID: 21145
		public bool ShouldSave;

		// Token: 0x0400529A RID: 21146
		public bool PositiveTrait;

		// Token: 0x0400529B RID: 21147
		public bool ValidStarterTrait;

		// Token: 0x0400529C RID: 21148
		public Action<GameObject> OnAddTrait;

		// Token: 0x0400529D RID: 21149
		public Func<string> TooltipCB;

		// Token: 0x0400529E RID: 21150
		public Func<string> ExtendedTooltip;

		// Token: 0x0400529F RID: 21151
		public ChoreGroup[] disabledChoreGroups;

		// Token: 0x040052A0 RID: 21152
		public bool isTaskBeingRefused;

		// Token: 0x040052A1 RID: 21153
		public string[] ignoredEffects;
	}
}
