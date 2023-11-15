using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF0 RID: 3568
	[DebuggerDisplay("{Id}")]
	public class Effect : Modifier
	{
		// Token: 0x06006D99 RID: 28057 RVA: 0x002B39EC File Offset: 0x002B1BEC
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote = null, float emote_cooldown = -1f, float max_initial_delay = 0f, string stompGroup = null, string custom_icon = "") : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emote = emote;
			this.emoteCooldown = emote_cooldown;
			this.maxInitialDelay = max_initial_delay;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x002B3A4C File Offset: 0x002B1C4C
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, string emoteAnim, float emote_cooldown = -1f, string stompGroup = null, string custom_icon = "") : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emoteAnim = emoteAnim;
			this.emoteCooldown = emote_cooldown;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x002B3AA2 File Offset: 0x002B1CA2
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x002B3AAB File Offset: 0x002B1CAB
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x002B3AB4 File Offset: 0x002B1CB4
		public void SetEmote(Emote emote, float emoteCooldown = -1f)
		{
			this.emote = emote;
			this.emoteCooldown = emoteCooldown;
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x002B3AC4 File Offset: 0x002B1CC4
		public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
		{
			if (this.emotePreconditions == null)
			{
				this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
			}
			this.emotePreconditions.Add(precon);
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x002B3AE8 File Offset: 0x002B1CE8
		public static string CreateTooltip(Effect effect, bool showDuration, string linePrefix = "\n    • ", bool showHeader = true)
		{
			string text = showHeader ? DUPLICANTS.MODIFIERS.EFFECT_HEADER.text : "";
			foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().CritterAttributes.TryGet(attributeModifier.AttributeId);
				}
				if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
				{
					text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
				}
			}
			StringEntry entry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".ADDITIONAL_EFFECTS", out entry))
			{
				text = text + linePrefix + entry;
			}
			if (showDuration && effect.duration > 0f)
			{
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
			}
			return text;
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x002B3C1C File Offset: 0x002B1E1C
		public static string CreateFullTooltip(Effect effect, bool showDuration)
		{
			return string.Concat(new string[]
			{
				effect.Name,
				"\n\n",
				effect.description,
				"\n\n",
				Effect.CreateTooltip(effect, showDuration, "\n    • ", true)
			});
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x002B3C5B File Offset: 0x002B1E5B
		public static void AddModifierDescriptions(GameObject parent, List<Descriptor> descs, string effect_id, bool increase_indent = false)
		{
			Effect.AddModifierDescriptions(descs, effect_id, increase_indent, "STRINGS.DUPLICANTS.ATTRIBUTES.");
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x002B3C6C File Offset: 0x002B1E6C
		public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool increase_indent = false, string prefix = "STRINGS.DUPLICANTS.ATTRIBUTES.")
		{
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
			{
				Descriptor item = new Descriptor(Strings.Get(prefix + attributeModifier.AttributeId.ToUpper() + ".NAME") + ": " + attributeModifier.GetFormattedString(), "", Descriptor.DescriptorType.Effect, false);
				if (increase_indent)
				{
					item.IncreaseIndent();
				}
				descs.Add(item);
			}
		}

		// Token: 0x0400522A RID: 21034
		public float duration;

		// Token: 0x0400522B RID: 21035
		public bool showInUI;

		// Token: 0x0400522C RID: 21036
		public bool triggerFloatingText;

		// Token: 0x0400522D RID: 21037
		public bool isBad;

		// Token: 0x0400522E RID: 21038
		public string customIcon;

		// Token: 0x0400522F RID: 21039
		public string emoteAnim;

		// Token: 0x04005230 RID: 21040
		public Emote emote;

		// Token: 0x04005231 RID: 21041
		public float emoteCooldown;

		// Token: 0x04005232 RID: 21042
		public float maxInitialDelay;

		// Token: 0x04005233 RID: 21043
		public List<Reactable.ReactablePrecondition> emotePreconditions;

		// Token: 0x04005234 RID: 21044
		public string stompGroup;

		// Token: 0x04005235 RID: 21045
		public int stompPriority;
	}
}
