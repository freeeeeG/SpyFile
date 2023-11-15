using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD1 RID: 3537
	public class Attribute : Resource
	{
		// Token: 0x06006CE7 RID: 27879 RVA: 0x002AED48 File Offset: 0x002ACF48
		public Attribute(string id, bool is_trainable, Attribute.Display show_in_ui, bool is_profession, float base_value = 0f, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null, string[] overrideDLCIDs = null) : base(id, null, null)
		{
			string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
			this.Name = Strings.Get(new StringKey(str + ".NAME"));
			this.ProfessionName = Strings.Get(new StringKey(str + ".NAME"));
			this.Description = Strings.Get(new StringKey(str + ".DESC"));
			this.IsTrainable = is_trainable;
			this.IsProfession = is_profession;
			this.ShowInUI = show_in_ui;
			this.BaseValue = base_value;
			this.formatter = Attribute.defaultFormatter;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (overrideDLCIDs != null)
			{
				this.DLCIds = overrideDLCIDs;
			}
		}

		// Token: 0x06006CE8 RID: 27880 RVA: 0x002AEE34 File Offset: 0x002AD034
		public Attribute(string id, string name, string profession_name, string attribute_description, float base_value, Attribute.Display show_in_ui, bool is_trainable, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null) : base(id, name)
		{
			this.Description = attribute_description;
			this.ProfessionName = profession_name;
			this.BaseValue = base_value;
			this.ShowInUI = show_in_ui;
			this.IsTrainable = is_trainable;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (this.ProfessionName == "")
			{
				this.ProfessionName = null;
			}
		}

		// Token: 0x06006CE9 RID: 27881 RVA: 0x002AEEB7 File Offset: 0x002AD0B7
		public void SetFormatter(IAttributeFormatter formatter)
		{
			this.formatter = formatter;
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x002AEEC0 File Offset: 0x002AD0C0
		public AttributeInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06006CEB RID: 27883 RVA: 0x002AEED0 File Offset: 0x002AD0D0
		public AttributeInstance Lookup(GameObject go)
		{
			Attributes attributes = go.GetAttributes();
			if (attributes != null)
			{
				return attributes.Get(this);
			}
			return null;
		}

		// Token: 0x06006CEC RID: 27884 RVA: 0x002AEEF0 File Offset: 0x002AD0F0
		public string GetDescription(AttributeInstance instance)
		{
			return instance.GetDescription();
		}

		// Token: 0x06006CED RID: 27885 RVA: 0x002AEEF8 File Offset: 0x002AD0F8
		public string GetTooltip(AttributeInstance instance)
		{
			return this.formatter.GetTooltip(this, instance);
		}

		// Token: 0x040051BB RID: 20923
		private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);

		// Token: 0x040051BC RID: 20924
		public string Description;

		// Token: 0x040051BD RID: 20925
		public float BaseValue;

		// Token: 0x040051BE RID: 20926
		public Attribute.Display ShowInUI;

		// Token: 0x040051BF RID: 20927
		public bool IsTrainable;

		// Token: 0x040051C0 RID: 20928
		public bool IsProfession;

		// Token: 0x040051C1 RID: 20929
		public string ProfessionName;

		// Token: 0x040051C2 RID: 20930
		public List<AttributeConverter> converters = new List<AttributeConverter>();

		// Token: 0x040051C3 RID: 20931
		public string uiSprite;

		// Token: 0x040051C4 RID: 20932
		public string thoughtSprite;

		// Token: 0x040051C5 RID: 20933
		public string uiFullColourSprite;

		// Token: 0x040051C6 RID: 20934
		public string[] DLCIds = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x040051C7 RID: 20935
		public IAttributeFormatter formatter;

		// Token: 0x02001F5E RID: 8030
		public enum Display
		{
			// Token: 0x04008DE8 RID: 36328
			Normal,
			// Token: 0x04008DE9 RID: 36329
			Skill,
			// Token: 0x04008DEA RID: 36330
			Expectation,
			// Token: 0x04008DEB RID: 36331
			General,
			// Token: 0x04008DEC RID: 36332
			Details,
			// Token: 0x04008DED RID: 36333
			Never
		}
	}
}
