using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DEC RID: 3564
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		// Token: 0x06006D8B RID: 28043 RVA: 0x002B379C File Offset: 0x002B199C
		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

		// Token: 0x06006D8C RID: 28044 RVA: 0x002B37AC File Offset: 0x002B19AC
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Add(modifier);
			}
			return null;
		}

		// Token: 0x06006D8D RID: 28045 RVA: 0x002B37E4 File Offset: 0x002B19E4
		public override void OnCure(GameObject go, object instance_data)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Remove(modifier);
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06006D8E RID: 28046 RVA: 0x002B381B File Offset: 0x002B1A1B
		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x002B3824 File Offset: 0x002B1A24
		public override List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x04005225 RID: 21029
		private AttributeModifier[] attributeModifiers;
	}
}
