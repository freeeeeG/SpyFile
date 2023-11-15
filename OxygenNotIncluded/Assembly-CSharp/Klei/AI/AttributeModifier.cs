using System;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02000DD8 RID: 3544
	[DebuggerDisplay("{AttributeId}")]
	public class AttributeModifier
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06006D23 RID: 27939 RVA: 0x002AFB93 File Offset: 0x002ADD93
		// (set) Token: 0x06006D24 RID: 27940 RVA: 0x002AFB9B File Offset: 0x002ADD9B
		public string AttributeId { get; private set; }

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x002AFBA4 File Offset: 0x002ADDA4
		// (set) Token: 0x06006D26 RID: 27942 RVA: 0x002AFBAC File Offset: 0x002ADDAC
		public float Value { get; private set; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06006D27 RID: 27943 RVA: 0x002AFBB5 File Offset: 0x002ADDB5
		// (set) Token: 0x06006D28 RID: 27944 RVA: 0x002AFBBD File Offset: 0x002ADDBD
		public bool IsMultiplier { get; private set; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06006D29 RID: 27945 RVA: 0x002AFBC6 File Offset: 0x002ADDC6
		// (set) Token: 0x06006D2A RID: 27946 RVA: 0x002AFBCE File Offset: 0x002ADDCE
		public bool UIOnly { get; private set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06006D2B RID: 27947 RVA: 0x002AFBD7 File Offset: 0x002ADDD7
		// (set) Token: 0x06006D2C RID: 27948 RVA: 0x002AFBDF File Offset: 0x002ADDDF
		public bool IsReadonly { get; private set; }

		// Token: 0x06006D2D RID: 27949 RVA: 0x002AFBE8 File Offset: 0x002ADDE8
		public AttributeModifier(string attribute_id, float value, string description = null, bool is_multiplier = false, bool uiOnly = false, bool is_readonly = true)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.Description = ((description == null) ? attribute_id : description);
			this.DescriptionCB = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.IsReadonly = is_readonly;
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x002AFC38 File Offset: 0x002ADE38
		public AttributeModifier(string attribute_id, float value, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.DescriptionCB = description_cb;
			this.Description = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			if (description_cb == null)
			{
				global::Debug.LogWarning("AttributeModifier being constructed without a description callback: " + attribute_id);
			}
		}

		// Token: 0x06006D2F RID: 27951 RVA: 0x002AFC8A File Offset: 0x002ADE8A
		public void SetValue(float value)
		{
			this.Value = value;
		}

		// Token: 0x06006D30 RID: 27952 RVA: 0x002AFC94 File Offset: 0x002ADE94
		public string GetName()
		{
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
			{
				return attribute.Name;
			}
			return "";
		}

		// Token: 0x06006D31 RID: 27953 RVA: 0x002AFCCF File Offset: 0x002ADECF
		public string GetDescription()
		{
			if (this.DescriptionCB == null)
			{
				return this.Description;
			}
			return this.DescriptionCB();
		}

		// Token: 0x06006D32 RID: 27954 RVA: 0x002AFCEC File Offset: 0x002ADEEC
		public string GetFormattedString()
		{
			IAttributeFormatter attributeFormatter = null;
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (!this.IsMultiplier)
			{
				if (attribute != null)
				{
					attributeFormatter = attribute.formatter;
				}
				else
				{
					attribute = Db.Get().BuildingAttributes.TryGet(this.AttributeId);
					if (attribute != null)
					{
						attributeFormatter = attribute.formatter;
					}
					else
					{
						attribute = Db.Get().PlantAttributes.TryGet(this.AttributeId);
						if (attribute != null)
						{
							attributeFormatter = attribute.formatter;
						}
					}
				}
			}
			string text = "";
			if (attributeFormatter != null)
			{
				text = attributeFormatter.GetFormattedModifier(this);
			}
			else if (this.IsMultiplier)
			{
				text += GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None);
			}
			else
			{
				text += GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, null);
			}
			if (text != null && text.Length > 0 && text[0] != '-')
			{
				text = GameUtil.AddPositiveSign(text, this.Value > 0f);
			}
			return text;
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x002AFDDD File Offset: 0x002ADFDD
		public AttributeModifier Clone()
		{
			return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
		}

		// Token: 0x040051E0 RID: 20960
		public string Description;

		// Token: 0x040051E1 RID: 20961
		public Func<string> DescriptionCB;
	}
}
