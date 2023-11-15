using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D20 RID: 3360
	public class Spices : ResourceSet<Spice>
	{
		// Token: 0x06006A14 RID: 27156 RVA: 0x00294960 File Offset: 0x00292B60
		public Spices(ResourceSet parent) : base("Spices", parent)
		{
			this.PreservingSpice = new Spice(this, "PRESERVING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"BasicSingleHarvestPlantSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Salt.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.961f, 0.827f, 0.29f), Color.white, new AttributeModifier("RotDelta", 0.5f, "Spices", false, false, true), null, "spice_recipe1", null);
			this.PilotingSpice = new Spice(this, "PILOTING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"MushroomSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Sucrose.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.039f, 0.725f, 0.831f), Color.white, null, new AttributeModifier("SpaceNavigation", 3f, "Spices", false, false, true), "spice_recipe2", DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.StrengthSpice = new Spice(this, "STRENGTH_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"SeaLettuceSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Iron.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.588f, 0.278f, 0.788f), Color.white, null, new AttributeModifier("Strength", 3f, "Spices", false, false, true), "spice_recipe3", null);
			this.MachinerySpice = new Spice(this, "MACHINERY_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"PrickleFlowerSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.SlimeMold.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.788f, 0.443f, 0.792f), Color.white, null, new AttributeModifier("Machinery", 3f, "Spices", false, false, true), "spice_recipe4", null);
		}

		// Token: 0x04004D1B RID: 19739
		public Spice PreservingSpice;

		// Token: 0x04004D1C RID: 19740
		public Spice PilotingSpice;

		// Token: 0x04004D1D RID: 19741
		public Spice StrengthSpice;

		// Token: 0x04004D1E RID: 19742
		public Spice MachinerySpice;
	}
}
