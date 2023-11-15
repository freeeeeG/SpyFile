using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D1F RID: 3359
	public class Spice : Resource
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x0029485B File Offset: 0x00292A5B
		// (set) Token: 0x06006A06 RID: 27142 RVA: 0x00294863 File Offset: 0x00292A63
		public AttributeModifier StatBonus { get; private set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06006A07 RID: 27143 RVA: 0x0029486C File Offset: 0x00292A6C
		// (set) Token: 0x06006A08 RID: 27144 RVA: 0x00294874 File Offset: 0x00292A74
		public AttributeModifier FoodModifier { get; private set; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06006A09 RID: 27145 RVA: 0x0029487D File Offset: 0x00292A7D
		// (set) Token: 0x06006A0A RID: 27146 RVA: 0x00294885 File Offset: 0x00292A85
		public AttributeModifier CalorieModifier { get; private set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06006A0B RID: 27147 RVA: 0x0029488E File Offset: 0x00292A8E
		// (set) Token: 0x06006A0C RID: 27148 RVA: 0x00294896 File Offset: 0x00292A96
		public Color PrimaryColor { get; private set; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06006A0D RID: 27149 RVA: 0x0029489F File Offset: 0x00292A9F
		// (set) Token: 0x06006A0E RID: 27150 RVA: 0x002948A7 File Offset: 0x00292AA7
		public Color SecondaryColor { get; private set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06006A10 RID: 27152 RVA: 0x002948B9 File Offset: 0x00292AB9
		// (set) Token: 0x06006A0F RID: 27151 RVA: 0x002948B0 File Offset: 0x00292AB0
		public string Image { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06006A12 RID: 27154 RVA: 0x002948CA File Offset: 0x00292ACA
		// (set) Token: 0x06006A11 RID: 27153 RVA: 0x002948C1 File Offset: 0x00292AC1
		public string[] DlcIds { get; private set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x06006A13 RID: 27155 RVA: 0x002948D4 File Offset: 0x00292AD4
		public Spice(ResourceSet parent, string id, Spice.Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null) : base(id, parent, null)
		{
			if (dlcID != null)
			{
				this.DlcIds = dlcID;
			}
			this.StatBonus = statBonus;
			this.FoodModifier = foodMod;
			this.Ingredients = ingredients;
			this.Image = imageName;
			this.PrimaryColor = primaryColor;
			this.SecondaryColor = secondaryColor;
			for (int i = 0; i < this.Ingredients.Length; i++)
			{
				this.TotalKG += this.Ingredients[i].AmountKG;
			}
		}

		// Token: 0x04004D17 RID: 19735
		public readonly Spice.Ingredient[] Ingredients;

		// Token: 0x04004D18 RID: 19736
		public readonly float TotalKG;

		// Token: 0x02001C3B RID: 7227
		public class Ingredient : IConfigurableConsumerIngredient
		{
			// Token: 0x06009CC6 RID: 40134 RVA: 0x0034EDC3 File Offset: 0x0034CFC3
			public float GetAmount()
			{
				return this.AmountKG;
			}

			// Token: 0x06009CC7 RID: 40135 RVA: 0x0034EDCB File Offset: 0x0034CFCB
			public Tag[] GetIDSets()
			{
				return this.IngredientSet;
			}

			// Token: 0x04008030 RID: 32816
			public Tag[] IngredientSet;

			// Token: 0x04008031 RID: 32817
			public float AmountKG;
		}
	}
}
