using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E05 RID: 3589
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
	public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
	{
		// Token: 0x06006E1C RID: 28188 RVA: 0x002B5F04 File Offset: 0x002B4104
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.amounts = new Amounts(base.gameObject);
			this.sicknesses = new Sicknesses(base.gameObject);
			this.attributes = new Attributes(base.gameObject);
			foreach (string id in this.initialAmounts)
			{
				this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(id), base.gameObject));
			}
			foreach (string text in this.initialAttributes)
			{
				Attribute attribute = Db.Get().CritterAttributes.TryGet(text);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.TryGet(text);
				}
				if (attribute == null)
				{
					attribute = Db.Get().Attributes.TryGet(text);
				}
				DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", text);
				this.attributes.Add(attribute);
			}
			Traits component = base.GetComponent<Traits>();
			if (this.initialTraits != null)
			{
				foreach (string id2 in this.initialTraits)
				{
					Trait trait = Db.Get().traits.Get(id2);
					component.Add(trait);
				}
			}
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x002B60B0 File Offset: 0x002B42B0
		public float GetPreModifiedAttributeValue(Attribute attribute)
		{
			return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x002B60C0 File Offset: 0x002B42C0
		public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x002B60F4 File Offset: 0x002B42F4
		public string GetPreModifiedAttributeDescription(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, attribute.Name, attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x002B6131 File Offset: 0x002B4331
		public string GetPreModifiedAttributeToolTip(Attribute attribute)
		{
			return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), null);
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x002B6148 File Offset: 0x002B4348
		public List<AttributeModifier> GetPreModifiers(Attribute attribute)
		{
			List<AttributeModifier> list = new List<AttributeModifier>();
			foreach (string id in this.initialTraits)
			{
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(id).SelfModifiers)
				{
					if (attributeModifier.AttributeId == attribute.Id)
					{
						list.Add(attributeModifier);
					}
				}
			}
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null && component.MutationIDs != null)
			{
				foreach (string id2 in component.MutationIDs)
				{
					foreach (AttributeModifier attributeModifier2 in Db.Get().PlantMutations.Get(id2).SelfModifiers)
					{
						if (attributeModifier2.AttributeId == attribute.Id)
						{
							list.Add(attributeModifier2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06006E22 RID: 28194 RVA: 0x002B62C8 File Offset: 0x002B44C8
		public void Serialize(BinaryWriter writer)
		{
			this.OnSerialize(writer);
		}

		// Token: 0x06006E23 RID: 28195 RVA: 0x002B62D1 File Offset: 0x002B44D1
		public void Deserialize(IReader reader)
		{
			this.OnDeserialize(reader);
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x002B62DA File Offset: 0x002B44DA
		public virtual void OnSerialize(BinaryWriter writer)
		{
			this.amounts.Serialize(writer);
			this.sicknesses.Serialize(writer);
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x002B62F4 File Offset: 0x002B44F4
		public virtual void OnDeserialize(IReader reader)
		{
			this.amounts.Deserialize(reader);
			this.sicknesses.Deserialize(reader);
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x002B630E File Offset: 0x002B450E
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.amounts != null)
			{
				this.amounts.Cleanup();
			}
		}

		// Token: 0x0400528A RID: 21130
		public Amounts amounts;

		// Token: 0x0400528B RID: 21131
		public Attributes attributes;

		// Token: 0x0400528C RID: 21132
		public Sicknesses sicknesses;

		// Token: 0x0400528D RID: 21133
		public List<string> initialTraits = new List<string>();

		// Token: 0x0400528E RID: 21134
		public List<string> initialAmounts = new List<string>();

		// Token: 0x0400528F RID: 21135
		public List<string> initialAttributes = new List<string>();
	}
}
