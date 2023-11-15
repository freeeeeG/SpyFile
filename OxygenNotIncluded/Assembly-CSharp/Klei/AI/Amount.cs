using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DCE RID: 3534
	[DebuggerDisplay("{Id}")]
	public class Amount : Resource
	{
		// Token: 0x06006CC7 RID: 27847 RVA: 0x002AE98C File Offset: 0x002ACB8C
		public Amount(string id, string name, string description, Attribute min_attribute, Attribute max_attribute, Attribute delta_attribute, bool show_max, Units units, float visual_delta_threshold, bool show_in_ui, string uiSprite = null, string thoughtSprite = null)
		{
			this.Id = id;
			this.Name = name;
			this.description = description;
			this.minAttribute = min_attribute;
			this.maxAttribute = max_attribute;
			this.deltaAttribute = delta_attribute;
			this.showMax = show_max;
			this.units = units;
			this.visualDeltaThreshold = visual_delta_threshold;
			this.showInUI = show_in_ui;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x002AE9FC File Offset: 0x002ACBFC
		public void SetDisplayer(IAmountDisplayer displayer)
		{
			this.displayer = displayer;
			this.minAttribute.SetFormatter(displayer.Formatter);
			this.maxAttribute.SetFormatter(displayer.Formatter);
			this.deltaAttribute.SetFormatter(displayer.Formatter);
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x002AEA38 File Offset: 0x002ACC38
		public AmountInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06006CCA RID: 27850 RVA: 0x002AEA48 File Offset: 0x002ACC48
		public AmountInstance Lookup(GameObject go)
		{
			Amounts amounts = go.GetAmounts();
			if (amounts != null)
			{
				return amounts.Get(this);
			}
			return null;
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x002AEA68 File Offset: 0x002ACC68
		public void Copy(GameObject to, GameObject from)
		{
			AmountInstance amountInstance = this.Lookup(to);
			AmountInstance amountInstance2 = this.Lookup(from);
			amountInstance.value = amountInstance2.value;
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x002AEA8F File Offset: 0x002ACC8F
		public string GetValueString(AmountInstance instance)
		{
			return this.displayer.GetValueString(this, instance);
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x002AEA9E File Offset: 0x002ACC9E
		public string GetDescription(AmountInstance instance)
		{
			return this.displayer.GetDescription(this, instance);
		}

		// Token: 0x06006CCE RID: 27854 RVA: 0x002AEAAD File Offset: 0x002ACCAD
		public string GetTooltip(AmountInstance instance)
		{
			return this.displayer.GetTooltip(this, instance);
		}

		// Token: 0x040051A7 RID: 20903
		public string description;

		// Token: 0x040051A8 RID: 20904
		public bool showMax;

		// Token: 0x040051A9 RID: 20905
		public Units units;

		// Token: 0x040051AA RID: 20906
		public float visualDeltaThreshold;

		// Token: 0x040051AB RID: 20907
		public Attribute minAttribute;

		// Token: 0x040051AC RID: 20908
		public Attribute maxAttribute;

		// Token: 0x040051AD RID: 20909
		public Attribute deltaAttribute;

		// Token: 0x040051AE RID: 20910
		public bool showInUI;

		// Token: 0x040051AF RID: 20911
		public string uiSprite;

		// Token: 0x040051B0 RID: 20912
		public string thoughtSprite;

		// Token: 0x040051B1 RID: 20913
		public IAmountDisplayer displayer;
	}
}
