using System;

namespace STRINGS
{
	// Token: 0x02000DBB RID: 3515
	public static class WORLD_TRAITS
	{
		// Token: 0x0400513E RID: 20798
		public static LocString MISSING_TRAIT = "<missing traits>";

		// Token: 0x02001F35 RID: 7989
		public static class NO_TRAITS
		{
			// Token: 0x04008CE7 RID: 36071
			public static LocString NAME = "<i>This world is stable and has no unusual features.</i>";

			// Token: 0x04008CE8 RID: 36072
			public static LocString NAME_SHORTHAND = "No unusual features";

			// Token: 0x04008CE9 RID: 36073
			public static LocString DESCRIPTION = "This world exists in a particularly stable configuration each time it is encountered";
		}

		// Token: 0x02001F36 RID: 7990
		public static class BOULDERS_LARGE
		{
			// Token: 0x04008CEA RID: 36074
			public static LocString NAME = "Large Boulders";

			// Token: 0x04008CEB RID: 36075
			public static LocString DESCRIPTION = "Huge boulders make digging through this world more difficult";
		}

		// Token: 0x02001F37 RID: 7991
		public static class BOULDERS_MEDIUM
		{
			// Token: 0x04008CEC RID: 36076
			public static LocString NAME = "Medium Boulders";

			// Token: 0x04008CED RID: 36077
			public static LocString DESCRIPTION = "Mid-sized boulders make digging through this world more difficult";
		}

		// Token: 0x02001F38 RID: 7992
		public static class BOULDERS_MIXED
		{
			// Token: 0x04008CEE RID: 36078
			public static LocString NAME = "Mixed Boulders";

			// Token: 0x04008CEF RID: 36079
			public static LocString DESCRIPTION = "Boulders of various sizes make digging through this world more difficult";
		}

		// Token: 0x02001F39 RID: 7993
		public static class BOULDERS_SMALL
		{
			// Token: 0x04008CF0 RID: 36080
			public static LocString NAME = "Small Boulders";

			// Token: 0x04008CF1 RID: 36081
			public static LocString DESCRIPTION = "Tiny boulders make digging through this world more difficult";
		}

		// Token: 0x02001F3A RID: 7994
		public static class DEEP_OIL
		{
			// Token: 0x04008CF2 RID: 36082
			public static LocString NAME = "Trapped Oil";

