using System;

namespace STRINGS
{
	// Token: 0x02000DB4 RID: 3508
	public class ELEMENTS
	{
		// Token: 0x0400512F RID: 20783
		public static LocString ELEMENTDESCSOLID = "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";

		// Token: 0x04005130 RID: 20784
		public static LocString ELEMENTDESCLIQUID = "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";

		// Token: 0x04005131 RID: 20785
		public static LocString ELEMENTDESCGAS = "Resource Type: {0}\nCondensation point: {1}";

		// Token: 0x04005132 RID: 20786
		public static LocString ELEMENTDESCVACUUM = "Resource Type: {0}";

		// Token: 0x04005133 RID: 20787
		public static LocString BREATHABLEDESC = "<color=#{0}>({1})</color>";

		// Token: 0x04005134 RID: 20788
		public static LocString THERMALPROPERTIES = "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";

		// Token: 0x04005135 RID: 20789
		public static LocString RADIATIONPROPERTIES = "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";

		// Token: 0x04005136 RID: 20790
		public static LocString ELEMENTPROPERTIES = "Properties: {0}";

		// Token: 0x02001E13 RID: 7699
		public class STATE
		{
			// Token: 0x04008A02 RID: 35330
			public static LocString SOLID = "Solid";

			// Token: 0x04008A03 RID: 35331
			public static LocString LIQUID = "Liquid";

			// Token: 0x04008A04 RID: 35332
			public static LocString GAS = "Gas";

			// Token: 0x04008A05 RID: 35333
			public static LocString VACUUM = "None";
		}

		// Token: 0x02001E14 RID: 7700
		public class MATERIAL_MODIFIERS
		{
			// Token: 0x04008A06 RID: 35334
			public static LocString EFFECTS_HEADER = "<b>Resource Effects:</b>";

			// Token: 0x04008A07 RID: 35335
			public static LocString DECOR = UI.FormatAsLink("Decor", "DECOR") + ": {0}";

			// Token: 0x04008A08 RID: 35336
			public static LocString OVERHEATTEMPERATURE = UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}";

			// Token: 0x04008A09 RID: 35337
			public static LocString HIGH_THERMAL_CONDUCTIVITY = UI.FormatAsLink("High Thermal Conductivity", "HEAT");

			// Token: 0x04008A0A RID: 35338
			public static LocString LOW_THERMAL_CONDUCTIVITY = UI.FormatAsLink("Insulator", "HEAT");

			// Token: 0x04008A0B RID: 35339
			public static LocString LOW_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Thermally Reactive", "HEAT");

			// Token: 0x04008A0C RID: 35340
			public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Slow Heating", "HEAT");

			// Token: 0x04008A0D RID: 35341
			public static LocString EXCELLENT_RADIATION_SHIELD = UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

			// Token: 0x02002E8A RID: 11914
			public class TOOLTIP
			{
				// Token: 0x0400BEC5 RID: 48837
				public static LocString EFFECTS_HEADER = "Buildings constructed from this material will have these properties";

				// Token: 0x0400BEC6 RID: 48838
				public static LocString DECOR = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;

				// Token: 0x0400BEC7 RID: 48839
				public static LocString OVERHEATTEMPERATURE = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Overheat Temperature" + UI.PST_KEYWORD;

