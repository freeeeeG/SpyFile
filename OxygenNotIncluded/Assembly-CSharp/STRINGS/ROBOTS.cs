using System;

namespace STRINGS
{
	// Token: 0x02000DB3 RID: 3507
	public class ROBOTS
	{
		// Token: 0x0400512E RID: 20782
		public static LocString CATEGORY_NAME = "Robots";

		// Token: 0x02001E0F RID: 7695
		public class STATS
		{
			// Token: 0x02002E7A RID: 11898
			public class INTERNALBATTERY
			{
				// Token: 0x0400BE9A RID: 48794
				public static LocString NAME = "Rechargeable Battery";

				// Token: 0x0400BE9B RID: 48795
				public static LocString TOOLTIP = "When this bot's battery runs out it must temporarily stop working to go recharge";
			}

			// Token: 0x02002E7B RID: 11899
			public class INTERNALCHEMICALBATTERY
			{
				// Token: 0x0400BE9C RID: 48796
				public static LocString NAME = "Chemical Battery";

				// Token: 0x0400BE9D RID: 48797
				public static LocString TOOLTIP = "This bot will shut down permanently when its battery runs out";
			}
		}

		// Token: 0x02001E10 RID: 7696
		public class ATTRIBUTES
		{
			// Token: 0x02002E7C RID: 11900
			public class INTERNALBATTERYDELTA
			{
				// Token: 0x0400BE9E RID: 48798
				public static LocString NAME = "Rechargeable Battery Drain";

				// Token: 0x0400BE9F RID: 48799
				public static LocString TOOLTIP = "The rate at which battery life is depleted";
			}
		}

		// Token: 0x02001E11 RID: 7697
		public class STATUSITEMS
		{
			// Token: 0x02002E7D RID: 11901
			public class CANTREACHSTATION
			{
				// Token: 0x0400BEA0 RID: 48800
				public static LocString NAME = "Unreachable Dock";

				// Token: 0x0400BEA1 RID: 48801
				public static LocString DESC = "Obstacles are preventing {0} from heading home";

				// Token: 0x0400BEA2 RID: 48802
				public static LocString TOOLTIP = "Obstacles are preventing {0} from heading home";
			}

			// Token: 0x02002E7E RID: 11902
			public class MOVINGTOCHARGESTATION
			{
				// Token: 0x0400BEA3 RID: 48803
				public static LocString NAME = "Traveling to Dock";

				// Token: 0x0400BEA4 RID: 48804
				public static LocString DESC = "{0} is on its way home to recharge";

				// Token: 0x0400BEA5 RID: 48805
				public static LocString TOOLTIP = "{0} is on its way home to recharge";
			}

			// Token: 0x02002E7F RID: 11903
			public class LOWBATTERY
			{
				// Token: 0x0400BEA6 RID: 48806
				public static LocString NAME = "Low Battery";

				// Token: 0x0400BEA7 RID: 48807
				public static LocString DESC = "{0}'s battery is low and needs to recharge";

				// Token: 0x0400BEA8 RID: 48808
				public static LocString TOOLTIP = "{0}'s battery is low and needs to recharge";
			}

			// Token: 0x02002E80 RID: 11904
			public class LOWBATTERYNOCHARGE
			{
				// Token: 0x0400BEA9 RID: 48809
				public static LocString NAME = "Low Battery";

				// Token: 0x0400BEAA RID: 48810
				public static LocString DESC = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";

				// Token: 0x0400BEAB RID: 48811
				public static LocString TOOLTIP = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
			}

			// Token: 0x02002E81 RID: 11905
			public class DEADBATTERY
			{
				// Token: 0x0400BEAC RID: 48812
				public static LocString NAME = "Shut Down";

				// Token: 0x0400BEAD RID: 48813
				public static LocString DESC = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";

				// Token: 0x0400BEAE RID: 48814
				public static LocString TOOLTIP = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
			}

			// Token: 0x02002E82 RID: 11906
			public class DUSTBINFULL
			{
				// Token: 0x0400BEAF RID: 48815
				public static LocString NAME = "Dust Bin Full";

				// Token: 0x0400BEB0 RID: 48816
				public static LocString DESC = "{0} must return to its dock to unload";

				// Token: 0x0400BEB1 RID: 48817
				public static LocString TOOLTIP = "{0} must return to its dock to unload";
			}

			// Token: 0x02002E83 RID: 11907
			public class WORKING
			{
				// Token: 0x0400BEB2 RID: 48818
				public static LocString NAME = "Working";

				// Token: 0x0400BEB3 RID: 48819
				public static LocString DESC = "{0} is working diligently. Great job, {0}!";

				// Token: 0x0400BEB4 RID: 48820
				public static LocString TOOLTIP = "{0} is working diligently. Great job, {0}!";
			}

			// Token: 0x02002E84 RID: 11908
			public class UNLOADINGSTORAGE
			{
				// Token: 0x0400BEB5 RID: 48821
				public static LocString NAME = "Unloading";

				// Token: 0x0400BEB6 RID: 48822
				public static LocString DESC = "{0} is emptying out its dust bin";

				// Token: 0x0400BEB7 RID: 48823
				public static LocString TOOLTIP = "{0} is emptying out its dust bin";
			}

			// Token: 0x02002E85 RID: 11909
			public class CHARGING
			{
				// Token: 0x0400BEB8 RID: 48824
				public static LocString NAME = "Charging";

				// Token: 0x0400BEB9 RID: 48825
				public static LocString DESC = "{0} is recharging its battery";

				// Token: 0x0400BEBA RID: 48826
				public static LocString TOOLTIP = "{0} is recharging its battery";
			}

			// Token: 0x02002E86 RID: 11910
			public class REACTPOSITIVE
			{
				// Token: 0x0400BEBB RID: 48827
				public static LocString NAME = "Happy Reaction";

				// Token: 0x0400BEBC RID: 48828
				public static LocString DESC = "This bot saw something nice!";

				// Token: 0x0400BEBD RID: 48829
				public static LocString TOOLTIP = "This bot saw something nice!";
			}

			// Token: 0x02002E87 RID: 11911
			public class REACTNEGATIVE
			{
				// Token: 0x0400BEBE RID: 48830
				public static LocString NAME = "Bothered Reaction";

				// Token: 0x0400BEBF RID: 48831
				public static LocString DESC = "This bot saw something upsetting";

				// Token: 0x0400BEC0 RID: 48832
				public static LocString TOOLTIP = "This bot saw something upsetting";
			}
		}

		// Token: 0x02001E12 RID: 7698
		public class MODELS
		{
			// Token: 0x02002E88 RID: 11912
			public class SCOUT
			{
				// Token: 0x0400BEC1 RID: 48833
				public static LocString NAME = "Rover";

				// Token: 0x0400BEC2 RID: 48834
				public static LocString DESC = "A curious bot that can remotely explore new " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " locations.";
			}

			// Token: 0x02002E89 RID: 11913
			public class SWEEPBOT
			{
				// Token: 0x0400BEC3 RID: 48835
				public static LocString NAME = "Sweepy";

				// Token: 0x0400BEC4 RID: 48836
				public static LocString DESC = string.Concat(new string[]
				{
					"An automated sweeping robot.\n\nSweeps up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills and stores the material back in its ",
					UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION"),
					"."
				});
			}
		}
	}
}