			// Token: 0x04008CF3 RID: 36083
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Most of the ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" in this world will need to be extracted with ",
				BUILDINGS.PREFABS.OILWELLCAP.NAME,
				"s"
			});
		}

		// Token: 0x02001F3B RID: 7995
		public static class FROZEN_CORE
		{
			// Token: 0x04008CF4 RID: 36084
			public static LocString NAME = "Frozen Core";

			// Token: 0x04008CF5 RID: 36085
			public static LocString DESCRIPTION = "This world has a chilly core of solid " + ELEMENTS.ICE.NAME;
		}

		// Token: 0x02001F3C RID: 7996
		public static class GEOACTIVE
		{
			// Token: 0x04008CF6 RID: 36086
			public static LocString NAME = "Geoactive";

			// Token: 0x04008CF7 RID: 36087
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has more ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02001F3D RID: 7997
		public static class GEODES
		{
			// Token: 0x04008CF8 RID: 36088
			public static LocString NAME = "Geodes";

			// Token: 0x04008CF9 RID: 36089
			public static LocString DESCRIPTION = "Large geodes containing rare material caches are deposited across this world";
		}

		// Token: 0x02001F3E RID: 7998
		public static class GEODORMANT
		{
			// Token: 0x04008CFA RID: 36090
			public static LocString NAME = "Geodormant";

			// Token: 0x04008CFB RID: 36091
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has fewer ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02001F3F RID: 7999
		public static class GLACIERS_LARGE
		{
			// Token: 0x04008CFC RID: 36092
			public static LocString NAME = "Large Glaciers";

			// Token: 0x04008CFD RID: 36093
			public static LocString DESCRIPTION = "Huge chunks of primordial " + ELEMENTS.ICE.NAME + " are scattered across this world";
		}

		// Token: 0x02001F40 RID: 8000
		public static class IRREGULAR_OIL
		{
			// Token: 0x04008CFE RID: 36094
			public static LocString NAME = "Irregular Oil";

			// Token: 0x04008CFF RID: 36095
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" on this asteroid is anything but regular!"
			});
		}

		// Token: 0x02001F41 RID: 8001
		public static class MAGMA_VENTS
		{
			// Token: 0x04008D00 RID: 36096
			public static LocString NAME = "Magma Channels";

			// Token: 0x04008D01 RID: 36097
			public static LocString DESCRIPTION = "The " + ELEMENTS.MAGMA.NAME + " from this world's core has leaked into the mantle and crust";
		}

		// Token: 0x02001F42 RID: 8002
		public static class METAL_POOR
		{
			// Token: 0x04008D02 RID: 36098
			public static LocString NAME = "Metal Poor";

			// Token: 0x04008D03 RID: 36099
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"There is a reduced amount of ",
				UI.PRE_KEYWORD,
				"Metal Ore",
				UI.PST_KEYWORD,
				" on this world, proceed with caution!"
			});
		}

		// Token: 0x02001F43 RID: 8003
		public static class METAL_RICH
		{
			// Token: 0x04008D04 RID: 36100
			public static LocString NAME = "Metal Rich";

			// Token: 0x04008D05 RID: 36101
			public static LocString DESCRIPTION = "This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD;
		}

		// Token: 0x02001F44 RID: 8004
		public static class MISALIGNED_START
		{
			// Token: 0x04008D06 RID: 36102
			public static LocString NAME = "Alternate Pod Location";

			// Token: 0x04008D07 RID: 36103
			public static LocString DESCRIPTION = "The " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " didn't end up in the asteroid's exact center this time... but it's still nowhere near the surface";
		}

		// Token: 0x02001F45 RID: 8005
		public static class SLIME_SPLATS
		{
			// Token: 0x04008D08 RID: 36104
			public static LocString NAME = "Slime Molds";

			// Token: 0x04008D09 RID: 36105
			public static LocString DESCRIPTION = "Sickly " + ELEMENTS.SLIMEMOLD.NAME + " growths have crept all over this world";
		}

		// Token: 0x02001F46 RID: 8006
		public static class SUBSURFACE_OCEAN
		{
			// Token: 0x04008D0A RID: 36106
			public static LocString NAME = "Subsurface Ocean";

			// Token: 0x04008D0B RID: 36107
			public static LocString DESCRIPTION = "Below the crust of this world is a " + ELEMENTS.SALTWATER.NAME + " sea";
		}

		// Token: 0x02001F47 RID: 8007
		public static class VOLCANOES
		{
			// Token: 0x04008D0C RID: 36108
			public static LocString NAME = "Volcanic Activity";

			// Token: 0x04008D0D RID: 36109
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Several active ",
				UI.PRE_KEYWORD,
				"Volcanoes",
				UI.PST_KEYWORD,
				" have been detected in this world"
			});
		}

		// Token: 0x02001F48 RID: 8008
		public static class RADIOACTIVE_CRUST
		{
			// Token: 0x04008D0E RID: 36110
			public static LocString NAME = "Radioactive Crust";

			// Token: 0x04008D0F RID: 36111
			public static LocString DESCRIPTION = "Deposits of " + ELEMENTS.URANIUMORE.NAME + " are found in this world's crust";
		}

		// Token: 0x02001F49 RID: 8009
		public static class LUSH_CORE
		{
			// Token: 0x04008D10 RID: 36112
			public static LocString NAME = "Lush Core";

			// Token: 0x04008D11 RID: 36113
			public static LocString DESCRIPTION = "This world has a lush forest core";
		}

		// Token: 0x02001F4A RID: 8010
		public static class METAL_CAVES
		{
			// Token: 0x04008D12 RID: 36114
			public static LocString NAME = "Metallic Caves";

			// Token: 0x04008D13 RID: 36115
			public static LocString DESCRIPTION = "This world has caves of metal ore";
		}

		// Token: 0x02001F4B RID: 8011
		public static class DISTRESS_SIGNAL
		{
			// Token: 0x04008D14 RID: 36116
			public static LocString NAME = "Frozen Friend";

			// Token: 0x04008D15 RID: 36117
			public static LocString DESCRIPTION = "This world contains a frozen friend from a long time ago";
		}

		// Token: 0x02001F4C RID: 8012
		public static class CRASHED_SATELLITES
		{
			// Token: 0x04008D16 RID: 36118
			public static LocString NAME = "Crashed Satellites";

			// Token: 0x04008D17 RID: 36119
			public static LocString DESCRIPTION = "This world contains crashed radioactive satellites";
		}
	}
}