				// Token: 0x0400BEC8 RID: 48840
				public static LocString HIGH_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material disperses ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers quickly through materials with high ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400BEC9 RID: 48841
				public static LocString LOW_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material retains ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers slowly through materials with low ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400BECA RID: 48842
				public static LocString LOW_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Thermally Reactive",
					UI.PST_KEYWORD,
					" materials require little energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool quickly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400BECB RID: 48843
				public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Slow Heating",
					UI.PST_KEYWORD,
					" materials require a large amount of energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool slowly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400BECC RID: 48844
				public static LocString EXCELLENT_RADIATION_SHIELD = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Excellent Radiation Shield",
					UI.PST_KEYWORD,
					" radiation has a hard time passing through materials with a high ",
					UI.PRE_KEYWORD,
					"Radiation Absorption Factor",
					UI.PST_KEYWORD,
					" value. \n\nRadiation Absorption Factor: {1}"
				});
			}
		}

		// Token: 0x02001E15 RID: 7701
		public class HARDNESS
		{
			// Token: 0x04008A0E RID: 35342
			public static LocString NA = "N/A";

			// Token: 0x04008A0F RID: 35343
			public static LocString SOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT + ")";

			// Token: 0x04008A10 RID: 35344
			public static LocString VERYSOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT + ")";

			// Token: 0x04008A11 RID: 35345
			public static LocString FIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM + ")";

			// Token: 0x04008A12 RID: 35346
			public static LocString VERYFIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + ")";

			// Token: 0x04008A13 RID: 35347
			public static LocString NEARLYIMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE + ")";

			// Token: 0x04008A14 RID: 35348
			public static LocString IMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE + ")";

			// Token: 0x02002E8B RID: 11915
			public class HARDNESS_DESCRIPTOR
			{
				// Token: 0x0400BECD RID: 48845
				public static LocString SOFT = "Soft";

				// Token: 0x0400BECE RID: 48846
				public static LocString VERYSOFT = "Very Soft";

				// Token: 0x0400BECF RID: 48847
				public static LocString FIRM = "Firm";

				// Token: 0x0400BED0 RID: 48848
				public static LocString VERYFIRM = "Very Firm";

				// Token: 0x0400BED1 RID: 48849
				public static LocString NEARLYIMPENETRABLE = "Nearly Impenetrable";

				// Token: 0x0400BED2 RID: 48850
				public static LocString IMPENETRABLE = "Impenetrable";
			}
		}

		// Token: 0x02001E16 RID: 7702
		public class AEROGEL
		{
			// Token: 0x04008A15 RID: 35349
			public static LocString NAME = UI.FormatAsLink("Aerogel", "AEROGEL");

			// Token: 0x04008A16 RID: 35350
			public static LocString DESC = "";
		}

		// Token: 0x02001E17 RID: 7703
		public class ALGAE
		{
			// Token: 0x04008A17 RID: 35351
			public static LocString NAME = UI.FormatAsLink("Algae", "ALGAE");

			// Token: 0x04008A18 RID: 35352
			public static LocString DESC = string.Concat(new string[]
			{
				"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce ",
				ELEMENTS.OXYGEN.NAME,
				" when used in an ",
				BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME,
				"."
			});
		}

		// Token: 0x02001E18 RID: 7704
		public class ALUMINUMORE
		{
			// Token: 0x04008A19 RID: 35353
			public static LocString NAME = UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE");

			// Token: 0x04008A1A RID: 35354
			public static LocString DESC = "Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into " + UI.FormatAsLink("Aluminum", "ALUMINUM") + ".";
		}

		// Token: 0x02001E19 RID: 7705
		public class ALUMINUM
		{
			// Token: 0x04008A1B RID: 35355
			public static LocString NAME = UI.FormatAsLink("Aluminum", "ALUMINUM");

			// Token: 0x04008A1C RID: 35356
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				".\n\nIt has high Thermal Conductivity and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E1A RID: 7706
		public class MOLTENALUMINUM
		{
			// Token: 0x04008A1D RID: 35357
			public static LocString NAME = UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM");

			// Token: 0x04008A1E RID: 35358
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Molten Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E1B RID: 7707
		public class ALUMINUMGAS
		{
			// Token: 0x04008A1F RID: 35359
			public static LocString NAME = UI.FormatAsLink("Aluminum Gas", "ALUMINUMGAS");

			// Token: 0x04008A20 RID: 35360
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum Gas is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E1C RID: 7708
		public class BLEACHSTONE
		{
			// Token: 0x04008A21 RID: 35361
			public static LocString NAME = UI.FormatAsLink("Bleach Stone", "BLEACHSTONE");

			// Token: 0x04008A22 RID: 35362
			public static LocString DESC = string.Concat(new string[]
			{
				"Bleach stone is an unstable compound that emits unbreathable ",
				UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Hygienic", "HYGIENE"),
				" processes."
			});
		}

		// Token: 0x02001E1D RID: 7709
		public class BITUMEN
		{
			// Token: 0x04008A23 RID: 35363
			public static LocString NAME = UI.FormatAsLink("Bitumen", "BITUMEN");

			// Token: 0x04008A24 RID: 35364
			public static LocString DESC = "Bitumen is a sticky viscous residue left behind from " + ELEMENTS.PETROLEUM.NAME + " production.";
		}

		// Token: 0x02001E1E RID: 7710
		public class BOTTLEDWATER
		{
			// Token: 0x04008A25 RID: 35365
			public static LocString NAME = UI.FormatAsLink("Water", "BOTTLEDWATER");

			// Token: 0x04008A26 RID: 35366
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + ELEMENTS.WATER.NAME + ", prepped for transport.";
		}

		// Token: 0x02001E1F RID: 7711
		public class BRINEICE
		{
			// Token: 0x04008A27 RID: 35367
			public static LocString NAME = UI.FormatAsLink("Brine Ice", "BRINEICE");

			// Token: 0x04008A28 RID: 35368
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine Ice is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				" and frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02001E20 RID: 7712
		public class MILKICE
		{
			// Token: 0x04008A29 RID: 35369
			public static LocString NAME = UI.FormatAsLink("Frozen Brackene", "MILKICE");

			// Token: 0x04008A2A RID: 35370
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Brackene is ",
				UI.FormatAsLink("Brackene", "MILK"),
				" frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001E21 RID: 7713
		public class BRINE
		{
			// Token: 0x04008A2B RID: 35371
			public static LocString NAME = UI.FormatAsLink("Brine", "BRINE");

			// Token: 0x04008A2C RID: 35372
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02001E22 RID: 7714
		public class CARBON
		{
			// Token: 0x04008A2D RID: 35373
			public static LocString NAME = UI.FormatAsLink("Coal", "CARBON");

			// Token: 0x04008A2E RID: 35374
			public static LocString DESC = "(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x02001E23 RID: 7715
		public class REFINEDCARBON
		{
			// Token: 0x04008A2F RID: 35375
			public static LocString NAME = UI.FormatAsLink("Refined Carbon", "REFINEDCARBON");

			// Token: 0x04008A30 RID: 35376
			public static LocString DESC = "(C) Refined carbon is solid element purified from raw " + ELEMENTS.CARBON.NAME + ".";
		}

		// Token: 0x02001E24 RID: 7716
		public class ETHANOLGAS
		{
			// Token: 0x04008A31 RID: 35377
			public static LocString NAME = UI.FormatAsLink("Ethanol Gas", "ETHANOLGAS");

			// Token: 0x04008A32 RID: 35378
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02001E25 RID: 7717
		public class ETHANOL
		{
			// Token: 0x04008A33 RID: 35379
			public static LocString NAME = UI.FormatAsLink("Ethanol", "ETHANOL");

			// Token: 0x04008A34 RID: 35380
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x02001E26 RID: 7718
		public class SOLIDETHANOL
		{
			// Token: 0x04008A35 RID: 35381
			public static LocString NAME = UI.FormatAsLink("Solid Ethanol", "SOLIDETHANOL");

			// Token: 0x04008A36 RID: 35382
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x02001E27 RID: 7719
		public class CARBONDIOXIDE
		{
			// Token: 0x04008A37 RID: 35383
			public static LocString NAME = UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");

			// Token: 0x04008A38 RID: 35384
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.\n\nIt tends to sink below other gases.";
		}

		// Token: 0x02001E28 RID: 7720
		public class CARBONFIBRE
		{
			// Token: 0x04008A39 RID: 35385
			public static LocString NAME = UI.FormatAsLink("Carbon Fiber", "CARBONFIBRE");

			// Token: 0x04008A3A RID: 35386
			public static LocString DESC = "Carbon Fiber is a " + UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL") + " with high tensile strength.";
		}

		// Token: 0x02001E29 RID: 7721
		public class CARBONGAS
		{
			// Token: 0x04008A3B RID: 35387
			public static LocString NAME = UI.FormatAsLink("Carbon Gas", "CARBONGAS");

			// Token: 0x04008A3C RID: 35388
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02001E2A RID: 7722
		public class CHLORINE
		{
			// Token: 0x04008A3D RID: 35389
			public static LocString NAME = UI.FormatAsLink("Liquid Chlorine", "CHLORINE");

			// Token: 0x04008A3E RID: 35390
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E2B RID: 7723
		public class CHLORINEGAS
		{
			// Token: 0x04008A3F RID: 35391
			public static LocString NAME = UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS");

			// Token: 0x04008A40 RID: 35392
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E2C RID: 7724
		public class CLAY
		{
			// Token: 0x04008A41 RID: 35393
			public static LocString NAME = UI.FormatAsLink("Clay", "CLAY");

			// Token: 0x04008A42 RID: 35394
			public static LocString DESC = "Clay is a soft, naturally occurring composite of stone and soil that hardens at high " + UI.FormatAsLink("Temperatures", "HEAT") + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02001E2D RID: 7725
		public class BRICK
		{
			// Token: 0x04008A43 RID: 35395
			public static LocString NAME = UI.FormatAsLink("Brick", "BRICK");

			// Token: 0x04008A44 RID: 35396
			public static LocString DESC = "Brick is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02001E2E RID: 7726
		public class CERAMIC
		{
			// Token: 0x04008A45 RID: 35397
			public static LocString NAME = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x04008A46 RID: 35398
			public static LocString DESC = "Ceramic is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02001E2F RID: 7727
		public class CEMENT
		{
			// Token: 0x04008A47 RID: 35399
			public static LocString NAME = UI.FormatAsLink("Cement", "CEMENT");

			// Token: 0x04008A48 RID: 35400
			public static LocString DESC = "Cement is a refined building material used for assembling advanced buildings.";
		}

		// Token: 0x02001E30 RID: 7728
		public class CEMENTMIX
		{
			// Token: 0x04008A49 RID: 35401
			public static LocString NAME = UI.FormatAsLink("Cement Mix", "CEMENTMIX");

			// Token: 0x04008A4A RID: 35402
			public static LocString DESC = "Cement Mix can be used to create " + ELEMENTS.CEMENT.NAME + " for advanced building assembly.";
		}

		// Token: 0x02001E31 RID: 7729
		public class CONTAMINATEDOXYGEN
		{
			// Token: 0x04008A4B RID: 35403
			public static LocString NAME = UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN");

			// Token: 0x04008A4C RID: 35404
			public static LocString DESC = "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
		}

		// Token: 0x02001E32 RID: 7730
		public class COPPER
		{
			// Token: 0x04008A4D RID: 35405
			public static LocString NAME = UI.FormatAsLink("Copper", "COPPER");

			// Token: 0x04008A4E RID: 35406
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E33 RID: 7731
		public class COPPERGAS
		{
			// Token: 0x04008A4F RID: 35407
			public static LocString NAME = UI.FormatAsLink("Copper Gas", "COPPERGAS");

			// Token: 0x04008A50 RID: 35408
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E34 RID: 7732
		public class CREATURE
		{
			// Token: 0x04008A51 RID: 35409
			public static LocString NAME = UI.FormatAsLink("Genetic Ooze", "CREATURE");

			// Token: 0x04008A52 RID: 35410
			public static LocString DESC = "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
		}

		// Token: 0x02001E35 RID: 7733
		public class CRUDEOIL
		{
			// Token: 0x04008A53 RID: 35411
			public static LocString NAME = UI.FormatAsLink("Crude Oil", "CRUDEOIL");

			// Token: 0x04008A54 RID: 35412
			public static LocString DESC = "Crude Oil is a raw potential " + UI.FormatAsLink("Power", "POWER") + " source composed of billions of dead, primordial organisms.";
		}

		// Token: 0x02001E36 RID: 7734
		public class PETROLEUM
		{
			// Token: 0x04008A55 RID: 35413
			public static LocString NAME = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x04008A56 RID: 35414
			public static LocString NAME_TWO = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x04008A57 RID: 35415
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source refined from ",
				UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
				".\n\nIt is also an essential ingredient in the production of ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				"."
			});
		}

		// Token: 0x02001E37 RID: 7735
		public class SOURGAS
		{
			// Token: 0x04008A58 RID: 35416
			public static LocString NAME = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x04008A59 RID: 35417
			public static LocString NAME_TWO = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x04008A5A RID: 35418
			public static LocString DESC = string.Concat(new string[]
			{
				"Sour Gas is a hydrocarbon ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated ",
				UI.FormatAsLink("Petroleum", "PETROLEUM"),
				"."
			});
		}

		// Token: 0x02001E38 RID: 7736
		public class CRUSHEDICE
		{
			// Token: 0x04008A5B RID: 35419
			public static LocString NAME = UI.FormatAsLink("Crushed Ice", "CRUSHEDICE");

			// Token: 0x04008A5C RID: 35420
			public static LocString DESC = "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
		}

		// Token: 0x02001E39 RID: 7737
		public class CRUSHEDROCK
		{
			// Token: 0x04008A5D RID: 35421
			public static LocString NAME = UI.FormatAsLink("Crushed Rock", "CRUSHEDROCK");

			// Token: 0x04008A5E RID: 35422
			public static LocString DESC = "Crushed Rock is " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + " crushed into a mechanical mixture.";
		}

		// Token: 0x02001E3A RID: 7738
		public class CUPRITE
		{
			// Token: 0x04008A5F RID: 35423
			public static LocString NAME = UI.FormatAsLink("Copper Ore", "CUPRITE");

			// Token: 0x04008A60 RID: 35424
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu<sub>2</sub>O) Copper Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E3B RID: 7739
		public class DEPLETEDURANIUM
		{
			// Token: 0x04008A61 RID: 35425
			public static LocString NAME = UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM");

			// Token: 0x04008A62 RID: 35426
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Depleted Uranium is ",
				UI.FormatAsLink("Uranium", "URANIUMORE"),
				" with a low U-235 content.\n\nIt is created as a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" and is no longer suitable as fuel."
			});
		}

		// Token: 0x02001E3C RID: 7740
		public class DIAMOND
		{
			// Token: 0x04008A63 RID: 35427
			public static LocString NAME = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x04008A64 RID: 35428
			public static LocString DESC = "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
		}

		// Token: 0x02001E3D RID: 7741
		public class DIRT
		{
			// Token: 0x04008A65 RID: 35429
			public static LocString NAME = UI.FormatAsLink("Dirt", "DIRT");

			// Token: 0x04008A66 RID: 35430
			public static LocString DESC = "Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of " + UI.FormatAsLink("Food", "FOOD") + " production.";
		}

		// Token: 0x02001E3E RID: 7742
		public class DIRTYICE
		{
			// Token: 0x04008A67 RID: 35431
			public static LocString NAME = UI.FormatAsLink("Polluted Ice", "DIRTYICE");

			// Token: 0x04008A68 RID: 35432
			public static LocString DESC = "Polluted Ice is dirty, unfiltered water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E3F RID: 7743
		public class DIRTYWATER
		{
			// Token: 0x04008A69 RID: 35433
			public static LocString NAME = UI.FormatAsLink("Polluted Water", "DIRTYWATER");

			// Token: 0x04008A6A RID: 35434
			public static LocString DESC = "Polluted Water is dirty, unfiltered " + UI.FormatAsLink("Water", "WATER") + ".\n\nIt is not fit for consumption.";
		}

		// Token: 0x02001E40 RID: 7744
		public class ELECTRUM
		{
			// Token: 0x04008A6B RID: 35435
			public static LocString NAME = UI.FormatAsLink("Electrum", "ELECTRUM");

			// Token: 0x04008A6C RID: 35436
			public static LocString DESC = string.Concat(new string[]
			{
				"Electrum is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy composed of gold and silver.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E41 RID: 7745
		public class ENRICHEDURANIUM
		{
			// Token: 0x04008A6D RID: 35437
			public static LocString NAME = UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM");

			// Token: 0x04008A6E RID: 35438
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Enriched Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				", refined substance.\n\nIt is primarily used to ",
				UI.FormatAsLink("Power", "POWER"),
				" potent research reactors."
			});
		}

		// Token: 0x02001E42 RID: 7746
		public class FERTILIZER
		{
			// Token: 0x04008A6F RID: 35439
			public static LocString NAME = UI.FormatAsLink("Fertilizer", "FERTILIZER");

			// Token: 0x04008A70 RID: 35440
			public static LocString DESC = "Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain " + UI.FormatAsLink("Plants", "PLANTS") + ".";
		}

		// Token: 0x02001E43 RID: 7747
		public class PONDSCUM
		{
			// Token: 0x04008A71 RID: 35441
			public static LocString NAME = UI.FormatAsLink("Pondscum", "PONDSCUM");

			// Token: 0x04008A72 RID: 35442
			public static LocString DESC = string.Concat(new string[]
			{
				"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into ",
				UI.FormatAsLink("Fertilizer", "FERTILIZER"),
				" and aids in the growth of certain ",
				UI.FormatAsLink("Plants", "PLANTS"),
				"."
			});
		}

		// Token: 0x02001E44 RID: 7748
		public class FALLOUT
		{
			// Token: 0x04008A73 RID: 35443
			public static LocString NAME = UI.FormatAsLink("Nuclear Fallout", "FALLOUT");

			// Token: 0x04008A74 RID: 35444
			public static LocString DESC = string.Concat(new string[]
			{
				"Nuclear Fallout is a highly toxic gas full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				". Condenses into ",
				UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE"),
				"."
			});
		}

		// Token: 0x02001E45 RID: 7749
		public class FOOLSGOLD
		{
			// Token: 0x04008A75 RID: 35445
			public static LocString NAME = UI.FormatAsLink("Pyrite", "FOOLSGOLD");

			// Token: 0x04008A76 RID: 35446
			public static LocString DESC = string.Concat(new string[]
			{
				"(FeS<sub>2</sub>) Pyrite is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nAlso known as \"Fool's Gold\", is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E46 RID: 7750
		public class FULLERENE
		{
			// Token: 0x04008A77 RID: 35447
			public static LocString NAME = UI.FormatAsLink("Fullerene", "FULLERENE");

			// Token: 0x04008A78 RID: 35448
			public static LocString DESC = "(C<sub>60</sub>) Fullerene is a form of " + UI.FormatAsLink("Coal", "CARBON") + " consisting of spherical molecules.";
		}

		// Token: 0x02001E47 RID: 7751
		public class GLASS
		{
			// Token: 0x04008A79 RID: 35449
			public static LocString NAME = UI.FormatAsLink("Glass", "GLASS");

			// Token: 0x04008A7A RID: 35450
			public static LocString DESC = "Glass is a brittle, transparent substance formed from " + UI.FormatAsLink("Sand", "SAND") + " fired at high temperatures.";
		}

		// Token: 0x02001E48 RID: 7752
		public class GOLD
		{
			// Token: 0x04008A7B RID: 35451
			public static LocString NAME = UI.FormatAsLink("Gold", "GOLD");

			// Token: 0x04008A7C RID: 35452
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E49 RID: 7753
		public class GOLDAMALGAM
		{
			// Token: 0x04008A7D RID: 35453
			public static LocString NAME = UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM");

			// Token: 0x04008A7E RID: 35454
			public static LocString DESC = "Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building " + UI.FormatAsLink("Power", "POWER") + " systems.";
		}

		// Token: 0x02001E4A RID: 7754
		public class GOLDGAS
		{
			// Token: 0x04008A7F RID: 35455
			public static LocString NAME = UI.FormatAsLink("Gold Gas", "GOLDGAS");

			// Token: 0x04008A80 RID: 35456
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold Gas is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E4B RID: 7755
		public class GRANITE
		{
			// Token: 0x04008A81 RID: 35457
			public static LocString NAME = UI.FormatAsLink("Granite", "GRANITE");

			// Token: 0x04008A82 RID: 35458
			public static LocString DESC = "Granite is a dense composite of " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + ".\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E4C RID: 7756
		public class GRAPHITE
		{
			// Token: 0x04008A83 RID: 35459
			public static LocString NAME = UI.FormatAsLink("Graphite", "GRAPHITE");

			// Token: 0x04008A84 RID: 35460
			public static LocString DESC = "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E4D RID: 7757
		public class SOLIDNUCLEARWASTE
		{
			// Token: 0x04008A85 RID: 35461
			public static LocString NAME = UI.FormatAsLink("Solid Nuclear Waste", "SOLIDNUCLEARWASTE");

			// Token: 0x04008A86 RID: 35462
			public static LocString DESC = "Highly toxic solid full of " + UI.FormatAsLink("Radioactive Contaminants", "RADIATION") + ".";
		}

		// Token: 0x02001E4E RID: 7758
		public class HELIUM
		{
			// Token: 0x04008A87 RID: 35463
			public static LocString NAME = UI.FormatAsLink("Helium", "HELIUM");

			// Token: 0x04008A88 RID: 35464
			public static LocString DESC = "(He) Helium is an atomically lightweight, chemical " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
		}

		// Token: 0x02001E4F RID: 7759
		public class HYDROGEN
		{
			// Token: 0x04008A89 RID: 35465
			public static LocString NAME = UI.FormatAsLink("Hydrogen Gas", "HYDROGEN");

			// Token: 0x04008A8A RID: 35466
			public static LocString DESC = "(H) Hydrogen Gas is the universe's most common and atomically light element in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02001E50 RID: 7760
		public class ICE
		{
			// Token: 0x04008A8B RID: 35467
			public static LocString NAME = UI.FormatAsLink("Ice", "ICE");

			// Token: 0x04008A8C RID: 35468
			public static LocString DESC = "(H<sub>2</sub>O) Ice is clean water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E51 RID: 7761
		public class IGNEOUSROCK
		{
			// Token: 0x04008A8D RID: 35469
			public static LocString NAME = UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK");

			// Token: 0x04008A8E RID: 35470
			public static LocString DESC = "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E52 RID: 7762
		public class ISORESIN
		{
			// Token: 0x04008A8F RID: 35471
			public static LocString NAME = UI.FormatAsLink("Isoresin", "ISORESIN");

			// Token: 0x04008A90 RID: 35472
			public static LocString DESC = "Isoresin is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
		}

		// Token: 0x02001E53 RID: 7763
		public class RESIN
		{
			// Token: 0x04008A91 RID: 35473
			public static LocString NAME = UI.FormatAsLink("Liquid Resin", "RESIN");

			// Token: 0x04008A92 RID: 35474
			public static LocString DESC = "Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into " + UI.FormatAsLink("Isoresin", "ISORESIN") + " by boiling away its excess moisture.";
		}

		// Token: 0x02001E54 RID: 7764
		public class SOLIDRESIN
		{
			// Token: 0x04008A93 RID: 35475
			public static LocString NAME = UI.FormatAsLink("Solid Resin", "SOLIDRESIN");

			// Token: 0x04008A94 RID: 35476
			public static LocString DESC = "Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of " + UI.FormatAsLink("Isoresin", "ISORESIN") + ".";
		}

		// Token: 0x02001E55 RID: 7765
		public class IRON
		{
			// Token: 0x04008A95 RID: 35477
			public static LocString NAME = UI.FormatAsLink("Iron", "IRON");

			// Token: 0x04008A96 RID: 35478
			public static LocString DESC = "(Fe) Iron is a common industrial " + UI.FormatAsLink("Metal", "RAWMETAL") + ".";
		}

		// Token: 0x02001E56 RID: 7766
		public class IRONGAS
		{
			// Token: 0x04008A97 RID: 35479
			public static LocString NAME = UI.FormatAsLink("Iron Gas", "IRONGAS");

			// Token: 0x04008A98 RID: 35480
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Gas is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02001E57 RID: 7767
		public class IRONORE
		{
			// Token: 0x04008A99 RID: 35481
			public static LocString NAME = UI.FormatAsLink("Iron Ore", "IRONORE");

			// Token: 0x04008A9A RID: 35482
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Ore is a soft ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E58 RID: 7768
		public class COBALTGAS
		{
			// Token: 0x04008A9B RID: 35483
			public static LocString NAME = UI.FormatAsLink("Cobalt Gas", "COBALTGAS");

			// Token: 0x04008A9C RID: 35484
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02001E59 RID: 7769
		public class COBALT
		{
			// Token: 0x04008A9D RID: 35485
			public static LocString NAME = UI.FormatAsLink("Cobalt", "COBALT");

			// Token: 0x04008A9E RID: 35486
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" made from ",
				UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
				"."
			});
		}

		// Token: 0x02001E5A RID: 7770
		public class COBALTITE
		{
			// Token: 0x04008A9F RID: 35487
			public static LocString NAME = UI.FormatAsLink("Cobalt Ore", "COBALTITE");

			// Token: 0x04008AA0 RID: 35488
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt Ore is a blue-hued ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E5B RID: 7771
		public class KATAIRITE
		{
			// Token: 0x04008AA1 RID: 35489
			public static LocString NAME = UI.FormatAsLink("Abyssalite", "KATAIRITE");

			// Token: 0x04008AA2 RID: 35490
			public static LocString DESC = "(Ab) Abyssalite is a resilient, crystalline element.";
		}

		// Token: 0x02001E5C RID: 7772
		public class LIME
		{
			// Token: 0x04008AA3 RID: 35491
			public static LocString NAME = UI.FormatAsLink("Lime", "LIME");

			// Token: 0x04008AA4 RID: 35492
			public static LocString DESC = "(CaCO<sub>3</sub>) Lime is a mineral commonly found in " + UI.FormatAsLink("Critter", "CREATURES") + " egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E5D RID: 7773
		public class FOSSIL
		{
			// Token: 0x04008AA5 RID: 35493
			public static LocString NAME = UI.FormatAsLink("Fossil", "FOSSIL");

			// Token: 0x04008AA6 RID: 35494
			public static LocString DESC = "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E5E RID: 7774
		public class LEADGAS
		{
			// Token: 0x04008AA7 RID: 35495
			public static LocString NAME = UI.FormatAsLink("Lead Gas", "LEADGAS");

			// Token: 0x04008AA8 RID: 35496
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead Gas is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02001E5F RID: 7775
		public class LEAD
		{
			// Token: 0x04008AA9 RID: 35497
			public static LocString NAME = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x04008AAA RID: 35498
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				".\n\nIt has a low Overheat Temperature and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001E60 RID: 7776
		public class LIQUIDCARBONDIOXIDE
		{
			// Token: 0x04008AAB RID: 35499
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE");

			// Token: 0x04008AAC RID: 35500
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E61 RID: 7777
		public class LIQUIDHELIUM
		{
			// Token: 0x04008AAD RID: 35501
			public static LocString NAME = UI.FormatAsLink("Helium", "LIQUIDHELIUM");

			// Token: 0x04008AAE RID: 35502
			public static LocString DESC = "(He) Helium is an atomically lightweight chemical element cooled into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E62 RID: 7778
		public class LIQUIDHYDROGEN
		{
			// Token: 0x04008AAF RID: 35503
			public static LocString NAME = UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN");

			// Token: 0x04008AB0 RID: 35504
			public static LocString DESC = "(H) Hydrogen in its " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt freezes most substances that come into contact with it.";
		}

		// Token: 0x02001E63 RID: 7779
		public class LIQUIDOXYGEN
		{
			// Token: 0x04008AB1 RID: 35505
			public static LocString NAME = UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN");

			// Token: 0x04008AB2 RID: 35506
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E64 RID: 7780
		public class LIQUIDMETHANE
		{
			// Token: 0x04008AB3 RID: 35507
			public static LocString NAME = UI.FormatAsLink("Liquid Methane", "LIQUIDMETHANE");

			// Token: 0x04008AB4 RID: 35508
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E65 RID: 7781
		public class LIQUIDPHOSPHORUS
		{
			// Token: 0x04008AB5 RID: 35509
			public static LocString NAME = UI.FormatAsLink("Liquid Phosphorus", "LIQUIDPHOSPHORUS");

			// Token: 0x04008AB6 RID: 35510
			public static LocString DESC = "(P) Phosphorus is a chemical element.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E66 RID: 7782
		public class LIQUIDPROPANE
		{
			// Token: 0x04008AB7 RID: 35511
			public static LocString NAME = UI.FormatAsLink("Liquid Propane", "LIQUIDPROPANE");

			// Token: 0x04008AB8 RID: 35512
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02001E67 RID: 7783
		public class LIQUIDSULFUR
		{
			// Token: 0x04008AB9 RID: 35513
			public static LocString NAME = UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR");

			// Token: 0x04008ABA RID: 35514
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E68 RID: 7784
		public class MAFICROCK
		{
			// Token: 0x04008ABB RID: 35515
			public static LocString NAME = UI.FormatAsLink("Mafic Rock", "MAFICROCK");

			// Token: 0x04008ABC RID: 35516
			public static LocString DESC = string.Concat(new string[]
			{
				"Mafic Rock is a variation of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" that is rich in ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful as a <b>Construction Material</b>."
			});
		}

		// Token: 0x02001E69 RID: 7785
		public class MAGMA
		{
			// Token: 0x04008ABD RID: 35517
			public static LocString NAME = UI.FormatAsLink("Magma", "MAGMA");

			// Token: 0x04008ABE RID: 35518
			public static LocString DESC = string.Concat(new string[]
			{
				"Magma is a composite of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" heated into a molten, ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E6A RID: 7786
		public class MERCURY
		{
			// Token: 0x04008ABF RID: 35519
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURY");

			// Token: 0x04008AC0 RID: 35520
			public static LocString DESC = "(Hg) Mercury is a metallic " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".";
		}

		// Token: 0x02001E6B RID: 7787
		public class MERCURYGAS
		{
			// Token: 0x04008AC1 RID: 35521
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURYGAS");

			// Token: 0x04008AC2 RID: 35522
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E6C RID: 7788
		public class METHANE
		{
			// Token: 0x04008AC3 RID: 35523
			public static LocString NAME = UI.FormatAsLink("Natural Gas", "METHANE");

			// Token: 0x04008AC4 RID: 35524
			public static LocString DESC = string.Concat(new string[]
			{
				"Natural Gas is a mixture of various alkanes in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02001E6D RID: 7789
		public class MILK
		{
			// Token: 0x04008AC5 RID: 35525
			public static LocString NAME = UI.FormatAsLink("Brackene", "MILK");

			// Token: 0x04008AC6 RID: 35526
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackene is a sodium-rich ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Ranching", "RANCHING"),
				"."
			});
		}

		// Token: 0x02001E6E RID: 7790
		public class MILKFAT
		{
			// Token: 0x04008AC7 RID: 35527
			public static LocString NAME = UI.FormatAsLink("Brackwax", "MILKFAT");

			// Token: 0x04008AC8 RID: 35528
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackwax is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" byproduct of ",
				UI.FormatAsLink("Brackene", "MILK"),
				"."
			});
		}

		// Token: 0x02001E6F RID: 7791
		public class MOLTENCARBON
		{
			// Token: 0x04008AC9 RID: 35529
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon", "MOLTENCARBON");

			// Token: 0x04008ACA RID: 35530
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E70 RID: 7792
		public class MOLTENCOPPER
		{
			// Token: 0x04008ACB RID: 35531
			public static LocString NAME = UI.FormatAsLink("Molten Copper", "MOLTENCOPPER");

			// Token: 0x04008ACC RID: 35532
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Molten Copper is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E71 RID: 7793
		public class MOLTENGLASS
		{
			// Token: 0x04008ACD RID: 35533
			public static LocString NAME = UI.FormatAsLink("Molten Glass", "MOLTENGLASS");

			// Token: 0x04008ACE RID: 35534
			public static LocString DESC = "Molten Glass is a composite of granular rock, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E72 RID: 7794
		public class MOLTENGOLD
		{
			// Token: 0x04008ACF RID: 35535
			public static LocString NAME = UI.FormatAsLink("Molten Gold", "MOLTENGOLD");

			// Token: 0x04008AD0 RID: 35536
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold, a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E73 RID: 7795
		public class MOLTENIRON
		{
			// Token: 0x04008AD1 RID: 35537
			public static LocString NAME = UI.FormatAsLink("Molten Iron", "MOLTENIRON");

			// Token: 0x04008AD2 RID: 35538
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Molten Iron is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E74 RID: 7796
		public class MOLTENCOBALT
		{
			// Token: 0x04008AD3 RID: 35539
			public static LocString NAME = UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT");

			// Token: 0x04008AD4 RID: 35540
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Molten Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E75 RID: 7797
		public class MOLTENLEAD
		{
			// Token: 0x04008AD5 RID: 35541
			public static LocString NAME = UI.FormatAsLink("Molten Lead", "MOLTENLEAD");

			// Token: 0x04008AD6 RID: 35542
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is an extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E76 RID: 7798
		public class MOLTENNIOBIUM
		{
			// Token: 0x04008AD7 RID: 35543
			public static LocString NAME = UI.FormatAsLink("Molten Niobium", "MOLTENNIOBIUM");

			// Token: 0x04008AD8 RID: 35544
			public static LocString DESC = string.Concat(new string[]
			{
				"(Nb) Molten Niobium is a ",
				UI.FormatAsLink("Rare Metal", "RAREMATERIALS"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E77 RID: 7799
		public class MOLTENTUNGSTEN
		{
			// Token: 0x04008AD9 RID: 35545
			public static LocString NAME = UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN");

			// Token: 0x04008ADA RID: 35546
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Molten Tungsten is a crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E78 RID: 7800
		public class MOLTENTUNGSTENDISELENIDE
		{
			// Token: 0x04008ADB RID: 35547
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "MOLTENTUNGSTENDISELENIDE");

			// Token: 0x04008ADC RID: 35548
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E79 RID: 7801
		public class MOLTENSTEEL
		{
			// Token: 0x04008ADD RID: 35549
			public static LocString NAME = UI.FormatAsLink("Molten Steel", "MOLTENSTEEL");

			// Token: 0x04008ADE RID: 35550
			public static LocString DESC = string.Concat(new string[]
			{
				"Molten Steel is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy of iron and carbon, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001E7A RID: 7802
		public class MOLTENURANIUM
		{
			// Token: 0x04008ADF RID: 35551
			public static LocString NAME = UI.FormatAsLink("Liquid Uranium", "MOLTENURANIUM");

			// Token: 0x04008AE0 RID: 35552
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Liquid Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" substance, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				"."
			});
		}

		// Token: 0x02001E7B RID: 7803
		public class NIOBIUM
		{
			// Token: 0x04008AE1 RID: 35553
			public static LocString NAME = UI.FormatAsLink("Niobium", "NIOBIUM");

			// Token: 0x04008AE2 RID: 35554
			public static LocString DESC = string.Concat(new string[]
			{
				"(Nb) Niobium is a ",
				UI.FormatAsLink("Rare Metal", "RAREMATERIALS"),
				" with many practical applications in metallurgy and superconductor ",
				UI.FormatAsLink("Research", "RESEARCH"),
				"."
			});
		}

		// Token: 0x02001E7C RID: 7804
		public class NIOBIUMGAS
		{
			// Token: 0x04008AE3 RID: 35555
			public static LocString NAME = UI.FormatAsLink("Niobium Gas", "NIOBIUMGAS");

			// Token: 0x04008AE4 RID: 35556
			public static LocString DESC = string.Concat(new string[]
			{
				"(Nb) Niobium Gas is a ",
				UI.FormatAsLink("Rare Metal", "RAREMATERIALS"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001E7D RID: 7805
		public class NUCLEARWASTE
		{
			// Token: 0x04008AE5 RID: 35557
			public static LocString NAME = UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE");

			// Token: 0x04008AE6 RID: 35558
			public static LocString DESC = string.Concat(new string[]
			{
				"Highly toxic liquid full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				" which emit ",
				UI.FormatAsLink("Radiation", "RADIATION"),
				" that can be absorbed by ",
				UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
				"."
			});
		}

		// Token: 0x02001E7E RID: 7806
		public class OBSIDIAN
		{
			// Token: 0x04008AE7 RID: 35559
			public static LocString NAME = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x04008AE8 RID: 35560
			public static LocString DESC = "Obsidian is a brittle composite of volcanic " + UI.FormatAsLink("Glass", "GLASS") + ".";
		}

		// Token: 0x02001E7F RID: 7807
		public class OXYGEN
		{
			// Token: 0x04008AE9 RID: 35561
			public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN");

			// Token: 0x04008AEA RID: 35562
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", necessary for sustaining life.\n\nIt tends to rise above other gases.";
		}

		// Token: 0x02001E80 RID: 7808
		public class OXYROCK
		{
			// Token: 0x04008AEB RID: 35563
			public static LocString NAME = UI.FormatAsLink("Oxylite", "OXYROCK");

			// Token: 0x04008AEC RID: 35564
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				".\n\nExcavating ",
				ELEMENTS.OXYROCK.NAME,
				" increases its emission rate, but depletes the ore more rapidly."
			});
		}

		// Token: 0x02001E81 RID: 7809
		public class PHOSPHATENODULES
		{
			// Token: 0x04008AED RID: 35565
			public static LocString NAME = UI.FormatAsLink("Phosphate Nodules", "PHOSPHATENODULES");

			// Token: 0x04008AEE RID: 35566
			public static LocString DESC = "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
		}

		// Token: 0x02001E82 RID: 7810
		public class PHOSPHORITE
		{
			// Token: 0x04008AEF RID: 35567
			public static LocString NAME = UI.FormatAsLink("Phosphorite", "PHOSPHORITE");

			// Token: 0x04008AF0 RID: 35568
			public static LocString DESC = "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
		}

		// Token: 0x02001E83 RID: 7811
		public class PHOSPHORUS
		{
			// Token: 0x04008AF1 RID: 35569
			public static LocString NAME = UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS");

			// Token: 0x04008AF2 RID: 35570
			public static LocString DESC = "(P) Refined Phosphorus is a chemical element in its " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E84 RID: 7812
		public class PHOSPHORUSGAS
		{
			// Token: 0x04008AF3 RID: 35571
			public static LocString NAME = UI.FormatAsLink("Phosphorus Gas", "PHOSPHORUSGAS");

			// Token: 0x04008AF4 RID: 35572
			public static LocString DESC = string.Concat(new string[]
			{
				"(P) Phosphorus Gas is the ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state of ",
				UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS"),
				"."
			});
		}

		// Token: 0x02001E85 RID: 7813
		public class PROPANE
		{
			// Token: 0x04008AF5 RID: 35573
			public static LocString NAME = UI.FormatAsLink("Propane Gas", "PROPANE");

			// Token: 0x04008AF6 RID: 35574
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02001E86 RID: 7814
		public class RADIUM
		{
			// Token: 0x04008AF7 RID: 35575
			public static LocString NAME = UI.FormatAsLink("Radium", "RADIUM");

			// Token: 0x04008AF8 RID: 35576
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ra) Radium is a ",
				UI.FormatAsLink("Light", "LIGHT"),
				" emitting radioactive substance.\n\nIt is useful as a ",
				UI.FormatAsLink("Power", "POWER"),
				" source."
			});
		}

		// Token: 0x02001E87 RID: 7815
		public class YELLOWCAKE
		{
			// Token: 0x04008AF9 RID: 35577
			public static LocString NAME = UI.FormatAsLink("Yellowcake", "YELLOWCAKE");

			// Token: 0x04008AFA RID: 35578
			public static LocString DESC = string.Concat(new string[]
			{
				"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of ",
				UI.FormatAsLink("Uranium", "URANIUM"),
				" mining.\n\nIt is useful in preparing fuel for ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				".\n\nNote: Do not eat."
			});
		}

		// Token: 0x02001E88 RID: 7816
		public class ROCKGAS
		{
			// Token: 0x04008AFB RID: 35579
			public static LocString NAME = UI.FormatAsLink("Rock Gas", "ROCKGAS");

			// Token: 0x04008AFC RID: 35580
			public static LocString DESC = "Rock Gas is rock that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02001E89 RID: 7817
		public class RUST
		{
			// Token: 0x04008AFD RID: 35581
			public static LocString NAME = UI.FormatAsLink("Rust", "RUST");

			// Token: 0x04008AFE RID: 35582
			public static LocString DESC = string.Concat(new string[]
			{
				"Rust is an iron oxide that forms from the breakdown of ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful in some ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" production processes."
			});
		}

		// Token: 0x02001E8A RID: 7818
		public class REGOLITH
		{
			// Token: 0x04008AFF RID: 35583
			public static LocString NAME = UI.FormatAsLink("Regolith", "REGOLITH");

			// Token: 0x04008B00 RID: 35584
			public static LocString DESC = "Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "REGOLITH") + ".";
		}

		// Token: 0x02001E8B RID: 7819
		public class SALTGAS
		{
			// Token: 0x04008B01 RID: 35585
			public static LocString NAME = UI.FormatAsLink("Salt Gas", "SALTGAS");

			// Token: 0x04008B02 RID: 35586
			public static LocString DESC = "(NaCl) Salt Gas is an edible chemical compound that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02001E8C RID: 7820
		public class MOLTENSALT
		{
			// Token: 0x04008B03 RID: 35587
			public static LocString NAME = UI.FormatAsLink("Molten Salt", "MOLTENSALT");

			// Token: 0x04008B04 RID: 35588
			public static LocString DESC = "(NaCl) Molten Salt is an edible chemical compound that has been heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02001E8D RID: 7821
		public class SALT
		{
			// Token: 0x04008B05 RID: 35589
			public static LocString NAME = UI.FormatAsLink("Salt", "SALT");

			// Token: 0x04008B06 RID: 35590
			public static LocString DESC = "(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02001E8E RID: 7822
		public class SALTWATER
		{
			// Token: 0x04008B07 RID: 35591
			public static LocString NAME = UI.FormatAsLink("Salt Water", "SALTWATER");

			// Token: 0x04008B08 RID: 35592
			public static LocString DESC = string.Concat(new string[]
			{
				"Salt Water is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02001E8F RID: 7823
		public class SAND
		{
			// Token: 0x04008B09 RID: 35593
			public static LocString NAME = UI.FormatAsLink("Sand", "SAND");

			// Token: 0x04008B0A RID: 35594
			public static LocString DESC = "Sand is a composite of granular rock.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x02001E90 RID: 7824
		public class SANDCEMENT
		{
			// Token: 0x04008B0B RID: 35595
			public static LocString NAME = UI.FormatAsLink("Sand Cement", "SANDCEMENT");

			// Token: 0x04008B0C RID: 35596
			public static LocString DESC = "";
		}

		// Token: 0x02001E91 RID: 7825
		public class SANDSTONE
		{
			// Token: 0x04008B0D RID: 35597
			public static LocString NAME = UI.FormatAsLink("Sandstone", "SANDSTONE");

			// Token: 0x04008B0E RID: 35598
			public static LocString DESC = "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E92 RID: 7826
		public class SEDIMENTARYROCK
		{
			// Token: 0x04008B0F RID: 35599
			public static LocString NAME = UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK");

			// Token: 0x04008B10 RID: 35600
			public static LocString DESC = "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02001E93 RID: 7827
		public class SLIMEMOLD
		{
			// Token: 0x04008B11 RID: 35601
			public static LocString NAME = UI.FormatAsLink("Slime", "SLIMEMOLD");

			// Token: 0x04008B12 RID: 35602
			public static LocString DESC = string.Concat(new string[]
			{
				"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into ",
				UI.FormatAsLink("Algae", "ALGAE"),
				" and emits ",
				ELEMENTS.CONTAMINATEDOXYGEN.NAME,
				" once dug up."
			});
		}

		// Token: 0x02001E94 RID: 7828
		public class SNOW
		{
			// Token: 0x04008B13 RID: 35603
			public static LocString NAME = UI.FormatAsLink("Snow", "SNOW");

			// Token: 0x04008B14 RID: 35604
			public static LocString DESC = "(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes " + UI.FormatAsLink("Water", "WATER") + " when melted.";
		}

		// Token: 0x02001E95 RID: 7829
		public class SOLIDCARBONDIOXIDE
		{
			// Token: 0x04008B15 RID: 35605
			public static LocString NAME = UI.FormatAsLink("Solid Carbon Dioxide", "SOLIDCARBONDIOXIDE");

			// Token: 0x04008B16 RID: 35606
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E96 RID: 7830
		public class SOLIDCHLORINE
		{
			// Token: 0x04008B17 RID: 35607
			public static LocString NAME = UI.FormatAsLink("Solid Chlorine", "SOLIDCHLORINE");

			// Token: 0x04008B18 RID: 35608
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001E97 RID: 7831
		public class SOLIDCRUDEOIL
		{
			// Token: 0x04008B19 RID: 35609
			public static LocString NAME = UI.FormatAsLink("Solid Crude Oil", "SOLIDCRUDEOIL");

			// Token: 0x04008B1A RID: 35610
			public static LocString DESC = "";
		}

		// Token: 0x02001E98 RID: 7832
		public class SOLIDHYDROGEN
		{
			// Token: 0x04008B1B RID: 35611
			public static LocString NAME = UI.FormatAsLink("Solid Hydrogen", "SOLIDHYDROGEN");

			// Token: 0x04008B1C RID: 35612
			public static LocString DESC = "(H) Solid Hydrogen is the universe's most common element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E99 RID: 7833
		public class SOLIDMERCURY
		{
			// Token: 0x04008B1D RID: 35613
			public static LocString NAME = UI.FormatAsLink("Mercury", "SOLIDMERCURY");

			// Token: 0x04008B1E RID: 35614
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a rare ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001E9A RID: 7834
		public class SOLIDOXYGEN
		{
			// Token: 0x04008B1F RID: 35615
			public static LocString NAME = UI.FormatAsLink("Solid Oxygen", "SOLIDOXYGEN");

			// Token: 0x04008B20 RID: 35616
			public static LocString DESC = "(O<sub>2</sub>) Solid Oxygen is a breathable element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E9B RID: 7835
		public class SOLIDMETHANE
		{
			// Token: 0x04008B21 RID: 35617
			public static LocString NAME = UI.FormatAsLink("Solid Methane", "SOLIDMETHANE");

			// Token: 0x04008B22 RID: 35618
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E9C RID: 7836
		public class SOLIDNAPHTHA
		{
			// Token: 0x04008B23 RID: 35619
			public static LocString NAME = UI.FormatAsLink("Solid Naphtha", "SOLIDNAPHTHA");

			// Token: 0x04008B24 RID: 35620
			public static LocString DESC = "Naphtha is a distilled hydrocarbon mixture in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001E9D RID: 7837
		public class CORIUM
		{
			// Token: 0x04008B25 RID: 35621
			public static LocString NAME = UI.FormatAsLink("Corium", "CORIUM");

			// Token: 0x04008B26 RID: 35622
			public static LocString DESC = "A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + " gas.";
		}

		// Token: 0x02001E9E RID: 7838
		public class SOLIDPETROLEUM
		{
			// Token: 0x04008B27 RID: 35623
			public static LocString NAME = UI.FormatAsLink("Solid Petroleum", "SOLIDPETROLEUM");

			// Token: 0x04008B28 RID: 35624
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001E9F RID: 7839
		public class SOLIDPROPANE
		{
			// Token: 0x04008B29 RID: 35625
			public static LocString NAME = UI.FormatAsLink("Solid Propane", "SOLIDPROPANE");

			// Token: 0x04008B2A RID: 35626
			public static LocString DESC = "(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02001EA0 RID: 7840
		public class SOLIDSUPERCOOLANT
		{
			// Token: 0x04008B2B RID: 35627
			public static LocString NAME = UI.FormatAsLink("Solid Super Coolant", "SOLIDSUPERCOOLANT");

			// Token: 0x04008B2C RID: 35628
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001EA1 RID: 7841
		public class SOLIDVISCOGEL
		{
			// Token: 0x04008B2D RID: 35629
			public static LocString NAME = UI.FormatAsLink("Solid Visco-Gel", "SOLIDVISCOGEL");

			// Token: 0x04008B2E RID: 35630
			public static LocString DESC = string.Concat(new string[]
			{
				"Visco-Gel is a polymer that has high surface tension when in ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" form.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001EA2 RID: 7842
		public class SYNGAS
		{
			// Token: 0x04008B2F RID: 35631
			public static LocString NAME = UI.FormatAsLink("Synthesis Gas", "SYNGAS");

			// Token: 0x04008B30 RID: 35632
			public static LocString DESC = "Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02001EA3 RID: 7843
		public class MOLTENSYNGAS
		{
			// Token: 0x04008B31 RID: 35633
			public static LocString NAME = UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");

			// Token: 0x04008B32 RID: 35634
			public static LocString DESC = "Molten Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02001EA4 RID: 7844
		public class SOLIDSYNGAS
		{
			// Token: 0x04008B33 RID: 35635
			public static LocString NAME = UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");

			// Token: 0x04008B34 RID: 35636
			public static LocString DESC = "Solid Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02001EA5 RID: 7845
		public class STEAM
		{
			// Token: 0x04008B35 RID: 35637
			public static LocString NAME = UI.FormatAsLink("Steam", "STEAM");

			// Token: 0x04008B36 RID: 35638
			public static LocString DESC = string.Concat(new string[]
			{
				"(H<sub>2</sub>O) Steam is ",
				ELEMENTS.WATER.NAME,
				" that has been heated into a scalding ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02001EA6 RID: 7846
		public class STEEL
		{
			// Token: 0x04008B37 RID: 35639
			public static LocString NAME = UI.FormatAsLink("Steel", "STEEL");

			// Token: 0x04008B38 RID: 35640
			public static LocString DESC = "Steel is a " + UI.FormatAsLink("Metal Alloy", "REFINEDMETAL") + " composed of iron and carbon.";
		}

		// Token: 0x02001EA7 RID: 7847
		public class STEELGAS
		{
			// Token: 0x04008B39 RID: 35641
			public static LocString NAME = UI.FormatAsLink("Steel Gas", "STEELGAS");

			// Token: 0x04008B3A RID: 35642
			public static LocString DESC = string.Concat(new string[]
			{
				"Steel Gas is a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" composed of iron and carbon."
			});
		}

		// Token: 0x02001EA8 RID: 7848
		public class SULFUR
		{
			// Token: 0x04008B3B RID: 35643
			public static LocString NAME = UI.FormatAsLink("Sulfur", "SULFUR");

			// Token: 0x04008B3C RID: 35644
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02001EA9 RID: 7849
		public class SULFURGAS
		{
			// Token: 0x04008B3D RID: 35645
			public static LocString NAME = UI.FormatAsLink("Sulfur Gas", "SULFURGAS");

			// Token: 0x04008B3E RID: 35646
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001EAA RID: 7850
		public class SUPERCOOLANT
		{
			// Token: 0x04008B3F RID: 35647
			public static LocString NAME = UI.FormatAsLink("Super Coolant", "SUPERCOOLANT");

			// Token: 0x04008B40 RID: 35648
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02001EAB RID: 7851
		public class SUPERCOOLANTGAS
		{
			// Token: 0x04008B41 RID: 35649
			public static LocString NAME = UI.FormatAsLink("Super Coolant Gas", "SUPERCOOLANTGAS");

			// Token: 0x04008B42 RID: 35650
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001EAC RID: 7852
		public class SUPERINSULATOR
		{
			// Token: 0x04008B43 RID: 35651
			public static LocString NAME = UI.FormatAsLink("Insulation", "SUPERINSULATOR");

			// Token: 0x04008B44 RID: 35652
			public static LocString DESC = string.Concat(new string[]
			{
				"Insulation reduces ",
				UI.FormatAsLink("Heat Transfer", "HEAT"),
				" and is composed of recrystallized ",
				UI.FormatAsLink("Abyssalite", "KATAIRITE"),
				"."
			});
		}

		// Token: 0x02001EAD RID: 7853
		public class TEMPCONDUCTORSOLID
		{
			// Token: 0x04008B45 RID: 35653
			public static LocString NAME = UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID");

			// Token: 0x04008B46 RID: 35654
			public static LocString DESC = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";
		}

		// Token: 0x02001EAE RID: 7854
		public class TUNGSTEN
		{
			// Token: 0x04008B47 RID: 35655
			public static LocString NAME = UI.FormatAsLink("Tungsten", "TUNGSTEN");

			// Token: 0x04008B48 RID: 35656
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is an extremely tough crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001EAF RID: 7855
		public class TUNGSTENGAS
		{
			// Token: 0x04008B49 RID: 35657
			public static LocString NAME = UI.FormatAsLink("Tungsten Gas", "TUNGSTENGAS");

			// Token: 0x04008B4A RID: 35658
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is a superheated crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001EB0 RID: 7856
		public class TUNGSTENDISELENIDE
		{
			// Token: 0x04008B4B RID: 35659
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "TUNGSTENDISELENIDE");

			// Token: 0x04008B4C RID: 35660
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound with a crystalline structure.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001EB1 RID: 7857
		public class TUNGSTENDISELENIDEGAS
		{
			// Token: 0x04008B4D RID: 35661
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide Gas", "TUNGSTENDISELENIDEGAS");

			// Token: 0x04008B4E RID: 35662
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02001EB2 RID: 7858
		public class TOXICSAND
		{
			// Token: 0x04008B4F RID: 35663
			public static LocString NAME = UI.FormatAsLink("Polluted Dirt", "TOXICSAND");

			// Token: 0x04008B50 RID: 35664
			public static LocString DESC = "Polluted Dirt is unprocessed biological waste.\n\nIt emits " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " over time.";
		}

		// Token: 0x02001EB3 RID: 7859
		public class UNOBTANIUM
		{
			// Token: 0x04008B51 RID: 35665
			public static LocString NAME = UI.FormatAsLink("Neutronium", "UNOBTANIUM");

			// Token: 0x04008B52 RID: 35666
			public static LocString DESC = "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
		}

		// Token: 0x02001EB4 RID: 7860
		public class URANIUMORE
		{
			// Token: 0x04008B53 RID: 35667
			public static LocString NAME = UI.FormatAsLink("Uranium Ore", "URANIUMORE");

			// Token: 0x04008B54 RID: 35668
			public static LocString DESC = "(U) Uranium Ore is a highly " + UI.FormatAsLink("Radioactive", "RADIATION") + " substance.\n\nIt can be refined into fuel for research reactors.";
		}

		// Token: 0x02001EB5 RID: 7861
		public class VACUUM
		{
			// Token: 0x04008B55 RID: 35669
			public static LocString NAME = UI.FormatAsLink("Vacuum", "VACUUM");

			// Token: 0x04008B56 RID: 35670
			public static LocString DESC = "A vacuum is a space devoid of all matter.";
		}

		// Token: 0x02001EB6 RID: 7862
		public class VISCOGEL
		{
			// Token: 0x04008B57 RID: 35671
			public static LocString NAME = UI.FormatAsLink("Visco-Gel Fluid", "VISCOGEL");

			// Token: 0x04008B58 RID: 35672
			public static LocString DESC = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
		}

		// Token: 0x02001EB7 RID: 7863
		public class VOID
		{
			// Token: 0x04008B59 RID: 35673
			public static LocString NAME = UI.FormatAsLink("Void", "VOID");

			// Token: 0x04008B5A RID: 35674
			public static LocString DESC = "Cold, infinite nothingness.";
		}

		// Token: 0x02001EB8 RID: 7864
		public class COMPOSITION
		{
			// Token: 0x04008B5B RID: 35675
			public static LocString NAME = UI.FormatAsLink("Composition", "COMPOSITION");

			// Token: 0x04008B5C RID: 35676
			public static LocString DESC = "A mixture of two or more elements.";
		}

		// Token: 0x02001EB9 RID: 7865
		public class WATER
		{
			// Token: 0x04008B5D RID: 35677
			public static LocString NAME = UI.FormatAsLink("Water", "WATER");

			// Token: 0x04008B5E RID: 35678
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + UI.FormatAsLink("Water", "WATER") + ", suitable for consumption.";
		}

		// Token: 0x02001EBA RID: 7866
		public class WOLFRAMITE
		{
			// Token: 0x04008B5F RID: 35679
			public static LocString NAME = UI.FormatAsLink("Wolframite", "WOLFRAMITE");

			// Token: 0x04008B60 RID: 35680
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001EBB RID: 7867
		public class TESTELEMENT
		{
			// Token: 0x04008B61 RID: 35681
			public static LocString NAME = UI.FormatAsLink("Test Element", "TESTELEMENT");

			// Token: 0x04008B62 RID: 35682
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02001EBC RID: 7868
		public class POLYPROPYLENE
		{
			// Token: 0x04008B63 RID: 35683
			public static LocString NAME = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x04008B64 RID: 35684
			public static LocString DESC = "(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> " + ELEMENTS.POLYPROPYLENE.NAME + " is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";

			// Token: 0x04008B65 RID: 35685
			public static LocString BUILD_DESC = "Buildings made of this " + ELEMENTS.POLYPROPYLENE.NAME + " have antiseptic properties";
		}

		// Token: 0x02001EBD RID: 7869
		public class HARDPOLYPROPYLENE
		{
			// Token: 0x04008B66 RID: 35686
			public static LocString NAME = UI.FormatAsLink("Plastium", "HARDPOLYPROPYLENE");

			// Token: 0x04008B67 RID: 35687
			public static LocString DESC = string.Concat(new string[]
			{
				ELEMENTS.HARDPOLYPROPYLENE.NAME,
				" is an advanced thermoplastic polymer made from ",
				UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID"),
				", ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" and ",
				UI.FormatAsLink("Brackwax", "MILKFAT"),
				".\n\nIt is highly heat-resistant and suitable for use in space buildings."
			});
		}

		// Token: 0x02001EBE RID: 7870
		public class NAPHTHA
		{
			// Token: 0x04008B68 RID: 35688
			public static LocString NAME = UI.FormatAsLink("Liquid Naphtha", "NAPHTHA");

			// Token: 0x04008B69 RID: 35689
			public static LocString DESC = "Naphtha a distilled hydrocarbon mixture produced from the burning of " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".";
		}

		// Token: 0x02001EBF RID: 7871
		public class SLABS
		{
			// Token: 0x04008B6A RID: 35690
			public static LocString NAME = UI.FormatAsLink("Building Slab", "SLABS");

			// Token: 0x04008B6B RID: 35691
			public static LocString DESC = "Slabs are a refined mineral building block used for assembling advanced buildings.";
		}

		// Token: 0x02001EC0 RID: 7872
		public class TOXICMUD
		{
			// Token: 0x04008B6C RID: 35692
			public static LocString NAME = UI.FormatAsLink("Polluted Mud", "TOXICMUD");

			// Token: 0x04008B6D RID: 35693
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" and ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02001EC1 RID: 7873
		public class MUD
		{
			// Token: 0x04008B6E RID: 35694
			public static LocString NAME = UI.FormatAsLink("Mud", "MUD");

			// Token: 0x04008B6F RID: 35695
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Dirt", "DIRT"),
				" and ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02001EC2 RID: 7874
		public class SUCROSE
		{
			// Token: 0x04008B70 RID: 35696
			public static LocString NAME = UI.FormatAsLink("Sucrose", "SUCROSE");

			// Token: 0x04008B71 RID: 35697
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used directly for cooking, or refined and eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02001EC3 RID: 7875
		public class MOLTENSUCROSE
		{
			// Token: 0x04008B72 RID: 35698
			public static LocString NAME = UI.FormatAsLink("Liquid Sucrose", "MOLTENSUCROSE");

			// Token: 0x04008B73 RID: 35699
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar, heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt can be used directly for cooking, or refined and eaten with meals to increase Duplicant ",
				UI.FormatAsLink("Morale", "MORALE"),
				"."
			});
		}
	}
}
