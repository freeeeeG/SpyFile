using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD5 RID: 3541
	[DebuggerDisplay("{Attribute.Id}")]
	public class AttributeInstance : ModifierInstance<Attribute>
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06006CFB RID: 27899 RVA: 0x002AF1DF File Offset: 0x002AD3DF
		public string Id
		{
			get
			{
				return this.Attribute.Id;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06006CFC RID: 27900 RVA: 0x002AF1EC File Offset: 0x002AD3EC
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06006CFD RID: 27901 RVA: 0x002AF1F9 File Offset: 0x002AD3F9
		public string Description
		{
			get
			{
				return this.Attribute.Description;
			}
		}

		// Token: 0x06006CFE RID: 27902 RVA: 0x002AF206 File Offset: 0x002AD406
		public float GetBaseValue()
		{
			return this.Attribute.BaseValue;
		}

		// Token: 0x06006CFF RID: 27903 RVA: 0x002AF214 File Offset: 0x002AD414
		public float GetTotalDisplayValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06006D00 RID: 27904 RVA: 0x002AF288 File Offset: 0x002AD488
		public float GetTotalValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06006D01 RID: 27905 RVA: 0x002AF304 File Offset: 0x002AD504
		public static float GetTotalDisplayValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06006D02 RID: 27906 RVA: 0x002AF368 File Offset: 0x002AD568
		public static float GetTotalValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x002AF3D4 File Offset: 0x002AD5D4
		public float GetModifierContribution(AttributeModifier testModifier)
		{
			if (!testModifier.IsMultiplier)
			{
				return testModifier.Value;
			}
			float num = this.Attribute.BaseValue;
			for (int num2 = 0; num2 != this.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num2];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
			}
			return num * testModifier.Value;
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x002AF438 File Offset: 0x002AD638
		public AttributeInstance(GameObject game_object, Attribute attribute) : base(game_object, attribute)
		{
			DebugUtil.Assert(attribute != null);
			this.Attribute = attribute;
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x002AF452 File Offset: 0x002AD652
		public void Add(AttributeModifier modifier)
		{
			this.Modifiers.Add(modifier);
			if (this.OnDirty != null)
			{
				this.OnDirty();
			}
		}

		// Token: 0x06006D06 RID: 27910 RVA: 0x002AF474 File Offset: 0x002AD674
		public void Remove(AttributeModifier modifier)
		{
			int i = 0;
			while (i < this.Modifiers.Count)
			{
				if (this.Modifiers[i] == modifier)
				{
					this.Modifiers.RemoveAt(i);
					if (this.OnDirty != null)
					{
						this.OnDirty();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06006D07 RID: 27911 RVA: 0x002AF4C6 File Offset: 0x002AD6C6
		public void ClearModifiers()
		{
			if (this.Modifiers.Count > 0)
			{
				this.Modifiers.Clear();
				if (this.OnDirty != null)
				{
					this.OnDirty();
				}
			}
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x002AF4F4 File Offset: 0x002AD6F4
		public string GetDescription()
		{
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, this.Name, this.GetFormattedValue());
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x002AF511 File Offset: 0x002AD711
		public string GetFormattedValue()
		{
			return this.Attribute.formatter.GetFormattedAttribute(this);
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x002AF524 File Offset: 0x002AD724
		public string GetAttributeValueTooltip()
		{
			return this.Attribute.GetTooltip(this);
		}

		// Token: 0x040051D0 RID: 20944
		public Attribute Attribute;

		// Token: 0x040051D1 RID: 20945
		public System.Action OnDirty;

		// Token: 0x040051D2 RID: 20946
		public ArrayRef<AttributeModifier> Modifiers;

		// Token: 0x040051D3 RID: 20947
		public bool hide;
	}
}
