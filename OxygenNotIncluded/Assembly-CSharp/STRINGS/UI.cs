using System;
using System.Collections.Generic;

namespace STRINGS
{
	// Token: 0x02000DA8 RID: 3496
	public class UI
	{
		// Token: 0x06006C3B RID: 27707 RVA: 0x002AC322 File Offset: 0x002AA522
		public static string FormatAsBuildMenuTab(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x002AC334 File Offset: 0x002AA534
		public static string FormatAsBuildMenuTab(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x002AC34C File Offset: 0x002AA54C
		public static string FormatAsBuildMenuTab(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x002AC364 File Offset: 0x002AA564
		public static string FormatAsOverlay(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06006C3F RID: 27711 RVA: 0x002AC376 File Offset: 0x002AA576
		public static string FormatAsOverlay(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06006C40 RID: 27712 RVA: 0x002AC38E File Offset: 0x002AA58E
		public static string FormatAsOverlay(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x002AC3A6 File Offset: 0x002AA5A6
		public static string FormatAsManagementMenu(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x002AC3B8 File Offset: 0x002AA5B8
		public static string FormatAsManagementMenu(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x002AC3D0 File Offset: 0x002AA5D0
		public static string FormatAsManagementMenu(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06006C44 RID: 27716 RVA: 0x002AC3E8 File Offset: 0x002AA5E8
		public static string FormatAsKeyWord(string text)
		{
			return UI.PRE_KEYWORD + text + UI.PST_KEYWORD;
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x002AC3FA File Offset: 0x002AA5FA
		public static string FormatAsHotkey(string text)
		{
			return "<b><color=#F44A4A>" + text + "</b></color>";
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x002AC40C File Offset: 0x002AA60C
		public static string FormatAsHotKey(global::Action a)
		{
			return "{Hotkey/" + a.ToString() + "}";
		}

		// Token: 0x06006C47 RID: 27719 RVA: 0x002AC42A File Offset: 0x002AA62A
		public static string FormatAsTool(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06006C48 RID: 27720 RVA: 0x002AC442 File Offset: 0x002AA642
		public static string FormatAsTool(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x002AC45A File Offset: 0x002AA65A
		public static string FormatAsLink(string text, string linkID)
		{
			text = UI.StripLinkFormatting(text);
			linkID = CodexCache.FormatLinkID(linkID);
			return string.Concat(new string[]
			{
				"<link=\"",
				linkID,
				"\">",
				text,
				"</link>"
			});
		}

		// Token: 0x06006C4A RID: 27722 RVA: 0x002AC497 File Offset: 0x002AA697
		public static string FormatAsPositiveModifier(string text)
		{
			return UI.PRE_POS_MODIFIER + text + UI.PST_POS_MODIFIER;
		}

		// Token: 0x06006C4B RID: 27723 RVA: 0x002AC4A9 File Offset: 0x002AA6A9
		public static string FormatAsNegativeModifier(string text)
		{
			return UI.PRE_NEG_MODIFIER + text + UI.PST_NEG_MODIFIER;
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x002AC4BB File Offset: 0x002AA6BB
		public static string FormatAsPositiveRate(string text)
		{
			return UI.PRE_RATE_POSITIVE + text + UI.PST_RATE;
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x002AC4CD File Offset: 0x002AA6CD
		public static string FormatAsNegativeRate(string text)
		{
			return UI.PRE_RATE_NEGATIVE + text + UI.PST_RATE;
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x002AC4DF File Offset: 0x002AA6DF
		public static string CLICK(UI.ClickType c)
		{
			return "(ClickType/" + c.ToString() + ")";
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x002AC4FD File Offset: 0x002AA6FD
		public static string FormatAsAutomationState(string text, UI.AutomationState state)
		{
			if (state == UI.AutomationState.Active)
			{
				return UI.PRE_AUTOMATION_ACTIVE + text + UI.PST_AUTOMATION;
			}
			return UI.PRE_AUTOMATION_STANDBY + text + UI.PST_AUTOMATION;
		}

		// Token: 0x06006C50 RID: 27728 RVA: 0x002AC523 File Offset: 0x002AA723
		public static string FormatAsCaps(string text)
		{
			return text.ToUpper();
		}

		// Token: 0x06006C51 RID: 27729 RVA: 0x002AC52C File Offset: 0x002AA72C
		public static string ExtractLinkID(string text)
		{
			string text2 = text;
			int num = text2.IndexOf("<link=");
			if (num != -1)
			{
				int num2 = num + 7;
				int num3 = text2.IndexOf(">") - 1;
				text2 = text.Substring(num2, num3 - num2);
			}
			return text2;
		}

		// Token: 0x06006C52 RID: 27730 RVA: 0x002AC56C File Offset: 0x002AA76C
		public static string StripLinkFormatting(string text)
		{
			string text2 = text;
			try
			{
				while (text2.Contains("<link="))
				{
					int num = text2.IndexOf("</link>");
					if (num > -1)
					{
						text2 = text2.Remove(num, 7);
					}
					else
					{
						Debug.LogWarningFormat("String has no closing link tag: {0}", Array.Empty<object>());
					}
					int num2 = text2.IndexOf("<link=");
					if (num2 != -1)
					{
						text2 = text2.Remove(num2, 7);
					}
					else
					{
						Debug.LogWarningFormat("String has no open link tag: {0}", Array.Empty<object>());
					}
					int num3 = text2.IndexOf("\">");
					if (num3 != -1)
					{
						text2 = text2.Remove(num2, num3 - num2 + 2);
					}
					else
					{
						Debug.LogWarningFormat("String has no open link tag: {0}", Array.Empty<object>());
					}
				}
			}
			catch
			{
				Debug.Log("STRIP LINK FORMATTING FAILED ON: " + text);
				text2 = text;
			}
			return text2;
		}

		// Token: 0x04005083 RID: 20611
		public static string PRE_KEYWORD = "<style=\"KKeyword\">";

		// Token: 0x04005084 RID: 20612
		public static string PST_KEYWORD = "</style>";

		// Token: 0x04005085 RID: 20613
		public static string PRE_POS_MODIFIER = "<b>";

		// Token: 0x04005086 RID: 20614
		public static string PST_POS_MODIFIER = "</b>";

		// Token: 0x04005087 RID: 20615
		public static string PRE_NEG_MODIFIER = "<b>";

		// Token: 0x04005088 RID: 20616
		public static string PST_NEG_MODIFIER = "</b>";

		// Token: 0x04005089 RID: 20617
		public static string PRE_RATE_NEGATIVE = "<style=\"consumed\">";

		// Token: 0x0400508A RID: 20618
		public static string PRE_RATE_POSITIVE = "<style=\"produced\">";

		// Token: 0x0400508B RID: 20619
		public static string PST_RATE = "</style>";

		// Token: 0x0400508C RID: 20620
		public static string PRE_AUTOMATION_ACTIVE = "<b><style=\"logic_on\">";

		// Token: 0x0400508D RID: 20621
		public static string PRE_AUTOMATION_STANDBY = "<b><style=\"logic_off\">";

		// Token: 0x0400508E RID: 20622
		public static string PST_AUTOMATION = "</style></b>";

		// Token: 0x0400508F RID: 20623
		public static string YELLOW_PREFIX = "<color=#ffff00ff>";

		// Token: 0x04005090 RID: 20624
		public static string COLOR_SUFFIX = "</color>";

		// Token: 0x04005091 RID: 20625
		public static string HORIZONTAL_RULE = "------------------";

		// Token: 0x04005092 RID: 20626
		public static string HORIZONTAL_BR_RULE = "\n" + UI.HORIZONTAL_RULE + "\n";

		// Token: 0x04005093 RID: 20627
		public static LocString POS_INFINITY = "Infinity";

		// Token: 0x04005094 RID: 20628
		public static LocString NEG_INFINITY = "-Infinity";

		// Token: 0x04005095 RID: 20629
		public static LocString PROCEED_BUTTON = "PROCEED";

		// Token: 0x04005096 RID: 20630
		public static LocString COPY_BUILDING = "Copy";

		// Token: 0x04005097 RID: 20631
		public static LocString COPY_BUILDING_TOOLTIP = "Create new build orders using the most recent building selection as a template. {Hotkey}";

		// Token: 0x04005098 RID: 20632
		public static LocString NAME_WITH_UNITS = "{0} x {1}";

		// Token: 0x04005099 RID: 20633
		public static LocString NA = "N/A";

		// Token: 0x0400509A RID: 20634
		public static LocString POSITIVE_FORMAT = "+{0}";

		// Token: 0x0400509B RID: 20635
		public static LocString NEGATIVE_FORMAT = "-{0}";

		// Token: 0x0400509C RID: 20636
		public static LocString FILTER = "Filter";

		// Token: 0x0400509D RID: 20637
		public static LocString SPEED_SLOW = "SLOW";

		// Token: 0x0400509E RID: 20638
		public static LocString SPEED_MEDIUM = "MEDIUM";

		// Token: 0x0400509F RID: 20639
		public static LocString SPEED_FAST = "FAST";

		// Token: 0x040050A0 RID: 20640
		public static LocString RED_ALERT = "RED ALERT";

		// Token: 0x040050A1 RID: 20641
		public static LocString JOBS = "PRIORITIES";

		// Token: 0x040050A2 RID: 20642
		public static LocString CONSUMABLES = "CONSUMABLES";

		// Token: 0x040050A3 RID: 20643
		public static LocString VITALS = "VITALS";

		// Token: 0x040050A4 RID: 20644
		public static LocString RESEARCH = "RESEARCH";

		// Token: 0x040050A5 RID: 20645
		public static LocString ROLES = "JOB ASSIGNMENTS";

		// Token: 0x040050A6 RID: 20646
		public static LocString RESEARCHPOINTS = "Research points";

		// Token: 0x040050A7 RID: 20647
		public static LocString SCHEDULE = "SCHEDULE";

		// Token: 0x040050A8 RID: 20648
		public static LocString REPORT = "REPORTS";

		// Token: 0x040050A9 RID: 20649
		public static LocString SKILLS = "SKILLS";

		// Token: 0x040050AA RID: 20650
		public static LocString OVERLAYSTITLE = "OVERLAYS";

		// Token: 0x040050AB RID: 20651
		public static LocString ALERTS = "ALERTS";

		// Token: 0x040050AC RID: 20652
		public static LocString MESSAGES = "MESSAGES";

		// Token: 0x040050AD RID: 20653
		public static LocString ACTIONS = "ACTIONS";

		// Token: 0x040050AE RID: 20654
		public static LocString QUEUE = "Queue";

		// Token: 0x040050AF RID: 20655
		public static LocString BASECOUNT = "Base {0}";

		// Token: 0x040050B0 RID: 20656
		public static LocString CHARACTERCONTAINER_SKILLS_TITLE = "ATTRIBUTES";

		// Token: 0x040050B1 RID: 20657
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE = "TRAITS";

		// Token: 0x040050B2 RID: 20658
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE = "INTERESTS";

		// Token: 0x040050B3 RID: 20659
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE_TOOLTIP = "A Duplicant's starting Attributes are determined by their Interests\n\nLearning Skills related to their Interests will give Duplicants a Morale Boost";

		// Token: 0x040050B4 RID: 20660
		public static LocString CHARACTERCONTAINER_EXPECTATIONS_TITLE = "ADDITIONAL INFORMATION";

		// Token: 0x040050B5 RID: 20661
		public static LocString CHARACTERCONTAINER_SKILL_VALUE = " {0} {1}";

		// Token: 0x040050B6 RID: 20662
		public static LocString CHARACTERCONTAINER_NEED = "{0}: {1}";

		// Token: 0x040050B7 RID: 20663
		public static LocString CHARACTERCONTAINER_STRESSTRAIT = "Stress Reaction: {0}";

		// Token: 0x040050B8 RID: 20664
		public static LocString CHARACTERCONTAINER_JOYTRAIT = "Overjoyed Response: {0}";

		// Token: 0x040050B9 RID: 20665
		public static LocString CHARACTERCONTAINER_CONGENITALTRAIT = "Genetic Trait: {0}";

		// Token: 0x040050BA RID: 20666
		public static LocString CHARACTERCONTAINER_NOARCHETYPESELECTED = "Random";

		// Token: 0x040050BB RID: 20667
		public static LocString CHARACTERCONTAINER_ARCHETYPESELECT_TOOLTIP = "Influence what type of Duplicant the reroll button will produce";

		// Token: 0x040050BC RID: 20668
		public static LocString CAREPACKAGECONTAINER_INFORMATION_TITLE = "CARE PACKAGE";

		// Token: 0x040050BD RID: 20669
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED = "Increased <b>{0}</b>";

		// Token: 0x040050BE RID: 20670
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED = "Decreased <b>{0}</b>";

		// Token: 0x040050BF RID: 20671
		public static LocString PRODUCTINFO_SELECTMATERIAL = "Select {0}:";

		// Token: 0x040050C0 RID: 20672
		public static LocString PRODUCTINFO_RESEARCHREQUIRED = "Research required...";

		// Token: 0x040050C1 RID: 20673
		public static LocString PRODUCTINFO_REQUIRESRESEARCHDESC = "Requires {0} Research";

		// Token: 0x040050C2 RID: 20674
		public static LocString PRODUCTINFO_APPLICABLERESOURCES = "Required resources:";

		// Token: 0x040050C3 RID: 20675
		public static LocString PRODUCTINFO_MISSINGRESOURCES_TITLE = "Requires {0}: {1}";

		// Token: 0x040050C4 RID: 20676
		public static LocString PRODUCTINFO_MISSINGRESOURCES_HOVER = "Missing resources";

		// Token: 0x040050C5 RID: 20677
		public static LocString PRODUCTINFO_MISSINGRESOURCES_DESC = "{0} has yet to be discovered";

		// Token: 0x040050C6 RID: 20678
		public static LocString PRODUCTINFO_UNIQUE_PER_WORLD = "Limit one per " + UI.CLUSTERMAP.PLANETOID_KEYWORD;

		// Token: 0x040050C7 RID: 20679
		public static LocString PRODUCTINFO_ROCKET_INTERIOR = "Rocket interior only";

		// Token: 0x040050C8 RID: 20680
		public static LocString PRODUCTINFO_ROCKET_NOT_INTERIOR = "Cannot build inside rocket";

		// Token: 0x040050C9 RID: 20681
		public static LocString BUILDTOOL_ROTATE = "Rotate this building";

		// Token: 0x040050CA RID: 20682
		public static LocString BUILDTOOL_ROTATE_CURRENT_DEGREES = "Currently rotated {Degrees} degrees";

		// Token: 0x040050CB RID: 20683
		public static LocString BUILDTOOL_ROTATE_CURRENT_LEFT = "Currently facing left";

		// Token: 0x040050CC RID: 20684
		public static LocString BUILDTOOL_ROTATE_CURRENT_RIGHT = "Currently facing right";

		// Token: 0x040050CD RID: 20685
		public static LocString BUILDTOOL_ROTATE_CURRENT_UP = "Currently facing up";

		// Token: 0x040050CE RID: 20686
		public static LocString BUILDTOOL_ROTATE_CURRENT_DOWN = "Currently facing down";

		// Token: 0x040050CF RID: 20687
		public static LocString BUILDTOOL_ROTATE_CURRENT_UPRIGHT = "Currently upright";

		// Token: 0x040050D0 RID: 20688
		public static LocString BUILDTOOL_ROTATE_CURRENT_ON_SIDE = "Currently on its side";

		// Token: 0x040050D1 RID: 20689
		public static LocString BUILDTOOL_CANT_ROTATE = "This building cannot be rotated";

		// Token: 0x040050D2 RID: 20690
		public static LocString EQUIPMENTTAB_OWNED = "Owned Items";

		// Token: 0x040050D3 RID: 20691
		public static LocString EQUIPMENTTAB_HELD = "Held Items";

		// Token: 0x040050D4 RID: 20692
		public static LocString EQUIPMENTTAB_ROOM = "Assigned Rooms";

		// Token: 0x040050D5 RID: 20693
		public static LocString JOBSCREEN_PRIORITY = "Priority";

		// Token: 0x040050D6 RID: 20694
		public static LocString JOBSCREEN_HIGH = "High";

		// Token: 0x040050D7 RID: 20695
		public static LocString JOBSCREEN_LOW = "Low";

		// Token: 0x040050D8 RID: 20696
		public static LocString JOBSCREEN_EVERYONE = "Everyone";

		// Token: 0x040050D9 RID: 20697
		public static LocString JOBSCREEN_DEFAULT = "New Duplicants";

		// Token: 0x040050DA RID: 20698
		public static LocString BUILD_REQUIRES_SKILL = "Skill: {Skill}";

		// Token: 0x040050DB RID: 20699
		public static LocString BUILD_REQUIRES_SKILL_TOOLTIP = "At least one Duplicant must have the {Skill} Skill to construct this building";

		// Token: 0x040050DC RID: 20700
		public static LocString VITALSSCREEN_NAME = "Name";

		// Token: 0x040050DD RID: 20701
		public static LocString VITALSSCREEN_STRESS = "Stress";

		// Token: 0x040050DE RID: 20702
		public static LocString VITALSSCREEN_HEALTH = "Health";

		// Token: 0x040050DF RID: 20703
		public static LocString VITALSSCREEN_SICKNESS = "Disease";

		// Token: 0x040050E0 RID: 20704
		public static LocString VITALSSCREEN_CALORIES = "Fullness";

		// Token: 0x040050E1 RID: 20705
		public static LocString VITALSSCREEN_RATIONS = "Calories / Cycle";

		// Token: 0x040050E2 RID: 20706
		public static LocString VITALSSCREEN_EATENTODAY = "Eaten Today";

		// Token: 0x040050E3 RID: 20707
		public static LocString VITALSSCREEN_RATIONS_TOOLTIP = "Set how many calories this Duplicant may consume daily";

		// Token: 0x040050E4 RID: 20708
		public static LocString VITALSSCREEN_EATENTODAY_TOOLTIP = "The amount of food this Duplicant has eaten this cycle";

		// Token: 0x040050E5 RID: 20709
		public static LocString VITALSSCREEN_UNTIL_FULL = "Until Full";

		// Token: 0x040050E6 RID: 20710
		public static LocString RESEARCHSCREEN_UNLOCKSTOOLTIP = "Unlocks: {0}";

		// Token: 0x040050E7 RID: 20711
		public static LocString RESEARCHSCREEN_FILTER = "Search Tech";

		// Token: 0x040050E8 RID: 20712
		public static LocString ATTRIBUTELEVEL = "Expertise: Level {0} {1}";

		// Token: 0x040050E9 RID: 20713
		public static LocString ATTRIBUTELEVEL_SHORT = "Level {0} {1}";

		// Token: 0x040050EA RID: 20714
		public static LocString NEUTRONIUMMASS = "Immeasurable";

		// Token: 0x040050EB RID: 20715
		public static LocString CALCULATING = "Calculating...";

		// Token: 0x040050EC RID: 20716
		public static LocString FORMATDAY = "{0} cycles";

		// Token: 0x040050ED RID: 20717
		public static LocString FORMATSECONDS = "{0}s";

		// Token: 0x040050EE RID: 20718
		public static LocString DELIVERED = "Delivered: {0} {1}";

		// Token: 0x040050EF RID: 20719
		public static LocString PICKEDUP = "Picked Up: {0} {1}";

		// Token: 0x040050F0 RID: 20720
		public static LocString COPIED_SETTINGS = "Settings Applied";

		// Token: 0x040050F1 RID: 20721
		public static LocString WELCOMEMESSAGETITLE = "- ALERT -";

		// Token: 0x040050F2 RID: 20722
		public static LocString WELCOMEMESSAGEBODY = "I've awoken at the target location, but colonization efforts have already hit a hitch. I was supposed to land on the planet's surface, but became trapped many miles underground instead.\n\nAlthough the conditions are not ideal, it's imperative that I establish a colony here and begin mounting efforts to escape.";

		// Token: 0x040050F3 RID: 20723
		public static LocString WELCOMEMESSAGEBODY_SPACEDOUT = "The asteroid we call home has collided with an anomalous planet, decimating our colony. Rebuilding it is of the utmost importance.\n\nI've detected a new cluster of material-rich planetoids in nearby space. If I can guide the Duplicants through the perils of space travel, we could build a colony even bigger and better than before.";

		// Token: 0x040050F4 RID: 20724
		public static LocString WELCOMEMESSAGEBODY_KF23 = "This asteroid is oddly tilted, as though a powerful external force once knocked it off its axis.\n\nI'll need to recalibrate my approach to colony-building in order to make the most of this unusual distribution of resources.";

		// Token: 0x040050F5 RID: 20725
		public static LocString WELCOMEMESSAGEBEGIN = "BEGIN";

		// Token: 0x040050F6 RID: 20726
		public static LocString VIEWDUPLICANTS = "Choose a Blueprint";

		// Token: 0x040050F7 RID: 20727
		public static LocString DUPLICANTPRINTING = "Duplicant Printing";

		// Token: 0x040050F8 RID: 20728
		public static LocString ASSIGNDUPLICANT = "Assign Duplicant";

		// Token: 0x040050F9 RID: 20729
		public static LocString CRAFT = "ADD TO QUEUE";

		// Token: 0x040050FA RID: 20730
		public static LocString CLEAR_COMPLETED = "CLEAR COMPLETED ORDERS";

		// Token: 0x040050FB RID: 20731
		public static LocString CRAFT_CONTINUOUS = "CONTINUOUS";

		// Token: 0x040050FC RID: 20732
		public static LocString INCUBATE_CONTINUOUS_TOOLTIP = "When checked, this building will continuously incubate eggs of the selected type";

		// Token: 0x040050FD RID: 20733
		public static LocString PLACEINRECEPTACLE = "Plant";

		// Token: 0x040050FE RID: 20734
		public static LocString REMOVEFROMRECEPTACLE = "Uproot";

		// Token: 0x040050FF RID: 20735
		public static LocString CANCELPLACEINRECEPTACLE = "Cancel";

		// Token: 0x04005100 RID: 20736
		public static LocString CANCELREMOVALFROMRECEPTACLE = "Cancel";

		// Token: 0x04005101 RID: 20737
		public static LocString CHANGEPERSECOND = "Change per second: {0}";

		// Token: 0x04005102 RID: 20738
		public static LocString CHANGEPERCYCLE = "Total change per cycle: {0}";

		// Token: 0x04005103 RID: 20739
		public static LocString MODIFIER_ITEM_TEMPLATE = "    • {0}: {1}";

		// Token: 0x04005104 RID: 20740
		public static LocString LISTENTRYSTRING = "     {0}\n";

		// Token: 0x04005105 RID: 20741
		public static LocString LISTENTRYSTRINGNOLINEBREAK = "     {0}";

		// Token: 0x02001D86 RID: 7558
		public static class PLATFORMS
		{
			// Token: 0x0400853E RID: 34110
			public static LocString UNKNOWN = "Your game client";

			// Token: 0x0400853F RID: 34111
			public static LocString STEAM = "Steam";

			// Token: 0x04008540 RID: 34112
			public static LocString EPIC = "Epic Games Store";

			// Token: 0x04008541 RID: 34113
			public static LocString WEGAME = "Wegame";
		}

		// Token: 0x02001D87 RID: 7559
		private enum KeywordType
		{
			// Token: 0x04008543 RID: 34115
			Hotkey,
			// Token: 0x04008544 RID: 34116
			BuildMenu,
			// Token: 0x04008545 RID: 34117
			Attribute,
			// Token: 0x04008546 RID: 34118
			Generic
		}

		// Token: 0x02001D88 RID: 7560
		public enum ClickType
		{
			// Token: 0x04008548 RID: 34120
			Click,
			// Token: 0x04008549 RID: 34121
			Clicked,
			// Token: 0x0400854A RID: 34122
			Clicking,
			// Token: 0x0400854B RID: 34123
			Clickable,
			// Token: 0x0400854C RID: 34124
			Clicks,
			// Token: 0x0400854D RID: 34125
			click,
			// Token: 0x0400854E RID: 34126
			clicked,
			// Token: 0x0400854F RID: 34127
			clicking,
			// Token: 0x04008550 RID: 34128
			clickable,
			// Token: 0x04008551 RID: 34129
			clicks,
			// Token: 0x04008552 RID: 34130
			CLICK,
			// Token: 0x04008553 RID: 34131
			CLICKED,
			// Token: 0x04008554 RID: 34132
			CLICKING,
			// Token: 0x04008555 RID: 34133
			CLICKABLE,
			// Token: 0x04008556 RID: 34134
			CLICKS
		}

		// Token: 0x02001D89 RID: 7561
		public enum AutomationState
		{
			// Token: 0x04008558 RID: 34136
			Active,
			// Token: 0x04008559 RID: 34137
			Standby
		}

		// Token: 0x02001D8A RID: 7562
		public class VANILLA
		{
			// Token: 0x0400855A RID: 34138
			public static LocString NAME = "base game";

			// Token: 0x0400855B RID: 34139
			public static LocString NAME_ITAL = "<i>" + UI.VANILLA.NAME + "</i>";
		}

		// Token: 0x02001D8B RID: 7563
		public class DLC1
		{
			// Token: 0x0400855C RID: 34140
			public static LocString NAME = "Spaced Out!";

			// Token: 0x0400855D RID: 34141
			public static LocString NAME_ITAL = "<i>" + UI.DLC1.NAME + "</i>";
		}

		// Token: 0x02001D8C RID: 7564
		public class DIAGNOSTICS_SCREEN
		{
			// Token: 0x0400855E RID: 34142
			public static LocString TITLE = "Diagnostics";

			// Token: 0x0400855F RID: 34143
			public static LocString DIAGNOSTIC = "Diagnostic";

			// Token: 0x04008560 RID: 34144
			public static LocString TOTAL = "Total";

			// Token: 0x04008561 RID: 34145
			public static LocString RESERVED = "Reserved";

			// Token: 0x04008562 RID: 34146
			public static LocString STATUS = "Status";

			// Token: 0x04008563 RID: 34147
			public static LocString SEARCH = "Search";

			// Token: 0x04008564 RID: 34148
			public static LocString CRITERIA_HEADER_TOOLTIP = "Expand or collapse diagnostic criteria panel";

			// Token: 0x04008565 RID: 34149
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x04008566 RID: 34150
			public static LocString CRITERIA_TOOLTIP = "Toggle the <b>{0}</b> diagnostics evaluation of the <b>{1}</b> criteria.";

			// Token: 0x04008567 RID: 34151
			public static LocString CRITERIA_ENABLED_COUNT = "{0}/{1} criteria enabled";

			// Token: 0x02002519 RID: 9497
			public class CLICK_TOGGLE_MESSAGE
			{
				// Token: 0x0400A28F RID: 41615
				public static LocString ALWAYS = UI.CLICK(UI.ClickType.Click) + " to pin this diagnostic to the sidebar - Current State: <b>Visible On Alert Only</b>";

				// Token: 0x0400A290 RID: 41616
				public static LocString ALERT_ONLY = UI.CLICK(UI.ClickType.Click) + " to subscribe to this diagnostic - Current State: <b>Never Visible</b>";

				// Token: 0x0400A291 RID: 41617
				public static LocString NEVER = UI.CLICK(UI.ClickType.Click) + " to mute this diagnostic on the sidebar - Current State: <b>Always Visible</b>";

				// Token: 0x0400A292 RID: 41618
				public static LocString TUTORIAL_DISABLED = UI.CLICK(UI.ClickType.Click) + " to enable this diagnostic -  Current State: <b>Temporarily disabled</b>";
			}
		}

		// Token: 0x02001D8D RID: 7565
		public class WORLD_SELECTOR_SCREEN
		{
			// Token: 0x04008568 RID: 34152
			public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
		}

		// Token: 0x02001D8E RID: 7566
		public class COLONY_DIAGNOSTICS
		{
			// Token: 0x04008569 RID: 34153
			public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid";

			// Token: 0x0400856A RID: 34154
			public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket";

			// Token: 0x0400856B RID: 34155
			public static LocString ROCKET = "rocket";

			// Token: 0x0400856C RID: 34156
			public static LocString NO_MINIONS_REQUESTED = "    • Crew must be requested to update this diagnostic";

			// Token: 0x0400856D RID: 34157
			public static LocString NO_DATA = "    • Not enough data for evaluation";

			// Token: 0x0400856E RID: 34158
			public static LocString NO_DATA_SHORT = "    • No data";

			// Token: 0x0400856F RID: 34159
			public static LocString MUTE_TUTORIAL = "Diagnostic can be muted in the <b><color=#E5B000>See All</color></b> panel";

			// Token: 0x04008570 RID: 34160
			public static LocString GENERIC_STATUS_NORMAL = "All values nominal";

			// Token: 0x04008571 RID: 34161
			public static LocString PLACEHOLDER_CRITERIA_NAME = "Placeholder Criteria Name";

			// Token: 0x04008572 RID: 34162
			public static LocString GENERIC_CRITERIA_PASS = "Criteria met";

			// Token: 0x04008573 RID: 34163
			public static LocString GENERIC_CRITERIA_FAIL = "Criteria not met";

			// Token: 0x0200251A RID: 9498
			public class GENERIC_CRITERIA
			{
				// Token: 0x0400A293 RID: 41619
				public static LocString CHECKWORLDHASMINIONS = "Check world has Duplicants";
			}

			// Token: 0x0200251B RID: 9499
			public class IDLEDIAGNOSTIC
			{
				// Token: 0x0400A294 RID: 41620
				public static LocString ALL_NAME = "Idleness";

				// Token: 0x0400A295 RID: 41621
				public static LocString TOOLTIP_NAME = "<b>Idleness</b>";

				// Token: 0x0400A296 RID: 41622
				public static LocString NORMAL = "    • All Duplicants currently have tasks";

				// Token: 0x0400A297 RID: 41623
				public static LocString IDLE = "    • One or more Duplicants are idle";

				// Token: 0x02002FF7 RID: 12279
				public static class CRITERIA
				{
					// Token: 0x0400C2D1 RID: 49873
					public static LocString CHECKIDLE = "Check idle";
				}
			}

			// Token: 0x0200251C RID: 9500
			public class CHOREGROUPDIAGNOSTIC
			{
				// Token: 0x0400A298 RID: 41624
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02002FF8 RID: 12280
				public static class CRITERIA
				{
				}
			}

			// Token: 0x0200251D RID: 9501
			public class ALLCHORESDIAGNOSTIC
			{
				// Token: 0x0400A299 RID: 41625
				public static LocString ALL_NAME = "Errands";

				// Token: 0x0400A29A RID: 41626
				public static LocString TOOLTIP_NAME = "<b>Errands</b>";

				// Token: 0x0400A29B RID: 41627
				public static LocString NORMAL = "    • {0} errands pending or in progress";

				// Token: 0x02002FF9 RID: 12281
				public static class CRITERIA
				{
				}
			}

			// Token: 0x0200251E RID: 9502
			public class WORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400A29C RID: 41628
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02002FFA RID: 12282
				public static class CRITERIA
				{
				}
			}

			// Token: 0x0200251F RID: 9503
			public class ALLWORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400A29D RID: 41629
				public static LocString ALL_NAME = "Work Time";

				// Token: 0x0400A29E RID: 41630
				public static LocString TOOLTIP_NAME = "<b>Work Time</b>";

				// Token: 0x0400A29F RID: 41631
				public static LocString NORMAL = "    • {0} of Duplicant time spent working";

				// Token: 0x02002FFB RID: 12283
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002520 RID: 9504
			public class TRAVEL_TIME
			{
				// Token: 0x0400A2A0 RID: 41632
				public static LocString ALL_NAME = "Travel Time";

				// Token: 0x0400A2A1 RID: 41633
				public static LocString TOOLTIP_NAME = "<b>Travel Time</b>";

				// Token: 0x0400A2A2 RID: 41634
				public static LocString NORMAL = "    • {0} of Duplicant time spent traveling between errands";

				// Token: 0x02002FFC RID: 12284
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002521 RID: 9505
			public class TRAPPEDDUPLICANTDIAGNOSTIC
			{
				// Token: 0x0400A2A3 RID: 41635
				public static LocString ALL_NAME = "Trapped";

				// Token: 0x0400A2A4 RID: 41636
				public static LocString TOOLTIP_NAME = "<b>Trapped</b>";

				// Token: 0x0400A2A5 RID: 41637
				public static LocString NORMAL = "    • No Duplicants are trapped";

				// Token: 0x0400A2A6 RID: 41638
				public static LocString STUCK = "    • One or more Duplicants are trapped";

				// Token: 0x02002FFD RID: 12285
				public static class CRITERIA
				{
					// Token: 0x0400C2D2 RID: 49874
					public static LocString CHECKTRAPPED = "Check Trapped";
				}
			}

			// Token: 0x02002522 RID: 9506
			public class BREATHABILITYDIAGNOSTIC
			{
				// Token: 0x0400A2A7 RID: 41639
				public static LocString ALL_NAME = "Breathability";

				// Token: 0x0400A2A8 RID: 41640
				public static LocString TOOLTIP_NAME = "<b>Breathability</b>";

				// Token: 0x0400A2A9 RID: 41641
				public static LocString NORMAL = "    • Oxygen levels are satisfactory";

				// Token: 0x0400A2AA RID: 41642
				public static LocString POOR = "    • Oxygen is becoming scarce or low pressure";

				// Token: 0x0400A2AB RID: 41643
				public static LocString SUFFOCATING = "    • One or more Duplicants are suffocating";

				// Token: 0x02002FFE RID: 12286
				public static class CRITERIA
				{
					// Token: 0x0400C2D3 RID: 49875
					public static LocString CHECKSUFFOCATION = "Check suffocation";

					// Token: 0x0400C2D4 RID: 49876
					public static LocString CHECKLOWBREATHABILITY = "Check low breathability";
				}
			}

			// Token: 0x02002523 RID: 9507
			public class STRESSDIAGNOSTIC
			{
				// Token: 0x0400A2AC RID: 41644
				public static LocString ALL_NAME = "Max Stress";

				// Token: 0x0400A2AD RID: 41645
				public static LocString TOOLTIP_NAME = "<b>Max Stress</b>";

				// Token: 0x0400A2AE RID: 41646
				public static LocString HIGH_STRESS = "    • One or more Duplicants is suffering high stress";

				// Token: 0x0400A2AF RID: 41647
				public static LocString NORMAL = "    • Duplicants have acceptable stress levels";

				// Token: 0x02002FFF RID: 12287
				public static class CRITERIA
				{
					// Token: 0x0400C2D5 RID: 49877
					public static LocString CHECKSTRESSED = "Check stressed";
				}
			}

			// Token: 0x02002524 RID: 9508
			public class DECORDIAGNOSTIC
			{
				// Token: 0x0400A2B0 RID: 41648
				public static LocString ALL_NAME = "Decor";

				// Token: 0x0400A2B1 RID: 41649
				public static LocString TOOLTIP_NAME = "<b>Decor</b>";

				// Token: 0x0400A2B2 RID: 41650
				public static LocString LOW = "    • Decor levels are low";

				// Token: 0x0400A2B3 RID: 41651
				public static LocString NORMAL = "    • Decor levels are satisfactory";

				// Token: 0x02003000 RID: 12288
				public static class CRITERIA
				{
					// Token: 0x0400C2D6 RID: 49878
					public static LocString CHECKDECOR = "Check decor";
				}
			}

			// Token: 0x02002525 RID: 9509
			public class TOILETDIAGNOSTIC
			{
				// Token: 0x0400A2B4 RID: 41652
				public static LocString ALL_NAME = "Toilets";

				// Token: 0x0400A2B5 RID: 41653
				public static LocString TOOLTIP_NAME = "<b>Toilets</b>";

				// Token: 0x0400A2B6 RID: 41654
				public static LocString NO_TOILETS = "    • Colony has no toilets";

				// Token: 0x0400A2B7 RID: 41655
				public static LocString NO_WORKING_TOILETS = "    • Colony has no working toilets";

				// Token: 0x0400A2B8 RID: 41656
				public static LocString TOILET_URGENT = "    • Duplicants urgently need to use a toilet";

				// Token: 0x0400A2B9 RID: 41657
				public static LocString FEW_TOILETS = "    • Toilet-to-Duplicant ratio is low";

				// Token: 0x0400A2BA RID: 41658
				public static LocString INOPERATIONAL = "    • One or more toilets are out of order";

				// Token: 0x0400A2BB RID: 41659
				public static LocString NORMAL = "    • Colony has adequate working toilets";

				// Token: 0x02003001 RID: 12289
				public static class CRITERIA
				{
					// Token: 0x0400C2D7 RID: 49879
					public static LocString CHECKHASANYTOILETS = "Check has any toilets";

					// Token: 0x0400C2D8 RID: 49880
					public static LocString CHECKENOUGHTOILETS = "Check enough toilets";

					// Token: 0x0400C2D9 RID: 49881
					public static LocString CHECKBLADDERS = "Check Duplicants really need to use the toilet";
				}
			}

			// Token: 0x02002526 RID: 9510
			public class BEDDIAGNOSTIC
			{
				// Token: 0x0400A2BC RID: 41660
				public static LocString ALL_NAME = "Beds";

				// Token: 0x0400A2BD RID: 41661
				public static LocString TOOLTIP_NAME = "<b>Beds</b>";

				// Token: 0x0400A2BE RID: 41662
				public static LocString NORMAL = "    • Colony has adequate bedding";

				// Token: 0x0400A2BF RID: 41663
				public static LocString NOT_ENOUGH_BEDS = "    • One or more Duplicants are missing a bed";

				// Token: 0x0400A2C0 RID: 41664
				public static LocString MISSING_ASSIGNMENT = "    • One or more Duplicants don't have an assigned bed";

				// Token: 0x02003002 RID: 12290
				public static class CRITERIA
				{
					// Token: 0x0400C2DA RID: 49882
					public static LocString CHECKENOUGHBEDS = "Check enough beds";
				}
			}

			// Token: 0x02002527 RID: 9511
			public class FOODDIAGNOSTIC
			{
				// Token: 0x0400A2C1 RID: 41665
				public static LocString ALL_NAME = "Food";

				// Token: 0x0400A2C2 RID: 41666
				public static LocString TOOLTIP_NAME = "<b>Food</b>";

				// Token: 0x0400A2C3 RID: 41667
				public static LocString NORMAL = "    • Food supply is currently adequate";

				// Token: 0x0400A2C4 RID: 41668
				public static LocString LOW_CALORIES = "    • Food-to-Duplicant ratio is low";

				// Token: 0x0400A2C5 RID: 41669
				public static LocString HUNGRY = "    • One or more Duplicants are very hungry";

				// Token: 0x0400A2C6 RID: 41670
				public static LocString NO_FOOD = "    • Duplicants have no food";

				// Token: 0x02003003 RID: 12291
				public class CRITERIA_HAS_FOOD
				{
					// Token: 0x0400C2DB RID: 49883
					public static LocString PASS = "    • Duplicants have food";

					// Token: 0x0400C2DC RID: 49884
					public static LocString FAIL = "    • Duplicants have no food";
				}

				// Token: 0x02003004 RID: 12292
				public static class CRITERIA
				{
					// Token: 0x0400C2DD RID: 49885
					public static LocString CHECKENOUGHFOOD = "Check enough food";

					// Token: 0x0400C2DE RID: 49886
					public static LocString CHECKSTARVATION = "Check starvation";
				}
			}

			// Token: 0x02002528 RID: 9512
			public class FARMDIAGNOSTIC
			{
				// Token: 0x0400A2C7 RID: 41671
				public static LocString ALL_NAME = "Crops";

				// Token: 0x0400A2C8 RID: 41672
				public static LocString TOOLTIP_NAME = "<b>Crops</b>";

				// Token: 0x0400A2C9 RID: 41673
				public static LocString NORMAL = "    • Crops are being grown in sufficient quantity";

				// Token: 0x0400A2CA RID: 41674
				public static LocString NONE = "    • No farm plots";

				// Token: 0x0400A2CB RID: 41675
				public static LocString NONE_PLANTED = "    • No crops planted";

				// Token: 0x0400A2CC RID: 41676
				public static LocString WILTING = "    • One or more crops are wilting";

				// Token: 0x0400A2CD RID: 41677
				public static LocString INOPERATIONAL = "    • One or more farm plots are inoperable";

				// Token: 0x02003005 RID: 12293
				public static class CRITERIA
				{
					// Token: 0x0400C2DF RID: 49887
					public static LocString CHECKHASFARMS = "Check colony has farms";

					// Token: 0x0400C2E0 RID: 49888
					public static LocString CHECKPLANTED = "Check farms are planted";

					// Token: 0x0400C2E1 RID: 49889
					public static LocString CHECKWILTING = "Check crops wilting";

					// Token: 0x0400C2E2 RID: 49890
					public static LocString CHECKOPERATIONAL = "Check farm plots operational";
				}
			}

			// Token: 0x02002529 RID: 9513
			public class POWERUSEDIAGNOSTIC
			{
				// Token: 0x0400A2CE RID: 41678
				public static LocString ALL_NAME = "Power use";

				// Token: 0x0400A2CF RID: 41679
				public static LocString TOOLTIP_NAME = "<b>Power use</b>";

				// Token: 0x0400A2D0 RID: 41680
				public static LocString NORMAL = "    • Power supply is satisfactory";

				// Token: 0x0400A2D1 RID: 41681
				public static LocString OVERLOADED = "    • One or more power grids are damaged";

				// Token: 0x0400A2D2 RID: 41682
				public static LocString SIGNIFICANT_POWER_CHANGE_DETECTED = "Significant power use change detected. (Average:{0}, Current:{1})";

				// Token: 0x0400A2D3 RID: 41683
				public static LocString CIRCUIT_OVER_CAPACITY = "Circuit overloaded {0}/{1}";

				// Token: 0x02003006 RID: 12294
				public static class CRITERIA
				{
					// Token: 0x0400C2E3 RID: 49891
					public static LocString CHECKOVERWATTAGE = "Check circuit overloaded";

					// Token: 0x0400C2E4 RID: 49892
					public static LocString CHECKPOWERUSECHANGE = "Check power use change";
				}
			}

			// Token: 0x0200252A RID: 9514
			public class HEATDIAGNOSTIC
			{
				// Token: 0x0400A2D4 RID: 41684
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME;

				// Token: 0x02003007 RID: 12295
				public static class CRITERIA
				{
					// Token: 0x0400C2E5 RID: 49893
					public static LocString CHECKHEAT = "Check heat";
				}
			}

			// Token: 0x0200252B RID: 9515
			public class BATTERYDIAGNOSTIC
			{
				// Token: 0x0400A2D5 RID: 41685
				public static LocString ALL_NAME = "Battery";

				// Token: 0x0400A2D6 RID: 41686
				public static LocString TOOLTIP_NAME = "<b>Battery</b>";

				// Token: 0x0400A2D7 RID: 41687
				public static LocString NORMAL = "    • All batteries functional";

				// Token: 0x0400A2D8 RID: 41688
				public static LocString NONE = "    • No batteries are connected to a power grid";

				// Token: 0x0400A2D9 RID: 41689
				public static LocString DEAD_BATTERY = "    • One or more batteries have died";

				// Token: 0x0400A2DA RID: 41690
				public static LocString LIMITED_CAPACITY = "    • Low battery capacity relative to power use";

				// Token: 0x02003008 RID: 12296
				public class CRITERIA_CHECK_CAPACITY
				{
					// Token: 0x0400C2E6 RID: 49894
					public static LocString PASS = "";

					// Token: 0x0400C2E7 RID: 49895
					public static LocString FAIL = "";
				}

				// Token: 0x02003009 RID: 12297
				public static class CRITERIA
				{
					// Token: 0x0400C2E8 RID: 49896
					public static LocString CHECKCAPACITY = "Check capacity";

					// Token: 0x0400C2E9 RID: 49897
					public static LocString CHECKDEAD = "Check dead";
				}
			}

			// Token: 0x0200252C RID: 9516
			public class RADIATIONDIAGNOSTIC
			{
				// Token: 0x0400A2DB RID: 41691
				public static LocString ALL_NAME = "Radiation";

				// Token: 0x0400A2DC RID: 41692
				public static LocString TOOLTIP_NAME = "<b>Radiation</b>";

				// Token: 0x0400A2DD RID: 41693
				public static LocString NORMAL = "    • No Radiation concerns";

				// Token: 0x0400A2DE RID: 41694
				public static LocString AVERAGE_RADS = "Avg. {0}";

				// Token: 0x0200300A RID: 12298
				public class CRITERIA_RADIATION_SICKNESS
				{
					// Token: 0x0400C2EA RID: 49898
					public static LocString PASS = "Healthy";

					// Token: 0x0400C2EB RID: 49899
					public static LocString FAIL = "Sick";
				}

				// Token: 0x0200300B RID: 12299
				public class CRITERIA_RADIATION_EXPOSURE
				{
					// Token: 0x0400C2EC RID: 49900
					public static LocString PASS = "Safe exposure levels";

					// Token: 0x0400C2ED RID: 49901
					public static LocString FAIL_CONCERN = "Exposure levels are above safe limits for one or more Duplicants";

					// Token: 0x0400C2EE RID: 49902
					public static LocString FAIL_WARNING = "One or more Duplicants are being exposed to extreme levels of radiation";
				}

				// Token: 0x0200300C RID: 12300
				public static class CRITERIA
				{
					// Token: 0x0400C2EF RID: 49903
					public static LocString CHECKSICK = "Check sick";

					// Token: 0x0400C2F0 RID: 49904
					public static LocString CHECKEXPOSED = "Check exposed";
				}
			}

			// Token: 0x0200252D RID: 9517
			public class METEORDIAGNOSTIC
			{
				// Token: 0x0400A2DF RID: 41695
				public static LocString ALL_NAME = "Meteor Showers";

				// Token: 0x0400A2E0 RID: 41696
				public static LocString TOOLTIP_NAME = "<b>Meteor Showers</b>";

				// Token: 0x0400A2E1 RID: 41697
				public static LocString NORMAL = "    • No meteor showers in progress";

				// Token: 0x0400A2E2 RID: 41698
				public static LocString SHOWER_UNDERWAY = "    • Meteor bombardment underway! {0} remaining";

				// Token: 0x0200300D RID: 12301
				public static class CRITERIA
				{
					// Token: 0x0400C2F1 RID: 49905
					public static LocString CHECKUNDERWAY = "Check meteor bombardment";
				}
			}

			// Token: 0x0200252E RID: 9518
			public class ENTOMBEDDIAGNOSTIC
			{
				// Token: 0x0400A2E3 RID: 41699
				public static LocString ALL_NAME = "Entombed";

				// Token: 0x0400A2E4 RID: 41700
				public static LocString TOOLTIP_NAME = "<b>Entombed</b>";

				// Token: 0x0400A2E5 RID: 41701
				public static LocString NORMAL = "    • No buildings are entombed";

				// Token: 0x0400A2E6 RID: 41702
				public static LocString BUILDING_ENTOMBED = "    • One or more buildings are entombed";

				// Token: 0x0200300E RID: 12302
				public static class CRITERIA
				{
					// Token: 0x0400C2F2 RID: 49906
					public static LocString CHECKENTOMBED = "Check entombed";
				}
			}

			// Token: 0x0200252F RID: 9519
			public class ROCKETFUELDIAGNOSTIC
			{
				// Token: 0x0400A2E7 RID: 41703
				public static LocString ALL_NAME = "Rocket Fuel";

				// Token: 0x0400A2E8 RID: 41704
				public static LocString TOOLTIP_NAME = "<b>Rocket Fuel</b>";

				// Token: 0x0400A2E9 RID: 41705
				public static LocString NORMAL = "    • This rocket has sufficient fuel";

				// Token: 0x0400A2EA RID: 41706
				public static LocString WARNING = "    • This rocket has no fuel";

				// Token: 0x0200300F RID: 12303
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002530 RID: 9520
			public class ROCKETOXIDIZERDIAGNOSTIC
			{
				// Token: 0x0400A2EB RID: 41707
				public static LocString ALL_NAME = "Rocket Oxidizer";

				// Token: 0x0400A2EC RID: 41708
				public static LocString TOOLTIP_NAME = "<b>Rocket Oxidizer</b>";

				// Token: 0x0400A2ED RID: 41709
				public static LocString NORMAL = "    • This rocket has sufficient oxidizer";

				// Token: 0x0400A2EE RID: 41710
				public static LocString WARNING = "    • This rocket has insufficient oxidizer";

				// Token: 0x02003010 RID: 12304
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002531 RID: 9521
			public class REACTORDIAGNOSTIC
			{
				// Token: 0x0400A2EF RID: 41711
				public static LocString ALL_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400A2F0 RID: 41712
				public static LocString TOOLTIP_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400A2F1 RID: 41713
				public static LocString NORMAL = "    • Safe";

				// Token: 0x0400A2F2 RID: 41714
				public static LocString CRITERIA_TEMPERATURE_WARNING = "    • Temperature dangerously high";

				// Token: 0x0400A2F3 RID: 41715
				public static LocString CRITERIA_COOLANT_WARNING = "    • Coolant tank low";

				// Token: 0x02003011 RID: 12305
				public static class CRITERIA
				{
					// Token: 0x0400C2F3 RID: 49907
					public static LocString CHECKTEMPERATURE = "Check temperature";

					// Token: 0x0400C2F4 RID: 49908
					public static LocString CHECKCOOLANT = "Check coolant";
				}
			}

			// Token: 0x02002532 RID: 9522
			public class FLOATINGROCKETDIAGNOSTIC
			{
				// Token: 0x0400A2F4 RID: 41716
				public static LocString ALL_NAME = "Flight Status";

				// Token: 0x0400A2F5 RID: 41717
				public static LocString TOOLTIP_NAME = "<b>Flight Status</b>";

				// Token: 0x0400A2F6 RID: 41718
				public static LocString NORMAL_FLIGHT = "    • This rocket is in flight towards its destination";

				// Token: 0x0400A2F7 RID: 41719
				public static LocString NORMAL_UTILITY = "    • This rocket is performing a task at its destination";

				// Token: 0x0400A2F8 RID: 41720
				public static LocString NORMAL_LANDED = "    • This rocket is currently landed on a " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;

				// Token: 0x0400A2F9 RID: 41721
				public static LocString WARNING_NO_DESTINATION = "    • This rocket is suspended in space with no set destination";

				// Token: 0x0400A2FA RID: 41722
				public static LocString WARNING_NO_SPEED = "    • This rocket's flight has been halted";

				// Token: 0x02003012 RID: 12306
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002533 RID: 9523
			public class ROCKETINORBITDIAGNOSTIC
			{
				// Token: 0x0400A2FB RID: 41723
				public static LocString ALL_NAME = "Rockets in Orbit";

				// Token: 0x0400A2FC RID: 41724
				public static LocString TOOLTIP_NAME = "<b>Rockets in Orbit</b>";

				// Token: 0x0400A2FD RID: 41725
				public static LocString NORMAL_ONE_IN_ORBIT = "    • {0} is in orbit waiting to land";

				// Token: 0x0400A2FE RID: 41726
				public static LocString NORMAL_IN_ORBIT = "    • There are {0} rockets in orbit waiting to land";

				// Token: 0x0400A2FF RID: 41727
				public static LocString WARNING_ONE_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} stranded";

				// Token: 0x0400A300 RID: 41728
				public static LocString WARNING_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} rockets stranded";

				// Token: 0x0400A301 RID: 41729
				public static LocString NORMAL_NO_ROCKETS = "    • No rockets waiting to land";

				// Token: 0x02003013 RID: 12307
				public static class CRITERIA
				{
					// Token: 0x0400C2F5 RID: 49909
					public static LocString CHECKORBIT = "Check Orbiting Rockets";
				}
			}
		}

		// Token: 0x02001D8F RID: 7567
		public class TRACKERS
		{
			// Token: 0x04008574 RID: 34164
			public static LocString BREATHABILITY = "Breathability";

			// Token: 0x04008575 RID: 34165
			public static LocString FOOD = "Food";

			// Token: 0x04008576 RID: 34166
			public static LocString STRESS = "Max Stress";

			// Token: 0x04008577 RID: 34167
			public static LocString IDLE = "Idle Duplicants";
		}

		// Token: 0x02001D90 RID: 7568
		public class CONTROLS
		{
			// Token: 0x04008578 RID: 34168
			public static LocString PRESS = "Press";

			// Token: 0x04008579 RID: 34169
			public static LocString PRESSLOWER = "press";

			// Token: 0x0400857A RID: 34170
			public static LocString PRESSUPPER = "PRESS";

			// Token: 0x0400857B RID: 34171
			public static LocString PRESSING = "Pressing";

			// Token: 0x0400857C RID: 34172
			public static LocString PRESSINGLOWER = "pressing";

			// Token: 0x0400857D RID: 34173
			public static LocString PRESSINGUPPER = "PRESSING";

			// Token: 0x0400857E RID: 34174
			public static LocString PRESSED = "Pressed";

			// Token: 0x0400857F RID: 34175
			public static LocString PRESSEDLOWER = "pressed";

			// Token: 0x04008580 RID: 34176
			public static LocString PRESSEDUPPER = "PRESSED";

			// Token: 0x04008581 RID: 34177
			public static LocString PRESSES = "Presses";

			// Token: 0x04008582 RID: 34178
			public static LocString PRESSESLOWER = "presses";

			// Token: 0x04008583 RID: 34179
			public static LocString PRESSESUPPER = "PRESSES";

			// Token: 0x04008584 RID: 34180
			public static LocString PRESSABLE = "Pressable";

			// Token: 0x04008585 RID: 34181
			public static LocString PRESSABLELOWER = "pressable";

			// Token: 0x04008586 RID: 34182
			public static LocString PRESSABLEUPPER = "PRESSABLE";

			// Token: 0x04008587 RID: 34183
			public static LocString CLICK = "Click";

			// Token: 0x04008588 RID: 34184
			public static LocString CLICKLOWER = "click";

			// Token: 0x04008589 RID: 34185
			public static LocString CLICKUPPER = "CLICK";

			// Token: 0x0400858A RID: 34186
			public static LocString CLICKING = "Clicking";

			// Token: 0x0400858B RID: 34187
			public static LocString CLICKINGLOWER = "clicking";

			// Token: 0x0400858C RID: 34188
			public static LocString CLICKINGUPPER = "CLICKING";

			// Token: 0x0400858D RID: 34189
			public static LocString CLICKED = "Clicked";

			// Token: 0x0400858E RID: 34190
			public static LocString CLICKEDLOWER = "clicked";

			// Token: 0x0400858F RID: 34191
			public static LocString CLICKEDUPPER = "CLICKED";

			// Token: 0x04008590 RID: 34192
			public static LocString CLICKS = "Clicks";

			// Token: 0x04008591 RID: 34193
			public static LocString CLICKSLOWER = "clicks";

			// Token: 0x04008592 RID: 34194
			public static LocString CLICKSUPPER = "CLICKS";

			// Token: 0x04008593 RID: 34195
			public static LocString CLICKABLE = "Clickable";

			// Token: 0x04008594 RID: 34196
			public static LocString CLICKABLELOWER = "clickable";

			// Token: 0x04008595 RID: 34197
			public static LocString CLICKABLEUPPER = "CLICKABLE";
		}

		// Token: 0x02001D91 RID: 7569
		public class MATH_PICTURES
		{
			// Token: 0x02002534 RID: 9524
			public class AXIS_LABELS
			{
				// Token: 0x0400A302 RID: 41730
				public static LocString CYCLES = "Cycles";
			}
		}

		// Token: 0x02001D92 RID: 7570
		public class SPACEDESTINATIONS
		{
			// Token: 0x02002535 RID: 9525
			public class WORMHOLE
			{
				// Token: 0x0400A303 RID: 41731
				public static LocString NAME = "Temporal Tear";

				// Token: 0x0400A304 RID: 41732
				public static LocString DESCRIPTION = "The source of our misfortune, though it may also be our shot at freedom. Traces of Neutronium are detectable in my readings.";
			}

			// Token: 0x02002536 RID: 9526
			public class RESEARCHDESTINATION
			{
				// Token: 0x0400A305 RID: 41733
				public static LocString NAME = "Alluring Anomaly";

				// Token: 0x0400A306 RID: 41734
				public static LocString DESCRIPTION = "Our researchers would have a field day with this if they could only get close enough.";
			}

			// Token: 0x02002537 RID: 9527
			public class DEBRIS
			{
				// Token: 0x02003014 RID: 12308
				public class SATELLITE
				{
					// Token: 0x0400C2F6 RID: 49910
					public static LocString NAME = "Satellite";

					// Token: 0x0400C2F7 RID: 49911
					public static LocString DESCRIPTION = "An artificial construct that has escaped its orbit. It no longer appears to be monitored.";
				}
			}

			// Token: 0x02002538 RID: 9528
			public class NONE
			{
				// Token: 0x0400A307 RID: 41735
				public static LocString NAME = "Unselected";
			}

			// Token: 0x02002539 RID: 9529
			public class ORBIT
			{
				// Token: 0x0400A308 RID: 41736
				public static LocString NAME_FMT = "Orbiting {Name}";
			}

			// Token: 0x0200253A RID: 9530
			public class EMPTY_SPACE
			{
				// Token: 0x0400A309 RID: 41737
				public static LocString NAME = "Empty Space";
			}

			// Token: 0x0200253B RID: 9531
			public class FOG_OF_WAR_SPACE
			{
				// Token: 0x0400A30A RID: 41738
				public static LocString NAME = "Unexplored Space";
			}

			// Token: 0x0200253C RID: 9532
			public class ARTIFACT_POI
			{
				// Token: 0x02003015 RID: 12309
				public class GRAVITASSPACESTATION1
				{
					// Token: 0x0400C2F8 RID: 49912
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400C2F9 RID: 49913
					public static LocString DESC = "The remnants of a bygone era, lost in time.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003016 RID: 12310
				public class GRAVITASSPACESTATION2
				{
					// Token: 0x0400C2FA RID: 49914
					public static LocString NAME = "Demolished Rocket";

					// Token: 0x0400C2FB RID: 49915
					public static LocString DESC = "A defunct rocket from a corporation that vanished long ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003017 RID: 12311
				public class GRAVITASSPACESTATION3
				{
					// Token: 0x0400C2FC RID: 49916
					public static LocString NAME = "Ruined Rocket";

					// Token: 0x0400C2FD RID: 49917
					public static LocString DESC = "The ruins of a rocket that stopped functioning ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003018 RID: 12312
				public class GRAVITASSPACESTATION4
				{
					// Token: 0x0400C2FE RID: 49918
					public static LocString NAME = "Retired Planetary Excursion Module";

					// Token: 0x0400C2FF RID: 49919
					public static LocString DESC = "A rocket part from a society that has been wiped out.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003019 RID: 12313
				public class GRAVITASSPACESTATION5
				{
					// Token: 0x0400C300 RID: 49920
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400C301 RID: 49921
					public static LocString DESC = "A destroyed Gravitas satellite.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200301A RID: 12314
				public class GRAVITASSPACESTATION6
				{
					// Token: 0x0400C302 RID: 49922
					public static LocString NAME = "Annihilated Satellite";

					// Token: 0x0400C303 RID: 49923
					public static LocString DESC = "The remains of a satellite made some time in the past.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200301B RID: 12315
				public class GRAVITASSPACESTATION7
				{
					// Token: 0x0400C304 RID: 49924
					public static LocString NAME = "Wrecked Space Shuttle";

					// Token: 0x0400C305 RID: 49925
					public static LocString DESC = "A defunct space shuttle that floats through space unattended.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200301C RID: 12316
				public class GRAVITASSPACESTATION8
				{
					// Token: 0x0400C306 RID: 49926
					public static LocString NAME = "Obsolete Space Station Module";

					// Token: 0x0400C307 RID: 49927
					public static LocString DESC = "The module from a space station that ceased to exist ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200301D RID: 12317
				public class RUSSELLSTEAPOT
				{
					// Token: 0x0400C308 RID: 49928
					public static LocString NAME = "Russell's Teapot";

					// Token: 0x0400C309 RID: 49929
					public static LocString DESC = "Has never been disproven to not exist.";
				}
			}

			// Token: 0x0200253D RID: 9533
			public class HARVESTABLE_POI
			{
				// Token: 0x0400A30B RID: 41739
				public static LocString POI_PRODUCTION = "{0}";

				// Token: 0x0400A30C RID: 41740
				public static LocString POI_PRODUCTION_TOOLTIP = "{0}";

				// Token: 0x0200301E RID: 12318
				public class CARBONASTEROIDFIELD
				{
					// Token: 0x0400C30A RID: 49930
					public static LocString NAME = "Carbon Asteroid Field";

					// Token: 0x0400C30B RID: 49931
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid containing ",
						UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
						" and ",
						UI.FormatAsLink("Coal", "CARBON"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200301F RID: 12319
				public class METALLICASTEROIDFIELD
				{
					// Token: 0x0400C30C RID: 49932
					public static LocString NAME = "Metallic Asteroid Field";

					// Token: 0x0400C30D RID: 49933
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Iron", "IRON"),
						", ",
						UI.FormatAsLink("Copper", "COPPER"),
						" and ",
						UI.FormatAsLink("Obsidian", "OBSIDIAN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003020 RID: 12320
				public class SATELLITEFIELD
				{
					// Token: 0x0400C30E RID: 49934
					public static LocString NAME = "Space Debris";

					// Token: 0x0400C30F RID: 49935
					public static LocString DESC = "Space junk from a forgotten age.\n\nHarvesting resources requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003021 RID: 12321
				public class ROCKYASTEROIDFIELD
				{
					// Token: 0x0400C310 RID: 49936
					public static LocString NAME = "Rocky Asteroid Field";

					// Token: 0x0400C311 RID: 49937
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						", ",
						UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK"),
						" and ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003022 RID: 12322
				public class INTERSTELLARICEFIELD
				{
					// Token: 0x0400C312 RID: 49938
					public static LocString NAME = "Ice Asteroid Field";

					// Token: 0x0400C313 RID: 49939
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003023 RID: 12323
				public class ORGANICMASSFIELD
				{
					// Token: 0x0400C314 RID: 49940
					public static LocString NAME = "Organic Mass Field";

					// Token: 0x0400C315 RID: 49941
					public static LocString DESC = string.Concat(new string[]
					{
						"A mass of harvestable resources containing ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Dirt", "DIRT"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003024 RID: 12324
				public class ICEASTEROIDFIELD
				{
					// Token: 0x0400C316 RID: 49942
					public static LocString NAME = "Exploded Ice Giant";

					// Token: 0x0400C317 RID: 49943
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of planetary remains containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						" and ",
						UI.FormatAsLink("Natural Gas", "METHANE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003025 RID: 12325
				public class GASGIANTCLOUD
				{
					// Token: 0x0400C318 RID: 49944
					public static LocString NAME = "Exploded Gas Giant";

					// Token: 0x0400C319 RID: 49945
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Hydrogen", "HYDROGEN"),
						" in ",
						UI.FormatAsLink("gas", "ELEMENTS_GAS"),
						" form, and ",
						UI.FormatAsLink("Methane", "SOLIDMETHANE"),
						" in ",
						UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
						" and ",
						UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
						" form.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003026 RID: 12326
				public class CHLORINECLOUD
				{
					// Token: 0x0400C31A RID: 49946
					public static LocString NAME = "Chlorine Cloud";

					// Token: 0x0400C31B RID: 49947
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of harvestable debris containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						" and ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003027 RID: 12327
				public class GILDEDASTEROIDFIELD
				{
					// Token: 0x0400C31C RID: 49948
					public static LocString NAME = "Gilded Asteroid Field";

					// Token: 0x0400C31D RID: 49949
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Gold", "GOLD"),
						", ",
						UI.FormatAsLink("Fullerene", "FULLERENE"),
						", ",
						UI.FormatAsLink("Regolith", "REGOLITH"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003028 RID: 12328
				public class GLIMMERINGASTEROIDFIELD
				{
					// Token: 0x0400C31E RID: 49950
					public static LocString NAME = "Glimmering Asteroid Field";

					// Token: 0x0400C31F RID: 49951
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Tungsten", "TUNGSTEN"),
						", ",
						UI.FormatAsLink("Wolframite", "WOLFRAMITE"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003029 RID: 12329
				public class HELIUMCLOUD
				{
					// Token: 0x0400C320 RID: 49952
					public static LocString NAME = "Helium Cloud";

					// Token: 0x0400C321 RID: 49953
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Water", "WATER"),
						" and ",
						UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302A RID: 12330
				public class OILYASTEROIDFIELD
				{
					// Token: 0x0400C322 RID: 49954
					public static LocString NAME = "Oily Asteroid Field";

					// Token: 0x0400C323 RID: 49955
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Solid Methane", "SOLIDMETHANE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302B RID: 12331
				public class OXIDIZEDASTEROIDFIELD
				{
					// Token: 0x0400C324 RID: 49956
					public static LocString NAME = "Oxidized Asteroid Field";

					// Token: 0x0400C325 RID: 49957
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Rust", "RUST"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302C RID: 12332
				public class SALTYASTEROIDFIELD
				{
					// Token: 0x0400C326 RID: 49958
					public static LocString NAME = "Salty Asteroid Field";

					// Token: 0x0400C327 RID: 49959
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						",",
						UI.FormatAsLink("Brine", "BRINE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302D RID: 12333
				public class FROZENOREFIELD
				{
					// Token: 0x0400C328 RID: 49960
					public static LocString NAME = "Frozen Ore Asteroid Field";

					// Token: 0x0400C329 RID: 49961
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Polluted Ice", "DIRTYICE"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Snow", "SNOW"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302E RID: 12334
				public class FORESTYOREFIELD
				{
					// Token: 0x0400C32A RID: 49962
					public static LocString NAME = "Forested Ore Field";

					// Token: 0x0400C32B RID: 49963
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200302F RID: 12335
				public class SWAMPYOREFIELD
				{
					// Token: 0x0400C32C RID: 49964
					public static LocString NAME = "Swampy Ore Field";

					// Token: 0x0400C32D RID: 49965
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Mud", "MUD"),
						", ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						" and ",
						UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003030 RID: 12336
				public class SANDYOREFIELD
				{
					// Token: 0x0400C32E RID: 49966
					public static LocString NAME = "Sandy Ore Field";

					// Token: 0x0400C32F RID: 49967
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Sandstone", "SANDSTONE"),
						", ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						" and ",
						UI.FormatAsLink("Sand", "SAND"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003031 RID: 12337
				public class RADIOACTIVEGASCLOUD
				{
					// Token: 0x0400C330 RID: 49968
					public static LocString NAME = "Radioactive Gas Cloud";

					// Token: 0x0400C331 RID: 49969
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003032 RID: 12338
				public class RADIOACTIVEASTEROIDFIELD
				{
					// Token: 0x0400C332 RID: 49970
					public static LocString NAME = "Radioactive Asteroid Field";

					// Token: 0x0400C333 RID: 49971
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						", ",
						UI.FormatAsLink("Rust", "RUST"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Sulfur", "SULFUR"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003033 RID: 12339
				public class OXYGENRICHASTEROIDFIELD
				{
					// Token: 0x0400C334 RID: 49972
					public static LocString NAME = "Oxygen Rich Asteroid Field";

					// Token: 0x0400C335 RID: 49973
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Water", "WATER"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003034 RID: 12340
				public class INTERSTELLAROCEAN
				{
					// Token: 0x0400C336 RID: 49974
					public static LocString NAME = "Interstellar Ocean";

					// Token: 0x0400C337 RID: 49975
					public static LocString DESC = string.Concat(new string[]
					{
						"An interplanetary body that consists of ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						", ",
						UI.FormatAsLink("Brine", "BRINE"),
						", ",
						UI.FormatAsLink("Salt", "SALT"),
						" and ",
						UI.FormatAsLink("Ice", "ICE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}
			}

			// Token: 0x0200253E RID: 9534
			public class GRAVITAS_SPACE_POI
			{
				// Token: 0x0400A30D RID: 41741
				public static LocString STATION = "Destroyed Gravitas Space Station";
			}

			// Token: 0x0200253F RID: 9535
			public class TELESCOPE_TARGET
			{
				// Token: 0x0400A30E RID: 41742
				public static LocString NAME = "Telescope Target";
			}

			// Token: 0x02002540 RID: 9536
			public class ASTEROIDS
			{
				// Token: 0x02003035 RID: 12341
				public class ROCKYASTEROID
				{
					// Token: 0x0400C338 RID: 49976
					public static LocString NAME = "Rocky Asteroid";

					// Token: 0x0400C339 RID: 49977
					public static LocString DESCRIPTION = "A minor mineral planet. Unlike a comet, it does not possess a tail.";
				}

				// Token: 0x02003036 RID: 12342
				public class METALLICASTEROID
				{
					// Token: 0x0400C33A RID: 49978
					public static LocString NAME = "Metallic Asteroid";

					// Token: 0x0400C33B RID: 49979
					public static LocString DESCRIPTION = "A shimmering conglomerate of various metals.";
				}

				// Token: 0x02003037 RID: 12343
				public class CARBONACEOUSASTEROID
				{
					// Token: 0x0400C33C RID: 49980
					public static LocString NAME = "Carbon Asteroid";

					// Token: 0x0400C33D RID: 49981
					public static LocString DESCRIPTION = "A common asteroid containing several useful resources.";
				}

				// Token: 0x02003038 RID: 12344
				public class OILYASTEROID
				{
					// Token: 0x0400C33E RID: 49982
					public static LocString NAME = "Oily Asteroid";

					// Token: 0x0400C33F RID: 49983
					public static LocString DESCRIPTION = "A viscous asteroid that is only loosely held together. Contains fossil fuel resources.";
				}

				// Token: 0x02003039 RID: 12345
				public class GOLDASTEROID
				{
					// Token: 0x0400C340 RID: 49984
					public static LocString NAME = "Gilded Asteroid";

					// Token: 0x0400C341 RID: 49985
					public static LocString DESCRIPTION = "A rich asteroid with thin gold coating and veins of gold deposits throughout.";
				}
			}

			// Token: 0x02002541 RID: 9537
			public class CLUSTERMAPMETEORSHOWERS
			{
				// Token: 0x0200303A RID: 12346
				public class UNIDENTIFIED
				{
					// Token: 0x0400C342 RID: 49986
					public static LocString NAME = "Unidentified Object";

					// Token: 0x0400C343 RID: 49987
					public static LocString DESCRIPTION = "A cosmic anomaly is traveling through the galaxy.\n\nIts origins and purpose are currently unknown, though a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " could change that.";
				}

				// Token: 0x0200303B RID: 12347
				public class SLIME
				{
					// Token: 0x0400C344 RID: 49988
					public static LocString NAME = "Slimy Meteor Shower";

					// Token: 0x0400C345 RID: 49989
					public static LocString DESCRIPTION = "A shower of slimy, biodynamic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200303C RID: 12348
				public class SNOW
				{
					// Token: 0x0400C346 RID: 49990
					public static LocString NAME = "Blizzard Meteor Shower";

					// Token: 0x0400C347 RID: 49991
					public static LocString DESCRIPTION = "A shower of cold, cold meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200303D RID: 12349
				public class ICE
				{
					// Token: 0x0400C348 RID: 49992
					public static LocString NAME = "Ice Meteor Shower";

					// Token: 0x0400C349 RID: 49993
					public static LocString DESCRIPTION = "A hailstorm of icy space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200303E RID: 12350
				public class COPPER
				{
					// Token: 0x0400C34A RID: 49994
					public static LocString NAME = "Copper Meteor Shower";

					// Token: 0x0400C34B RID: 49995
					public static LocString DESCRIPTION = "A shower of metallic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200303F RID: 12351
				public class IRON
				{
					// Token: 0x0400C34C RID: 49996
					public static LocString NAME = "Iron Meteor Shower";

					// Token: 0x0400C34D RID: 49997
					public static LocString DESCRIPTION = "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003040 RID: 12352
				public class GOLD
				{
					// Token: 0x0400C34E RID: 49998
					public static LocString NAME = "Gold Meteor Shower";

					// Token: 0x0400C34F RID: 49999
					public static LocString DESCRIPTION = "A shower of shiny metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003041 RID: 12353
				public class URANIUM
				{
					// Token: 0x0400C350 RID: 50000
					public static LocString NAME = "Uranium Meteor Shower";

					// Token: 0x0400C351 RID: 50001
					public static LocString DESCRIPTION = "A toxic shower of radioactive meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003042 RID: 12354
				public class LIGHTDUST
				{
					// Token: 0x0400C352 RID: 50002
					public static LocString NAME = "Dust Fluff Meteor Shower";

					// Token: 0x0400C353 RID: 50003
					public static LocString DESCRIPTION = "A cloud-like shower of dust fluff meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003043 RID: 12355
				public class HEAVYDUST
				{
					// Token: 0x0400C354 RID: 50004
					public static LocString NAME = "Dense Dust Meteor Shower";

					// Token: 0x0400C355 RID: 50005
					public static LocString DESCRIPTION = "A dark cloud of heavy dust meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003044 RID: 12356
				public class REGOLITH
				{
					// Token: 0x0400C356 RID: 50006
					public static LocString NAME = "Regolith Meteor Shower";

					// Token: 0x0400C357 RID: 50007
					public static LocString DESCRIPTION = "A shower of rocky meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003045 RID: 12357
				public class OXYLITE
				{
					// Token: 0x0400C358 RID: 50008
					public static LocString NAME = "Oxylite Meteor Shower";

					// Token: 0x0400C359 RID: 50009
					public static LocString DESCRIPTION = "A shower of rocky, oxygen-rich meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003046 RID: 12358
				public class BLEACHSTONE
				{
					// Token: 0x0400C35A RID: 50010
					public static LocString NAME = "Bleach Stone Meteor Shower";

					// Token: 0x0400C35B RID: 50011
					public static LocString DESCRIPTION = "A shower of bleach stone meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003047 RID: 12359
				public class MOO
				{
					// Token: 0x0400C35C RID: 50012
					public static LocString NAME = "Gassy Mooteor Shower";

					// Token: 0x0400C35D RID: 50013
					public static LocString DESCRIPTION = "A herd of methane-infused meteors that cause a bit of a stink, but do no actual damage.";
				}
			}

			// Token: 0x02002542 RID: 9538
			public class COMETS
			{
				// Token: 0x02003048 RID: 12360
				public class ROCKCOMET
				{
					// Token: 0x0400C35E RID: 50014
					public static LocString NAME = "Rock Meteor";
				}

				// Token: 0x02003049 RID: 12361
				public class DUSTCOMET
				{
					// Token: 0x0400C35F RID: 50015
					public static LocString NAME = "Dust Meteor";
				}

				// Token: 0x0200304A RID: 12362
				public class IRONCOMET
				{
					// Token: 0x0400C360 RID: 50016
					public static LocString NAME = "Iron Meteor";
				}

				// Token: 0x0200304B RID: 12363
				public class COPPERCOMET
				{
					// Token: 0x0400C361 RID: 50017
					public static LocString NAME = "Copper Meteor";
				}

				// Token: 0x0200304C RID: 12364
				public class GOLDCOMET
				{
					// Token: 0x0400C362 RID: 50018
					public static LocString NAME = "Gold Meteor";
				}

				// Token: 0x0200304D RID: 12365
				public class FULLERENECOMET
				{
					// Token: 0x0400C363 RID: 50019
					public static LocString NAME = "Fullerene Meteor";
				}

				// Token: 0x0200304E RID: 12366
				public class URANIUMORECOMET
				{
					// Token: 0x0400C364 RID: 50020
					public static LocString NAME = "Uranium Meteor";
				}

				// Token: 0x0200304F RID: 12367
				public class NUCLEAR_WASTE
				{
					// Token: 0x0400C365 RID: 50021
					public static LocString NAME = "Radioactive Meteor";
				}

				// Token: 0x02003050 RID: 12368
				public class SATELLITE
				{
					// Token: 0x0400C366 RID: 50022
					public static LocString NAME = "Defunct Satellite";
				}

				// Token: 0x02003051 RID: 12369
				public class FOODCOMET
				{
					// Token: 0x0400C367 RID: 50023
					public static LocString NAME = "Snack Bomb";
				}

				// Token: 0x02003052 RID: 12370
				public class GASSYMOOCOMET
				{
					// Token: 0x0400C368 RID: 50024
					public static LocString NAME = "Gassy Mooteor";
				}

				// Token: 0x02003053 RID: 12371
				public class SLIMECOMET
				{
					// Token: 0x0400C369 RID: 50025
					public static LocString NAME = "Slime Meteor";
				}

				// Token: 0x02003054 RID: 12372
				public class SNOWBALLCOMET
				{
					// Token: 0x0400C36A RID: 50026
					public static LocString NAME = "Snow Meteor";
				}

				// Token: 0x02003055 RID: 12373
				public class HARDICECOMET
				{
					// Token: 0x0400C36B RID: 50027
					public static LocString NAME = "Ice Meteor";
				}

				// Token: 0x02003056 RID: 12374
				public class LIGHTDUSTCOMET
				{
					// Token: 0x0400C36C RID: 50028
					public static LocString NAME = "Dust Fluff Meteor";
				}

				// Token: 0x02003057 RID: 12375
				public class ALGAECOMET
				{
					// Token: 0x0400C36D RID: 50029
					public static LocString NAME = "Algae Meteor";
				}

				// Token: 0x02003058 RID: 12376
				public class PHOSPHORICCOMET
				{
					// Token: 0x0400C36E RID: 50030
					public static LocString NAME = "Phosphoric Meteor";
				}

				// Token: 0x02003059 RID: 12377
				public class OXYLITECOMET
				{
					// Token: 0x0400C36F RID: 50031
					public static LocString NAME = "Oxylite Meteor";
				}

				// Token: 0x0200305A RID: 12378
				public class BLEACHSTONECOMET
				{
					// Token: 0x0400C370 RID: 50032
					public static LocString NAME = "Bleach Stone Meteor";
				}
			}

			// Token: 0x02002543 RID: 9539
			public class DWARFPLANETS
			{
				// Token: 0x0200305B RID: 12379
				public class ICYDWARF
				{
					// Token: 0x0400C371 RID: 50033
					public static LocString NAME = "Interstellar Ice";

					// Token: 0x0400C372 RID: 50034
					public static LocString DESCRIPTION = "A terrestrial destination, frozen completely solid.";
				}

				// Token: 0x0200305C RID: 12380
				public class ORGANICDWARF
				{
					// Token: 0x0400C373 RID: 50035
					public static LocString NAME = "Organic Mass";

					// Token: 0x0400C374 RID: 50036
					public static LocString DESCRIPTION = "A mass of organic material similar to the ooze used to print Duplicants. This sample is heavily degraded.";
				}

				// Token: 0x0200305D RID: 12381
				public class DUSTYDWARF
				{
					// Token: 0x0400C375 RID: 50037
					public static LocString NAME = "Dusty Dwarf";

					// Token: 0x0400C376 RID: 50038
					public static LocString DESCRIPTION = "A loosely held together composite of minerals.";
				}

				// Token: 0x0200305E RID: 12382
				public class SALTDWARF
				{
					// Token: 0x0400C377 RID: 50039
					public static LocString NAME = "Salty Dwarf";

					// Token: 0x0400C378 RID: 50040
					public static LocString DESCRIPTION = "A dwarf planet with unusually high sodium concentrations.";
				}

				// Token: 0x0200305F RID: 12383
				public class REDDWARF
				{
					// Token: 0x0400C379 RID: 50041
					public static LocString NAME = "Red Dwarf";

					// Token: 0x0400C37A RID: 50042
					public static LocString DESCRIPTION = "An M-class star orbited by clusters of extractable aluminum and methane.";
				}
			}

			// Token: 0x02002544 RID: 9540
			public class PLANETS
			{
				// Token: 0x02003060 RID: 12384
				public class TERRAPLANET
				{
					// Token: 0x0400C37B RID: 50043
					public static LocString NAME = "Terrestrial Planet";

					// Token: 0x0400C37C RID: 50044
					public static LocString DESCRIPTION = "A planet with a walkable surface, though it does not possess the resources to sustain long-term life.";
				}

				// Token: 0x02003061 RID: 12385
				public class VOLCANOPLANET
				{
					// Token: 0x0400C37D RID: 50045
					public static LocString NAME = "Volcanic Planet";

					// Token: 0x0400C37E RID: 50046
					public static LocString DESCRIPTION = "A large terrestrial object composed mainly of molten rock.";
				}

				// Token: 0x02003062 RID: 12386
				public class SHATTEREDPLANET
				{
					// Token: 0x0400C37F RID: 50047
					public static LocString NAME = "Shattered Planet";

					// Token: 0x0400C380 RID: 50048
					public static LocString DESCRIPTION = "A once-habitable planet that has sustained massive damage.\n\nA powerful containment field prevents our rockets from traveling to its surface.";
				}

				// Token: 0x02003063 RID: 12387
				public class RUSTPLANET
				{
					// Token: 0x0400C381 RID: 50049
					public static LocString NAME = "Oxidized Asteroid";

					// Token: 0x0400C382 RID: 50050
					public static LocString DESCRIPTION = "A small planet covered in large swathes of brown rust.";
				}

				// Token: 0x02003064 RID: 12388
				public class FORESTPLANET
				{
					// Token: 0x0400C383 RID: 50051
					public static LocString NAME = "Living Planet";

					// Token: 0x0400C384 RID: 50052
					public static LocString DESCRIPTION = "A small green planet displaying several markers of primitive life.";
				}

				// Token: 0x02003065 RID: 12389
				public class SHINYPLANET
				{
					// Token: 0x0400C385 RID: 50053
					public static LocString NAME = "Glimmering Planet";

					// Token: 0x0400C386 RID: 50054
					public static LocString DESCRIPTION = "A planet composed of rare, shimmering minerals. From the distance, it looks like gem in the sky.";
				}

				// Token: 0x02003066 RID: 12390
				public class CHLORINEPLANET
				{
					// Token: 0x0400C387 RID: 50055
					public static LocString NAME = "Chlorine Planet";

					// Token: 0x0400C388 RID: 50056
					public static LocString DESCRIPTION = "A noxious planet permeated by unbreathable chlorine.";
				}

				// Token: 0x02003067 RID: 12391
				public class SALTDESERTPLANET
				{
					// Token: 0x0400C389 RID: 50057
					public static LocString NAME = "Arid Planet";

					// Token: 0x0400C38A RID: 50058
					public static LocString DESCRIPTION = "A sweltering, desert-like planet covered in surface salt deposits.";
				}
			}

			// Token: 0x02002545 RID: 9541
			public class GIANTS
			{
				// Token: 0x02003068 RID: 12392
				public class GASGIANT
				{
					// Token: 0x0400C38B RID: 50059
					public static LocString NAME = "Gas Giant";

					// Token: 0x0400C38C RID: 50060
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + " formed around a small solid center.";
				}

				// Token: 0x02003069 RID: 12393
				public class ICEGIANT
				{
					// Token: 0x0400C38D RID: 50061
					public static LocString NAME = "Ice Giant";

					// Token: 0x0400C38E RID: 50062
					public static LocString DESCRIPTION = "A massive volume of frozen material, primarily composed of " + UI.FormatAsLink("Ice", "ICE") + ".";
				}

				// Token: 0x0200306A RID: 12394
				public class HYDROGENGIANT
				{
					// Token: 0x0400C38F RID: 50063
					public static LocString NAME = "Helium Giant";

					// Token: 0x0400C390 RID: 50064
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Helium", "HELIUM") + " formed around a small solid center.";
				}
			}
		}

		// Token: 0x02001D93 RID: 7571
		public class SPACEARTIFACTS
		{
			// Token: 0x02002546 RID: 9542
			public class ARTIFACTTIERS
			{
				// Token: 0x0400A30F RID: 41743
				public static LocString TIER_NONE = "Nothing";

				// Token: 0x0400A310 RID: 41744
				public static LocString TIER0 = "Rarity 0";

				// Token: 0x0400A311 RID: 41745
				public static LocString TIER1 = "Rarity 1";

				// Token: 0x0400A312 RID: 41746
				public static LocString TIER2 = "Rarity 2";

				// Token: 0x0400A313 RID: 41747
				public static LocString TIER3 = "Rarity 3";

				// Token: 0x0400A314 RID: 41748
				public static LocString TIER4 = "Rarity 4";

				// Token: 0x0400A315 RID: 41749
				public static LocString TIER5 = "Rarity 5";
			}

			// Token: 0x02002547 RID: 9543
			public class PACUPERCOLATOR
			{
				// Token: 0x0400A316 RID: 41750
				public static LocString NAME = "Percolator";

				// Token: 0x0400A317 RID: 41751
				public static LocString DESCRIPTION = "Don't drink from it! There was a pacu... IN the percolator!";

				// Token: 0x0400A318 RID: 41752
				public static LocString ARTIFACT = "A coffee percolator with the remnants of a blend of coffee that was a personal favorite of Dr. Hassan Aydem.\n\nHe would specifically reserve the consumption of this particular blend for when he was reviewing research papers on Sunday afternoons.";
			}

			// Token: 0x02002548 RID: 9544
			public class ROBOTARM
			{
				// Token: 0x0400A319 RID: 41753
				public static LocString NAME = "Robot Arm";

				// Token: 0x0400A31A RID: 41754
				public static LocString DESCRIPTION = "It's not functional. Just cool.";

				// Token: 0x0400A31B RID: 41755
				public static LocString ARTIFACT = "A commercially available robot arm that has had a significant amount of modifications made to it.\n\nThe initials B.A. appear on one of the fingers.";
			}

			// Token: 0x02002549 RID: 9545
			public class HATCHFOSSIL
			{
				// Token: 0x0400A31C RID: 41756
				public static LocString NAME = "Pristine Fossil";

				// Token: 0x0400A31D RID: 41757
				public static LocString DESCRIPTION = "The preserved bones of an early species of Hatch.";

				// Token: 0x0400A31E RID: 41758
				public static LocString ARTIFACT = "The preservation of this skeleton occurred artificially using a technique called the \"The Ali Method\".\n\nIt should be noted that this fossilization technique was pioneered by one Dr. Ashkan Seyed Ali, an employee of Gravitas.";
			}

			// Token: 0x0200254A RID: 9546
			public class MODERNART
			{
				// Token: 0x0400A31F RID: 41759
				public static LocString NAME = "Modern Art";

				// Token: 0x0400A320 RID: 41760
				public static LocString DESCRIPTION = "I don't get it.";

				// Token: 0x0400A321 RID: 41761
				public static LocString ARTIFACT = "A sculpture of the Neoplastism movement of Modern Art.\n\nGravitas records show that this piece was once used in a presentation called 'Form and Function in Corporate Aesthetic'.";
			}

			// Token: 0x0200254B RID: 9547
			public class EGGROCK
			{
				// Token: 0x0400A322 RID: 41762
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400A323 RID: 41763
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.";

				// Token: 0x0400A324 RID: 41764
				public static LocString ARTIFACT = "The words \"Happy Farters Day Dad. Love Macy\" appear on the bottom of this rock, written in a childlish scrawl.";
			}

			// Token: 0x0200254C RID: 9548
			public class RAINBOWEGGROCK
			{
				// Token: 0x0400A325 RID: 41765
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400A326 RID: 41766
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.\n\nThis one is rainbow colored.";

				// Token: 0x0400A327 RID: 41767
				public static LocString ARTIFACT = "The words \"Happy Father's Day, Dad. Love you!\" appear on the bottom of this rock, written in very neat handwriting. The words are surrounded by four hearts drawn in what appears to be a pink gel pen.";
			}

			// Token: 0x0200254D RID: 9549
			public class OKAYXRAY
			{
				// Token: 0x0400A328 RID: 41768
				public static LocString NAME = "Old X-Ray";

				// Token: 0x0400A329 RID: 41769
				public static LocString DESCRIPTION = "Ew, weird. It has five fingers!";

				// Token: 0x0400A32A RID: 41770
				public static LocString ARTIFACT = "The description on this X-ray indicates that it was taken in the Gravitas Medical Facility.\n\nMost likely this X-ray was performed while investigating an injury that occurred within the facility.";
			}

			// Token: 0x0200254E RID: 9550
			public class SHIELDGENERATOR
			{
				// Token: 0x0400A32B RID: 41771
				public static LocString NAME = "Shield Generator";

				// Token: 0x0400A32C RID: 41772
				public static LocString DESCRIPTION = "A mechanical prototype capable of producing a small section of shielding.";

				// Token: 0x0400A32D RID: 41773
				public static LocString ARTIFACT = "The energy field produced by this shield generator completely ignores those light behaviors which are wave-like and focuses instead on its particle behaviors.\n\nThis seemingly paradoxical state is possible when light is slowed down to the point at which it stops entirely.";
			}

			// Token: 0x0200254F RID: 9551
			public class TEAPOT
			{
				// Token: 0x0400A32E RID: 41774
				public static LocString NAME = "Encrusted Teapot";

				// Token: 0x0400A32F RID: 41775
				public static LocString DESCRIPTION = "A teapot from the depths of space, coated in a thick layer of Neutronium.";

				// Token: 0x0400A330 RID: 41776
				public static LocString ARTIFACT = "The amount of Neutronium present in this teapot suggests that it has crossed the threshold of the spacetime continuum on countless occasions, floating through many multiple universes over a plethora of times and spaces.\n\nThough there are, theoretically, an infinite amount of outcomes to any one event over many multi-verses, the homogeneity of the still relatively young multiverse suggests that this is then not the only teapot which has crossed into multiple universes. Despite the infinite possible outcomes of infinite multiverses it appears one high probability constant is that there is, or once was, a teapot floating somewhere in space within every universe.";
			}

			// Token: 0x02002550 RID: 9552
			public class DNAMODEL
			{
				// Token: 0x0400A331 RID: 41777
				public static LocString NAME = "Double Helix Model";

				// Token: 0x0400A332 RID: 41778
				public static LocString DESCRIPTION = "An educational model of genetic information.";

				// Token: 0x0400A333 RID: 41779
				public static LocString ARTIFACT = "A physical representation of the building blocks of life.\n\nThis one contains trace amounts of a Genetic Ooze prototype that was once used by Gravitas.";
			}

			// Token: 0x02002551 RID: 9553
			public class SANDSTONE
			{
				// Token: 0x0400A334 RID: 41780
				public static LocString NAME = "Sandstone";

				// Token: 0x0400A335 RID: 41781
				public static LocString DESCRIPTION = "A beautiful rock composed of multiple layers of sediment.";

				// Token: 0x0400A336 RID: 41782
				public static LocString ARTIFACT = "This sample of sandstone appears to have been processed by the Gravitas Mining Gun that was made available to the general public.\n\nNote: The Gravitas public Mining Gun model is different than ones used by Duplicants in its larger size, and extra precautionary features added in order to be compliant with national safety standards.";
			}

			// Token: 0x02002552 RID: 9554
			public class MAGMALAMP
			{
				// Token: 0x0400A337 RID: 41783
				public static LocString NAME = "Magma Lamp";

				// Token: 0x0400A338 RID: 41784
				public static LocString DESCRIPTION = "The sequel to \"Lava Lamp\".";

				// Token: 0x0400A339 RID: 41785
				public static LocString ARTIFACT = "Molten lava and obsidian combined in a way that allows the lava to maintain just enough heat to remain in liquid form.\n\nPlans of this lamp found in the Gravitas archives have been attributed to one Robin Nisbet, PhD.";
			}

			// Token: 0x02002553 RID: 9555
			public class OBELISK
			{
				// Token: 0x0400A33A RID: 41786
				public static LocString NAME = "Small Obelisk";

				// Token: 0x0400A33B RID: 41787
				public static LocString DESCRIPTION = "A rectangular stone piece.\n\nIts function is unclear.";

				// Token: 0x0400A33C RID: 41788
				public static LocString ARTIFACT = "On close inspection this rectangle is actually a stone box built with a covert, almost seamless, lid, housing a tiny key.\n\nIt is still unclear what the key unlocks.";
			}

			// Token: 0x02002554 RID: 9556
			public class RUBIKSCUBE
			{
				// Token: 0x0400A33D RID: 41789
				public static LocString NAME = "Rubik's Cube";

				// Token: 0x0400A33E RID: 41790
				public static LocString DESCRIPTION = "This mystery of the universe has already been solved.";

				// Token: 0x0400A33F RID: 41791
				public static LocString ARTIFACT = "A well-used, competition-compliant version of the popular puzzle cube.\n\nIt's worth noting that Dr. Dylan 'Nails' Winslow was once a regional Rubik's Cube champion.";
			}

			// Token: 0x02002555 RID: 9557
			public class OFFICEMUG
			{
				// Token: 0x0400A340 RID: 41792
				public static LocString NAME = "Office Mug";

				// Token: 0x0400A341 RID: 41793
				public static LocString DESCRIPTION = "An intermediary place to store espresso before you move it to your mouth.";

				// Token: 0x0400A342 RID: 41794
				public static LocString ARTIFACT = "An office mug with the Gravitas logo on it. Though their office mugs were all emblazoned with the same logo, Gravitas colored their mugs differently to distinguish between their various departments.\n\nThis one is from the AI department.";
			}

			// Token: 0x02002556 RID: 9558
			public class AMELIASWATCH
			{
				// Token: 0x0400A343 RID: 41795
				public static LocString NAME = "Wrist Watch";

				// Token: 0x0400A344 RID: 41796
				public static LocString DESCRIPTION = "It was discovered in a package labeled \"To be entrusted to Dr. Walker\".";

				// Token: 0x0400A345 RID: 41797
				public static LocString ARTIFACT = "This watch once belonged to pioneering aviator Amelia Earhart and travelled to space via astronaut Dr. Shannon Walker.\n\nHow it came to be floating in space is a matter of speculation, but perhaps the adventurous spirit of its original stewards became infused within the fabric of this timepiece and compelled the universe to launch it into the great unknown.";
			}

			// Token: 0x02002557 RID: 9559
			public class MOONMOONMOON
			{
				// Token: 0x0400A346 RID: 41798
				public static LocString NAME = "Moonmoonmoon";

				// Token: 0x0400A347 RID: 41799
				public static LocString DESCRIPTION = "A moon's moon's moon. It's very small.";

				// Token: 0x0400A348 RID: 41800
				public static LocString ARTIFACT = "In contrast to most moons, this object's glowing properties do not come from reflecting an external source of light, but rather from an internal glow of mysterious origin.\n\nThe glow of this object also grants an extraordinary amount of Decor bonus to nearby Duplicants, almost as if it was designed that way.";
			}

			// Token: 0x02002558 RID: 9560
			public class BIOLUMINESCENTROCK
			{
				// Token: 0x0400A349 RID: 41801
				public static LocString NAME = "Bioluminescent Rock";

				// Token: 0x0400A34A RID: 41802
				public static LocString DESCRIPTION = "A thriving colony of tiny, microscopic organisms is responsible for giving it its bluish glow.";

				// Token: 0x0400A34B RID: 41803
				public static LocString ARTIFACT = "The microscopic organisms within this rock are of a unique variety whose genetic code shows many tell-tale signs of being genetically engineered within a lab.\n\nFurther analysis reveals they share 99.999% of their genetic code with Shine Bugs.";
			}

			// Token: 0x02002559 RID: 9561
			public class PLASMALAMP
			{
				// Token: 0x0400A34C RID: 41804
				public static LocString NAME = "Plasma Lamp";

				// Token: 0x0400A34D RID: 41805
				public static LocString DESCRIPTION = "No space colony is complete without one.";

				// Token: 0x0400A34E RID: 41806
				public static LocString ARTIFACT = "The bottom of this lamp contains the words 'Property of the Atmospheric Sciences Department'.\n\nIt's worth noting that the Gravitas Atmospheric Sciences Department once simulated an experiment testing the feasibility of survival in an environment filled with noble gasses, similar to the ones contained within this device.";
			}

			// Token: 0x0200255A RID: 9562
			public class MOLDAVITE
			{
				// Token: 0x0400A34F RID: 41807
				public static LocString NAME = "Moldavite";

				// Token: 0x0400A350 RID: 41808
				public static LocString DESCRIPTION = "A unique green stone formed from the impact of a meteorite.";

				// Token: 0x0400A351 RID: 41809
				public static LocString ARTIFACT = "This extremely rare, museum grade moldavite once sat on the desk of Dr. Ren Sato, but it was stolen by some unknown person.\n\nDr. Sato suspected the perpetrator was none other than Director Stern, but was never able to confirm this theory.";
			}

			// Token: 0x0200255B RID: 9563
			public class BRICKPHONE
			{
				// Token: 0x0400A352 RID: 41810
				public static LocString NAME = "Strange Brick";

				// Token: 0x0400A353 RID: 41811
				public static LocString DESCRIPTION = "It still works.";

				// Token: 0x0400A354 RID: 41812
				public static LocString ARTIFACT = "This cordless phone once held a direct line to an unknown location in which strange distant voices can be heard but not understood, nor interacted with.\n\nThough Gravitas spent a lot of money and years of study dedicated to discovering its secret, the mystery was never solved.";
			}

			// Token: 0x0200255C RID: 9564
			public class SOLARSYSTEM
			{
				// Token: 0x0400A355 RID: 41813
				public static LocString NAME = "Self-Contained System";

				// Token: 0x0400A356 RID: 41814
				public static LocString DESCRIPTION = "A marvel of the cosmos, inside this display is an entirely self-contained solar system.";

				// Token: 0x0400A357 RID: 41815
				public static LocString ARTIFACT = "This marvel of a device was built using parts from an old Tornado-in-a-Box science fair project.\n\nVery faint, faded letters are still visible on the display bottom that read 'Camille P. Grade 5'.";
			}

			// Token: 0x0200255D RID: 9565
			public class SINK
			{
				// Token: 0x0400A358 RID: 41816
				public static LocString NAME = "Sink";

				// Token: 0x0400A359 RID: 41817
				public static LocString DESCRIPTION = "No collection is complete without it.";

				// Token: 0x0400A35A RID: 41818
				public static LocString ARTIFACT = "A small trace of encrusted soap on this sink strongly suggests it was installed in a personal bathroom, rather than a public one which would have used a soap dispenser.\n\nThe soap sliver is light blue and contains a manufactured blueberry fragrance.";
			}

			// Token: 0x0200255E RID: 9566
			public class ROCKTORNADO
			{
				// Token: 0x0400A35B RID: 41819
				public static LocString NAME = "Tornado Rock";

				// Token: 0x0400A35C RID: 41820
				public static LocString DESCRIPTION = "It's unclear how it formed, although I'm glad it did.";

				// Token: 0x0400A35D RID: 41821
				public static LocString ARTIFACT = "Speculations about the origin of this rock include a paper written by one Harold P. Moreson, Ph.D. in which he theorized it could be a rare form of hollow geode which failed to form any crystals inside.\n\nThis paper appears in the Gravitas archives, and in all probability, was one of the factors in the hiring of Moreson into the Geology department of the company.";
			}

			// Token: 0x0200255F RID: 9567
			public class BLENDER
			{
				// Token: 0x0400A35E RID: 41822
				public static LocString NAME = "Blender";

				// Token: 0x0400A35F RID: 41823
				public static LocString DESCRIPTION = "Equipment used to conduct experiments answering the age-old question, \"Could that blend\"?";

				// Token: 0x0400A360 RID: 41824
				public static LocString ARTIFACT = "Trace amounts of edible foodstuffs present in this blender indicate that it was probably used to emulsify the ingredients of a mush bar.\n\nIt is also very likely that it was employed at least once in the production of a peanut butter and banana smoothie.";
			}

			// Token: 0x02002560 RID: 9568
			public class SAXOPHONE
			{
				// Token: 0x0400A361 RID: 41825
				public static LocString NAME = "Mangled Saxophone";

				// Token: 0x0400A362 RID: 41826
				public static LocString DESCRIPTION = "The name \"Pesquet\" is barely legible on the inside.";

				// Token: 0x0400A363 RID: 41827
				public static LocString ARTIFACT = "Though it is often remarked that \"in space, no one can hear you scream\", Thomas Pesquet proved the same cannot be said for the smooth jazzy sounds of a saxophone.\n\nAlthough this instrument once belonged to the eminent French Astronaut its current bumped and bent shape suggests it has seen many adventures beyond that of just being used to perform an out-of-this-world saxophone solo.";
			}

			// Token: 0x02002561 RID: 9569
			public class STETHOSCOPE
			{
				// Token: 0x0400A364 RID: 41828
				public static LocString NAME = "Stethoscope";

				// Token: 0x0400A365 RID: 41829
				public static LocString DESCRIPTION = "Listens to Duplicant heartbeats, or gurgly tummies.";

				// Token: 0x0400A366 RID: 41830
				public static LocString ARTIFACT = "The size and shape of this stethescope suggests it was not intended to be used by neither a human-sized nor a Duplicant-sized person but something half-way in between the two beings.";
			}

			// Token: 0x02002562 RID: 9570
			public class VHS
			{
				// Token: 0x0400A367 RID: 41831
				public static LocString NAME = "Archaic Tech";

				// Token: 0x0400A368 RID: 41832
				public static LocString DESCRIPTION = "Be kind when you handle it. It's very fragile.";

				// Token: 0x0400A369 RID: 41833
				public static LocString ARTIFACT = "The label on this VHS tape reads \"Jackie and Olivia's House Warming Party\".\n\nUnfortunately, a device with which to play this recording no longer exists in this universe.";
			}

			// Token: 0x02002563 RID: 9571
			public class REACTORMODEL
			{
				// Token: 0x0400A36A RID: 41834
				public static LocString NAME = "Model Nuclear Power Plant";

				// Token: 0x0400A36B RID: 41835
				public static LocString DESCRIPTION = "It's pronounced nu-clear.";

				// Token: 0x0400A36C RID: 41836
				public static LocString ARTIFACT = "Though this Nuclear Power Plant was never built, this model exists as an artifact to a time early in the life of Gravitas when it was researching all alternatives to solving the global energy problem.\n\nUltimately, the idea of building a Nuclear Power Plant was abandoned in favor of the \"much safer\" alternative of developing the Temporal Bow.";
			}

			// Token: 0x02002564 RID: 9572
			public class MOODRING
			{
				// Token: 0x0400A36D RID: 41837
				public static LocString NAME = "Radiation Mood Ring";

				// Token: 0x0400A36E RID: 41838
				public static LocString DESCRIPTION = "How radioactive are you feeling?";

				// Token: 0x0400A36F RID: 41839
				public static LocString ARTIFACT = "A wholly unique ring not found anywhere outside of the Gravitas Laboratory.\n\nThough it can't be determined for sure who worked on this extraordinary curiousity it's worth noting that, for his Ph.D. thesis, Dr. Travaldo Farrington wrote a paper entitled \"Novelty Uses for Radiochromatic Dyes\".";
			}

			// Token: 0x02002565 RID: 9573
			public class ORACLE
			{
				// Token: 0x0400A370 RID: 41840
				public static LocString NAME = "Useless Machine";

				// Token: 0x0400A371 RID: 41841
				public static LocString DESCRIPTION = "What does it do?";

				// Token: 0x0400A372 RID: 41842
				public static LocString ARTIFACT = "All of the parts for this contraption are recycled from projects abandoned by the Robotics department.\n\nThe design is very close to one published in an amateur DIY magazine that once sat in the lobby of the 'Employees Only' area of Gravitas' facilities.";
			}

			// Token: 0x02002566 RID: 9574
			public class GRUBSTATUE
			{
				// Token: 0x0400A373 RID: 41843
				public static LocString NAME = "Grubgrub Statue";

				// Token: 0x0400A374 RID: 41844
				public static LocString DESCRIPTION = "A moving tribute to a tiny plant hugger.";

				// Token: 0x0400A375 RID: 41845
				public static LocString ARTIFACT = "It's very likely this statue was placed in a hidden, secluded place in the Gravitas laboratory since the creation of Grubgrubs was a closely held secret that the general public was not privy to.\n\nThis is a shame since the artistic quality of this statue is really quite accomplished.";
			}

			// Token: 0x02002567 RID: 9575
			public class HONEYJAR
			{
				// Token: 0x0400A376 RID: 41846
				public static LocString NAME = "Honey Jar";

				// Token: 0x0400A377 RID: 41847
				public static LocString DESCRIPTION = "Sweet golden liquid with just a touch of uranium.";

				// Token: 0x0400A378 RID: 41848
				public static LocString ARTIFACT = "Records from the Genetics and Biology Lab of the Gravitas facility show that several early iterations of a radioactive Bee would continue to produce honey and that this honey was once accidentally stored in the employee kitchen which resulted in several incidents of minor radiation poisoning when it was erroneously labled as a sweetener for tea.\n\nEmployees who used this product reported that it was the \"sweetest honey they'd ever tasted\" and expressed no regret at the mix-up.";
			}
		}

		// Token: 0x02001D94 RID: 7572
		public class KEEPSAKES
		{
			// Token: 0x02002568 RID: 9576
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400A379 RID: 41849
				public static LocString NAME = "Ceramic Morb";

				// Token: 0x0400A37A RID: 41850
				public static LocString DESCRIPTION = "A pottery project produced in an HR-mandated art therapy class.\n\nIt's glazed with a substance that once landed a curious lab technician in the ER.";
			}

			// Token: 0x02002569 RID: 9577
			public class MEGA_BRAIN
			{
				// Token: 0x0400A37B RID: 41851
				public static LocString NAME = "Model Plane";

				// Token: 0x0400A37C RID: 41852
				public static LocString DESCRIPTION = "A treasured souvenir that was once a common accompaniment to children's meals during commercial flights. There's a hole in the bottom from when Dr. Holland had it mounted on a stand.";
			}

			// Token: 0x0200256A RID: 9578
			public class LONELY_MINION
			{
				// Token: 0x0400A37D RID: 41853
				public static LocString NAME = "Rusty Toolbox";

				// Token: 0x0400A37E RID: 41854
				public static LocString DESCRIPTION = "On the inside of the lid, someone used a screwdriver to carve a drawing of a group of smiling Duplicants gathered around a massive crater.";
			}

			// Token: 0x0200256B RID: 9579
			public class FOSSIL_HUNT
			{
				// Token: 0x0400A37F RID: 41855
				public static LocString NAME = "Critter Collar";

				// Token: 0x0400A380 RID: 41856
				public static LocString DESCRIPTION = "The tag reads \"Molly\".\n\nOn the reverse is \"Designed by B363\" stamped above what appears to be an unusually shaped pawprint.";
			}
		}

		// Token: 0x02001D95 RID: 7573
		public class SANDBOXTOOLS
		{
			// Token: 0x0200256C RID: 9580
			public class SETTINGS
			{
				// Token: 0x0200306B RID: 12395
				public class INSTANT_BUILD
				{
					// Token: 0x0400C391 RID: 50065
					public static LocString NAME = "Instant build mode ON";

					// Token: 0x0400C392 RID: 50066
					public static LocString TOOLTIP = "Toggle between placing construction plans and fully built buildings";
				}

				// Token: 0x0200306C RID: 12396
				public class BRUSH_SIZE
				{
					// Token: 0x0400C393 RID: 50067
					public static LocString NAME = "Size";

					// Token: 0x0400C394 RID: 50068
					public static LocString TOOLTIP = "Adjust brush size";
				}

				// Token: 0x0200306D RID: 12397
				public class BRUSH_NOISE_SCALE
				{
					// Token: 0x0400C395 RID: 50069
					public static LocString NAME = "Noise A";

					// Token: 0x0400C396 RID: 50070
					public static LocString TOOLTIP = "Adjust brush noisiness A";
				}

				// Token: 0x0200306E RID: 12398
				public class BRUSH_NOISE_DENSITY
				{
					// Token: 0x0400C397 RID: 50071
					public static LocString NAME = "Noise B";

					// Token: 0x0400C398 RID: 50072
					public static LocString TOOLTIP = "Adjust brush noisiness B";
				}

				// Token: 0x0200306F RID: 12399
				public class TEMPERATURE
				{
					// Token: 0x0400C399 RID: 50073
					public static LocString NAME = "Temperature";

					// Token: 0x0400C39A RID: 50074
					public static LocString TOOLTIP = "Adjust absolute temperature";
				}

				// Token: 0x02003070 RID: 12400
				public class TEMPERATURE_ADDITIVE
				{
					// Token: 0x0400C39B RID: 50075
					public static LocString NAME = "Temperature";

					// Token: 0x0400C39C RID: 50076
					public static LocString TOOLTIP = "Adjust additive temperature";
				}

				// Token: 0x02003071 RID: 12401
				public class RADIATION
				{
					// Token: 0x0400C39D RID: 50077
					public static LocString NAME = "Absolute radiation";

					// Token: 0x0400C39E RID: 50078
					public static LocString TOOLTIP = "Adjust absolute radiation";
				}

				// Token: 0x02003072 RID: 12402
				public class RADIATION_ADDITIVE
				{
					// Token: 0x0400C39F RID: 50079
					public static LocString NAME = "Additive radiation";

					// Token: 0x0400C3A0 RID: 50080
					public static LocString TOOLTIP = "Adjust additive radiation";
				}

				// Token: 0x02003073 RID: 12403
				public class STRESS_ADDITIVE
				{
					// Token: 0x0400C3A1 RID: 50081
					public static LocString NAME = "Reduce Stress";

					// Token: 0x0400C3A2 RID: 50082
					public static LocString TOOLTIP = "Adjust stress reduction";
				}

				// Token: 0x02003074 RID: 12404
				public class MORALE
				{
					// Token: 0x0400C3A3 RID: 50083
					public static LocString NAME = "Adjust Morale";

					// Token: 0x0400C3A4 RID: 50084
					public static LocString TOOLTIP = "Bonus Morale adjustment";
				}

				// Token: 0x02003075 RID: 12405
				public class MASS
				{
					// Token: 0x0400C3A5 RID: 50085
					public static LocString NAME = "Mass";

					// Token: 0x0400C3A6 RID: 50086
					public static LocString TOOLTIP = "Adjust mass";
				}

				// Token: 0x02003076 RID: 12406
				public class DISEASE
				{
					// Token: 0x0400C3A7 RID: 50087
					public static LocString NAME = "Germ";

					// Token: 0x0400C3A8 RID: 50088
					public static LocString TOOLTIP = "Adjust type of germ";
				}

				// Token: 0x02003077 RID: 12407
				public class DISEASE_COUNT
				{
					// Token: 0x0400C3A9 RID: 50089
					public static LocString NAME = "Germs";

					// Token: 0x0400C3AA RID: 50090
					public static LocString TOOLTIP = "Adjust germ count";
				}

				// Token: 0x02003078 RID: 12408
				public class BRUSH
				{
					// Token: 0x0400C3AB RID: 50091
					public static LocString NAME = "Brush";

					// Token: 0x0400C3AC RID: 50092
					public static LocString TOOLTIP = "Paint elements into the world simulation {Hotkey}";
				}

				// Token: 0x02003079 RID: 12409
				public class ELEMENT
				{
					// Token: 0x0400C3AD RID: 50093
					public static LocString NAME = "Element";

					// Token: 0x0400C3AE RID: 50094
					public static LocString TOOLTIP = "Adjust type of element";
				}

				// Token: 0x0200307A RID: 12410
				public class SPRINKLE
				{
					// Token: 0x0400C3AF RID: 50095
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400C3B0 RID: 50096
					public static LocString TOOLTIP = "Paint elements into the simulation using noise {Hotkey}";
				}

				// Token: 0x0200307B RID: 12411
				public class FLOOD
				{
					// Token: 0x0400C3B1 RID: 50097
					public static LocString NAME = "Fill";

					// Token: 0x0400C3B2 RID: 50098
					public static LocString TOOLTIP = "Fill a section of the simulation with the chosen element {Hotkey}";
				}

				// Token: 0x0200307C RID: 12412
				public class SAMPLE
				{
					// Token: 0x0400C3B3 RID: 50099
					public static LocString NAME = "Sample";

					// Token: 0x0400C3B4 RID: 50100
					public static LocString TOOLTIP = "Copy the settings from a cell to use with brush tools {Hotkey}";
				}

				// Token: 0x0200307D RID: 12413
				public class HEATGUN
				{
					// Token: 0x0400C3B5 RID: 50101
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400C3B6 RID: 50102
					public static LocString TOOLTIP = "Inject thermal energy into the simulation {Hotkey}";
				}

				// Token: 0x0200307E RID: 12414
				public class RADSTOOL
				{
					// Token: 0x0400C3B7 RID: 50103
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400C3B8 RID: 50104
					public static LocString TOOLTIP = "Inject or remove radiation from the simulation {Hotkey}";
				}

				// Token: 0x0200307F RID: 12415
				public class SPAWNER
				{
					// Token: 0x0400C3B9 RID: 50105
					public static LocString NAME = "Spawner";

					// Token: 0x0400C3BA RID: 50106
					public static LocString TOOLTIP = "Spawn critters, food, equipment, and other entities {Hotkey}";
				}

				// Token: 0x02003080 RID: 12416
				public class STRESS
				{
					// Token: 0x0400C3BB RID: 50107
					public static LocString NAME = "Stress";

					// Token: 0x0400C3BC RID: 50108
					public static LocString TOOLTIP = "Manage Duplicants' stress levels {Hotkey}";
				}

				// Token: 0x02003081 RID: 12417
				public class CLEAR_FLOOR
				{
					// Token: 0x0400C3BD RID: 50109
					public static LocString NAME = "Clear Debris";

					// Token: 0x0400C3BE RID: 50110
					public static LocString TOOLTIP = "Delete loose items cluttering the floor {Hotkey}";
				}

				// Token: 0x02003082 RID: 12418
				public class DESTROY
				{
					// Token: 0x0400C3BF RID: 50111
					public static LocString NAME = "Destroy";

					// Token: 0x0400C3C0 RID: 50112
					public static LocString TOOLTIP = "Delete everything in the selected cell(s) {Hotkey}";
				}

				// Token: 0x02003083 RID: 12419
				public class SPAWN_ENTITY
				{
					// Token: 0x0400C3C1 RID: 50113
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003084 RID: 12420
				public class FOW
				{
					// Token: 0x0400C3C2 RID: 50114
					public static LocString NAME = "Reveal";

					// Token: 0x0400C3C3 RID: 50115
					public static LocString TOOLTIP = "Dispel the Fog of War shrouding the map {Hotkey}";
				}

				// Token: 0x02003085 RID: 12421
				public class CRITTER
				{
					// Token: 0x0400C3C4 RID: 50116
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400C3C5 RID: 50117
					public static LocString TOOLTIP = "Remove Critters! {Hotkey}";
				}
			}

			// Token: 0x0200256D RID: 9581
			public class FILTERS
			{
				// Token: 0x0400A381 RID: 41857
				public static LocString BACK = "Back";

				// Token: 0x0400A382 RID: 41858
				public static LocString COMMON = "Common Substances";

				// Token: 0x0400A383 RID: 41859
				public static LocString SOLID = "Solids";

				// Token: 0x0400A384 RID: 41860
				public static LocString LIQUID = "Liquids";

				// Token: 0x0400A385 RID: 41861
				public static LocString GAS = "Gases";

				// Token: 0x02003086 RID: 12422
				public class ENTITIES
				{
					// Token: 0x0400C3C6 RID: 50118
					public static LocString SPECIAL = "Special";

					// Token: 0x0400C3C7 RID: 50119
					public static LocString GRAVITAS = "Gravitas";

					// Token: 0x0400C3C8 RID: 50120
					public static LocString PLANTS = "Plants";

					// Token: 0x0400C3C9 RID: 50121
					public static LocString SEEDS = "Seeds";

					// Token: 0x0400C3CA RID: 50122
					public static LocString CREATURE = "Critters";

					// Token: 0x0400C3CB RID: 50123
					public static LocString CREATURE_EGG = "Eggs";

					// Token: 0x0400C3CC RID: 50124
					public static LocString FOOD = "Foods";

					// Token: 0x0400C3CD RID: 50125
					public static LocString EQUIPMENT = "Equipment";

					// Token: 0x0400C3CE RID: 50126
					public static LocString GEYSERS = "Geysers";

					// Token: 0x0400C3CF RID: 50127
					public static LocString EXPERIMENTS = "Experimental";

					// Token: 0x0400C3D0 RID: 50128
					public static LocString INDUSTRIAL_PRODUCTS = "Industrial";

					// Token: 0x0400C3D1 RID: 50129
					public static LocString COMETS = "Meteors";

					// Token: 0x0400C3D2 RID: 50130
					public static LocString ARTIFACTS = "Artifacts";

					// Token: 0x0400C3D3 RID: 50131
					public static LocString STORYTRAITS = "Story Traits";
				}
			}

			// Token: 0x0200256E RID: 9582
			public class CLEARFLOOR
			{
				// Token: 0x0400A386 RID: 41862
				public static LocString DELETED = "Deleted";
			}
		}

		// Token: 0x02001D96 RID: 7574
		public class RETIRED_COLONY_INFO_SCREEN
		{
			// Token: 0x04008596 RID: 34198
			public static LocString SECONDS = "Seconds";

			// Token: 0x04008597 RID: 34199
			public static LocString CYCLES = "Cycles";

			// Token: 0x04008598 RID: 34200
			public static LocString CYCLE_COUNT = "Cycle Count: {0}";

			// Token: 0x04008599 RID: 34201
			public static LocString DUPLICANT_AGE = "Age: {0} cycles";

			// Token: 0x0400859A RID: 34202
			public static LocString SKILL_LEVEL = "Skill Level: {0}";

			// Token: 0x0400859B RID: 34203
			public static LocString BUILDING_COUNT = "Count: {0}";

			// Token: 0x0400859C RID: 34204
			public static LocString PREVIEW_UNAVAILABLE = "Preview\nUnavailable";

			// Token: 0x0400859D RID: 34205
			public static LocString TIMELAPSE_UNAVAILABLE = "Timelapse\nUnavailable";

			// Token: 0x0400859E RID: 34206
			public static LocString SEARCH = "SEARCH...";

			// Token: 0x0200256F RID: 9583
			public class BUTTONS
			{
				// Token: 0x0400A387 RID: 41863
				public static LocString RETURN_TO_GAME = "RETURN TO GAME";

				// Token: 0x0400A388 RID: 41864
				public static LocString VIEW_OTHER_COLONIES = "BACK";

				// Token: 0x0400A389 RID: 41865
				public static LocString QUIT_TO_MENU = "QUIT TO MAIN MENU";

				// Token: 0x0400A38A RID: 41866
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002570 RID: 9584
			public class TITLES
			{
				// Token: 0x0400A38B RID: 41867
				public static LocString EXPLORER_HEADER = "COLONIES";

				// Token: 0x0400A38C RID: 41868
				public static LocString RETIRED_COLONIES = "Colony Summaries";

				// Token: 0x0400A38D RID: 41869
				public static LocString COLONY_STATISTICS = "Colony Statistics";

				// Token: 0x0400A38E RID: 41870
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400A38F RID: 41871
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400A390 RID: 41872
				public static LocString CHEEVOS = "Colony Achievements";

				// Token: 0x0400A391 RID: 41873
				public static LocString ACHIEVEMENT_HEADER = "ACHIEVEMENTS";

				// Token: 0x0400A392 RID: 41874
				public static LocString TIMELAPSE = "Timelapse";
			}

			// Token: 0x02002571 RID: 9585
			public class STATS
			{
				// Token: 0x0400A393 RID: 41875
				public static LocString OXYGEN_CREATED = "Total Oxygen Produced";

				// Token: 0x0400A394 RID: 41876
				public static LocString OXYGEN_CONSUMED = "Total Oxygen Consumed";

				// Token: 0x0400A395 RID: 41877
				public static LocString POWER_CREATED = "Average Power Produced";

				// Token: 0x0400A396 RID: 41878
				public static LocString POWER_WASTED = "Average Power Wasted";

				// Token: 0x0400A397 RID: 41879
				public static LocString TRAVEL_TIME = "Total Travel Time";

				// Token: 0x0400A398 RID: 41880
				public static LocString WORK_TIME = "Total Work Time";

				// Token: 0x0400A399 RID: 41881
				public static LocString AVERAGE_TRAVEL_TIME = "Average Travel Time";

				// Token: 0x0400A39A RID: 41882
				public static LocString AVERAGE_WORK_TIME = "Average Work Time";

				// Token: 0x0400A39B RID: 41883
				public static LocString CALORIES_CREATED = "Calorie Generation";

				// Token: 0x0400A39C RID: 41884
				public static LocString CALORIES_CONSUMED = "Calorie Consumption";

				// Token: 0x0400A39D RID: 41885
				public static LocString LIVE_DUPLICANTS = "Duplicants";

				// Token: 0x0400A39E RID: 41886
				public static LocString AVERAGE_STRESS_CREATED = "Average Stress Created";

				// Token: 0x0400A39F RID: 41887
				public static LocString AVERAGE_STRESS_REMOVED = "Average Stress Removed";

				// Token: 0x0400A3A0 RID: 41888
				public static LocString NUMBER_DOMESTICATED_CRITTERS = "Domesticated Critters";

				// Token: 0x0400A3A1 RID: 41889
				public static LocString NUMBER_WILD_CRITTERS = "Wild Critters";

				// Token: 0x0400A3A2 RID: 41890
				public static LocString AVERAGE_GERMS = "Average Germs";

				// Token: 0x0400A3A3 RID: 41891
				public static LocString ROCKET_MISSIONS = "Rocket Missions Underway";
			}
		}

		// Token: 0x02001D97 RID: 7575
		public class DROPDOWN
		{
			// Token: 0x0400859F RID: 34207
			public static LocString NONE = "Unassigned";
		}

		// Token: 0x02001D98 RID: 7576
		public class FRONTEND
		{
			// Token: 0x040085A0 RID: 34208
			public static LocString GAME_VERSION = "Game Version: ";

			// Token: 0x040085A1 RID: 34209
			public static LocString LOADING = "Loading...";

			// Token: 0x040085A2 RID: 34210
			public static LocString DONE_BUTTON = "DONE";

			// Token: 0x02002572 RID: 9586
			public class DEMO_OVER_SCREEN
			{
				// Token: 0x0400A3A4 RID: 41892
				public static LocString TITLE = "Thanks for playing!";

				// Token: 0x0400A3A5 RID: 41893
				public static LocString BODY = "Thank you for playing the demo for Oxygen Not Included!\n\nThis game is still in development.\n\nGo to kleigames.com/o2 or ask one of us if you'd like more information.";

				// Token: 0x0400A3A6 RID: 41894
				public static LocString BUTTON_EXIT_TO_MENU = "EXIT TO MENU";
			}

			// Token: 0x02002573 RID: 9587
			public class CUSTOMGAMESETTINGSSCREEN
			{
				// Token: 0x02003087 RID: 12423
				public class SETTINGS
				{
					// Token: 0x020033B8 RID: 13240
					public class SANDBOXMODE
					{
						// Token: 0x0400CBD0 RID: 52176
						public static LocString NAME = "Sandbox Mode";

						// Token: 0x0400CBD1 RID: 52177
						public static LocString TOOLTIP = "Manipulate and customize the simulation with tools that ignore regular game constraints";

						// Token: 0x02003430 RID: 13360
						public static class LEVELS
						{
							// Token: 0x02003446 RID: 13382
							public static class DISABLED
							{
								// Token: 0x0400CD04 RID: 52484
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD05 RID: 52485
								public static LocString TOOLTIP = "Unchecked: Sandbox Mode is turned off (Default)";
							}

							// Token: 0x02003447 RID: 13383
							public static class ENABLED
							{
								// Token: 0x0400CD06 RID: 52486
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD07 RID: 52487
								public static LocString TOOLTIP = "Checked: Sandbox Mode is turned on";
							}
						}
					}

					// Token: 0x020033B9 RID: 13241
					public class FASTWORKERSMODE
					{
						// Token: 0x0400CBD2 RID: 52178
						public static LocString NAME = "Fast Workers Mode";

						// Token: 0x0400CBD3 RID: 52179
						public static LocString TOOLTIP = "Dupes will finish most work immediately and require little sleep";

						// Token: 0x02003431 RID: 13361
						public static class LEVELS
						{
							// Token: 0x02003448 RID: 13384
							public static class DISABLED
							{
								// Token: 0x0400CD08 RID: 52488
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD09 RID: 52489
								public static LocString TOOLTIP = "Unchecked: Fast Workers Mode is turned off (Default)";
							}

							// Token: 0x02003449 RID: 13385
							public static class ENABLED
							{
								// Token: 0x0400CD0A RID: 52490
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD0B RID: 52491
								public static LocString TOOLTIP = "Checked: Fast Workers Mode is turned on";
							}
						}
					}

					// Token: 0x020033BA RID: 13242
					public class EXPANSION1ACTIVE
					{
						// Token: 0x0400CBD4 RID: 52180
						public static LocString NAME = UI.DLC1.NAME_ITAL + " Content Enabled";

						// Token: 0x0400CBD5 RID: 52181
						public static LocString TOOLTIP = "If checked, content from the " + UI.DLC1.NAME_ITAL + " Expansion will be available";

						// Token: 0x02003432 RID: 13362
						public static class LEVELS
						{
							// Token: 0x0200344A RID: 13386
							public static class DISABLED
							{
								// Token: 0x0400CD0C RID: 52492
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD0D RID: 52493
								public static LocString TOOLTIP = "Unchecked: " + UI.DLC1.NAME_ITAL + " Content is turned off (Default)";
							}

							// Token: 0x0200344B RID: 13387
							public static class ENABLED
							{
								// Token: 0x0400CD0E RID: 52494
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD0F RID: 52495
								public static LocString TOOLTIP = "Checked: " + UI.DLC1.NAME_ITAL + " Content is turned on";
							}
						}
					}

					// Token: 0x020033BB RID: 13243
					public class SAVETOCLOUD
					{
						// Token: 0x0400CBD6 RID: 52182
						public static LocString NAME = "Save To Cloud";

						// Token: 0x0400CBD7 RID: 52183
						public static LocString TOOLTIP = "This colony will be created in the cloud saves folder, and synced by the game platform.";

						// Token: 0x0400CBD8 RID: 52184
						public static LocString TOOLTIP_LOCAL = "This colony will be created in the local saves folder. It will not be a cloud save and will not be synced by the game platform.";

						// Token: 0x0400CBD9 RID: 52185
						public static LocString TOOLTIP_EXTRA = "This can be changed later with the colony management options in the load screen, from the main menu.";

						// Token: 0x02003433 RID: 13363
						public static class LEVELS
						{
							// Token: 0x0200344C RID: 13388
							public static class DISABLED
							{
								// Token: 0x0400CD10 RID: 52496
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD11 RID: 52497
								public static LocString TOOLTIP = "Unchecked: This colony will be a local save";
							}

							// Token: 0x0200344D RID: 13389
							public static class ENABLED
							{
								// Token: 0x0400CD12 RID: 52498
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD13 RID: 52499
								public static LocString TOOLTIP = "Checked: This colony will be a cloud save (Default)";
							}
						}
					}

					// Token: 0x020033BC RID: 13244
					public class CAREPACKAGES
					{
						// Token: 0x0400CBDA RID: 52186
						public static LocString NAME = "Care Packages";

						// Token: 0x0400CBDB RID: 52187
						public static LocString TOOLTIP = "Affects what resources can be printed from the Printing Pod";

						// Token: 0x02003434 RID: 13364
						public static class LEVELS
						{
							// Token: 0x0200344E RID: 13390
							public static class NORMAL
							{
								// Token: 0x0400CD14 RID: 52500
								public static LocString NAME = "All";

								// Token: 0x0400CD15 RID: 52501
								public static LocString TOOLTIP = "Checked: The Printing Pod will offer both Duplicant blueprints and care packages (Default)";
							}

							// Token: 0x0200344F RID: 13391
							public static class DUPLICANTS_ONLY
							{
								// Token: 0x0400CD16 RID: 52502
								public static LocString NAME = "Duplicants Only";

								// Token: 0x0400CD17 RID: 52503
								public static LocString TOOLTIP = "Unchecked: The Printing Pod will only offer Duplicant blueprints";
							}
						}
					}

					// Token: 0x020033BD RID: 13245
					public class IMMUNESYSTEM
					{
						// Token: 0x0400CBDC RID: 52188
						public static LocString NAME = "Disease";

						// Token: 0x0400CBDD RID: 52189
						public static LocString TOOLTIP = "Affects Duplicants' chances of contracting a disease after germ exposure";

						// Token: 0x02003435 RID: 13365
						public static class LEVELS
						{
							// Token: 0x02003450 RID: 13392
							public static class COMPROMISED
							{
								// Token: 0x0400CD18 RID: 52504
								public static LocString NAME = "Outbreak Prone";

								// Token: 0x0400CD19 RID: 52505
								public static LocString TOOLTIP = "The whole colony will be ravaged by plague if a Duplicant so much as sneezes funny";

								// Token: 0x0400CD1A RID: 52506
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Outbreak Prone (Highest Difficulty)";
							}

							// Token: 0x02003451 RID: 13393
							public static class WEAK
							{
								// Token: 0x0400CD1B RID: 52507
								public static LocString NAME = "Germ Susceptible";

								// Token: 0x0400CD1C RID: 52508
								public static LocString TOOLTIP = "These Duplicants have an increased chance of contracting diseases from germ exposure";

								// Token: 0x0400CD1D RID: 52509
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Susceptibility (Difficulty Up)";
							}

							// Token: 0x02003452 RID: 13394
							public static class DEFAULT
							{
								// Token: 0x0400CD1E RID: 52510
								public static LocString NAME = "Default";

								// Token: 0x0400CD1F RID: 52511
								public static LocString TOOLTIP = "Default disease chance";
							}

							// Token: 0x02003453 RID: 13395
							public static class STRONG
							{
								// Token: 0x0400CD20 RID: 52512
								public static LocString NAME = "Germ Resistant";

								// Token: 0x0400CD21 RID: 52513
								public static LocString TOOLTIP = "These Duplicants have a decreased chance of contracting diseases from germ exposure";

								// Token: 0x0400CD22 RID: 52514
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Resistance (Difficulty Down)";
							}

							// Token: 0x02003454 RID: 13396
							public static class INVINCIBLE
							{
								// Token: 0x0400CD23 RID: 52515
								public static LocString NAME = "Total Immunity";

								// Token: 0x0400CD24 RID: 52516
								public static LocString TOOLTIP = "Like diplomatic immunity, but without the diplomacy. These Duplicants will never get sick";

								// Token: 0x0400CD25 RID: 52517
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Total Immunity (No Disease)";
							}
						}
					}

					// Token: 0x020033BE RID: 13246
					public class MORALE
					{
						// Token: 0x0400CBDE RID: 52190
						public static LocString NAME = "Morale";

						// Token: 0x0400CBDF RID: 52191
						public static LocString TOOLTIP = "Adjusts the minimum morale Duplicants must maintain to avoid gaining stress";

						// Token: 0x02003436 RID: 13366
						public static class LEVELS
						{
							// Token: 0x02003455 RID: 13397
							public static class VERYHARD
							{
								// Token: 0x0400CD26 RID: 52518
								public static LocString NAME = "Draconian";

								// Token: 0x0400CD27 RID: 52519
								public static LocString TOOLTIP = "The finest of the finest can barely keep up with these Duplicants' stringent demands";

								// Token: 0x0400CD28 RID: 52520
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Draconian (Highest Difficulty)";
							}

							// Token: 0x02003456 RID: 13398
							public static class HARD
							{
								// Token: 0x0400CD29 RID: 52521
								public static LocString NAME = "A Bit Persnickety";

								// Token: 0x0400CD2A RID: 52522
								public static LocString TOOLTIP = "Duplicants require higher morale than usual to fend off stress";

								// Token: 0x0400CD2B RID: 52523
								public static LocString ATTRIBUTE_MODIFIER_NAME = "A Bit Persnickety (Difficulty Up)";
							}

							// Token: 0x02003457 RID: 13399
							public static class DEFAULT
							{
								// Token: 0x0400CD2C RID: 52524
								public static LocString NAME = "Default";

								// Token: 0x0400CD2D RID: 52525
								public static LocString TOOLTIP = "Default morale needs";
							}

							// Token: 0x02003458 RID: 13400
							public static class EASY
							{
								// Token: 0x0400CD2E RID: 52526
								public static LocString NAME = "Chill";

								// Token: 0x0400CD2F RID: 52527
								public static LocString TOOLTIP = "Duplicants require lower morale than usual to fend off stress";

								// Token: 0x0400CD30 RID: 52528
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chill (Difficulty Down)";
							}

							// Token: 0x02003459 RID: 13401
							public static class DISABLED
							{
								// Token: 0x0400CD31 RID: 52529
								public static LocString NAME = "Totally Blasé";

								// Token: 0x0400CD32 RID: 52530
								public static LocString TOOLTIP = "These Duplicants have zero standards and will never gain stress, regardless of their morale";

								// Token: 0x0400CD33 RID: 52531
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Totally Blasé (No Morale)";
							}
						}
					}

					// Token: 0x020033BF RID: 13247
					public class CALORIE_BURN
					{
						// Token: 0x0400CBE0 RID: 52192
						public static LocString NAME = "Hunger";

						// Token: 0x0400CBE1 RID: 52193
						public static LocString TOOLTIP = "Affects how quickly Duplicants burn calories and become hungry";

						// Token: 0x02003437 RID: 13367
						public static class LEVELS
						{
							// Token: 0x0200345A RID: 13402
							public static class VERYHARD
							{
								// Token: 0x0400CD34 RID: 52532
								public static LocString NAME = "Ravenous";

								// Token: 0x0400CD35 RID: 52533
								public static LocString TOOLTIP = "Your Duplicants are on a see-food diet... They see food and they eat it";

								// Token: 0x0400CD36 RID: 52534
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Ravenous (Highest Difficulty)";
							}

							// Token: 0x0200345B RID: 13403
							public static class HARD
							{
								// Token: 0x0400CD37 RID: 52535
								public static LocString NAME = "Rumbly Tummies";

								// Token: 0x0400CD38 RID: 52536
								public static LocString TOOLTIP = "Duplicants burn calories quickly and require more feeding than usual";

								// Token: 0x0400CD39 RID: 52537
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Rumbly Tummies (Difficulty Up)";
							}

							// Token: 0x0200345C RID: 13404
							public static class DEFAULT
							{
								// Token: 0x0400CD3A RID: 52538
								public static LocString NAME = "Default";

								// Token: 0x0400CD3B RID: 52539
								public static LocString TOOLTIP = "Default calorie burn rate";
							}

							// Token: 0x0200345D RID: 13405
							public static class EASY
							{
								// Token: 0x0400CD3C RID: 52540
								public static LocString NAME = "Fasting";

								// Token: 0x0400CD3D RID: 52541
								public static LocString TOOLTIP = "Duplicants burn calories slowly and get by with fewer meals";

								// Token: 0x0400CD3E RID: 52542
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Fasting (Difficulty Down)";
							}

							// Token: 0x0200345E RID: 13406
							public static class DISABLED
							{
								// Token: 0x0400CD3F RID: 52543
								public static LocString NAME = "Tummyless";

								// Token: 0x0400CD40 RID: 52544
								public static LocString TOOLTIP = "These Duplicants were printed without tummies and need no food at all";

								// Token: 0x0400CD41 RID: 52545
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Tummyless (No Hunger)";
							}
						}
					}

					// Token: 0x020033C0 RID: 13248
					public class WORLD_CHOICE
					{
						// Token: 0x0400CBE2 RID: 52194
						public static LocString NAME = "World";

						// Token: 0x0400CBE3 RID: 52195
						public static LocString TOOLTIP = "New worlds added by mods can be selected here";
					}

					// Token: 0x020033C1 RID: 13249
					public class CLUSTER_CHOICE
					{
						// Token: 0x0400CBE4 RID: 52196
						public static LocString NAME = "Asteroid Belt";

						// Token: 0x0400CBE5 RID: 52197
						public static LocString TOOLTIP = "New asteroid belts added by mods can be selected here";
					}

					// Token: 0x020033C2 RID: 13250
					public class STORY_TRAIT_COUNT
					{
						// Token: 0x0400CBE6 RID: 52198
						public static LocString NAME = "Story Traits";

						// Token: 0x0400CBE7 RID: 52199
						public static LocString TOOLTIP = "Determines the number of story traits spawned";

						// Token: 0x02003438 RID: 13368
						public static class LEVELS
						{
							// Token: 0x0200345F RID: 13407
							public static class NONE
							{
								// Token: 0x0400CD42 RID: 52546
								public static LocString NAME = "Zilch";

								// Token: 0x0400CD43 RID: 52547
								public static LocString TOOLTIP = "Zero story traits. Zip. Nada. None";
							}

							// Token: 0x02003460 RID: 13408
							public static class FEW
							{
								// Token: 0x0400CD44 RID: 52548
								public static LocString NAME = "Stingy";

								// Token: 0x0400CD45 RID: 52549
								public static LocString TOOLTIP = "Not zero, but not a lot";
							}

							// Token: 0x02003461 RID: 13409
							public static class LOTS
							{
								// Token: 0x0400CD46 RID: 52550
								public static LocString NAME = "Oodles";

								// Token: 0x0400CD47 RID: 52551
								public static LocString TOOLTIP = "Plenty of story traits to go around";
							}
						}
					}

					// Token: 0x020033C3 RID: 13251
					public class DURABILITY
					{
						// Token: 0x0400CBE8 RID: 52200
						public static LocString NAME = "Durability";

						// Token: 0x0400CBE9 RID: 52201
						public static LocString TOOLTIP = "Affects how quickly equippable suits wear out";

						// Token: 0x02003439 RID: 13369
						public static class LEVELS
						{
							// Token: 0x02003462 RID: 13410
							public static class INDESTRUCTIBLE
							{
								// Token: 0x0400CD48 RID: 52552
								public static LocString NAME = "Indestructible";

								// Token: 0x0400CD49 RID: 52553
								public static LocString TOOLTIP = "Duplicants have perfected clothes manufacturing and are able to make suits that last forever";

								// Token: 0x0400CD4A RID: 52554
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Indestructible Suits (No Durability)";
							}

							// Token: 0x02003463 RID: 13411
							public static class REINFORCED
							{
								// Token: 0x0400CD4B RID: 52555
								public static LocString NAME = "Reinforced";

								// Token: 0x0400CD4C RID: 52556
								public static LocString TOOLTIP = "Suits are more durable than usual";

								// Token: 0x0400CD4D RID: 52557
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Reinforced Suits (Difficulty Down)";
							}

							// Token: 0x02003464 RID: 13412
							public static class DEFAULT
							{
								// Token: 0x0400CD4E RID: 52558
								public static LocString NAME = "Default";

								// Token: 0x0400CD4F RID: 52559
								public static LocString TOOLTIP = "Default suit durability";
							}

							// Token: 0x02003465 RID: 13413
							public static class FLIMSY
							{
								// Token: 0x0400CD50 RID: 52560
								public static LocString NAME = "Flimsy";

								// Token: 0x0400CD51 RID: 52561
								public static LocString TOOLTIP = "Suits wear out faster than usual";

								// Token: 0x0400CD52 RID: 52562
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Flimsy Suits (Difficulty Up)";
							}

							// Token: 0x02003466 RID: 13414
							public static class THREADBARE
							{
								// Token: 0x0400CD53 RID: 52563
								public static LocString NAME = "Threadbare";

								// Token: 0x0400CD54 RID: 52564
								public static LocString TOOLTIP = "These Duplicants are no tailors - suits wear out much faster than usual";

								// Token: 0x0400CD55 RID: 52565
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Threadbare Suits (Highest Difficulty)";
							}
						}
					}

					// Token: 0x020033C4 RID: 13252
					public class RADIATION
					{
						// Token: 0x0400CBEA RID: 52202
						public static LocString NAME = "Radiation";

						// Token: 0x0400CBEB RID: 52203
						public static LocString TOOLTIP = "Affects how susceptible Duplicants are to radiation sickness";

						// Token: 0x0200343A RID: 13370
						public static class LEVELS
						{
							// Token: 0x02003467 RID: 13415
							public static class HARDEST
							{
								// Token: 0x0400CD56 RID: 52566
								public static LocString NAME = "Critical Mass";

								// Token: 0x0400CD57 RID: 52567
								public static LocString TOOLTIP = "Duplicants feel ill at the merest mention of radiation...and may never truly recover";

								// Token: 0x0400CD58 RID: 52568
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Super Radiation (Highest Difficulty)";
							}

							// Token: 0x02003468 RID: 13416
							public static class HARDER
							{
								// Token: 0x0400CD59 RID: 52569
								public static LocString NAME = "Toxic Positivity";

								// Token: 0x0400CD5A RID: 52570
								public static LocString TOOLTIP = "Duplicants are more sensitive to radiation exposure than usual";

								// Token: 0x0400CD5B RID: 52571
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Vulnerable (Difficulty Up)";
							}

							// Token: 0x02003469 RID: 13417
							public static class DEFAULT
							{
								// Token: 0x0400CD5C RID: 52572
								public static LocString NAME = "Default";

								// Token: 0x0400CD5D RID: 52573
								public static LocString TOOLTIP = "Default radiation settings";
							}

							// Token: 0x0200346A RID: 13418
							public static class EASIER
							{
								// Token: 0x0400CD5E RID: 52574
								public static LocString NAME = "Healthy Glow";

								// Token: 0x0400CD5F RID: 52575
								public static LocString TOOLTIP = "Duplicants are more resistant to radiation exposure than usual";

								// Token: 0x0400CD60 RID: 52576
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Shielded (Difficulty Down)";
							}

							// Token: 0x0200346B RID: 13419
							public static class EASIEST
							{
								// Token: 0x0400CD61 RID: 52577
								public static LocString NAME = "Nuke-Proof";

								// Token: 0x0400CD62 RID: 52578
								public static LocString TOOLTIP = "Duplicants could bathe in radioactive waste and not even notice";

								// Token: 0x0400CD63 RID: 52579
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Protection (Lowest Difficulty)";
							}
						}
					}

					// Token: 0x020033C5 RID: 13253
					public class STRESS
					{
						// Token: 0x0400CBEC RID: 52204
						public static LocString NAME = "Stress";

						// Token: 0x0400CBED RID: 52205
						public static LocString TOOLTIP = "Affects how quickly Duplicant stress rises";

						// Token: 0x0200343B RID: 13371
						public static class LEVELS
						{
							// Token: 0x0200346C RID: 13420
							public static class INDOMITABLE
							{
								// Token: 0x0400CD64 RID: 52580
								public static LocString NAME = "Cloud Nine";

								// Token: 0x0400CD65 RID: 52581
								public static LocString TOOLTIP = "A strong emotional support system makes these Duplicants impervious to all stress";

								// Token: 0x0400CD66 RID: 52582
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Cloud Nine (No Stress)";
							}

							// Token: 0x0200346D RID: 13421
							public static class OPTIMISTIC
							{
								// Token: 0x0400CD67 RID: 52583
								public static LocString NAME = "Chipper";

								// Token: 0x0400CD68 RID: 52584
								public static LocString TOOLTIP = "Duplicants gain stress slower than usual";

								// Token: 0x0400CD69 RID: 52585
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chipper (Difficulty Down)";
							}

							// Token: 0x0200346E RID: 13422
							public static class DEFAULT
							{
								// Token: 0x0400CD6A RID: 52586
								public static LocString NAME = "Default";

								// Token: 0x0400CD6B RID: 52587
								public static LocString TOOLTIP = "Default stress change rate";
							}

							// Token: 0x0200346F RID: 13423
							public static class PESSIMISTIC
							{
								// Token: 0x0400CD6C RID: 52588
								public static LocString NAME = "Glum";

								// Token: 0x0400CD6D RID: 52589
								public static LocString TOOLTIP = "Duplicants gain stress more quickly than usual";

								// Token: 0x0400CD6E RID: 52590
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Glum (Difficulty Up)";
							}

							// Token: 0x02003470 RID: 13424
							public static class DOOMED
							{
								// Token: 0x0400CD6F RID: 52591
								public static LocString NAME = "Frankly Depressing";

								// Token: 0x0400CD70 RID: 52592
								public static LocString TOOLTIP = "These Duplicants were never taught coping mechanisms... they're devastated by stress as a result";

								// Token: 0x0400CD71 RID: 52593
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Frankly Depressing (Highest Difficulty)";
							}
						}
					}

					// Token: 0x020033C6 RID: 13254
					public class STRESS_BREAKS
					{
						// Token: 0x0400CBEE RID: 52206
						public static LocString NAME = "Stress Reactions";

						// Token: 0x0400CBEF RID: 52207
						public static LocString TOOLTIP = "Determines whether Duplicants wreak havoc on the colony when they reach maximum stress";

						// Token: 0x0200343C RID: 13372
						public static class LEVELS
						{
							// Token: 0x02003471 RID: 13425
							public static class DEFAULT
							{
								// Token: 0x0400CD72 RID: 52594
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD73 RID: 52595
								public static LocString TOOLTIP = "Checked: Duplicants will wreak havoc when they reach 100% stress (Default)";
							}

							// Token: 0x02003472 RID: 13426
							public static class DISABLED
							{
								// Token: 0x0400CD74 RID: 52596
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD75 RID: 52597
								public static LocString TOOLTIP = "Unchecked: Duplicants will not wreak havoc at maximum stress";
							}
						}
					}

					// Token: 0x020033C7 RID: 13255
					public class WORLDGEN_SEED
					{
						// Token: 0x0400CBF0 RID: 52208
						public static LocString NAME = "Worldgen Seed";

						// Token: 0x0400CBF1 RID: 52209
						public static LocString TOOLTIP = "This number chooses the procedural parameters that create your unique map\n\nWorldgen seeds can be copied and pasted so others can play a replica of your world configuration";

						// Token: 0x0400CBF2 RID: 52210
						public static LocString FIXEDSEED = "This is a predetermined seed, and cannot be changed";
					}

					// Token: 0x020033C8 RID: 13256
					public class TELEPORTERS
					{
						// Token: 0x0400CBF3 RID: 52211
						public static LocString NAME = "Teleporters";

						// Token: 0x0400CBF4 RID: 52212
						public static LocString TOOLTIP = "Determines whether teleporters will be spawned during Worldgen";

						// Token: 0x0200343D RID: 13373
						public static class LEVELS
						{
							// Token: 0x02003473 RID: 13427
							public static class ENABLED
							{
								// Token: 0x0400CD76 RID: 52598
								public static LocString NAME = "Enabled";

								// Token: 0x0400CD77 RID: 52599
								public static LocString TOOLTIP = "Checked: Teleporters will spawn during Worldgen (Default)";
							}

							// Token: 0x02003474 RID: 13428
							public static class DISABLED
							{
								// Token: 0x0400CD78 RID: 52600
								public static LocString NAME = "Disabled";

								// Token: 0x0400CD79 RID: 52601
								public static LocString TOOLTIP = "Unchecked: No Teleporters will spawn during Worldgen";
							}
						}
					}

					// Token: 0x020033C9 RID: 13257
					public class METEORSHOWERS
					{
						// Token: 0x0400CBF5 RID: 52213
						public static LocString NAME = "Meteor Showers";

						// Token: 0x0400CBF6 RID: 52214
						public static LocString TOOLTIP = "Adjusts the intensity of incoming space rocks";

						// Token: 0x0200343E RID: 13374
						public static class LEVELS
						{
							// Token: 0x02003475 RID: 13429
							public static class CLEAR_SKIES
							{
								// Token: 0x0400CD7A RID: 52602
								public static LocString NAME = "Clear Skies";

								// Token: 0x0400CD7B RID: 52603
								public static LocString TOOLTIP = "No meteor damage, no worries";
							}

							// Token: 0x02003476 RID: 13430
							public static class INFREQUENT
							{
								// Token: 0x0400CD7C RID: 52604
								public static LocString NAME = "Spring Showers";

								// Token: 0x0400CD7D RID: 52605
								public static LocString TOOLTIP = "Meteor showers are less frequent and less intense than usual";
							}

							// Token: 0x02003477 RID: 13431
							public static class DEFAULT
							{
								// Token: 0x0400CD7E RID: 52606
								public static LocString NAME = "Default";

								// Token: 0x0400CD7F RID: 52607
								public static LocString TOOLTIP = "Default meteor shower frequency and intensity";
							}

							// Token: 0x02003478 RID: 13432
							public static class INTENSE
							{
								// Token: 0x0400CD80 RID: 52608
								public static LocString NAME = "Cosmic Storm";

								// Token: 0x0400CD81 RID: 52609
								public static LocString TOOLTIP = "Meteor showers are more frequent and more intense than usual";
							}

							// Token: 0x02003479 RID: 13433
							public static class DOOMED
							{
								// Token: 0x0400CD82 RID: 52610
								public static LocString NAME = "Doomsday";

								// Token: 0x0400CD83 RID: 52611
								public static LocString TOOLTIP = "An onslaught of apocalyptic hailstorms that feels almost personal";
							}
						}
					}
				}
			}

			// Token: 0x02002574 RID: 9588
			public class MAINMENU
			{
				// Token: 0x0400A3A7 RID: 41895
				public static LocString STARTDEMO = "START DEMO";

				// Token: 0x0400A3A8 RID: 41896
				public static LocString NEWGAME = "NEW GAME";

				// Token: 0x0400A3A9 RID: 41897
				public static LocString RESUMEGAME = "RESUME GAME";

				// Token: 0x0400A3AA RID: 41898
				public static LocString LOADGAME = "LOAD GAME";

				// Token: 0x0400A3AB RID: 41899
				public static LocString RETIREDCOLONIES = "COLONY SUMMARIES";

				// Token: 0x0400A3AC RID: 41900
				public static LocString KLEIINVENTORY = "KLEI INVENTORY";

				// Token: 0x0400A3AD RID: 41901
				public static LocString LOCKERMENU = "SUPPLY CLOSET";

				// Token: 0x0400A3AE RID: 41902
				public static LocString SCENARIOS = "SCENARIOS";

				// Token: 0x0400A3AF RID: 41903
				public static LocString TRANSLATIONS = "TRANSLATIONS";

				// Token: 0x0400A3B0 RID: 41904
				public static LocString OPTIONS = "OPTIONS";

				// Token: 0x0400A3B1 RID: 41905
				public static LocString QUITTODESKTOP = "QUIT";

				// Token: 0x0400A3B2 RID: 41906
				public static LocString RESTARTCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400A3B3 RID: 41907
				public static LocString QUITCONFIRM = "Should I quit to the main menu?\nAll unsaved progress will be lost.";

				// Token: 0x0400A3B4 RID: 41908
				public static LocString RETIRECONFIRM = "Should I surrender under the soul-crushing weight of this universe's entropy and retire my colony?";

				// Token: 0x0400A3B5 RID: 41909
				public static LocString DESKTOPQUITCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400A3B6 RID: 41910
				public static LocString RESUMEBUTTON_BASENAME = "{0}: Cycle {1}";

				// Token: 0x02003088 RID: 12424
				public class DLC
				{
					// Token: 0x0400C3D4 RID: 50132
					public static LocString ACTIVATE_EXPANSION1 = "ACTIVATE DLC";

					// Token: 0x0400C3D5 RID: 50133
					public static LocString ACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to activate <i>Spaced Out!</i>";

					// Token: 0x0400C3D6 RID: 50134
					public static LocString ACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be activated the next time you launch the game. The game will now close.";

					// Token: 0x0400C3D7 RID: 50135
					public static LocString DEACTIVATE_EXPANSION1 = "DEACTIVATE DLC";

					// Token: 0x0400C3D8 RID: 50136
					public static LocString DEACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to activate the <i>Oxygen Not Included</i> base game.";

					// Token: 0x0400C3D9 RID: 50137
					public static LocString DEACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be deactivated the next time you launch the game. The game will now close.";

					// Token: 0x0400C3DA RID: 50138
					public static LocString AD_DLC1 = "Spaced Out! DLC";
				}
			}

			// Token: 0x02002575 RID: 9589
			public class DEVTOOLS
			{
				// Token: 0x0400A3B7 RID: 41911
				public static LocString TITLE = "About Dev Tools";

				// Token: 0x0400A3B8 RID: 41912
				public static LocString WARNING = "DANGER!!\n\nDev Tools are intended for developer use only. Using them may result in your save becoming unplayable, unstable, or severely damaged.\n\nThese tools are completely unsupported and may contain bugs. Are you sure you want to continue?";

				// Token: 0x0400A3B9 RID: 41913
				public static LocString DONTSHOW = "Do not show this message again";

				// Token: 0x0400A3BA RID: 41914
				public static LocString BUTTON = "Show Dev Tools";
			}

			// Token: 0x02002576 RID: 9590
			public class NEWGAMESETTINGS
			{
				// Token: 0x0400A3BB RID: 41915
				public static LocString HEADER = "GAME SETTINGS";

				// Token: 0x02003089 RID: 12425
				public class BUTTONS
				{
					// Token: 0x0400C3DB RID: 50139
					public static LocString STANDARDGAME = "Standard Game";

					// Token: 0x0400C3DC RID: 50140
					public static LocString CUSTOMGAME = "Custom Game";

					// Token: 0x0400C3DD RID: 50141
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400C3DE RID: 50142
					public static LocString STARTGAME = "Start Game";
				}
			}

			// Token: 0x02002577 RID: 9591
			public class COLONYDESTINATIONSCREEN
			{
				// Token: 0x0400A3BC RID: 41916
				public static LocString TITLE = "CHOOSE A DESTINATION";

				// Token: 0x0400A3BD RID: 41917
				public static LocString GENTLE_ZONE = "Habitable Zone";

				// Token: 0x0400A3BE RID: 41918
				public static LocString DETAILS = "Destination Details";

				// Token: 0x0400A3BF RID: 41919
				public static LocString START_SITE = "Immediate Surroundings";

				// Token: 0x0400A3C0 RID: 41920
				public static LocString COORDINATE = "Coordinates:";

				// Token: 0x0400A3C1 RID: 41921
				public static LocString CANCEL = "Back";

				// Token: 0x0400A3C2 RID: 41922
				public static LocString CUSTOMIZE = "Game Settings";

				// Token: 0x0400A3C3 RID: 41923
				public static LocString START_GAME = "Start Game";

				// Token: 0x0400A3C4 RID: 41924
				public static LocString SHUFFLE = "Shuffle";

				// Token: 0x0400A3C5 RID: 41925
				public static LocString SHUFFLETOOLTIP = "Reroll World Seed\n\nThis will shuffle the layout of your world and the geographical traits listed below";

				// Token: 0x0400A3C6 RID: 41926
				public static LocString SHUFFLETOOLTIP_DISABLED = "This world's seed is predetermined. It cannot be changed";

				// Token: 0x0400A3C7 RID: 41927
				public static LocString HEADER_ASTEROID_STARTING = "Starting Asteroid";

				// Token: 0x0400A3C8 RID: 41928
				public static LocString HEADER_ASTEROID_NEARBY = "Nearby Asteroids";

				// Token: 0x0400A3C9 RID: 41929
				public static LocString HEADER_ASTEROID_DISTANT = "Distant Asteroids";

				// Token: 0x0400A3CA RID: 41930
				public static LocString TRAITS_HEADER = "World Traits";

				// Token: 0x0400A3CB RID: 41931
				public static LocString STORY_TRAITS_HEADER = "Story Traits";

				// Token: 0x0400A3CC RID: 41932
				public static LocString NO_TRAITS = "No Traits";

				// Token: 0x0400A3CD RID: 41933
				public static LocString SINGLE_TRAIT = "1 Trait";

				// Token: 0x0400A3CE RID: 41934
				public static LocString TRAIT_COUNT = "{0} Traits";

				// Token: 0x0400A3CF RID: 41935
				public static LocString TOO_MANY_TRAITS_WARNING = UI.YELLOW_PREFIX + "Too many!" + UI.COLOR_SUFFIX;

				// Token: 0x0400A3D0 RID: 41936
				public static LocString TOO_MANY_TRAITS_WARNING_TOOLTIP = UI.YELLOW_PREFIX + "Squeezing this many story traits into this asteroid may cause worldgen to fail\n\nConsider lowering the number of story traits or changing the selected asteroid" + UI.COLOR_SUFFIX;

				// Token: 0x0400A3D1 RID: 41937
				public static LocString SHUFFLE_STORY_TRAITS_TOOLTIP = "Randomize Story Traits\n\nThis will select a comfortable number of story traits for the starting asteroid";

				// Token: 0x0400A3D2 RID: 41938
				public static LocString SELECTED_CLUSTER_TRAITS_HEADER = "Target Details";
			}

			// Token: 0x02002578 RID: 9592
			public class MODESELECTSCREEN
			{
				// Token: 0x0400A3D3 RID: 41939
				public static LocString HEADER = "GAME MODE";

				// Token: 0x0400A3D4 RID: 41940
				public static LocString BLANK_DESC = "Select a playstyle...";

				// Token: 0x0400A3D5 RID: 41941
				public static LocString SURVIVAL_TITLE = "SURVIVAL";

				// Token: 0x0400A3D6 RID: 41942
				public static LocString SURVIVAL_DESC = "Stay on your toes and one step ahead of this unforgiving world. One slip up could bring your colony crashing down.";

				// Token: 0x0400A3D7 RID: 41943
				public static LocString NOSWEAT_TITLE = "NO SWEAT";

				// Token: 0x0400A3D8 RID: 41944
				public static LocString NOSWEAT_DESC = "When disaster strikes (and it inevitably will), take a deep breath and stay calm. You have ample time to find a solution.";
			}

			// Token: 0x02002579 RID: 9593
			public class CLUSTERCATEGORYSELECTSCREEN
			{
				// Token: 0x0400A3D9 RID: 41945
				public static LocString HEADER = "ASTEROID STYLE";

				// Token: 0x0400A3DA RID: 41946
				public static LocString BLANK_DESC = "Select an asteroid style...";

				// Token: 0x0400A3DB RID: 41947
				public static LocString VANILLA_TITLE = "Standard";

				// Token: 0x0400A3DC RID: 41948
				public static LocString VANILLA_DESC = "Scenarios designed for classic gameplay.";

				// Token: 0x0400A3DD RID: 41949
				public static LocString CLASSIC_TITLE = "Classic";

				// Token: 0x0400A3DE RID: 41950
				public static LocString CLASSIC_DESC = "Scenarios similar to the <b>classic Oxygen Not Included</b> experience. Large starting asteroids with many resources.\nLess emphasis on space travel.";

				// Token: 0x0400A3DF RID: 41951
				public static LocString SPACEDOUT_TITLE = "Spaced Out!";

				// Token: 0x0400A3E0 RID: 41952
				public static LocString SPACEDOUT_DESC = "Scenarios designed for the <b>Spaced Out! DLC</b>.\nSmaller starting asteroids with resources distributed across the starmap. More emphasis on space travel.";

				// Token: 0x0400A3E1 RID: 41953
				public static LocString EVENT_TITLE = "The Lab";

				// Token: 0x0400A3E2 RID: 41954
				public static LocString EVENT_DESC = "Alternative gameplay experiences, including experimental scenarios designed for special events.";
			}

			// Token: 0x0200257A RID: 9594
			public class PATCHNOTESSCREEN
			{
				// Token: 0x0400A3E3 RID: 41955
				public static LocString HEADER = "IMPORTANT UPDATE NOTES";

				// Token: 0x0400A3E4 RID: 41956
				public static LocString OK_BUTTON = "OK";

				// Token: 0x0400A3E5 RID: 41957
				public static LocString FULLPATCHNOTES_TOOLTIP = "View the full patch notes online";
			}

			// Token: 0x0200257B RID: 9595
			public class MOTD
			{
				// Token: 0x0400A3E6 RID: 41958
				public static LocString IMAGE_HEADER = "OCTOBER 2023 QOL UPDATE";

				// Token: 0x0400A3E7 RID: 41959
				public static LocString NEWS_HEADER = "JOIN THE DISCUSSION";

				// Token: 0x0400A3E8 RID: 41960
				public static LocString NEWS_BODY = "Stay up to date by joining our mailing list, or head on over to the forums and join the discussion.";

				// Token: 0x0400A3E9 RID: 41961
				public static LocString PATCH_NOTES_SUMMARY = "This update includes:\n\n•<indent=20px>Critters can be moved with Move To tool.</indent>\n•<indent=20px>Blueprints in a colony's collection are recyclable.</indent>\n•<indent=20px>Added searchable element filter and other improvements to UI.</indent>\n•<indent=20px>Revised Industrial Machinery categorization for buildings.</indent>\n•<indent=20px>Bug fixes and more.</indent>\n\n   Check out the full patch notes for more details!";

				// Token: 0x0400A3EA RID: 41962
				public static LocString UPDATE_TEXT = "LAUNCHED!";

				// Token: 0x0400A3EB RID: 41963
				public static LocString UPDATE_TEXT_EXPANSION1 = "LAUNCHED!";
			}

			// Token: 0x0200257C RID: 9596
			public class LOADSCREEN
			{
				// Token: 0x0400A3EC RID: 41964
				public static LocString TITLE = "LOAD GAME";

				// Token: 0x0400A3ED RID: 41965
				public static LocString TITLE_INSPECT = "LOAD GAME";

				// Token: 0x0400A3EE RID: 41966
				public static LocString DELETEBUTTON = "DELETE";

				// Token: 0x0400A3EF RID: 41967
				public static LocString BACKBUTTON = "< BACK";

				// Token: 0x0400A3F0 RID: 41968
				public static LocString CONFIRMDELETE = "Are you sure you want to delete {0}?\nYou cannot undo this action.";

				// Token: 0x0400A3F1 RID: 41969
				public static LocString SAVEDETAILS = "<b>File:</b> {0}\n\n<b>Save Date:</b>\n{1}\n\n<b>Base Name:</b> {2}\n<b>Duplicants Alive:</b> {3}\n<b>Cycle(s) Survived:</b> {4}";

				// Token: 0x0400A3F2 RID: 41970
				public static LocString AUTOSAVEWARNING = "<color=#F44A47FF>Autosave: This file will get deleted as new autosaves are created</color>";

				// Token: 0x0400A3F3 RID: 41971
				public static LocString CORRUPTEDSAVE = "<b><color=#F44A47FF>Could not load file {0}. Its data may be corrupted.</color></b>";

				// Token: 0x0400A3F4 RID: 41972
				public static LocString SAVE_FROM_SPACED_OUT_TOOLTIP = "<color=#F44A47FF>This save was created in the <i>Spaced Out!</i> DLC. Activate the DLC to play it</color>";

				// Token: 0x0400A3F5 RID: 41973
				public static LocString SAVE_FROM_VANILLA_TOOLTIP = "<color=#F44A47FF>This save was created in the base game. Deactivate the <i>Spaced Out!</i> DLC to play it.</color>";

				// Token: 0x0400A3F6 RID: 41974
				public static LocString SAVE_IS_SPACED_OUT_TOOLTIP = "<i>Spaced Out!</i> DLC save";

				// Token: 0x0400A3F7 RID: 41975
				public static LocString SAVE_IS_VANILLA_TOOLTIP = "Base game save";

				// Token: 0x0400A3F8 RID: 41976
				public static LocString SAVE_TOO_NEW = "<b><color=#F44A47FF>Could not load file {0}. File is using build {1}, v{2}. This build is {3}, v{4}.</color></b>";

				// Token: 0x0400A3F9 RID: 41977
				public static LocString SAVE_MISSING_CONTENT = "<b><color=#F44A47FF>Could not load file {0}. File was saved with content that is not currently installed.</color></b>";

				// Token: 0x0400A3FA RID: 41978
				public static LocString UNSUPPORTED_SAVE_VERSION = "<b><color=#F44A47FF>This save file is from a previous version of the game and is no longer supported.</color></b>";

				// Token: 0x0400A3FB RID: 41979
				public static LocString MORE_INFO = "More Info";

				// Token: 0x0400A3FC RID: 41980
				public static LocString NEWEST_SAVE = "Newest Save";

				// Token: 0x0400A3FD RID: 41981
				public static LocString BASE_NAME = "Base Name";

				// Token: 0x0400A3FE RID: 41982
				public static LocString CYCLES_SURVIVED = "Cycles Survived";

				// Token: 0x0400A3FF RID: 41983
				public static LocString DUPLICANTS_ALIVE = "Duplicants Alive";

				// Token: 0x0400A400 RID: 41984
				public static LocString WORLD_NAME = "Asteroid Type";

				// Token: 0x0400A401 RID: 41985
				public static LocString NO_FILE_SELECTED = "No file selected";

				// Token: 0x0400A402 RID: 41986
				public static LocString COLONY_INFO_FMT = "{0}: {1}";

				// Token: 0x0400A403 RID: 41987
				public static LocString LOAD_MORE_COLONIES_BUTTON = "Load more...";

				// Token: 0x0400A404 RID: 41988
				public static LocString VANILLA_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content disabled";

				// Token: 0x0400A405 RID: 41989
				public static LocString EXPANSION1_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content enabled";

				// Token: 0x0400A406 RID: 41990
				public static LocString UNSUPPORTED_VANILLA_TEMP = "<b><color=#F44A47FF>This save file is from the base version of the game and currently cannot be loaded while " + UI.DLC1.NAME_ITAL + " is installed.</color></b>";

				// Token: 0x0400A407 RID: 41991
				public static LocString CONTENT = "Content";

				// Token: 0x0400A408 RID: 41992
				public static LocString VANILLA_CONTENT = "Vanilla FIXME";

				// Token: 0x0400A409 RID: 41993
				public static LocString EXPANSION1_CONTENT = UI.DLC1.NAME_ITAL + " Expansion FIXME";

				// Token: 0x0400A40A RID: 41994
				public static LocString SAVE_INFO = "{0} saves  {1} autosaves  {2}";

				// Token: 0x0400A40B RID: 41995
				public static LocString COLONIES_TITLE = "Colony View";

				// Token: 0x0400A40C RID: 41996
				public static LocString COLONY_TITLE = "Viewing colony '{0}'";

				// Token: 0x0400A40D RID: 41997
				public static LocString COLONY_FILE_SIZE = "Size: {0}";

				// Token: 0x0400A40E RID: 41998
				public static LocString COLONY_FILE_NAME = "File: '{0}'";

				// Token: 0x0400A40F RID: 41999
				public static LocString NO_PREVIEW = "NO PREVIEW";

				// Token: 0x0400A410 RID: 42000
				public static LocString LOCAL_SAVE = "local";

				// Token: 0x0400A411 RID: 42001
				public static LocString CLOUD_SAVE = "cloud";

				// Token: 0x0400A412 RID: 42002
				public static LocString CONVERT_COLONY = "CONVERT COLONY";

				// Token: 0x0400A413 RID: 42003
				public static LocString CONVERT_ALL_COLONIES = "CONVERT ALL";

				// Token: 0x0400A414 RID: 42004
				public static LocString CONVERT_ALL_WARNING = UI.PRE_KEYWORD + "\nWarning:" + UI.PST_KEYWORD + " Converting all colonies may take some time.";

				// Token: 0x0400A415 RID: 42005
				public static LocString SAVE_INFO_DIALOG_TITLE = "SAVE INFORMATION";

				// Token: 0x0400A416 RID: 42006
				public static LocString SAVE_INFO_DIALOG_TEXT = "Access your save files using the options below.";

				// Token: 0x0400A417 RID: 42007
				public static LocString SAVE_INFO_DIALOG_TOOLTIP = "Access your save file locations from here.";

				// Token: 0x0400A418 RID: 42008
				public static LocString CONVERT_ERROR_TITLE = "SAVE CONVERSION UNSUCCESSFUL";

				// Token: 0x0400A419 RID: 42009
				public static LocString CONVERT_ERROR = string.Concat(new string[]
				{
					"Converting the colony ",
					UI.PRE_KEYWORD,
					"{Colony}",
					UI.PST_KEYWORD,
					" was unsuccessful!\nThe error was:\n\n<b>{Error}</b>\n\nPlease try again, or post a bug in the forums if this problem keeps happening."
				});

				// Token: 0x0400A41A RID: 42010
				public static LocString CONVERT_TO_CLOUD = "CONVERT TO CLOUD SAVES";

				// Token: 0x0400A41B RID: 42011
				public static LocString CONVERT_TO_LOCAL = "CONVERT TO LOCAL SAVES";

				// Token: 0x0400A41C RID: 42012
				public static LocString CONVERT_COLONY_TO_CLOUD = "Convert colony to use cloud saves";

				// Token: 0x0400A41D RID: 42013
				public static LocString CONVERT_COLONY_TO_LOCAL = "Convert to colony to use local saves";

				// Token: 0x0400A41E RID: 42014
				public static LocString CONVERT_ALL_TO_CLOUD = "Convert <b>all</b> colonies below to use cloud saves";

				// Token: 0x0400A41F RID: 42015
				public static LocString CONVERT_ALL_TO_LOCAL = "Convert <b>all</b> colonies below to use local saves";

				// Token: 0x0400A420 RID: 42016
				public static LocString CONVERT_ALL_TO_CLOUD_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400A421 RID: 42017
				public static LocString CONVERT_ALL_TO_LOCAL_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400A422 RID: 42018
				public static LocString CONVERT_TO_CLOUD_DETAILS = "Converting a colony to use cloud saves will move all of the save files for that colony into the cloud saves folder.\n\nThis allows your game platform to sync this colony to the cloud for your account, so it can be played on multiple machines.";

				// Token: 0x0400A423 RID: 42019
				public static LocString CONVERT_TO_LOCAL_DETAILS = "Converting a colony to NOT use cloud saves will move all of the save files for that colony into the local saves folder.\n\n" + UI.PRE_KEYWORD + "These save files will no longer be synced to the cloud." + UI.PST_KEYWORD;

				// Token: 0x0400A424 RID: 42020
				public static LocString OPEN_SAVE_FOLDER = "LOCAL SAVES";

				// Token: 0x0400A425 RID: 42021
				public static LocString OPEN_CLOUDSAVE_FOLDER = "CLOUD SAVES";

				// Token: 0x0400A426 RID: 42022
				public static LocString MIGRATE_TITLE = "SAVE FILE MIGRATION";

				// Token: 0x0400A427 RID: 42023
				public static LocString MIGRATE_SAVE_FILES = "MIGRATE SAVE FILES";

				// Token: 0x0400A428 RID: 42024
				public static LocString MIGRATE_COUNT = string.Concat(new string[]
				{
					"\nFound ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" autosaves that require migration."
				});

				// Token: 0x0400A429 RID: 42025
				public static LocString MIGRATE_RESULT = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400A42A RID: 42026
				public static LocString MIGRATE_RESULT_FAILURES = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"<b>WARNING:</b> Not all saves could be migrated.",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves.\n\nThe file ",
					UI.PRE_KEYWORD,
					"{ErrorColony}",
					UI.PST_KEYWORD,
					" encountered this error:\n\n<b>{ErrorMessage}</b>"
				});

				// Token: 0x0400A42B RID: 42027
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE = "MIGRATION INCOMPLETE";

				// Token: 0x0400A42C RID: 42028
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_PRE = "<b>The game was unable to move all save files to their new location.\nTo fix this, please:</b>\n\n";

				// Token: 0x0400A42D RID: 42029
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1 = "    1. Try temporarily disabling virus scanners and malware\n         protection programs.";

				// Token: 0x0400A42E RID: 42030
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2 = "    2. Turn off file sync services such as OneDrive and DropBox.";

				// Token: 0x0400A42F RID: 42031
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3 = "    3. Restart the game to retry file migration.";

				// Token: 0x0400A430 RID: 42032
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_POST = "\n<b>If this still doesn't solve the problem, please post a bug in the forums and we will attempt to assist with your issue.</b>";

				// Token: 0x0400A431 RID: 42033
				public static LocString MIGRATE_INFO = "We've changed how save files are organized!\nPlease " + UI.CLICK(UI.ClickType.click) + " the button below to automatically update your save file storage.";

				// Token: 0x0400A432 RID: 42034
				public static LocString MIGRATE_DONE = "CONTINUE";

				// Token: 0x0400A433 RID: 42035
				public static LocString MIGRATE_FAILURES_FORUM_BUTTON = "VISIT FORUMS";

				// Token: 0x0400A434 RID: 42036
				public static LocString MIGRATE_FAILURES_DONE = "MORE INFO";

				// Token: 0x0400A435 RID: 42037
				public static LocString CLOUD_TUTORIAL_BOUNCER = "Upload Saves to Cloud";
			}

			// Token: 0x0200257D RID: 9597
			public class SAVESCREEN
			{
				// Token: 0x0400A436 RID: 42038
				public static LocString TITLE = "SAVE SLOTS";

				// Token: 0x0400A437 RID: 42039
				public static LocString NEWSAVEBUTTON = "New Save";

				// Token: 0x0400A438 RID: 42040
				public static LocString OVERWRITEMESSAGE = "Are you sure you want to overwrite {0}?";

				// Token: 0x0400A439 RID: 42041
				public static LocString SAVENAMETITLE = "SAVE NAME";

				// Token: 0x0400A43A RID: 42042
				public static LocString CONFIRMNAME = "Confirm";

				// Token: 0x0400A43B RID: 42043
				public static LocString CANCELNAME = "Cancel";

				// Token: 0x0400A43C RID: 42044
				public static LocString IO_ERROR = "An error occurred trying to save your game. Please ensure there is sufficient disk space.\n\n{0}";

				// Token: 0x0400A43D RID: 42045
				public static LocString REPORT_BUG = "Report Bug";
			}

			// Token: 0x0200257E RID: 9598
			public class RAILFORCEQUIT
			{
				// Token: 0x0400A43E RID: 42046
				public static LocString SAVE_EXIT = "Play time has expired and the game is exiting. Would you like to overwrite {0}?";

				// Token: 0x0400A43F RID: 42047
				public static LocString WARN_EXIT = "Play time has expired and the game will now exit.";

				// Token: 0x0400A440 RID: 42048
				public static LocString DLC_NOT_PURCHASED = "The <i>Spaced Out!</i> DLC has not yet been purchased in the WeGame store. Purchase <i>Spaced Out!</i> to support <i>Oxygen Not Included</i> and enjoy the new content!";
			}

			// Token: 0x0200257F RID: 9599
			public class MOD_ERRORS
			{
				// Token: 0x0400A441 RID: 42049
				public static LocString TITLE = "MOD ERRORS";

				// Token: 0x0400A442 RID: 42050
				public static LocString DETAILS = "DETAILS";

				// Token: 0x0400A443 RID: 42051
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002580 RID: 9600
			public class MODS
			{
				// Token: 0x0400A444 RID: 42052
				public static LocString TITLE = "MODS";

				// Token: 0x0400A445 RID: 42053
				public static LocString MANAGE = "Subscription";

				// Token: 0x0400A446 RID: 42054
				public static LocString MANAGE_LOCAL = "Browse";

				// Token: 0x0400A447 RID: 42055
				public static LocString WORKSHOP = "STEAM WORKSHOP";

				// Token: 0x0400A448 RID: 42056
				public static LocString ENABLE_ALL = "ENABLE ALL";

				// Token: 0x0400A449 RID: 42057
				public static LocString DISABLE_ALL = "DISABLE ALL";

				// Token: 0x0400A44A RID: 42058
				public static LocString DRAG_TO_REORDER = "Drag to reorder";

				// Token: 0x0400A44B RID: 42059
				public static LocString REQUIRES_RESTART = "Mod changes require restart";

				// Token: 0x0400A44C RID: 42060
				public static LocString FAILED_TO_LOAD = "A mod failed to load and is being disabled:\n\n{0}: {1}\n\n{2}";

				// Token: 0x0400A44D RID: 42061
				public static LocString DB_CORRUPT = "An error occurred trying to load the Mod Database.\n\n{0}";

				// Token: 0x0200308A RID: 12426
				public class CONTENT_FAILURE
				{
					// Token: 0x0400C3DF RID: 50143
					public static LocString DISABLED_CONTENT = " - <b>Not compatible with <i>{Content}</i></b>";

					// Token: 0x0400C3E0 RID: 50144
					public static LocString NO_CONTENT = " - <b>No compatible mod found</b>";

					// Token: 0x0400C3E1 RID: 50145
					public static LocString OLD_API = " - <b>Mod out-of-date</b>";
				}

				// Token: 0x0200308B RID: 12427
				public class TOOLTIPS
				{
					// Token: 0x0400C3E2 RID: 50146
					public static LocString ENABLED = "Enabled";

					// Token: 0x0400C3E3 RID: 50147
					public static LocString DISABLED = "Disabled";

					// Token: 0x0400C3E4 RID: 50148
					public static LocString MANAGE_STEAM_SUBSCRIPTION = "Manage Steam Subscription";

					// Token: 0x0400C3E5 RID: 50149
					public static LocString MANAGE_RAIL_SUBSCRIPTION = "Manage Subscription";

					// Token: 0x0400C3E6 RID: 50150
					public static LocString MANAGE_LOCAL_MOD = "Manage Local Mod";
				}

				// Token: 0x0200308C RID: 12428
				public class RAILMODUPLOAD
				{
					// Token: 0x0400C3E7 RID: 50151
					public static LocString TITLE = "Upload Mod";

					// Token: 0x0400C3E8 RID: 50152
					public static LocString NAME = "Mod Name";

					// Token: 0x0400C3E9 RID: 50153
					public static LocString DESCRIPTION = "Mod Description";

					// Token: 0x0400C3EA RID: 50154
					public static LocString VERSION = "Version Number";

					// Token: 0x0400C3EB RID: 50155
					public static LocString PREVIEW_IMAGE = "Preview Image Path";

					// Token: 0x0400C3EC RID: 50156
					public static LocString CONTENT_FOLDER = "Content Folder Path";

					// Token: 0x0400C3ED RID: 50157
					public static LocString SHARE_TYPE = "Share Type";

					// Token: 0x0400C3EE RID: 50158
					public static LocString SUBMIT = "Submit";

					// Token: 0x0400C3EF RID: 50159
					public static LocString SUBMIT_READY = "This mod is ready to submit";

					// Token: 0x0400C3F0 RID: 50160
					public static LocString SUBMIT_NOT_READY = "The mod cannot be submitted. Check that all fields are properly entered and that the paths are valid.";

					// Token: 0x020033CA RID: 13258
					public static class MOD_SHARE_TYPE
					{
						// Token: 0x0400CBF7 RID: 52215
						public static LocString PRIVATE = "Private";

						// Token: 0x0400CBF8 RID: 52216
						public static LocString TOOLTIP_PRIVATE = "This mod will only be visible to its creator";

						// Token: 0x0400CBF9 RID: 52217
						public static LocString FRIEND = "Friend";

						// Token: 0x0400CBFA RID: 52218
						public static LocString TOOLTIP_FRIEND = "Friend";

						// Token: 0x0400CBFB RID: 52219
						public static LocString PUBLIC = "Public";

						// Token: 0x0400CBFC RID: 52220
						public static LocString TOOLTIP_PUBLIC = "This mod will be available to all players after publishing. It may be subject to review before being allowed to be published.";
					}

					// Token: 0x020033CB RID: 13259
					public static class MOD_UPLOAD_RESULT
					{
						// Token: 0x0400CBFD RID: 52221
						public static LocString SUCCESS = "Mod upload succeeded.";

						// Token: 0x0400CBFE RID: 52222
						public static LocString FAILURE = "Mod upload failed.";
					}
				}
			}

			// Token: 0x02002581 RID: 9601
			public class MOD_EVENTS
			{
				// Token: 0x0400A44E RID: 42062
				public static LocString REQUIRED = "REQUIRED";

				// Token: 0x0400A44F RID: 42063
				public static LocString NOT_FOUND = "NOT FOUND";

				// Token: 0x0400A450 RID: 42064
				public static LocString INSTALL_INFO_INACCESSIBLE = "INACCESSIBLE";

				// Token: 0x0400A451 RID: 42065
				public static LocString OUT_OF_ORDER = "ORDERING CHANGED";

				// Token: 0x0400A452 RID: 42066
				public static LocString ACTIVE_DURING_CRASH = "ACTIVE DURING CRASH";

				// Token: 0x0400A453 RID: 42067
				public static LocString EXPECTED_ENABLED = "NEWLY DISABLED";

				// Token: 0x0400A454 RID: 42068
				public static LocString EXPECTED_DISABLED = "NEWLY ENABLED";

				// Token: 0x0400A455 RID: 42069
				public static LocString VERSION_UPDATE = "VERSION UPDATE";

				// Token: 0x0400A456 RID: 42070
				public static LocString AVAILABLE_CONTENT_CHANGED = "CONTENT CHANGED";

				// Token: 0x0400A457 RID: 42071
				public static LocString INSTALL_FAILED = "INSTALL FAILED";

				// Token: 0x0400A458 RID: 42072
				public static LocString INSTALLED = "INSTALLED";

				// Token: 0x0400A459 RID: 42073
				public static LocString UNINSTALLED = "UNINSTALLED";

				// Token: 0x0400A45A RID: 42074
				public static LocString REQUIRES_RESTART = "RESTART REQUIRED";

				// Token: 0x0400A45B RID: 42075
				public static LocString BAD_WORLD_GEN = "LOAD FAILED";

				// Token: 0x0400A45C RID: 42076
				public static LocString DEACTIVATED = "DEACTIVATED";

				// Token: 0x0400A45D RID: 42077
				public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "DEACTIVATED";

				// Token: 0x0200308D RID: 12429
				public class TOOLTIPS
				{
					// Token: 0x0400C3F1 RID: 50161
					public static LocString REQUIRED = "The current save game couldn't load this mod. Unexpected things may happen!";

					// Token: 0x0400C3F2 RID: 50162
					public static LocString NOT_FOUND = "This mod isn't installed";

					// Token: 0x0400C3F3 RID: 50163
					public static LocString INSTALL_INFO_INACCESSIBLE = "Mod files are inaccessible";

					// Token: 0x0400C3F4 RID: 50164
					public static LocString OUT_OF_ORDER = "Active mod has changed order with respect to some other active mod";

					// Token: 0x0400C3F5 RID: 50165
					public static LocString ACTIVE_DURING_CRASH = "Mod was active during a crash and may be the cause";

					// Token: 0x0400C3F6 RID: 50166
					public static LocString EXPECTED_ENABLED = "This mod needs to be enabled";

					// Token: 0x0400C3F7 RID: 50167
					public static LocString EXPECTED_DISABLED = "This mod needs to be disabled";

					// Token: 0x0400C3F8 RID: 50168
					public static LocString VERSION_UPDATE = "New version detected";

					// Token: 0x0400C3F9 RID: 50169
					public static LocString AVAILABLE_CONTENT_CHANGED = "Content added or removed";

					// Token: 0x0400C3FA RID: 50170
					public static LocString INSTALL_FAILED = "Installation failed";

					// Token: 0x0400C3FB RID: 50171
					public static LocString INSTALLED = "Installation succeeded";

					// Token: 0x0400C3FC RID: 50172
					public static LocString UNINSTALLED = "Uninstalled";

					// Token: 0x0400C3FD RID: 50173
					public static LocString BAD_WORLD_GEN = "Encountered an error while loading file";

					// Token: 0x0400C3FE RID: 50174
					public static LocString DEACTIVATED = "Deactivated due to errors";

					// Token: 0x0400C3FF RID: 50175
					public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "Deactivated due to Early Access for " + UI.DLC1.NAME_ITAL;
				}
			}

			// Token: 0x02002582 RID: 9602
			public class MOD_DIALOGS
			{
				// Token: 0x0400A45E RID: 42078
				public static LocString ADDITIONAL_MOD_EVENTS = "(...additional entries omitted)";

				// Token: 0x0200308E RID: 12430
				public class INSTALL_INFO_INACCESSIBLE
				{
					// Token: 0x0400C400 RID: 50176
					public static LocString TITLE = "STEAM CONTENT ERROR";

					// Token: 0x0400C401 RID: 50177
					public static LocString MESSAGE = "Failed to access local Steam files for mod {0}.\nTry restarting Oxygen not Included.\nIf that doesn't work, try re-subscribing to the mod via Steam.";
				}

				// Token: 0x0200308F RID: 12431
				public class STEAM_SUBSCRIBED
				{
					// Token: 0x0400C402 RID: 50178
					public static LocString TITLE = "STEAM MOD SUBSCRIBED";

					// Token: 0x0400C403 RID: 50179
					public static LocString MESSAGE = "Subscribed to Steam mod: {0}";
				}

				// Token: 0x02003090 RID: 12432
				public class STEAM_UPDATED
				{
					// Token: 0x0400C404 RID: 50180
					public static LocString TITLE = "STEAM MOD UPDATE";

					// Token: 0x0400C405 RID: 50181
					public static LocString MESSAGE = "Updating version of Steam mod: {0}";
				}

				// Token: 0x02003091 RID: 12433
				public class STEAM_UNSUBSCRIBED
				{
					// Token: 0x0400C406 RID: 50182
					public static LocString TITLE = "STEAM MOD UNSUBSCRIBED";

					// Token: 0x0400C407 RID: 50183
					public static LocString MESSAGE = "Unsubscribed from Steam mod: {0}";
				}

				// Token: 0x02003092 RID: 12434
				public class STEAM_REFRESH
				{
					// Token: 0x0400C408 RID: 50184
					public static LocString TITLE = "STEAM MODS REFRESHED";

					// Token: 0x0400C409 RID: 50185
					public static LocString MESSAGE = "Refreshed Steam mods:\n{0}";
				}

				// Token: 0x02003093 RID: 12435
				public class ALL_MODS_DISABLED_EARLY_ACCESS
				{
					// Token: 0x0400C40A RID: 50186
					public static LocString TITLE = "ALL MODS DISABLED";

					// Token: 0x0400C40B RID: 50187
					public static LocString MESSAGE = "Mod support is temporarily suspended for the initial launch of " + UI.DLC1.NAME_ITAL + " into Early Access:\n{0}";
				}

				// Token: 0x02003094 RID: 12436
				public class LOAD_FAILURE
				{
					// Token: 0x0400C40C RID: 50188
					public static LocString TITLE = "LOAD FAILURE";

					// Token: 0x0400C40D RID: 50189
					public static LocString MESSAGE = "Failed to load one or more mods:\n{0}\nThey will be re-installed when the game is restarted.\nGame may be unstable until restarted.";
				}

				// Token: 0x02003095 RID: 12437
				public class SAVE_GAME_MODS_DIFFER
				{
					// Token: 0x0400C40E RID: 50190
					public static LocString TITLE = "MOD DIFFERENCES";

					// Token: 0x0400C40F RID: 50191
					public static LocString MESSAGE = "Save game mods differ from currently active mods:\n{0}";
				}

				// Token: 0x02003096 RID: 12438
				public class MOD_ERRORS_ON_BOOT
				{
					// Token: 0x0400C410 RID: 50192
					public static LocString TITLE = "MOD ERRORS";

					// Token: 0x0400C411 RID: 50193
					public static LocString MESSAGE = "An error occurred during start-up with mods active.\nAll mods have been disabled to ensure a clean restart.\n{0}";

					// Token: 0x0400C412 RID: 50194
					public static LocString DEV_MESSAGE = "An error occurred during start-up with mods active.\n{0}\nDisable all mods and restart, or continue in an unstable state?";
				}

				// Token: 0x02003097 RID: 12439
				public class MODS_SCREEN_CHANGES
				{
					// Token: 0x0400C413 RID: 50195
					public static LocString TITLE = "MODS CHANGED";

					// Token: 0x0400C414 RID: 50196
					public static LocString MESSAGE = "{0}\nRestart required to reload mods.\nGame may be unstable until restarted.";
				}

				// Token: 0x02003098 RID: 12440
				public class MOD_EVENTS
				{
					// Token: 0x0400C415 RID: 50197
					public static LocString TITLE = "MOD EVENTS";

					// Token: 0x0400C416 RID: 50198
					public static LocString MESSAGE = "{0}";

					// Token: 0x0400C417 RID: 50199
					public static LocString DEV_MESSAGE = "{0}\nCheck Player.log for details.";
				}

				// Token: 0x02003099 RID: 12441
				public class RESTART
				{
					// Token: 0x0400C418 RID: 50200
					public static LocString OK = "RESTART";

					// Token: 0x0400C419 RID: 50201
					public static LocString CANCEL = "CONTINUE";

					// Token: 0x0400C41A RID: 50202
					public static LocString MESSAGE = "{0}\nRestart required.";

					// Token: 0x0400C41B RID: 50203
					public static LocString DEV_MESSAGE = "{0}\nRestart required.\nGame may be unstable until restarted.";
				}
			}

			// Token: 0x02002583 RID: 9603
			public class PAUSE_SCREEN
			{
				// Token: 0x0400A45F RID: 42079
				public static LocString TITLE = "PAUSED";

				// Token: 0x0400A460 RID: 42080
				public static LocString RESUME = "Resume";

				// Token: 0x0400A461 RID: 42081
				public static LocString LOGBOOK = "Logbook";

				// Token: 0x0400A462 RID: 42082
				public static LocString OPTIONS = "Options";

				// Token: 0x0400A463 RID: 42083
				public static LocString SAVE = "Save";

				// Token: 0x0400A464 RID: 42084
				public static LocString SAVEAS = "Save As";

				// Token: 0x0400A465 RID: 42085
				public static LocString COLONY_SUMMARY = "Colony Summary";

				// Token: 0x0400A466 RID: 42086
				public static LocString LOCKERMENU = "Supply Closet";

				// Token: 0x0400A467 RID: 42087
				public static LocString LOAD = "Load";

				// Token: 0x0400A468 RID: 42088
				public static LocString QUIT = "Main Menu";

				// Token: 0x0400A469 RID: 42089
				public static LocString DESKTOPQUIT = "Quit to Desktop";

				// Token: 0x0400A46A RID: 42090
				public static LocString WORLD_SEED = "Coordinates: {0}";

				// Token: 0x0400A46B RID: 42091
				public static LocString WORLD_SEED_TOOLTIP = "Share coordinates with a friend and they can start a colony on an identical asteroid!\n\n{0} - The asteroid\n\n{1} - The world seed\n\n{2} - Difficulty and Custom settings\n\n{3} - Story Trait settings";

				// Token: 0x0400A46C RID: 42092
				public static LocString WORLD_SEED_COPY_TOOLTIP = "Copy Coordinates to clipboard\n\nShare coordinates with a friend and they can start a colony on an identical asteroid!";

				// Token: 0x0400A46D RID: 42093
				public static LocString MANAGEMENT_BUTTON = "Pause Menu";
			}

			// Token: 0x02002584 RID: 9604
			public class OPTIONS_SCREEN
			{
				// Token: 0x0400A46E RID: 42094
				public static LocString TITLE = "OPTIONS";

				// Token: 0x0400A46F RID: 42095
				public static LocString GRAPHICS = "Graphics";

				// Token: 0x0400A470 RID: 42096
				public static LocString AUDIO = "Audio";

				// Token: 0x0400A471 RID: 42097
				public static LocString GAME = "Game";

				// Token: 0x0400A472 RID: 42098
				public static LocString CONTROLS = "Controls";

				// Token: 0x0400A473 RID: 42099
				public static LocString UNITS = "Temperature Units";

				// Token: 0x0400A474 RID: 42100
				public static LocString METRICS = "Data Communication";

				// Token: 0x0400A475 RID: 42101
				public static LocString LANGUAGE = "Change Language";

				// Token: 0x0400A476 RID: 42102
				public static LocString WORLD_GEN = "World Generation Key";

				// Token: 0x0400A477 RID: 42103
				public static LocString RESET_TUTORIAL = "Reset Tutorial Messages";

				// Token: 0x0400A478 RID: 42104
				public static LocString RESET_TUTORIAL_WARNING = "All tutorial messages will be reset, and\nwill show up again the next time you play the game.";

				// Token: 0x0400A479 RID: 42105
				public static LocString FEEDBACK = "Feedback";

				// Token: 0x0400A47A RID: 42106
				public static LocString CREDITS = "Credits";

				// Token: 0x0400A47B RID: 42107
				public static LocString BACK = "Done";

				// Token: 0x0400A47C RID: 42108
				public static LocString UNLOCK_SANDBOX = "Unlock Sandbox Mode";

				// Token: 0x0400A47D RID: 42109
				public static LocString MODS = "MODS";

				// Token: 0x0400A47E RID: 42110
				public static LocString SAVE_OPTIONS = "Save Options";

				// Token: 0x0200309A RID: 12442
				public class TOGGLE_SANDBOX_SCREEN
				{
					// Token: 0x0400C41C RID: 50204
					public static LocString UNLOCK_SANDBOX_WARNING = "Sandbox Mode will be enabled for this save file";

					// Token: 0x0400C41D RID: 50205
					public static LocString CONFIRM = "Enable Sandbox Mode";

					// Token: 0x0400C41E RID: 50206
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400C41F RID: 50207
					public static LocString CONFIRM_SAVE_BACKUP = "Enable Sandbox Mode, but save a backup first";

					// Token: 0x0400C420 RID: 50208
					public static LocString BACKUP_SAVE_GAME_APPEND = " (BACKUP)";
				}
			}

			// Token: 0x02002585 RID: 9605
			public class INPUT_BINDINGS_SCREEN
			{
				// Token: 0x0400A47F RID: 42111
				public static LocString TITLE = "CUSTOMIZE KEYS";

				// Token: 0x0400A480 RID: 42112
				public static LocString RESET = "Reset";

				// Token: 0x0400A481 RID: 42113
				public static LocString APPLY = "Done";

				// Token: 0x0400A482 RID: 42114
				public static LocString DUPLICATE = "{0} was already bound to {1} and is now unbound.";

				// Token: 0x0400A483 RID: 42115
				public static LocString UNBOUND_ACTION = "{0} is unbound. Are you sure you want to continue?";

				// Token: 0x0400A484 RID: 42116
				public static LocString MULTIPLE_UNBOUND_ACTIONS = "You have multiple unbound actions, this may result in difficulty playing the game. Are you sure you want to continue?";

				// Token: 0x0400A485 RID: 42117
				public static LocString WAITING_FOR_INPUT = "???";
			}

			// Token: 0x02002586 RID: 9606
			public class TRANSLATIONS_SCREEN
			{
				// Token: 0x0400A486 RID: 42118
				public static LocString TITLE = "TRANSLATIONS";

				// Token: 0x0400A487 RID: 42119
				public static LocString UNINSTALL = "Uninstall";

				// Token: 0x0400A488 RID: 42120
				public static LocString PREINSTALLED_HEADER = "Preinstalled Language Packs";

				// Token: 0x0400A489 RID: 42121
				public static LocString UGC_HEADER = "Subscribed Workshop Language Packs";

				// Token: 0x0400A48A RID: 42122
				public static LocString UGC_MOD_TITLE_FORMAT = "{0} (workshop)";

				// Token: 0x0400A48B RID: 42123
				public static LocString ARE_YOU_SURE = "Are you sure you want to uninstall this language pack?";

				// Token: 0x0400A48C RID: 42124
				public static LocString PLEASE_REBOOT = "Please restart your game for these changes to take effect.";

				// Token: 0x0400A48D RID: 42125
				public static LocString NO_PACKS = "Steam Workshop";

				// Token: 0x0400A48E RID: 42126
				public static LocString DOWNLOAD = "Start Download";

				// Token: 0x0400A48F RID: 42127
				public static LocString INSTALL = "Install";

				// Token: 0x0400A490 RID: 42128
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400A491 RID: 42129
				public static LocString NO_STEAM = "Unable to retrieve language list from Steam";

				// Token: 0x0400A492 RID: 42130
				public static LocString RESTART = "RESTART";

				// Token: 0x0400A493 RID: 42131
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400A494 RID: 42132
				public static LocString MISSING_LANGUAGE_PACK = "Selected language pack ({0}) not found.\nReverting to default language.";

				// Token: 0x0400A495 RID: 42133
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x0200309B RID: 12443
				public class PREINSTALLED_LANGUAGES
				{
					// Token: 0x0400C421 RID: 50209
					public static LocString EN = "English (Klei)";

					// Token: 0x0400C422 RID: 50210
					public static LocString ZH_KLEI = "Chinese (Klei)";

					// Token: 0x0400C423 RID: 50211
					public static LocString KO_KLEI = "Korean (Klei)";

					// Token: 0x0400C424 RID: 50212
					public static LocString RU_KLEI = "Russian (Klei)";
				}
			}

			// Token: 0x02002587 RID: 9607
			public class SCENARIOS_MENU
			{
				// Token: 0x0400A496 RID: 42134
				public static LocString TITLE = "Scenarios";

				// Token: 0x0400A497 RID: 42135
				public static LocString UNSUBSCRIBE = "Unsubscribe";

				// Token: 0x0400A498 RID: 42136
				public static LocString UNSUBSCRIBE_CONFIRM = "Are you sure you want to unsubscribe from this scenario?";

				// Token: 0x0400A499 RID: 42137
				public static LocString LOAD_SCENARIO_CONFIRM = "Load the \"{SCENARIO_NAME}\" scenario?";

				// Token: 0x0400A49A RID: 42138
				public static LocString LOAD_CONFIRM_TITLE = "LOAD";

				// Token: 0x0400A49B RID: 42139
				public static LocString SCENARIO_NAME = "Name:";

				// Token: 0x0400A49C RID: 42140
				public static LocString SCENARIO_DESCRIPTION = "Description";

				// Token: 0x0400A49D RID: 42141
				public static LocString BUTTON_DONE = "Done";

				// Token: 0x0400A49E RID: 42142
				public static LocString BUTTON_LOAD = "Load";

				// Token: 0x0400A49F RID: 42143
				public static LocString BUTTON_WORKSHOP = "Steam Workshop";

				// Token: 0x0400A4A0 RID: 42144
				public static LocString NO_SCENARIOS_AVAILABLE = "No scenarios available.\n\nSubscribe to some in the Steam Workshop.";
			}

			// Token: 0x02002588 RID: 9608
			public class AUDIO_OPTIONS_SCREEN
			{
				// Token: 0x0400A4A1 RID: 42145
				public static LocString TITLE = "AUDIO OPTIONS";

				// Token: 0x0400A4A2 RID: 42146
				public static LocString HEADER_VOLUME = "VOLUME";

				// Token: 0x0400A4A3 RID: 42147
				public static LocString HEADER_SETTINGS = "SETTINGS";

				// Token: 0x0400A4A4 RID: 42148
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400A4A5 RID: 42149
				public static LocString MUSIC_EVERY_CYCLE = "Play background music each morning";

				// Token: 0x0400A4A6 RID: 42150
				public static LocString MUSIC_EVERY_CYCLE_TOOLTIP = "If enabled, background music will play every cycle instead of every few cycles";

				// Token: 0x0400A4A7 RID: 42151
				public static LocString AUTOMATION_SOUNDS_ALWAYS = "Always play automation sounds";

				// Token: 0x0400A4A8 RID: 42152
				public static LocString AUTOMATION_SOUNDS_ALWAYS_TOOLTIP = "If enabled, automation sound effects will play even when outside of the " + UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400A4A9 RID: 42153
				public static LocString MUTE_ON_FOCUS_LOST = "Mute when unfocused";

				// Token: 0x0400A4AA RID: 42154
				public static LocString MUTE_ON_FOCUS_LOST_TOOLTIP = "If enabled, the game will be muted while minimized or if the application loses focus";

				// Token: 0x0400A4AB RID: 42155
				public static LocString AUDIO_BUS_MASTER = "Master";

				// Token: 0x0400A4AC RID: 42156
				public static LocString AUDIO_BUS_SFX = "SFX";

				// Token: 0x0400A4AD RID: 42157
				public static LocString AUDIO_BUS_MUSIC = "Music";

				// Token: 0x0400A4AE RID: 42158
				public static LocString AUDIO_BUS_AMBIENCE = "Ambience";

				// Token: 0x0400A4AF RID: 42159
				public static LocString AUDIO_BUS_UI = "UI";
			}

			// Token: 0x02002589 RID: 9609
			public class GAME_OPTIONS_SCREEN
			{
				// Token: 0x0400A4B0 RID: 42160
				public static LocString TITLE = "GAME OPTIONS";

				// Token: 0x0400A4B1 RID: 42161
				public static LocString GENERAL_GAME_OPTIONS = "GENERAL";

				// Token: 0x0400A4B2 RID: 42162
				public static LocString DISABLED_WARNING = "More options available in-game";

				// Token: 0x0400A4B3 RID: 42163
				public static LocString DEFAULT_TO_CLOUD_SAVES = "Default to cloud saves";

				// Token: 0x0400A4B4 RID: 42164
				public static LocString DEFAULT_TO_CLOUD_SAVES_TOOLTIP = "When a new colony is created, this controls whether it will be saved into the cloud saves folder for syncing or not.";

				// Token: 0x0400A4B5 RID: 42165
				public static LocString RESET_TUTORIAL_DESCRIPTION = "Mark all tutorial messages \"unread\"";

				// Token: 0x0400A4B6 RID: 42166
				public static LocString SANDBOX_DESCRIPTION = "Enable sandbox tools";

				// Token: 0x0400A4B7 RID: 42167
				public static LocString CONTROLS_DESCRIPTION = "Change key bindings";

				// Token: 0x0400A4B8 RID: 42168
				public static LocString TEMPERATURE_UNITS = "TEMPERATURE UNITS";

				// Token: 0x0400A4B9 RID: 42169
				public static LocString SAVE_OPTIONS = "SAVE";

				// Token: 0x0400A4BA RID: 42170
				public static LocString CAMERA_SPEED_LABEL = "Camera Pan Speed: {0}%";
			}

			// Token: 0x0200258A RID: 9610
			public class METRIC_OPTIONS_SCREEN
			{
				// Token: 0x0400A4BB RID: 42171
				public static LocString TITLE = "DATA COMMUNICATION";

				// Token: 0x0400A4BC RID: 42172
				public static LocString HEADER_METRICS = "USER DATA";
			}

			// Token: 0x0200258B RID: 9611
			public class COLONY_SAVE_OPTIONS_SCREEN
			{
				// Token: 0x0400A4BD RID: 42173
				public static LocString TITLE = "COLONY SAVE OPTIONS";

				// Token: 0x0400A4BE RID: 42174
				public static LocString DESCRIPTION = "Note: These values are configured per save file";

				// Token: 0x0400A4BF RID: 42175
				public static LocString AUTOSAVE_FREQUENCY = "Autosave frequency:";

				// Token: 0x0400A4C0 RID: 42176
				public static LocString AUTOSAVE_FREQUENCY_DESCRIPTION = "Every: {0} cycle(s)";

				// Token: 0x0400A4C1 RID: 42177
				public static LocString AUTOSAVE_NEVER = "Never";

				// Token: 0x0400A4C2 RID: 42178
				public static LocString TIMELAPSE_RESOLUTION = "Timelapse resolution:";

				// Token: 0x0400A4C3 RID: 42179
				public static LocString TIMELAPSE_RESOLUTION_DESCRIPTION = "{0}x{1}";

				// Token: 0x0400A4C4 RID: 42180
				public static LocString TIMELAPSE_DISABLED_DESCRIPTION = "Disabled";
			}

			// Token: 0x0200258C RID: 9612
			public class FEEDBACK_SCREEN
			{
				// Token: 0x0400A4C5 RID: 42181
				public static LocString TITLE = "FEEDBACK";

				// Token: 0x0400A4C6 RID: 42182
				public static LocString HEADER = "We would love to hear from you!";

				// Token: 0x0400A4C7 RID: 42183
				public static LocString DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file. The buttons to the right will help you find those files on your local drive.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400A4C8 RID: 42184
				public static LocString ALT_DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400A4C9 RID: 42185
				public static LocString BUG_FORUMS_BUTTON = "Report a Bug";

				// Token: 0x0400A4CA RID: 42186
				public static LocString SUGGESTION_FORUMS_BUTTON = "Suggestions Forum";

				// Token: 0x0400A4CB RID: 42187
				public static LocString LOGS_DIRECTORY_BUTTON = "Browse Log Files";

				// Token: 0x0400A4CC RID: 42188
				public static LocString SAVE_FILES_DIRECTORY_BUTTON = "Browse Save Files";
			}

			// Token: 0x0200258D RID: 9613
			public class WORLD_GEN_OPTIONS_SCREEN
			{
				// Token: 0x0400A4CD RID: 42189
				public static LocString TITLE = "WORLD GENERATION OPTIONS";

				// Token: 0x0400A4CE RID: 42190
				public static LocString USE_SEED = "Set Worldgen Seed";

				// Token: 0x0400A4CF RID: 42191
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400A4D0 RID: 42192
				public static LocString RANDOM_BUTTON = "Randomize";

				// Token: 0x0400A4D1 RID: 42193
				public static LocString RANDOM_BUTTON_TOOLTIP = "Randomize a new worldgen seed";

				// Token: 0x0400A4D2 RID: 42194
				public static LocString TOOLTIP = "This will override the current worldgen seed";
			}

			// Token: 0x0200258E RID: 9614
			public class METRICS_OPTIONS_SCREEN
			{
				// Token: 0x0400A4D3 RID: 42195
				public static LocString TITLE = "DATA COMMUNICATION OPTIONS";

				// Token: 0x0400A4D4 RID: 42196
				public static LocString ENABLE_BUTTON = "Enable Data Communication";

				// Token: 0x0400A4D5 RID: 42197
				public static LocString DESCRIPTION = "Collecting user data helps us improve the game.\n\nPlayers who opt out of data communication will no longer send us crash reports and play data.\n\nThey will also be unable to receive new item unlocks from our servers, though existing unlocked items will continue to function.\n\nFor more details on our privacy policy and how we use the data we collect, please visit our <color=#ECA6C9><u><b>privacy center</b></u></color>.";

				// Token: 0x0400A4D6 RID: 42198
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400A4D7 RID: 42199
				public static LocString RESTART_BUTTON = "Restart Game";

				// Token: 0x0400A4D8 RID: 42200
				public static LocString TOOLTIP = "Uncheck to disable data communication";

				// Token: 0x0400A4D9 RID: 42201
				public static LocString RESTART_WARNING = "A game restart is required to apply settings.";
			}

			// Token: 0x0200258F RID: 9615
			public class UNIT_OPTIONS_SCREEN
			{
				// Token: 0x0400A4DA RID: 42202
				public static LocString TITLE = "TEMPERATURE UNITS";

				// Token: 0x0400A4DB RID: 42203
				public static LocString CELSIUS = "Celsius";

				// Token: 0x0400A4DC RID: 42204
				public static LocString CELSIUS_TOOLTIP = "Change temperature unit to Celsius (°C)";

				// Token: 0x0400A4DD RID: 42205
				public static LocString KELVIN = "Kelvin";

				// Token: 0x0400A4DE RID: 42206
				public static LocString KELVIN_TOOLTIP = "Change temperature unit to Kelvin (K)";

				// Token: 0x0400A4DF RID: 42207
				public static LocString FAHRENHEIT = "Fahrenheit";

				// Token: 0x0400A4E0 RID: 42208
				public static LocString FAHRENHEIT_TOOLTIP = "Change temperature unit to Fahrenheit (°F)";
			}

			// Token: 0x02002590 RID: 9616
			public class GRAPHICS_OPTIONS_SCREEN
			{
				// Token: 0x0400A4E1 RID: 42209
				public static LocString TITLE = "GRAPHICS OPTIONS";

				// Token: 0x0400A4E2 RID: 42210
				public static LocString FULLSCREEN = "Fullscreen";

				// Token: 0x0400A4E3 RID: 42211
				public static LocString RESOLUTION = "Resolution:";

				// Token: 0x0400A4E4 RID: 42212
				public static LocString LOWRES = "Low Resolution Textures";

				// Token: 0x0400A4E5 RID: 42213
				public static LocString APPLYBUTTON = "Apply";

				// Token: 0x0400A4E6 RID: 42214
				public static LocString REVERTBUTTON = "Revert";

				// Token: 0x0400A4E7 RID: 42215
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400A4E8 RID: 42216
				public static LocString UI_SCALE = "UI Scale";

				// Token: 0x0400A4E9 RID: 42217
				public static LocString HEADER_DISPLAY = "DISPLAY";

				// Token: 0x0400A4EA RID: 42218
				public static LocString HEADER_UI = "INTERFACE";

				// Token: 0x0400A4EB RID: 42219
				public static LocString COLORMODE = "Color Mode:";

				// Token: 0x0400A4EC RID: 42220
				public static LocString COLOR_MODE_DEFAULT = "Default";

				// Token: 0x0400A4ED RID: 42221
				public static LocString COLOR_MODE_PROTANOPIA = "Protanopia";

				// Token: 0x0400A4EE RID: 42222
				public static LocString COLOR_MODE_DEUTERANOPIA = "Deuteranopia";

				// Token: 0x0400A4EF RID: 42223
				public static LocString COLOR_MODE_TRITANOPIA = "Tritanopia";

				// Token: 0x0400A4F0 RID: 42224
				public static LocString ACCEPT_CHANGES = "Accept Changes?";

				// Token: 0x0400A4F1 RID: 42225
				public static LocString ACCEPT_CHANGES_STRING_COLOR = "Interface changes will be visible immediately, but applying color changes to in-game text will require a restart.\n\nAccept Changes?";

				// Token: 0x0400A4F2 RID: 42226
				public static LocString COLORBLIND_FEEDBACK = "Color blindness options are currently in progress.\n\nIf you would benefit from an alternative color mode or have had difficulties with any of the default colors, please visit the forums and let us know about your experiences.\n\nYour feedback is extremely helpful to us!";

				// Token: 0x0400A4F3 RID: 42227
				public static LocString COLORBLIND_FEEDBACK_BUTTON = "Provide Feedback";
			}

			// Token: 0x02002591 RID: 9617
			public class WORLDGENSCREEN
			{
				// Token: 0x0400A4F4 RID: 42228
				public static LocString TITLE = "NEW GAME";

				// Token: 0x0400A4F5 RID: 42229
				public static LocString GENERATINGWORLD = "GENERATING WORLD";

				// Token: 0x0400A4F6 RID: 42230
				public static LocString SELECTSIZEPROMPT = "A new world is about to be created. Please select its size.";

				// Token: 0x0400A4F7 RID: 42231
				public static LocString LOADINGGAME = "LOADING WORLD...";

				// Token: 0x0200309C RID: 12444
				public class SIZES
				{
					// Token: 0x0400C425 RID: 50213
					public static LocString TINY = "Tiny";

					// Token: 0x0400C426 RID: 50214
					public static LocString SMALL = "Small";

					// Token: 0x0400C427 RID: 50215
					public static LocString STANDARD = "Standard";

					// Token: 0x0400C428 RID: 50216
					public static LocString LARGE = "Big";

					// Token: 0x0400C429 RID: 50217
					public static LocString HUGE = "Colossal";
				}
			}

			// Token: 0x02002592 RID: 9618
			public class MINSPECSCREEN
			{
				// Token: 0x0400A4F8 RID: 42232
				public static LocString TITLE = "WARNING!";

				// Token: 0x0400A4F9 RID: 42233
				public static LocString SIMFAILEDTOLOAD = "A problem occurred loading Oxygen Not Included. This is usually caused by the Visual Studio C++ 2015 runtime being improperly installed on the system. Please exit the game, run Windows Update, and try re-launching Oxygen Not Included.";

				// Token: 0x0400A4FA RID: 42234
				public static LocString BODY = "We've detected that this computer does not meet the minimum requirements to run Oxygen Not Included. While you may continue with your current specs, the game might not run smoothly for you.\n\nPlease be aware that your experience may suffer as a result.";

				// Token: 0x0400A4FB RID: 42235
				public static LocString OKBUTTON = "Okay, thanks!";

				// Token: 0x0400A4FC RID: 42236
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002593 RID: 9619
			public class SUPPORTWARNINGS
			{
				// Token: 0x0400A4FD RID: 42237
				public static LocString AUDIO_DRIVERS = "A problem occurred initializing your audio device.\nSorry about that!\n\nThis is usually caused by outdated audio drivers.\n\nPlease visit your audio device manufacturer's website to download the latest drivers.";

				// Token: 0x0400A4FE RID: 42238
				public static LocString AUDIO_DRIVERS_MORE_INFO = "More Info";

				// Token: 0x0400A4FF RID: 42239
				public static LocString DUPLICATE_KEY_BINDINGS = "<b>Duplicate key bindings were detected.\nThis may be because your custom key bindings conflicted with a new feature's default key.\nPlease visit the controls screen to ensure your key bindings are set how you like them.</b>\n{0}";

				// Token: 0x0400A500 RID: 42240
				public static LocString SAVE_DIRECTORY_READ_ONLY = "A problem occurred while accessing your save directory.\nThis may be because your directory is set to read-only.\n\nPlease ensure your save directory is readable as well as writable and re-launch the game.\n{0}";

				// Token: 0x0400A501 RID: 42241
				public static LocString SAVE_DIRECTORY_INSUFFICIENT_SPACE = "There is insufficient disk space to write to your save directory.\n\nPlease free at least 15 MB to give your saves some room to breathe.\n{0}";

				// Token: 0x0400A502 RID: 42242
				public static LocString WORLD_GEN_FILES = "A problem occurred while accessing certain game files that will prevent starting new games.\n\nPlease ensure that the directory and files are readable as well as writable and re-launch the game:\n\n{0}";

				// Token: 0x0400A503 RID: 42243
				public static LocString WORLD_GEN_FAILURE = "A problem occurred while generating a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with a different seed.";

				// Token: 0x0400A504 RID: 42244
				public static LocString WORLD_GEN_FAILURE_STORY = "A problem occurred while generating a world from this seed:\n{0}.\n\nNot all story traits were able to be placed. Please try again with a different seed or fewer story traits.";

				// Token: 0x0400A505 RID: 42245
				public static LocString PLAYER_PREFS_CORRUPTED = "A problem occurred while loading your game options.\nThey have been reset to their default settings.\n\n";

				// Token: 0x0400A506 RID: 42246
				public static LocString IO_UNAUTHORIZED = "An Unauthorized Access Error occurred when trying to write to disk.\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400A507 RID: 42247
				public static LocString IO_SUFFICIENT_SPACE = "An Insufficient Space Error occurred when trying to write to disk. \n\nPlease free up some space.\n{0}";

				// Token: 0x0400A508 RID: 42248
				public static LocString IO_UNKNOWN = "An unknown error occurred when trying to write or access a file.\n{0}";

				// Token: 0x0400A509 RID: 42249
				public static LocString MORE_INFO_BUTTON = "More Info";
			}

			// Token: 0x02002594 RID: 9620
			public class SAVEUPGRADEWARNINGS
			{
				// Token: 0x0400A50A RID: 42250
				public static LocString SUDDENMORALEHELPER_TITLE = "MORALE CHANGES";

				// Token: 0x0400A50B RID: 42251
				public static LocString SUDDENMORALEHELPER = "Welcome to the Expressive Upgrade! This update introduces a new Morale system that replaces Food and Decor Expectations that were found in previous versions of the game.\n\nThe game you are trying to load was created before this system was introduced, and will need to be updated. You may either:\n\n\n1) Enable the new Morale system in this save, removing Food and Decor Expectations. It's possible that when you load your save your old colony won't meet your Duplicants' new Morale needs, so they'll receive a 5 cycle Morale boost to give you time to adjust.\n\n2) Disable Morale in this save. The new Morale mechanics will still be visible, but won't affect your Duplicants' stress. Food and Decor expectations will no longer exist in this save.";

				// Token: 0x0400A50C RID: 42252
				public static LocString SUDDENMORALEHELPER_BUFF = "1) Bring on Morale!";

				// Token: 0x0400A50D RID: 42253
				public static LocString SUDDENMORALEHELPER_DISABLE = "2) Disable Morale";

				// Token: 0x0400A50E RID: 42254
				public static LocString NEWAUTOMATIONWARNING_TITLE = "AUTOMATION CHANGES";

				// Token: 0x0400A50F RID: 42255
				public static LocString NEWAUTOMATIONWARNING = "The following buildings have acquired new automation ports!\n\nTake a moment to check whether these buildings in your colony are now unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400A510 RID: 42256
				public static LocString MERGEDOWNCHANGES_TITLE = "BREATH OF FRESH AIR UPDATE CHANGES";

				// Token: 0x0400A511 RID: 42257
				public static LocString MERGEDOWNCHANGES = "Oxygen Not Included has had a <b>major update</b> since this save file was created! In addition to the <b>multitude of bug fixes and quality-of-life features</b>, please pay attention to these changes which may affect your existing colony:";

				// Token: 0x0400A512 RID: 42258
				public static LocString MERGEDOWNCHANGES_FOOD = "•<indent=20px>Fridges are more effective for early-game food storage</indent>\n•<indent=20px><b>Both</b> freezing temperatures and a sterile gas are needed for <b>total food preservation</b>.</indent>";

				// Token: 0x0400A513 RID: 42259
				public static LocString MERGEDOWNCHANGES_AIRFILTER = "•<indent=20px>" + BUILDINGS.PREFABS.AIRFILTER.NAME + " now requires <b>5w Power</b>.</indent>\n•<indent=20px>Duplicants will get <b>Stinging Eyes</b> from gasses such as chlorine and hydrogen.</indent>";

				// Token: 0x0400A514 RID: 42260
				public static LocString MERGEDOWNCHANGES_SIMULATION = "•<indent=20px>Many <b>simulation bugs</b> have been fixed.</indent>\n•<indent=20px>This may <b>change the effectiveness</b> of certain contraptions and " + BUILDINGS.PREFABS.STEAMTURBINE2.NAME + " setups.</indent>";

				// Token: 0x0400A515 RID: 42261
				public static LocString MERGEDOWNCHANGES_BUILDINGS = "•<indent=20px>The <b>" + BUILDINGS.PREFABS.OXYGENMASKSTATION.NAME + "</b> has been added to aid early-game exploration.</indent>\n•<indent=20px>Use the new <b>Meter Valves</b> for precise control of resources in pipes.</indent>";

				// Token: 0x0400A516 RID: 42262
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TITLE = "JUNE 2023 QoL UPDATE CHANGES";

				// Token: 0x0400A517 RID: 42263
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SUMMARY = "There have been significant changes to <b>Space Scanners</b> and <b>Telescopes</b> since this save file was created!\n\nMeteor showers have been disabled for 20 cycles to provide time to adapt.";

				// Token: 0x0400A518 RID: 42264
				public static LocString SPACESCANNERANDTELESCOPECHANGES_WARNING = "Please note these changes which may affect your existing colony:\n\n";

				// Token: 0x0400A519 RID: 42265
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS = "•<indent=20px>Automation is synced between all Space Scanners targeting the same object.</indent>\n•<indent=20px>Network quality based on the total percentage of sky covered.</indent>\n•<indent=20px>Industrial machinery no longer impacts network quality.</indent>";

				// Token: 0x0400A51A RID: 42266
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TELESCOPES = "•<indent=20px>Telescopes have a symmetrical scanning range.</indent>\n•<indent=20px>Obstructions block visibility from the blocked tile out toward the outer edge of scanning range.</indent>";
			}
		}

		// Token: 0x02001D99 RID: 7577
		public class SANDBOX_TOGGLE
		{
			// Token: 0x040085A3 RID: 34211
			public static LocString TOOLTIP_LOCKED = "<b>Sandbox Mode</b> must be unlocked in the options menu before it can be used. {Hotkey}";

			// Token: 0x040085A4 RID: 34212
			public static LocString TOOLTIP_UNLOCKED = "Toggle <b>Sandbox Mode</b> {Hotkey}";
		}

		// Token: 0x02001D9A RID: 7578
		public class SKILLS_SCREEN
		{
			// Token: 0x040085A5 RID: 34213
			public static LocString CURRENT_MORALE = "Current Morale: {0}\nMorale Need: {1}";

			// Token: 0x040085A6 RID: 34214
			public static LocString SORT_BY_DUPLICANT = "Duplicants";

			// Token: 0x040085A7 RID: 34215
			public static LocString SORT_BY_MORALE = "Morale";

			// Token: 0x040085A8 RID: 34216
			public static LocString SORT_BY_EXPERIENCE = "Skill Points";

			// Token: 0x040085A9 RID: 34217
			public static LocString SORT_BY_SKILL_AVAILABLE = "Skill Points";

			// Token: 0x040085AA RID: 34218
			public static LocString SORT_BY_HAT = "Hat";

			// Token: 0x040085AB RID: 34219
			public static LocString SELECT_HAT = "<b>SELECT HAT</b>";

			// Token: 0x040085AC RID: 34220
			public static LocString POINTS_AVAILABLE = "<b>SKILL POINTS AVAILABLE</b>";

			// Token: 0x040085AD RID: 34221
			public static LocString MORALE = "<b>Morale</b>";

			// Token: 0x040085AE RID: 34222
			public static LocString MORALE_EXPECTATION = "<b>Morale Need</b>";

			// Token: 0x040085AF RID: 34223
			public static LocString EXPERIENCE = "EXPERIENCE TO NEXT LEVEL";

			// Token: 0x040085B0 RID: 34224
			public static LocString EXPERIENCE_TOOLTIP = "{0}exp to next Skill Point";

			// Token: 0x040085B1 RID: 34225
			public static LocString NOT_AVAILABLE = "Not available";

			// Token: 0x02002595 RID: 9621
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400A51B RID: 42267
				public static LocString EXPECTATION_TARGET_SKILL = "Current Morale: {0}\nSkill Morale Needs: {1}";

				// Token: 0x0400A51C RID: 42268
				public static LocString EXPECTATION_ALERT_TARGET_SKILL = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

				// Token: 0x0400A51D RID: 42269
				public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

				// Token: 0x0200309D RID: 12445
				public class SKILLGROUP_ENABLED
				{
					// Token: 0x0400C42A RID: 50218
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400C42B RID: 50219
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> skills";
				}

				// Token: 0x0200309E RID: 12446
				public class MASTERY
				{
					// Token: 0x0400C42C RID: 50220
					public static LocString CAN_MASTER = "{0} <b>can learn</b> {1}";

					// Token: 0x0400C42D RID: 50221
					public static LocString HAS_MASTERED = "{0} has <b>already learned</b> {1}";

					// Token: 0x0400C42E RID: 50222
					public static LocString CANNOT_MASTER = "{0} <b>cannot learn</b> {1}";

					// Token: 0x0400C42F RID: 50223
					public static LocString STRESS_WARNING_MESSAGE = string.Concat(new string[]
					{
						"Learning {0} will put {1} into a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" deficit and cause unnecessary ",
						UI.PRE_KEYWORD,
						"Stress",
						UI.PST_KEYWORD,
						"!"
					});

					// Token: 0x0400C430 RID: 50224
					public static LocString REQUIRES_MORE_SKILL_POINTS = "    • Not enough " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

					// Token: 0x0400C431 RID: 50225
					public static LocString REQUIRES_PREVIOUS_SKILLS = "    • Missing prerequisite " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;

					// Token: 0x0400C432 RID: 50226
					public static LocString PREVENTED_BY_TRAIT = string.Concat(new string[]
					{
						"    • This Duplicant possesses the ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" Trait and cannot learn this Skill"
					});

					// Token: 0x0400C433 RID: 50227
					public static LocString SKILL_APTITUDE = string.Concat(new string[]
					{
						"{0} is interested in {1} and will receive a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" bonus for learning it!"
					});

					// Token: 0x0400C434 RID: 50228
					public static LocString SKILL_GRANTED = "{0} has been granted {1} by a Trait, but does not have increased " + UI.FormatAsKeyWord("Morale Requirements") + " from learning it";
				}
			}
		}

		// Token: 0x02001D9B RID: 7579
		public class KLEI_INVENTORY_SCREEN
		{
			// Token: 0x040085B2 RID: 34226
			public static LocString OPEN_INVENTORY_BUTTON = "Open Klei Inventory";

			// Token: 0x040085B3 RID: 34227
			public static LocString ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName}.";

			// Token: 0x040085B4 RID: 34228
			public static LocString ARTABLE_ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName} of {ArtableQuality} quality.";

			// Token: 0x040085B5 RID: 34229
			public static LocString CLOTHING_ITEM_FACADE_FOR = "This blueprint can be used in any outfit.";

			// Token: 0x040085B6 RID: 34230
			public static LocString BALLOON_ARTIST_FACADE_FOR = "This blueprint can be used by any Balloon Artist.";

			// Token: 0x040085B7 RID: 34231
			public static LocString ITEM_RARITY_DETAILS = "{RarityName} quality.";

			// Token: 0x040085B8 RID: 34232
			public static LocString ITEM_PLAYER_OWNED_AMOUNT = "My colony has {OwnedCount} of these blueprints.";

			// Token: 0x040085B9 RID: 34233
			public static LocString ITEM_PLAYER_OWN_NONE = "My colony doesn't have any of these yet.";

			// Token: 0x040085BA RID: 34234
			public static LocString ITEM_PLAYER_OWNED_AMOUNT_ICON = "x{OwnedCount}";

			// Token: 0x040085BB RID: 34235
			public static LocString ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE = "This blueprint is part of my colony's permanent collection.";

			// Token: 0x040085BC RID: 34236
			public static LocString ITEM_UNKNOWN_NAME = "Uh oh!";

			// Token: 0x040085BD RID: 34237
			public static LocString ITEM_UNKNOWN_DESCRIPTION = "Hmm. Looks like this blueprint is missing from the supply closet. Perhaps due to a temporal anomaly...";

			// Token: 0x040085BE RID: 34238
			public static LocString SEARCH_PLACEHOLDER = "Search";

			// Token: 0x040085BF RID: 34239
			public static LocString CLEAR_SEARCH_BUTTON_TOOLTIP = "Clear search";

			// Token: 0x040085C0 RID: 34240
			public static LocString TOOLTIP_VIEW_ALL_ITEMS = "Filter: Showing all items\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x040085C1 RID: 34241
			public static LocString TOOLTIP_VIEW_OWNED_ONLY = "Filter: Showing owned items Only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x040085C2 RID: 34242
			public static LocString TOOLTIP_VIEW_DOUBLES_ONLY = "Filter: Showing multiples owned only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x02002596 RID: 9622
			public static class BARTERING
			{
				// Token: 0x0400A51E RID: 42270
				public static LocString TOOLTIP_ACTION_INVALID_OFFLINE = "Currently unavailable";

				// Token: 0x0400A51F RID: 42271
				public static LocString BUY = "PRINT";

				// Token: 0x0400A520 RID: 42272
				public static LocString TOOLTIP_BUY_ACTIVE = "This item requires {0} spools of Filament to print";

				// Token: 0x0400A521 RID: 42273
				public static LocString TOOLTIP_UNBUYABLE = "This item is unprintable";

				// Token: 0x0400A522 RID: 42274
				public static LocString TOOLTIP_UNBUYABLE_BETA = "This item will be printable after the public testing period";

				// Token: 0x0400A523 RID: 42275
				public static LocString TOOLTIP_BUY_CANT_AFFORD = "Filament supply is too low";

				// Token: 0x0400A524 RID: 42276
				public static LocString SELL = "RECYCLE";

				// Token: 0x0400A525 RID: 42277
				public static LocString TOOLTIP_SELL_ACTIVE = "Recycle this blueprint for {0} spools of Filament";

				// Token: 0x0400A526 RID: 42278
				public static LocString TOOLTIP_UNSELLABLE = "This item is non-recyclable";

				// Token: 0x0400A527 RID: 42279
				public static LocString TOOLTIP_NONE_TO_SELL = "My colony does not own any of these blueprints";

				// Token: 0x0400A528 RID: 42280
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400A529 RID: 42281
				public static LocString CONFIRM_RECYCLE_HEADER = "RECYCLE INTO FILAMENT?";

				// Token: 0x0400A52A RID: 42282
				public static LocString CONFIRM_PRINT_HEADER = "PRINT ITEM?";

				// Token: 0x0400A52B RID: 42283
				public static LocString OFFLINE_LABEL = "Not connected to Klei server";

				// Token: 0x0400A52C RID: 42284
				public static LocString LOADING = "Connecting to server...";

				// Token: 0x0400A52D RID: 42285
				public static LocString TRANSACTION_ERROR = "Whoops! Something's gone wrong.";

				// Token: 0x0400A52E RID: 42286
				public static LocString ACTION_DESCRIPTION_RECYCLE = "Recycling this blueprint will recover Filament that my colony can use to print other items.\n\nOne copy of this blueprint will be removed from my colony's supply closet.";

				// Token: 0x0400A52F RID: 42287
				public static LocString ACTION_DESCRIPTION_PRINT = "Producing this blueprint requires Filament from my colony's supply.\n\nOne copy of this blueprint will be extruded at a time.";

				// Token: 0x0400A530 RID: 42288
				public static LocString WALLET_TOOLTIP = "{0} spool of Filament available";

				// Token: 0x0400A531 RID: 42289
				public static LocString WALLET_PLURAL_TOOLTIP = "{0} spools of Filament available";

				// Token: 0x0400A532 RID: 42290
				public static LocString TRANSACTION_COMPLETE_HEADER = "SUCCESS!";

				// Token: 0x0400A533 RID: 42291
				public static LocString TRANSACTION_INCOMPLETE_HEADER = "ERROR";

				// Token: 0x0400A534 RID: 42292
				public static LocString PURCHASE_SUCCESS = "One copy of this blueprint has been added to my colony's supply closet.";

				// Token: 0x0400A535 RID: 42293
				public static LocString SELL_SUCCESS = "The Filament recovered from recycling this item can now be used to print other items.";
			}

			// Token: 0x02002597 RID: 9623
			public static class CATEGORIES
			{
				// Token: 0x0400A536 RID: 42294
				public static LocString EQUIPMENT = "Equipment";

				// Token: 0x0400A537 RID: 42295
				public static LocString DUPE_TOPS = "Tops & Onesies";

				// Token: 0x0400A538 RID: 42296
				public static LocString DUPE_BOTTOMS = "Bottoms";

				// Token: 0x0400A539 RID: 42297
				public static LocString DUPE_GLOVES = "Gloves";

				// Token: 0x0400A53A RID: 42298
				public static LocString DUPE_SHOES = "Footwear";

				// Token: 0x0400A53B RID: 42299
				public static LocString DUPE_HATS = "Headgear";

				// Token: 0x0400A53C RID: 42300
				public static LocString DUPE_ACCESSORIES = "Accessories";

				// Token: 0x0400A53D RID: 42301
				public static LocString ATMO_SUIT_HELMET = "Atmo Helmets";

				// Token: 0x0400A53E RID: 42302
				public static LocString ATMO_SUIT_BODY = "Atmo Suits";

				// Token: 0x0400A53F RID: 42303
				public static LocString ATMO_SUIT_GLOVES = "Atmo Gloves";

				// Token: 0x0400A540 RID: 42304
				public static LocString ATMO_SUIT_BELT = "Atmo Belts";

				// Token: 0x0400A541 RID: 42305
				public static LocString ATMO_SUIT_SHOES = "Atmo Boots";

				// Token: 0x0400A542 RID: 42306
				public static LocString PRIMOGARB = "Primo Garb";

				// Token: 0x0400A543 RID: 42307
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400A544 RID: 42308
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400A545 RID: 42309
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400A546 RID: 42310
				public static LocString SWEEPYS = "Sweepys";

				// Token: 0x0400A547 RID: 42311
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400A548 RID: 42312
				public static LocString ARTWORKS = "Artwork";

				// Token: 0x0400A549 RID: 42313
				public static LocString MONUMENTPARTS = "Monuments";

				// Token: 0x0400A54A RID: 42314
				public static LocString JOY_RESPONSE = "Overjoyed Responses";

				// Token: 0x0200309F RID: 12447
				public static class JOY_RESPONSES
				{
					// Token: 0x0400C435 RID: 50229
					public static LocString BALLOON_ARTIST = "Balloon Artist";
				}
			}

			// Token: 0x02002598 RID: 9624
			public static class TOP_LEVEL_CATEGORIES
			{
				// Token: 0x0400A54B RID: 42315
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400A54C RID: 42316
				public static LocString CLOTHING_TOPS = "Tops & Onesies";

				// Token: 0x0400A54D RID: 42317
				public static LocString CLOTHING_BOTTOMS = "Bottoms";

				// Token: 0x0400A54E RID: 42318
				public static LocString CLOTHING_GLOVES = "Gloves";

				// Token: 0x0400A54F RID: 42319
				public static LocString CLOTHING_SHOES = "Footwear";

				// Token: 0x0400A550 RID: 42320
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400A551 RID: 42321
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400A552 RID: 42322
				public static LocString WALLPAPERS = "Wallpapers";

				// Token: 0x0400A553 RID: 42323
				public static LocString ARTWORK = "Artwork";

				// Token: 0x0400A554 RID: 42324
				public static LocString JOY_RESPONSES = "Joy Responses";
			}

			// Token: 0x02002599 RID: 9625
			public static class SUBCATEGORIES
			{
				// Token: 0x0400A555 RID: 42325
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400A556 RID: 42326
				public static LocString UNCATEGORIZED = "BUG: UNCATEGORIZED";

				// Token: 0x0400A557 RID: 42327
				public static LocString YAML = "YAML";

				// Token: 0x0400A558 RID: 42328
				public static LocString DEFAULT = "Default";

				// Token: 0x0400A559 RID: 42329
				public static LocString JOY_BALLOON = "Balloons";

				// Token: 0x0400A55A RID: 42330
				public static LocString JOY_STICKER = "Stickers";

				// Token: 0x0400A55B RID: 42331
				public static LocString PRIMO_GARB = "Primo Garb";

				// Token: 0x0400A55C RID: 42332
				public static LocString CLOTHING_TOPS_BASIC = "Basic Shirts";

				// Token: 0x0400A55D RID: 42333
				public static LocString CLOTHING_TOPS_TSHIRT = "Tees";

				// Token: 0x0400A55E RID: 42334
				public static LocString CLOTHING_TOPS_JACKET = "Jackets";

				// Token: 0x0400A55F RID: 42335
				public static LocString CLOTHING_TOPS_UNDERSHIRT = "Undershirts";

				// Token: 0x0400A560 RID: 42336
				public static LocString CLOTHING_BOTTOMS_BASIC = "Basic Pants";

				// Token: 0x0400A561 RID: 42337
				public static LocString CLOTHING_BOTTOMS_FANCY = "Fancy Pants";

				// Token: 0x0400A562 RID: 42338
				public static LocString CLOTHING_BOTTOMS_SHORTS = "Shorts";

				// Token: 0x0400A563 RID: 42339
				public static LocString CLOTHING_BOTTOMS_SKIRTS = "Skirts";

				// Token: 0x0400A564 RID: 42340
				public static LocString CLOTHING_BOTTOMS_UNDERWEAR = "Underwear";

				// Token: 0x0400A565 RID: 42341
				public static LocString CLOTHING_GLOVES_BASIC = "Basic Gloves";

				// Token: 0x0400A566 RID: 42342
				public static LocString CLOTHING_GLOVES_SHORT = "Short Gloves";

				// Token: 0x0400A567 RID: 42343
				public static LocString CLOTHING_GLOVES_PRINTS = "Specialty Gloves";

				// Token: 0x0400A568 RID: 42344
				public static LocString CLOTHING_SHOES_BASIC = "Basic Shoes";

				// Token: 0x0400A569 RID: 42345
				public static LocString CLOTHING_SHOE_SOCKS = "Socks";

				// Token: 0x0400A56A RID: 42346
				public static LocString ATMOSUIT_HELMETS_BASIC = "Atmo Helmets";

				// Token: 0x0400A56B RID: 42347
				public static LocString ATMOSUIT_HELMETS_FANCY = "Fancy Atmo Helmets";

				// Token: 0x0400A56C RID: 42348
				public static LocString ATMOSUIT_BODIES_BASIC = "Atmo Suits";

				// Token: 0x0400A56D RID: 42349
				public static LocString ATMOSUIT_BODIES_FANCY = "Fancy Atmo Suits";

				// Token: 0x0400A56E RID: 42350
				public static LocString ATMOSUIT_GLOVES_BASIC = "Atmo Gloves";

				// Token: 0x0400A56F RID: 42351
				public static LocString ATMOSUIT_GLOVES_FANCY = "Fancy Atmo Gloves";

				// Token: 0x0400A570 RID: 42352
				public static LocString ATMOSUIT_BELTS_BASIC = "Atmo Belts";

				// Token: 0x0400A571 RID: 42353
				public static LocString ATMOSUIT_BELTS_FANCY = "Fancy Atmo Belts";

				// Token: 0x0400A572 RID: 42354
				public static LocString ATMOSUIT_SHOES_BASIC = "Atmo Boots";

				// Token: 0x0400A573 RID: 42355
				public static LocString ATMOSUIT_SHOES_FANCY = "Fancy Atmo Boots";

				// Token: 0x0400A574 RID: 42356
				public static LocString BUILDING_WALLPAPER_BASIC = "Solid Wallpapers";

				// Token: 0x0400A575 RID: 42357
				public static LocString BUILDING_WALLPAPER_FANCY = "Geometric Wallpapers";

				// Token: 0x0400A576 RID: 42358
				public static LocString BUILDING_WALLPAPER_PRINTS = "Patterned Wallpapers";

				// Token: 0x0400A577 RID: 42359
				public static LocString BUILDING_CANVAS_STANDARD = "Standard Canvas";

				// Token: 0x0400A578 RID: 42360
				public static LocString BUILDING_CANVAS_PORTRAIT = "Portrait Canvas";

				// Token: 0x0400A579 RID: 42361
				public static LocString BUILDING_CANVAS_LANDSCAPE = "Landscape Canvas";

				// Token: 0x0400A57A RID: 42362
				public static LocString BUILDING_SCULPTURE = "Sculptures";

				// Token: 0x0400A57B RID: 42363
				public static LocString MONUMENT_PARTS = "Monuments";

				// Token: 0x0400A57C RID: 42364
				public static LocString BUILDINGS_FLOWER_VASE = "Pots and Planters";

				// Token: 0x0400A57D RID: 42365
				public static LocString BUILDINGS_BED_COT = "Cots";

				// Token: 0x0400A57E RID: 42366
				public static LocString BUILDINGS_BED_LUXURY = "Comfy Beds";

				// Token: 0x0400A57F RID: 42367
				public static LocString BUILDING_CEILING_LIGHT = "Lights";

				// Token: 0x0400A580 RID: 42368
				public static LocString BUILDINGS_STORAGE = "Storage";

				// Token: 0x0400A581 RID: 42369
				public static LocString BUILDINGS_INDUSTRIAL = "Industrial";

				// Token: 0x0400A582 RID: 42370
				public static LocString BUILDINGS_FOOD = "Culinary";

				// Token: 0x0400A583 RID: 42371
				public static LocString BUILDINGS_RECREATION = "Recreation and Decor";
			}

			// Token: 0x0200259A RID: 9626
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400A584 RID: 42372
				public static LocString CATEGORY_HEADER = "BLUEPRINTS";

				// Token: 0x0400A585 RID: 42373
				public static LocString ITEMS_HEADER = "Items";

				// Token: 0x0400A586 RID: 42374
				public static LocString DETAILS_HEADER = "Details";
			}
		}

		// Token: 0x02001D9C RID: 7580
		public class ITEM_DROP_SCREEN
		{
			// Token: 0x040085C3 RID: 34243
			public static LocString THANKS_FOR_PLAYING = "New blueprints unlocked!";

			// Token: 0x040085C4 RID: 34244
			public static LocString WEB_REWARDS_AVAILABLE = "Rewards available online!";

			// Token: 0x040085C5 RID: 34245
			public static LocString NOTHING_AVAILABLE = "All available blueprints claimed";

			// Token: 0x040085C6 RID: 34246
			public static LocString OPEN_URL_BUTTON = "CLAIM";

			// Token: 0x040085C7 RID: 34247
			public static LocString PRINT_ITEM_BUTTON = "PRINT";

			// Token: 0x040085C8 RID: 34248
			public static LocString DISMISS_BUTTON = "DISMISS";

			// Token: 0x040085C9 RID: 34249
			public static LocString ERROR_CANNOTLOADITEM = "Whoops! Something's gone wrong.";

			// Token: 0x0200259B RID: 9627
			public static class IN_GAME_BUTTON
			{
				// Token: 0x0400A587 RID: 42375
				public static LocString TOOLTIP_ITEMS_AVAILABLE = "Unlock new blueprints";

				// Token: 0x0400A588 RID: 42376
				public static LocString TOOLTIP_ERROR_NO_ITEMS = "No new blueprints to unlock";
			}
		}

		// Token: 0x02001D9D RID: 7581
		public class OUTFIT_BROWSER_SCREEN
		{
			// Token: 0x040085CA RID: 34250
			public static LocString BUTTON_ADD_OUTFIT = "New Outfit";

			// Token: 0x040085CB RID: 34251
			public static LocString BUTTON_PICK_OUTFIT = "Assign Outfit";

			// Token: 0x040085CC RID: 34252
			public static LocString TOOLTIP_PICK_OUTFIT_ERROR_LOCKED = "Cannot assign this outfit to {MinionName} because my colony doesn't have all of these blueprints yet";

			// Token: 0x040085CD RID: 34253
			public static LocString BUTTON_EDIT_OUTFIT = "Restyle Outfit";

			// Token: 0x040085CE RID: 34254
			public static LocString BUTTON_COPY_OUTFIT = "Copy Outfit";

			// Token: 0x040085CF RID: 34255
			public static LocString TOOLTIP_DELETE_OUTFIT = "Delete Outfit";

			// Token: 0x040085D0 RID: 34256
			public static LocString TOOLTIP_DELETE_OUTFIT_ERROR_READONLY = "This outfit cannot be deleted";

			// Token: 0x040085D1 RID: 34257
			public static LocString TOOLTIP_RENAME_OUTFIT = "Rename Outfit";

			// Token: 0x040085D2 RID: 34258
			public static LocString TOOLTIP_RENAME_OUTFIT_ERROR_READONLY = "This outfit cannot be renamed";

			// Token: 0x040085D3 RID: 34259
			public static LocString TOOLTIP_FILTER_BY_CLOTHING = "View your Clothing Outfits";

			// Token: 0x040085D4 RID: 34260
			public static LocString TOOLTIP_FILTER_BY_ATMO_SUITS = "View your Atmo Suit Outfits";

			// Token: 0x0200259C RID: 9628
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400A589 RID: 42377
				public static LocString GALLERY_HEADER = "OUTFITS";

				// Token: 0x0400A58A RID: 42378
				public static LocString MINION_GALLERY_HEADER = "WARDROBE";

				// Token: 0x0400A58B RID: 42379
				public static LocString DETAILS_HEADER = "Preview";
			}

			// Token: 0x0200259D RID: 9629
			public class DELETE_WARNING_POPUP
			{
				// Token: 0x0400A58C RID: 42380
				public static LocString HEADER = "Delete \"{OutfitName}\"?";

				// Token: 0x0400A58D RID: 42381
				public static LocString BODY = "Are you sure you want to delete \"{OutfitName}\"?\n\nAny Duplicants assigned to wear this outfit on spawn will be printed wearing their default outfit instead. Existing Duplicants in saved games won't be affected.\n\nThis <b>cannot</b> be undone.";

				// Token: 0x0400A58E RID: 42382
				public static LocString BUTTON_YES_DELETE = "Yes, delete outfit";

				// Token: 0x0400A58F RID: 42383
				public static LocString BUTTON_DONT_DELETE = "Cancel";
			}

			// Token: 0x0200259E RID: 9630
			public class RENAME_POPUP
			{
				// Token: 0x0400A590 RID: 42384
				public static LocString HEADER = "RENAME OUTFIT";
			}
		}

		// Token: 0x02001D9E RID: 7582
		public class LOCKER_MENU
		{
			// Token: 0x040085D5 RID: 34261
			public static LocString TITLE = "SUPPLY CLOSET";

			// Token: 0x040085D6 RID: 34262
			public static LocString BUTTON_INVENTORY = "All";

			// Token: 0x040085D7 RID: 34263
			public static LocString BUTTON_INVENTORY_DESCRIPTION = "View all of my colony's blueprints";

			// Token: 0x040085D8 RID: 34264
			public static LocString BUTTON_DUPLICANTS = "Duplicants";

			// Token: 0x040085D9 RID: 34265
			public static LocString BUTTON_DUPLICANTS_DESCRIPTION = "Manage individual Duplicants' outfits";

			// Token: 0x040085DA RID: 34266
			public static LocString BUTTON_OUTFITS = "Wardrobe";

			// Token: 0x040085DB RID: 34267
			public static LocString BUTTON_OUTFITS_DESCRIPTION = "Manage my colony's collection of outfits";

			// Token: 0x040085DC RID: 34268
			public static LocString DEFAULT_DESCRIPTION = "Select a screen";

			// Token: 0x040085DD RID: 34269
			public static LocString BUTTON_CLAIM = "Claim Blueprints";

			// Token: 0x040085DE RID: 34270
			public static LocString BUTTON_CLAIM_DESCRIPTION = "Claim any available blueprints";

			// Token: 0x040085DF RID: 34271
			public static LocString BUTTON_CLAIM_NONE_DESCRIPTION = "All available blueprints claimed";

			// Token: 0x040085E0 RID: 34272
			public static LocString UNOPENED_ITEMS_TOOLTIP = "New blueprints available";

			// Token: 0x040085E1 RID: 34273
			public static LocString UNOPENED_ITEMS_NONE_TOOLTIP = "All available blueprints claimed";

			// Token: 0x040085E2 RID: 34274
			public static LocString OFFLINE_ICON_TOOLTIP = "Not connected to Klei server";
		}

		// Token: 0x02001D9F RID: 7583
		public class LOCKER_NAVIGATOR
		{
			// Token: 0x040085E3 RID: 34275
			public static LocString BUTTON_BACK = "BACK";

			// Token: 0x040085E4 RID: 34276
			public static LocString BUTTON_CLOSE = "CLOSE";

			// Token: 0x0200259F RID: 9631
			public class DATA_COLLECTION_WARNING_POPUP
			{
				// Token: 0x0400A591 RID: 42385
				public static LocString HEADER = "Data Communication is Disabled";

				// Token: 0x0400A592 RID: 42386
				public static LocString BODY = "Data Communication must be enabled in order to access newly unlocked items. This setting can be found in the Options menu.\n\nExisting item unlocks can still be used while Data Communication is disabled.";

				// Token: 0x0400A593 RID: 42387
				public static LocString BUTTON_OK = "Continue";

				// Token: 0x0400A594 RID: 42388
				public static LocString BUTTON_OPEN_SETTINGS = "Options Menu";
			}
		}

		// Token: 0x02001DA0 RID: 7584
		public class JOY_RESPONSE_DESIGNER_SCREEN
		{
			// Token: 0x040085E5 RID: 34277
			public static LocString CATEGORY_HEADER = "OVERJOYED RESPONSES";

			// Token: 0x040085E6 RID: 34278
			public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

			// Token: 0x040085E7 RID: 34279
			public static LocString TOOLTIP_NO_FACADES_FOR_JOY_TRAIT = "There aren't any blueprints for {JoyResponseType} Duplicants yet";

			// Token: 0x040085E8 RID: 34280
			public static LocString TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED = "This Overjoyed Response blueprint cannot be assigned because my colony doesn't own it yet";

			// Token: 0x020025A0 RID: 9632
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400A595 RID: 42389
				public static LocString HEADER = "Discard changes to {MinionName}'s Overjoyed Response?";
			}
		}

		// Token: 0x02001DA1 RID: 7585
		public class OUTFIT_DESIGNER_SCREEN
		{
			// Token: 0x040085E9 RID: 34281
			public static LocString CATEGORY_HEADER = "CLOTHING";

			// Token: 0x020025A1 RID: 9633
			public class MINION_INSTANCE
			{
				// Token: 0x0400A596 RID: 42390
				public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

				// Token: 0x0400A597 RID: 42391
				public static LocString BUTTON_APPLY_TO_TEMPLATE = "Apply to Template";

				// Token: 0x020030A0 RID: 12448
				public class APPLY_TEMPLATE_POPUP
				{
					// Token: 0x0400C436 RID: 50230
					public static LocString HEADER = "SAVE AS TEMPLATE";

					// Token: 0x0400C437 RID: 50231
					public static LocString DESC_SAVE_EXISTING = "\"{OutfitName}\" will be updated and applied to {MinionName} on save.";

					// Token: 0x0400C438 RID: 50232
					public static LocString DESC_SAVE_NEW = "A new outfit named \"{OutfitName}\" will be created and assigned to {MinionName} on save.";

					// Token: 0x0400C439 RID: 50233
					public static LocString BUTTON_SAVE_EXISTING = "Update Outfit";

					// Token: 0x0400C43A RID: 50234
					public static LocString BUTTON_SAVE_NEW = "Save New Outfit";
				}
			}

			// Token: 0x020025A2 RID: 9634
			public class OUTFIT_TEMPLATE
			{
				// Token: 0x0400A598 RID: 42392
				public static LocString BUTTON_SAVE = "Save Template";

				// Token: 0x0400A599 RID: 42393
				public static LocString BUTTON_COPY = "Save a Copy";

				// Token: 0x0400A59A RID: 42394
				public static LocString TOOLTIP_SAVE_ERROR_LOCKED = "Cannot save this outfit because my colony doesn't own all of its blueprints yet";

				// Token: 0x0400A59B RID: 42395
				public static LocString TOOLTIP_SAVE_ERROR_READONLY = "This wardrobe staple cannot be altered\n\nMake a copy to save your changes";
			}

			// Token: 0x020025A3 RID: 9635
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400A59C RID: 42396
				public static LocString HEADER = "Discard changes to \"{OutfitName}\"?";

				// Token: 0x0400A59D RID: 42397
				public static LocString BODY = "There are unsaved changes which will be lost if you exit now.\n\nAre you sure you want to discard your changes?";

				// Token: 0x0400A59E RID: 42398
				public static LocString BUTTON_DISCARD = "Yes, discard changes";

				// Token: 0x0400A59F RID: 42399
				public static LocString BUTTON_RETURN = "Cancel";
			}

			// Token: 0x020025A4 RID: 9636
			public class COPY_POPUP
			{
				// Token: 0x0400A5A0 RID: 42400
				public static LocString HEADER = "RENAME COPY";
			}
		}

		// Token: 0x02001DA2 RID: 7586
		public class OUTFIT_NAME
		{
			// Token: 0x040085EA RID: 34282
			public static LocString NEW = "Custom Outfit";

			// Token: 0x040085EB RID: 34283
			public static LocString COPY_OF = "Copy of {OutfitName}";

			// Token: 0x040085EC RID: 34284
			public static LocString RESOLVE_CONFLICT = "{OutfitName} ({ConflictNumber})";

			// Token: 0x040085ED RID: 34285
			public static LocString ERROR_NAME_EXISTS = "There's already an outfit named \"{OutfitName}\"";

			// Token: 0x040085EE RID: 34286
			public static LocString MINIONS_OUTFIT = "{MinionName}'s Current Outfit";

			// Token: 0x040085EF RID: 34287
			public static LocString NONE = "Default Outfit";

			// Token: 0x040085F0 RID: 34288
			public static LocString NONE_JOY_RESPONSE = "Default Overjoyed Response";

			// Token: 0x040085F1 RID: 34289
			public static LocString NONE_ATMO_SUIT = "Default Atmo Suit";
		}

		// Token: 0x02001DA3 RID: 7587
		public class OUTFIT_DESCRIPTION
		{
			// Token: 0x040085F2 RID: 34290
			public static LocString CONTAINS_NON_OWNED_ITEMS = "This outfit cannot be worn because my colony doesn't have all of its blueprints yet.";

			// Token: 0x040085F3 RID: 34291
			public static LocString NO_JOY_RESPONSE_NAME = "Default Overjoyed Response";

			// Token: 0x040085F4 RID: 34292
			public static LocString NO_JOY_RESPONSE_DESC = "Default response to an overjoyed state.";
		}

		// Token: 0x02001DA4 RID: 7588
		public class MINION_BROWSER_SCREEN
		{
			// Token: 0x040085F5 RID: 34293
			public static LocString CATEGORY_HEADER = "DUPLICANTS";

			// Token: 0x040085F6 RID: 34294
			public static LocString BUTTON_CHANGE_OUTFIT = "Open Wardrobe";

			// Token: 0x040085F7 RID: 34295
			public static LocString BUTTON_EDIT_OUTFIT_ITEMS = "Restyle Outfit";

			// Token: 0x040085F8 RID: 34296
			public static LocString BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS = "Restyle Atmo Suit";

			// Token: 0x040085F9 RID: 34297
			public static LocString BUTTON_EDIT_JOY_RESPONSE = "Restyle Overjoyed Response";

			// Token: 0x040085FA RID: 34298
			public static LocString OUTFIT_TYPE_CLOTHING = "CLOTHING";

			// Token: 0x040085FB RID: 34299
			public static LocString OUTFIT_TYPE_JOY_RESPONSE = "OVERJOYED RESPONSE";

			// Token: 0x040085FC RID: 34300
			public static LocString OUTFIT_TYPE_ATMOSUIT = "ATMO SUIT";
		}

		// Token: 0x02001DA5 RID: 7589
		public class PERMIT_RARITY
		{
			// Token: 0x040085FD RID: 34301
			public static readonly LocString UNKNOWN = "Unknown";

			// Token: 0x040085FE RID: 34302
			public static readonly LocString UNIVERSAL = "Universal";

			// Token: 0x040085FF RID: 34303
			public static readonly LocString LOYALTY = "<color=#FFB037>Loyalty</color>";

			// Token: 0x04008600 RID: 34304
			public static readonly LocString COMMON = "<color=#97B2B9>Common</color>";

			// Token: 0x04008601 RID: 34305
			public static readonly LocString DECENT = "<color=#81EBDE>Decent</color>";

			// Token: 0x04008602 RID: 34306
			public static readonly LocString NIFTY = "<color=#71E379>Nifty</color>";

			// Token: 0x04008603 RID: 34307
			public static readonly LocString SPLENDID = "<color=#FF6DE7>Splendid</color>";
		}

		// Token: 0x02001DA6 RID: 7590
		public class OUTFITS
		{
			// Token: 0x020025A5 RID: 9637
			public class BASIC_BLACK
			{
				// Token: 0x0400A5A1 RID: 42401
				public static LocString NAME = "Basic Black Outfit";
			}

			// Token: 0x020025A6 RID: 9638
			public class BASIC_WHITE
			{
				// Token: 0x0400A5A2 RID: 42402
				public static LocString NAME = "Basic White Outfit";
			}

			// Token: 0x020025A7 RID: 9639
			public class BASIC_RED
			{
				// Token: 0x0400A5A3 RID: 42403
				public static LocString NAME = "Basic Red Outfit";
			}

			// Token: 0x020025A8 RID: 9640
			public class BASIC_ORANGE
			{
				// Token: 0x0400A5A4 RID: 42404
				public static LocString NAME = "Basic Orange Outfit";
			}

			// Token: 0x020025A9 RID: 9641
			public class BASIC_YELLOW
			{
				// Token: 0x0400A5A5 RID: 42405
				public static LocString NAME = "Basic Yellow Outfit";
			}

			// Token: 0x020025AA RID: 9642
			public class BASIC_GREEN
			{
				// Token: 0x0400A5A6 RID: 42406
				public static LocString NAME = "Basic Green Outfit";
			}

			// Token: 0x020025AB RID: 9643
			public class BASIC_AQUA
			{
				// Token: 0x0400A5A7 RID: 42407
				public static LocString NAME = "Basic Aqua Outfit";
			}

			// Token: 0x020025AC RID: 9644
			public class BASIC_PURPLE
			{
				// Token: 0x0400A5A8 RID: 42408
				public static LocString NAME = "Basic Purple Outfit";
			}

			// Token: 0x020025AD RID: 9645
			public class BASIC_PINK_ORCHID
			{
				// Token: 0x0400A5A9 RID: 42409
				public static LocString NAME = "Basic Bubblegum Outfit";
			}

			// Token: 0x020025AE RID: 9646
			public class BASIC_DEEPRED
			{
				// Token: 0x0400A5AA RID: 42410
				public static LocString NAME = "Team Captain Outfit";
			}

			// Token: 0x020025AF RID: 9647
			public class BASIC_BLUE_COBALT
			{
				// Token: 0x0400A5AB RID: 42411
				public static LocString NAME = "True Blue Outfit";
			}

			// Token: 0x020025B0 RID: 9648
			public class BASIC_PINK_FLAMINGO
			{
				// Token: 0x0400A5AC RID: 42412
				public static LocString NAME = "Pep Rally Outfit";
			}

			// Token: 0x020025B1 RID: 9649
			public class BASIC_GREEN_KELLY
			{
				// Token: 0x0400A5AD RID: 42413
				public static LocString NAME = "Go Team! Outfit";
			}

			// Token: 0x020025B2 RID: 9650
			public class BASIC_GREY_CHARCOAL
			{
				// Token: 0x0400A5AE RID: 42414
				public static LocString NAME = "Underdog Outfit";
			}

			// Token: 0x020025B3 RID: 9651
			public class BASIC_LEMON
			{
				// Token: 0x0400A5AF RID: 42415
				public static LocString NAME = "Team Hype Outfit";
			}

			// Token: 0x020025B4 RID: 9652
			public class BASIC_SATSUMA
			{
				// Token: 0x0400A5B0 RID: 42416
				public static LocString NAME = "Superfan Outfit";
			}

			// Token: 0x020025B5 RID: 9653
			public class JELLYPUFF_BLUEBERRY
			{
				// Token: 0x0400A5B1 RID: 42417
				public static LocString NAME = "Blueberry Jelly Outfit";
			}

			// Token: 0x020025B6 RID: 9654
			public class JELLYPUFF_GRAPE
			{
				// Token: 0x0400A5B2 RID: 42418
				public static LocString NAME = "Grape Jelly Outfit";
			}

			// Token: 0x020025B7 RID: 9655
			public class JELLYPUFF_LEMON
			{
				// Token: 0x0400A5B3 RID: 42419
				public static LocString NAME = "Lemon Jelly Outfit";
			}

			// Token: 0x020025B8 RID: 9656
			public class JELLYPUFF_LIME
			{
				// Token: 0x0400A5B4 RID: 42420
				public static LocString NAME = "Lime Jelly Outfit";
			}

			// Token: 0x020025B9 RID: 9657
			public class JELLYPUFF_SATSUMA
			{
				// Token: 0x0400A5B5 RID: 42421
				public static LocString NAME = "Satsuma Jelly Outfit";
			}

			// Token: 0x020025BA RID: 9658
			public class JELLYPUFF_STRAWBERRY
			{
				// Token: 0x0400A5B6 RID: 42422
				public static LocString NAME = "Strawberry Jelly Outfit";
			}

			// Token: 0x020025BB RID: 9659
			public class JELLYPUFF_WATERMELON
			{
				// Token: 0x0400A5B7 RID: 42423
				public static LocString NAME = "Watermelon Jelly Outfit";
			}

			// Token: 0x020025BC RID: 9660
			public class ATHLETE
			{
				// Token: 0x0400A5B8 RID: 42424
				public static LocString NAME = "Racing Outfit";
			}

			// Token: 0x020025BD RID: 9661
			public class CIRCUIT
			{
				// Token: 0x0400A5B9 RID: 42425
				public static LocString NAME = "LED Party Outfit";
			}

			// Token: 0x020025BE RID: 9662
			public class ATMOSUIT_LIMONE
			{
				// Token: 0x0400A5BA RID: 42426
				public static LocString NAME = "Citrus Atmo Outfit";
			}

			// Token: 0x020025BF RID: 9663
			public class ATMOSUIT_SPARKLE_RED
			{
				// Token: 0x0400A5BB RID: 42427
				public static LocString NAME = "Red Glitter Atmo Outfit";
			}

			// Token: 0x020025C0 RID: 9664
			public class ATMOSUIT_SPARKLE_BLUE
			{
				// Token: 0x0400A5BC RID: 42428
				public static LocString NAME = "Blue Glitter Atmo Outfit";
			}

			// Token: 0x020025C1 RID: 9665
			public class ATMOSUIT_SPARKLE_GREEN
			{
				// Token: 0x0400A5BD RID: 42429
				public static LocString NAME = "Green Glitter Atmo Outfit";
			}

			// Token: 0x020025C2 RID: 9666
			public class ATMOSUIT_SPARKLE_LAVENDER
			{
				// Token: 0x0400A5BE RID: 42430
				public static LocString NAME = "Violet Glitter Atmo Outfit";
			}

			// Token: 0x020025C3 RID: 9667
			public class ATMOSUIT_PUFT
			{
				// Token: 0x0400A5BF RID: 42431
				public static LocString NAME = "Puft Atmo Outfit";
			}

			// Token: 0x020025C4 RID: 9668
			public class ATMOSUIT_CONFETTI
			{
				// Token: 0x0400A5C0 RID: 42432
				public static LocString NAME = "Confetti Atmo Outfit";
			}

			// Token: 0x020025C5 RID: 9669
			public class ATMOSUIT_BASIC_PURPLE
			{
				// Token: 0x0400A5C1 RID: 42433
				public static LocString NAME = "Eggplant Atmo Outfit";
			}

			// Token: 0x020025C6 RID: 9670
			public class ATMOSUIT_PINK_PURPLE
			{
				// Token: 0x0400A5C2 RID: 42434
				public static LocString NAME = "Pink Punch Atmo Outfit";
			}

			// Token: 0x020025C7 RID: 9671
			public class ATMOSUIT_RED_GREY
			{
				// Token: 0x0400A5C3 RID: 42435
				public static LocString NAME = "Blastoff Atmo Outfit";
			}

			// Token: 0x020025C8 RID: 9672
			public class CANUXTUX
			{
				// Token: 0x0400A5C4 RID: 42436
				public static LocString NAME = "Canadian Tuxedo Outfit";
			}

			// Token: 0x020025C9 RID: 9673
			public class GONCHIES_STRAWBERRY
			{
				// Token: 0x0400A5C5 RID: 42437
				public static LocString NAME = "Executive Undies Outfit";
			}

			// Token: 0x020025CA RID: 9674
			public class GONCHIES_SATSUMA
			{
				// Token: 0x0400A5C6 RID: 42438
				public static LocString NAME = "Underling Undies Outfit";
			}

			// Token: 0x020025CB RID: 9675
			public class GONCHIES_LEMON
			{
				// Token: 0x0400A5C7 RID: 42439
				public static LocString NAME = "Groupthink Undies Outfit";
			}

			// Token: 0x020025CC RID: 9676
			public class GONCHIES_LIME
			{
				// Token: 0x0400A5C8 RID: 42440
				public static LocString NAME = "Stakeholder Undies Outfit";
			}

			// Token: 0x020025CD RID: 9677
			public class GONCHIES_BLUEBERRY
			{
				// Token: 0x0400A5C9 RID: 42441
				public static LocString NAME = "Admin Undies Outfit";
			}

			// Token: 0x020025CE RID: 9678
			public class GONCHIES_GRAPE
			{
				// Token: 0x0400A5CA RID: 42442
				public static LocString NAME = "Buzzword Undies Outfit";
			}

			// Token: 0x020025CF RID: 9679
			public class GONCHIES_WATERMELON
			{
				// Token: 0x0400A5CB RID: 42443
				public static LocString NAME = "Synergy Undies Outfit";
			}

			// Token: 0x020025D0 RID: 9680
			public class NERD
			{
				// Token: 0x0400A5CC RID: 42444
				public static LocString NAME = "Research Outfit";
			}

			// Token: 0x020025D1 RID: 9681
			public class REBELGI
			{
				// Token: 0x0400A5CD RID: 42445
				public static LocString NAME = "Rebel Gi Outfit";
			}
		}

		// Token: 0x02001DA7 RID: 7591
		public class ROLES_SCREEN
		{
			// Token: 0x04008604 RID: 34308
			public static LocString MANAGEMENT_BUTTON = "JOBS";

			// Token: 0x04008605 RID: 34309
			public static LocString ROLE_PROGRESS = "<b>Job Experience: {0}/{1}</b>\nDuplicants can become eligible for specialized jobs by maxing their current job experience";

			// Token: 0x04008606 RID: 34310
			public static LocString NO_JOB_STATION_WARNING = string.Concat(new string[]
			{
				"Build a ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" to unlock this menu\n\nThe ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x04008607 RID: 34311
			public static LocString AUTO_PRIORITIZE = "Auto-Prioritize:";

			// Token: 0x04008608 RID: 34312
			public static LocString AUTO_PRIORITIZE_ENABLED = "Duplicant priorities are automatically reconfigured when they are assigned a new job";

			// Token: 0x04008609 RID: 34313
			public static LocString AUTO_PRIORITIZE_DISABLED = "Duplicant priorities can only be changed manually";

			// Token: 0x0400860A RID: 34314
			public static LocString EXPECTATION_ALERT_EXPECTATION = "Current Morale: {0}\nJob Morale Needs: {1}";

			// Token: 0x0400860B RID: 34315
			public static LocString EXPECTATION_ALERT_JOB = "Current Morale: {0}\n{2} Minimum Morale: {1}";

			// Token: 0x0400860C RID: 34316
			public static LocString EXPECTATION_ALERT_TARGET_JOB = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

			// Token: 0x0400860D RID: 34317
			public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x0400860E RID: 34318
			public static LocString EXPECTATION_ALERT_DESC_JOB = "This Duplicant's Morale is too low to handle the assigned job, which will cause them Stress over time.";

			// Token: 0x0400860F RID: 34319
			public static LocString EXPECTATION_ALERT_DESC_TARGET_JOB = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x04008610 RID: 34320
			public static LocString HIGHEST_EXPECTATIONS_TIER = "<b>Highest Expectations</b>";

			// Token: 0x04008611 RID: 34321
			public static LocString ADDED_EXPECTATIONS_AMOUNT = " (+{0} Expectation)";

			// Token: 0x020025D2 RID: 9682
			public class WIDGET
			{
				// Token: 0x0400A5CE RID: 42446
				public static LocString NUMBER_OF_MASTERS_TOOLTIP = "<b>Duplicants who have mastered this job:</b>{0}";

				// Token: 0x0400A5CF RID: 42447
				public static LocString NO_MASTERS_TOOLTIP = "<b>No Duplicants have mastered this job</b>";
			}

			// Token: 0x020025D3 RID: 9683
			public class TIER_NAMES
			{
				// Token: 0x0400A5D0 RID: 42448
				public static LocString ZERO = "Tier 0";

				// Token: 0x0400A5D1 RID: 42449
				public static LocString ONE = "Tier 1";

				// Token: 0x0400A5D2 RID: 42450
				public static LocString TWO = "Tier 2";

				// Token: 0x0400A5D3 RID: 42451
				public static LocString THREE = "Tier 3";

				// Token: 0x0400A5D4 RID: 42452
				public static LocString FOUR = "Tier 4";

				// Token: 0x0400A5D5 RID: 42453
				public static LocString FIVE = "Tier 5";

				// Token: 0x0400A5D6 RID: 42454
				public static LocString SIX = "Tier 6";

				// Token: 0x0400A5D7 RID: 42455
				public static LocString SEVEN = "Tier 7";

				// Token: 0x0400A5D8 RID: 42456
				public static LocString EIGHT = "Tier 8";

				// Token: 0x0400A5D9 RID: 42457
				public static LocString NINE = "Tier 9";
			}

			// Token: 0x020025D4 RID: 9684
			public class SLOTS
			{
				// Token: 0x0400A5DA RID: 42458
				public static LocString UNASSIGNED = "Vacant Position";

				// Token: 0x0400A5DB RID: 42459
				public static LocString UNASSIGNED_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to assign a Duplicant to this job opening";

				// Token: 0x0400A5DC RID: 42460
				public static LocString NOSLOTS = "No slots available";

				// Token: 0x0400A5DD RID: 42461
				public static LocString NO_ELIGIBLE_DUPLICANTS = "No Duplicants meet the requirements for this job";

				// Token: 0x0400A5DE RID: 42462
				public static LocString ASSIGNMENT_PENDING = "(Pending)";

				// Token: 0x0400A5DF RID: 42463
				public static LocString PICK_JOB = "No Job";

				// Token: 0x0400A5E0 RID: 42464
				public static LocString PICK_DUPLICANT = "None";
			}

			// Token: 0x020025D5 RID: 9685
			public class DROPDOWN
			{
				// Token: 0x0400A5E1 RID: 42465
				public static LocString NAME_AND_ROLE = "{0} <color=#F44A47FF>({1})</color>";

				// Token: 0x0400A5E2 RID: 42466
				public static LocString ALREADY_ROLE = "(Currently {0})";
			}

			// Token: 0x020025D6 RID: 9686
			public class SIDEBAR
			{
				// Token: 0x0400A5E3 RID: 42467
				public static LocString ASSIGNED_DUPLICANTS = "Assigned Duplicants";

				// Token: 0x0400A5E4 RID: 42468
				public static LocString UNASSIGNED_DUPLICANTS = "Unassigned Duplicants";

				// Token: 0x0400A5E5 RID: 42469
				public static LocString UNASSIGN = "Unassign job";
			}

			// Token: 0x020025D7 RID: 9687
			public class PRIORITY
			{
				// Token: 0x0400A5E6 RID: 42470
				public static LocString TITLE = "Job Priorities";

				// Token: 0x0400A5E7 RID: 42471
				public static LocString DESCRIPTION = "{0}s prioritize these work errands: ";

				// Token: 0x0400A5E8 RID: 42472
				public static LocString NO_EFFECT = "This job does not affect errand prioritization";
			}

			// Token: 0x020025D8 RID: 9688
			public class RESUME
			{
				// Token: 0x0400A5E9 RID: 42473
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400A5EA RID: 42474
				public static LocString PREVIOUS_ROLES = "PREVIOUS DUTIES";

				// Token: 0x0400A5EB RID: 42475
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400A5EC RID: 42476
				public static LocString NO_SELECTION = "No Duplicant selected";
			}

			// Token: 0x020025D9 RID: 9689
			public class PERKS
			{
				// Token: 0x0400A5ED RID: 42477
				public static LocString TITLE_BASICTRAINING = "Basic Job Training";

				// Token: 0x0400A5EE RID: 42478
				public static LocString TITLE_MORETRAINING = "Additional Job Training";

				// Token: 0x0400A5EF RID: 42479
				public static LocString NO_PERKS = "This job comes with no training";

				// Token: 0x0400A5F0 RID: 42480
				public static LocString ATTRIBUTE_EFFECT_FMT = "<b>{0}</b> " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

				// Token: 0x020030A1 RID: 12449
				public class CAN_DIG_VERY_FIRM
				{
					// Token: 0x0400C43B RID: 50235
					public static LocString DESCRIPTION = UI.FormatAsLink(ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + " Material", "HARDNESS") + " Mining";
				}

				// Token: 0x020030A2 RID: 12450
				public class CAN_DIG_NEARLY_IMPENETRABLE
				{
					// Token: 0x0400C43C RID: 50236
					public static LocString DESCRIPTION = UI.FormatAsLink("Abyssalite", "KATAIRITE") + " Mining";
				}

				// Token: 0x020030A3 RID: 12451
				public class CAN_DIG_SUPER_SUPER_HARD
				{
					// Token: 0x0400C43D RID: 50237
					public static LocString DESCRIPTION = UI.FormatAsLink("Diamond", "DIAMOND") + " and " + UI.FormatAsLink("Obsidian", "OBSIDIAN") + " Mining";
				}

				// Token: 0x020030A4 RID: 12452
				public class CAN_DIG_RADIOACTIVE_MATERIALS
				{
					// Token: 0x0400C43E RID: 50238
					public static LocString DESCRIPTION = UI.FormatAsLink("Corium", "CORIUM") + " Mining";
				}

				// Token: 0x020030A5 RID: 12453
				public class CAN_DIG_UNOBTANIUM
				{
					// Token: 0x0400C43F RID: 50239
					public static LocString DESCRIPTION = UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " Mining";
				}

				// Token: 0x020030A6 RID: 12454
				public class CAN_ART
				{
					// Token: 0x0400C440 RID: 50240
					public static LocString DESCRIPTION = "Can produce artwork using " + BUILDINGS.PREFABS.CANVAS.NAME + " and " + BUILDINGS.PREFABS.SCULPTURE.NAME;
				}

				// Token: 0x020030A7 RID: 12455
				public class CAN_ART_UGLY
				{
					// Token: 0x0400C441 RID: 50241
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Crude" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020030A8 RID: 12456
				public class CAN_ART_OKAY
				{
					// Token: 0x0400C442 RID: 50242
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Mediocre" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020030A9 RID: 12457
				public class CAN_ART_GREAT
				{
					// Token: 0x0400C443 RID: 50243
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Master" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020030AA RID: 12458
				public class CAN_FARM_TINKER
				{
					// Token: 0x0400C444 RID: 50244
					public static LocString DESCRIPTION = UI.FormatAsLink("Crop Tending", "PLANTS") + " and " + ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x020030AB RID: 12459
				public class CAN_IDENTIFY_MUTANT_SEEDS
				{
					// Token: 0x0400C445 RID: 50245
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can identify ",
						UI.PRE_KEYWORD,
						"Mutant Seeds",
						UI.PST_KEYWORD,
						" at the ",
						BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME
					});
				}

				// Token: 0x020030AC RID: 12460
				public class CAN_WRANGLE_CREATURES
				{
					// Token: 0x0400C446 RID: 50246
					public static LocString DESCRIPTION = "Critter Wrangling";
				}

				// Token: 0x020030AD RID: 12461
				public class CAN_USE_RANCH_STATION
				{
					// Token: 0x0400C447 RID: 50247
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.RANCHSTATION.NAME + " Usage";
				}

				// Token: 0x020030AE RID: 12462
				public class CAN_USE_MILKING_STATION
				{
					// Token: 0x0400C448 RID: 50248
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MILKINGSTATION.NAME + " Usage";
				}

				// Token: 0x020030AF RID: 12463
				public class CAN_POWER_TINKER
				{
					// Token: 0x0400C449 RID: 50249
					public static LocString DESCRIPTION = UI.FormatAsLink("Generator Tuning", "POWER") + " usage and " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x020030B0 RID: 12464
				public class CAN_ELECTRIC_GRILL
				{
					// Token: 0x0400C44A RID: 50250
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COOKINGSTATION.NAME + " Usage";
				}

				// Token: 0x020030B1 RID: 12465
				public class CAN_SPICE_GRINDER
				{
					// Token: 0x0400C44B RID: 50251
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SPICEGRINDER.NAME + " Usage";
				}

				// Token: 0x020030B2 RID: 12466
				public class CAN_MAKE_MISSILES
				{
					// Token: 0x0400C44C RID: 50252
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSILEFABRICATOR.NAME + " Usage";
				}

				// Token: 0x020030B3 RID: 12467
				public class ADVANCED_RESEARCH
				{
					// Token: 0x0400C44D RID: 50253
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020030B4 RID: 12468
				public class INTERSTELLAR_RESEARCH
				{
					// Token: 0x0400C44E RID: 50254
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020030B5 RID: 12469
				public class NUCLEAR_RESEARCH
				{
					// Token: 0x0400C44F RID: 50255
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020030B6 RID: 12470
				public class ORBITAL_RESEARCH
				{
					// Token: 0x0400C450 RID: 50256
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020030B7 RID: 12471
				public class GEYSER_TUNING
				{
					// Token: 0x0400C451 RID: 50257
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.GEOTUNER.NAME + " Usage";
				}

				// Token: 0x020030B8 RID: 12472
				public class CAN_CLOTHING_ALTERATION
				{
					// Token: 0x0400C452 RID: 50258
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLOTHINGALTERATIONSTATION.NAME + " Usage";
				}

				// Token: 0x020030B9 RID: 12473
				public class CAN_STUDY_WORLD_OBJECTS
				{
					// Token: 0x0400C453 RID: 50259
					public static LocString DESCRIPTION = "Geographical Analysis";
				}

				// Token: 0x020030BA RID: 12474
				public class CAN_STUDY_ARTIFACTS
				{
					// Token: 0x0400C454 RID: 50260
					public static LocString DESCRIPTION = "Artifact Analysis";
				}

				// Token: 0x020030BB RID: 12475
				public class CAN_USE_CLUSTER_TELESCOPE
				{
					// Token: 0x0400C455 RID: 50261
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " Usage";
				}

				// Token: 0x020030BC RID: 12476
				public class EXOSUIT_EXPERTISE
				{
					// Token: 0x0400C456 RID: 50262
					public static LocString DESCRIPTION = UI.FormatAsLink("Exosuit", "EXOSUIT") + " Penalty Reduction";
				}

				// Token: 0x020030BD RID: 12477
				public class EXOSUIT_DURABILITY
				{
					// Token: 0x0400C457 RID: 50263
					public static LocString DESCRIPTION = "Slows " + UI.FormatAsLink("Exosuit", "EXOSUIT") + " Durability Damage";
				}

				// Token: 0x020030BE RID: 12478
				public class CONVEYOR_BUILD
				{
					// Token: 0x0400C458 RID: 50264
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " Construction";
				}

				// Token: 0x020030BF RID: 12479
				public class CAN_DO_PLUMBING
				{
					// Token: 0x0400C459 RID: 50265
					public static LocString DESCRIPTION = "Pipe Emptying";
				}

				// Token: 0x020030C0 RID: 12480
				public class CAN_USE_ROCKETS
				{
					// Token: 0x0400C45A RID: 50266
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COMMANDMODULE.NAME + " Usage";
				}

				// Token: 0x020030C1 RID: 12481
				public class CAN_DO_ASTRONAUT_TRAINING
				{
					// Token: 0x0400C45B RID: 50267
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ASTRONAUTTRAININGCENTER.NAME + " Usage";
				}

				// Token: 0x020030C2 RID: 12482
				public class CAN_MISSION_CONTROL
				{
					// Token: 0x0400C45C RID: 50268
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSIONCONTROL.NAME + " Usage";
				}

				// Token: 0x020030C3 RID: 12483
				public class CAN_PILOT_ROCKET
				{
					// Token: 0x0400C45D RID: 50269
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " Usage";
				}

				// Token: 0x020030C4 RID: 12484
				public class CAN_COMPOUND
				{
					// Token: 0x0400C45E RID: 50270
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.APOTHECARY.NAME + " Usage";
				}

				// Token: 0x020030C5 RID: 12485
				public class CAN_DOCTOR
				{
					// Token: 0x0400C45F RID: 50271
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x020030C6 RID: 12486
				public class CAN_ADVANCED_MEDICINE
				{
					// Token: 0x0400C460 RID: 50272
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x020030C7 RID: 12487
				public class CAN_DEMOLISH
				{
					// Token: 0x0400C461 RID: 50273
					public static LocString DESCRIPTION = "Demolish Gravitas Buildings";
				}
			}

			// Token: 0x020025DA RID: 9690
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400A5F1 RID: 42481
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400A5F2 RID: 42482
				public static LocString NONE = "This position has no qualification requirements";

				// Token: 0x0400A5F3 RID: 42483
				public static LocString ALREADY_IS_ROLE = "{0} <b>is already</b> assigned to the {1} position";

				// Token: 0x0400A5F4 RID: 42484
				public static LocString ALREADY_IS_JOBLESS = "{0} <b>is already</b> unemployed";

				// Token: 0x0400A5F5 RID: 42485
				public static LocString MASTERED = "{0} has mastered the {1} position";

				// Token: 0x0400A5F6 RID: 42486
				public static LocString WILL_BE_UNASSIGNED = "Note: Assigning {0} to {1} will <color=#F44A47FF>unassign</color> them from {2}";

				// Token: 0x0400A5F7 RID: 42487
				public static LocString RELEVANT_ATTRIBUTES = "Relevant skills:";

				// Token: 0x0400A5F8 RID: 42488
				public static LocString APTITUDES = "Interests";

				// Token: 0x0400A5F9 RID: 42489
				public static LocString RELEVANT_APTITUDES = "Relevant Interests:";

				// Token: 0x0400A5FA RID: 42490
				public static LocString NO_APTITUDE = "None";

				// Token: 0x020030C8 RID: 12488
				public class ELIGIBILITY
				{
					// Token: 0x0400C462 RID: 50274
					public static LocString ELIGIBLE = "{0} is qualified for the {1} position";

					// Token: 0x0400C463 RID: 50275
					public static LocString INELIGIBLE = "{0} is <color=#F44A47FF>not qualified</color> for the {1} position";
				}

				// Token: 0x020030C9 RID: 12489
				public class UNEMPLOYED
				{
					// Token: 0x0400C464 RID: 50276
					public static LocString NAME = "Unassigned";

					// Token: 0x0400C465 RID: 50277
					public static LocString DESCRIPTION = "Duplicant must not already have a job assignment";
				}

				// Token: 0x020030CA RID: 12490
				public class HAS_COLONY_LEADER
				{
					// Token: 0x0400C466 RID: 50278
					public static LocString NAME = "Has colony leader";

					// Token: 0x0400C467 RID: 50279
					public static LocString DESCRIPTION = "A colony leader must be assigned";
				}

				// Token: 0x020030CB RID: 12491
				public class HAS_ATTRIBUTE_DIGGING_BASIC
				{
					// Token: 0x0400C468 RID: 50280
					public static LocString NAME = "Basic Digging";

					// Token: 0x0400C469 RID: 50281
					public static LocString DESCRIPTION = "Must have at least {0} digging skill";
				}

				// Token: 0x020030CC RID: 12492
				public class HAS_ATTRIBUTE_COOKING_BASIC
				{
					// Token: 0x0400C46A RID: 50282
					public static LocString NAME = "Basic Cooking";

					// Token: 0x0400C46B RID: 50283
					public static LocString DESCRIPTION = "Must have at least {0} cooking skill";
				}

				// Token: 0x020030CD RID: 12493
				public class HAS_ATTRIBUTE_LEARNING_BASIC
				{
					// Token: 0x0400C46C RID: 50284
					public static LocString NAME = "Basic Learning";

					// Token: 0x0400C46D RID: 50285
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x020030CE RID: 12494
				public class HAS_ATTRIBUTE_LEARNING_MEDIUM
				{
					// Token: 0x0400C46E RID: 50286
					public static LocString NAME = "Medium Learning";

					// Token: 0x0400C46F RID: 50287
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x020030CF RID: 12495
				public class HAS_EXPERIENCE
				{
					// Token: 0x0400C470 RID: 50288
					public static LocString NAME = "{0} Experience";

					// Token: 0x0400C471 RID: 50289
					public static LocString DESCRIPTION = "Mastery of the <b>{0}</b> job";
				}

				// Token: 0x020030D0 RID: 12496
				public class HAS_COMPLETED_ANY_OTHER_ROLE
				{
					// Token: 0x0400C472 RID: 50290
					public static LocString NAME = "General Experience";

					// Token: 0x0400C473 RID: 50291
					public static LocString DESCRIPTION = "Mastery of <b>at least one</b> job";
				}

				// Token: 0x020030D1 RID: 12497
				public class CHOREGROUP_ENABLED
				{
					// Token: 0x0400C474 RID: 50292
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400C475 RID: 50293
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> jobs";
				}
			}

			// Token: 0x020025DB RID: 9691
			public class EXPECTATIONS
			{
				// Token: 0x0400A5FB RID: 42491
				public static LocString TITLE = "Special Provisions Request";

				// Token: 0x0400A5FC RID: 42492
				public static LocString NO_EXPECTATIONS = "No additional provisions are required to perform this job";

				// Token: 0x020030D2 RID: 12498
				public class PRIVATE_ROOM
				{
					// Token: 0x0400C476 RID: 50294
					public static LocString NAME = "Private Bedroom";

					// Token: 0x0400C477 RID: 50295
					public static LocString DESCRIPTION = "Duplicants in this job would appreciate their own place to unwind";
				}

				// Token: 0x020030D3 RID: 12499
				public class FOOD_QUALITY
				{
					// Token: 0x020033CC RID: 13260
					public class MINOR
					{
						// Token: 0x0400CBFF RID: 52223
						public static LocString NAME = "Standard Food";

						// Token: 0x0400CC00 RID: 52224
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire food that meets basic living standards";
					}

					// Token: 0x020033CD RID: 13261
					public class MEDIUM
					{
						// Token: 0x0400CC01 RID: 52225
						public static LocString NAME = "Good Food";

						// Token: 0x0400CC02 RID: 52226
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire decent food for their efforts";
					}

					// Token: 0x020033CE RID: 13262
					public class HIGH
					{
						// Token: 0x0400CC03 RID: 52227
						public static LocString NAME = "Great Food";

						// Token: 0x0400CC04 RID: 52228
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire better than average food";
					}

					// Token: 0x020033CF RID: 13263
					public class VERY_HIGH
					{
						// Token: 0x0400CC05 RID: 52229
						public static LocString NAME = "Superb Food";

						// Token: 0x0400CC06 RID: 52230
						public static LocString DESCRIPTION = "Duplicants employed in this Tier have a refined taste for food";
					}

					// Token: 0x020033D0 RID: 13264
					public class EXCEPTIONAL
					{
						// Token: 0x0400CC07 RID: 52231
						public static LocString NAME = "Ambrosial Food";

						// Token: 0x0400CC08 RID: 52232
						public static LocString DESCRIPTION = "Duplicants employed in this Tier expect only the best cuisine";
					}
				}

				// Token: 0x020030D4 RID: 12500
				public class DECOR
				{
					// Token: 0x020033D1 RID: 13265
					public class MINOR
					{
						// Token: 0x0400CC09 RID: 52233
						public static LocString NAME = "Minor Decor";

						// Token: 0x0400CC0A RID: 52234
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire slightly improved colony decor";
					}

					// Token: 0x020033D2 RID: 13266
					public class MEDIUM
					{
						// Token: 0x0400CC0B RID: 52235
						public static LocString NAME = "Medium Decor";

						// Token: 0x0400CC0C RID: 52236
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire reasonably improved colony decor";
					}

					// Token: 0x020033D3 RID: 13267
					public class HIGH
					{
						// Token: 0x0400CC0D RID: 52237
						public static LocString NAME = "High Decor";

						// Token: 0x0400CC0E RID: 52238
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire a decent increase in colony decor";
					}

					// Token: 0x020033D4 RID: 13268
					public class VERY_HIGH
					{
						// Token: 0x0400CC0F RID: 52239
						public static LocString NAME = "Superb Decor";

						// Token: 0x0400CC10 RID: 52240
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire majorly improved colony decor";
					}

					// Token: 0x020033D5 RID: 13269
					public class UNREASONABLE
					{
						// Token: 0x0400CC11 RID: 52241
						public static LocString NAME = "Decadent Decor";

						// Token: 0x0400CC12 RID: 52242
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire unrealistically luxurious improvements to decor";
					}
				}

				// Token: 0x020030D5 RID: 12501
				public class QUALITYOFLIFE
				{
					// Token: 0x020033D6 RID: 13270
					public class TIER0
					{
						// Token: 0x0400CC13 RID: 52243
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC14 RID: 52244
						public static LocString DESCRIPTION = "Tier 0";
					}

					// Token: 0x020033D7 RID: 13271
					public class TIER1
					{
						// Token: 0x0400CC15 RID: 52245
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC16 RID: 52246
						public static LocString DESCRIPTION = "Tier 1";
					}

					// Token: 0x020033D8 RID: 13272
					public class TIER2
					{
						// Token: 0x0400CC17 RID: 52247
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC18 RID: 52248
						public static LocString DESCRIPTION = "Tier 2";
					}

					// Token: 0x020033D9 RID: 13273
					public class TIER3
					{
						// Token: 0x0400CC19 RID: 52249
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC1A RID: 52250
						public static LocString DESCRIPTION = "Tier 3";
					}

					// Token: 0x020033DA RID: 13274
					public class TIER4
					{
						// Token: 0x0400CC1B RID: 52251
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC1C RID: 52252
						public static LocString DESCRIPTION = "Tier 4";
					}

					// Token: 0x020033DB RID: 13275
					public class TIER5
					{
						// Token: 0x0400CC1D RID: 52253
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC1E RID: 52254
						public static LocString DESCRIPTION = "Tier 5";
					}

					// Token: 0x020033DC RID: 13276
					public class TIER6
					{
						// Token: 0x0400CC1F RID: 52255
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC20 RID: 52256
						public static LocString DESCRIPTION = "Tier 6";
					}

					// Token: 0x020033DD RID: 13277
					public class TIER7
					{
						// Token: 0x0400CC21 RID: 52257
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC22 RID: 52258
						public static LocString DESCRIPTION = "Tier 7";
					}

					// Token: 0x020033DE RID: 13278
					public class TIER8
					{
						// Token: 0x0400CC23 RID: 52259
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400CC24 RID: 52260
						public static LocString DESCRIPTION = "Tier 8";
					}
				}
			}
		}

		// Token: 0x02001DA8 RID: 7592
		public class GAMEPLAY_EVENT_INFO_SCREEN
		{
			// Token: 0x04008612 RID: 34322
			public static LocString WHERE = "WHERE: {0}";

			// Token: 0x04008613 RID: 34323
			public static LocString WHEN = "WHEN: {0}";
		}

		// Token: 0x02001DA9 RID: 7593
		public class DEBUG_TOOLS
		{
			// Token: 0x04008614 RID: 34324
			public static LocString ENTER_TEXT = "";

			// Token: 0x04008615 RID: 34325
			public static LocString DEBUG_ACTIVE = "Debug tools active";

			// Token: 0x04008616 RID: 34326
			public static LocString INVALID_LOCATION = "Invalid Location";

			// Token: 0x020025DC RID: 9692
			public class PAINT_ELEMENTS_SCREEN
			{
				// Token: 0x0400A5FD RID: 42493
				public static LocString TITLE = "CELL PAINTER";

				// Token: 0x0400A5FE RID: 42494
				public static LocString ELEMENT = "Element";

				// Token: 0x0400A5FF RID: 42495
				public static LocString MASS_KG = "Mass (kg)";

				// Token: 0x0400A600 RID: 42496
				public static LocString TEMPERATURE_KELVIN = "Temperature (K)";

				// Token: 0x0400A601 RID: 42497
				public static LocString DISEASE = "Disease";

				// Token: 0x0400A602 RID: 42498
				public static LocString DISEASE_COUNT = "Disease Count";

				// Token: 0x0400A603 RID: 42499
				public static LocString BUILDINGS = "Buildings:";

				// Token: 0x0400A604 RID: 42500
				public static LocString CELLS = "Cells:";

				// Token: 0x0400A605 RID: 42501
				public static LocString ADD_FOW_MASK = "Prevent FoW Reveal";

				// Token: 0x0400A606 RID: 42502
				public static LocString REMOVE_FOW_MASK = "Allow FoW Reveal";

				// Token: 0x0400A607 RID: 42503
				public static LocString PAINT = "Paint";

				// Token: 0x0400A608 RID: 42504
				public static LocString SAMPLE = "Sample";

				// Token: 0x0400A609 RID: 42505
				public static LocString STORE = "Store";

				// Token: 0x0400A60A RID: 42506
				public static LocString FILL = "Fill";

				// Token: 0x0400A60B RID: 42507
				public static LocString SPAWN_ALL = "Spawn All (Slow)";
			}

			// Token: 0x020025DD RID: 9693
			public class SAVE_BASE_TEMPLATE
			{
				// Token: 0x0400A60C RID: 42508
				public static LocString TITLE = "Base and World Tools";

				// Token: 0x0400A60D RID: 42509
				public static LocString SAVE_TITLE = "Save Selection";

				// Token: 0x0400A60E RID: 42510
				public static LocString CLEAR_BUTTON = "Clear Floor";

				// Token: 0x0400A60F RID: 42511
				public static LocString DESTROY_BUTTON = "Destroy";

				// Token: 0x0400A610 RID: 42512
				public static LocString DECONSTRUCT_BUTTON = "Deconstruct";

				// Token: 0x0400A611 RID: 42513
				public static LocString CLEAR_SELECTION_BUTTON = "Clear Selection";

				// Token: 0x0400A612 RID: 42514
				public static LocString DEFAULT_SAVE_NAME = "TemplateSaveName";

				// Token: 0x0400A613 RID: 42515
				public static LocString MORE = "More";

				// Token: 0x0400A614 RID: 42516
				public static LocString BASE_GAME_FOLDER_NAME = "Base Game";

				// Token: 0x020030D6 RID: 12502
				public class SELECTION_INFO_PANEL
				{
					// Token: 0x0400C478 RID: 50296
					public static LocString TOTAL_MASS = "Total mass: {0}";

					// Token: 0x0400C479 RID: 50297
					public static LocString AVERAGE_MASS = "Average cell mass: {0}";

					// Token: 0x0400C47A RID: 50298
					public static LocString AVERAGE_TEMPERATURE = "Average temperature: {0}";

					// Token: 0x0400C47B RID: 50299
					public static LocString TOTAL_JOULES = "Total joules: {0}";

					// Token: 0x0400C47C RID: 50300
					public static LocString JOULES_PER_KILOGRAM = "Joules per kilogram: {0}";

					// Token: 0x0400C47D RID: 50301
					public static LocString TOTAL_RADS = "Total rads: {0}";

					// Token: 0x0400C47E RID: 50302
					public static LocString AVERAGE_RADS = "Average rads: {0}";
				}
			}
		}

		// Token: 0x02001DAA RID: 7594
		public class WORLDGEN
		{
			// Token: 0x04008617 RID: 34327
			public static LocString NOHEADERS = "";

			// Token: 0x04008618 RID: 34328
			public static LocString COMPLETE = "Success! Space adventure awaits.";

			// Token: 0x04008619 RID: 34329
			public static LocString FAILED = "Goodness, has this ever gone terribly wrong!";

			// Token: 0x0400861A RID: 34330
			public static LocString RESTARTING = "Rebooting...";

			// Token: 0x0400861B RID: 34331
			public static LocString LOADING = "Loading world...";

			// Token: 0x0400861C RID: 34332
			public static LocString GENERATINGWORLD = "The Galaxy Synthesizer";

			// Token: 0x0400861D RID: 34333
			public static LocString CHOOSEWORLDSIZE = "Select the magnitude of your new galaxy.";

			// Token: 0x0400861E RID: 34334
			public static LocString USING_PLAYER_SEED = "Using selected worldgen seed: {0}";

			// Token: 0x0400861F RID: 34335
			public static LocString CLEARINGLEVEL = "Staring into the void...";

			// Token: 0x04008620 RID: 34336
			public static LocString RETRYCOUNT = "Oh dear, let's try that again.";

			// Token: 0x04008621 RID: 34337
			public static LocString GENERATESOLARSYSTEM = "Catalyzing Big Bang...";

			// Token: 0x04008622 RID: 34338
			public static LocString GENERATESOLARSYSTEM1 = "Catalyzing Big Bang...";

			// Token: 0x04008623 RID: 34339
			public static LocString GENERATESOLARSYSTEM2 = "Catalyzing Big Bang...";

			// Token: 0x04008624 RID: 34340
			public static LocString GENERATESOLARSYSTEM3 = "Catalyzing Big Bang...";

			// Token: 0x04008625 RID: 34341
			public static LocString GENERATESOLARSYSTEM4 = "Catalyzing Big Bang...";

			// Token: 0x04008626 RID: 34342
			public static LocString GENERATESOLARSYSTEM5 = "Catalyzing Big Bang...";

			// Token: 0x04008627 RID: 34343
			public static LocString GENERATESOLARSYSTEM6 = "Approaching event horizon...";

			// Token: 0x04008628 RID: 34344
			public static LocString GENERATESOLARSYSTEM7 = "Approaching event horizon...";

			// Token: 0x04008629 RID: 34345
			public static LocString GENERATESOLARSYSTEM8 = "Approaching event horizon...";

			// Token: 0x0400862A RID: 34346
			public static LocString GENERATESOLARSYSTEM9 = "Approaching event horizon...";

			// Token: 0x0400862B RID: 34347
			public static LocString SETUPNOISE = "BANG!";

			// Token: 0x0400862C RID: 34348
			public static LocString BUILDNOISESOURCE = "Sorting quadrillions of atoms...";

			// Token: 0x0400862D RID: 34349
			public static LocString BUILDNOISESOURCE1 = "Sorting quadrillions of atoms...";

			// Token: 0x0400862E RID: 34350
			public static LocString BUILDNOISESOURCE2 = "Sorting quadrillions of atoms...";

			// Token: 0x0400862F RID: 34351
			public static LocString BUILDNOISESOURCE3 = "Ironing the fabric of creation...";

			// Token: 0x04008630 RID: 34352
			public static LocString BUILDNOISESOURCE4 = "Ironing the fabric of creation...";

			// Token: 0x04008631 RID: 34353
			public static LocString BUILDNOISESOURCE5 = "Ironing the fabric of creation...";

			// Token: 0x04008632 RID: 34354
			public static LocString BUILDNOISESOURCE6 = "Taking hot meteor shower...";

			// Token: 0x04008633 RID: 34355
			public static LocString BUILDNOISESOURCE7 = "Tightening asteroid belts...";

			// Token: 0x04008634 RID: 34356
			public static LocString BUILDNOISESOURCE8 = "Tightening asteroid belts...";

			// Token: 0x04008635 RID: 34357
			public static LocString BUILDNOISESOURCE9 = "Tightening asteroid belts...";

			// Token: 0x04008636 RID: 34358
			public static LocString GENERATENOISE = "Baking igneous rock...";

			// Token: 0x04008637 RID: 34359
			public static LocString GENERATENOISE1 = "Multilayering sediment...";

			// Token: 0x04008638 RID: 34360
			public static LocString GENERATENOISE2 = "Multilayering sediment...";

			// Token: 0x04008639 RID: 34361
			public static LocString GENERATENOISE3 = "Multilayering sediment...";

			// Token: 0x0400863A RID: 34362
			public static LocString GENERATENOISE4 = "Superheating gases...";

			// Token: 0x0400863B RID: 34363
			public static LocString GENERATENOISE5 = "Superheating gases...";

			// Token: 0x0400863C RID: 34364
			public static LocString GENERATENOISE6 = "Superheating gases...";

			// Token: 0x0400863D RID: 34365
			public static LocString GENERATENOISE7 = "Vacuuming out vacuums...";

			// Token: 0x0400863E RID: 34366
			public static LocString GENERATENOISE8 = "Vacuuming out vacuums...";

			// Token: 0x0400863F RID: 34367
			public static LocString GENERATENOISE9 = "Vacuuming out vacuums...";

			// Token: 0x04008640 RID: 34368
			public static LocString NORMALISENOISE = "Interpolating suffocating gas...";

			// Token: 0x04008641 RID: 34369
			public static LocString WORLDLAYOUT = "Freezing ice formations...";

			// Token: 0x04008642 RID: 34370
			public static LocString WORLDLAYOUT1 = "Freezing ice formations...";

			// Token: 0x04008643 RID: 34371
			public static LocString WORLDLAYOUT2 = "Freezing ice formations...";

			// Token: 0x04008644 RID: 34372
			public static LocString WORLDLAYOUT3 = "Freezing ice formations...";

			// Token: 0x04008645 RID: 34373
			public static LocString WORLDLAYOUT4 = "Melting magma...";

			// Token: 0x04008646 RID: 34374
			public static LocString WORLDLAYOUT5 = "Melting magma...";

			// Token: 0x04008647 RID: 34375
			public static LocString WORLDLAYOUT6 = "Melting magma...";

			// Token: 0x04008648 RID: 34376
			public static LocString WORLDLAYOUT7 = "Sprinkling sand...";

			// Token: 0x04008649 RID: 34377
			public static LocString WORLDLAYOUT8 = "Sprinkling sand...";

			// Token: 0x0400864A RID: 34378
			public static LocString WORLDLAYOUT9 = "Sprinkling sand...";

			// Token: 0x0400864B RID: 34379
			public static LocString WORLDLAYOUT10 = "Sprinkling sand...";

			// Token: 0x0400864C RID: 34380
			public static LocString COMPLETELAYOUT = "Cooling glass...";

			// Token: 0x0400864D RID: 34381
			public static LocString COMPLETELAYOUT1 = "Cooling glass...";

			// Token: 0x0400864E RID: 34382
			public static LocString COMPLETELAYOUT2 = "Cooling glass...";

			// Token: 0x0400864F RID: 34383
			public static LocString COMPLETELAYOUT3 = "Cooling glass...";

			// Token: 0x04008650 RID: 34384
			public static LocString COMPLETELAYOUT4 = "Digging holes...";

			// Token: 0x04008651 RID: 34385
			public static LocString COMPLETELAYOUT5 = "Digging holes...";

			// Token: 0x04008652 RID: 34386
			public static LocString COMPLETELAYOUT6 = "Digging holes...";

			// Token: 0x04008653 RID: 34387
			public static LocString COMPLETELAYOUT7 = "Adding buckets of dirt...";

			// Token: 0x04008654 RID: 34388
			public static LocString COMPLETELAYOUT8 = "Adding buckets of dirt...";

			// Token: 0x04008655 RID: 34389
			public static LocString COMPLETELAYOUT9 = "Adding buckets of dirt...";

			// Token: 0x04008656 RID: 34390
			public static LocString COMPLETELAYOUT10 = "Adding buckets of dirt...";

			// Token: 0x04008657 RID: 34391
			public static LocString PROCESSRIVERS = "Pouring rivers...";

			// Token: 0x04008658 RID: 34392
			public static LocString CONVERTTERRAINCELLSTOEDGES = "Hardening diamonds...";

			// Token: 0x04008659 RID: 34393
			public static LocString PROCESSING = "Embedding metals...";

			// Token: 0x0400865A RID: 34394
			public static LocString PROCESSING1 = "Embedding metals...";

			// Token: 0x0400865B RID: 34395
			public static LocString PROCESSING2 = "Embedding metals...";

			// Token: 0x0400865C RID: 34396
			public static LocString PROCESSING3 = "Burying precious ore...";

			// Token: 0x0400865D RID: 34397
			public static LocString PROCESSING4 = "Burying precious ore...";

			// Token: 0x0400865E RID: 34398
			public static LocString PROCESSING5 = "Burying precious ore...";

			// Token: 0x0400865F RID: 34399
			public static LocString PROCESSING6 = "Burying precious ore...";

			// Token: 0x04008660 RID: 34400
			public static LocString PROCESSING7 = "Excavating tunnels...";

			// Token: 0x04008661 RID: 34401
			public static LocString PROCESSING8 = "Excavating tunnels...";

			// Token: 0x04008662 RID: 34402
			public static LocString PROCESSING9 = "Excavating tunnels...";

			// Token: 0x04008663 RID: 34403
			public static LocString BORDERS = "Just adding water...";

			// Token: 0x04008664 RID: 34404
			public static LocString BORDERS1 = "Just adding water...";

			// Token: 0x04008665 RID: 34405
			public static LocString BORDERS2 = "Staring at the void...";

			// Token: 0x04008666 RID: 34406
			public static LocString BORDERS3 = "Staring at the void...";

			// Token: 0x04008667 RID: 34407
			public static LocString BORDERS4 = "Staring at the void...";

			// Token: 0x04008668 RID: 34408
			public static LocString BORDERS5 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04008669 RID: 34409
			public static LocString BORDERS6 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400866A RID: 34410
			public static LocString BORDERS7 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400866B RID: 34411
			public static LocString BORDERS8 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400866C RID: 34412
			public static LocString BORDERS9 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400866D RID: 34413
			public static LocString DRAWWORLDBORDER = "Establishing personal boundaries...";

			// Token: 0x0400866E RID: 34414
			public static LocString PLACINGTEMPLATES = "Generating interest...";

			// Token: 0x0400866F RID: 34415
			public static LocString SETTLESIM = "Infusing oxygen...";

			// Token: 0x04008670 RID: 34416
			public static LocString SETTLESIM1 = "Infusing oxygen...";

			// Token: 0x04008671 RID: 34417
			public static LocString SETTLESIM2 = "Too much oxygen. Removing...";

			// Token: 0x04008672 RID: 34418
			public static LocString SETTLESIM3 = "Too much oxygen. Removing...";

			// Token: 0x04008673 RID: 34419
			public static LocString SETTLESIM4 = "Ideal oxygen levels achieved...";

			// Token: 0x04008674 RID: 34420
			public static LocString SETTLESIM5 = "Ideal oxygen levels achieved...";

			// Token: 0x04008675 RID: 34421
			public static LocString SETTLESIM6 = "Planting space flora...";

			// Token: 0x04008676 RID: 34422
			public static LocString SETTLESIM7 = "Planting space flora...";

			// Token: 0x04008677 RID: 34423
			public static LocString SETTLESIM8 = "Releasing wildlife...";

			// Token: 0x04008678 RID: 34424
			public static LocString SETTLESIM9 = "Releasing wildlife...";

			// Token: 0x04008679 RID: 34425
			public static LocString ANALYZINGWORLD = "Shuffling DNA Blueprints...";

			// Token: 0x0400867A RID: 34426
			public static LocString ANALYZINGWORLDCOMPLETE = "Tidying up for the Duplicants...";

			// Token: 0x0400867B RID: 34427
			public static LocString PLACINGCREATURES = "Building the suspense...";
		}

		// Token: 0x02001DAB RID: 7595
		public class TOOLTIPS
		{
			// Token: 0x0400867C RID: 34428
			public static LocString MANAGEMENTMENU_JOBS = string.Concat(new string[]
			{
				"Manage my Duplicant Priorities {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize)
			});

			// Token: 0x0400867D RID: 34429
			public static LocString MANAGEMENTMENU_CONSUMABLES = "Manage my Duplicants' diets and medications {Hotkey}";

			// Token: 0x0400867E RID: 34430
			public static LocString MANAGEMENTMENU_VITALS = "View my Duplicants' vitals {Hotkey}";

			// Token: 0x0400867F RID: 34431
			public static LocString MANAGEMENTMENU_RESEARCH = "View the Research Tree {Hotkey}";

			// Token: 0x04008680 RID: 34432
			public static LocString MANAGEMENTMENU_REQUIRES_RESEARCH = string.Concat(new string[]
			{
				"Build a Research Station to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x04008681 RID: 34433
			public static LocString MANAGEMENTMENU_DAILYREPORT = "View each cycle's Colony Report {Hotkey}";

			// Token: 0x04008682 RID: 34434
			public static LocString MANAGEMENTMENU_CODEX = "Browse entries in my Database {Hotkey}";

			// Token: 0x04008683 RID: 34435
			public static LocString MANAGEMENTMENU_SCHEDULE = "Adjust the colony's time usage {Hotkey}";

			// Token: 0x04008684 RID: 34436
			public static LocString MANAGEMENTMENU_STARMAP = "Manage astronaut rocket missions {Hotkey}";

			// Token: 0x04008685 RID: 34437
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x04008686 RID: 34438
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE_CLUSTER = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Rocketry Tab", global::Action.Plan14),
				" of the Build Menu"
			});

			// Token: 0x04008687 RID: 34439
			public static LocString MANAGEMENTMENU_SKILLS = "Manage Duplicants' Skill assignments {Hotkey}";

			// Token: 0x04008688 RID: 34440
			public static LocString MANAGEMENTMENU_REQUIRES_SKILL_STATION = string.Concat(new string[]
			{
				"Build a Printing Pod to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x04008689 RID: 34441
			public static LocString MANAGEMENTMENU_PAUSEMENU = "Open the game menu {Hotkey}";

			// Token: 0x0400868A RID: 34442
			public static LocString MANAGEMENTMENU_RESOURCES = "Open the resource management screen {Hotkey}";

			// Token: 0x0400868B RID: 34443
			public static LocString OPEN_CODEX_ENTRY = "View full entry in database";

			// Token: 0x0400868C RID: 34444
			public static LocString NO_CODEX_ENTRY = "No database entry available";

			// Token: 0x0400868D RID: 34445
			public static LocString CHANGE_OUTFIT = "Change this Duplicant's outfit";

			// Token: 0x0400868E RID: 34446
			public static LocString METERSCREEN_AVGSTRESS = "Highest Stress: {0}";

			// Token: 0x0400868F RID: 34447
			public static LocString METERSCREEN_MEALHISTORY = "Calories Available: {0}";

			// Token: 0x04008690 RID: 34448
			public static LocString METERSCREEN_POPULATION = "Population: {0}";

			// Token: 0x04008691 RID: 34449
			public static LocString METERSCREEN_POPULATION_CLUSTER = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " Population: {1}\nTotal Population: {2}";

			// Token: 0x04008692 RID: 34450
			public static LocString METERSCREEN_SICK_DUPES = "Sick Duplicants: {0}";

			// Token: 0x04008693 RID: 34451
			public static LocString METERSCREEN_INVALID_FOOD_TYPE = "Invalid Food Type: {0}";

			// Token: 0x04008694 RID: 34452
			public static LocString PLAYBUTTON = "Start";

			// Token: 0x04008695 RID: 34453
			public static LocString PAUSEBUTTON = "Pause";

			// Token: 0x04008696 RID: 34454
			public static LocString PAUSE = "Pause {Hotkey}";

			// Token: 0x04008697 RID: 34455
			public static LocString UNPAUSE = "Unpause {Hotkey}";

			// Token: 0x04008698 RID: 34456
			public static LocString SPEEDBUTTON_SLOW = "Slow speed {Hotkey}";

			// Token: 0x04008699 RID: 34457
			public static LocString SPEEDBUTTON_MEDIUM = "Medium speed {Hotkey}";

			// Token: 0x0400869A RID: 34458
			public static LocString SPEEDBUTTON_FAST = "Fast speed {Hotkey}";

			// Token: 0x0400869B RID: 34459
			public static LocString RED_ALERT_TITLE = "Toggle Red Alert";

			// Token: 0x0400869C RID: 34460
			public static LocString RED_ALERT_CONTENT = "Duplicants will work, ignoring schedules and their basic needs\n\nUse in case of emergency";

			// Token: 0x0400869D RID: 34461
			public static LocString DISINFECTBUTTON = "Disinfect buildings {Hotkey}";

			// Token: 0x0400869E RID: 34462
			public static LocString MOPBUTTON = "Mop liquid spills {Hotkey}";

			// Token: 0x0400869F RID: 34463
			public static LocString DIGBUTTON = "Set dig errands {Hotkey}";

			// Token: 0x040086A0 RID: 34464
			public static LocString CANCELBUTTON = "Cancel errands {Hotkey}";

			// Token: 0x040086A1 RID: 34465
			public static LocString DECONSTRUCTBUTTON = "Demolish buildings {Hotkey}";

			// Token: 0x040086A2 RID: 34466
			public static LocString ATTACKBUTTON = "Attack poor, wild critters {Hotkey}";

			// Token: 0x040086A3 RID: 34467
			public static LocString CAPTUREBUTTON = "Capture critters {Hotkey}";

			// Token: 0x040086A4 RID: 34468
			public static LocString CLEARBUTTON = "Move debris into storage {Hotkey}";

			// Token: 0x040086A5 RID: 34469
			public static LocString HARVESTBUTTON = "Harvest plants {Hotkey}";

			// Token: 0x040086A6 RID: 34470
			public static LocString PRIORITIZEMAINBUTTON = "";

			// Token: 0x040086A7 RID: 34471
			public static LocString PRIORITIZEBUTTON = string.Concat(new string[]
			{
				"Set Building Priority {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x040086A8 RID: 34472
			public static LocString CLEANUPMAINBUTTON = "Mop and sweep messy floors {Hotkey}";

			// Token: 0x040086A9 RID: 34473
			public static LocString CANCELDECONSTRUCTIONBUTTON = "Cancel queued orders or deconstruct existing buildings {Hotkey}";

			// Token: 0x040086AA RID: 34474
			public static LocString HELP_ROTATE_KEY = "Press " + UI.FormatAsHotKey(global::Action.RotateBuilding) + " to Rotate";

			// Token: 0x040086AB RID: 34475
			public static LocString HELP_BUILDLOCATION_INVALID_CELL = "Invalid Cell";

			// Token: 0x040086AC RID: 34476
			public static LocString HELP_BUILDLOCATION_MISSING_TELEPAD = "World has no " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " or " + BUILDINGS.PREFABS.EXOBASEHEADQUARTERS.NAME;

			// Token: 0x040086AD RID: 34477
			public static LocString HELP_BUILDLOCATION_FLOOR = "Must be built on solid ground";

			// Token: 0x040086AE RID: 34478
			public static LocString HELP_BUILDLOCATION_WALL = "Must be built against a wall";

			// Token: 0x040086AF RID: 34479
			public static LocString HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT = "Must be built on solid ground or overlapping an {0}";

			// Token: 0x040086B0 RID: 34480
			public static LocString HELP_BUILDLOCATION_OCCUPIED = "Must be built in unoccupied space";

			// Token: 0x040086B1 RID: 34481
			public static LocString HELP_BUILDLOCATION_CEILING = "Must be built on the ceiling";

			// Token: 0x040086B2 RID: 34482
			public static LocString HELP_BUILDLOCATION_INSIDEGROUND = "Must be built in the ground";

			// Token: 0x040086B3 RID: 34483
			public static LocString HELP_BUILDLOCATION_ATTACHPOINT = "Must be built overlapping a {0}";

			// Token: 0x040086B4 RID: 34484
			public static LocString HELP_BUILDLOCATION_SPACE = "Must be built on the surface in space";

			// Token: 0x040086B5 RID: 34485
			public static LocString HELP_BUILDLOCATION_CORNER = "Must be built in a corner";

			// Token: 0x040086B6 RID: 34486
			public static LocString HELP_BUILDLOCATION_CORNER_FLOOR = "Must be built in a corner on the ground";

			// Token: 0x040086B7 RID: 34487
			public static LocString HELP_BUILDLOCATION_BELOWROCKETCEILING = "Must be placed further from the edge of space";

			// Token: 0x040086B8 RID: 34488
			public static LocString HELP_BUILDLOCATION_ONROCKETENVELOPE = "Must be built on the interior wall of a rocket";

			// Token: 0x040086B9 RID: 34489
			public static LocString HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN = "Obstructed by a building";

			// Token: 0x040086BA RID: 34490
			public static LocString HELP_BUILDLOCATION_NOT_IN_TILES = "Cannot be built inside tile";

			// Token: 0x040086BB RID: 34491
			public static LocString HELP_BUILDLOCATION_GASPORTS_OVERLAP = "Gas ports cannot overlap";

			// Token: 0x040086BC RID: 34492
			public static LocString HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP = "Liquid ports cannot overlap";

			// Token: 0x040086BD RID: 34493
			public static LocString HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP = "Solid ports cannot overlap";

			// Token: 0x040086BE RID: 34494
			public static LocString HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED = "Automation ports cannot overlap";

			// Token: 0x040086BF RID: 34495
			public static LocString HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP = "Power connectors cannot overlap";

			// Token: 0x040086C0 RID: 34496
			public static LocString HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE = "Heavi-Watt connectors cannot be built inside tile";

			// Token: 0x040086C1 RID: 34497
			public static LocString HELP_BUILDLOCATION_WIRE_OBSTRUCTION = "Obstructed by Heavi-Watt Wire";

			// Token: 0x040086C2 RID: 34498
			public static LocString HELP_BUILDLOCATION_BACK_WALL = "Obstructed by back wall";

			// Token: 0x040086C3 RID: 34499
			public static LocString HELP_TUBELOCATION_NO_UTURNS = "Can't U-Turn";

			// Token: 0x040086C4 RID: 34500
			public static LocString HELP_TUBELOCATION_STRAIGHT_BRIDGES = "Can't Turn Here";

			// Token: 0x040086C5 RID: 34501
			public static LocString HELP_REQUIRES_ROOM = "Must be in a " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD;

			// Token: 0x040086C6 RID: 34502
			public static LocString OXYGENOVERLAYSTRING = "Displays ambient oxygen density {Hotkey}";

			// Token: 0x040086C7 RID: 34503
			public static LocString POWEROVERLAYSTRING = "Displays power grid components {Hotkey}";

			// Token: 0x040086C8 RID: 34504
			public static LocString TEMPERATUREOVERLAYSTRING = "Displays ambient temperature {Hotkey}";

			// Token: 0x040086C9 RID: 34505
			public static LocString HEATFLOWOVERLAYSTRING = "Displays comfortable temperatures for Duplicants {Hotkey}";

			// Token: 0x040086CA RID: 34506
			public static LocString SUITOVERLAYSTRING = "Displays Exosuits and related buildings {Hotkey}";

			// Token: 0x040086CB RID: 34507
			public static LocString LOGICOVERLAYSTRING = "Displays automation grid components {Hotkey}";

			// Token: 0x040086CC RID: 34508
			public static LocString ROOMSOVERLAYSTRING = "Displays special purpose rooms and bonuses {Hotkey}";

			// Token: 0x040086CD RID: 34509
			public static LocString JOULESOVERLAYSTRING = "Displays the thermal energy in each cell";

			// Token: 0x040086CE RID: 34510
			public static LocString LIGHTSOVERLAYSTRING = "Displays the visibility radius of light sources {Hotkey}";

			// Token: 0x040086CF RID: 34511
			public static LocString LIQUIDVENTOVERLAYSTRING = "Displays liquid pipe system components {Hotkey}";

			// Token: 0x040086D0 RID: 34512
			public static LocString GASVENTOVERLAYSTRING = "Displays gas pipe system components {Hotkey}";

			// Token: 0x040086D1 RID: 34513
			public static LocString DECOROVERLAYSTRING = "Displays areas with Morale-boosting decor values {Hotkey}";

			// Token: 0x040086D2 RID: 34514
			public static LocString PRIORITIESOVERLAYSTRING = "Displays work priority values {Hotkey}";

			// Token: 0x040086D3 RID: 34515
			public static LocString DISEASEOVERLAYSTRING = "Displays areas of disease risk {Hotkey}";

			// Token: 0x040086D4 RID: 34516
			public static LocString NOISE_POLLUTION_OVERLAY_STRING = "Displays ambient noise levels {Hotkey}";

			// Token: 0x040086D5 RID: 34517
			public static LocString CROPS_OVERLAY_STRING = "Displays plant growth progress {Hotkey}";

			// Token: 0x040086D6 RID: 34518
			public static LocString CONVEYOR_OVERLAY_STRING = "Displays conveyor transport components {Hotkey}";

			// Token: 0x040086D7 RID: 34519
			public static LocString TILEMODE_OVERLAY_STRING = "Displays material information {Hotkey}";

			// Token: 0x040086D8 RID: 34520
			public static LocString REACHABILITYOVERLAYSTRING = "Displays areas accessible by Duplicants";

			// Token: 0x040086D9 RID: 34521
			public static LocString RADIATIONOVERLAYSTRING = "Displays radiation levels {Hotkey}";

			// Token: 0x040086DA RID: 34522
			public static LocString ENERGYREQUIRED = UI.FormatAsLink("Power", "POWER") + " Required";

			// Token: 0x040086DB RID: 34523
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + " Produced";

			// Token: 0x040086DC RID: 34524
			public static LocString INFOPANEL = "The Info Panel contains an overview of the basic information about my Duplicant";

			// Token: 0x040086DD RID: 34525
			public static LocString VITALSPANEL = "The Vitals Panel monitors the status and well being of my Duplicant";

			// Token: 0x040086DE RID: 34526
			public static LocString STRESSPANEL = "The Stress Panel offers a detailed look at what is affecting my Duplicant psychologically";

			// Token: 0x040086DF RID: 34527
			public static LocString STATSPANEL = "The Stats Panel gives me an overview of my Duplicant's individual stats";

			// Token: 0x040086E0 RID: 34528
			public static LocString ITEMSPANEL = "The Items Panel displays everything this Duplicant is in possession of";

			// Token: 0x040086E1 RID: 34529
			public static LocString STRESSDESCRIPTION = string.Concat(new string[]
			{
				"Accommodate my Duplicant's needs to manage their ",
				UI.FormatAsLink("Stress", "STRESS"),
				".\n\nLow ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can provide a productivity boost, while high ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can impair production or even lead to a nervous breakdown."
			});

			// Token: 0x040086E2 RID: 34530
			public static LocString ALERTSTOOLTIP = "Alerts provide important information about what's happening in the colony right now";

			// Token: 0x040086E3 RID: 34531
			public static LocString MESSAGESTOOLTIP = "Messages are events that have happened and tips to help me manage my colony";

			// Token: 0x040086E4 RID: 34532
			public static LocString NEXTMESSAGESTOOLTIP = "Next message";

			// Token: 0x040086E5 RID: 34533
			public static LocString CLOSETOOLTIP = "Close";

			// Token: 0x040086E6 RID: 34534
			public static LocString DISMISSMESSAGE = "Dismiss message";

			// Token: 0x040086E7 RID: 34535
			public static LocString RECIPE_QUEUE = "Queue {0} for continuous fabrication";

			// Token: 0x040086E8 RID: 34536
			public static LocString RED_ALERT_BUTTON_ON = "Enable Red Alert";

			// Token: 0x040086E9 RID: 34537
			public static LocString RED_ALERT_BUTTON_OFF = "Disable Red Alert";

			// Token: 0x040086EA RID: 34538
			public static LocString JOBSSCREEN_PRIORITY = "High priority tasks are always performed before low priority tasks.\n\nHowever, a busy Duplicant will continue to work on their current work errand until it's complete, even if a more important errand becomes available.";

			// Token: 0x040086EB RID: 34539
			public static LocString JOBSSCREEN_ATTRIBUTES = "The following attributes affect a Duplicant's efficiency at this errand:";

			// Token: 0x040086EC RID: 34540
			public static LocString JOBSSCREEN_CANNOTPERFORMTASK = "{0} cannot perform this errand.";

			// Token: 0x040086ED RID: 34541
			public static LocString JOBSSCREEN_RELEVANT_ATTRIBUTES = "Relevant Attributes:";

			// Token: 0x040086EE RID: 34542
			public static LocString SORTCOLUMN = UI.CLICK(UI.ClickType.Click) + " to sort";

			// Token: 0x040086EF RID: 34543
			public static LocString NOMATERIAL = "Not enough materials";

			// Token: 0x040086F0 RID: 34544
			public static LocString SELECTAMATERIAL = "There are insufficient materials to construct this building";

			// Token: 0x040086F1 RID: 34545
			public static LocString EDITNAME = "Give this Duplicant a new name";

			// Token: 0x040086F2 RID: 34546
			public static LocString RANDOMIZENAME = "Randomize this Duplicant's name";

			// Token: 0x040086F3 RID: 34547
			public static LocString EDITNAMEGENERIC = "Rename {0}";

			// Token: 0x040086F4 RID: 34548
			public static LocString EDITNAMEROCKET = "Rename this rocket";

			// Token: 0x040086F5 RID: 34549
			public static LocString BASE_VALUE = "Base Value";

			// Token: 0x040086F6 RID: 34550
			public static LocString MATIERIAL_MOD = "Made out of {0}";

			// Token: 0x040086F7 RID: 34551
			public static LocString VITALS_CHECKBOX_TEMPERATURE = string.Concat(new string[]
			{
				"This plant's internal ",
				UI.PRE_KEYWORD,
				"Temperature",
				UI.PST_KEYWORD,
				" is <b>{temperature}</b>"
			});

			// Token: 0x040086F8 RID: 34552
			public static LocString VITALS_CHECKBOX_PRESSURE = string.Concat(new string[]
			{
				"The current ",
				UI.PRE_KEYWORD,
				"Gas",
				UI.PST_KEYWORD,
				" pressure is <b>{pressure}</b>"
			});

			// Token: 0x040086F9 RID: 34553
			public static LocString VITALS_CHECKBOX_ATMOSPHERE = "This plant is immersed in {element}";

			// Token: 0x040086FA RID: 34554
			public static LocString VITALS_CHECKBOX_ILLUMINATION_DARK = "This plant is currently in the dark";

			// Token: 0x040086FB RID: 34555
			public static LocString VITALS_CHECKBOX_ILLUMINATION_LIGHT = "This plant is currently lit";

			// Token: 0x040086FC RID: 34556
			public static LocString VITALS_CHECKBOX_FERTILIZER = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Fertilizer",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x040086FD RID: 34557
			public static LocString VITALS_CHECKBOX_IRRIGATION = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Liquid",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x040086FE RID: 34558
			public static LocString VITALS_CHECKBOX_SUBMERGED_TRUE = "This plant is fully submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PRE_KEYWORD;

			// Token: 0x040086FF RID: 34559
			public static LocString VITALS_CHECKBOX_SUBMERGED_FALSE = "This plant must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x04008700 RID: 34560
			public static LocString VITALS_CHECKBOX_DROWNING_TRUE = "This plant is not drowning";

			// Token: 0x04008701 RID: 34561
			public static LocString VITALS_CHECKBOX_DROWNING_FALSE = "This plant is drowning in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x04008702 RID: 34562
			public static LocString VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL = "This plant is housed in an operational farm plot";

			// Token: 0x04008703 RID: 34563
			public static LocString VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL = "This plant is not housed in an operational farm plot";

			// Token: 0x04008704 RID: 34564
			public static LocString VITALS_CHECKBOX_RADIATION = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs at between {minRads} and {maxRads} to grow"
			});

			// Token: 0x04008705 RID: 34565
			public static LocString VITALS_CHECKBOX_RADIATION_NO_MIN = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs less than {maxRads} to grow"
			});
		}

		// Token: 0x02001DAC RID: 7596
		public class CLUSTERMAP
		{
			// Token: 0x04008706 RID: 34566
			public static LocString PLANETOID = "Planetoid";

			// Token: 0x04008707 RID: 34567
			public static LocString PLANETOID_KEYWORD = UI.PRE_KEYWORD + UI.CLUSTERMAP.PLANETOID + UI.PST_KEYWORD;

			// Token: 0x04008708 RID: 34568
			public static LocString TITLE = "STARMAP";

			// Token: 0x04008709 RID: 34569
			public static LocString LANDING_SITES = "LANDING SITES";

			// Token: 0x0400870A RID: 34570
			public static LocString DESTINATION = "DESTINATION";

			// Token: 0x0400870B RID: 34571
			public static LocString OCCUPANTS = "CREW";

			// Token: 0x0400870C RID: 34572
			public static LocString ELEMENTS = "ELEMENTS";

			// Token: 0x0400870D RID: 34573
			public static LocString UNKNOWN_DESTINATION = "Unknown";

			// Token: 0x0400870E RID: 34574
			public static LocString TILES = "Tiles";

			// Token: 0x0400870F RID: 34575
			public static LocString TILES_PER_CYCLE = "Tiles per cycle";

			// Token: 0x04008710 RID: 34576
			public static LocString CHANGE_DESTINATION = UI.CLICK(UI.ClickType.Click) + " to change destination";

			// Token: 0x04008711 RID: 34577
			public static LocString SELECT_DESTINATION = "Select a new destination on the map";

			// Token: 0x04008712 RID: 34578
			public static LocString TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR = "Rockets cannot travel to this hex until it has been analyzed\n\nSpace can be analyzed with a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " or " + BUILDINGS.PREFABS.SCANNERMODULE.NAME;

			// Token: 0x04008713 RID: 34579
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_PATH = string.Concat(new string[]
			{
				"There is no navigable rocket path to this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				"\n\nSpace can be analyzed with a ",
				BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCANNERMODULE.NAME,
				" to clear the way"
			});

			// Token: 0x04008714 RID: 34580
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD = string.Concat(new string[]
			{
				"There is no ",
				BUILDINGS.PREFABS.LAUNCHPAD.NAME,
				" on this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				" for a rocket to land on\n\nUse a ",
				BUILDINGS.PREFABS.PIONEERMODULE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCOUTMODULE.NAME,
				" to deploy a scout and make first contact"
			});

			// Token: 0x04008715 RID: 34581
			public static LocString TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID = "Must select a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " destination";

			// Token: 0x04008716 RID: 34582
			public static LocString TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE = "This destination is further away than the rocket's maximum range of {0}.";

			// Token: 0x04008717 RID: 34583
			public static LocString TOOLTIP_HIDDEN_HEX = "???";

			// Token: 0x04008718 RID: 34584
			public static LocString TOOLTIP_PEEKED_HEX_WITH_OBJECT = "UNKNOWN OBJECT DETECTED!";

			// Token: 0x04008719 RID: 34585
			public static LocString TOOLTIP_EMPTY_HEX = "EMPTY SPACE";

			// Token: 0x0400871A RID: 34586
			public static LocString TOOLTIP_PATH_LENGTH = "Trip Distance: {0}/{1}";

			// Token: 0x0400871B RID: 34587
			public static LocString TOOLTIP_PATH_LENGTH_RETURN = "Trip Distance: {0}/{1} (Return Trip)";

			// Token: 0x020025DE RID: 9694
			public class STATUS
			{
				// Token: 0x0400A615 RID: 42517
				public static LocString NORMAL = "Normal";

				// Token: 0x020030D7 RID: 12503
				public class ROCKET
				{
					// Token: 0x0400C47F RID: 50303
					public static LocString GROUNDED = "Normal";

					// Token: 0x0400C480 RID: 50304
					public static LocString TRAVELING = "Traveling";

					// Token: 0x0400C481 RID: 50305
					public static LocString STRANDED = "Stranded";

					// Token: 0x0400C482 RID: 50306
					public static LocString IDLE = "Idle";
				}
			}

			// Token: 0x020025DF RID: 9695
			public class ASTEROIDS
			{
				// Token: 0x020030D8 RID: 12504
				public class ELEMENT_AMOUNTS
				{
					// Token: 0x0400C483 RID: 50307
					public static LocString LOTS = "Plentiful";

					// Token: 0x0400C484 RID: 50308
					public static LocString SOME = "Significant amount";

					// Token: 0x0400C485 RID: 50309
					public static LocString LITTLE = "Small amount";

					// Token: 0x0400C486 RID: 50310
					public static LocString VERY_LITTLE = "Trace amount";
				}

				// Token: 0x020030D9 RID: 12505
				public class SURFACE_CONDITIONS
				{
					// Token: 0x0400C487 RID: 50311
					public static LocString LIGHT = "Peak Light";

					// Token: 0x0400C488 RID: 50312
					public static LocString RADIATION = "Cosmic Radiation";
				}
			}

			// Token: 0x020025E0 RID: 9696
			public class POI
			{
				// Token: 0x0400A616 RID: 42518
				public static LocString TITLE = "POINT OF INTEREST";

				// Token: 0x0400A617 RID: 42519
				public static LocString MASS_REMAINING = "<b>Total Mass Remaining</b>";

				// Token: 0x0400A618 RID: 42520
				public static LocString ROCKETS_AT_THIS_LOCATION = "<b>Rockets at this location</b>";

				// Token: 0x0400A619 RID: 42521
				public static LocString ARTIFACTS = "Artifact";

				// Token: 0x0400A61A RID: 42522
				public static LocString ARTIFACTS_AVAILABLE = "Available";

				// Token: 0x0400A61B RID: 42523
				public static LocString ARTIFACTS_DEPLETED = "Collected\nRecharge: {0}";
			}

			// Token: 0x020025E1 RID: 9697
			public class ROCKETS
			{
				// Token: 0x020030DA RID: 12506
				public class SPEED
				{
					// Token: 0x0400C489 RID: 50313
					public static LocString NAME = "Rocket Speed: ";

					// Token: 0x0400C48A RID: 50314
					public static LocString TOOLTIP = "<b>Rocket Speed</b> is calculated by dividing <b>Engine Power</b> by <b>Burden</b>.\n\nRockets operating on autopilot will have a reduced speed.\n\nRocket speed can be further increased by the skill of the Duplicant flying the rocket.";
				}

				// Token: 0x020030DB RID: 12507
				public class FUEL_REMAINING
				{
					// Token: 0x0400C48B RID: 50315
					public static LocString NAME = "Fuel Remaining: ";

					// Token: 0x0400C48C RID: 50316
					public static LocString TOOLTIP = "This rocket has {0} fuel in its tank";
				}

				// Token: 0x020030DC RID: 12508
				public class OXIDIZER_REMAINING
				{
					// Token: 0x0400C48D RID: 50317
					public static LocString NAME = "Oxidizer Power Remaining: ";

					// Token: 0x0400C48E RID: 50318
					public static LocString TOOLTIP = "This rocket has enough oxidizer in its tank for {0} of fuel";
				}

				// Token: 0x020030DD RID: 12509
				public class RANGE
				{
					// Token: 0x0400C48F RID: 50319
					public static LocString NAME = "Range Remaining: ";

					// Token: 0x0400C490 RID: 50320
					public static LocString TOOLTIP = "<b>Range remaining</b> is calculated by dividing the lesser of <b>fuel remaining</b> and <b>oxidizer power remaining</b> by <b>fuel consumed per tile</b>";
				}

				// Token: 0x020030DE RID: 12510
				public class FUEL_PER_HEX
				{
					// Token: 0x0400C491 RID: 50321
					public static LocString NAME = "Fuel consumed per Tile: {0}";

					// Token: 0x0400C492 RID: 50322
					public static LocString TOOLTIP = "This rocket can travel one tile per {0} of fuel";
				}

				// Token: 0x020030DF RID: 12511
				public class BURDEN_TOTAL
				{
					// Token: 0x0400C493 RID: 50323
					public static LocString NAME = "Rocket burden: ";

					// Token: 0x0400C494 RID: 50324
					public static LocString TOOLTIP = "The combined burden of all the modules in this rocket";
				}

				// Token: 0x020030E0 RID: 12512
				public class BURDEN_MODULE
				{
					// Token: 0x0400C495 RID: 50325
					public static LocString NAME = "Module Burden: ";

					// Token: 0x0400C496 RID: 50326
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME;
				}

				// Token: 0x020030E1 RID: 12513
				public class POWER_TOTAL
				{
					// Token: 0x0400C497 RID: 50327
					public static LocString NAME = "Rocket engine power: ";

					// Token: 0x0400C498 RID: 50328
					public static LocString TOOLTIP = "The total engine power added by all the modules in this rocket";
				}

				// Token: 0x020030E2 RID: 12514
				public class POWER_MODULE
				{
					// Token: 0x0400C499 RID: 50329
					public static LocString NAME = "Module Engine Power: ";

					// Token: 0x0400C49A RID: 50330
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME;
				}

				// Token: 0x020030E3 RID: 12515
				public class MODULE_STATS
				{
					// Token: 0x0400C49B RID: 50331
					public static LocString NAME = "Module Stats: ";

					// Token: 0x0400C49C RID: 50332
					public static LocString NAME_HEADER = "Module Stats";

					// Token: 0x0400C49D RID: 50333
					public static LocString TOOLTIP = "Properties of the selected module";
				}

				// Token: 0x020030E4 RID: 12516
				public class MAX_MODULES
				{
					// Token: 0x0400C49E RID: 50334
					public static LocString NAME = "Max Modules: ";

					// Token: 0x0400C49F RID: 50335
					public static LocString TOOLTIP = "The {0} can support {1} rocket modules, plus itself";
				}

				// Token: 0x020030E5 RID: 12517
				public class MAX_HEIGHT
				{
					// Token: 0x0400C4A0 RID: 50336
					public static LocString NAME = "Height: {0}/{1}";

					// Token: 0x0400C4A1 RID: 50337
					public static LocString NAME_RAW = "Height: ";

					// Token: 0x0400C4A2 RID: 50338
					public static LocString NAME_MAX_SUPPORTED = "Maximum supported rocket height: ";

					// Token: 0x0400C4A3 RID: 50339
					public static LocString TOOLTIP = "The {0} can support a total rocket height {1}";
				}

				// Token: 0x020030E6 RID: 12518
				public class ARTIFACT_MODULE
				{
					// Token: 0x0400C4A4 RID: 50340
					public static LocString EMPTY = "Empty";
				}
			}
		}

		// Token: 0x02001DAD RID: 7597
		public class STARMAP
		{
			// Token: 0x0400871C RID: 34588
			public static LocString TITLE = "STARMAP";

			// Token: 0x0400871D RID: 34589
			public static LocString MANAGEMENT_BUTTON = "STARMAP";

			// Token: 0x0400871E RID: 34590
			public static LocString SUBROW = "•  {0}";

			// Token: 0x0400871F RID: 34591
			public static LocString UNKNOWN_DESTINATION = "Destination Unknown";

			// Token: 0x04008720 RID: 34592
			public static LocString ANALYSIS_AMOUNT = "Analysis {0} Complete";

			// Token: 0x04008721 RID: 34593
			public static LocString ANALYSIS_COMPLETE = "ANALYSIS COMPLETE";

			// Token: 0x04008722 RID: 34594
			public static LocString NO_ANALYZABLE_DESTINATION_SELECTED = "No destination selected";

			// Token: 0x04008723 RID: 34595
			public static LocString UNKNOWN_TYPE = "Type Unknown";

			// Token: 0x04008724 RID: 34596
			public static LocString DISTANCE = "{0} km";

			// Token: 0x04008725 RID: 34597
			public static LocString MODULE_MASS = "+ {0} t";

			// Token: 0x04008726 RID: 34598
			public static LocString MODULE_STORAGE = "{0} / {1}";

			// Token: 0x04008727 RID: 34599
			public static LocString ANALYSIS_DESCRIPTION = "Use a Telescope to analyze space destinations.\n\nCompleting analysis on an object will unlock rocket missions to that destination.";

			// Token: 0x04008728 RID: 34600
			public static LocString RESEARCH_DESCRIPTION = "Gather Interstellar Research Data using Research Modules.";

			// Token: 0x04008729 RID: 34601
			public static LocString ROCKET_RENAME_BUTTON_TOOLTIP = "Rename this rocket";

			// Token: 0x0400872A RID: 34602
			public static LocString NO_ROCKETS_HELP_TEXT = "Rockets allow you to visit nearby celestial bodies.\n\nEach rocket must have a Command Module, an Engine, and Fuel.\n\nYou can also carry other modules that allow you to gather specific resources from the places you visit.\n\nRemember the more weight a rocket has, the more limited it'll be on the distance it can travel. You can add more fuel to fix that, but fuel will add weight as well.";

			// Token: 0x0400872B RID: 34603
			public static LocString CONTAINER_REQUIRED = "{0} installation required to retrieve material";

			// Token: 0x0400872C RID: 34604
			public static LocString CAN_CARRY_ELEMENT = "Gathered by: {1}";

			// Token: 0x0400872D RID: 34605
			public static LocString CANT_CARRY_ELEMENT = "{0} installation required to retrieve material";

			// Token: 0x0400872E RID: 34606
			public static LocString STATUS = "SELECTED";

			// Token: 0x0400872F RID: 34607
			public static LocString DISTANCE_OVERLAY = "TOO FAR FOR THIS ROCKET";

			// Token: 0x04008730 RID: 34608
			public static LocString COMPOSITION_UNDISCOVERED = "?????????";

			// Token: 0x04008731 RID: 34609
			public static LocString COMPOSITION_UNDISCOVERED_TOOLTIP = "Further research required to identify resource\n\nSend a Research Module to this destination for more information";

			// Token: 0x04008732 RID: 34610
			public static LocString COMPOSITION_UNDISCOVERED_AMOUNT = "???";

			// Token: 0x04008733 RID: 34611
			public static LocString COMPOSITION_SMALL_AMOUNT = "Trace Amount";

			// Token: 0x04008734 RID: 34612
			public static LocString CURRENT_MASS = "Current Mass";

			// Token: 0x04008735 RID: 34613
			public static LocString CURRENT_MASS_TOOLTIP = "Warning: Missions to this destination will not return a full cargo load to avoid depleting the destination for future explorations\n\nDestination: {0} Resources Available\nRocket Capacity: {1}";

			// Token: 0x04008736 RID: 34614
			public static LocString MAXIMUM_MASS = "Maximum Mass";

			// Token: 0x04008737 RID: 34615
			public static LocString MINIMUM_MASS = "Minimum Mass";

			// Token: 0x04008738 RID: 34616
			public static LocString MINIMUM_MASS_TOOLTIP = "This destination must retain at least this much mass in order to prevent depletion and allow the future regeneration of resources.\n\nDuplicants will always maintain a destination's minimum mass requirements, potentially returning with less cargo than their rocket can hold";

			// Token: 0x04008739 RID: 34617
			public static LocString REPLENISH_RATE = "Replenished/Cycle:";

			// Token: 0x0400873A RID: 34618
			public static LocString REPLENISH_RATE_TOOLTIP = "The rate at which this destination regenerates resources";

			// Token: 0x0400873B RID: 34619
			public static LocString ROCKETLIST = "Rocket Hangar";

			// Token: 0x0400873C RID: 34620
			public static LocString NO_ROCKETS_TITLE = "NO ROCKETS";

			// Token: 0x0400873D RID: 34621
			public static LocString ROCKET_COUNT = "ROCKETS: {0}";

			// Token: 0x0400873E RID: 34622
			public static LocString LAUNCH_MISSION = "LAUNCH MISSION";

			// Token: 0x0400873F RID: 34623
			public static LocString CANT_LAUNCH_MISSION = "CANNOT LAUNCH";

			// Token: 0x04008740 RID: 34624
			public static LocString LAUNCH_ROCKET = "Launch Rocket";

			// Token: 0x04008741 RID: 34625
			public static LocString LAND_ROCKET = "Land Rocket";

			// Token: 0x04008742 RID: 34626
			public static LocString SEE_ROCKETS_LIST = "See Rockets List";

			// Token: 0x04008743 RID: 34627
			public static LocString DEFAULT_NAME = "Rocket";

			// Token: 0x04008744 RID: 34628
			public static LocString ANALYZE_DESTINATION = "ANALYZE OBJECT";

			// Token: 0x04008745 RID: 34629
			public static LocString SUSPEND_DESTINATION_ANALYSIS = "PAUSE ANALYSIS";

			// Token: 0x04008746 RID: 34630
			public static LocString DESTINATIONTITLE = "Destination Status";

			// Token: 0x020025E2 RID: 9698
			public class DESTINATIONSTUDY
			{
				// Token: 0x0400A61C RID: 42524
				public static LocString UPPERATMO = "Study upper atmosphere";

				// Token: 0x0400A61D RID: 42525
				public static LocString LOWERATMO = "Study lower atmosphere";

				// Token: 0x0400A61E RID: 42526
				public static LocString MAGNETICFIELD = "Study magnetic field";

				// Token: 0x0400A61F RID: 42527
				public static LocString SURFACE = "Study surface";

				// Token: 0x0400A620 RID: 42528
				public static LocString SUBSURFACE = "Study subsurface";
			}

			// Token: 0x020025E3 RID: 9699
			public class COMPONENT
			{
				// Token: 0x0400A621 RID: 42529
				public static LocString FUEL_TANK = "Fuel Tank";

				// Token: 0x0400A622 RID: 42530
				public static LocString ROCKET_ENGINE = "Rocket Engine";

				// Token: 0x0400A623 RID: 42531
				public static LocString CARGO_BAY = "Cargo Bay";

				// Token: 0x0400A624 RID: 42532
				public static LocString OXIDIZER_TANK = "Oxidizer Tank";
			}

			// Token: 0x020025E4 RID: 9700
			public class MISSION_STATUS
			{
				// Token: 0x0400A625 RID: 42533
				public static LocString GROUNDED = "Grounded";

				// Token: 0x0400A626 RID: 42534
				public static LocString LAUNCHING = "Launching";

				// Token: 0x0400A627 RID: 42535
				public static LocString WAITING_TO_LAND = "Waiting To Land";

				// Token: 0x0400A628 RID: 42536
				public static LocString LANDING = "Landing";

				// Token: 0x0400A629 RID: 42537
				public static LocString UNDERWAY = "Underway";

				// Token: 0x0400A62A RID: 42538
				public static LocString UNDERWAY_BOOSTED = "Underway <color=#5FDB37FF>(Boosted)</color>";

				// Token: 0x0400A62B RID: 42539
				public static LocString DESTROYED = "Destroyed";

				// Token: 0x0400A62C RID: 42540
				public static LocString GO = "ALL SYSTEMS GO";
			}

			// Token: 0x020025E5 RID: 9701
			public class LISTTITLES
			{
				// Token: 0x0400A62D RID: 42541
				public static LocString MISSIONSTATUS = "Mission Status";

				// Token: 0x0400A62E RID: 42542
				public static LocString LAUNCHCHECKLIST = "Launch Checklist";

				// Token: 0x0400A62F RID: 42543
				public static LocString MAXRANGE = "Max Range";

				// Token: 0x0400A630 RID: 42544
				public static LocString MASS = "Mass";

				// Token: 0x0400A631 RID: 42545
				public static LocString STORAGE = "Storage";

				// Token: 0x0400A632 RID: 42546
				public static LocString FUEL = "Fuel";

				// Token: 0x0400A633 RID: 42547
				public static LocString OXIDIZER = "Oxidizer";

				// Token: 0x0400A634 RID: 42548
				public static LocString PASSENGERS = "Passengers";

				// Token: 0x0400A635 RID: 42549
				public static LocString RESEARCH = "Research";

				// Token: 0x0400A636 RID: 42550
				public static LocString ARTIFACTS = "Artifacts";

				// Token: 0x0400A637 RID: 42551
				public static LocString ANALYSIS = "Analysis";

				// Token: 0x0400A638 RID: 42552
				public static LocString WORLDCOMPOSITION = "World Composition";

				// Token: 0x0400A639 RID: 42553
				public static LocString RESOURCES = "Resources";

				// Token: 0x0400A63A RID: 42554
				public static LocString MODULES = "Modules";

				// Token: 0x0400A63B RID: 42555
				public static LocString TYPE = "Type";

				// Token: 0x0400A63C RID: 42556
				public static LocString DISTANCE = "Distance";

				// Token: 0x0400A63D RID: 42557
				public static LocString DESTINATION_MASS = "World Mass Available";

				// Token: 0x0400A63E RID: 42558
				public static LocString STORAGECAPACITY = "Storage Capacity";
			}

			// Token: 0x020025E6 RID: 9702
			public class ROCKETWEIGHT
			{
				// Token: 0x0400A63F RID: 42559
				public static LocString MASS = "Mass: ";

				// Token: 0x0400A640 RID: 42560
				public static LocString MASSPENALTY = "Mass Penalty: ";

				// Token: 0x0400A641 RID: 42561
				public static LocString CURRENTMASS = "Current Rocket Mass: ";

				// Token: 0x0400A642 RID: 42562
				public static LocString CURRENTMASSPENALTY = "Current Weight Penalty: ";
			}

			// Token: 0x020025E7 RID: 9703
			public class DESTINATIONSELECTION
			{
				// Token: 0x0400A643 RID: 42563
				public static LocString REACHABLE = "Destination set";

				// Token: 0x0400A644 RID: 42564
				public static LocString UNREACHABLE = "Destination set";

				// Token: 0x0400A645 RID: 42565
				public static LocString NOTSELECTED = "Destination set";
			}

			// Token: 0x020025E8 RID: 9704
			public class DESTINATIONSELECTION_TOOLTIP
			{
				// Token: 0x0400A646 RID: 42566
				public static LocString REACHABLE = "Viable destination selected, ready for launch";

				// Token: 0x0400A647 RID: 42567
				public static LocString UNREACHABLE = "The selected destination is beyond rocket reach";

				// Token: 0x0400A648 RID: 42568
				public static LocString NOTSELECTED = "Select the rocket's Command Module to set a destination";
			}

			// Token: 0x020025E9 RID: 9705
			public class HASFOOD
			{
				// Token: 0x0400A649 RID: 42569
				public static LocString NAME = "Food Loaded";

				// Token: 0x0400A64A RID: 42570
				public static LocString TOOLTIP = "Sufficient food stores have been loaded, ready for launch";
			}

			// Token: 0x020025EA RID: 9706
			public class HASSUIT
			{
				// Token: 0x0400A64B RID: 42571
				public static LocString NAME = "Has " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400A64C RID: 42572
				public static LocString TOOLTIP = "An " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " has been loaded";
			}

			// Token: 0x020025EB RID: 9707
			public class NOSUIT
			{
				// Token: 0x0400A64D RID: 42573
				public static LocString NAME = "Missing " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400A64E RID: 42574
				public static LocString TOOLTIP = "Rocket cannot launch without an " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " loaded";
			}

			// Token: 0x020025EC RID: 9708
			public class NOFOOD
			{
				// Token: 0x0400A64F RID: 42575
				public static LocString NAME = "Insufficient Food";

				// Token: 0x0400A650 RID: 42576
				public static LocString TOOLTIP = "Rocket cannot launch without adequate food stores for passengers";
			}

			// Token: 0x020025ED RID: 9709
			public class CARGOEMPTY
			{
				// Token: 0x0400A651 RID: 42577
				public static LocString NAME = "Emptied Cargo Bay";

				// Token: 0x0400A652 RID: 42578
				public static LocString TOOLTIP = "Cargo Bays must be emptied of all materials before launch";
			}

			// Token: 0x020025EE RID: 9710
			public class LAUNCHCHECKLIST
			{
				// Token: 0x0400A653 RID: 42579
				public static LocString ASTRONAUT_TITLE = "Astronaut";

				// Token: 0x0400A654 RID: 42580
				public static LocString HASASTRONAUT = "Astronaut ready for liftoff";

				// Token: 0x0400A655 RID: 42581
				public static LocString ASTRONAUGHT = "No Astronaut assigned";

				// Token: 0x0400A656 RID: 42582
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400A657 RID: 42583
				public static LocString INSTALLED_TOOLTIP = "A suitable {0} has been installed";

				// Token: 0x0400A658 RID: 42584
				public static LocString REQUIRED = "Required";

				// Token: 0x0400A659 RID: 42585
				public static LocString REQUIRED_TOOLTIP = "A {0} must be installed before launch";

				// Token: 0x0400A65A RID: 42586
				public static LocString MISSING_TOOLTIP = "No {0} installed\n\nThis rocket cannot launch without a completed {0}";

				// Token: 0x0400A65B RID: 42587
				public static LocString NO_DESTINATION = "No destination selected";

				// Token: 0x0400A65C RID: 42588
				public static LocString MINIMUM_MASS = "Resources available {0}";

				// Token: 0x0400A65D RID: 42589
				public static LocString RESOURCE_MASS_TOOLTIP = "{0} has {1} resources available\nThis rocket has capacity for {2}";

				// Token: 0x0400A65E RID: 42590
				public static LocString INSUFFICENT_MASS_TOOLTIP = "Launching to this destination will not return a full cargo load";

				// Token: 0x020030E7 RID: 12519
				public class CONSTRUCTION_COMPLETE
				{
					// Token: 0x020033DF RID: 13279
					public class STATUS
					{
						// Token: 0x0400CC25 RID: 52261
						public static LocString READY = "No active construction";

						// Token: 0x0400CC26 RID: 52262
						public static LocString FAILURE = "No active construction";

						// Token: 0x0400CC27 RID: 52263
						public static LocString WARNING = "No active construction";
					}

					// Token: 0x020033E0 RID: 13280
					public class TOOLTIP
					{
						// Token: 0x0400CC28 RID: 52264
						public static LocString READY = "Construction of all modules is complete";

						// Token: 0x0400CC29 RID: 52265
						public static LocString FAILURE = "In-progress module construction is preventing takeoff";

						// Token: 0x0400CC2A RID: 52266
						public static LocString WARNING = "Construction warning";
					}
				}

				// Token: 0x020030E8 RID: 12520
				public class PILOT_BOARDED
				{
					// Token: 0x0400C4A5 RID: 50341
					public static LocString READY = "Pilot boarded";

					// Token: 0x0400C4A6 RID: 50342
					public static LocString FAILURE = "Pilot boarded";

					// Token: 0x0400C4A7 RID: 50343
					public static LocString WARNING = "Pilot boarded";

					// Token: 0x020033E1 RID: 13281
					public class TOOLTIP
					{
						// Token: 0x0400CC2B RID: 52267
						public static LocString READY = "A Duplicant with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill is currently onboard";

						// Token: 0x0400CC2C RID: 52268
						public static LocString FAILURE = "At least one crew member aboard the rocket must possess the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill to launch\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";

						// Token: 0x0400CC2D RID: 52269
						public static LocString WARNING = "Pilot warning";
					}
				}

				// Token: 0x020030E9 RID: 12521
				public class CREW_BOARDED
				{
					// Token: 0x0400C4A8 RID: 50344
					public static LocString READY = "All crew boarded";

					// Token: 0x0400C4A9 RID: 50345
					public static LocString FAILURE = "All crew boarded";

					// Token: 0x0400C4AA RID: 50346
					public static LocString WARNING = "All crew boarded";

					// Token: 0x020033E2 RID: 13282
					public class TOOLTIP
					{
						// Token: 0x0400CC2E RID: 52270
						public static LocString READY = "All Duplicants assigned to the rocket crew are boarded and ready for launch\n\n    • {0}/{1} Boarded";

						// Token: 0x0400CC2F RID: 52271
						public static LocString FAILURE = "No crew members have boarded this rocket\n\nDuplicants must be assigned to the rocket crew and have access to the module's hatch to board\n\n    • {0}/{1} Boarded";

						// Token: 0x0400CC30 RID: 52272
						public static LocString WARNING = "Some Duplicants assigned to this rocket crew have not yet boarded\n    • {0}/{1} Boarded";

						// Token: 0x0400CC31 RID: 52273
						public static LocString NONE = "There are no Duplicants assigned to this rocket crew\n    • {0}/{1} Boarded";
					}
				}

				// Token: 0x020030EA RID: 12522
				public class NO_EXTRA_PASSENGERS
				{
					// Token: 0x0400C4AB RID: 50347
					public static LocString READY = "Non-crew exited";

					// Token: 0x0400C4AC RID: 50348
					public static LocString FAILURE = "Non-crew exited";

					// Token: 0x0400C4AD RID: 50349
					public static LocString WARNING = "Non-crew exited";

					// Token: 0x020033E3 RID: 13283
					public class TOOLTIP
					{
						// Token: 0x0400CC32 RID: 52274
						public static LocString READY = "All non-crew Duplicants have disembarked";

						// Token: 0x0400CC33 RID: 52275
						public static LocString FAILURE = "Non-crew Duplicants must exit the rocket before launch";

						// Token: 0x0400CC34 RID: 52276
						public static LocString WARNING = "Non-crew warning";
					}
				}

				// Token: 0x020030EB RID: 12523
				public class FLIGHT_PATH_CLEAR
				{
					// Token: 0x020033E4 RID: 13284
					public class STATUS
					{
						// Token: 0x0400CC35 RID: 52277
						public static LocString READY = "Clear launch path";

						// Token: 0x0400CC36 RID: 52278
						public static LocString FAILURE = "Clear launch path";

						// Token: 0x0400CC37 RID: 52279
						public static LocString WARNING = "Clear launch path";
					}

					// Token: 0x020033E5 RID: 13285
					public class TOOLTIP
					{
						// Token: 0x0400CC38 RID: 52280
						public static LocString READY = "The rocket's launch path is clear for takeoff";

						// Token: 0x0400CC39 RID: 52281
						public static LocString FAILURE = "This rocket does not have a clear line of sight to space, preventing launch\n\nThe rocket's launch path can be cleared by excavating undug tiles and deconstructing any buildings above the rocket";

						// Token: 0x0400CC3A RID: 52282
						public static LocString WARNING = "";
					}
				}

				// Token: 0x020030EC RID: 12524
				public class HAS_FUEL_TANK
				{
					// Token: 0x020033E6 RID: 13286
					public class STATUS
					{
						// Token: 0x0400CC3B RID: 52283
						public static LocString READY = "Fuel Tank";

						// Token: 0x0400CC3C RID: 52284
						public static LocString FAILURE = "Fuel Tank";

						// Token: 0x0400CC3D RID: 52285
						public static LocString WARNING = "Fuel Tank";
					}

					// Token: 0x020033E7 RID: 13287
					public class TOOLTIP
					{
						// Token: 0x0400CC3E RID: 52286
						public static LocString READY = "A fuel tank has been installed";

						// Token: 0x0400CC3F RID: 52287
						public static LocString FAILURE = "No fuel tank installed\n\nThis rocket cannot launch without a completed fuel tank";

						// Token: 0x0400CC40 RID: 52288
						public static LocString WARNING = "Fuel tank warning";
					}
				}

				// Token: 0x020030ED RID: 12525
				public class HAS_ENGINE
				{
					// Token: 0x020033E8 RID: 13288
					public class STATUS
					{
						// Token: 0x0400CC41 RID: 52289
						public static LocString READY = "Engine";

						// Token: 0x0400CC42 RID: 52290
						public static LocString FAILURE = "Engine";

						// Token: 0x0400CC43 RID: 52291
						public static LocString WARNING = "Engine";
					}

					// Token: 0x020033E9 RID: 13289
					public class TOOLTIP
					{
						// Token: 0x0400CC44 RID: 52292
						public static LocString READY = "A suitable engine has been installed";

						// Token: 0x0400CC45 RID: 52293
						public static LocString FAILURE = "No engine installed\n\nThis rocket cannot launch without a completed engine";

						// Token: 0x0400CC46 RID: 52294
						public static LocString WARNING = "Engine warning";
					}
				}

				// Token: 0x020030EE RID: 12526
				public class HAS_NOSECONE
				{
					// Token: 0x020033EA RID: 13290
					public class STATUS
					{
						// Token: 0x0400CC47 RID: 52295
						public static LocString READY = "Nosecone";

						// Token: 0x0400CC48 RID: 52296
						public static LocString FAILURE = "Nosecone";

						// Token: 0x0400CC49 RID: 52297
						public static LocString WARNING = "Nosecone";
					}

					// Token: 0x020033EB RID: 13291
					public class TOOLTIP
					{
						// Token: 0x0400CC4A RID: 52298
						public static LocString READY = "A suitable nosecone has been installed";

						// Token: 0x0400CC4B RID: 52299
						public static LocString FAILURE = "No nosecone installed\n\nThis rocket cannot launch without a completed nosecone";

						// Token: 0x0400CC4C RID: 52300
						public static LocString WARNING = "Nosecone warning";
					}
				}

				// Token: 0x020030EF RID: 12527
				public class HAS_CONTROLSTATION
				{
					// Token: 0x020033EC RID: 13292
					public class STATUS
					{
						// Token: 0x0400CC4D RID: 52301
						public static LocString READY = "Control Station";

						// Token: 0x0400CC4E RID: 52302
						public static LocString FAILURE = "Control Station";

						// Token: 0x0400CC4F RID: 52303
						public static LocString WARNING = "Control Station";
					}

					// Token: 0x020033ED RID: 13293
					public class TOOLTIP
					{
						// Token: 0x0400CC50 RID: 52304
						public static LocString READY = "The control station is installed and waiting for the pilot";

						// Token: 0x0400CC51 RID: 52305
						public static LocString FAILURE = "No Control Station\n\nA new Rocket Control Station must be installed inside the rocket";

						// Token: 0x0400CC52 RID: 52306
						public static LocString WARNING = "Control Station warning";
					}
				}

				// Token: 0x020030F0 RID: 12528
				public class LOADING_COMPLETE
				{
					// Token: 0x020033EE RID: 13294
					public class STATUS
					{
						// Token: 0x0400CC53 RID: 52307
						public static LocString READY = "Cargo Loading Complete";

						// Token: 0x0400CC54 RID: 52308
						public static LocString FAILURE = "";

						// Token: 0x0400CC55 RID: 52309
						public static LocString WARNING = "Cargo Loading Complete";
					}

					// Token: 0x020033EF RID: 13295
					public class TOOLTIP
					{
						// Token: 0x0400CC56 RID: 52310
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400CC57 RID: 52311
						public static LocString FAILURE = "";

						// Token: 0x0400CC58 RID: 52312
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x020030F1 RID: 12529
				public class CARGO_TRANSFER_COMPLETE
				{
					// Token: 0x020033F0 RID: 13296
					public class STATUS
					{
						// Token: 0x0400CC59 RID: 52313
						public static LocString READY = "Cargo Transfer Complete";

						// Token: 0x0400CC5A RID: 52314
						public static LocString FAILURE = "";

						// Token: 0x0400CC5B RID: 52315
						public static LocString WARNING = "Cargo Transfer Complete";
					}

					// Token: 0x020033F1 RID: 13297
					public class TOOLTIP
					{
						// Token: 0x0400CC5C RID: 52316
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400CC5D RID: 52317
						public static LocString FAILURE = "";

						// Token: 0x0400CC5E RID: 52318
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x020030F2 RID: 12530
				public class INTERNAL_CONSTRUCTION_COMPLETE
				{
					// Token: 0x020033F2 RID: 13298
					public class STATUS
					{
						// Token: 0x0400CC5F RID: 52319
						public static LocString READY = "Landers Ready";

						// Token: 0x0400CC60 RID: 52320
						public static LocString FAILURE = "Landers Ready";

						// Token: 0x0400CC61 RID: 52321
						public static LocString WARNING = "";
					}

					// Token: 0x020033F3 RID: 13299
					public class TOOLTIP
					{
						// Token: 0x0400CC62 RID: 52322
						public static LocString READY = "All requested landers have been built and are ready for deployment";

						// Token: 0x0400CC63 RID: 52323
						public static LocString FAILURE = "Additional landers must be constructed to fulfill the lander requests of this rocket";

						// Token: 0x0400CC64 RID: 52324
						public static LocString WARNING = "";
					}
				}

				// Token: 0x020030F3 RID: 12531
				public class MAX_MODULES
				{
					// Token: 0x020033F4 RID: 13300
					public class STATUS
					{
						// Token: 0x0400CC65 RID: 52325
						public static LocString READY = "Module limit";

						// Token: 0x0400CC66 RID: 52326
						public static LocString FAILURE = "Module limit";

						// Token: 0x0400CC67 RID: 52327
						public static LocString WARNING = "Module limit";
					}

					// Token: 0x020033F5 RID: 13301
					public class TOOLTIP
					{
						// Token: 0x0400CC68 RID: 52328
						public static LocString READY = "The rocket's engine can support the number of installed rocket modules";

						// Token: 0x0400CC69 RID: 52329
						public static LocString FAILURE = "The number of installed modules exceeds the engine's module limit\n\nExcess modules must be removed";

						// Token: 0x0400CC6A RID: 52330
						public static LocString WARNING = "Module limit warning";
					}
				}

				// Token: 0x020030F4 RID: 12532
				public class HAS_RESOURCE
				{
					// Token: 0x020033F6 RID: 13302
					public class STATUS
					{
						// Token: 0x0400CC6B RID: 52331
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400CC6C RID: 52332
						public static LocString FAILURE = "{0} missing {1}";

						// Token: 0x0400CC6D RID: 52333
						public static LocString WARNING = "{0} missing {1}";
					}

					// Token: 0x020033F7 RID: 13303
					public class TOOLTIP
					{
						// Token: 0x0400CC6E RID: 52334
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400CC6F RID: 52335
						public static LocString FAILURE = "{0} has less than {1} {2}";

						// Token: 0x0400CC70 RID: 52336
						public static LocString WARNING = "{0} has less than {1} {2}";
					}
				}

				// Token: 0x020030F5 RID: 12533
				public class MAX_HEIGHT
				{
					// Token: 0x020033F8 RID: 13304
					public class STATUS
					{
						// Token: 0x0400CC71 RID: 52337
						public static LocString READY = "Height limit";

						// Token: 0x0400CC72 RID: 52338
						public static LocString FAILURE = "Height limit";

						// Token: 0x0400CC73 RID: 52339
						public static LocString WARNING = "Height limit";
					}

					// Token: 0x020033F9 RID: 13305
					public class TOOLTIP
					{
						// Token: 0x0400CC74 RID: 52340
						public static LocString READY = "The rocket's engine can support the height of the rocket";

						// Token: 0x0400CC75 RID: 52341
						public static LocString FAILURE = "The height of the rocket exceeds the engine's limit\n\nExcess modules must be removed";

						// Token: 0x0400CC76 RID: 52342
						public static LocString WARNING = "Height limit warning";
					}
				}

				// Token: 0x020030F6 RID: 12534
				public class PROPERLY_FUELED
				{
					// Token: 0x020033FA RID: 13306
					public class STATUS
					{
						// Token: 0x0400CC77 RID: 52343
						public static LocString READY = "Fueled";

						// Token: 0x0400CC78 RID: 52344
						public static LocString FAILURE = "Fueled";

						// Token: 0x0400CC79 RID: 52345
						public static LocString WARNING = "Fueled";
					}

					// Token: 0x020033FB RID: 13307
					public class TOOLTIP
					{
						// Token: 0x0400CC7A RID: 52346
						public static LocString READY = "The rocket is sufficiently fueled for a roundtrip to its destination and back";

						// Token: 0x0400CC7B RID: 52347
						public static LocString READY_NO_DESTINATION = "This rocket's fuel tanks have been filled to capacity, but it has no destination";

						// Token: 0x0400CC7C RID: 52348
						public static LocString FAILURE = "This rocket does not have enough fuel to reach its destination\n\nIf the tanks are full, a different Fuel Tank Module may be required";

						// Token: 0x0400CC7D RID: 52349
						public static LocString WARNING = "The rocket has enough fuel for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x020030F7 RID: 12535
				public class SUFFICIENT_OXIDIZER
				{
					// Token: 0x020033FC RID: 13308
					public class STATUS
					{
						// Token: 0x0400CC7E RID: 52350
						public static LocString READY = "Sufficient Oxidizer";

						// Token: 0x0400CC7F RID: 52351
						public static LocString FAILURE = "Sufficient Oxidizer";

						// Token: 0x0400CC80 RID: 52352
						public static LocString WARNING = "Warning: Limited oxidizer";
					}

					// Token: 0x020033FD RID: 13309
					public class TOOLTIP
					{
						// Token: 0x0400CC81 RID: 52353
						public static LocString READY = "This rocket has sufficient oxidizer for a roundtrip to its destination and back";

						// Token: 0x0400CC82 RID: 52354
						public static LocString FAILURE = "This rocket does not have enough oxidizer to reach its destination\n\nIf the oxidizer tanks are full, a different Oxidizer Tank Module may be required";

						// Token: 0x0400CC83 RID: 52355
						public static LocString WARNING = "The rocket has enough oxidizer for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x020030F8 RID: 12536
				public class ON_LAUNCHPAD
				{
					// Token: 0x020033FE RID: 13310
					public class STATUS
					{
						// Token: 0x0400CC84 RID: 52356
						public static LocString READY = "On a launch pad";

						// Token: 0x0400CC85 RID: 52357
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400CC86 RID: 52358
						public static LocString WARNING = "No launch pad";
					}

					// Token: 0x020033FF RID: 13311
					public class TOOLTIP
					{
						// Token: 0x0400CC87 RID: 52359
						public static LocString READY = "On a launch pad";

						// Token: 0x0400CC88 RID: 52360
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400CC89 RID: 52361
						public static LocString WARNING = "No launch pad";
					}
				}
			}

			// Token: 0x020025EF RID: 9711
			public class FULLTANK
			{
				// Token: 0x0400A65F RID: 42591
				public static LocString NAME = "Fuel Tank full";

				// Token: 0x0400A660 RID: 42592
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x020025F0 RID: 9712
			public class EMPTYTANK
			{
				// Token: 0x0400A661 RID: 42593
				public static LocString NAME = "Fuel Tank not full";

				// Token: 0x0400A662 RID: 42594
				public static LocString TOOLTIP = "Fuel tank must be filled before launch";
			}

			// Token: 0x020025F1 RID: 9713
			public class FULLOXIDIZERTANK
			{
				// Token: 0x0400A663 RID: 42595
				public static LocString NAME = "Oxidizer Tank full";

				// Token: 0x0400A664 RID: 42596
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x020025F2 RID: 9714
			public class EMPTYOXIDIZERTANK
			{
				// Token: 0x0400A665 RID: 42597
				public static LocString NAME = "Oxidizer Tank not full";

				// Token: 0x0400A666 RID: 42598
				public static LocString TOOLTIP = "Oxidizer tank must be filled before launch";
			}

			// Token: 0x020025F3 RID: 9715
			public class ROCKETSTATUS
			{
				// Token: 0x0400A667 RID: 42599
				public static LocString STATUS_TITLE = "Rocket Status";

				// Token: 0x0400A668 RID: 42600
				public static LocString NONE = "NONE";

				// Token: 0x0400A669 RID: 42601
				public static LocString SELECTED = "SELECTED";

				// Token: 0x0400A66A RID: 42602
				public static LocString LOCKEDIN = "LOCKED IN";

				// Token: 0x0400A66B RID: 42603
				public static LocString NODESTINATION = "No destination selected";

				// Token: 0x0400A66C RID: 42604
				public static LocString DESTINATIONVALUE = "None";

				// Token: 0x0400A66D RID: 42605
				public static LocString NOPASSENGERS = "No passengers";

				// Token: 0x0400A66E RID: 42606
				public static LocString STATUS = "Status";

				// Token: 0x0400A66F RID: 42607
				public static LocString TOTAL = "Total";

				// Token: 0x0400A670 RID: 42608
				public static LocString WEIGHTPENALTY = "Weight Penalty";

				// Token: 0x0400A671 RID: 42609
				public static LocString TIMEREMAINING = "Time Remaining";

				// Token: 0x0400A672 RID: 42610
				public static LocString BOOSTED_TIME_MODIFIER = "Less Than ";
			}

			// Token: 0x020025F4 RID: 9716
			public class ROCKETSTATS
			{
				// Token: 0x0400A673 RID: 42611
				public static LocString TOTAL_OXIDIZABLE_FUEL = "Total oxidizable fuel";

				// Token: 0x0400A674 RID: 42612
				public static LocString TOTAL_OXIDIZER = "Total oxidizer";

				// Token: 0x0400A675 RID: 42613
				public static LocString TOTAL_FUEL = "Total fuel";

				// Token: 0x0400A676 RID: 42614
				public static LocString NO_ENGINE = "NO ENGINE";

				// Token: 0x0400A677 RID: 42615
				public static LocString ENGINE_EFFICIENCY = "Main engine efficiency";

				// Token: 0x0400A678 RID: 42616
				public static LocString OXIDIZER_EFFICIENCY = "Average oxidizer efficiency";

				// Token: 0x0400A679 RID: 42617
				public static LocString SOLID_BOOSTER = "Solid boosters";

				// Token: 0x0400A67A RID: 42618
				public static LocString TOTAL_THRUST = "Total thrust";

				// Token: 0x0400A67B RID: 42619
				public static LocString TOTAL_RANGE = "Total range";

				// Token: 0x0400A67C RID: 42620
				public static LocString DRY_MASS = "Dry mass";

				// Token: 0x0400A67D RID: 42621
				public static LocString WET_MASS = "Wet mass";
			}

			// Token: 0x020025F5 RID: 9717
			public class STORAGESTATS
			{
				// Token: 0x0400A67E RID: 42622
				public static LocString STORAGECAPACITY = "{0} / {1}";
			}
		}

		// Token: 0x02001DAE RID: 7598
		public class RESEARCHSCREEN
		{
			// Token: 0x020025F6 RID: 9718
			public class FILTER_BUTTONS
			{
				// Token: 0x0400A67F RID: 42623
				public static LocString HEADER = "Preset Filters";

				// Token: 0x0400A680 RID: 42624
				public static LocString ALL = "All";

				// Token: 0x0400A681 RID: 42625
				public static LocString AVAILABLE = "Next";

				// Token: 0x0400A682 RID: 42626
				public static LocString COMPLETED = "Completed";

				// Token: 0x0400A683 RID: 42627
				public static LocString OXYGEN = "Oxygen";

				// Token: 0x0400A684 RID: 42628
				public static LocString FOOD = "Food";

				// Token: 0x0400A685 RID: 42629
				public static LocString WATER = "Water";

				// Token: 0x0400A686 RID: 42630
				public static LocString POWER = "Power";

				// Token: 0x0400A687 RID: 42631
				public static LocString MORALE = "Morale";

				// Token: 0x0400A688 RID: 42632
				public static LocString RANCHING = "Ranching";

				// Token: 0x0400A689 RID: 42633
				public static LocString FILTER = "Filters";

				// Token: 0x0400A68A RID: 42634
				public static LocString TILE = "Tiles";

				// Token: 0x0400A68B RID: 42635
				public static LocString TRANSPORT = "Transport";

				// Token: 0x0400A68C RID: 42636
				public static LocString AUTOMATION = "Automation";

				// Token: 0x0400A68D RID: 42637
				public static LocString MEDICINE = "Medicine";

				// Token: 0x0400A68E RID: 42638
				public static LocString ROCKET = "Rockets";

				// Token: 0x0400A68F RID: 42639
				public static LocString RADIATION = "Radiation";
			}
		}

		// Token: 0x02001DAF RID: 7599
		public class CODEX
		{
			// Token: 0x04008747 RID: 34631
			public static LocString SEARCH_HEADER = "Search Database";

			// Token: 0x04008748 RID: 34632
			public static LocString BACK_BUTTON = "Back ({0})";

			// Token: 0x04008749 RID: 34633
			public static LocString TIPS = "Tips";

			// Token: 0x0400874A RID: 34634
			public static LocString GAME_SYSTEMS = "Systems";

			// Token: 0x0400874B RID: 34635
			public static LocString DETAILS = "Details";

			// Token: 0x0400874C RID: 34636
			public static LocString RECIPE_ITEM = "{0} x {1}{2}";

			// Token: 0x0400874D RID: 34637
			public static LocString RECIPE_FABRICATOR = "{1} ({0} seconds)";

			// Token: 0x0400874E RID: 34638
			public static LocString RECIPE_FABRICATOR_HEADER = "Produced by";

			// Token: 0x0400874F RID: 34639
			public static LocString BACK_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\n{0}";

			// Token: 0x04008750 RID: 34640
			public static LocString BACK_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\nN/A";

			// Token: 0x04008751 RID: 34641
			public static LocString FORWARD_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\n{0}";

			// Token: 0x04008752 RID: 34642
			public static LocString FORWARD_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\nN/A";

			// Token: 0x04008753 RID: 34643
			public static LocString TITLE = "DATABASE";

			// Token: 0x04008754 RID: 34644
			public static LocString MANAGEMENT_BUTTON = "DATABASE";

			// Token: 0x020025F7 RID: 9719
			public class CODEX_DISCOVERED_MESSAGE
			{
				// Token: 0x0400A690 RID: 42640
				public static LocString TITLE = "New Log Entry";

				// Token: 0x0400A691 RID: 42641
				public static LocString BODY = "I've added a new entry to my log: {codex}\n";
			}

			// Token: 0x020025F8 RID: 9720
			public class SUBWORLDS
			{
				// Token: 0x0400A692 RID: 42642
				public static LocString ELEMENTS = "Elements";

				// Token: 0x0400A693 RID: 42643
				public static LocString PLANTS = "Plants";

				// Token: 0x0400A694 RID: 42644
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400A695 RID: 42645
				public static LocString NONE = "None";
			}

			// Token: 0x020025F9 RID: 9721
			public class GEYSERS
			{
				// Token: 0x0400A696 RID: 42646
				public static LocString DESC = "Geysers and Fumaroles emit elements at variable intervals. They provide a sustainable source of material, albeit in typically low volumes.\n\nThe variable factors of a geyser are:\n\n    • Emission element \n    • Emission temperature \n    • Emission mass \n    • Cycle length \n    • Dormancy duration \n    • Disease emitted";
			}

			// Token: 0x020025FA RID: 9722
			public class EQUIPMENT
			{
				// Token: 0x0400A697 RID: 42647
				public static LocString DESC = "Equipment description";
			}

			// Token: 0x020025FB RID: 9723
			public class FOOD
			{
				// Token: 0x0400A698 RID: 42648
				public static LocString QUALITY = "Quality: {0}";

				// Token: 0x0400A699 RID: 42649
				public static LocString CALORIES = "Calories: {0}";

				// Token: 0x0400A69A RID: 42650
				public static LocString SPOILPROPERTIES = "Refrigeration temperature: {0}\nDeep Freeze temperature: {1}\nSpoil time: {2}";

				// Token: 0x0400A69B RID: 42651
				public static LocString NON_PERISHABLE = "Spoil time: Never";
			}

			// Token: 0x020025FC RID: 9724
			public class CATEGORYNAMES
			{
				// Token: 0x0400A69C RID: 42652
				public static LocString ROOT = UI.FormatAsLink("Index", "HOME");

				// Token: 0x0400A69D RID: 42653
				public static LocString PLANTS = UI.FormatAsLink("Plants", "PLANTS");

				// Token: 0x0400A69E RID: 42654
				public static LocString CREATURES = UI.FormatAsLink("Critters", "CREATURES");

				// Token: 0x0400A69F RID: 42655
				public static LocString EMAILS = UI.FormatAsLink("E-mail", "EMAILS");

				// Token: 0x0400A6A0 RID: 42656
				public static LocString JOURNALS = UI.FormatAsLink("Journals", "JOURNALS");

				// Token: 0x0400A6A1 RID: 42657
				public static LocString MYLOG = UI.FormatAsLink("My Log", "MYLOG");

				// Token: 0x0400A6A2 RID: 42658
				public static LocString INVESTIGATIONS = UI.FormatAsLink("Investigations", "Investigations");

				// Token: 0x0400A6A3 RID: 42659
				public static LocString RESEARCHNOTES = UI.FormatAsLink("Research Notes", "RESEARCHNOTES");

				// Token: 0x0400A6A4 RID: 42660
				public static LocString NOTICES = UI.FormatAsLink("Notices", "NOTICES");

				// Token: 0x0400A6A5 RID: 42661
				public static LocString FOOD = UI.FormatAsLink("Food", "FOOD");

				// Token: 0x0400A6A6 RID: 42662
				public static LocString MINION_MODIFIERS = UI.FormatAsLink("Duplicant Effects (EDITOR ONLY)", "MINION_MODIFIERS");

				// Token: 0x0400A6A7 RID: 42663
				public static LocString BUILDINGS = UI.FormatAsLink("Buildings", "BUILDINGS");

				// Token: 0x0400A6A8 RID: 42664
				public static LocString ROOMS = UI.FormatAsLink("Rooms", "ROOMS");

				// Token: 0x0400A6A9 RID: 42665
				public static LocString TECH = UI.FormatAsLink("Research", "TECH");

				// Token: 0x0400A6AA RID: 42666
				public static LocString TIPS = UI.FormatAsLink("Lessons", "LESSONS");

				// Token: 0x0400A6AB RID: 42667
				public static LocString EQUIPMENT = UI.FormatAsLink("Equipment", "EQUIPMENT");

				// Token: 0x0400A6AC RID: 42668
				public static LocString BIOMES = UI.FormatAsLink("Biomes", "BIOMES");

				// Token: 0x0400A6AD RID: 42669
				public static LocString STORYTRAITS = UI.FormatAsLink("Story Traits", "STORYTRAITS");

				// Token: 0x0400A6AE RID: 42670
				public static LocString VIDEOS = UI.FormatAsLink("Videos", "VIDEOS");

				// Token: 0x0400A6AF RID: 42671
				public static LocString MISCELLANEOUSTIPS = UI.FormatAsLink("Tips", "MISCELLANEOUSTIPS");

				// Token: 0x0400A6B0 RID: 42672
				public static LocString MISCELLANEOUSITEMS = UI.FormatAsLink("Items", "MISCELLANEOUSITEMS");

				// Token: 0x0400A6B1 RID: 42673
				public static LocString ELEMENTS = UI.FormatAsLink("Elements", "ELEMENTS");

				// Token: 0x0400A6B2 RID: 42674
				public static LocString ELEMENTSSOLID = UI.FormatAsLink("Solids", "ELEMENTS_SOLID");

				// Token: 0x0400A6B3 RID: 42675
				public static LocString ELEMENTSGAS = UI.FormatAsLink("Gases", "ELEMENTS_GAS");

				// Token: 0x0400A6B4 RID: 42676
				public static LocString ELEMENTSLIQUID = UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID");

				// Token: 0x0400A6B5 RID: 42677
				public static LocString ELEMENTSOTHER = UI.FormatAsLink("Other", "ELEMENTS_OTHER");

				// Token: 0x0400A6B6 RID: 42678
				public static LocString BUILDINGMATERIALCLASSES = UI.FormatAsLink("Building Materials", "BUILDING_MATERIAL_CLASSES");

				// Token: 0x0400A6B7 RID: 42679
				public static LocString INDUSTRIALINGREDIENTS = UI.FormatAsLink("Industrial Ingredients", "INDUSTRIALINGREDIENTS");

				// Token: 0x0400A6B8 RID: 42680
				public static LocString GEYSERS = UI.FormatAsLink("Geysers", "GEYSERS");

				// Token: 0x0400A6B9 RID: 42681
				public static LocString SYSTEMS = UI.FormatAsLink("Systems", "SYSTEMS");

				// Token: 0x0400A6BA RID: 42682
				public static LocString ROLES = UI.FormatAsLink("Duplicant Skills", "ROLES");

				// Token: 0x0400A6BB RID: 42683
				public static LocString DISEASE = UI.FormatAsLink("Disease", "DISEASE");

				// Token: 0x0400A6BC RID: 42684
				public static LocString SICKNESS = UI.FormatAsLink("Sickness", "SICKNESS");

				// Token: 0x0400A6BD RID: 42685
				public static LocString MEDIA = UI.FormatAsLink("Media", "MEDIA");
			}
		}

		// Token: 0x02001DB0 RID: 7600
		public class DEVELOPMENTBUILDS
		{
			// Token: 0x04008755 RID: 34645
			public static LocString WATERMARK = "BUILD: {0}";

			// Token: 0x04008756 RID: 34646
			public static LocString TESTING_WATERMARK = "TESTING BUILD: {0}";

			// Token: 0x04008757 RID: 34647
			public static LocString TESTING_TOOLTIP = "This game is currently running a Test version.\n\n" + UI.CLICK(UI.ClickType.Click) + " for more info.";

			// Token: 0x04008758 RID: 34648
			public static LocString TESTING_MESSAGE_TITLE = "TESTING BUILD";

			// Token: 0x04008759 RID: 34649
			public static LocString TESTING_MESSAGE = "This game is running a Test version of Oxygen Not Included. This means that some features may be in development or buggier than normal, and require more testing before they can be moved into the Release build.\n\nIf you encounter any bugs or strange behavior, please add a report to the bug forums. We appreciate it!";

			// Token: 0x0400875A RID: 34650
			public static LocString TESTING_MORE_INFO = "BUG FORUMS";

			// Token: 0x0400875B RID: 34651
			public static LocString FULL_PATCH_NOTES = "Full Patch Notes";

			// Token: 0x0400875C RID: 34652
			public static LocString PREVIOUS_VERSION = "Previous Version";

			// Token: 0x020025FD RID: 9725
			public class ALPHA
			{
				// Token: 0x020030F9 RID: 12537
				public class MESSAGES
				{
					// Token: 0x0400C4AE RID: 50350
					public static LocString FORUMBUTTON = "FORUMS";

					// Token: 0x0400C4AF RID: 50351
					public static LocString MAILINGLIST = "MAILING LIST";

					// Token: 0x0400C4B0 RID: 50352
					public static LocString PATCHNOTES = "PATCH NOTES";

					// Token: 0x0400C4B1 RID: 50353
					public static LocString FEEDBACK = "FEEDBACK";
				}

				// Token: 0x020030FA RID: 12538
				public class LOADING
				{
					// Token: 0x0400C4B2 RID: 50354
					public static LocString TITLE = "<b>Welcome to Oxygen Not Included!</b>";

					// Token: 0x0400C4B3 RID: 50355
					public static LocString BODY = "This game is in the early stages of development which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\nDuring this time Oxygen Not Included will be receiving regular updates to fix bugs, add features, and introduce additional content, so if you encounter issues or just have suggestions to share, please let us know on our forums: <u>http://forums.kleientertainment.com</u>\n\nA special thanks to those who joined us during our time in Alpha. We value your feedback and thank you for joining us in the development process. We couldn't do this without you.\n\nEnjoy your time in deep space!\n\n- Klei";

					// Token: 0x0400C4B4 RID: 50356
					public static LocString BODY_NOLINKS = "This DLC is currently in active development, which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\n During this time Spaced Out! will be receiving regular updates to fix bugs, add features, and introduce additional content.\n\n We've got lots of content old and new to add to this DLC before it's ready, and we're happy to have you along with us. Enjoy your time in deep space!\n\n - The Team at Klei";

					// Token: 0x0400C4B5 RID: 50357
					public static LocString FORUMBUTTON = "Visit Forums";
				}

				// Token: 0x020030FB RID: 12539
				public class HEALTHY_MESSAGE
				{
					// Token: 0x0400C4B6 RID: 50358
					public static LocString CONTINUEBUTTON = "Thanks!";
				}
			}

			// Token: 0x020025FE RID: 9726
			public class PREVIOUS_UPDATE
			{
				// Token: 0x0400A6BE RID: 42686
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included!</b>";

				// Token: 0x0400A6BF RID: 42687
				public static LocString BODY = "Whoops!\n\nYou're about to opt in to the <b>Previous Update branch</b>. That means opting out of all new features, fixes and content from the live branch.\n\nThis branch is temporary. It will be replaced when the next update is released. It's also completely unsupported: please don't report bugs or issues you find here.\n\nAre you sure you want to opt in?";

				// Token: 0x0400A6C0 RID: 42688
				public static LocString CONTINUEBUTTON = "Play Old Version";

				// Token: 0x0400A6C1 RID: 42689
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400A6C2 RID: 42690
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x020025FF RID: 9727
			public class UPDATES
			{
				// Token: 0x0400A6C3 RID: 42691
				public static LocString UPDATES_HEADER = "NEXT UPGRADE LIVE IN";

				// Token: 0x0400A6C4 RID: 42692
				public static LocString NOW = "Less than a day";

				// Token: 0x0400A6C5 RID: 42693
				public static LocString TWENTY_FOUR_HOURS = "Less than a day";

				// Token: 0x0400A6C6 RID: 42694
				public static LocString FINAL_WEEK = "{0} days";

				// Token: 0x0400A6C7 RID: 42695
				public static LocString BIGGER_TIMES = "{1} weeks {0} days";
			}
		}

		// Token: 0x02001DB1 RID: 7601
		public class UNITSUFFIXES
		{
			// Token: 0x0400875D RID: 34653
			public static LocString SECOND = " s";

			// Token: 0x0400875E RID: 34654
			public static LocString PERSECOND = "/s";

			// Token: 0x0400875F RID: 34655
			public static LocString PERCYCLE = "/cycle";

			// Token: 0x04008760 RID: 34656
			public static LocString UNIT = " unit";

			// Token: 0x04008761 RID: 34657
			public static LocString UNITS = " units";

			// Token: 0x04008762 RID: 34658
			public static LocString PERCENT = "%";

			// Token: 0x04008763 RID: 34659
			public static LocString DEGREES = " degrees";

			// Token: 0x04008764 RID: 34660
			public static LocString CRITTERS = " critters";

			// Token: 0x04008765 RID: 34661
			public static LocString GROWTH = "growth";

			// Token: 0x04008766 RID: 34662
			public static LocString SECONDS = "Seconds";

			// Token: 0x04008767 RID: 34663
			public static LocString DUPLICANTS = "Duplicants";

			// Token: 0x04008768 RID: 34664
			public static LocString GERMS = "Germs";

			// Token: 0x04008769 RID: 34665
			public static LocString ROCKET_MISSIONS = "Missions";

			// Token: 0x0400876A RID: 34666
			public static LocString TILES = "Tiles";

			// Token: 0x02002600 RID: 9728
			public class MASS
			{
				// Token: 0x0400A6C8 RID: 42696
				public static LocString TONNE = " t";

				// Token: 0x0400A6C9 RID: 42697
				public static LocString KILOGRAM = " kg";

				// Token: 0x0400A6CA RID: 42698
				public static LocString GRAM = " g";

				// Token: 0x0400A6CB RID: 42699
				public static LocString MILLIGRAM = " mg";

				// Token: 0x0400A6CC RID: 42700
				public static LocString MICROGRAM = " mcg";

				// Token: 0x0400A6CD RID: 42701
				public static LocString POUND = " lb";

				// Token: 0x0400A6CE RID: 42702
				public static LocString DRACHMA = " dr";

				// Token: 0x0400A6CF RID: 42703
				public static LocString GRAIN = " gr";
			}

			// Token: 0x02002601 RID: 9729
			public class TEMPERATURE
			{
				// Token: 0x0400A6D0 RID: 42704
				public static LocString CELSIUS = " " + 'º'.ToString() + "C";

				// Token: 0x0400A6D1 RID: 42705
				public static LocString FAHRENHEIT = " " + 'º'.ToString() + "F";

				// Token: 0x0400A6D2 RID: 42706
				public static LocString KELVIN = " K";
			}

			// Token: 0x02002602 RID: 9730
			public class CALORIES
			{
				// Token: 0x0400A6D3 RID: 42707
				public static LocString CALORIE = " cal";

				// Token: 0x0400A6D4 RID: 42708
				public static LocString KILOCALORIE = " kcal";
			}

			// Token: 0x02002603 RID: 9731
			public class ELECTRICAL
			{
				// Token: 0x0400A6D5 RID: 42709
				public static LocString JOULE = " J";

				// Token: 0x0400A6D6 RID: 42710
				public static LocString KILOJOULE = " kJ";

				// Token: 0x0400A6D7 RID: 42711
				public static LocString MEGAJOULE = " MJ";

				// Token: 0x0400A6D8 RID: 42712
				public static LocString WATT = " W";

				// Token: 0x0400A6D9 RID: 42713
				public static LocString KILOWATT = " kW";
			}

			// Token: 0x02002604 RID: 9732
			public class HEAT
			{
				// Token: 0x0400A6DA RID: 42714
				public static LocString DTU = " DTU";

				// Token: 0x0400A6DB RID: 42715
				public static LocString KDTU = " kDTU";

				// Token: 0x0400A6DC RID: 42716
				public static LocString DTU_S = " DTU/s";

				// Token: 0x0400A6DD RID: 42717
				public static LocString KDTU_S = " kDTU/s";
			}

			// Token: 0x02002605 RID: 9733
			public class DISTANCE
			{
				// Token: 0x0400A6DE RID: 42718
				public static LocString METER = " m";

				// Token: 0x0400A6DF RID: 42719
				public static LocString KILOMETER = " km";
			}

			// Token: 0x02002606 RID: 9734
			public class DISEASE
			{
				// Token: 0x0400A6E0 RID: 42720
				public static LocString UNITS = " germs";
			}

			// Token: 0x02002607 RID: 9735
			public class NOISE
			{
				// Token: 0x0400A6E1 RID: 42721
				public static LocString UNITS = " dB";
			}

			// Token: 0x02002608 RID: 9736
			public class INFORMATION
			{
				// Token: 0x0400A6E2 RID: 42722
				public static LocString BYTE = "B";

				// Token: 0x0400A6E3 RID: 42723
				public static LocString KILOBYTE = "kB";

				// Token: 0x0400A6E4 RID: 42724
				public static LocString MEGABYTE = "MB";

				// Token: 0x0400A6E5 RID: 42725
				public static LocString GIGABYTE = "GB";

				// Token: 0x0400A6E6 RID: 42726
				public static LocString TERABYTE = "TB";
			}

			// Token: 0x02002609 RID: 9737
			public class LIGHT
			{
				// Token: 0x0400A6E7 RID: 42727
				public static LocString LUX = " lux";
			}

			// Token: 0x0200260A RID: 9738
			public class RADIATION
			{
				// Token: 0x0400A6E8 RID: 42728
				public static LocString RADS = " rads";
			}

			// Token: 0x0200260B RID: 9739
			public class HIGHENERGYPARTICLES
			{
				// Token: 0x0400A6E9 RID: 42729
				public static LocString PARTRICLE = " Radbolt";

				// Token: 0x0400A6EA RID: 42730
				public static LocString PARTRICLES = " Radbolts";
			}
		}

		// Token: 0x02001DB2 RID: 7602
		public class OVERLAYS
		{
			// Token: 0x0200260C RID: 9740
			public class TILEMODE
			{
				// Token: 0x0400A6EB RID: 42731
				public static LocString NAME = "MATERIALS OVERLAY";

				// Token: 0x0400A6EC RID: 42732
				public static LocString BUTTON = "Materials Overlay";
			}

			// Token: 0x0200260D RID: 9741
			public class OXYGEN
			{
				// Token: 0x0400A6ED RID: 42733
				public static LocString NAME = "OXYGEN OVERLAY";

				// Token: 0x0400A6EE RID: 42734
				public static LocString BUTTON = "Oxygen Overlay";

				// Token: 0x0400A6EF RID: 42735
				public static LocString LEGEND1 = "Very Breathable";

				// Token: 0x0400A6F0 RID: 42736
				public static LocString LEGEND2 = "Breathable";

				// Token: 0x0400A6F1 RID: 42737
				public static LocString LEGEND3 = "Barely Breathable";

				// Token: 0x0400A6F2 RID: 42738
				public static LocString LEGEND4 = "Unbreathable";

				// Token: 0x0400A6F3 RID: 42739
				public static LocString LEGEND5 = "Barely Breathable";

				// Token: 0x0400A6F4 RID: 42740
				public static LocString LEGEND6 = "Unbreathable";

				// Token: 0x020030FC RID: 12540
				public class TOOLTIPS
				{
					// Token: 0x0400C4B7 RID: 50359
					public static LocString LEGEND1 = string.Concat(new string[]
					{
						"<b>Very Breathable</b>\nHigh ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400C4B8 RID: 50360
					public static LocString LEGEND2 = string.Concat(new string[]
					{
						"<b>Breathable</b>\nSufficient ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400C4B9 RID: 50361
					public static LocString LEGEND3 = string.Concat(new string[]
					{
						"<b>Barely Breathable</b>\nLow ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400C4BA RID: 50362
					public static LocString LEGEND4 = string.Concat(new string[]
					{
						"<b>Unbreathable</b>\nExtremely low or absent ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations\n\nDuplicants will suffocate if trapped in these areas"
					});

					// Token: 0x0400C4BB RID: 50363
					public static LocString LEGEND5 = "<b>Slightly Toxic</b>\nHarmful gas concentration";

					// Token: 0x0400C4BC RID: 50364
					public static LocString LEGEND6 = "<b>Very Toxic</b>\nLethal gas concentration";
				}
			}

			// Token: 0x0200260E RID: 9742
			public class ELECTRICAL
			{
				// Token: 0x0400A6F5 RID: 42741
				public static LocString NAME = "POWER OVERLAY";

				// Token: 0x0400A6F6 RID: 42742
				public static LocString BUTTON = "Power Overlay";

				// Token: 0x0400A6F7 RID: 42743
				public static LocString LEGEND1 = "<b>BUILDING POWER</b>";

				// Token: 0x0400A6F8 RID: 42744
				public static LocString LEGEND2 = "Consumer";

				// Token: 0x0400A6F9 RID: 42745
				public static LocString LEGEND3 = "Producer";

				// Token: 0x0400A6FA RID: 42746
				public static LocString LEGEND4 = "<b>CIRCUIT POWER HEALTH</b>";

				// Token: 0x0400A6FB RID: 42747
				public static LocString LEGEND5 = "Inactive";

				// Token: 0x0400A6FC RID: 42748
				public static LocString LEGEND6 = "Safe";

				// Token: 0x0400A6FD RID: 42749
				public static LocString LEGEND7 = "Strained";

				// Token: 0x0400A6FE RID: 42750
				public static LocString LEGEND8 = "Overloaded";

				// Token: 0x0400A6FF RID: 42751
				public static LocString DIAGRAM_HEADER = "Energy from the <b>Left Outlet</b> is used by the <b>Right Outlet</b>";

				// Token: 0x0400A700 RID: 42752
				public static LocString LEGEND_SWITCH = "Switch";

				// Token: 0x020030FD RID: 12541
				public class TOOLTIPS
				{
					// Token: 0x0400C4BD RID: 50365
					public static LocString LEGEND1 = "Displays whether buildings use or generate " + UI.FormatAsLink("Power", "POWER");

					// Token: 0x0400C4BE RID: 50366
					public static LocString LEGEND2 = "<b>Consumer</b>\nThese buildings draw power from a circuit";

					// Token: 0x0400C4BF RID: 50367
					public static LocString LEGEND3 = "<b>Producer</b>\nThese buildings generate power for a circuit";

					// Token: 0x0400C4C0 RID: 50368
					public static LocString LEGEND4 = "Displays the health of wire systems";

					// Token: 0x0400C4C1 RID: 50369
					public static LocString LEGEND5 = "<b>Inactive</b>\nThere is no power activity on these circuits";

					// Token: 0x0400C4C2 RID: 50370
					public static LocString LEGEND6 = "<b>Safe</b>\nThese circuits are not in danger of overloading";

					// Token: 0x0400C4C3 RID: 50371
					public static LocString LEGEND7 = "<b>Strained</b>\nThese circuits are close to consuming more power than their wires support";

					// Token: 0x0400C4C4 RID: 50372
					public static LocString LEGEND8 = "<b>Overloaded</b>\nThese circuits are consuming more power than their wires support";

					// Token: 0x0400C4C5 RID: 50373
					public static LocString LEGEND_SWITCH = "<b>Switch</b>\nActivates or deactivates connected circuits";
				}
			}

			// Token: 0x0200260F RID: 9743
			public class TEMPERATURE
			{
				// Token: 0x0400A701 RID: 42753
				public static LocString NAME = "TEMPERATURE OVERLAY";

				// Token: 0x0400A702 RID: 42754
				public static LocString BUTTON = "Temperature Overlay";

				// Token: 0x0400A703 RID: 42755
				public static LocString EXTREMECOLD = "Absolute Zero";

				// Token: 0x0400A704 RID: 42756
				public static LocString VERYCOLD = "Cold";

				// Token: 0x0400A705 RID: 42757
				public static LocString COLD = "Chilled";

				// Token: 0x0400A706 RID: 42758
				public static LocString TEMPERATE = "Temperate";

				// Token: 0x0400A707 RID: 42759
				public static LocString HOT = "Warm";

				// Token: 0x0400A708 RID: 42760
				public static LocString VERYHOT = "Hot";

				// Token: 0x0400A709 RID: 42761
				public static LocString EXTREMEHOT = "Scorching";

				// Token: 0x0400A70A RID: 42762
				public static LocString MAXHOT = "Molten";

				// Token: 0x020030FE RID: 12542
				public class TOOLTIPS
				{
					// Token: 0x0400C4C6 RID: 50374
					public static LocString TEMPERATURE = "Temperatures reaching {0}";
				}
			}

			// Token: 0x02002610 RID: 9744
			public class STATECHANGE
			{
				// Token: 0x0400A70B RID: 42763
				public static LocString LOWPOINT = "Low energy state change";

				// Token: 0x0400A70C RID: 42764
				public static LocString STABLE = "Stable";

				// Token: 0x0400A70D RID: 42765
				public static LocString HIGHPOINT = "High energy state change";

				// Token: 0x020030FF RID: 12543
				public class TOOLTIPS
				{
					// Token: 0x0400C4C7 RID: 50375
					public static LocString LOWPOINT = "Nearing a low energy state change";

					// Token: 0x0400C4C8 RID: 50376
					public static LocString STABLE = "Not near any state changes";

					// Token: 0x0400C4C9 RID: 50377
					public static LocString HIGHPOINT = "Nearing high energy state change";
				}
			}

			// Token: 0x02002611 RID: 9745
			public class HEATFLOW
			{
				// Token: 0x0400A70E RID: 42766
				public static LocString NAME = "THERMAL TOLERANCE OVERLAY";

				// Token: 0x0400A70F RID: 42767
				public static LocString HOVERTITLE = "THERMAL TOLERANCE";

				// Token: 0x0400A710 RID: 42768
				public static LocString BUTTON = "Thermal Tolerance Overlay";

				// Token: 0x0400A711 RID: 42769
				public static LocString COOLING = "Body Heat Loss";

				// Token: 0x0400A712 RID: 42770
				public static LocString NEUTRAL = "Comfort Zone";

				// Token: 0x0400A713 RID: 42771
				public static LocString HEATING = "Body Heat Retention";

				// Token: 0x02003100 RID: 12544
				public class TOOLTIPS
				{
					// Token: 0x0400C4CA RID: 50378
					public static LocString COOLING = "<b>Body Heat Loss</b>\nUncomfortably cold\n\nDuplicants lose more heat in these areas than they can absorb\n* Warm Sweaters help Duplicants retain body heat";

					// Token: 0x0400C4CB RID: 50379
					public static LocString NEUTRAL = "<b>Comfort Zone</b>\nComfortable area\n\nDuplicants can regulate their internal temperatures in these areas";

					// Token: 0x0400C4CC RID: 50380
					public static LocString HEATING = "<b>Body Heat Retention</b>\nUncomfortably warm\n\nDuplicants absorb more heat in these areas than they can release\n* Cool Vests help Duplicants shed excess body heat";
				}
			}

			// Token: 0x02002612 RID: 9746
			public class ROOMS
			{
				// Token: 0x0400A714 RID: 42772
				public static LocString NAME = "ROOM OVERLAY";

				// Token: 0x0400A715 RID: 42773
				public static LocString BUTTON = "Room Overlay";

				// Token: 0x0400A716 RID: 42774
				public static LocString ROOM = "Room {0}";

				// Token: 0x0400A717 RID: 42775
				public static LocString HOVERTITLE = "ROOMS";

				// Token: 0x02003101 RID: 12545
				public static class NOROOM
				{
					// Token: 0x0400C4CD RID: 50381
					public static LocString HEADER = "No Room";

					// Token: 0x0400C4CE RID: 50382
					public static LocString DESC = "Enclose this space with walls and doors to make a room";

					// Token: 0x0400C4CF RID: 50383
					public static LocString TOO_BIG = "<color=#F44A47FF>    • Size: {0} Tiles\n    • Maximum room size: {1} Tiles</color>";
				}

				// Token: 0x02003102 RID: 12546
				public class TOOLTIPS
				{
					// Token: 0x0400C4D0 RID: 50384
					public static LocString ROOM = "Completed Duplicant bedrooms";

					// Token: 0x0400C4D1 RID: 50385
					public static LocString NOROOMS = "Duplicants have nowhere to sleep";
				}
			}

			// Token: 0x02002613 RID: 9747
			public class JOULES
			{
				// Token: 0x0400A718 RID: 42776
				public static LocString NAME = "JOULES";

				// Token: 0x0400A719 RID: 42777
				public static LocString HOVERTITLE = "JOULES";

				// Token: 0x0400A71A RID: 42778
				public static LocString BUTTON = "Joules Overlay";
			}

			// Token: 0x02002614 RID: 9748
			public class LIGHTING
			{
				// Token: 0x0400A71B RID: 42779
				public static LocString NAME = "LIGHT OVERLAY";

				// Token: 0x0400A71C RID: 42780
				public static LocString BUTTON = "Light Overlay";

				// Token: 0x0400A71D RID: 42781
				public static LocString LITAREA = "Lit Area";

				// Token: 0x0400A71E RID: 42782
				public static LocString DARK = "Unlit Area";

				// Token: 0x0400A71F RID: 42783
				public static LocString HOVERTITLE = "LIGHT";

				// Token: 0x0400A720 RID: 42784
				public static LocString DESC = "{0} Lux";

				// Token: 0x02003103 RID: 12547
				public class RANGES
				{
					// Token: 0x0400C4D2 RID: 50386
					public static LocString NO_LIGHT = "Pitch Black";

					// Token: 0x0400C4D3 RID: 50387
					public static LocString VERY_LOW_LIGHT = "Dark";

					// Token: 0x0400C4D4 RID: 50388
					public static LocString LOW_LIGHT = "Dim";

					// Token: 0x0400C4D5 RID: 50389
					public static LocString MEDIUM_LIGHT = "Well Lit";

					// Token: 0x0400C4D6 RID: 50390
					public static LocString HIGH_LIGHT = "Bright";

					// Token: 0x0400C4D7 RID: 50391
					public static LocString VERY_HIGH_LIGHT = "Brilliant";

					// Token: 0x0400C4D8 RID: 50392
					public static LocString MAX_LIGHT = "Blinding";
				}

				// Token: 0x02003104 RID: 12548
				public class TOOLTIPS
				{
					// Token: 0x0400C4D9 RID: 50393
					public static LocString NAME = "LIGHT OVERLAY";

					// Token: 0x0400C4DA RID: 50394
					public static LocString LITAREA = "<b>Lit Area</b>\nWorking in well-lit areas improves Duplicant " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

					// Token: 0x0400C4DB RID: 50395
					public static LocString DARK = "<b>Unlit Area</b>\nWorking in the dark has no effect on Duplicants";
				}
			}

			// Token: 0x02002615 RID: 9749
			public class CROP
			{
				// Token: 0x0400A721 RID: 42785
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400A722 RID: 42786
				public static LocString BUTTON = "Farming Overlay";

				// Token: 0x0400A723 RID: 42787
				public static LocString GROWTH_HALTED = "Halted Growth";

				// Token: 0x0400A724 RID: 42788
				public static LocString GROWING = "Growing";

				// Token: 0x0400A725 RID: 42789
				public static LocString FULLY_GROWN = "Fully Grown";

				// Token: 0x02003105 RID: 12549
				public class TOOLTIPS
				{
					// Token: 0x0400C4DC RID: 50396
					public static LocString GROWTH_HALTED = "<b>Halted Growth</b>\nSubstandard conditions prevent these plants from growing";

					// Token: 0x0400C4DD RID: 50397
					public static LocString GROWING = "<b>Growing</b>\nThese plants are thriving in their current conditions";

					// Token: 0x0400C4DE RID: 50398
					public static LocString FULLY_GROWN = "<b>Fully Grown</b>\nThese plants have reached maturation\n\nSelect the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to batch harvest";
				}
			}

			// Token: 0x02002616 RID: 9750
			public class LIQUIDPLUMBING
			{
				// Token: 0x0400A726 RID: 42790
				public static LocString NAME = "PLUMBING OVERLAY";

				// Token: 0x0400A727 RID: 42791
				public static LocString BUTTON = "Plumbing Overlay";

				// Token: 0x0400A728 RID: 42792
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400A729 RID: 42793
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400A72A RID: 42794
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400A72B RID: 42795
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400A72C RID: 42796
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400A72D RID: 42797
				public static LocString NETWORK = "Liquid Network {0}";

				// Token: 0x0400A72E RID: 42798
				public static LocString DIAGRAM_BEFORE_ARROW = "Liquid flows from <b>Output Pipe</b>";

				// Token: 0x0400A72F RID: 42799
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003106 RID: 12550
				public class TOOLTIPS
				{
					// Token: 0x0400C4DF RID: 50399
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400C4E0 RID: 50400
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400C4E1 RID: 50401
					public static LocString CONSUMER = "<b>Output Pipe</b>\nOutputs send liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400C4E2 RID: 50402
					public static LocString FILTERED = "<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400C4E3 RID: 50403
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send liquid into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "LIQUIDPIPING");

					// Token: 0x0400C4E4 RID: 50404
					public static LocString NETWORK = "Liquid network {0}";
				}
			}

			// Token: 0x02002617 RID: 9751
			public class GASPLUMBING
			{
				// Token: 0x0400A730 RID: 42800
				public static LocString NAME = "VENTILATION OVERLAY";

				// Token: 0x0400A731 RID: 42801
				public static LocString BUTTON = "Ventilation Overlay";

				// Token: 0x0400A732 RID: 42802
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400A733 RID: 42803
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400A734 RID: 42804
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400A735 RID: 42805
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400A736 RID: 42806
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400A737 RID: 42807
				public static LocString NETWORK = "Gas Network {0}";

				// Token: 0x0400A738 RID: 42808
				public static LocString DIAGRAM_BEFORE_ARROW = "Gas flows from <b>Output Pipe</b>";

				// Token: 0x0400A739 RID: 42809
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003107 RID: 12551
				public class TOOLTIPS
				{
					// Token: 0x0400C4E5 RID: 50405
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400C4E6 RID: 50406
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400C4E7 RID: 50407
					public static LocString CONSUMER = string.Concat(new string[]
					{
						"<b>Output Pipe</b>\nOutputs send ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400C4E8 RID: 50408
					public static LocString FILTERED = string.Concat(new string[]
					{
						"<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400C4E9 RID: 50409
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send gas into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "GASPIPING");

					// Token: 0x0400C4EA RID: 50410
					public static LocString NETWORK = "Gas network {0}";
				}
			}

			// Token: 0x02002618 RID: 9752
			public class SUIT
			{
				// Token: 0x0400A73A RID: 42810
				public static LocString NAME = "EXOSUIT OVERLAY";

				// Token: 0x0400A73B RID: 42811
				public static LocString BUTTON = "Exosuit Overlay";

				// Token: 0x0400A73C RID: 42812
				public static LocString SUIT_ICON = "Exosuit";

				// Token: 0x0400A73D RID: 42813
				public static LocString SUIT_ICON_TOOLTIP = "<b>Exosuit</b>\nHighlights the current location of equippable exosuits";
			}

			// Token: 0x02002619 RID: 9753
			public class LOGIC
			{
				// Token: 0x0400A73E RID: 42814
				public static LocString NAME = "AUTOMATION OVERLAY";

				// Token: 0x0400A73F RID: 42815
				public static LocString BUTTON = "Automation Overlay";

				// Token: 0x0400A740 RID: 42816
				public static LocString INPUT = "Input Port";

				// Token: 0x0400A741 RID: 42817
				public static LocString OUTPUT = "Output Port";

				// Token: 0x0400A742 RID: 42818
				public static LocString RIBBON_INPUT = "Ribbon Input Port";

				// Token: 0x0400A743 RID: 42819
				public static LocString RIBBON_OUTPUT = "Ribbon Output Port";

				// Token: 0x0400A744 RID: 42820
				public static LocString RESET_UPDATE = "Reset Port";

				// Token: 0x0400A745 RID: 42821
				public static LocString CONTROL_INPUT = "Control Port";

				// Token: 0x0400A746 RID: 42822
				public static LocString CIRCUIT_STATUS_HEADER = "GRID STATUS";

				// Token: 0x0400A747 RID: 42823
				public static LocString ONE = "Green";

				// Token: 0x0400A748 RID: 42824
				public static LocString ZERO = "Red";

				// Token: 0x0400A749 RID: 42825
				public static LocString DISCONNECTED = "DISCONNECTED";

				// Token: 0x02003108 RID: 12552
				public abstract class TOOLTIPS
				{
					// Token: 0x0400C4EB RID: 50411
					public static LocString INPUT = "<b>Input Port</b>\nReceives a signal from an automation grid";

					// Token: 0x0400C4EC RID: 50412
					public static LocString OUTPUT = "<b>Output Port</b>\nSends a signal out to an automation grid";

					// Token: 0x0400C4ED RID: 50413
					public static LocString RIBBON_INPUT = "<b>Ribbon Input Port</b>\nReceives a 4-bit signal from an automation grid";

					// Token: 0x0400C4EE RID: 50414
					public static LocString RIBBON_OUTPUT = "<b>Ribbon Output Port</b>\nSends a 4-bit signal out to an automation grid";

					// Token: 0x0400C4EF RID: 50415
					public static LocString RESET_UPDATE = "<b>Reset Port</b>\nReset a " + BUILDINGS.PREFABS.LOGICMEMORY.NAME + "'s internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400C4F0 RID: 50416
					public static LocString CONTROL_INPUT = "<b>Control Port</b>\nControl the signal selection of a " + BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.NAME + " or " + BUILDINGS.PREFABS.LOGICGATEDEMULTIPLEXER.NAME;

					// Token: 0x0400C4F1 RID: 50417
					public static LocString ONE = "<b>Green</b>\nThis port is currently " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

					// Token: 0x0400C4F2 RID: 50418
					public static LocString ZERO = "<b>Red</b>\nThis port is currently " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400C4F3 RID: 50419
					public static LocString DISCONNECTED = "<b>Disconnected</b>\nThis port is not connected to an automation grid";
				}
			}

			// Token: 0x0200261A RID: 9754
			public class CONVEYOR
			{
				// Token: 0x0400A74A RID: 42826
				public static LocString NAME = "CONVEYOR OVERLAY";

				// Token: 0x0400A74B RID: 42827
				public static LocString BUTTON = "Conveyor Overlay";

				// Token: 0x0400A74C RID: 42828
				public static LocString OUTPUT = "Loader";

				// Token: 0x0400A74D RID: 42829
				public static LocString INPUT = "Receptacle";

				// Token: 0x02003109 RID: 12553
				public abstract class TOOLTIPS
				{
					// Token: 0x0400C4F4 RID: 50420
					public static LocString OUTPUT = string.Concat(new string[]
					{
						"<b>Loader</b>\nLoads material onto a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" for transport to Receptacles"
					});

					// Token: 0x0400C4F5 RID: 50421
					public static LocString INPUT = string.Concat(new string[]
					{
						"<b>Receptacle</b>\nReceives material from a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" and stores it for Duplicant use"
					});
				}
			}

			// Token: 0x0200261B RID: 9755
			public class DECOR
			{
				// Token: 0x0400A74E RID: 42830
				public static LocString NAME = "DECOR OVERLAY";

				// Token: 0x0400A74F RID: 42831
				public static LocString BUTTON = "Decor Overlay";

				// Token: 0x0400A750 RID: 42832
				public static LocString TOTAL = "Total Decor: ";

				// Token: 0x0400A751 RID: 42833
				public static LocString ENTRY = "{0} {1} {2}";

				// Token: 0x0400A752 RID: 42834
				public static LocString COUNT = "({0})";

				// Token: 0x0400A753 RID: 42835
				public static LocString VALUE = "{0}{1}";

				// Token: 0x0400A754 RID: 42836
				public static LocString VALUE_ZERO = "{0}{1}";

				// Token: 0x0400A755 RID: 42837
				public static LocString HEADER_POSITIVE = "Positive Value:";

				// Token: 0x0400A756 RID: 42838
				public static LocString HEADER_NEGATIVE = "Negative Value:";

				// Token: 0x0400A757 RID: 42839
				public static LocString LOWDECOR = "Negative Decor";

				// Token: 0x0400A758 RID: 42840
				public static LocString HIGHDECOR = "Positive Decor";

				// Token: 0x0400A759 RID: 42841
				public static LocString CLUTTER = "Debris";

				// Token: 0x0400A75A RID: 42842
				public static LocString LIGHTING = "Lighting";

				// Token: 0x0400A75B RID: 42843
				public static LocString CLOTHING = "{0}'s Outfit";

				// Token: 0x0400A75C RID: 42844
				public static LocString CLOTHING_TRAIT_DECORUP = "{0}'s Outfit (Innately Stylish)";

				// Token: 0x0400A75D RID: 42845
				public static LocString CLOTHING_TRAIT_DECORDOWN = "{0}'s Outfit (Shabby Dresser)";

				// Token: 0x0400A75E RID: 42846
				public static LocString HOVERTITLE = "DECOR";

				// Token: 0x0400A75F RID: 42847
				public static LocString MAXIMUM_DECOR = "{0}{1} (Maximum Decor)";

				// Token: 0x0200310A RID: 12554
				public class TOOLTIPS
				{
					// Token: 0x0400C4F6 RID: 50422
					public static LocString LOWDECOR = string.Concat(new string[]
					{
						"<b>Negative Decor</b>\nArea with insufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Resources on the floor are considered \"debris\" and will decrease decor"
					});

					// Token: 0x0400C4F7 RID: 50423
					public static LocString HIGHDECOR = string.Concat(new string[]
					{
						"<b>Positive Decor</b>\nArea with sufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Lighting and aesthetically pleasing buildings increase decor"
					});
				}
			}

			// Token: 0x0200261C RID: 9756
			public class PRIORITIES
			{
				// Token: 0x0400A760 RID: 42848
				public static LocString NAME = "PRIORITY OVERLAY";

				// Token: 0x0400A761 RID: 42849
				public static LocString BUTTON = "Priority Overlay";

				// Token: 0x0400A762 RID: 42850
				public static LocString ONE = "1 (Low Urgency)";

				// Token: 0x0400A763 RID: 42851
				public static LocString ONE_TOOLTIP = "Priority 1";

				// Token: 0x0400A764 RID: 42852
				public static LocString TWO = "2";

				// Token: 0x0400A765 RID: 42853
				public static LocString TWO_TOOLTIP = "Priority 2";

				// Token: 0x0400A766 RID: 42854
				public static LocString THREE = "3";

				// Token: 0x0400A767 RID: 42855
				public static LocString THREE_TOOLTIP = "Priority 3";

				// Token: 0x0400A768 RID: 42856
				public static LocString FOUR = "4";

				// Token: 0x0400A769 RID: 42857
				public static LocString FOUR_TOOLTIP = "Priority 4";

				// Token: 0x0400A76A RID: 42858
				public static LocString FIVE = "5";

				// Token: 0x0400A76B RID: 42859
				public static LocString FIVE_TOOLTIP = "Priority 5";

				// Token: 0x0400A76C RID: 42860
				public static LocString SIX = "6";

				// Token: 0x0400A76D RID: 42861
				public static LocString SIX_TOOLTIP = "Priority 6";

				// Token: 0x0400A76E RID: 42862
				public static LocString SEVEN = "7";

				// Token: 0x0400A76F RID: 42863
				public static LocString SEVEN_TOOLTIP = "Priority 7";

				// Token: 0x0400A770 RID: 42864
				public static LocString EIGHT = "8";

				// Token: 0x0400A771 RID: 42865
				public static LocString EIGHT_TOOLTIP = "Priority 8";

				// Token: 0x0400A772 RID: 42866
				public static LocString NINE = "9 (High Urgency)";

				// Token: 0x0400A773 RID: 42867
				public static LocString NINE_TOOLTIP = "Priority 9";
			}

			// Token: 0x0200261D RID: 9757
			public class DISEASE
			{
				// Token: 0x0400A774 RID: 42868
				public static LocString NAME = "GERM OVERLAY";

				// Token: 0x0400A775 RID: 42869
				public static LocString BUTTON = "Germ Overlay";

				// Token: 0x0400A776 RID: 42870
				public static LocString HOVERTITLE = "Germ";

				// Token: 0x0400A777 RID: 42871
				public static LocString INFECTION_SOURCE = "Germ Source";

				// Token: 0x0400A778 RID: 42872
				public static LocString INFECTION_SOURCE_TOOLTIP = "<b>Germ Source</b>\nAreas where germs are produced\n* Placing Wash Basins or Hand Sanitizers near these areas may prevent disease spread";

				// Token: 0x0400A779 RID: 42873
				public static LocString NO_DISEASE = "Zero surface germs";

				// Token: 0x0400A77A RID: 42874
				public static LocString DISEASE_NAME_FORMAT = "{0}<color=#{1}></color>";

				// Token: 0x0400A77B RID: 42875
				public static LocString DISEASE_NAME_FORMAT_NO_COLOR = "{0}";

				// Token: 0x0400A77C RID: 42876
				public static LocString DISEASE_FORMAT = "{1} [{0}]<color=#{2}></color>";

				// Token: 0x0400A77D RID: 42877
				public static LocString DISEASE_FORMAT_NO_COLOR = "{1} [{0}]";

				// Token: 0x0400A77E RID: 42878
				public static LocString CONTAINER_FORMAT = "\n    {0}: {1}";

				// Token: 0x0200310B RID: 12555
				public class DISINFECT_THRESHOLD_DIAGRAM
				{
					// Token: 0x0400C4F8 RID: 50424
					public static LocString UNITS = "Germs";

					// Token: 0x0400C4F9 RID: 50425
					public static LocString MIN_LABEL = "0";

					// Token: 0x0400C4FA RID: 50426
					public static LocString MAX_LABEL = "1m";

					// Token: 0x0400C4FB RID: 50427
					public static LocString THRESHOLD_PREFIX = "Disinfect At:";

					// Token: 0x0400C4FC RID: 50428
					public static LocString TOOLTIP = "Automatically disinfect any building with more than {NumberOfGerms} germs.";

					// Token: 0x0400C4FD RID: 50429
					public static LocString TOOLTIP_DISABLED = "Automatic building disinfection disabled.";
				}
			}

			// Token: 0x0200261E RID: 9758
			public class CROPS
			{
				// Token: 0x0400A77F RID: 42879
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400A780 RID: 42880
				public static LocString BUTTON = "Farming Overlay";
			}

			// Token: 0x0200261F RID: 9759
			public class POWER
			{
				// Token: 0x0400A781 RID: 42881
				public static LocString WATTS_GENERATED = "Watts Generated";

				// Token: 0x0400A782 RID: 42882
				public static LocString WATTS_CONSUMED = "Watts Consumed";
			}

			// Token: 0x02002620 RID: 9760
			public class RADIATION
			{
				// Token: 0x0400A783 RID: 42883
				public static LocString NAME = "RADIATION";

				// Token: 0x0400A784 RID: 42884
				public static LocString BUTTON = "Radiation Overlay";

				// Token: 0x0400A785 RID: 42885
				public static LocString DESC = "{rads} per cycle ({description})";

				// Token: 0x0400A786 RID: 42886
				public static LocString SHIELDING_DESC = "Radiation Blocking: {radiationAbsorptionFactor}";

				// Token: 0x0400A787 RID: 42887
				public static LocString HOVERTITLE = "RADIATION";

				// Token: 0x0200310C RID: 12556
				public class RANGES
				{
					// Token: 0x0400C4FE RID: 50430
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400C4FF RID: 50431
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400C500 RID: 50432
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400C501 RID: 50433
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400C502 RID: 50434
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400C503 RID: 50435
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400C504 RID: 50436
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400C505 RID: 50437
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400C506 RID: 50438
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}

				// Token: 0x0200310D RID: 12557
				public class TOOLTIPS
				{
					// Token: 0x0400C507 RID: 50439
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400C508 RID: 50440
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400C509 RID: 50441
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400C50A RID: 50442
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400C50B RID: 50443
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400C50C RID: 50444
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400C50D RID: 50445
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400C50E RID: 50446
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400C50F RID: 50447
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}
			}
		}

		// Token: 0x02001DB3 RID: 7603
		public class TABLESCREENS
		{
			// Token: 0x0400876B RID: 34667
			public static LocString DUPLICANT_PROPERNAME = "<b>{0}</b>";

			// Token: 0x0400876C RID: 34668
			public static LocString SELECT_DUPLICANT_BUTTON = UI.CLICK(UI.ClickType.Click) + " to select <b>{0}</b>";

			// Token: 0x0400876D RID: 34669
			public static LocString GOTO_DUPLICANT_BUTTON = "Double-" + UI.CLICK(UI.ClickType.click) + " to go to <b>{0}</b>";

			// Token: 0x0400876E RID: 34670
			public static LocString COLUMN_SORT_BY_NAME = "Sort by <b>Name</b>";

			// Token: 0x0400876F RID: 34671
			public static LocString COLUMN_SORT_BY_STRESS = "Sort by <b>Stress</b>";

			// Token: 0x04008770 RID: 34672
			public static LocString COLUMN_SORT_BY_HITPOINTS = "Sort by <b>Health</b>";

			// Token: 0x04008771 RID: 34673
			public static LocString COLUMN_SORT_BY_SICKNESSES = "Sort by <b>Disease</b>";

			// Token: 0x04008772 RID: 34674
			public static LocString COLUMN_SORT_BY_FULLNESS = "Sort by <b>Fullness</b>";

			// Token: 0x04008773 RID: 34675
			public static LocString COLUMN_SORT_BY_EATEN_TODAY = "Sort by number of <b>Calories</b> consumed today";

			// Token: 0x04008774 RID: 34676
			public static LocString COLUMN_SORT_BY_EXPECTATIONS = "Sort by <b>Morale</b>";

			// Token: 0x04008775 RID: 34677
			public static LocString NA = "N/A";

			// Token: 0x04008776 RID: 34678
			public static LocString INFORMATION_NOT_AVAILABLE_TOOLTIP = "Information is not available because {1} is in {0}";

			// Token: 0x04008777 RID: 34679
			public static LocString NOBODY_HERE = "Nobody here...";
		}

		// Token: 0x02001DB4 RID: 7604
		public class CONSUMABLESSCREEN
		{
			// Token: 0x04008778 RID: 34680
			public static LocString TITLE = "CONSUMABLES";

			// Token: 0x04008779 RID: 34681
			public static LocString TOOLTIP_TOGGLE_ALL = "Toggle <b>all</b> food permissions <b>colonywide</b>";

			// Token: 0x0400877A RID: 34682
			public static LocString TOOLTIP_TOGGLE_COLUMN = "Toggle colonywide <b>{0}</b> permission";

			// Token: 0x0400877B RID: 34683
			public static LocString TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>{0}</b>";

			// Token: 0x0400877C RID: 34684
			public static LocString NEW_MINIONS_TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>New Duplicants</b>";

			// Token: 0x0400877D RID: 34685
			public static LocString NEW_MINIONS_FOOD_PERMISSION_ON = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				"</b> by default"
			});

			// Token: 0x0400877E RID: 34686
			public static LocString NEW_MINIONS_FOOD_PERMISSION_OFF = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>not allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" by default"
			});

			// Token: 0x0400877F RID: 34687
			public static LocString FOOD_PERMISSION_ON = "<b>{0}</b> is <b>allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04008780 RID: 34688
			public static LocString FOOD_PERMISSION_OFF = "<b>{0}</b> is <b>not allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04008781 RID: 34689
			public static LocString FOOD_CANT_CONSUME = "<b>{0}</b> <b>physically cannot</b> eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04008782 RID: 34690
			public static LocString FOOD_REFUSE = "<b>{0}</b> <b>refuses</b> to eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04008783 RID: 34691
			public static LocString FOOD_AVAILABLE = "Available: {0}";

			// Token: 0x04008784 RID: 34692
			public static LocString FOOD_MORALE = UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x04008785 RID: 34693
			public static LocString FOOD_QUALITY = UI.PRE_KEYWORD + "Quality" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x04008786 RID: 34694
			public static LocString FOOD_QUALITY_VS_EXPECTATION = string.Concat(new string[]
			{
				"\nThis food will give ",
				UI.PRE_KEYWORD,
				"Morale",
				UI.PST_KEYWORD,
				" <b>{0}</b> if {1} eats it"
			});

			// Token: 0x04008787 RID: 34695
			public static LocString CANNOT_ADJUST_PERMISSIONS = "Cannot adjust consumable permissions because they're in {0}";
		}

		// Token: 0x02001DB5 RID: 7605
		public class JOBSSCREEN
		{
			// Token: 0x04008788 RID: 34696
			public static LocString TITLE = "MANAGE DUPLICANT PRIORITIES";

			// Token: 0x04008789 RID: 34697
			public static LocString TOOLTIP_TOGGLE_ALL = "Set priority of all Errand Types colonywide";

			// Token: 0x0400878A RID: 34698
			public static LocString HEADER_TOOLTIP = string.Concat(new string[]
			{
				"<size=16>{Job} Errand Type</size>\n\n{Details}\n\nDuplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform based on ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				",\nthen they will choose individual tasks within that type using ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsLink("Priority Tool", "PRIORITIES"),
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities)
			});

			// Token: 0x0400878B RID: 34699
			public static LocString HEADER_DETAILS_TOOLTIP = "{Description}\n\nAffected errands: {ChoreList}";

			// Token: 0x0400878C RID: 34700
			public static LocString HEADER_CHANGE_TOOLTIP = string.Concat(new string[]
			{
				"Set the priority for the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type colonywide\n"
			});

			// Token: 0x0400878D RID: 34701
			public static LocString NEW_MINION_ITEM_TOOLTIP = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type is automatically a {Priority} ",
				UI.PRE_KEYWORD,
				"Priority",
				UI.PST_KEYWORD,
				" for <b>Arriving Duplicants</b>"
			});

			// Token: 0x0400878E RID: 34702
			public static LocString ITEM_TOOLTIP = UI.PRE_KEYWORD + "{Job}" + UI.PST_KEYWORD + " Priority for {Name}:\n<b>{Priority} Priority ({PriorityValue})</b>";

			// Token: 0x0400878F RID: 34703
			public static LocString MINION_SKILL_TOOLTIP = string.Concat(new string[]
			{
				"{Name}'s ",
				UI.PRE_KEYWORD,
				"{Attribute}",
				UI.PST_KEYWORD,
				" Skill: "
			});

			// Token: 0x04008790 RID: 34704
			public static LocString TRAIT_DISABLED = string.Concat(new string[]
			{
				"{Name} possesses the ",
				UI.PRE_KEYWORD,
				"{Trait}",
				UI.PST_KEYWORD,
				" trait and <b>cannot</b> do ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x04008791 RID: 34705
			public static LocString INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x04008792 RID: 34706
			public static LocString DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x04008793 RID: 34707
			public static LocString INCREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x04008794 RID: 34708
			public static LocString DECREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x04008795 RID: 34709
			public static LocString INCREASE_PRIORITY_TUTORIAL = "{Hotkey} Increase Priority";

			// Token: 0x04008796 RID: 34710
			public static LocString DECREASE_PRIORITY_TUTORIAL = "{Hotkey} Decrease Priority";

			// Token: 0x04008797 RID: 34711
			public static LocString CANNOT_ADJUST_PRIORITY = string.Concat(new string[]
			{
				"Priorities for ",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" cannot be adjusted currently because they're in {1}"
			});

			// Token: 0x04008798 RID: 34712
			public static LocString SORT_TOOLTIP = string.Concat(new string[]
			{
				"Sort by the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type"
			});

			// Token: 0x04008799 RID: 34713
			public static LocString DISABLED_TOOLTIP = string.Concat(new string[]
			{
				"{Name} may not perform ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400879A RID: 34714
			public static LocString OPTIONS = "Options";

			// Token: 0x0400879B RID: 34715
			public static LocString TOGGLE_ADVANCED_MODE = "Enable Proximity";

			// Token: 0x0400879C RID: 34716
			public static LocString TOGGLE_ADVANCED_MODE_TOOLTIP = "<b>Errand Proximity Settings</b>\n\nEnabling Proximity settings tells my Duplicants to always choose the closest, most urgent errand to perform.\n\nWhen disabled, Duplicants will choose between two high priority errands based on a hidden priority hierarchy instead.\n\nEnabling Proximity helps cut down on travel time in areas with lots of high priority errands, and is useful for large colonies.";

			// Token: 0x0400879D RID: 34717
			public static LocString RESET_SETTINGS = "Reset Priorities";

			// Token: 0x0400879E RID: 34718
			public static LocString RESET_SETTINGS_TOOLTIP = "<b>Reset Priorities</b>\n\nReturns all priorities to their default values.\n\nProximity Enabled: Priorities will be adjusted high-to-low.\n\nProximity Disabled: All priorities will be reset to neutral.";

			// Token: 0x02002621 RID: 9761
			public class PRIORITY
			{
				// Token: 0x0400A788 RID: 42888
				public static LocString VERYHIGH = "Very High";

				// Token: 0x0400A789 RID: 42889
				public static LocString HIGH = "High";

				// Token: 0x0400A78A RID: 42890
				public static LocString STANDARD = "Standard";

				// Token: 0x0400A78B RID: 42891
				public static LocString LOW = "Low";

				// Token: 0x0400A78C RID: 42892
				public static LocString VERYLOW = "Very Low";

				// Token: 0x0400A78D RID: 42893
				public static LocString DISABLED = "Disallowed";
			}

			// Token: 0x02002622 RID: 9762
			public class PRIORITY_CLASS
			{
				// Token: 0x0400A78E RID: 42894
				public static LocString IDLE = "Idle";

				// Token: 0x0400A78F RID: 42895
				public static LocString BASIC = "Normal";

				// Token: 0x0400A790 RID: 42896
				public static LocString HIGH = "Urgent";

				// Token: 0x0400A791 RID: 42897
				public static LocString PERSONAL_NEEDS = "Personal Needs";

				// Token: 0x0400A792 RID: 42898
				public static LocString EMERGENCY = "Emergency";

				// Token: 0x0400A793 RID: 42899
				public static LocString COMPULSORY = "Involuntary";
			}
		}

		// Token: 0x02001DB6 RID: 7606
		public class VITALSSCREEN
		{
			// Token: 0x0400879F RID: 34719
			public static LocString HEALTH = "Health";

			// Token: 0x040087A0 RID: 34720
			public static LocString SICKNESS = "Diseases";

			// Token: 0x040087A1 RID: 34721
			public static LocString NO_SICKNESSES = "No diseases";

			// Token: 0x040087A2 RID: 34722
			public static LocString MULTIPLE_SICKNESSES = "Multiple diseases ({0})";

			// Token: 0x040087A3 RID: 34723
			public static LocString SICKNESS_REMAINING = "{0}\n({1})";

			// Token: 0x040087A4 RID: 34724
			public static LocString STRESS = "Stress";

			// Token: 0x040087A5 RID: 34725
			public static LocString EXPECTATIONS = "Expectations";

			// Token: 0x040087A6 RID: 34726
			public static LocString CALORIES = "Fullness";

			// Token: 0x040087A7 RID: 34727
			public static LocString EATEN_TODAY = "Eaten Today";

			// Token: 0x040087A8 RID: 34728
			public static LocString EATEN_TODAY_TOOLTIP = "Consumed {0} of food this cycle";

			// Token: 0x040087A9 RID: 34729
			public static LocString ATMOSPHERE_CONDITION = "Atmosphere:";

			// Token: 0x040087AA RID: 34730
			public static LocString SUBMERSION = "Liquid Level";

			// Token: 0x040087AB RID: 34731
			public static LocString NOT_DROWNING = "Liquid Level";

			// Token: 0x040087AC RID: 34732
			public static LocString FOOD_EXPECTATIONS = "Food Expectation";

			// Token: 0x040087AD RID: 34733
			public static LocString FOOD_EXPECTATIONS_TOOLTIP = "This Duplicant desires food that is {0} quality or better";

			// Token: 0x040087AE RID: 34734
			public static LocString DECOR_EXPECTATIONS = "Decor Expectation";

			// Token: 0x040087AF RID: 34735
			public static LocString DECOR_EXPECTATIONS_TOOLTIP = "This Duplicant desires decor that is {0} or higher";

			// Token: 0x040087B0 RID: 34736
			public static LocString QUALITYOFLIFE_EXPECTATIONS = "Morale";

			// Token: 0x040087B1 RID: 34737
			public static LocString QUALITYOFLIFE_EXPECTATIONS_TOOLTIP = "This Duplicant requires " + UI.FormatAsLink("{0} Morale", "MORALE") + ".\n\nCurrent Morale:";

			// Token: 0x02002623 RID: 9763
			public class CONDITIONS_GROWING
			{
				// Token: 0x0200310E RID: 12558
				public class WILD
				{
					// Token: 0x0400C510 RID: 50448
					public static LocString BASE = "<b>Wild Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400C511 RID: 50449
					public static LocString TOOLTIP = "This plant will take {0} to grow in the wild";
				}

				// Token: 0x0200310F RID: 12559
				public class DOMESTIC
				{
					// Token: 0x0400C512 RID: 50450
					public static LocString BASE = "<b>Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400C513 RID: 50451
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02003110 RID: 12560
				public class ADDITIONAL_DOMESTIC
				{
					// Token: 0x0400C514 RID: 50452
					public static LocString BASE = "<b>Additional Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400C515 RID: 50453
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02003111 RID: 12561
				public class WILD_DECOR
				{
					// Token: 0x0400C516 RID: 50454
					public static LocString BASE = "<b>Wild Growth</b>";

					// Token: 0x0400C517 RID: 50455
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02003112 RID: 12562
				public class WILD_INSTANT
				{
					// Token: 0x0400C518 RID: 50456
					public static LocString BASE = "<b>Wild Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400C519 RID: 50457
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02003113 RID: 12563
				public class ADDITIONAL_DOMESTIC_INSTANT
				{
					// Token: 0x0400C51A RID: 50458
					public static LocString BASE = "<b>Domestic Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400C51B RID: 50459
					public static LocString TOOLTIP = "This plant must have these requirements met to grow domestically";
				}
			}
		}

		// Token: 0x02001DB7 RID: 7607
		public class SCHEDULESCREEN
		{
			// Token: 0x040087B2 RID: 34738
			public static LocString SCHEDULE_EDITOR = "Schedule Editor";

			// Token: 0x040087B3 RID: 34739
			public static LocString SCHEDULE_NAME_DEFAULT = "Default Schedule";

			// Token: 0x040087B4 RID: 34740
			public static LocString SCHEDULE_NAME_FORMAT = "Schedule {0}";

			// Token: 0x040087B5 RID: 34741
			public static LocString SCHEDULE_DROPDOWN_ASSIGNED = "{0} (Assigned)";

			// Token: 0x040087B6 RID: 34742
			public static LocString SCHEDULE_DROPDOWN_BLANK = "<i>Move Duplicant...</i>";

			// Token: 0x040087B7 RID: 34743
			public static LocString SCHEDULE_DOWNTIME_MORALE = "Duplicants will receive {0} Morale from the scheduled Downtime shifts";

			// Token: 0x040087B8 RID: 34744
			public static LocString RENAME_BUTTON_TOOLTIP = "Rename custom schedule";

			// Token: 0x040087B9 RID: 34745
			public static LocString ALARM_BUTTON_ON_TOOLTIP = "Toggle Notifications\n\nSounds and notifications will play when shifts change for this schedule.\n\nENABLED\n" + UI.CLICK(UI.ClickType.Click) + " to disable";

			// Token: 0x040087BA RID: 34746
			public static LocString ALARM_BUTTON_OFF_TOOLTIP = "Toggle Notifications\n\nNo sounds or notifications will play for this schedule.\n\nDISABLED\n" + UI.CLICK(UI.ClickType.Click) + " to enable";

			// Token: 0x040087BB RID: 34747
			public static LocString DELETE_BUTTON_TOOLTIP = "Delete Schedule";

			// Token: 0x040087BC RID: 34748
			public static LocString PAINT_TOOLS = "Paint Tools:";

			// Token: 0x040087BD RID: 34749
			public static LocString ADD_SCHEDULE = "Add New Schedule";

			// Token: 0x040087BE RID: 34750
			public static LocString POO = "dar";

			// Token: 0x040087BF RID: 34751
			public static LocString DOWNTIME_MORALE = "Downtime Morale: {0}";

			// Token: 0x040087C0 RID: 34752
			public static LocString ALARM_TITLE_ENABLED = "Alarm On";

			// Token: 0x040087C1 RID: 34753
			public static LocString ALARM_TITLE_DISABLED = "Alarm Off";

			// Token: 0x040087C2 RID: 34754
			public static LocString SETTINGS = "Settings";

			// Token: 0x040087C3 RID: 34755
			public static LocString ALARM_BUTTON = "Shift Alarms";

			// Token: 0x040087C4 RID: 34756
			public static LocString RESET_SETTINGS = "Reset Shifts";

			// Token: 0x040087C5 RID: 34757
			public static LocString RESET_SETTINGS_TOOLTIP = "Restore this schedule to default shifts";

			// Token: 0x040087C6 RID: 34758
			public static LocString DELETE_SCHEDULE = "Delete Schedule";

			// Token: 0x040087C7 RID: 34759
			public static LocString DELETE_SCHEDULE_TOOLTIP = "Remove this schedule and unassign all Duplicants from it";

			// Token: 0x040087C8 RID: 34760
			public static LocString DUPLICANT_NIGHTOWL_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.NIGHTOWL.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+3</b> at night"
			});

			// Token: 0x040087C9 RID: 34761
			public static LocString DUPLICANT_EARLYBIRD_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.EARLYBIRD.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+2</b> in the morning"
			});
		}

		// Token: 0x02001DB8 RID: 7608
		public class COLONYLOSTSCREEN
		{
			// Token: 0x040087CA RID: 34762
			public static LocString COLONYLOST = "COLONY LOST";

			// Token: 0x040087CB RID: 34763
			public static LocString COLONYLOSTDESCRIPTION = "All Duplicants are dead or incapacitated.";

			// Token: 0x040087CC RID: 34764
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to return to a previous colony, or begin a new one.";

			// Token: 0x040087CD RID: 34765
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x040087CE RID: 34766
			public static LocString QUITBUTTON = "MAIN MENU";
		}

		// Token: 0x02001DB9 RID: 7609
		public class VICTORYSCREEN
		{
			// Token: 0x040087CF RID: 34767
			public static LocString HEADER = "SUCCESS: IMPERATIVE ACHIEVED!";

			// Token: 0x040087D0 RID: 34768
			public static LocString DESCRIPTION = "I have fulfilled the conditions of one of my Hardwired Imperatives";

			// Token: 0x040087D1 RID: 34769
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to retire the colony and begin anew.";

			// Token: 0x040087D2 RID: 34770
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x040087D3 RID: 34771
			public static LocString RETIREBUTTON = "RETIRE COLONY";
		}

		// Token: 0x02001DBA RID: 7610
		public class GENESHUFFLERMESSAGE
		{
			// Token: 0x040087D4 RID: 34772
			public static LocString HEADER = "NEURAL VACILLATION COMPLETE";

			// Token: 0x040087D5 RID: 34773
			public static LocString BODY_SUCCESS = "Whew! <b>{0}'s</b> brain is still vibrating, but they've never felt better!\n\n<b>{0}</b> acquired the <b>{1}</b> trait.\n\n<b>{1}:</b>\n{2}";

			// Token: 0x040087D6 RID: 34774
			public static LocString BODY_FAILURE = "The machine attempted to alter this Duplicant, but there's no improving on perfection.\n\n<b>{0}</b> already has all positive traits!";

			// Token: 0x040087D7 RID: 34775
			public static LocString DISMISSBUTTON = "DISMISS";
		}

		// Token: 0x02001DBB RID: 7611
		public class CRASHSCREEN
		{
			// Token: 0x040087D8 RID: 34776
			public static LocString TITLE = "\"Whoops! We're sorry, but it seems your game has encountered an error. It's okay though - these errors are how we find and fix problems to make our game more fun for everyone. If you use the box below to submit a crash report to us, we can use this information to get the issue sorted out.\"";

			// Token: 0x040087D9 RID: 34777
			public static LocString TITLE_MODS = "\"Oops-a-daisy! We're sorry, but it seems your game has encountered an error. If you uncheck all of the mods below, we will be able to help the next time this happens. Any mods that could be related to this error have already been unchecked.\"";

			// Token: 0x040087DA RID: 34778
			public static LocString HEADER = "OPTIONAL CRASH DESCRIPTION";

			// Token: 0x040087DB RID: 34779
			public static LocString HEADER_MODS = "ACTIVE MODS";

			// Token: 0x040087DC RID: 34780
			public static LocString BODY = "Help! A black hole ate my game!";

			// Token: 0x040087DD RID: 34781
			public static LocString THANKYOU = "Thank you!\n\nYou're making our game better, one crash at a time.";

			// Token: 0x040087DE RID: 34782
			public static LocString UPLOAD_FAILED = "There was an issue in reporting this crash.\n\nPlease submit a bug report at:\n<u>https://forums.kleientertainment.com/klei-bug-tracker/oni/</u>";

			// Token: 0x040087DF RID: 34783
			public static LocString UPLOADINFO = "UPLOAD ADDITIONAL INFO ({0})";

			// Token: 0x040087E0 RID: 34784
			public static LocString REPORTBUTTON = "REPORT CRASH";

			// Token: 0x040087E1 RID: 34785
			public static LocString REPORTING = "REPORTING, PLEASE WAIT...";

			// Token: 0x040087E2 RID: 34786
			public static LocString CONTINUEBUTTON = "CONTINUE GAME";

			// Token: 0x040087E3 RID: 34787
			public static LocString MOREINFOBUTTON = "MORE INFO";

			// Token: 0x040087E4 RID: 34788
			public static LocString COPYTOCLIPBOARDBUTTON = "COPY TO CLIPBOARD";

			// Token: 0x040087E5 RID: 34789
			public static LocString QUITBUTTON = "QUIT TO DESKTOP";

			// Token: 0x040087E6 RID: 34790
			public static LocString SAVEFAILED = "Save Failed: {0}";

			// Token: 0x040087E7 RID: 34791
			public static LocString LOADFAILED = "Load Failed: {0}\nSave Version: {1}\nExpected: {2}";

			// Token: 0x040087E8 RID: 34792
			public static LocString REPORTEDERROR_SUCCESS = "Reported Error";

			// Token: 0x040087E9 RID: 34793
			public static LocString REPORTEDERROR_FAILURE = "Unable to report error. Please contact us using the bug tracker.";

			// Token: 0x040087EA RID: 34794
			public static LocString UPLOADINPROGRESS = "Submitting {0}";
		}

		// Token: 0x02001DBC RID: 7612
		public class DEMOOVERSCREEN
		{
			// Token: 0x040087EB RID: 34795
			public static LocString TIMEREMAINING = "Demo time remaining:";

			// Token: 0x040087EC RID: 34796
			public static LocString TIMERTOOLTIP = "Demo time remaining";

			// Token: 0x040087ED RID: 34797
			public static LocString TIMERINACTIVE = "Timer inactive";

			// Token: 0x040087EE RID: 34798
			public static LocString DEMOOVER = "END OF DEMO";

			// Token: 0x040087EF RID: 34799
			public static LocString DESCRIPTION = "Thank you for playing <color=#F44A47>Oxygen Not Included</color>!";

			// Token: 0x040087F0 RID: 34800
			public static LocString DESCRIPTION_2 = "";

			// Token: 0x040087F1 RID: 34801
			public static LocString QUITBUTTON = "RESET";
		}

		// Token: 0x02001DBD RID: 7613
		public class CREDITSSCREEN
		{
			// Token: 0x040087F2 RID: 34802
			public static LocString TITLE = "CREDITS";

			// Token: 0x040087F3 RID: 34803
			public static LocString CLOSEBUTTON = "CLOSE";

			// Token: 0x02002624 RID: 9764
			public class THIRD_PARTY
			{
				// Token: 0x0400A794 RID: 42900
				public static LocString FMOD = "FMOD Sound System\nCopyright Firelight Technologies";

				// Token: 0x0400A795 RID: 42901
				public static LocString HARMONY = "Harmony by Andreas Pardeike";
			}
		}

		// Token: 0x02001DBE RID: 7614
		public class ALLRESOURCESSCREEN
		{
			// Token: 0x040087F4 RID: 34804
			public static LocString RESOURCES_TITLE = "RESOURCES";

			// Token: 0x040087F5 RID: 34805
			public static LocString RESOURCES = "Resources";

			// Token: 0x040087F6 RID: 34806
			public static LocString SEARCH = "Search";

			// Token: 0x040087F7 RID: 34807
			public static LocString NAME = "Resource";

			// Token: 0x040087F8 RID: 34808
			public static LocString TOTAL = "Total";

			// Token: 0x040087F9 RID: 34809
			public static LocString AVAILABLE = "Available";

			// Token: 0x040087FA RID: 34810
			public static LocString RESERVED = "Reserved";

			// Token: 0x040087FB RID: 34811
			public static LocString SEARCH_PLACEHODLER = "Enter text...";

			// Token: 0x040087FC RID: 34812
			public static LocString FIRST_FRAME_NO_DATA = "...";

			// Token: 0x040087FD RID: 34813
			public static LocString PIN_TOOLTIP = "Check to pin resource to side panel";

			// Token: 0x040087FE RID: 34814
			public static LocString UNPIN_TOOLTIP = "Unpin resource";
		}

		// Token: 0x02001DBF RID: 7615
		public class PRIORITYSCREEN
		{
			// Token: 0x040087FF RID: 34815
			public static LocString BASIC = "Set the order in which specific pending errands should be done\n\n1: Least Urgent\n9: Most Urgent";

			// Token: 0x04008800 RID: 34816
			public static LocString HIGH = "";

			// Token: 0x04008801 RID: 34817
			public static LocString TOP_PRIORITY = "Top Priority\n\nThis priority will override all other priorities and set the colony on Yellow Alert until the errand is completed";

			// Token: 0x04008802 RID: 34818
			public static LocString HIGH_TOGGLE = "";

			// Token: 0x04008803 RID: 34819
			public static LocString OPEN_JOBS_SCREEN = string.Concat(new string[]
			{
				UI.CLICK(UI.ClickType.Click),
				" to open the Priorities Screen\n\nDuplicants will first decide what to work on based on their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				", and then decide where to work based on ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD
			});

			// Token: 0x04008804 RID: 34820
			public static LocString DIAGRAM = string.Concat(new string[]
			{
				"Duplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform using their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				"\n\nThey will then choose one ",
				UI.PRE_KEYWORD,
				"Errand",
				UI.PST_KEYWORD,
				" from within that type using the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x04008805 RID: 34821
			public static LocString DIAGRAM_TITLE = "BUILDING PRIORITY";
		}

		// Token: 0x02001DC0 RID: 7616
		public class RESOURCESCREEN
		{
			// Token: 0x04008806 RID: 34822
			public static LocString HEADER = "RESOURCES";

			// Token: 0x04008807 RID: 34823
			public static LocString CATEGORY_TOOLTIP = "Counts all unallocated resources within reach\n\n" + UI.CLICK(UI.ClickType.Click) + " to expand";

			// Token: 0x04008808 RID: 34824
			public static LocString AVAILABLE_TOOLTIP = "Available: <b>{0}</b>\n({1} of {2} allocated to pending errands)";

			// Token: 0x04008809 RID: 34825
			public static LocString TREND_TOOLTIP = "The available amount of this resource has {0} {1} in the last cycle";

			// Token: 0x0400880A RID: 34826
			public static LocString TREND_TOOLTIP_NO_CHANGE = "The available amount of this resource has NOT CHANGED in the last cycle";

			// Token: 0x0400880B RID: 34827
			public static LocString FLAT_STR = "<b>NOT CHANGED</b>";

			// Token: 0x0400880C RID: 34828
			public static LocString INCREASING_STR = "<color=" + Constants.POSITIVE_COLOR_STR + ">INCREASED</color>";

			// Token: 0x0400880D RID: 34829
			public static LocString DECREASING_STR = "<color=" + Constants.NEGATIVE_COLOR_STR + ">DECREASED</color>";

			// Token: 0x0400880E RID: 34830
			public static LocString CLEAR_NEW_RESOURCES = "Clear New";

			// Token: 0x0400880F RID: 34831
			public static LocString CLEAR_ALL = "Unpin all resources";

			// Token: 0x04008810 RID: 34832
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x04008811 RID: 34833
			public static LocString NEW_TAG = "NEW";
		}

		// Token: 0x02001DC1 RID: 7617
		public class CONFIRMDIALOG
		{
			// Token: 0x04008812 RID: 34834
			public static LocString OK = "OK";

			// Token: 0x04008813 RID: 34835
			public static LocString CANCEL = "CANCEL";

			// Token: 0x04008814 RID: 34836
			public static LocString DIALOG_HEADER = "MESSAGE";
		}

		// Token: 0x02001DC2 RID: 7618
		public class FACADE_SELECTION_PANEL
		{
			// Token: 0x04008815 RID: 34837
			public static LocString HEADER = "Select Blueprint";

			// Token: 0x04008816 RID: 34838
			public static LocString STORE_BUTTON_TOOLTIP = "More Blueprints";
		}

		// Token: 0x02001DC3 RID: 7619
		public class FILE_NAME_DIALOG
		{
			// Token: 0x04008817 RID: 34839
			public static LocString ENTER_TEXT = "Enter Text...";
		}

		// Token: 0x02001DC4 RID: 7620
		public class MINION_IDENTITY_SORT
		{
			// Token: 0x04008818 RID: 34840
			public static LocString TITLE = "Sort By";

			// Token: 0x04008819 RID: 34841
			public static LocString NAME = "Duplicant";

			// Token: 0x0400881A RID: 34842
			public static LocString ROLE = "Role";

			// Token: 0x0400881B RID: 34843
			public static LocString PERMISSION = "Permission";
		}

		// Token: 0x02001DC5 RID: 7621
		public class UISIDESCREENS
		{
			// Token: 0x02002625 RID: 9765
			public class ARTABLESELECTIONSIDESCREEN
			{
				// Token: 0x0400A796 RID: 42902
				public static LocString TITLE = "Style Selection";

				// Token: 0x0400A797 RID: 42903
				public static LocString BUTTON = "Redecorate";

				// Token: 0x0400A798 RID: 42904
				public static LocString BUTTON_TOOLTIP = "Clears current artwork\n\nCreates errand for a skilled Duplicant to create selected style";

				// Token: 0x0400A799 RID: 42905
				public static LocString CLEAR_BUTTON_TOOLTIP = "Clears current artwork\n\nAllows a skilled Duplicant to create artwork of their choice";
			}

			// Token: 0x02002626 RID: 9766
			public class ARTIFACTANALYSISSIDESCREEN
			{
				// Token: 0x0400A79A RID: 42906
				public static LocString NO_ARTIFACTS_DISCOVERED = "No artifacts analyzed";

				// Token: 0x0400A79B RID: 42907
				public static LocString NO_ARTIFACTS_DISCOVERED_TOOLTIP = "Analyzing artifacts requires a Duplicant with the Masterworks skill";
			}

			// Token: 0x02002627 RID: 9767
			public class BUTTONMENUSIDESCREEN
			{
				// Token: 0x0400A79C RID: 42908
				public static LocString TITLE = "Building Menu";

				// Token: 0x0400A79D RID: 42909
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR = "Enable Auto-Delivery";

				// Token: 0x0400A79E RID: 42910
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Order Duplicants to deliver {0}" + UI.FormatAsLink("s", "NONE") + " to this building automatically when they need replacing";

				// Token: 0x0400A79F RID: 42911
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR = "Cancel Auto-Delivery";

				// Token: 0x0400A7A0 RID: 42912
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Cancel automatic {0} deliveries to this building";
			}

			// Token: 0x02002628 RID: 9768
			public class CONFIGURECONSUMERSIDESCREEN
			{
				// Token: 0x0400A7A1 RID: 42913
				public static LocString TITLE = "Configure Building";

				// Token: 0x0400A7A2 RID: 42914
				public static LocString SELECTION_DESCRIPTION_HEADER = "Description";
			}

			// Token: 0x02002629 RID: 9769
			public class TREEFILTERABLESIDESCREEN
			{
				// Token: 0x0400A7A3 RID: 42915
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400A7A4 RID: 42916
				public static LocString TITLE_CRITTER = "Critter Filter";

				// Token: 0x0400A7A5 RID: 42917
				public static LocString ALLBUTTON = "All";

				// Token: 0x0400A7A6 RID: 42918
				public static LocString ALLBUTTONTOOLTIP = "Allow storage of all resource categories in this container";

				// Token: 0x0400A7A7 RID: 42919
				public static LocString CATEGORYBUTTONTOOLTIP = "Allow storage of anything in the {0} resource category";

				// Token: 0x0400A7A8 RID: 42920
				public static LocString MATERIALBUTTONTOOLTIP = "Add or remove this material from storage";

				// Token: 0x0400A7A9 RID: 42921
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTON = "Sweep Only";

				// Token: 0x0400A7AA RID: 42922
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP = "Only store objects marked Sweep <color=#F44A47><b>[K]</b></color> in this container";

				// Token: 0x0400A7AB RID: 42923
				public static LocString ONLYALLOWSPICEDITEMSBUTTON = "Spiced Food Only";

				// Token: 0x0400A7AC RID: 42924
				public static LocString ONLYALLOWSPICEDITEMSBUTTONTOOLTIP = "Only store foods that have been spiced at the " + UI.PRE_KEYWORD + "Spice Grinder" + UI.PST_KEYWORD;

				// Token: 0x0400A7AD RID: 42925
				public static LocString SEARCH_PLACEHOLDER = "Search";
			}

			// Token: 0x0200262A RID: 9770
			public class TELESCOPESIDESCREEN
			{
				// Token: 0x0400A7AE RID: 42926
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400A7AF RID: 42927
				public static LocString NO_SELECTED_ANALYSIS_TARGET = "No analysis focus selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to select a focus";

				// Token: 0x0400A7B0 RID: 42928
				public static LocString ANALYSIS_TARGET_SELECTED = "Object focus selected\nAnalysis underway";

				// Token: 0x0400A7B1 RID: 42929
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400A7B2 RID: 42930
				public static LocString ANALYSIS_TARGET_HEADER = "Object Analysis";
			}

			// Token: 0x0200262B RID: 9771
			public class CLUSTERTELESCOPESIDESCREEN
			{
				// Token: 0x0400A7B3 RID: 42931
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400A7B4 RID: 42932
				public static LocString CHECKBOX_METEORS = "Allow meteor shower identification";

				// Token: 0x0400A7B5 RID: 42933
				public static LocString CHECKBOX_TOOLTIP_METEORS = string.Concat(new string[]
				{
					"Prioritizes unidentified meteors that come within range in a previously revealed location\n\nWill interrupt a Duplicant working on revealing a new ",
					UI.PRE_KEYWORD,
					"Starmap",
					UI.PST_KEYWORD,
					" location"
				});
			}

			// Token: 0x0200262C RID: 9772
			public class TEMPORALTEARSIDESCREEN
			{
				// Token: 0x0400A7B6 RID: 42934
				public static LocString TITLE = "Temporal Tear";

				// Token: 0x0400A7B7 RID: 42935
				public static LocString BUTTON_OPEN = "Enter Tear";

				// Token: 0x0400A7B8 RID: 42936
				public static LocString BUTTON_CLOSED = "Tear Closed";

				// Token: 0x0400A7B9 RID: 42937
				public static LocString BUTTON_LABEL = "Enter Temporal Tear";

				// Token: 0x0400A7BA RID: 42938
				public static LocString CONFIRM_POPUP_MESSAGE = "Are you sure you want to fire this?";

				// Token: 0x0400A7BB RID: 42939
				public static LocString CONFIRM_POPUP_CONFIRM = "Yes, I'm ready for a meteor shower.";

				// Token: 0x0400A7BC RID: 42940
				public static LocString CONFIRM_POPUP_CANCEL = "No, I need more time to prepare.";

				// Token: 0x0400A7BD RID: 42941
				public static LocString CONFIRM_POPUP_TITLE = "Temporal Tear Opener";
			}

			// Token: 0x0200262D RID: 9773
			public class RAILGUNSIDESCREEN
			{
				// Token: 0x0400A7BE RID: 42942
				public static LocString TITLE = "Launcher Configuration";

				// Token: 0x0400A7BF RID: 42943
				public static LocString NO_SELECTED_LAUNCH_TARGET = "No destination selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to set a course";

				// Token: 0x0400A7C0 RID: 42944
				public static LocString LAUNCH_TARGET_SELECTED = "Launcher destination {0} set";

				// Token: 0x0400A7C1 RID: 42945
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400A7C2 RID: 42946
				public static LocString LAUNCH_RESOURCES_HEADER = "Launch Resources:";

				// Token: 0x0400A7C3 RID: 42947
				public static LocString MINIMUM_PAYLOAD_MASS = "Minimum launch mass:";
			}

			// Token: 0x0200262E RID: 9774
			public class CLUSTERWORLDSIDESCREEN
			{
				// Token: 0x0400A7C4 RID: 42948
				public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400A7C5 RID: 42949
				public static LocString VIEW_WORLD = "Oversee " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400A7C6 RID: 42950
				public static LocString VIEW_WORLD_DISABLE_TOOLTIP = "Cannot view " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400A7C7 RID: 42951
				public static LocString VIEW_WORLD_TOOLTIP = "View this " + UI.CLUSTERMAP.PLANETOID + "'s surface";
			}

			// Token: 0x0200262F RID: 9775
			public class ROCKETMODULESIDESCREEN
			{
				// Token: 0x0400A7C8 RID: 42952
				public static LocString TITLE = "Rocket Module";

				// Token: 0x0400A7C9 RID: 42953
				public static LocString CHANGEMODULEPANEL = "Add or Change Module";

				// Token: 0x0400A7CA RID: 42954
				public static LocString ENGINE_MAX_HEIGHT = "This engine allows a <b>Maximum Rocket Height</b> of {0}";

				// Token: 0x02003114 RID: 12564
				public class MODULESTATCHANGE
				{
					// Token: 0x0400C51C RID: 50460
					public static LocString TITLE = "Rocket stats on construction:";

					// Token: 0x0400C51D RID: 50461
					public static LocString BURDEN = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME + ": {0} ({1})";

					// Token: 0x0400C51E RID: 50462
					public static LocString RANGE = string.Concat(new string[]
					{
						"    • Potential ",
						DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME,
						": {0}/1",
						UI.UNITSUFFIXES.MASS.KILOGRAM,
						" Fuel ({1})"
					});

					// Token: 0x0400C51F RID: 50463
					public static LocString SPEED = "    • Speed: {0} ({1})";

					// Token: 0x0400C520 RID: 50464
					public static LocString ENGINEPOWER = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME + ": {0} ({1})";

					// Token: 0x0400C521 RID: 50465
					public static LocString HEIGHT = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0}/{2} ({1})";

					// Token: 0x0400C522 RID: 50466
					public static LocString HEIGHT_NOMAX = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0} ({1})";

					// Token: 0x0400C523 RID: 50467
					public static LocString POSITIVEDELTA = UI.FormatAsPositiveModifier("{0}");

					// Token: 0x0400C524 RID: 50468
					public static LocString NEGATIVEDELTA = UI.FormatAsNegativeModifier("{0}");
				}

				// Token: 0x02003115 RID: 12565
				public class BUTTONSWAPMODULEUP
				{
					// Token: 0x0400C525 RID: 50469
					public static LocString DESC = "Swap this rocket module with the one above";

					// Token: 0x0400C526 RID: 50470
					public static LocString INVALID = "No module above may be swapped.\n\n    • A module above may be unable to have modules placed above it.\n    • A module above may be unable to fit into the space below it.\n    • This module may be unable to fit into the space above it.";
				}

				// Token: 0x02003116 RID: 12566
				public class BUTTONVIEWINTERIOR
				{
					// Token: 0x0400C527 RID: 50471
					public static LocString LABEL = "View Interior";

					// Token: 0x0400C528 RID: 50472
					public static LocString DESC = "What's goin' on in there?";

					// Token: 0x0400C529 RID: 50473
					public static LocString INVALID = "This module does not have an interior view";
				}

				// Token: 0x02003117 RID: 12567
				public class BUTTONVIEWEXTERIOR
				{
					// Token: 0x0400C52A RID: 50474
					public static LocString LABEL = "View Exterior";

					// Token: 0x0400C52B RID: 50475
					public static LocString DESC = "Switch to external world view";

					// Token: 0x0400C52C RID: 50476
					public static LocString INVALID = "Not available in flight";
				}

				// Token: 0x02003118 RID: 12568
				public class BUTTONSWAPMODULEDOWN
				{
					// Token: 0x0400C52D RID: 50477
					public static LocString DESC = "Swap this rocket module with the one below";

					// Token: 0x0400C52E RID: 50478
					public static LocString INVALID = "No module below may be swapped.\n\n    • A module below may be unable to have modules placed below it.\n    • A module below may be unable to fit into the space above it.\n    • This module may be unable to fit into the space below it.";
				}

				// Token: 0x02003119 RID: 12569
				public class BUTTONCHANGEMODULE
				{
					// Token: 0x0400C52F RID: 50479
					public static LocString DESC = "Swap this module for a different module";

					// Token: 0x0400C530 RID: 50480
					public static LocString INVALID = "This module cannot be changed to a different type";
				}

				// Token: 0x0200311A RID: 12570
				public class BUTTONREMOVEMODULE
				{
					// Token: 0x0400C531 RID: 50481
					public static LocString DESC = "Remove this module";

					// Token: 0x0400C532 RID: 50482
					public static LocString INVALID = "This module cannot be removed";
				}

				// Token: 0x0200311B RID: 12571
				public class ADDMODULE
				{
					// Token: 0x0400C533 RID: 50483
					public static LocString DESC = "Add a new module above this one";

					// Token: 0x0400C534 RID: 50484
					public static LocString INVALID = "Modules cannot be added above this module, or there is no room above to add a module";
				}
			}

			// Token: 0x02002630 RID: 9776
			public class CLUSTERLOCATIONFILTERSIDESCREEN
			{
				// Token: 0x0400A7CB RID: 42955
				public static LocString TITLE = "Location Filter";

				// Token: 0x0400A7CC RID: 42956
				public static LocString HEADER = "Send Green signal at locations";

				// Token: 0x0400A7CD RID: 42957
				public static LocString EMPTY_SPACE_ROW = "In Space";
			}

			// Token: 0x02002631 RID: 9777
			public class DISPENSERSIDESCREEN
			{
				// Token: 0x0400A7CE RID: 42958
				public static LocString TITLE = "Dispenser";

				// Token: 0x0400A7CF RID: 42959
				public static LocString BUTTON_CANCEL = "Cancel order";

				// Token: 0x0400A7D0 RID: 42960
				public static LocString BUTTON_DISPENSE = "Dispense item";
			}

			// Token: 0x02002632 RID: 9778
			public class ROCKETRESTRICTIONSIDESCREEN
			{
				// Token: 0x0400A7D1 RID: 42961
				public static LocString TITLE = "Rocket Restrictions";

				// Token: 0x0400A7D2 RID: 42962
				public static LocString BUILDING_RESTRICTIONS_LABEL = "Interior Building Restrictions";

				// Token: 0x0400A7D3 RID: 42963
				public static LocString NONE_RESTRICTION_BUTTON = "None";

				// Token: 0x0400A7D4 RID: 42964
				public static LocString NONE_RESTRICTION_BUTTON_TOOLTIP = "There are no restrictions on buildings inside this rocket";

				// Token: 0x0400A7D5 RID: 42965
				public static LocString GROUNDED_RESTRICTION_BUTTON = "Grounded";

				// Token: 0x0400A7D6 RID: 42966
				public static LocString GROUNDED_RESTRICTION_BUTTON_TOOLTIP = "Buildings with their access restricted cannot be operated while grounded, though they can still be filled";

				// Token: 0x0400A7D7 RID: 42967
				public static LocString AUTOMATION = "Automation Controlled";

				// Token: 0x0400A7D8 RID: 42968
				public static LocString AUTOMATION_TOOLTIP = "Building restrictions are managed by automation\n\nBuildings with their access restricted cannot be operated, though they can still be filled";
			}

			// Token: 0x02002633 RID: 9779
			public class HABITATMODULESIDESCREEN
			{
				// Token: 0x0400A7D9 RID: 42969
				public static LocString TITLE = "Spacefarer Module";

				// Token: 0x0400A7DA RID: 42970
				public static LocString VIEW_BUTTON = "View Interior";

				// Token: 0x0400A7DB RID: 42971
				public static LocString VIEW_BUTTON_TOOLTIP = "What's goin' on in there?";
			}

			// Token: 0x02002634 RID: 9780
			public class HARVESTMODULESIDESCREEN
			{
				// Token: 0x0400A7DC RID: 42972
				public static LocString TITLE = "Resource Gathering";

				// Token: 0x0400A7DD RID: 42973
				public static LocString MINING_IN_PROGRESS = "Drilling...";

				// Token: 0x0400A7DE RID: 42974
				public static LocString MINING_STOPPED = "Not drilling";

				// Token: 0x0400A7DF RID: 42975
				public static LocString ENABLE = "Enable Drill";

				// Token: 0x0400A7E0 RID: 42976
				public static LocString DISABLE = "Disable Drill";
			}

			// Token: 0x02002635 RID: 9781
			public class SELECTMODULESIDESCREEN
			{
				// Token: 0x0400A7E1 RID: 42977
				public static LocString TITLE = "Select Module";

				// Token: 0x0400A7E2 RID: 42978
				public static LocString BUILDBUTTON = "Build";

				// Token: 0x0200311C RID: 12572
				public class CONSTRAINTS
				{
					// Token: 0x02003400 RID: 13312
					public class RESEARCHED
					{
						// Token: 0x0400CC8A RID: 52362
						public static LocString COMPLETE = "Research Completed";

						// Token: 0x0400CC8B RID: 52363
						public static LocString FAILED = "Research Incomplete";
					}

					// Token: 0x02003401 RID: 13313
					public class MATERIALS_AVAILABLE
					{
						// Token: 0x0400CC8C RID: 52364
						public static LocString COMPLETE = "Materials available";

						// Token: 0x0400CC8D RID: 52365
						public static LocString FAILED = "• Materials unavailable";
					}

					// Token: 0x02003402 RID: 13314
					public class ONE_COMMAND_PER_ROCKET
					{
						// Token: 0x0400CC8E RID: 52366
						public static LocString COMPLETE = "";

						// Token: 0x0400CC8F RID: 52367
						public static LocString FAILED = "• Command module already installed";
					}

					// Token: 0x02003403 RID: 13315
					public class ONE_ENGINE_PER_ROCKET
					{
						// Token: 0x0400CC90 RID: 52368
						public static LocString COMPLETE = "";

						// Token: 0x0400CC91 RID: 52369
						public static LocString FAILED = "• Engine module already installed";
					}

					// Token: 0x02003404 RID: 13316
					public class ENGINE_AT_BOTTOM
					{
						// Token: 0x0400CC92 RID: 52370
						public static LocString COMPLETE = "";

						// Token: 0x0400CC93 RID: 52371
						public static LocString FAILED = "• Must install at bottom of rocket";
					}

					// Token: 0x02003405 RID: 13317
					public class TOP_ONLY
					{
						// Token: 0x0400CC94 RID: 52372
						public static LocString COMPLETE = "";

						// Token: 0x0400CC95 RID: 52373
						public static LocString FAILED = "• Must install at top of rocket";
					}

					// Token: 0x02003406 RID: 13318
					public class SPACE_AVAILABLE
					{
						// Token: 0x0400CC96 RID: 52374
						public static LocString COMPLETE = "";

						// Token: 0x0400CC97 RID: 52375
						public static LocString FAILED = "• Space above rocket blocked";
					}

					// Token: 0x02003407 RID: 13319
					public class PASSENGER_MODULE_AVAILABLE
					{
						// Token: 0x0400CC98 RID: 52376
						public static LocString COMPLETE = "";

						// Token: 0x0400CC99 RID: 52377
						public static LocString FAILED = "• Max number of passenger modules installed";
					}

					// Token: 0x02003408 RID: 13320
					public class MAX_MODULES
					{
						// Token: 0x0400CC9A RID: 52378
						public static LocString COMPLETE = "";

						// Token: 0x0400CC9B RID: 52379
						public static LocString FAILED = "• Max module limit of engine reached";
					}

					// Token: 0x02003409 RID: 13321
					public class MAX_HEIGHT
					{
						// Token: 0x0400CC9C RID: 52380
						public static LocString COMPLETE = "";

						// Token: 0x0400CC9D RID: 52381
						public static LocString FAILED = "• Engine's height limit reached or exceeded";

						// Token: 0x0400CC9E RID: 52382
						public static LocString FAILED_NO_ENGINE = "• Rocket requires space for an engine";
					}
				}
			}

			// Token: 0x02002636 RID: 9782
			public class FILTERSIDESCREEN
			{
				// Token: 0x0400A7E3 RID: 42979
				public static LocString TITLE = "Filter Outputs";

				// Token: 0x0400A7E4 RID: 42980
				public static LocString NO_SELECTION = "None";

				// Token: 0x0400A7E5 RID: 42981
				public static LocString OUTPUTELEMENTHEADER = "Output 1";

				// Token: 0x0400A7E6 RID: 42982
				public static LocString SELECTELEMENTHEADER = "Output 2";

				// Token: 0x0400A7E7 RID: 42983
				public static LocString OUTPUTRED = "Output Red";

				// Token: 0x0400A7E8 RID: 42984
				public static LocString OUTPUTGREEN = "Output Green";

				// Token: 0x0400A7E9 RID: 42985
				public static LocString NOELEMENTSELECTED = "No element selected";

				// Token: 0x0200311D RID: 12573
				public static class UNFILTEREDELEMENTS
				{
					// Token: 0x0400C535 RID: 50485
					public static LocString GAS = "Gas Output:\nAll";

					// Token: 0x0400C536 RID: 50486
					public static LocString LIQUID = "Liquid Output:\nAll";

					// Token: 0x0400C537 RID: 50487
					public static LocString SOLID = "Solid Output:\nAll";
				}

				// Token: 0x0200311E RID: 12574
				public static class FILTEREDELEMENT
				{
					// Token: 0x0400C538 RID: 50488
					public static LocString GAS = "Filtered Gas Output:\n{0}";

					// Token: 0x0400C539 RID: 50489
					public static LocString LIQUID = "Filtered Liquid Output:\n{0}";

					// Token: 0x0400C53A RID: 50490
					public static LocString SOLID = "Filtered Solid Output:\n{0}";
				}
			}

			// Token: 0x02002637 RID: 9783
			public class FEWOPTIONSELECTIONSIDESCREEN
			{
				// Token: 0x0400A7EA RID: 42986
				public static LocString TITLE = "Options";
			}

			// Token: 0x02002638 RID: 9784
			public class LOGICBROADCASTCHANNELSIDESCREEN
			{
				// Token: 0x0400A7EB RID: 42987
				public static LocString TITLE = "Channel Selector";

				// Token: 0x0400A7EC RID: 42988
				public static LocString HEADER = "Channel Selector";

				// Token: 0x0400A7ED RID: 42989
				public static LocString IN_RANGE = "In Range";

				// Token: 0x0400A7EE RID: 42990
				public static LocString OUT_OF_RANGE = "Out of Range";

				// Token: 0x0400A7EF RID: 42991
				public static LocString NO_SENDERS = "No Channels Available";

				// Token: 0x0400A7F0 RID: 42992
				public static LocString NO_SENDERS_DESC = "Build a " + BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.NAME + " to transmit a signal.";
			}

			// Token: 0x02002639 RID: 9785
			public class CONDITIONLISTSIDESCREEN
			{
				// Token: 0x0400A7F1 RID: 42993
				public static LocString TITLE = "Condition List";
			}

			// Token: 0x0200263A RID: 9786
			public class FABRICATORSIDESCREEN
			{
				// Token: 0x0400A7F2 RID: 42994
				public static LocString TITLE = "{0} Production Orders";

				// Token: 0x0400A7F3 RID: 42995
				public static LocString SUBTITLE = "Recipes";

				// Token: 0x0400A7F4 RID: 42996
				public static LocString NORECIPEDISCOVERED = "No discovered recipes";

				// Token: 0x0400A7F5 RID: 42997
				public static LocString NORECIPEDISCOVERED_BODY = "Discover new ingredients or research new technology to unlock some recipes.";

				// Token: 0x0400A7F6 RID: 42998
				public static LocString NORECIPESELECTED = "No recipe selected";

				// Token: 0x0400A7F7 RID: 42999
				public static LocString SELECTRECIPE = "Select a recipe to fabricate.";

				// Token: 0x0400A7F8 RID: 43000
				public static LocString COST = "<b>Ingredients:</b>\n";

				// Token: 0x0400A7F9 RID: 43001
				public static LocString RESULTREQUIREMENTS = "<b>Requirements:</b>";

				// Token: 0x0400A7FA RID: 43002
				public static LocString RESULTEFFECTS = "<b>Effects:</b>";

				// Token: 0x0400A7FB RID: 43003
				public static LocString KG = "- {0}: {1}\n";

				// Token: 0x0400A7FC RID: 43004
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400A7FD RID: 43005
				public static LocString CANCEL = "Cancel";

				// Token: 0x0400A7FE RID: 43006
				public static LocString RECIPERQUIREMENT = "{0}: {1} / {2}";

				// Token: 0x0400A7FF RID: 43007
				public static LocString RECIPEPRODUCT = "{0}: {1}";

				// Token: 0x0400A800 RID: 43008
				public static LocString UNITS_AND_CALS = "{0} [{1}]";

				// Token: 0x0400A801 RID: 43009
				public static LocString CALS = "{0}";

				// Token: 0x0400A802 RID: 43010
				public static LocString QUEUED_MISSING_INGREDIENTS_TOOLTIP = "Missing {0} of {1}\n";

				// Token: 0x0400A803 RID: 43011
				public static LocString CURRENT_ORDER = "Current order: {0}";

				// Token: 0x0400A804 RID: 43012
				public static LocString NEXT_ORDER = "Next order: {0}";

				// Token: 0x0400A805 RID: 43013
				public static LocString NO_WORKABLE_ORDER = "No workable order";

				// Token: 0x0400A806 RID: 43014
				public static LocString RECIPE_DETAILS = "Recipe Details";

				// Token: 0x0400A807 RID: 43015
				public static LocString RECIPE_QUEUE = "Order Production Quantity:";

				// Token: 0x0400A808 RID: 43016
				public static LocString RECIPE_FOREVER = "Forever";

				// Token: 0x0400A809 RID: 43017
				public static LocString CHANGE_RECIPE_ARROW_LABEL = "Change recipe";

				// Token: 0x0400A80A RID: 43018
				public static LocString INGREDIENTS = "<b>Ingredients:</b>";

				// Token: 0x0400A80B RID: 43019
				public static LocString RECIPE_EFFECTS = "<b>Effects:</b>";

				// Token: 0x0400A80C RID: 43020
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS = "Building accepts mutant seeds";

				// Token: 0x0400A80D RID: 43021
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS_TOOLTIP = "Toggle whether Duplicants will deliver mutant seed species to this building as recipe ingredients.";

				// Token: 0x0200311F RID: 12575
				public class TOOLTIPS
				{
					// Token: 0x0400C53B RID: 50491
					public static LocString RECIPE_WORKTIME = "This recipe takes {0} to complete";

					// Token: 0x0400C53C RID: 50492
					public static LocString RECIPERQUIREMENT_SUFFICIENT = "This recipe consumes {1} of an available {2} of {0}";

					// Token: 0x0400C53D RID: 50493
					public static LocString RECIPERQUIREMENT_INSUFFICIENT = "This recipe requires {1} {0}\nAvailable: {2}";

					// Token: 0x0400C53E RID: 50494
					public static LocString RECIPEPRODUCT = "This recipe produces {1} {0}";
				}

				// Token: 0x02003120 RID: 12576
				public class EFFECTS
				{
					// Token: 0x0400C53F RID: 50495
					public static LocString OXYGEN_TANK = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME + " ({0})";

					// Token: 0x0400C540 RID: 50496
					public static LocString OXYGEN_TANK_UNDERWATER = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK_UNDERWATER.NAME + " ({0})";

					// Token: 0x0400C541 RID: 50497
					public static LocString JETSUIT_TANK = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.TANK_EFFECT_NAME + " ({0})";

					// Token: 0x0400C542 RID: 50498
					public static LocString LEADSUIT_BATTERY = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.BATTERY_EFFECT_NAME + " ({0})";

					// Token: 0x0400C543 RID: 50499
					public static LocString COOL_VEST = STRINGS.EQUIPMENT.PREFABS.COOL_VEST.NAME + " ({0})";

					// Token: 0x0400C544 RID: 50500
					public static LocString WARM_VEST = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.NAME + " ({0})";

					// Token: 0x0400C545 RID: 50501
					public static LocString FUNKY_VEST = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.NAME + " ({0})";

					// Token: 0x0400C546 RID: 50502
					public static LocString RESEARCHPOINT = "{0}: +1";
				}

				// Token: 0x02003121 RID: 12577
				public class RECIPE_CATEGORIES
				{
					// Token: 0x0400C547 RID: 50503
					public static LocString ATMO_SUIT_FACADES = "Atmo Suit Styles";

					// Token: 0x0400C548 RID: 50504
					public static LocString JET_SUIT_FACADES = "Jet Suit Styles";

					// Token: 0x0400C549 RID: 50505
					public static LocString LEAD_SUIT_FACADES = "Lead Suit Styles";

					// Token: 0x0400C54A RID: 50506
					public static LocString PRIMO_GARB_FACADES = "Primo Garb Styles";
				}
			}

			// Token: 0x0200263B RID: 9787
			public class ASSIGNMENTGROUPCONTROLLER
			{
				// Token: 0x0400A80E RID: 43022
				public static LocString TITLE = "Duplicant Assignment";

				// Token: 0x0400A80F RID: 43023
				public static LocString PILOT = "Pilot";

				// Token: 0x0400A810 RID: 43024
				public static LocString OFFWORLD = "Offworld";

				// Token: 0x02003122 RID: 12578
				public class TOOLTIPS
				{
					// Token: 0x0400C54B RID: 50507
					public static LocString DIFFERENT_WORLD = "This Duplicant is on a different " + UI.CLUSTERMAP.PLANETOID;

					// Token: 0x0400C54C RID: 50508
					public static LocString ASSIGN = "<b>Add</b> this Duplicant to rocket crew";

					// Token: 0x0400C54D RID: 50509
					public static LocString UNASSIGN = "<b>Remove</b> this Duplicant from rocket crew";
				}
			}

			// Token: 0x0200263C RID: 9788
			public class LAUNCHPADSIDESCREEN
			{
				// Token: 0x0400A811 RID: 43025
				public static LocString TITLE = "Rocket Platform";

				// Token: 0x0400A812 RID: 43026
				public static LocString WAITING_TO_LAND_PANEL = "Waiting to land";

				// Token: 0x0400A813 RID: 43027
				public static LocString NO_ROCKETS_WAITING = "No rockets in orbit";

				// Token: 0x0400A814 RID: 43028
				public static LocString IN_ORBIT_ABOVE_PANEL = "Rockets in orbit";

				// Token: 0x0400A815 RID: 43029
				public static LocString NEW_ROCKET_BUTTON = "NEW ROCKET";

				// Token: 0x0400A816 RID: 43030
				public static LocString LAND_BUTTON = "LAND HERE";

				// Token: 0x0400A817 RID: 43031
				public static LocString CANCEL_LAND_BUTTON = "CANCEL";

				// Token: 0x0400A818 RID: 43032
				public static LocString LAUNCH_BUTTON = "BEGIN LAUNCH SEQUENCE";

				// Token: 0x0400A819 RID: 43033
				public static LocString LAUNCH_BUTTON_DEBUG = "BEGIN LAUNCH SEQUENCE (DEBUG ENABLED)";

				// Token: 0x0400A81A RID: 43034
				public static LocString LAUNCH_BUTTON_TOOLTIP = "Blast off!";

				// Token: 0x0400A81B RID: 43035
				public static LocString LAUNCH_BUTTON_NOT_READY_TOOLTIP = "This rocket is <b>not</b> ready to launch\n\n<b>Review the Launch Checklist in the status panel for more detail</b>";

				// Token: 0x0400A81C RID: 43036
				public static LocString LAUNCH_WARNINGS_BUTTON = "ACKNOWLEDGE WARNINGS";

				// Token: 0x0400A81D RID: 43037
				public static LocString LAUNCH_WARNINGS_BUTTON_TOOLTIP = "Some items in the Launch Checklist require attention\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to ignore warnings and proceed with launch</b>";

				// Token: 0x0400A81E RID: 43038
				public static LocString LAUNCH_REQUESTED_BUTTON = "CANCEL LAUNCH";

				// Token: 0x0400A81F RID: 43039
				public static LocString LAUNCH_REQUESTED_BUTTON_TOOLTIP = "This rocket will take off as soon as a Duplicant takes the controls\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to cancel launch</b>";

				// Token: 0x0400A820 RID: 43040
				public static LocString LAUNCH_AUTOMATION_CONTROLLED = "AUTOMATION CONTROLLED";

				// Token: 0x0400A821 RID: 43041
				public static LocString LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP = "This " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + "'s launch operation is controlled by automation signals";

				// Token: 0x02003123 RID: 12579
				public class STATUS
				{
					// Token: 0x0400C54E RID: 50510
					public static LocString STILL_PREPPING = "Launch Checklist Incomplete";

					// Token: 0x0400C54F RID: 50511
					public static LocString READY_FOR_LAUNCH = "Ready to Launch";

					// Token: 0x0400C550 RID: 50512
					public static LocString LOADING_CREW = "Loading crew...";

					// Token: 0x0400C551 RID: 50513
					public static LocString UNLOADING_PASSENGERS = "Unloading non-crew...";

					// Token: 0x0400C552 RID: 50514
					public static LocString WAITING_FOR_PILOT = "Pilot requested at control station...";

					// Token: 0x0400C553 RID: 50515
					public static LocString COUNTING_DOWN = "5... 4... 3... 2... 1...";

					// Token: 0x0400C554 RID: 50516
					public static LocString TAKING_OFF = "Liftoff!!";
				}
			}

			// Token: 0x0200263D RID: 9789
			public class AUTOPLUMBERSIDESCREEN
			{
				// Token: 0x0400A822 RID: 43042
				public static LocString TITLE = "Automatic Building Configuration";

				// Token: 0x02003124 RID: 12580
				public class BUTTONS
				{
					// Token: 0x0200340A RID: 13322
					public class POWER
					{
						// Token: 0x0400CC9F RID: 52383
						public static LocString TOOLTIP = "Add Dev Generator and Electrical Wires";
					}

					// Token: 0x0200340B RID: 13323
					public class PIPES
					{
						// Token: 0x0400CCA0 RID: 52384
						public static LocString TOOLTIP = "Add Dev Pumps and Pipes";
					}

					// Token: 0x0200340C RID: 13324
					public class SOLIDS
					{
						// Token: 0x0400CCA1 RID: 52385
						public static LocString TOOLTIP = "Spawn solid resources for a relevant recipe or conversions";
					}

					// Token: 0x0200340D RID: 13325
					public class MINION
					{
						// Token: 0x0400CCA2 RID: 52386
						public static LocString TOOLTIP = "Spawn a Duplicant in front of the building";
					}

					// Token: 0x0200340E RID: 13326
					public class FACADE
					{
						// Token: 0x0400CCA3 RID: 52387
						public static LocString TOOLTIP = "Toggle the building blueprint";
					}
				}
			}

			// Token: 0x0200263E RID: 9790
			public class SELFDESTRUCTSIDESCREEN
			{
				// Token: 0x0400A823 RID: 43043
				public static LocString TITLE = "Self Destruct";

				// Token: 0x0400A824 RID: 43044
				public static LocString MESSAGE_TEXT = "EMERGENCY PROCEDURES";

				// Token: 0x0400A825 RID: 43045
				public static LocString BUTTON_TEXT = "ABANDON SHIP!";

				// Token: 0x0400A826 RID: 43046
				public static LocString BUTTON_TEXT_CONFIRM = "CONFIRM ABANDON SHIP";

				// Token: 0x0400A827 RID: 43047
				public static LocString BUTTON_TOOLTIP = "This rocket is equipped with an emergency escape system.\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";

				// Token: 0x0400A828 RID: 43048
				public static LocString BUTTON_TOOLTIP_CONFIRM = "<b>This will eject any passengers and destroy the rocket.<b>\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
			}

			// Token: 0x0200263F RID: 9791
			public class GENESHUFFLERSIDESREEN
			{
				// Token: 0x0400A829 RID: 43049
				public static LocString TITLE = "Neural Vacillator";

				// Token: 0x0400A82A RID: 43050
				public static LocString COMPLETE = "Something feels different.";

				// Token: 0x0400A82B RID: 43051
				public static LocString UNDERWAY = "Neural Vacillation in progress.";

				// Token: 0x0400A82C RID: 43052
				public static LocString CONSUMED = "There are no charges left in this Vacillator.";

				// Token: 0x0400A82D RID: 43053
				public static LocString CONSUMED_WAITING = "Recharge requested, awaiting delivery by Duplicant.";

				// Token: 0x0400A82E RID: 43054
				public static LocString BUTTON = "Complete Neural Process";

				// Token: 0x0400A82F RID: 43055
				public static LocString BUTTON_RECHARGE = "Recharge";

				// Token: 0x0400A830 RID: 43056
				public static LocString BUTTON_RECHARGE_CANCEL = "Cancel Recharge";
			}

			// Token: 0x02002640 RID: 9792
			public class MINIONTODOSIDESCREEN
			{
				// Token: 0x0400A831 RID: 43057
				public static LocString CURRENT_TITLE = "Current Errand";

				// Token: 0x0400A832 RID: 43058
				public static LocString LIST_TITLE = "\"To Do\" List";

				// Token: 0x0400A833 RID: 43059
				public static LocString CURRENT_SCHEDULE_BLOCK = "Schedule Block: {0}";

				// Token: 0x0400A834 RID: 43060
				public static LocString CHORE_TARGET = "{Target}";

				// Token: 0x0400A835 RID: 43061
				public static LocString CHORE_TARGET_AND_GROUP = "{Target} -- {Groups}";

				// Token: 0x0400A836 RID: 43062
				public static LocString SELF_LABEL = "Self";

				// Token: 0x0400A837 RID: 43063
				public static LocString TRUNCATED_CHORES = "{0} more";

				// Token: 0x0400A838 RID: 43064
				public static LocString TOOLTIP_IDLE = string.Concat(new string[]
				{
					"{IdleDescription}\n\nDuplicants will only <b>{Errand}</b> when there is nothing else for them to do\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.IDLE,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400A839 RID: 43065
				public static LocString TOOLTIP_NORMAL = string.Concat(new string[]
				{
					"{Description}\n\nErrand Type: {Groups}\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • {Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400A83A RID: 43066
				public static LocString TOOLTIP_PERSONAL = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					" errand and so will be performed before all Regular errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400A83B RID: 43067
				public static LocString TOOLTIP_EMERGENCY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is an ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" errand and so will be performed before all Regular and Personal errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" : {ClassPriority}\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400A83C RID: 43068
				public static LocString TOOLTIP_COMPULSORY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					" action and so will occur immediately\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400A83D RID: 43069
				public static LocString TOOLTIP_DESC_ACTIVE = "{Name}'s Current Errand: <b>{Errand}</b>";

				// Token: 0x0400A83E RID: 43070
				public static LocString TOOLTIP_DESC_INACTIVE = "{Name} could work on <b>{Errand}</b>, but it's not their top priority right now";

				// Token: 0x0400A83F RID: 43071
				public static LocString TOOLTIP_IDLEDESC_ACTIVE = "{Name} is currently <b>Idle</b>";

				// Token: 0x0400A840 RID: 43072
				public static LocString TOOLTIP_IDLEDESC_INACTIVE = "{Name} could become <b>Idle</b> when all other errands are canceled or completed";

				// Token: 0x0400A841 RID: 43073
				public static LocString TOOLTIP_NA = "--";

				// Token: 0x0400A842 RID: 43074
				public static LocString CHORE_GROUP_SEPARATOR = " or ";
			}

			// Token: 0x02002641 RID: 9793
			public class MODULEFLIGHTUTILITYSIDESCREEN
			{
				// Token: 0x0400A843 RID: 43075
				public static LocString TITLE = "Deployables";

				// Token: 0x0400A844 RID: 43076
				public static LocString DEPLOY_BUTTON = "Deploy";

				// Token: 0x0400A845 RID: 43077
				public static LocString DEPLOY_BUTTON_TOOLTIP = "Send this module's contents to the surface of the currently orbited " + UI.CLUSTERMAP.PLANETOID_KEYWORD + "\n\nA specific deploy location may need to be chosen for certain modules";

				// Token: 0x0400A846 RID: 43078
				public static LocString REPEAT_BUTTON_TOOLTIP = "Automatically deploy this module's contents when a destination orbit is reached";

				// Token: 0x0400A847 RID: 43079
				public static LocString SELECT_DUPLICANT = "Select Duplicant";

				// Token: 0x0400A848 RID: 43080
				public static LocString PILOT_FMT = "{0} - Pilot";
			}

			// Token: 0x02002642 RID: 9794
			public class HIGHENERGYPARTICLEDIRECTIONSIDESCREEN
			{
				// Token: 0x0400A849 RID: 43081
				public static LocString TITLE = "Emitting Particle Direction";

				// Token: 0x0400A84A RID: 43082
				public static LocString SELECTED_DIRECTION = "Selected direction: {0}";

				// Token: 0x0400A84B RID: 43083
				public static LocString DIRECTION_N = "N";

				// Token: 0x0400A84C RID: 43084
				public static LocString DIRECTION_NE = "NE";

				// Token: 0x0400A84D RID: 43085
				public static LocString DIRECTION_E = "E";

				// Token: 0x0400A84E RID: 43086
				public static LocString DIRECTION_SE = "SE";

				// Token: 0x0400A84F RID: 43087
				public static LocString DIRECTION_S = "S";

				// Token: 0x0400A850 RID: 43088
				public static LocString DIRECTION_SW = "SW";

				// Token: 0x0400A851 RID: 43089
				public static LocString DIRECTION_W = "W";

				// Token: 0x0400A852 RID: 43090
				public static LocString DIRECTION_NW = "NW";
			}

			// Token: 0x02002643 RID: 9795
			public class MONUMENTSIDESCREEN
			{
				// Token: 0x0400A853 RID: 43091
				public static LocString TITLE = "Great Monument";

				// Token: 0x0400A854 RID: 43092
				public static LocString FLIP_FACING_BUTTON = UI.CLICK(UI.ClickType.CLICK) + " TO ROTATE";
			}

			// Token: 0x02002644 RID: 9796
			public class PLANTERSIDESCREEN
			{
				// Token: 0x0400A855 RID: 43093
				public static LocString TITLE = "{0} Seeds";

				// Token: 0x0400A856 RID: 43094
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400A857 RID: 43095
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400A858 RID: 43096
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400A859 RID: 43097
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400A85A RID: 43098
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400A85B RID: 43099
				public static LocString MUTATIONS_HEADER = "Mutations";

				// Token: 0x0400A85C RID: 43100
				public static LocString DEPOSIT = "Plant";

				// Token: 0x0400A85D RID: 43101
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400A85E RID: 43102
				public static LocString REMOVE = "Uproot";

				// Token: 0x0400A85F RID: 43103
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400A860 RID: 43104
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400A861 RID: 43105
				public static LocString SELECT_DESC = "Select a seed to plant.";

				// Token: 0x0400A862 RID: 43106
				public static LocString LIFECYCLE = "<b>Life Cycle</b>:";

				// Token: 0x0400A863 RID: 43107
				public static LocString PLANTREQUIREMENTS = "<b>Growth Requirements</b>:";

				// Token: 0x0400A864 RID: 43108
				public static LocString PLANTEFFECTS = "<b>Effects</b>:";

				// Token: 0x0400A865 RID: 43109
				public static LocString NUMBEROFHARVESTS = "Harvests: {0}";

				// Token: 0x0400A866 RID: 43110
				public static LocString YIELD = "{0}: {1} ";

				// Token: 0x0400A867 RID: 43111
				public static LocString YIELD_NONFOOD = "{0}: {1} ";

				// Token: 0x0400A868 RID: 43112
				public static LocString YIELD_SINGLE = "{0}";

				// Token: 0x0400A869 RID: 43113
				public static LocString YIELDPERHARVEST = "{0} {1} per harvest";

				// Token: 0x0400A86A RID: 43114
				public static LocString TOTALHARVESTCALORIESWITHPERUNIT = "{0} [{1} / unit]";

				// Token: 0x0400A86B RID: 43115
				public static LocString TOTALHARVESTCALORIES = "{0}";

				// Token: 0x0400A86C RID: 43116
				public static LocString BONUS_SEEDS = "Base " + UI.FormatAsLink("Seed", "PLANTS") + " Harvest Chance: {0}";

				// Token: 0x0400A86D RID: 43117
				public static LocString YIELD_SEED = "{1} {0}";

				// Token: 0x0400A86E RID: 43118
				public static LocString YIELD_SEED_SINGLE = "{0}";

				// Token: 0x0400A86F RID: 43119
				public static LocString YIELD_SEED_FINAL_HARVEST = "{1} {0} - Final harvest only";

				// Token: 0x0400A870 RID: 43120
				public static LocString YIELD_SEED_SINGLE_FINAL_HARVEST = "{0} - Final harvest only";

				// Token: 0x0400A871 RID: 43121
				public static LocString ROTATION_NEED_FLOOR = "<b>Requires upward plot orientation.</b>";

				// Token: 0x0400A872 RID: 43122
				public static LocString ROTATION_NEED_WALL = "<b>Requires sideways plot orientation.</b>";

				// Token: 0x0400A873 RID: 43123
				public static LocString ROTATION_NEED_CEILING = "<b>Requires downward plot orientation.</b>";

				// Token: 0x0400A874 RID: 43124
				public static LocString NO_SPECIES_SELECTED = "Select a seed species above...";

				// Token: 0x0400A875 RID: 43125
				public static LocString DISEASE_DROPPER_BURST = "{Disease} Burst: {DiseaseAmount}";

				// Token: 0x0400A876 RID: 43126
				public static LocString DISEASE_DROPPER_CONSTANT = "{Disease}: {DiseaseAmount}";

				// Token: 0x0400A877 RID: 43127
				public static LocString DISEASE_ON_HARVEST = "{Disease} on crop: {DiseaseAmount}";

				// Token: 0x0400A878 RID: 43128
				public static LocString AUTO_SELF_HARVEST = "Self-Harvest On Grown";

				// Token: 0x02003125 RID: 12581
				public class TOOLTIPS
				{
					// Token: 0x0400C555 RID: 50517
					public static LocString PLANTLIFECYCLE = "Duration and number of harvests produced by this plant in a lifetime";

					// Token: 0x0400C556 RID: 50518
					public static LocString PLANTREQUIREMENTS = "Minimum conditions for basic plant growth";

					// Token: 0x0400C557 RID: 50519
					public static LocString PLANTEFFECTS = "Additional attributes of this plant";

					// Token: 0x0400C558 RID: 50520
					public static LocString YIELD = UI.FormatAsLink("{2}", "KCAL") + " produced [" + UI.FormatAsLink("{1}", "KCAL") + " / unit]";

					// Token: 0x0400C559 RID: 50521
					public static LocString YIELD_NONFOOD = "{0} produced per harvest";

					// Token: 0x0400C55A RID: 50522
					public static LocString NUMBEROFHARVESTS = "This plant can mature {0} times before the end of its life cycle";

					// Token: 0x0400C55B RID: 50523
					public static LocString YIELD_SEED = "Sow to grow more of this plant";

					// Token: 0x0400C55C RID: 50524
					public static LocString YIELD_SEED_FINAL_HARVEST = "{0}\n\nProduced in the final harvest of the plant's life cycle";

					// Token: 0x0400C55D RID: 50525
					public static LocString BONUS_SEEDS = "This plant has a {0} chance to produce new seeds when harvested";

					// Token: 0x0400C55E RID: 50526
					public static LocString DISEASE_DROPPER_BURST = "At certain points in this plant's lifecycle, it will emit a burst of {DiseaseAmount} {Disease}.";

					// Token: 0x0400C55F RID: 50527
					public static LocString DISEASE_DROPPER_CONSTANT = "This plant emits {DiseaseAmount} {Disease} while it is alive.";

					// Token: 0x0400C560 RID: 50528
					public static LocString DISEASE_ON_HARVEST = "The {Crop} produced by this plant will have {DiseaseAmount} {Disease} on it.";

					// Token: 0x0400C561 RID: 50529
					public static LocString AUTO_SELF_HARVEST = "This plant will instantly drop its crop and begin regrowing when it is matured.";
				}
			}

			// Token: 0x02002645 RID: 9797
			public class EGGINCUBATOR
			{
				// Token: 0x0400A879 RID: 43129
				public static LocString TITLE = "Critter Eggs";

				// Token: 0x0400A87A RID: 43130
				public static LocString AWAITINGREQUEST = "INCUBATE: {0}";

				// Token: 0x0400A87B RID: 43131
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400A87C RID: 43132
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400A87D RID: 43133
				public static LocString ENTITYDEPOSITED = "INCUBATING: {0}";

				// Token: 0x0400A87E RID: 43134
				public static LocString DEPOSIT = "Incubate";

				// Token: 0x0400A87F RID: 43135
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400A880 RID: 43136
				public static LocString REMOVE = "Remove";

				// Token: 0x0400A881 RID: 43137
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400A882 RID: 43138
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400A883 RID: 43139
				public static LocString SELECT_DESC = "Select an egg to incubate.";
			}

			// Token: 0x02002646 RID: 9798
			public class BASICRECEPTACLE
			{
				// Token: 0x0400A884 RID: 43140
				public static LocString TITLE = "Displayed Object";

				// Token: 0x0400A885 RID: 43141
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400A886 RID: 43142
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400A887 RID: 43143
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400A888 RID: 43144
				public static LocString ENTITYDEPOSITED = "DISPLAYING: {0}";

				// Token: 0x0400A889 RID: 43145
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400A88A RID: 43146
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400A88B RID: 43147
				public static LocString REMOVE = "Remove";

				// Token: 0x0400A88C RID: 43148
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400A88D RID: 43149
				public static LocString SELECT_TITLE = "SELECT OBJECT";

				// Token: 0x0400A88E RID: 43150
				public static LocString SELECT_DESC = "Select an object to display here.";
			}

			// Token: 0x02002647 RID: 9799
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400A88F RID: 43151
				public static LocString TITLE = "Target Critter";

				// Token: 0x0400A890 RID: 43152
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400A891 RID: 43153
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400A892 RID: 43154
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400A893 RID: 43155
				public static LocString ENTITYDEPOSITED = "CONTENTS: {0}";

				// Token: 0x0400A894 RID: 43156
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400A895 RID: 43157
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400A896 RID: 43158
				public static LocString REMOVE = "Remove";

				// Token: 0x0400A897 RID: 43159
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400A898 RID: 43160
				public static LocString SELECT_TITLE = "SELECT CRITTER";

				// Token: 0x0400A899 RID: 43161
				public static LocString SELECT_DESC = "Select a critter to store in this module.";
			}

			// Token: 0x02002648 RID: 9800
			public class LURE
			{
				// Token: 0x0400A89A RID: 43162
				public static LocString TITLE = "Select Bait";

				// Token: 0x0400A89B RID: 43163
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400A89C RID: 43164
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400A89D RID: 43165
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400A89E RID: 43166
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400A89F RID: 43167
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400A8A0 RID: 43168
				public static LocString ATTRACTS = "Attract {1}s";
			}

			// Token: 0x02002649 RID: 9801
			public class ROLESTATION
			{
				// Token: 0x0400A8A1 RID: 43169
				public static LocString TITLE = "Duplicant Skills";

				// Token: 0x0400A8A2 RID: 43170
				public static LocString OPENROLESBUTTON = "SKILLS";
			}

			// Token: 0x0200264A RID: 9802
			public class RESEARCHSIDESCREEN
			{
				// Token: 0x0400A8A3 RID: 43171
				public static LocString TITLE = "Select Research";

				// Token: 0x0400A8A4 RID: 43172
				public static LocString CURRENTLYRESEARCHING = "Currently Researching";

				// Token: 0x0400A8A5 RID: 43173
				public static LocString NOSELECTEDRESEARCH = "No Research selected";

				// Token: 0x0400A8A6 RID: 43174
				public static LocString OPENRESEARCHBUTTON = "RESEARCH";
			}

			// Token: 0x0200264B RID: 9803
			public class REFINERYSIDESCREEN
			{
				// Token: 0x0400A8A7 RID: 43175
				public static LocString RECIPE_FROM_TO = "{0} to {1}";

				// Token: 0x0400A8A8 RID: 43176
				public static LocString RECIPE_WITH = "{1} ({0})";

				// Token: 0x0400A8A9 RID: 43177
				public static LocString RECIPE_FROM_TO_WITH_NEWLINES = "{0}\nto\n{1}";

				// Token: 0x0400A8AA RID: 43178
				public static LocString RECIPE_FROM_TO_COMPOSITE = "{0} to {1} and {2}";

				// Token: 0x0400A8AB RID: 43179
				public static LocString RECIPE_FROM_TO_HEP = "{0} to " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {1}";

				// Token: 0x0400A8AC RID: 43180
				public static LocString RECIPE_SIMPLE_INCLUDE_AMOUNTS = "{0} {1}";

				// Token: 0x0400A8AD RID: 43181
				public static LocString RECIPE_FROM_TO_INCLUDE_AMOUNTS = "{2} {0} to {3} {1}";

				// Token: 0x0400A8AE RID: 43182
				public static LocString RECIPE_WITH_INCLUDE_AMOUNTS = "{3} {1} ({2} {0})";

				// Token: 0x0400A8AF RID: 43183
				public static LocString RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS = "{3} {0} to {4} {1} and {5} {2}";

				// Token: 0x0400A8B0 RID: 43184
				public static LocString RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS = "{2} {0} to {3} " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {4} {1}";
			}

			// Token: 0x0200264C RID: 9804
			public class SEALEDDOORSIDESCREEN
			{
				// Token: 0x0400A8B1 RID: 43185
				public static LocString TITLE = "Sealed Door";

				// Token: 0x0400A8B2 RID: 43186
				public static LocString LABEL = "This door requires a sample to unlock.";

				// Token: 0x0400A8B3 RID: 43187
				public static LocString BUTTON = "SUBMIT BIOSCAN";

				// Token: 0x0400A8B4 RID: 43188
				public static LocString AWAITINGBUTTON = "AWAITING BIOSCAN";
			}

			// Token: 0x0200264D RID: 9805
			public class ENCRYPTEDLORESIDESCREEN
			{
				// Token: 0x0400A8B5 RID: 43189
				public static LocString TITLE = "Encrypted File";

				// Token: 0x0400A8B6 RID: 43190
				public static LocString LABEL = "This computer contains encrypted files.";

				// Token: 0x0400A8B7 RID: 43191
				public static LocString BUTTON = "ATTEMPT DECRYPTION";

				// Token: 0x0400A8B8 RID: 43192
				public static LocString AWAITINGBUTTON = "AWAITING DECRYPTION";
			}

			// Token: 0x0200264E RID: 9806
			public class ACCESS_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400A8B9 RID: 43193
				public static LocString TITLE = "Door Access Control";

				// Token: 0x0400A8BA RID: 43194
				public static LocString DOOR_DEFAULT = "Default";

				// Token: 0x0400A8BB RID: 43195
				public static LocString MINION_ACCESS = "Duplicant Access Permissions";

				// Token: 0x0400A8BC RID: 43196
				public static LocString GO_LEFT_ENABLED = "Passing Left through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400A8BD RID: 43197
				public static LocString GO_LEFT_DISABLED = "Passing Left through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400A8BE RID: 43198
				public static LocString GO_RIGHT_ENABLED = "Passing Right through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400A8BF RID: 43199
				public static LocString GO_RIGHT_DISABLED = "Passing Right through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400A8C0 RID: 43200
				public static LocString GO_UP_ENABLED = "Passing Up through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400A8C1 RID: 43201
				public static LocString GO_UP_DISABLED = "Passing Up through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400A8C2 RID: 43202
				public static LocString GO_DOWN_ENABLED = "Passing Down through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400A8C3 RID: 43203
				public static LocString GO_DOWN_DISABLED = "Passing Down through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400A8C4 RID: 43204
				public static LocString SET_TO_DEFAULT = UI.CLICK(UI.ClickType.Click) + " to clear custom permissions";

				// Token: 0x0400A8C5 RID: 43205
				public static LocString SET_TO_CUSTOM = UI.CLICK(UI.ClickType.Click) + " to assign custom permissions";

				// Token: 0x0400A8C6 RID: 43206
				public static LocString USING_DEFAULT = "Default Access";

				// Token: 0x0400A8C7 RID: 43207
				public static LocString USING_CUSTOM = "Custom Access";
			}

			// Token: 0x0200264F RID: 9807
			public class ASSIGNABLESIDESCREEN
			{
				// Token: 0x0400A8C8 RID: 43208
				public static LocString TITLE = "Assign {0}";

				// Token: 0x0400A8C9 RID: 43209
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400A8CA RID: 43210
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400A8CB RID: 43211
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400A8CC RID: 43212
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400A8CD RID: 43213
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400A8CE RID: 43214
				public static LocString ASSIGN_TO_TOOLTIP = "Assign to {0}";

				// Token: 0x0400A8CF RID: 43215
				public static LocString UNASSIGN_TOOLTIP = "Assigned to {0}";

				// Token: 0x0400A8D0 RID: 43216
				public static LocString DISABLED_TOOLTIP = "{0} is ineligible for this skill assignment";

				// Token: 0x0400A8D1 RID: 43217
				public static LocString PUBLIC = "Public";
			}

			// Token: 0x02002650 RID: 9808
			public class COMETDETECTORSIDESCREEN
			{
				// Token: 0x0400A8D2 RID: 43218
				public static LocString TITLE = "Space Scanner";

				// Token: 0x0400A8D3 RID: 43219
				public static LocString HEADER = "Sends automation signal when selected object is detected";

				// Token: 0x0400A8D4 RID: 43220
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400A8D5 RID: 43221
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400A8D6 RID: 43222
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400A8D7 RID: 43223
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400A8D8 RID: 43224
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400A8D9 RID: 43225
				public static LocString ASSIGN_TO_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400A8DA RID: 43226
				public static LocString UNASSIGN_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400A8DB RID: 43227
				public static LocString NOTHING = "Nothing";

				// Token: 0x0400A8DC RID: 43228
				public static LocString COMETS = "Meteor Showers";

				// Token: 0x0400A8DD RID: 43229
				public static LocString ROCKETS = "Rocket Landing Ping";

				// Token: 0x0400A8DE RID: 43230
				public static LocString DUPEMADE = "Interplanetary Payloads";
			}

			// Token: 0x02002651 RID: 9809
			public class GEOTUNERSIDESCREEN
			{
				// Token: 0x0400A8DF RID: 43231
				public static LocString TITLE = "Select Geyser";

				// Token: 0x0400A8E0 RID: 43232
				public static LocString DESCRIPTION = "Select an analyzed geyser to transmit amplification data to.";

				// Token: 0x0400A8E1 RID: 43233
				public static LocString NOTHING = "No geyser selected";

				// Token: 0x0400A8E2 RID: 43234
				public static LocString UNSTUDIED_TOOLTIP = "This geyser must be analyzed before it can be selected\n\nDouble-click to view this geyser";

				// Token: 0x0400A8E3 RID: 43235
				public static LocString STUDIED_TOOLTIP = string.Concat(new string[]
				{
					"Increase this geyser's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" and output"
				});

				// Token: 0x0400A8E4 RID: 43236
				public static LocString GEOTUNER_LIMIT_TOOLTIP = "This geyser cannot be targeted by more " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;

				// Token: 0x0400A8E5 RID: 43237
				public static LocString STUDIED_TOOLTIP_MATERIAL = "Required resource: {MATERIAL}";

				// Token: 0x0400A8E6 RID: 43238
				public static LocString STUDIED_TOOLTIP_POTENTIAL_OUTPUT = "Potential Output {POTENTIAL_OUTPUT}";

				// Token: 0x0400A8E7 RID: 43239
				public static LocString STUDIED_TOOLTIP_BASE_TEMP = "Base {BASE}";

				// Token: 0x0400A8E8 RID: 43240
				public static LocString STUDIED_TOOLTIP_VISIT_GEYSER = "Double-click to view this geyser";

				// Token: 0x0400A8E9 RID: 43241
				public static LocString STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE = "Geotuned ";

				// Token: 0x0400A8EA RID: 43242
				public static LocString STUDIED_TOOLTIP_NUMBER_HOVERED = "This geyser is targeted by {0} Geotuners";
			}

			// Token: 0x02002652 RID: 9810
			public class COMMAND_MODULE_SIDE_SCREEN
			{
				// Token: 0x0400A8EB RID: 43243
				public static LocString TITLE = "Launch Conditions";

				// Token: 0x0400A8EC RID: 43244
				public static LocString DESTINATION_BUTTON = "Show Starmap";

				// Token: 0x0400A8ED RID: 43245
				public static LocString DESTINATION_BUTTON_EXPANSION = "Show Starmap";
			}

			// Token: 0x02002653 RID: 9811
			public class CLUSTERDESTINATIONSIDESCREEN
			{
				// Token: 0x0400A8EE RID: 43246
				public static LocString TITLE = "Destination";

				// Token: 0x0400A8EF RID: 43247
				public static LocString FIRSTAVAILABLE = "Any " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400A8F0 RID: 43248
				public static LocString NONEAVAILABLE = "No landing site";

				// Token: 0x0400A8F1 RID: 43249
				public static LocString NO_TALL_SITES_AVAILABLE = "No landing sites fit the height of this rocket";

				// Token: 0x0400A8F2 RID: 43250
				public static LocString DROPDOWN_TOOLTIP_VALID_SITE = "Land at {0} when the site is clear";

				// Token: 0x0400A8F3 RID: 43251
				public static LocString DROPDOWN_TOOLTIP_FIRST_AVAILABLE = "Select the first available landing site";

				// Token: 0x0400A8F4 RID: 43252
				public static LocString DROPDOWN_TOOLTIP_TOO_SHORT = "This rocket's height exceeds the space available in this landing site";

				// Token: 0x0400A8F5 RID: 43253
				public static LocString DROPDOWN_TOOLTIP_PATH_OBSTRUCTED = "Landing path obstructed";

				// Token: 0x0400A8F6 RID: 43254
				public static LocString DROPDOWN_TOOLTIP_SITE_OBSTRUCTED = "Landing position on the platform is obstructed";

				// Token: 0x0400A8F7 RID: 43255
				public static LocString DROPDOWN_TOOLTIP_PAD_DISABLED = BUILDINGS.PREFABS.LAUNCHPAD.NAME + " is disabled";

				// Token: 0x0400A8F8 RID: 43256
				public static LocString CHANGE_DESTINATION_BUTTON = "Change";

				// Token: 0x0400A8F9 RID: 43257
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP = "Select a new destination for this rocket";

				// Token: 0x0400A8FA RID: 43258
				public static LocString CLEAR_DESTINATION_BUTTON = "Clear";

				// Token: 0x0400A8FB RID: 43259
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP = "Clear this rocket's selected destination";

				// Token: 0x0400A8FC RID: 43260
				public static LocString LOOP_BUTTON_TOOLTIP = "Toggle a roundtrip flight between this rocket's destination and its original takeoff location";

				// Token: 0x02003126 RID: 12582
				public class ASSIGNMENTSTATUS
				{
					// Token: 0x0400C562 RID: 50530
					public static LocString LOCAL = "Current";

					// Token: 0x0400C563 RID: 50531
					public static LocString DESTINATION = "Destination";
				}
			}

			// Token: 0x02002654 RID: 9812
			public class EQUIPPABLESIDESCREEN
			{
				// Token: 0x0400A8FD RID: 43261
				public static LocString TITLE = "Equip {0}";

				// Token: 0x0400A8FE RID: 43262
				public static LocString ASSIGNEDTO = "Assigned to: {Assignee}";

				// Token: 0x0400A8FF RID: 43263
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400A900 RID: 43264
				public static LocString GENERAL_CURRENTASSIGNED = "(Owner)";
			}

			// Token: 0x02002655 RID: 9813
			public class EQUIPPABLE_SIDE_SCREEN
			{
				// Token: 0x0400A901 RID: 43265
				public static LocString TITLE = "Assign To Duplicant";

				// Token: 0x0400A902 RID: 43266
				public static LocString CURRENTLY_EQUIPPED = "Currently Equipped:\n{0}";

				// Token: 0x0400A903 RID: 43267
				public static LocString NONE_EQUIPPED = "None";

				// Token: 0x0400A904 RID: 43268
				public static LocString EQUIP_BUTTON = "Equip";

				// Token: 0x0400A905 RID: 43269
				public static LocString DROP_BUTTON = "Drop";

				// Token: 0x0400A906 RID: 43270
				public static LocString SWAP_BUTTON = "Swap";
			}

			// Token: 0x02002656 RID: 9814
			public class TELEPADSIDESCREEN
			{
				// Token: 0x0400A907 RID: 43271
				public static LocString TITLE = "Printables";

				// Token: 0x0400A908 RID: 43272
				public static LocString NEXTPRODUCTION = "Next Production: {0}";

				// Token: 0x0400A909 RID: 43273
				public static LocString GAMEOVER = "Colony Lost";

				// Token: 0x0400A90A RID: 43274
				public static LocString VICTORY_CONDITIONS = "Hardwired Imperatives";

				// Token: 0x0400A90B RID: 43275
				public static LocString SUMMARY_TITLE = "Colony Summary";

				// Token: 0x0400A90C RID: 43276
				public static LocString SKILLS_BUTTON = "Duplicant Skills";
			}

			// Token: 0x02002657 RID: 9815
			public class VALVESIDESCREEN
			{
				// Token: 0x0400A90D RID: 43277
				public static LocString TITLE = "Flow Control";
			}

			// Token: 0x02002658 RID: 9816
			public class LIMIT_VALVE_SIDE_SCREEN
			{
				// Token: 0x0400A90E RID: 43278
				public static LocString TITLE = "Meter Control";

				// Token: 0x0400A90F RID: 43279
				public static LocString AMOUNT = "Amount: {0}";

				// Token: 0x0400A910 RID: 43280
				public static LocString LIMIT = "Limit:";

				// Token: 0x0400A911 RID: 43281
				public static LocString RESET_BUTTON = "Reset Amount";

				// Token: 0x0400A912 RID: 43282
				public static LocString SLIDER_TOOLTIP_UNITS = "The amount of Units or Mass passing through the sensor.";
			}

			// Token: 0x02002659 RID: 9817
			public class NUCLEAR_REACTOR_SIDE_SCREEN
			{
				// Token: 0x0400A913 RID: 43283
				public static LocString TITLE = "Reaction Mass Target";

				// Token: 0x0400A914 RID: 43284
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will attempt to keep the reactor supplied with ",
					UI.PRE_KEYWORD,
					"{0}{1}",
					UI.PST_KEYWORD,
					" of ",
					UI.PRE_KEYWORD,
					"{2}",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200265A RID: 9818
			public class MANUALGENERATORSIDESCREEN
			{
				// Token: 0x0400A915 RID: 43285
				public static LocString TITLE = "Battery Recharge Threshold";

				// Token: 0x0400A916 RID: 43286
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400A917 RID: 43287
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to operate this generator when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x0200265B RID: 9819
			public class MANUALDELIVERYGENERATORSIDESCREEN
			{
				// Token: 0x0400A918 RID: 43288
				public static LocString TITLE = "Fuel Request Threshold";

				// Token: 0x0400A919 RID: 43289
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400A91A RID: 43290
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to deliver ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{1}%</b>"
				});
			}

			// Token: 0x0200265C RID: 9820
			public class TIME_OF_DAY_SIDE_SCREEN
			{
				// Token: 0x0400A91B RID: 43291
				public static LocString TITLE = "Time-of-Day Sensor";

				// Token: 0x0400A91C RID: 43292
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after the selected Turn On time, and a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" after the selected Turn Off time"
				});

				// Token: 0x0400A91D RID: 43293
				public static LocString START = "Turn On";

				// Token: 0x0400A91E RID: 43294
				public static LocString STOP = "Turn Off";
			}

			// Token: 0x0200265D RID: 9821
			public class CRITTER_COUNT_SIDE_SCREEN
			{
				// Token: 0x0400A91F RID: 43295
				public static LocString TITLE = "Critter Count Sensor";

				// Token: 0x0400A920 RID: 43296
				public static LocString TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are more than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400A921 RID: 43297
				public static LocString TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are fewer than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400A922 RID: 43298
				public static LocString START = "Turn On";

				// Token: 0x0400A923 RID: 43299
				public static LocString STOP = "Turn Off";

				// Token: 0x0400A924 RID: 43300
				public static LocString VALUE_NAME = "Count";
			}

			// Token: 0x0200265E RID: 9822
			public class OIL_WELL_CAP_SIDE_SCREEN
			{
				// Token: 0x0400A925 RID: 43301
				public static LocString TITLE = "Backpressure Release Threshold";

				// Token: 0x0400A926 RID: 43302
				public static LocString TOOLTIP = "Duplicants will be requested to release backpressure buildup when it exceeds <b>{0}%</b>";
			}

			// Token: 0x0200265F RID: 9823
			public class MODULAR_CONDUIT_PORT_SIDE_SCREEN
			{
				// Token: 0x0400A927 RID: 43303
				public static LocString TITLE = "Pump Control";

				// Token: 0x0400A928 RID: 43304
				public static LocString LABEL_UNLOAD = "Unload Only";

				// Token: 0x0400A929 RID: 43305
				public static LocString LABEL_BOTH = "Load/Unload";

				// Token: 0x0400A92A RID: 43306
				public static LocString LABEL_LOAD = "Load Only";

				// Token: 0x0400A92B RID: 43307
				public static readonly List<LocString> LABELS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_LOAD
				};

				// Token: 0x0400A92C RID: 43308
				public static LocString TOOLTIP_UNLOAD = "This pump will attempt to <b>Unload</b> cargo from the landed rocket, but not attempt to load new cargo";

				// Token: 0x0400A92D RID: 43309
				public static LocString TOOLTIP_BOTH = "This pump will both <b>Load</b> and <b>Unload</b> cargo from the landed rocket";

				// Token: 0x0400A92E RID: 43310
				public static LocString TOOLTIP_LOAD = "This pump will attempt to <b>Load</b> cargo onto the landed rocket, but will not unload it";

				// Token: 0x0400A92F RID: 43311
				public static readonly List<LocString> TOOLTIPS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_LOAD
				};

				// Token: 0x0400A930 RID: 43312
				public static LocString DESCRIPTION = "";
			}

			// Token: 0x02002660 RID: 9824
			public class LOGIC_BUFFER_SIDE_SCREEN
			{
				// Token: 0x0400A931 RID: 43313
				public static LocString TITLE = "Buffer Time";

				// Token: 0x0400A932 RID: 43314
				public static LocString TOOLTIP = "Will continue to send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for <b>{0} seconds</b> after receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002661 RID: 9825
			public class LOGIC_FILTER_SIDE_SCREEN
			{
				// Token: 0x0400A933 RID: 43315
				public static LocString TITLE = "Filter Time";

				// Token: 0x0400A934 RID: 43316
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will only send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if it receives ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than <b>{0} seconds</b>"
				});
			}

			// Token: 0x02002662 RID: 9826
			public class TIME_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400A935 RID: 43317
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400A936 RID: 43318
				public static LocString ON = "Activation Time";

				// Token: 0x0400A937 RID: 43319
				public static LocString ON_TOOLTIP = string.Concat(new string[]
				{
					"Activation time determines the time of day this sensor should begin sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" {0} through the day"
				});

				// Token: 0x0400A938 RID: 43320
				public static LocString DURATION = "Active Duration";

				// Token: 0x0400A939 RID: 43321
				public static LocString DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Active duration determines what percentage of the day this sensor will spend sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0} of the day"
				});
			}

			// Token: 0x02002663 RID: 9827
			public class TIMER_SIDE_SCREEN
			{
				// Token: 0x0400A93A RID: 43322
				public static LocString TITLE = "Timer";

				// Token: 0x0400A93B RID: 43323
				public static LocString ON = "Green Duration";

				// Token: 0x0400A93C RID: 43324
				public static LocString GREEN_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Green duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0}"
				});

				// Token: 0x0400A93D RID: 43325
				public static LocString OFF = "Red Duration";

				// Token: 0x0400A93E RID: 43326
				public static LocString RED_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Red duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" for {0}"
				});

				// Token: 0x0400A93F RID: 43327
				public static LocString CURRENT_TIME = "{0}/{1}";

				// Token: 0x0400A940 RID: 43328
				public static LocString MODE_LABEL_SECONDS = "Mode: Seconds";

				// Token: 0x0400A941 RID: 43329
				public static LocString MODE_LABEL_CYCLES = "Mode: Cycles";

				// Token: 0x0400A942 RID: 43330
				public static LocString RESET_BUTTON = "Reset Timer";

				// Token: 0x0400A943 RID: 43331
				public static LocString DISABLED = "Timer Disabled";
			}

			// Token: 0x02002664 RID: 9828
			public class COUNTER_SIDE_SCREEN
			{
				// Token: 0x0400A944 RID: 43332
				public static LocString TITLE = "Counter";

				// Token: 0x0400A945 RID: 43333
				public static LocString RESET_BUTTON = "Reset Counter";

				// Token: 0x0400A946 RID: 43334
				public static LocString DESCRIPTION = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when count is reached:";

				// Token: 0x0400A947 RID: 43335
				public static LocString INCREMENT_MODE = "Mode: Increment";

				// Token: 0x0400A948 RID: 43336
				public static LocString DECREMENT_MODE = "Mode: Decrement";

				// Token: 0x0400A949 RID: 43337
				public static LocString ADVANCED_MODE = "Advanced Mode";

				// Token: 0x0400A94A RID: 43338
				public static LocString CURRENT_COUNT_SIMPLE = "{0} of ";

				// Token: 0x0400A94B RID: 43339
				public static LocString CURRENT_COUNT_ADVANCED = "{0} % ";

				// Token: 0x02003127 RID: 12583
				public class TOOLTIPS
				{
					// Token: 0x0400C564 RID: 50532
					public static LocString ADVANCED_MODE = string.Concat(new string[]
					{
						"In Advanced Mode, the ",
						BUILDINGS.PREFABS.LOGICCOUNTER.NAME,
						" will count from <b>0</b> rather than <b>1</b>. It will reset when the max is reached, and send a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						" as a brief pulse rather than continuously."
					});
				}
			}

			// Token: 0x02002665 RID: 9829
			public class PASSENGERMODULESIDESCREEN
			{
				// Token: 0x0400A94C RID: 43340
				public static LocString REQUEST_CREW = "Crew";

				// Token: 0x0400A94D RID: 43341
				public static LocString REQUEST_CREW_TOOLTIP = "Crew may not leave the module, non crew-must exit";

				// Token: 0x0400A94E RID: 43342
				public static LocString AUTO_CREW = "Auto";

				// Token: 0x0400A94F RID: 43343
				public static LocString AUTO_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely until the rocket is ready for launch\n\nBefore launch the crew will automatically be requested";

				// Token: 0x0400A950 RID: 43344
				public static LocString RELEASE_CREW = "All";

				// Token: 0x0400A951 RID: 43345
				public static LocString RELEASE_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely";

				// Token: 0x0400A952 RID: 43346
				public static LocString REQUIRE_SUIT_LABEL = "Atmosuit Required";

				// Token: 0x0400A953 RID: 43347
				public static LocString REQUIRE_SUIT_LABEL_TOOLTIP = "If checked, Duplicants will be required to wear an Atmo Suit when entering this rocket";

				// Token: 0x0400A954 RID: 43348
				public static LocString CHANGE_CREW_BUTTON = "Change crew";

				// Token: 0x0400A955 RID: 43349
				public static LocString CHANGE_CREW_BUTTON_TOOLTIP = "Assign Duplicants to crew this rocket's missions";

				// Token: 0x0400A956 RID: 43350
				public static LocString ASSIGNED_TO_CREW = "Assigned to crew";

				// Token: 0x0400A957 RID: 43351
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02002666 RID: 9830
			public class TIMEDSWITCHSIDESCREEN
			{
				// Token: 0x0400A958 RID: 43352
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400A959 RID: 43353
				public static LocString ONTIME = "On Time:";

				// Token: 0x0400A95A RID: 43354
				public static LocString OFFTIME = "Off Time:";

				// Token: 0x0400A95B RID: 43355
				public static LocString TIMETODEACTIVATE = "Time until deactivation: {0}";

				// Token: 0x0400A95C RID: 43356
				public static LocString TIMETOACTIVATE = "Time until activation: {0}";

				// Token: 0x0400A95D RID: 43357
				public static LocString WARNING = "Switch must be connected to a " + UI.FormatAsLink("Power", "POWER") + " grid";

				// Token: 0x0400A95E RID: 43358
				public static LocString CURRENTSTATE = "Current State:";

				// Token: 0x0400A95F RID: 43359
				public static LocString ON = "On";

				// Token: 0x0400A960 RID: 43360
				public static LocString OFF = "Off";
			}

			// Token: 0x02002667 RID: 9831
			public class CAPTURE_POINT_SIDE_SCREEN
			{
				// Token: 0x0400A961 RID: 43361
				public static LocString TITLE = "Stable Management";

				// Token: 0x0400A962 RID: 43362
				public static LocString AUTOWRANGLE = "Auto-Wrangle Surplus";

				// Token: 0x0400A963 RID: 43363
				public static LocString AUTOWRANGLE_TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant will automatically wrangle any critters that exceed the population limit or that do not belong in this stable\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400A964 RID: 43364
				public static LocString LIMIT_TOOLTIP = "Critters exceeding this population limit will automatically be wrangled:";

				// Token: 0x0400A965 RID: 43365
				public static LocString UNITS_SUFFIX = " Critters";
			}

			// Token: 0x02002668 RID: 9832
			public class TEMPERATURESWITCHSIDESCREEN
			{
				// Token: 0x0400A966 RID: 43366
				public static LocString TITLE = "Temperature Threshold";

				// Token: 0x0400A967 RID: 43367
				public static LocString CURRENT_TEMPERATURE = "Current Temperature:\n{0}";

				// Token: 0x0400A968 RID: 43368
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400A969 RID: 43369
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400A96A RID: 43370
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002669 RID: 9833
			public class BRIGHTNESSSWITCHSIDESCREEN
			{
				// Token: 0x0400A96B RID: 43371
				public static LocString TITLE = "Brightness Threshold";

				// Token: 0x0400A96C RID: 43372
				public static LocString CURRENT_TEMPERATURE = "Current Brightness:\n{0}";

				// Token: 0x0400A96D RID: 43373
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400A96E RID: 43374
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400A96F RID: 43375
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x0200266A RID: 9834
			public class RADIATIONSWITCHSIDESCREEN
			{
				// Token: 0x0400A970 RID: 43376
				public static LocString TITLE = "Radiation Threshold";

				// Token: 0x0400A971 RID: 43377
				public static LocString CURRENT_TEMPERATURE = "Current Radiation:\n{0}/cycle";

				// Token: 0x0400A972 RID: 43378
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400A973 RID: 43379
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400A974 RID: 43380
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x0200266B RID: 9835
			public class WATTAGESWITCHSIDESCREEN
			{
				// Token: 0x0400A975 RID: 43381
				public static LocString TITLE = "Wattage Threshold";

				// Token: 0x0400A976 RID: 43382
				public static LocString CURRENT_TEMPERATURE = "Current Wattage:\n{0}";

				// Token: 0x0400A977 RID: 43383
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400A978 RID: 43384
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400A979 RID: 43385
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x0200266C RID: 9836
			public class HEPSWITCHSIDESCREEN
			{
				// Token: 0x0400A97A RID: 43386
				public static LocString TITLE = "Radbolt Threshold";
			}

			// Token: 0x0200266D RID: 9837
			public class THRESHOLD_SWITCH_SIDESCREEN
			{
				// Token: 0x0400A97B RID: 43387
				public static LocString TITLE = "Pressure";

				// Token: 0x0400A97C RID: 43388
				public static LocString THRESHOLD_SUBTITLE = "Threshold:";

				// Token: 0x0400A97D RID: 43389
				public static LocString CURRENT_VALUE = "Current {0}:\n{1}";

				// Token: 0x0400A97E RID: 43390
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400A97F RID: 43391
				public static LocString ABOVE_BUTTON = "Above";

				// Token: 0x0400A980 RID: 43392
				public static LocString BELOW_BUTTON = "Below";

				// Token: 0x0400A981 RID: 43393
				public static LocString STATUS_ACTIVE = "Switch Active";

				// Token: 0x0400A982 RID: 43394
				public static LocString STATUS_INACTIVE = "Switch Inactive";

				// Token: 0x0400A983 RID: 43395
				public static LocString PRESSURE = "Ambient Pressure";

				// Token: 0x0400A984 RID: 43396
				public static LocString PRESSURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A985 RID: 43397
				public static LocString PRESSURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400A986 RID: 43398
				public static LocString TEMPERATURE = "Ambient Temperature";

				// Token: 0x0400A987 RID: 43399
				public static LocString TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A988 RID: 43400
				public static LocString TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400A989 RID: 43401
				public static LocString CONTENT_TEMPERATURE = "Internal Temperature";

				// Token: 0x0400A98A RID: 43402
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is above <b>{0}</b>"
				});

				// Token: 0x0400A98B RID: 43403
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is below <b>{0}</b>"
				});

				// Token: 0x0400A98C RID: 43404
				public static LocString BRIGHTNESS = "Ambient Brightness";

				// Token: 0x0400A98D RID: 43405
				public static LocString BRIGHTNESS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A98E RID: 43406
				public static LocString BRIGHTNESS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400A98F RID: 43407
				public static LocString WATTAGE = "Wattage Reading";

				// Token: 0x0400A990 RID: 43408
				public static LocString WATTAGE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is above <b>{0}</b>"
				});

				// Token: 0x0400A991 RID: 43409
				public static LocString WATTAGE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is below <b>{0}</b>"
				});

				// Token: 0x0400A992 RID: 43410
				public static LocString DISEASE_TITLE = "Germ Threshold";

				// Token: 0x0400A993 RID: 43411
				public static LocString DISEASE = "Ambient Germs";

				// Token: 0x0400A994 RID: 43412
				public static LocString DISEASE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A995 RID: 43413
				public static LocString DISEASE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400A996 RID: 43414
				public static LocString DISEASE_UNITS = "";

				// Token: 0x0400A997 RID: 43415
				public static LocString CONTENT_DISEASE = "Germ Count";

				// Token: 0x0400A998 RID: 43416
				public static LocString RADIATION = "Ambient Radiation";

				// Token: 0x0400A999 RID: 43417
				public static LocString RADIATION_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A99A RID: 43418
				public static LocString RADIATION_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400A99B RID: 43419
				public static LocString HEPS = "Radbolt Reading";

				// Token: 0x0400A99C RID: 43420
				public static LocString HEPS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400A99D RID: 43421
				public static LocString HEPS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});
			}

			// Token: 0x0200266E RID: 9838
			public class CAPACITY_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400A99E RID: 43422
				public static LocString TITLE = "Automated Storage Capacity";

				// Token: 0x0400A99F RID: 43423
				public static LocString MAX_LABEL = "Max:";
			}

			// Token: 0x0200266F RID: 9839
			public class DOOR_TOGGLE_SIDE_SCREEN
			{
				// Token: 0x0400A9A0 RID: 43424
				public static LocString TITLE = "Door Setting";

				// Token: 0x0400A9A1 RID: 43425
				public static LocString OPEN = "Door is open.";

				// Token: 0x0400A9A2 RID: 43426
				public static LocString AUTO = "Door is on auto.";

				// Token: 0x0400A9A3 RID: 43427
				public static LocString CLOSE = "Door is locked.";

				// Token: 0x0400A9A4 RID: 43428
				public static LocString PENDING_FORMAT = "{0} {1}";

				// Token: 0x0400A9A5 RID: 43429
				public static LocString OPEN_PENDING = "Awaiting Duplicant to open door.";

				// Token: 0x0400A9A6 RID: 43430
				public static LocString AUTO_PENDING = "Awaiting Duplicant to automate door.";

				// Token: 0x0400A9A7 RID: 43431
				public static LocString CLOSE_PENDING = "Awaiting Duplicant to lock door.";

				// Token: 0x0400A9A8 RID: 43432
				public static LocString ACCESS_FORMAT = "{0}\n\n{1}";

				// Token: 0x0400A9A9 RID: 43433
				public static LocString ACCESS_OFFLINE = "Emergency Access Permissions:\nAll Duplicants are permitted to use this door until " + UI.FormatAsLink("Power", "POWER") + " is restored.";

				// Token: 0x0400A9AA RID: 43434
				public static LocString POI_INTERNAL = "This door cannot be manually controlled.";
			}

			// Token: 0x02002670 RID: 9840
			public class ACTIVATION_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400A9AB RID: 43435
				public static LocString NAME = "Breaktime Policy";

				// Token: 0x0400A9AC RID: 43436
				public static LocString ACTIVATE = "Break starts at:";

				// Token: 0x0400A9AD RID: 43437
				public static LocString DEACTIVATE = "Break ends at:";
			}

			// Token: 0x02002671 RID: 9841
			public class CAPACITY_SIDE_SCREEN
			{
				// Token: 0x0400A9AE RID: 43438
				public static LocString TOOLTIP = "Adjust the maximum amount that can be stored here";
			}

			// Token: 0x02002672 RID: 9842
			public class SUIT_SIDE_SCREEN
			{
				// Token: 0x0400A9AF RID: 43439
				public static LocString TITLE = "Dock Inventory";

				// Token: 0x0400A9B0 RID: 43440
				public static LocString CONFIGURATION_REQUIRED = "Configuration Required:";

				// Token: 0x0400A9B1 RID: 43441
				public static LocString CONFIG_REQUEST_SUIT = "Deliver Suit";

				// Token: 0x0400A9B2 RID: 43442
				public static LocString CONFIG_REQUEST_SUIT_TOOLTIP = "Duplicants will immediately deliver and dock the nearest unequipped suit";

				// Token: 0x0400A9B3 RID: 43443
				public static LocString CONFIG_NO_SUIT = "Leave Empty";

				// Token: 0x0400A9B4 RID: 43444
				public static LocString CONFIG_NO_SUIT_TOOLTIP = "The next suited Duplicant to pass by will unequip their suit and dock it here";

				// Token: 0x0400A9B5 RID: 43445
				public static LocString CONFIG_CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400A9B6 RID: 43446
				public static LocString CONFIG_CANCEL_REQUEST_TOOLTIP = "Cancel this suit delivery";

				// Token: 0x0400A9B7 RID: 43447
				public static LocString CONFIG_DROP_SUIT = "Undock Suit";

				// Token: 0x0400A9B8 RID: 43448
				public static LocString CONFIG_DROP_SUIT_TOOLTIP = "Disconnect this suit, dropping it on the ground";

				// Token: 0x0400A9B9 RID: 43449
				public static LocString CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP = "There is no suit in this building to undock";
			}

			// Token: 0x02002673 RID: 9843
			public class AUTOMATABLE_SIDE_SCREEN
			{
				// Token: 0x0400A9BA RID: 43450
				public static LocString TITLE = "Automatable Storage";

				// Token: 0x0400A9BB RID: 43451
				public static LocString ALLOWMANUALBUTTON = "Allow Manual Use";

				// Token: 0x0400A9BC RID: 43452
				public static LocString ALLOWMANUALBUTTONTOOLTIP = "Allow Duplicants to manually manage these storage materials";
			}

			// Token: 0x02002674 RID: 9844
			public class STUDYABLE_SIDE_SCREEN
			{
				// Token: 0x0400A9BD RID: 43453
				public static LocString TITLE = "Analyze Natural Feature";

				// Token: 0x0400A9BE RID: 43454
				public static LocString STUDIED_STATUS = "Researchers have completed their analysis and compiled their findings.";

				// Token: 0x0400A9BF RID: 43455
				public static LocString STUDIED_BUTTON = "ANALYSIS COMPLETE";

				// Token: 0x0400A9C0 RID: 43456
				public static LocString SEND_STATUS = "Send a researcher to gather data here.\n\nAnalyzing a feature takes time, but yields useful results.";

				// Token: 0x0400A9C1 RID: 43457
				public static LocString SEND_BUTTON = "ANALYZE";

				// Token: 0x0400A9C2 RID: 43458
				public static LocString PENDING_STATUS = "A researcher is in the process of studying this feature.";

				// Token: 0x0400A9C3 RID: 43459
				public static LocString PENDING_BUTTON = "CANCEL ANALYSIS";
			}

			// Token: 0x02002675 RID: 9845
			public class MEDICALCOTSIDESCREEN
			{
				// Token: 0x0400A9C4 RID: 43460
				public static LocString TITLE = "Severity Requirement";

				// Token: 0x0400A9C5 RID: 43461
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant may not use this cot until their ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002676 RID: 9846
			public class WARPPORTALSIDESCREEN
			{
				// Token: 0x0400A9C6 RID: 43462
				public static LocString TITLE = "Teleporter";

				// Token: 0x0400A9C7 RID: 43463
				public static LocString IDLE = "Teleporter online.\nPlease select a passenger:";

				// Token: 0x0400A9C8 RID: 43464
				public static LocString WAITING = "Ready to transmit passenger.";

				// Token: 0x0400A9C9 RID: 43465
				public static LocString COMPLETE = "Passenger transmitted!";

				// Token: 0x0400A9CA RID: 43466
				public static LocString UNDERWAY = "Transmitting passenger...";

				// Token: 0x0400A9CB RID: 43467
				public static LocString CONSUMED = "Teleporter recharging:";

				// Token: 0x0400A9CC RID: 43468
				public static LocString BUTTON = "Teleport!";

				// Token: 0x0400A9CD RID: 43469
				public static LocString CANCELBUTTON = "Cancel";
			}

			// Token: 0x02002677 RID: 9847
			public class RADBOLTTHRESHOLDSIDESCREEN
			{
				// Token: 0x0400A9CE RID: 43470
				public static LocString TITLE = "Radbolt Threshold";

				// Token: 0x0400A9CF RID: 43471
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400A9D0 RID: 43472
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Releases a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" when stored Radbolts exceed <b>{0}</b>"
				});

				// Token: 0x0400A9D1 RID: 43473
				public static LocString PROGRESS_BAR_LABEL = "Radbolt Generation";

				// Token: 0x0400A9D2 RID: 43474
				public static LocString PROGRESS_BAR_TOOLTIP = string.Concat(new string[]
				{
					"The building will emit a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" in the chosen direction when fully charged"
				});
			}

			// Token: 0x02002678 RID: 9848
			public class LOGICBITSELECTORSIDESCREEN
			{
				// Token: 0x0400A9D3 RID: 43475
				public static LocString RIBBON_READER_TITLE = "Ribbon Reader";

				// Token: 0x0400A9D4 RID: 43476
				public static LocString RIBBON_READER_DESCRIPTION = "Selected <b>Bit's Signal</b> will be read by the <b>Output Port</b>";

				// Token: 0x0400A9D5 RID: 43477
				public static LocString RIBBON_WRITER_TITLE = "Ribbon Writer";

				// Token: 0x0400A9D6 RID: 43478
				public static LocString RIBBON_WRITER_DESCRIPTION = "Received <b>Signal</b> will be written to selected <b>Bit</b>";

				// Token: 0x0400A9D7 RID: 43479
				public static LocString BIT = "Bit {0}";

				// Token: 0x0400A9D8 RID: 43480
				public static LocString STATE_ACTIVE = "Green";

				// Token: 0x0400A9D9 RID: 43481
				public static LocString STATE_INACTIVE = "Red";
			}

			// Token: 0x02002679 RID: 9849
			public class LOGICALARMSIDESCREEN
			{
				// Token: 0x0400A9DA RID: 43482
				public static LocString TITLE = "Notification Designer";

				// Token: 0x0400A9DB RID: 43483
				public static LocString DESCRIPTION = "Notification will be sent upon receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "\n\nMaking modifications will clear any existing notifications being sent by this building.";

				// Token: 0x0400A9DC RID: 43484
				public static LocString NAME = "<b>Name:</b>";

				// Token: 0x0400A9DD RID: 43485
				public static LocString NAME_DEFAULT = "Notification";

				// Token: 0x0400A9DE RID: 43486
				public static LocString TOOLTIP = "<b>Tooltip:</b>";

				// Token: 0x0400A9DF RID: 43487
				public static LocString TOOLTIP_DEFAULT = "Tooltip";

				// Token: 0x0400A9E0 RID: 43488
				public static LocString TYPE = "<b>Type:</b>";

				// Token: 0x0400A9E1 RID: 43489
				public static LocString PAUSE = "<b>Pause:</b>";

				// Token: 0x0400A9E2 RID: 43490
				public static LocString ZOOM = "<b>Zoom:</b>";

				// Token: 0x02003128 RID: 12584
				public class TOOLTIPS
				{
					// Token: 0x0400C565 RID: 50533
					public static LocString NAME = "Select notification text";

					// Token: 0x0400C566 RID: 50534
					public static LocString TOOLTIP = "Select notification hover text";

					// Token: 0x0400C567 RID: 50535
					public static LocString TYPE = "Select the visual and aural style of the notification";

					// Token: 0x0400C568 RID: 50536
					public static LocString PAUSE = "Time will pause upon notification when checked";

					// Token: 0x0400C569 RID: 50537
					public static LocString ZOOM = "The view will zoom to this building upon notification when checked";

					// Token: 0x0400C56A RID: 50538
					public static LocString BAD = "\"Boing boing!\"";

					// Token: 0x0400C56B RID: 50539
					public static LocString NEUTRAL = "\"Pop!\"";

					// Token: 0x0400C56C RID: 50540
					public static LocString DUPLICANT_THREATENING = "AHH!";
				}
			}

			// Token: 0x0200267A RID: 9850
			public class GENETICANALYSISSIDESCREEN
			{
				// Token: 0x0400A9E3 RID: 43491
				public static LocString TITLE = "Genetic Analysis";

				// Token: 0x0400A9E4 RID: 43492
				public static LocString NONE_DISCOVERED = "No mutant seeds have been found.";

				// Token: 0x0400A9E5 RID: 43493
				public static LocString SELECT_SEEDS = "Select which seed types to analyze:";

				// Token: 0x0400A9E6 RID: 43494
				public static LocString SEED_NO_MUTANTS = "</i>No mutants found</i>";

				// Token: 0x0400A9E7 RID: 43495
				public static LocString SEED_FORBIDDEN = "</i>Won't analyze</i>";

				// Token: 0x0400A9E8 RID: 43496
				public static LocString SEED_ALLOWED = "</i>Will analyze</i>";
			}
		}

		// Token: 0x02001DC6 RID: 7622
		public class USERMENUACTIONS
		{
			// Token: 0x0200267B RID: 9851
			public class TRANSITTUBEWAX
			{
				// Token: 0x0400A9E9 RID: 43497
				public static LocString NAME = "Enable Smooth Ride";

				// Token: 0x0400A9EA RID: 43498
				public static LocString TOOLTIP = "Enables the use of " + ELEMENTS.MILKFAT.NAME + " to boost travel speed";
			}

			// Token: 0x0200267C RID: 9852
			public class CANCELTRANSITTUBEWAX
			{
				// Token: 0x0400A9EB RID: 43499
				public static LocString NAME = "Disable Smooth Ride";

				// Token: 0x0400A9EC RID: 43500
				public static LocString TOOLTIP = "Disables travel speed boost and refunds stored " + ELEMENTS.MILKFAT.NAME;
			}

			// Token: 0x0200267D RID: 9853
			public class CLEANTOILET
			{
				// Token: 0x0400A9ED RID: 43501
				public static LocString NAME = "Clean Toilet";

				// Token: 0x0400A9EE RID: 43502
				public static LocString TOOLTIP = "Empty waste from this toilet";
			}

			// Token: 0x0200267E RID: 9854
			public class CANCELCLEANTOILET
			{
				// Token: 0x0400A9EF RID: 43503
				public static LocString NAME = "Cancel Clean";

				// Token: 0x0400A9F0 RID: 43504
				public static LocString TOOLTIP = "Cancel this cleaning order";
			}

			// Token: 0x0200267F RID: 9855
			public class EMPTYBEEHIVE
			{
				// Token: 0x0400A9F1 RID: 43505
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400A9F2 RID: 43506
				public static LocString TOOLTIP = "Automatically harvest this hive when full";
			}

			// Token: 0x02002680 RID: 9856
			public class CANCELEMPTYBEEHIVE
			{
				// Token: 0x0400A9F3 RID: 43507
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400A9F4 RID: 43508
				public static LocString TOOLTIP = "Do not automatically harvest this hive";
			}

			// Token: 0x02002681 RID: 9857
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400A9F5 RID: 43509
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400A9F6 RID: 43510
				public static LocString TOOLTIP = "Empty salt from this desalinator";
			}

			// Token: 0x02002682 RID: 9858
			public class CHANGE_ROOM
			{
				// Token: 0x0400A9F7 RID: 43511
				public static LocString REQUEST_OUTFIT = "Request Outfit";

				// Token: 0x0400A9F8 RID: 43512
				public static LocString REQUEST_OUTFIT_TOOLTIP = "Request outfit to be delivered to this change room";

				// Token: 0x0400A9F9 RID: 43513
				public static LocString CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400A9FA RID: 43514
				public static LocString CANCEL_REQUEST_TOOLTIP = "Cancel outfit request";

				// Token: 0x0400A9FB RID: 43515
				public static LocString DROP_OUTFIT = "Drop Outfit";

				// Token: 0x0400A9FC RID: 43516
				public static LocString DROP_OUTFIT_TOOLTIP = "Drop outfit on floor";
			}

			// Token: 0x02002683 RID: 9859
			public class DUMP
			{
				// Token: 0x0400A9FD RID: 43517
				public static LocString NAME = "Empty";

				// Token: 0x0400A9FE RID: 43518
				public static LocString TOOLTIP = "Dump bottle contents onto the floor";

				// Token: 0x0400A9FF RID: 43519
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400AA00 RID: 43520
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002684 RID: 9860
			public class TAGFILTER
			{
				// Token: 0x0400AA01 RID: 43521
				public static LocString NAME = "Filter Settings";

				// Token: 0x0400AA02 RID: 43522
				public static LocString TOOLTIP = "Assign materials to storage";
			}

			// Token: 0x02002685 RID: 9861
			public class CANCELCONSTRUCTION
			{
				// Token: 0x0400AA03 RID: 43523
				public static LocString NAME = "Cancel Build";

				// Token: 0x0400AA04 RID: 43524
				public static LocString TOOLTIP = "Cancel this build order";
			}

			// Token: 0x02002686 RID: 9862
			public class DIG
			{
				// Token: 0x0400AA05 RID: 43525
				public static LocString NAME = "Dig";

				// Token: 0x0400AA06 RID: 43526
				public static LocString TOOLTIP = "Dig out this cell";

				// Token: 0x0400AA07 RID: 43527
				public static LocString TOOLTIP_OFF = "Cancel this dig order";
			}

			// Token: 0x02002687 RID: 9863
			public class CANCELMOP
			{
				// Token: 0x0400AA08 RID: 43528
				public static LocString NAME = "Cancel Mop";

				// Token: 0x0400AA09 RID: 43529
				public static LocString TOOLTIP = "Cancel this mop order";
			}

			// Token: 0x02002688 RID: 9864
			public class CANCELDIG
			{
				// Token: 0x0400AA0A RID: 43530
				public static LocString NAME = "Cancel Dig";

				// Token: 0x0400AA0B RID: 43531
				public static LocString TOOLTIP = "Cancel this dig order";
			}

			// Token: 0x02002689 RID: 9865
			public class UPROOT
			{
				// Token: 0x0400AA0C RID: 43532
				public static LocString NAME = "Uproot";

				// Token: 0x0400AA0D RID: 43533
				public static LocString TOOLTIP = "Convert this plant into a seed";
			}

			// Token: 0x0200268A RID: 9866
			public class CANCELUPROOT
			{
				// Token: 0x0400AA0E RID: 43534
				public static LocString NAME = "Cancel Uproot";

				// Token: 0x0400AA0F RID: 43535
				public static LocString TOOLTIP = "Cancel this uproot order";
			}

			// Token: 0x0200268B RID: 9867
			public class HARVEST_WHEN_READY
			{
				// Token: 0x0400AA10 RID: 43536
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400AA11 RID: 43537
				public static LocString TOOLTIP = "Automatically harvest this plant when it matures";
			}

			// Token: 0x0200268C RID: 9868
			public class CANCEL_HARVEST_WHEN_READY
			{
				// Token: 0x0400AA12 RID: 43538
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400AA13 RID: 43539
				public static LocString TOOLTIP = "Do not automatically harvest this plant";
			}

			// Token: 0x0200268D RID: 9869
			public class HARVEST
			{
				// Token: 0x0400AA14 RID: 43540
				public static LocString NAME = "Harvest";

				// Token: 0x0400AA15 RID: 43541
				public static LocString TOOLTIP = "Harvest materials from this plant";

				// Token: 0x0400AA16 RID: 43542
				public static LocString TOOLTIP_DISABLED = "This plant has nothing to harvest";
			}

			// Token: 0x0200268E RID: 9870
			public class CANCELHARVEST
			{
				// Token: 0x0400AA17 RID: 43543
				public static LocString NAME = "Cancel Harvest";

				// Token: 0x0400AA18 RID: 43544
				public static LocString TOOLTIP = "Cancel this harvest order";
			}

			// Token: 0x0200268F RID: 9871
			public class ATTACK
			{
				// Token: 0x0400AA19 RID: 43545
				public static LocString NAME = "Attack";

				// Token: 0x0400AA1A RID: 43546
				public static LocString TOOLTIP = "Attack this critter";
			}

			// Token: 0x02002690 RID: 9872
			public class CANCELATTACK
			{
				// Token: 0x0400AA1B RID: 43547
				public static LocString NAME = "Cancel Attack";

				// Token: 0x0400AA1C RID: 43548
				public static LocString TOOLTIP = "Cancel this attack order";
			}

			// Token: 0x02002691 RID: 9873
			public class CAPTURE
			{
				// Token: 0x0400AA1D RID: 43549
				public static LocString NAME = "Wrangle";

				// Token: 0x0400AA1E RID: 43550
				public static LocString TOOLTIP = "Capture this critter alive";
			}

			// Token: 0x02002692 RID: 9874
			public class CANCELCAPTURE
			{
				// Token: 0x0400AA1F RID: 43551
				public static LocString NAME = "Cancel Wrangle";

				// Token: 0x0400AA20 RID: 43552
				public static LocString TOOLTIP = "Cancel this wrangle order";
			}

			// Token: 0x02002693 RID: 9875
			public class RELEASEELEMENT
			{
				// Token: 0x0400AA21 RID: 43553
				public static LocString NAME = "Empty Building";

				// Token: 0x0400AA22 RID: 43554
				public static LocString TOOLTIP = "Refund all resources currently in use by this building";
			}

			// Token: 0x02002694 RID: 9876
			public class DECONSTRUCT
			{
				// Token: 0x0400AA23 RID: 43555
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400AA24 RID: 43556
				public static LocString TOOLTIP = "Deconstruct this building and refund all resources";

				// Token: 0x0400AA25 RID: 43557
				public static LocString NAME_OFF = "Cancel Deconstruct";

				// Token: 0x0400AA26 RID: 43558
				public static LocString TOOLTIP_OFF = "Cancel this deconstruct order";
			}

			// Token: 0x02002695 RID: 9877
			public class DEMOLISH
			{
				// Token: 0x0400AA27 RID: 43559
				public static LocString NAME = "Demolish";

				// Token: 0x0400AA28 RID: 43560
				public static LocString TOOLTIP = "Demolish this building";

				// Token: 0x0400AA29 RID: 43561
				public static LocString NAME_OFF = "Cancel Demolition";

				// Token: 0x0400AA2A RID: 43562
				public static LocString TOOLTIP_OFF = "Cancel this demolition order";
			}

			// Token: 0x02002696 RID: 9878
			public class ROCKETUSAGERESTRICTION
			{
				// Token: 0x0400AA2B RID: 43563
				public static LocString NAME_UNCONTROLLED = "Uncontrolled";

				// Token: 0x0400AA2C RID: 43564
				public static LocString TOOLTIP_UNCONTROLLED = "Do not allow this building to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;

				// Token: 0x0400AA2D RID: 43565
				public static LocString NAME_CONTROLLED = "Controlled";

				// Token: 0x0400AA2E RID: 43566
				public static LocString TOOLTIP_CONTROLLED = "Allow this building's operation to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002697 RID: 9879
			public class MANUAL_DELIVERY
			{
				// Token: 0x0400AA2F RID: 43567
				public static LocString NAME = "Disable Delivery";

				// Token: 0x0400AA30 RID: 43568
				public static LocString TOOLTIP = "Do not deliver materials to this building";

				// Token: 0x0400AA31 RID: 43569
				public static LocString NAME_OFF = "Enable Delivery";

				// Token: 0x0400AA32 RID: 43570
				public static LocString TOOLTIP_OFF = "Deliver materials to this building";
			}

			// Token: 0x02002698 RID: 9880
			public class SELECTRESEARCH
			{
				// Token: 0x0400AA33 RID: 43571
				public static LocString NAME = "Select Research";

				// Token: 0x0400AA34 RID: 43572
				public static LocString TOOLTIP = "Choose a technology from the " + UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch);
			}

			// Token: 0x02002699 RID: 9881
			public class RELOCATE
			{
				// Token: 0x0400AA35 RID: 43573
				public static LocString NAME = "Relocate";

				// Token: 0x0400AA36 RID: 43574
				public static LocString TOOLTIP = "Move this building to a new location\n\nCosts no additional resources";

				// Token: 0x0400AA37 RID: 43575
				public static LocString NAME_OFF = "Cancel Relocation";

				// Token: 0x0400AA38 RID: 43576
				public static LocString TOOLTIP_OFF = "Cancel this relocation order";
			}

			// Token: 0x0200269A RID: 9882
			public class ENABLEBUILDING
			{
				// Token: 0x0400AA39 RID: 43577
				public static LocString NAME = "Disable Building";

				// Token: 0x0400AA3A RID: 43578
				public static LocString TOOLTIP = "Halt the use of this building {Hotkey}\n\nDisabled buildings consume no energy or resources";

				// Token: 0x0400AA3B RID: 43579
				public static LocString NAME_OFF = "Enable Building";

				// Token: 0x0400AA3C RID: 43580
				public static LocString TOOLTIP_OFF = "Resume the use of this building {Hotkey}";
			}

			// Token: 0x0200269B RID: 9883
			public class READLORE
			{
				// Token: 0x0400AA3D RID: 43581
				public static LocString NAME = "Inspect";

				// Token: 0x0400AA3E RID: 43582
				public static LocString ALREADYINSPECTED = "Already inspected";

				// Token: 0x0400AA3F RID: 43583
				public static LocString TOOLTIP = "Recover files from this structure";

				// Token: 0x0400AA40 RID: 43584
				public static LocString TOOLTIP_ALREADYINSPECTED = "This structure has already been inspected";

				// Token: 0x0400AA41 RID: 43585
				public static LocString GOTODATABASE = "View Entry";

				// Token: 0x0400AA42 RID: 43586
				public static LocString SEARCH_DISPLAY = "The display is still functional. I copy its message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA43 RID: 43587
				public static LocString SEARCH_ELLIESDESK = "All I find on the machine is a curt e-mail from a disgruntled employee.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA44 RID: 43588
				public static LocString SEARCH_POD = "I search my incoming message history and find a single entry. I move the odd message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA45 RID: 43589
				public static LocString ALREADY_SEARCHED = "I already took everything of interest from this. I can check the Database to re-read what I found.";

				// Token: 0x0400AA46 RID: 43590
				public static LocString SEARCH_CABINET = "One intact document remains - an old yellowing newspaper clipping. It won't be of much use, but I add it to my database nonetheless.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA47 RID: 43591
				public static LocString SEARCH_STERNSDESK = "There's an old magazine article from a publication called the \"Nucleoid\" tucked in the top drawer. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA48 RID: 43592
				public static LocString ALREADY_SEARCHED_STERNSDESK = "The desk is eerily empty inside.";

				// Token: 0x0400AA49 RID: 43593
				public static LocString SEARCH_TELEPORTER_SENDER = "While scanning the antiquated computer code of this machine I uncovered some research notes. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA4A RID: 43594
				public static LocString SEARCH_TELEPORTER_RECEIVER = "Incongruously placed research notes are hidden within the operating instructions of this device. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA4B RID: 43595
				public static LocString SEARCH_CRYO_TANK = "There are some safety instructions included in the operating instructions of this Cryotank. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400AA4C RID: 43596
				public static LocString SEARCH_PROPGRAVITASCREATUREPOSTER = "There's a handwritten note taped to the back of this poster. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x02003129 RID: 12585
				public class SEARCH_COMPUTER_PODIUM
				{
					// Token: 0x0400C56D RID: 50541
					public static LocString SEARCH1 = "I search through the computer's database and find an unredacted e-mail.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x0200312A RID: 12586
				public class SEARCH_COMPUTER_SUCCESS
				{
					// Token: 0x0400C56E RID: 50542
					public static LocString SEARCH1 = "After searching through the computer's database, I managed to piece together some files that piqued my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C56F RID: 50543
					public static LocString SEARCH2 = "Searching through the computer, I find some recoverable files that are still readable.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C570 RID: 50544
					public static LocString SEARCH3 = "The computer looks pristine on the outside, but is corrupted internally. Still, I managed to find one uncorrupted file, and have added it to my database.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C571 RID: 50545
					public static LocString SEARCH4 = "The computer was wiped almost completely clean, except for one file hidden in the recycle bin.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C572 RID: 50546
					public static LocString SEARCH5 = "I search the computer, storing what useful data I can find in my own memory.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C573 RID: 50547
					public static LocString SEARCH6 = "This computer is broken and requires some finessing to get working. Still, I recover a handful of interesting files.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x0200312B RID: 12587
				public class SEARCH_COMPUTER_FAIL
				{
					// Token: 0x0400C574 RID: 50548
					public static LocString SEARCH1 = "Unfortunately, the computer's hard drive is irreparably corrupted.";

					// Token: 0x0400C575 RID: 50549
					public static LocString SEARCH2 = "The computer was wiped clean before I got here. There is nothing to recover.";

					// Token: 0x0400C576 RID: 50550
					public static LocString SEARCH3 = "Some intact files are available on the computer, but nothing I haven't already discovered elsewhere. I find nothing else.";

					// Token: 0x0400C577 RID: 50551
					public static LocString SEARCH4 = "The computer has nothing of import.";

					// Token: 0x0400C578 RID: 50552
					public static LocString SEARCH5 = "Someone's left a solitaire game up. There's nothing else of interest on the computer.\n\nAlso, it looks as though they were about to lose.";

					// Token: 0x0400C579 RID: 50553
					public static LocString SEARCH6 = "The background on this computer depicts two kittens hugging in a field of daisies. There is nothing else of import to be found.";

					// Token: 0x0400C57A RID: 50554
					public static LocString SEARCH7 = "The user alphabetized the shortcuts on their desktop. There is nothing else of import to be found.";

					// Token: 0x0400C57B RID: 50555
					public static LocString SEARCH8 = "The background is a picture of a golden retriever in a science lab. It looks very confused. There is nothing else of import to be found.";

					// Token: 0x0400C57C RID: 50556
					public static LocString SEARCH9 = "This user never changed their default background. There is nothing else of import to be found. How dull.";
				}

				// Token: 0x0200312C RID: 12588
				public class SEARCH_TECHNOLOGY_SUCCESS
				{
					// Token: 0x0400C57D RID: 50557
					public static LocString SEARCH1 = "I scour the internal systems and find something of interest.\n\nNew Database Entry discovered.";

					// Token: 0x0400C57E RID: 50558
					public static LocString SEARCH2 = "I see if I can salvage anything from the electronics. I add what I find to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400C57F RID: 50559
					public static LocString SEARCH3 = "I look for anything of interest within the abandoned machinery and add what I find to my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x0200312D RID: 12589
				public class SEARCH_OBJECT_SUCCESS
				{
					// Token: 0x0400C580 RID: 50560
					public static LocString SEARCH1 = "I look around and recover an old file.\n\nNew Database Entry discovered.";

					// Token: 0x0400C581 RID: 50561
					public static LocString SEARCH2 = "There's a three-ringed binder inside. I scan the surviving documents.\n\nNew Database Entry discovered.";

					// Token: 0x0400C582 RID: 50562
					public static LocString SEARCH3 = "A discarded journal inside remains mostly intact. I scan the pages of use.\n\nNew Database Entry discovered.";

					// Token: 0x0400C583 RID: 50563
					public static LocString SEARCH4 = "A single page of a long printout remains legible. I scan it and add it to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400C584 RID: 50564
					public static LocString SEARCH5 = "A few loose papers can be found inside. I scan the ones that look interesting.\n\nNew Database Entry discovered.";

					// Token: 0x0400C585 RID: 50565
					public static LocString SEARCH6 = "I find a memory stick inside and copy its data into my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x0200312E RID: 12590
				public class SEARCH_OBJECT_FAIL
				{
					// Token: 0x0400C586 RID: 50566
					public static LocString SEARCH1 = "I look around but find nothing of interest.";
				}

				// Token: 0x0200312F RID: 12591
				public class SEARCH_SPACEPOI_SUCCESS
				{
					// Token: 0x0400C587 RID: 50567
					public static LocString SEARCH1 = "A quick analysis of the hardware of this debris has uncovered some searchable files within.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C588 RID: 50568
					public static LocString SEARCH2 = "There's an archaic interface I can interact with on this device.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C589 RID: 50569
					public static LocString SEARCH3 = "While investigating the software of this wreckage, a compelling file comes to my attention.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C58A RID: 50570
					public static LocString SEARCH4 = "Not much remains of the software that once ran this spacecraft except for one file that piques my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C58B RID: 50571
					public static LocString SEARCH5 = "I find some noteworthy data hidden amongst the system files of this space junk.\n\nNew Database Entry unlocked.";

					// Token: 0x0400C58C RID: 50572
					public static LocString SEARCH6 = "Despite being subjected to years of degradation, there are still recoverable files in this machinery.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003130 RID: 12592
				public class SEARCH_SPACEPOI_FAIL
				{
					// Token: 0x0400C58D RID: 50573
					public static LocString SEARCH1 = "There's nothing of interest left in this old space junk.";

					// Token: 0x0400C58E RID: 50574
					public static LocString SEARCH2 = "I've salvaged everything I can from this vehicle.";

					// Token: 0x0400C58F RID: 50575
					public static LocString SEARCH3 = "Years of neglect and radioactive decay have destroyed all the useful data from this derelict spacecraft.";
				}
			}

			// Token: 0x0200269C RID: 9884
			public class OPENPOI
			{
				// Token: 0x0400AA4D RID: 43597
				public static LocString NAME = "Rummage";

				// Token: 0x0400AA4E RID: 43598
				public static LocString TOOLTIP = "Scrounge for usable materials";

				// Token: 0x0400AA4F RID: 43599
				public static LocString NAME_OFF = "Cancel Rummage";

				// Token: 0x0400AA50 RID: 43600
				public static LocString TOOLTIP_OFF = "Cancel this rummage order";
			}

			// Token: 0x0200269D RID: 9885
			public class EMPTYSTORAGE
			{
				// Token: 0x0400AA51 RID: 43601
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400AA52 RID: 43602
				public static LocString TOOLTIP = "Eject all resources from this container";

				// Token: 0x0400AA53 RID: 43603
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400AA54 RID: 43604
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x0200269E RID: 9886
			public class COPY_BUILDING_SETTINGS
			{
				// Token: 0x0400AA55 RID: 43605
				public static LocString NAME = "Copy Settings";

				// Token: 0x0400AA56 RID: 43606
				public static LocString TOOLTIP = "Apply the settings and priorities of this building to other buildings of the same type {Hotkey}";
			}

			// Token: 0x0200269F RID: 9887
			public class CLEAR
			{
				// Token: 0x0400AA57 RID: 43607
				public static LocString NAME = "Sweep";

				// Token: 0x0400AA58 RID: 43608
				public static LocString TOOLTIP = "Put this object away in the nearest storage container";

				// Token: 0x0400AA59 RID: 43609
				public static LocString NAME_OFF = "Cancel Sweeping";

				// Token: 0x0400AA5A RID: 43610
				public static LocString TOOLTIP_OFF = "Cancel this sweep order";
			}

			// Token: 0x020026A0 RID: 9888
			public class COMPOST
			{
				// Token: 0x0400AA5B RID: 43611
				public static LocString NAME = "Compost";

				// Token: 0x0400AA5C RID: 43612
				public static LocString TOOLTIP = "Mark this object for compost";

				// Token: 0x0400AA5D RID: 43613
				public static LocString NAME_OFF = "Cancel Compost";

				// Token: 0x0400AA5E RID: 43614
				public static LocString TOOLTIP_OFF = "Cancel this compost order";
			}

			// Token: 0x020026A1 RID: 9889
			public class PICKUPABLEMOVE
			{
				// Token: 0x0400AA5F RID: 43615
				public static LocString NAME = "Move To";

				// Token: 0x0400AA60 RID: 43616
				public static LocString TOOLTIP = "Move this object to a specific location";

				// Token: 0x0400AA61 RID: 43617
				public static LocString NAME_OFF = "Cancel Move";

				// Token: 0x0400AA62 RID: 43618
				public static LocString TOOLTIP_OFF = "Cancel order to move this object";
			}

			// Token: 0x020026A2 RID: 9890
			public class UNEQUIP
			{
				// Token: 0x0400AA63 RID: 43619
				public static LocString NAME = "Unequip {0}";

				// Token: 0x0400AA64 RID: 43620
				public static LocString TOOLTIP = "Take off and drop this equipment";
			}

			// Token: 0x020026A3 RID: 9891
			public class QUARANTINE
			{
				// Token: 0x0400AA65 RID: 43621
				public static LocString NAME = "Quarantine";

				// Token: 0x0400AA66 RID: 43622
				public static LocString TOOLTIP = "Isolate this Duplicant\nThe Duplicant will return to their assigned Cot";

				// Token: 0x0400AA67 RID: 43623
				public static LocString TOOLTIP_DISABLED = "No quarantine zone assigned";

				// Token: 0x0400AA68 RID: 43624
				public static LocString NAME_OFF = "Cancel Quarantine";

				// Token: 0x0400AA69 RID: 43625
				public static LocString TOOLTIP_OFF = "Cancel this quarantine order";
			}

			// Token: 0x020026A4 RID: 9892
			public class DRAWPATHS
			{
				// Token: 0x0400AA6A RID: 43626
				public static LocString NAME = "Show Navigation";

				// Token: 0x0400AA6B RID: 43627
				public static LocString TOOLTIP = "Show all areas within this Duplicant's reach";

				// Token: 0x0400AA6C RID: 43628
				public static LocString NAME_OFF = "Hide Navigation";

				// Token: 0x0400AA6D RID: 43629
				public static LocString TOOLTIP_OFF = "Hide areas within this Duplicant's reach";
			}

			// Token: 0x020026A5 RID: 9893
			public class MOVETOLOCATION
			{
				// Token: 0x0400AA6E RID: 43630
				public static LocString NAME = "Move To";

				// Token: 0x0400AA6F RID: 43631
				public static LocString TOOLTIP = "Move this Duplicant to a specific location";
			}

			// Token: 0x020026A6 RID: 9894
			public class FOLLOWCAM
			{
				// Token: 0x0400AA70 RID: 43632
				public static LocString NAME = "Follow Cam";

				// Token: 0x0400AA71 RID: 43633
				public static LocString TOOLTIP = "Track this Duplicant with the camera";
			}

			// Token: 0x020026A7 RID: 9895
			public class WORKABLE_DIRECTION_BOTH
			{
				// Token: 0x0400AA72 RID: 43634
				public static LocString NAME = " Set Direction: Both";

				// Token: 0x0400AA73 RID: 43635
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by in either direction";
			}

			// Token: 0x020026A8 RID: 9896
			public class WORKABLE_DIRECTION_LEFT
			{
				// Token: 0x0400AA74 RID: 43636
				public static LocString NAME = "Set Direction: Left";

				// Token: 0x0400AA75 RID: 43637
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from right to left";
			}

			// Token: 0x020026A9 RID: 9897
			public class WORKABLE_DIRECTION_RIGHT
			{
				// Token: 0x0400AA76 RID: 43638
				public static LocString NAME = "Set Direction: Right";

				// Token: 0x0400AA77 RID: 43639
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from left to right";
			}

			// Token: 0x020026AA RID: 9898
			public class MANUAL_PUMP_DELIVERY
			{
				// Token: 0x02003131 RID: 12593
				public static class ALLOWED
				{
					// Token: 0x0400C590 RID: 50576
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400C591 RID: 50577
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver bottled liquids to this building directly from Pitcher Pumps";
				}

				// Token: 0x02003132 RID: 12594
				public static class DENIED
				{
					// Token: 0x0400C592 RID: 50578
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400C593 RID: 50579
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver bottled liquids directly from Pitcher Pumps";
				}

				// Token: 0x02003133 RID: 12595
				public static class ALLOWED_GAS
				{
					// Token: 0x0400C594 RID: 50580
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400C595 RID: 50581
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver gas canisters to this building directly from Canister Fillers";
				}

				// Token: 0x02003134 RID: 12596
				public static class DENIED_GAS
				{
					// Token: 0x0400C596 RID: 50582
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400C597 RID: 50583
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver gas canisters directly from Canister Fillers";
				}
			}

			// Token: 0x020026AB RID: 9899
			public class SUIT_MARKER_TRAVERSAL
			{
				// Token: 0x02003135 RID: 12597
				public static class ONLY_WHEN_ROOM_AVAILABLE
				{
					// Token: 0x0400C598 RID: 50584
					public static LocString NAME = "Clearance: Vacancy";

					// Token: 0x0400C599 RID: 50585
					public static LocString TOOLTIP = "Suited Duplicants may only pass if there is an available dock to store their suit";
				}

				// Token: 0x02003136 RID: 12598
				public static class ALWAYS
				{
					// Token: 0x0400C59A RID: 50586
					public static LocString NAME = "Clearance: Always";

					// Token: 0x0400C59B RID: 50587
					public static LocString TOOLTIP = "Suited Duplicants may pass even if there is no room to store their suits\n\nWhen all available docks are full, Duplicants will unequip their suits and drop them on the floor";
				}
			}

			// Token: 0x020026AC RID: 9900
			public class ACTIVATEBUILDING
			{
				// Token: 0x0400AA78 RID: 43640
				public static LocString ACTIVATE = "Activate";

				// Token: 0x0400AA79 RID: 43641
				public static LocString TOOLTIP_ACTIVATE = "Request a Duplicant to activate this building";

				// Token: 0x0400AA7A RID: 43642
				public static LocString TOOLTIP_ACTIVATED = "This building has already been activated";

				// Token: 0x0400AA7B RID: 43643
				public static LocString ACTIVATE_CANCEL = "Cancel Activation";

				// Token: 0x0400AA7C RID: 43644
				public static LocString ACTIVATED = "Activated";

				// Token: 0x0400AA7D RID: 43645
				public static LocString TOOLTIP_CANCEL = "Cancel activation of this building";
			}

			// Token: 0x020026AD RID: 9901
			public class ACCEPT_MUTANT_SEEDS
			{
				// Token: 0x0400AA7E RID: 43646
				public static LocString ACCEPT = "Allow Mutants";

				// Token: 0x0400AA7F RID: 43647
				public static LocString REJECT = "Forbid Mutants";

				// Token: 0x0400AA80 RID: 43648
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this building will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for recipes that could use them"
				});

				// Token: 0x0400AA81 RID: 43649
				public static LocString FISH_FEEDER_TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this feeder will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for critters who eat them"
				});
			}
		}

		// Token: 0x02001DC7 RID: 7623
		public class BUILDCATEGORIES
		{
			// Token: 0x020026AE RID: 9902
			public static class BASE
			{
				// Token: 0x0400AA82 RID: 43650
				public static LocString NAME = UI.FormatAsLink("Base", "BUILDCATEGORYBASE");

				// Token: 0x0400AA83 RID: 43651
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x020026AF RID: 9903
			public static class CONVEYANCE
			{
				// Token: 0x0400AA84 RID: 43652
				public static LocString NAME = UI.FormatAsLink("Shipping", "BUILDCATEGORYCONVEYANCE");

				// Token: 0x0400AA85 RID: 43653
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x020026B0 RID: 9904
			public static class OXYGEN
			{
				// Token: 0x0400AA86 RID: 43654
				public static LocString NAME = UI.FormatAsLink("Oxygen", "BUILDCATEGORYOXYGEN");

				// Token: 0x0400AA87 RID: 43655
				public static LocString TOOLTIP = "Everything I need to keep the colony breathing. {Hotkey}";
			}

			// Token: 0x020026B1 RID: 9905
			public static class POWER
			{
				// Token: 0x0400AA88 RID: 43656
				public static LocString NAME = UI.FormatAsLink("Power", "BUILDCATEGORYPOWER");

				// Token: 0x0400AA89 RID: 43657
				public static LocString TOOLTIP = "Need to power the colony? Here's how to do it! {Hotkey}";
			}

			// Token: 0x020026B2 RID: 9906
			public static class FOOD
			{
				// Token: 0x0400AA8A RID: 43658
				public static LocString NAME = UI.FormatAsLink("Food", "BUILDCATEGORYFOOD");

				// Token: 0x0400AA8B RID: 43659
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x020026B3 RID: 9907
			public static class UTILITIES
			{
				// Token: 0x0400AA8C RID: 43660
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILDCATEGORYUTILITIES");

				// Token: 0x0400AA8D RID: 43661
				public static LocString TOOLTIP = "Heat up and cool down. {Hotkey}";
			}

			// Token: 0x020026B4 RID: 9908
			public static class PLUMBING
			{
				// Token: 0x0400AA8E RID: 43662
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BUILDCATEGORYPLUMBING");

				// Token: 0x0400AA8F RID: 43663
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x020026B5 RID: 9909
			public static class HVAC
			{
				// Token: 0x0400AA90 RID: 43664
				public static LocString NAME = UI.FormatAsLink("Ventilation", "BUILDCATEGORYHVAC");

				// Token: 0x0400AA91 RID: 43665
				public static LocString TOOLTIP = "Control the flow of gas in the base. {Hotkey}";
			}

			// Token: 0x020026B6 RID: 9910
			public static class REFINING
			{
				// Token: 0x0400AA92 RID: 43666
				public static LocString NAME = UI.FormatAsLink("Refinement", "BUILDCATEGORYREFINING");

				// Token: 0x0400AA93 RID: 43667
				public static LocString TOOLTIP = "Use the resources I want, filter the ones I don't. {Hotkey}";
			}

			// Token: 0x020026B7 RID: 9911
			public static class ROCKETRY
			{
				// Token: 0x0400AA94 RID: 43668
				public static LocString NAME = UI.FormatAsLink("Rocketry", "BUILDCATEGORYROCKETRY");

				// Token: 0x0400AA95 RID: 43669
				public static LocString TOOLTIP = "With rockets, the sky's no longer the limit! {Hotkey}";
			}

			// Token: 0x020026B8 RID: 9912
			public static class MEDICAL
			{
				// Token: 0x0400AA96 RID: 43670
				public static LocString NAME = UI.FormatAsLink("Medicine", "BUILDCATEGORYMEDICAL");

				// Token: 0x0400AA97 RID: 43671
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x020026B9 RID: 9913
			public static class FURNITURE
			{
				// Token: 0x0400AA98 RID: 43672
				public static LocString NAME = UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE");

				// Token: 0x0400AA99 RID: 43673
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x020026BA RID: 9914
			public static class EQUIPMENT
			{
				// Token: 0x0400AA9A RID: 43674
				public static LocString NAME = UI.FormatAsLink("Stations", "BUILDCATEGORYEQUIPMENT");

				// Token: 0x0400AA9B RID: 43675
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x020026BB RID: 9915
			public static class MISC
			{
				// Token: 0x0400AA9C RID: 43676
				public static LocString NAME = UI.FormatAsLink("Decor", "BUILDCATEGORYMISC");

				// Token: 0x0400AA9D RID: 43677
				public static LocString TOOLTIP = "Spruce up my colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x020026BC RID: 9916
			public static class AUTOMATION
			{
				// Token: 0x0400AA9E RID: 43678
				public static LocString NAME = UI.FormatAsLink("Automation", "BUILDCATEGORYAUTOMATION");

				// Token: 0x0400AA9F RID: 43679
				public static LocString TOOLTIP = "Automate my base with a wide range of sensors. {Hotkey}";
			}

			// Token: 0x020026BD RID: 9917
			public static class HEP
			{
				// Token: 0x0400AAA0 RID: 43680
				public static LocString NAME = UI.FormatAsLink("Radiation", "BUILDCATEGORYHEP");

				// Token: 0x0400AAA1 RID: 43681
				public static LocString TOOLTIP = "Here's where things get rad. {Hotkey}";
			}
		}

		// Token: 0x02001DC8 RID: 7624
		public class NEWBUILDCATEGORIES
		{
			// Token: 0x020026BE RID: 9918
			public static class BASE
			{
				// Token: 0x0400AAA2 RID: 43682
				public static LocString NAME = UI.FormatAsLink("Base", "BUILD_CATEGORY_BASE");

				// Token: 0x0400AAA3 RID: 43683
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x020026BF RID: 9919
			public static class INFRASTRUCTURE
			{
				// Token: 0x0400AAA4 RID: 43684
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILD_CATEGORY_INFRASTRUCTURE");

				// Token: 0x0400AAA5 RID: 43685
				public static LocString TOOLTIP = "Power, plumbing, and ventilation can all be found here. {Hotkey}";
			}

			// Token: 0x020026C0 RID: 9920
			public static class FOODANDAGRICULTURE
			{
				// Token: 0x0400AAA6 RID: 43686
				public static LocString NAME = UI.FormatAsLink("Food", "BUILD_CATEGORY_FOODANDAGRICULTURE");

				// Token: 0x0400AAA7 RID: 43687
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x020026C1 RID: 9921
			public static class LOGISTICS
			{
				// Token: 0x0400AAA8 RID: 43688
				public static LocString NAME = UI.FormatAsLink("Logistics", "BUILD_CATEGORY_LOGISTICS");

				// Token: 0x0400AAA9 RID: 43689
				public static LocString TOOLTIP = "Devices for base automation and material transport. {Hotkey}";
			}

			// Token: 0x020026C2 RID: 9922
			public static class HEALTHANDHAPPINESS
			{
				// Token: 0x0400AAAA RID: 43690
				public static LocString NAME = UI.FormatAsLink("Accommodation", "BUILD_CATEGORY_HEALTHANDHAPPINESS");

				// Token: 0x0400AAAB RID: 43691
				public static LocString TOOLTIP = "Everything a Duplicant needs to stay happy, healthy, and fulfilled. {Hotkey}";
			}

			// Token: 0x020026C3 RID: 9923
			public static class INDUSTRIAL
			{
				// Token: 0x0400AAAC RID: 43692
				public static LocString NAME = UI.FormatAsLink("Industrials", "BUILD_CATEGORY_INDUSTRIAL");

				// Token: 0x0400AAAD RID: 43693
				public static LocString TOOLTIP = "Machinery for oxygen production, heat management, and material refinement. {Hotkey}";
			}

			// Token: 0x020026C4 RID: 9924
			public static class LADDERS
			{
				// Token: 0x0400AAAE RID: 43694
				public static LocString NAME = "Ladders";

				// Token: 0x0400AAAF RID: 43695
				public static LocString BUILDMENUTITLE = "Ladders";

				// Token: 0x0400AAB0 RID: 43696
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026C5 RID: 9925
			public static class TILES
			{
				// Token: 0x0400AAB1 RID: 43697
				public static LocString NAME = "Tiles and Drywall";

				// Token: 0x0400AAB2 RID: 43698
				public static LocString BUILDMENUTITLE = "Tiles and Drywall";

				// Token: 0x0400AAB3 RID: 43699
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026C6 RID: 9926
			public static class PRINTINGPODS
			{
				// Token: 0x0400AAB4 RID: 43700
				public static LocString NAME = "Printing Pods";

				// Token: 0x0400AAB5 RID: 43701
				public static LocString BUILDMENUTITLE = "Printing Pods";

				// Token: 0x0400AAB6 RID: 43702
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026C7 RID: 9927
			public static class DOORS
			{
				// Token: 0x0400AAB7 RID: 43703
				public static LocString NAME = "Doors";

				// Token: 0x0400AAB8 RID: 43704
				public static LocString BUILDMENUTITLE = "Doors";

				// Token: 0x0400AAB9 RID: 43705
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026C8 RID: 9928
			public static class STORAGE
			{
				// Token: 0x0400AABA RID: 43706
				public static LocString NAME = "Storage";

				// Token: 0x0400AABB RID: 43707
				public static LocString BUILDMENUTITLE = "Storage";

				// Token: 0x0400AABC RID: 43708
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026C9 RID: 9929
			public static class TRANSPORT
			{
				// Token: 0x0400AABD RID: 43709
				public static LocString NAME = "Transit Tubes";

				// Token: 0x0400AABE RID: 43710
				public static LocString BUILDMENUTITLE = "Transit Tubes";

				// Token: 0x0400AABF RID: 43711
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CA RID: 9930
			public static class PRODUCERS
			{
				// Token: 0x0400AAC0 RID: 43712
				public static LocString NAME = "Production";

				// Token: 0x0400AAC1 RID: 43713
				public static LocString BUILDMENUTITLE = "Production";

				// Token: 0x0400AAC2 RID: 43714
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CB RID: 9931
			public static class SCRUBBERS
			{
				// Token: 0x0400AAC3 RID: 43715
				public static LocString NAME = "Purification";

				// Token: 0x0400AAC4 RID: 43716
				public static LocString BUILDMENUTITLE = "Purification";

				// Token: 0x0400AAC5 RID: 43717
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CC RID: 9932
			public static class BATTERIES
			{
				// Token: 0x0400AAC6 RID: 43718
				public static LocString NAME = "Batteries";

				// Token: 0x0400AAC7 RID: 43719
				public static LocString BUILDMENUTITLE = "Batteries";

				// Token: 0x0400AAC8 RID: 43720
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CD RID: 9933
			public static class SWITCHES
			{
				// Token: 0x0400AAC9 RID: 43721
				public static LocString NAME = "Switches";

				// Token: 0x0400AACA RID: 43722
				public static LocString BUILDMENUTITLE = "Switches";

				// Token: 0x0400AACB RID: 43723
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CE RID: 9934
			public static class COOKING
			{
				// Token: 0x0400AACC RID: 43724
				public static LocString NAME = "Cooking";

				// Token: 0x0400AACD RID: 43725
				public static LocString BUILDMENUTITLE = "Cooking";

				// Token: 0x0400AACE RID: 43726
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026CF RID: 9935
			public static class FARMING
			{
				// Token: 0x0400AACF RID: 43727
				public static LocString NAME = "Farming";

				// Token: 0x0400AAD0 RID: 43728
				public static LocString BUILDMENUTITLE = "Farming";

				// Token: 0x0400AAD1 RID: 43729
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D0 RID: 9936
			public static class RANCHING
			{
				// Token: 0x0400AAD2 RID: 43730
				public static LocString NAME = "Ranching";

				// Token: 0x0400AAD3 RID: 43731
				public static LocString BUILDMENUTITLE = "Ranching";

				// Token: 0x0400AAD4 RID: 43732
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D1 RID: 9937
			public static class WASHROOM
			{
				// Token: 0x0400AAD5 RID: 43733
				public static LocString NAME = "Washroom";

				// Token: 0x0400AAD6 RID: 43734
				public static LocString BUILDMENUTITLE = "Washroom";

				// Token: 0x0400AAD7 RID: 43735
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D2 RID: 9938
			public static class VALVES
			{
				// Token: 0x0400AAD8 RID: 43736
				public static LocString NAME = "Valves";

				// Token: 0x0400AAD9 RID: 43737
				public static LocString BUILDMENUTITLE = "Valves";

				// Token: 0x0400AADA RID: 43738
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D3 RID: 9939
			public static class PUMPS
			{
				// Token: 0x0400AADB RID: 43739
				public static LocString NAME = "Pumps";

				// Token: 0x0400AADC RID: 43740
				public static LocString BUILDMENUTITLE = "Pumps";

				// Token: 0x0400AADD RID: 43741
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D4 RID: 9940
			public static class SENSORS
			{
				// Token: 0x0400AADE RID: 43742
				public static LocString NAME = "Sensors";

				// Token: 0x0400AADF RID: 43743
				public static LocString BUILDMENUTITLE = "Sensors";

				// Token: 0x0400AAE0 RID: 43744
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D5 RID: 9941
			public static class PORTS
			{
				// Token: 0x0400AAE1 RID: 43745
				public static LocString NAME = "Ports";

				// Token: 0x0400AAE2 RID: 43746
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400AAE3 RID: 43747
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D6 RID: 9942
			public static class MATERIALS
			{
				// Token: 0x0400AAE4 RID: 43748
				public static LocString NAME = "Materials";

				// Token: 0x0400AAE5 RID: 43749
				public static LocString BUILDMENUTITLE = "Materials";

				// Token: 0x0400AAE6 RID: 43750
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D7 RID: 9943
			public static class OIL
			{
				// Token: 0x0400AAE7 RID: 43751
				public static LocString NAME = "Oil";

				// Token: 0x0400AAE8 RID: 43752
				public static LocString BUILDMENUTITLE = "Oil";

				// Token: 0x0400AAE9 RID: 43753
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D8 RID: 9944
			public static class ADVANCED
			{
				// Token: 0x0400AAEA RID: 43754
				public static LocString NAME = "Advanced";

				// Token: 0x0400AAEB RID: 43755
				public static LocString BUILDMENUTITLE = "Advanced";

				// Token: 0x0400AAEC RID: 43756
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026D9 RID: 9945
			public static class ORGANIC
			{
				// Token: 0x0400AAED RID: 43757
				public static LocString NAME = "Organic";

				// Token: 0x0400AAEE RID: 43758
				public static LocString BUILDMENUTITLE = "Organic";

				// Token: 0x0400AAEF RID: 43759
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DA RID: 9946
			public static class BEDS
			{
				// Token: 0x0400AAF0 RID: 43760
				public static LocString NAME = "Beds";

				// Token: 0x0400AAF1 RID: 43761
				public static LocString BUILDMENUTITLE = "Beds";

				// Token: 0x0400AAF2 RID: 43762
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DB RID: 9947
			public static class LIGHTS
			{
				// Token: 0x0400AAF3 RID: 43763
				public static LocString NAME = "Lights";

				// Token: 0x0400AAF4 RID: 43764
				public static LocString BUILDMENUTITLE = "Lights";

				// Token: 0x0400AAF5 RID: 43765
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DC RID: 9948
			public static class DINING
			{
				// Token: 0x0400AAF6 RID: 43766
				public static LocString NAME = "Dining";

				// Token: 0x0400AAF7 RID: 43767
				public static LocString BUILDMENUTITLE = "Dining";

				// Token: 0x0400AAF8 RID: 43768
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DD RID: 9949
			public static class MANUFACTURING
			{
				// Token: 0x0400AAF9 RID: 43769
				public static LocString NAME = "Manufacturing";

				// Token: 0x0400AAFA RID: 43770
				public static LocString BUILDMENUTITLE = "Manufacturing";

				// Token: 0x0400AAFB RID: 43771
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DE RID: 9950
			public static class TEMPERATURE
			{
				// Token: 0x0400AAFC RID: 43772
				public static LocString NAME = "Temperature";

				// Token: 0x0400AAFD RID: 43773
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400AAFE RID: 43774
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026DF RID: 9951
			public static class RESEARCH
			{
				// Token: 0x0400AAFF RID: 43775
				public static LocString NAME = "Research";

				// Token: 0x0400AB00 RID: 43776
				public static LocString BUILDMENUTITLE = "Research";

				// Token: 0x0400AB01 RID: 43777
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E0 RID: 9952
			public static class GENERATORS
			{
				// Token: 0x0400AB02 RID: 43778
				public static LocString NAME = "Generators";

				// Token: 0x0400AB03 RID: 43779
				public static LocString BUILDMENUTITLE = "Generators";

				// Token: 0x0400AB04 RID: 43780
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E1 RID: 9953
			public static class WIRES
			{
				// Token: 0x0400AB05 RID: 43781
				public static LocString NAME = "Wires";

				// Token: 0x0400AB06 RID: 43782
				public static LocString BUILDMENUTITLE = "Wires";

				// Token: 0x0400AB07 RID: 43783
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E2 RID: 9954
			public static class LOGICGATES
			{
				// Token: 0x0400AB08 RID: 43784
				public static LocString NAME = "Gates";

				// Token: 0x0400AB09 RID: 43785
				public static LocString BUILDMENUTITLE = "Gates";

				// Token: 0x0400AB0A RID: 43786
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E3 RID: 9955
			public static class TRANSMISSIONS
			{
				// Token: 0x0400AB0B RID: 43787
				public static LocString NAME = "Transmissions";

				// Token: 0x0400AB0C RID: 43788
				public static LocString BUILDMENUTITLE = "Transmissions";

				// Token: 0x0400AB0D RID: 43789
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E4 RID: 9956
			public static class LOGICMANAGER
			{
				// Token: 0x0400AB0E RID: 43790
				public static LocString NAME = "Monitoring";

				// Token: 0x0400AB0F RID: 43791
				public static LocString BUILDMENUTITLE = "Monitoring";

				// Token: 0x0400AB10 RID: 43792
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E5 RID: 9957
			public static class LOGICAUDIO
			{
				// Token: 0x0400AB11 RID: 43793
				public static LocString NAME = "Ambience";

				// Token: 0x0400AB12 RID: 43794
				public static LocString BUILDMENUTITLE = "Ambience";

				// Token: 0x0400AB13 RID: 43795
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E6 RID: 9958
			public static class CONVEYANCESTRUCTURES
			{
				// Token: 0x0400AB14 RID: 43796
				public static LocString NAME = "Structural";

				// Token: 0x0400AB15 RID: 43797
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400AB16 RID: 43798
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E7 RID: 9959
			public static class BUILDMENUPORTS
			{
				// Token: 0x0400AB17 RID: 43799
				public static LocString NAME = "Ports";

				// Token: 0x0400AB18 RID: 43800
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400AB19 RID: 43801
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E8 RID: 9960
			public static class POWERCONTROL
			{
				// Token: 0x0400AB1A RID: 43802
				public static LocString NAME = "Power\nRegulation";

				// Token: 0x0400AB1B RID: 43803
				public static LocString BUILDMENUTITLE = "Power Regulation";

				// Token: 0x0400AB1C RID: 43804
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026E9 RID: 9961
			public static class PLUMBINGSTRUCTURES
			{
				// Token: 0x0400AB1D RID: 43805
				public static LocString NAME = "Plumbing";

				// Token: 0x0400AB1E RID: 43806
				public static LocString BUILDMENUTITLE = "Plumbing";

				// Token: 0x0400AB1F RID: 43807
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x020026EA RID: 9962
			public static class PIPES
			{
				// Token: 0x0400AB20 RID: 43808
				public static LocString NAME = "Pipes";

				// Token: 0x0400AB21 RID: 43809
				public static LocString BUILDMENUTITLE = "Pipes";

				// Token: 0x0400AB22 RID: 43810
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026EB RID: 9963
			public static class VENTILATIONSTRUCTURES
			{
				// Token: 0x0400AB23 RID: 43811
				public static LocString NAME = "Ventilation";

				// Token: 0x0400AB24 RID: 43812
				public static LocString BUILDMENUTITLE = "Ventilation";

				// Token: 0x0400AB25 RID: 43813
				public static LocString TOOLTIP = "Control the flow of gas in your base. {Hotkey}";
			}

			// Token: 0x020026EC RID: 9964
			public static class CONVEYANCE
			{
				// Token: 0x0400AB26 RID: 43814
				public static LocString NAME = "Ore\nTransport";

				// Token: 0x0400AB27 RID: 43815
				public static LocString BUILDMENUTITLE = "Ore Transport";

				// Token: 0x0400AB28 RID: 43816
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x020026ED RID: 9965
			public static class HYGIENE
			{
				// Token: 0x0400AB29 RID: 43817
				public static LocString NAME = "Hygiene";

				// Token: 0x0400AB2A RID: 43818
				public static LocString BUILDMENUTITLE = "Hygiene";

				// Token: 0x0400AB2B RID: 43819
				public static LocString TOOLTIP = "Keeps my Duplicants clean.";
			}

			// Token: 0x020026EE RID: 9966
			public static class MEDICAL
			{
				// Token: 0x0400AB2C RID: 43820
				public static LocString NAME = "Medical";

				// Token: 0x0400AB2D RID: 43821
				public static LocString BUILDMENUTITLE = "Medical";

				// Token: 0x0400AB2E RID: 43822
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x020026EF RID: 9967
			public static class WELLNESS
			{
				// Token: 0x0400AB2F RID: 43823
				public static LocString NAME = "Wellness";

				// Token: 0x0400AB30 RID: 43824
				public static LocString BUILDMENUTITLE = "Wellness";

				// Token: 0x0400AB31 RID: 43825
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026F0 RID: 9968
			public static class RECREATION
			{
				// Token: 0x0400AB32 RID: 43826
				public static LocString NAME = "Recreation";

				// Token: 0x0400AB33 RID: 43827
				public static LocString BUILDMENUTITLE = "Recreation";

				// Token: 0x0400AB34 RID: 43828
				public static LocString TOOLTIP = "Everything needed to reduce stress and increase fun.";
			}

			// Token: 0x020026F1 RID: 9969
			public static class FURNITURE
			{
				// Token: 0x0400AB35 RID: 43829
				public static LocString NAME = "Furniture";

				// Token: 0x0400AB36 RID: 43830
				public static LocString BUILDMENUTITLE = "Furniture";

				// Token: 0x0400AB37 RID: 43831
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x020026F2 RID: 9970
			public static class DECOR
			{
				// Token: 0x0400AB38 RID: 43832
				public static LocString NAME = "Decor";

				// Token: 0x0400AB39 RID: 43833
				public static LocString BUILDMENUTITLE = "Decor";

				// Token: 0x0400AB3A RID: 43834
				public static LocString TOOLTIP = "Spruce up your colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x020026F3 RID: 9971
			public static class OXYGEN
			{
				// Token: 0x0400AB3B RID: 43835
				public static LocString NAME = "Oxygen";

				// Token: 0x0400AB3C RID: 43836
				public static LocString BUILDMENUTITLE = "Oxygen";

				// Token: 0x0400AB3D RID: 43837
				public static LocString TOOLTIP = "Everything I need to keep my colony breathing. {Hotkey}";
			}

			// Token: 0x020026F4 RID: 9972
			public static class UTILITIES
			{
				// Token: 0x0400AB3E RID: 43838
				public static LocString NAME = "Temperature";

				// Token: 0x0400AB3F RID: 43839
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400AB40 RID: 43840
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026F5 RID: 9973
			public static class REFINING
			{
				// Token: 0x0400AB41 RID: 43841
				public static LocString NAME = "Refinement";

				// Token: 0x0400AB42 RID: 43842
				public static LocString BUILDMENUTITLE = "Refinement";

				// Token: 0x0400AB43 RID: 43843
				public static LocString TOOLTIP = "Use the resources you want, filter the ones you don't. {Hotkey}";
			}

			// Token: 0x020026F6 RID: 9974
			public static class EQUIPMENT
			{
				// Token: 0x0400AB44 RID: 43844
				public static LocString NAME = "Equipment";

				// Token: 0x0400AB45 RID: 43845
				public static LocString BUILDMENUTITLE = "Equipment";

				// Token: 0x0400AB46 RID: 43846
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x020026F7 RID: 9975
			public static class ARCHAEOLOGY
			{
				// Token: 0x0400AB47 RID: 43847
				public static LocString NAME = "Archaeology";

				// Token: 0x0400AB48 RID: 43848
				public static LocString BUILDMENUTITLE = "Archaeology";

				// Token: 0x0400AB49 RID: 43849
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026F8 RID: 9976
			public static class METEORDEFENSE
			{
				// Token: 0x0400AB4A RID: 43850
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400AB4B RID: 43851
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400AB4C RID: 43852
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026F9 RID: 9977
			public static class INDUSTRIALSTATION
			{
				// Token: 0x0400AB4D RID: 43853
				public static LocString NAME = "Industrial";

				// Token: 0x0400AB4E RID: 43854
				public static LocString BUILDMENUTITLE = "Industrial";

				// Token: 0x0400AB4F RID: 43855
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026FA RID: 9978
			public static class TELESCOPES
			{
				// Token: 0x0400AB50 RID: 43856
				public static LocString NAME = "Telescopes";

				// Token: 0x0400AB51 RID: 43857
				public static LocString BUILDMENUTITLE = "Telescopes";

				// Token: 0x0400AB52 RID: 43858
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x020026FB RID: 9979
			public static class MISSILES
			{
				// Token: 0x0400AB53 RID: 43859
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400AB54 RID: 43860
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400AB55 RID: 43861
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026FC RID: 9980
			public static class FITTINGS
			{
				// Token: 0x0400AB56 RID: 43862
				public static LocString NAME = "Fittings";

				// Token: 0x0400AB57 RID: 43863
				public static LocString BUILDMENUTITLE = "Fittings";

				// Token: 0x0400AB58 RID: 43864
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026FD RID: 9981
			public static class SANITATION
			{
				// Token: 0x0400AB59 RID: 43865
				public static LocString NAME = "Sanitation";

				// Token: 0x0400AB5A RID: 43866
				public static LocString BUILDMENUTITLE = "Sanitation";

				// Token: 0x0400AB5B RID: 43867
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026FE RID: 9982
			public static class AUTOMATED
			{
				// Token: 0x0400AB5C RID: 43868
				public static LocString NAME = "Automated";

				// Token: 0x0400AB5D RID: 43869
				public static LocString BUILDMENUTITLE = "Automated";

				// Token: 0x0400AB5E RID: 43870
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020026FF RID: 9983
			public static class ROCKETSTRUCTURES
			{
				// Token: 0x0400AB5F RID: 43871
				public static LocString NAME = "Structural";

				// Token: 0x0400AB60 RID: 43872
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400AB61 RID: 43873
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002700 RID: 9984
			public static class ROCKETNAV
			{
				// Token: 0x0400AB62 RID: 43874
				public static LocString NAME = "Navigation";

				// Token: 0x0400AB63 RID: 43875
				public static LocString BUILDMENUTITLE = "Navigation";

				// Token: 0x0400AB64 RID: 43876
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002701 RID: 9985
			public static class CONDUITSENSORS
			{
				// Token: 0x0400AB65 RID: 43877
				public static LocString NAME = "Pipe Sensors";

				// Token: 0x0400AB66 RID: 43878
				public static LocString BUILDMENUTITLE = "Pipe Sensors";

				// Token: 0x0400AB67 RID: 43879
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002702 RID: 9986
			public static class ROCKETRY
			{
				// Token: 0x0400AB68 RID: 43880
				public static LocString NAME = "Rocketry";

				// Token: 0x0400AB69 RID: 43881
				public static LocString BUILDMENUTITLE = "Rocketry";

				// Token: 0x0400AB6A RID: 43882
				public static LocString TOOLTIP = "Rocketry {Hotkey}";
			}

			// Token: 0x02002703 RID: 9987
			public static class ENGINES
			{
				// Token: 0x0400AB6B RID: 43883
				public static LocString NAME = "Engines";

				// Token: 0x0400AB6C RID: 43884
				public static LocString BUILDMENUTITLE = "Engines";

				// Token: 0x0400AB6D RID: 43885
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002704 RID: 9988
			public static class TANKS
			{
				// Token: 0x0400AB6E RID: 43886
				public static LocString NAME = "Tanks";

				// Token: 0x0400AB6F RID: 43887
				public static LocString BUILDMENUTITLE = "Tanks";

				// Token: 0x0400AB70 RID: 43888
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002705 RID: 9989
			public static class CARGO
			{
				// Token: 0x0400AB71 RID: 43889
				public static LocString NAME = "Cargo";

				// Token: 0x0400AB72 RID: 43890
				public static LocString BUILDMENUTITLE = "Cargo";

				// Token: 0x0400AB73 RID: 43891
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002706 RID: 9990
			public static class MODULE
			{
				// Token: 0x0400AB74 RID: 43892
				public static LocString NAME = "Modules";

				// Token: 0x0400AB75 RID: 43893
				public static LocString BUILDMENUTITLE = "Modules";

				// Token: 0x0400AB76 RID: 43894
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x02001DC9 RID: 7625
		public class TOOLS
		{
			// Token: 0x0400881C RID: 34844
			public static LocString TOOL_AREA_FMT = "{0} x {1}\n{2} tiles";

			// Token: 0x0400881D RID: 34845
			public static LocString TOOL_LENGTH_FMT = "{0}";

			// Token: 0x0400881E RID: 34846
			public static LocString FILTER_HOVERCARD_HEADER = "   <style=\"hovercard_element\">({0})</style>";

			// Token: 0x02002707 RID: 9991
			public class SANDBOX
			{
				// Token: 0x02003137 RID: 12599
				public class SANDBOX_TOGGLE
				{
					// Token: 0x0400C59C RID: 50588
					public static LocString NAME = "SANDBOX";
				}

				// Token: 0x02003138 RID: 12600
				public class BRUSH
				{
					// Token: 0x0400C59D RID: 50589
					public static LocString NAME = "Brush";

					// Token: 0x0400C59E RID: 50590
					public static LocString HOVERACTION = "PAINT SIM";
				}

				// Token: 0x02003139 RID: 12601
				public class SPRINKLE
				{
					// Token: 0x0400C59F RID: 50591
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400C5A0 RID: 50592
					public static LocString HOVERACTION = "SPRINKLE SIM";
				}

				// Token: 0x0200313A RID: 12602
				public class FLOOD
				{
					// Token: 0x0400C5A1 RID: 50593
					public static LocString NAME = "Fill";

					// Token: 0x0400C5A2 RID: 50594
					public static LocString HOVERACTION = "PAINT SECTION";
				}

				// Token: 0x0200313B RID: 12603
				public class MARQUEE
				{
					// Token: 0x0400C5A3 RID: 50595
					public static LocString NAME = "Marquee";
				}

				// Token: 0x0200313C RID: 12604
				public class SAMPLE
				{
					// Token: 0x0400C5A4 RID: 50596
					public static LocString NAME = "Sample";

					// Token: 0x0400C5A5 RID: 50597
					public static LocString HOVERACTION = "COPY SELECTION";
				}

				// Token: 0x0200313D RID: 12605
				public class HEATGUN
				{
					// Token: 0x0400C5A6 RID: 50598
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400C5A7 RID: 50599
					public static LocString HOVERACTION = "PAINT HEAT";
				}

				// Token: 0x0200313E RID: 12606
				public class RADSTOOL
				{
					// Token: 0x0400C5A8 RID: 50600
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400C5A9 RID: 50601
					public static LocString HOVERACTION = "PAINT RADS";
				}

				// Token: 0x0200313F RID: 12607
				public class STRESSTOOL
				{
					// Token: 0x0400C5AA RID: 50602
					public static LocString NAME = "Happy Tool";

					// Token: 0x0400C5AB RID: 50603
					public static LocString HOVERACTION = "PAINT CALM";
				}

				// Token: 0x02003140 RID: 12608
				public class SPAWNER
				{
					// Token: 0x0400C5AC RID: 50604
					public static LocString NAME = "Spawner";

					// Token: 0x0400C5AD RID: 50605
					public static LocString HOVERACTION = "SPAWN";
				}

				// Token: 0x02003141 RID: 12609
				public class CLEAR_FLOOR
				{
					// Token: 0x0400C5AE RID: 50606
					public static LocString NAME = "Clear Floor";

					// Token: 0x0400C5AF RID: 50607
					public static LocString HOVERACTION = "DELETE DEBRIS";
				}

				// Token: 0x02003142 RID: 12610
				public class DESTROY
				{
					// Token: 0x0400C5B0 RID: 50608
					public static LocString NAME = "Destroy";

					// Token: 0x0400C5B1 RID: 50609
					public static LocString HOVERACTION = "DELETE";
				}

				// Token: 0x02003143 RID: 12611
				public class SPAWN_ENTITY
				{
					// Token: 0x0400C5B2 RID: 50610
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003144 RID: 12612
				public class FOW
				{
					// Token: 0x0400C5B3 RID: 50611
					public static LocString NAME = "Reveal";

					// Token: 0x0400C5B4 RID: 50612
					public static LocString HOVERACTION = "DE-FOG";
				}

				// Token: 0x02003145 RID: 12613
				public class CRITTER
				{
					// Token: 0x0400C5B5 RID: 50613
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400C5B6 RID: 50614
					public static LocString HOVERACTION = "DELETE CRITTERS";
				}
			}

			// Token: 0x02002708 RID: 9992
			public class GENERIC
			{
				// Token: 0x0400AB77 RID: 43895
				public static LocString BACK = "Back";

				// Token: 0x0400AB78 RID: 43896
				public static LocString UNKNOWN = "UNKNOWN";

				// Token: 0x0400AB79 RID: 43897
				public static LocString BUILDING_HOVER_NAME_FMT = "{Name}    <style=\"hovercard_element\">({Element})</style>";

				// Token: 0x0400AB7A RID: 43898
				public static LocString LOGIC_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400AB7B RID: 43899
				public static LocString LOGIC_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400AB7C RID: 43900
				public static LocString LOGIC_MULTI_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400AB7D RID: 43901
				public static LocString LOGIC_MULTI_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";
			}

			// Token: 0x02002709 RID: 9993
			public class ATTACK
			{
				// Token: 0x0400AB7E RID: 43902
				public static LocString NAME = "Attack";

				// Token: 0x0400AB7F RID: 43903
				public static LocString TOOLNAME = "Attack tool";

				// Token: 0x0400AB80 RID: 43904
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200270A RID: 9994
			public class CAPTURE
			{
				// Token: 0x0400AB81 RID: 43905
				public static LocString NAME = "Wrangle";

				// Token: 0x0400AB82 RID: 43906
				public static LocString TOOLNAME = "Wrangle tool";

				// Token: 0x0400AB83 RID: 43907
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400AB84 RID: 43908
				public static LocString NOT_CAPTURABLE = "Cannot Wrangle";
			}

			// Token: 0x0200270B RID: 9995
			public class BUILD
			{
				// Token: 0x0400AB85 RID: 43909
				public static LocString NAME = "Build {0}";

				// Token: 0x0400AB86 RID: 43910
				public static LocString TOOLNAME = "Build tool";

				// Token: 0x0400AB87 RID: 43911
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO BUILD";

				// Token: 0x0400AB88 RID: 43912
				public static LocString TOOLACTION_DRAG = "DRAG";
			}

			// Token: 0x0200270C RID: 9996
			public class PLACE
			{
				// Token: 0x0400AB89 RID: 43913
				public static LocString NAME = "Place {0}";

				// Token: 0x0400AB8A RID: 43914
				public static LocString TOOLNAME = "Place tool";

				// Token: 0x0400AB8B RID: 43915
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO PLACE";

				// Token: 0x02003146 RID: 12614
				public class REASONS
				{
					// Token: 0x0400C5B7 RID: 50615
					public static LocString CAN_OCCUPY_AREA = "Location blocked";

					// Token: 0x0400C5B8 RID: 50616
					public static LocString ON_FOUNDATION = "Must place on the ground";

					// Token: 0x0400C5B9 RID: 50617
					public static LocString VISIBLE_TO_SPACE = "Must have a clear path to space";

					// Token: 0x0400C5BA RID: 50618
					public static LocString RESTRICT_TO_WORLD = "Incorrect " + UI.CLUSTERMAP.PLANETOID;
				}
			}

			// Token: 0x0200270D RID: 9997
			public class MOVETOLOCATION
			{
				// Token: 0x0400AB8C RID: 43916
				public static LocString NAME = "Move";

				// Token: 0x0400AB8D RID: 43917
				public static LocString TOOLNAME = "Move Here";

				// Token: 0x0400AB8E RID: 43918
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) ?? "";

				// Token: 0x0400AB8F RID: 43919
				public static LocString UNREACHABLE = "UNREACHABLE";
			}

			// Token: 0x0200270E RID: 9998
			public class COPYSETTINGS
			{
				// Token: 0x0400AB90 RID: 43920
				public static LocString NAME = "Paste Settings";

				// Token: 0x0400AB91 RID: 43921
				public static LocString TOOLNAME = "Paste Settings Tool";

				// Token: 0x0400AB92 RID: 43922
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200270F RID: 9999
			public class DIG
			{
				// Token: 0x0400AB93 RID: 43923
				public static LocString NAME = "Dig";

				// Token: 0x0400AB94 RID: 43924
				public static LocString TOOLNAME = "Dig tool";

				// Token: 0x0400AB95 RID: 43925
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002710 RID: 10000
			public class DISINFECT
			{
				// Token: 0x0400AB96 RID: 43926
				public static LocString NAME = "Disinfect";

				// Token: 0x0400AB97 RID: 43927
				public static LocString TOOLNAME = "Disinfect tool";

				// Token: 0x0400AB98 RID: 43928
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002711 RID: 10001
			public class DISCONNECT
			{
				// Token: 0x0400AB99 RID: 43929
				public static LocString NAME = "Disconnect";

				// Token: 0x0400AB9A RID: 43930
				public static LocString TOOLTIP = "Sever conduits and connectors {Hotkey}";

				// Token: 0x0400AB9B RID: 43931
				public static LocString TOOLNAME = "Disconnect tool";

				// Token: 0x0400AB9C RID: 43932
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002712 RID: 10002
			public class CANCEL
			{
				// Token: 0x0400AB9D RID: 43933
				public static LocString NAME = "Cancel";

				// Token: 0x0400AB9E RID: 43934
				public static LocString TOOLNAME = "Cancel tool";

				// Token: 0x0400AB9F RID: 43935
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002713 RID: 10003
			public class DECONSTRUCT
			{
				// Token: 0x0400ABA0 RID: 43936
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400ABA1 RID: 43937
				public static LocString TOOLNAME = "Deconstruct tool";

				// Token: 0x0400ABA2 RID: 43938
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002714 RID: 10004
			public class CLEANUPCATEGORY
			{
				// Token: 0x0400ABA3 RID: 43939
				public static LocString NAME = "Clean";

				// Token: 0x0400ABA4 RID: 43940
				public static LocString TOOLNAME = "Clean Up tools";
			}

			// Token: 0x02002715 RID: 10005
			public class PRIORITIESCATEGORY
			{
				// Token: 0x0400ABA5 RID: 43941
				public static LocString NAME = "Priority";
			}

			// Token: 0x02002716 RID: 10006
			public class MARKFORSTORAGE
			{
				// Token: 0x0400ABA6 RID: 43942
				public static LocString NAME = "Sweep";

				// Token: 0x0400ABA7 RID: 43943
				public static LocString TOOLNAME = "Sweep tool";

				// Token: 0x0400ABA8 RID: 43944
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002717 RID: 10007
			public class MOP
			{
				// Token: 0x0400ABA9 RID: 43945
				public static LocString NAME = "Mop";

				// Token: 0x0400ABAA RID: 43946
				public static LocString TOOLNAME = "Mop tool";

				// Token: 0x0400ABAB RID: 43947
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400ABAC RID: 43948
				public static LocString TOO_MUCH_LIQUID = "Too Much Liquid";

				// Token: 0x0400ABAD RID: 43949
				public static LocString NOT_ON_FLOOR = "Not On Floor";
			}

			// Token: 0x02002718 RID: 10008
			public class HARVEST
			{
				// Token: 0x0400ABAE RID: 43950
				public static LocString NAME = "Harvest";

				// Token: 0x0400ABAF RID: 43951
				public static LocString TOOLNAME = "Harvest tool";

				// Token: 0x0400ABB0 RID: 43952
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002719 RID: 10009
			public class PRIORITIZE
			{
				// Token: 0x0400ABB1 RID: 43953
				public static LocString NAME = "Priority";

				// Token: 0x0400ABB2 RID: 43954
				public static LocString TOOLNAME = "Priority tool";

				// Token: 0x0400ABB3 RID: 43955
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400ABB4 RID: 43956
				public static LocString SPECIFIC_PRIORITY = "Set Priority: {0}";
			}

			// Token: 0x0200271A RID: 10010
			public class EMPTY_PIPE
			{
				// Token: 0x0400ABB5 RID: 43957
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400ABB6 RID: 43958
				public static LocString TOOLTIP = "Extract pipe contents {Hotkey}";

				// Token: 0x0400ABB7 RID: 43959
				public static LocString TOOLNAME = "Empty Pipe tool";

				// Token: 0x0400ABB8 RID: 43960
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x0200271B RID: 10011
			public class FILTERSCREEN
			{
				// Token: 0x0400ABB9 RID: 43961
				public static LocString OPTIONS = "Tool Filter";
			}

			// Token: 0x0200271C RID: 10012
			public class FILTERLAYERS
			{
				// Token: 0x0400ABBA RID: 43962
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400ABBB RID: 43963
				public static LocString TILES = "Tiles";

				// Token: 0x0400ABBC RID: 43964
				public static LocString WIRES = "Power Wires";

				// Token: 0x0400ABBD RID: 43965
				public static LocString LIQUIDPIPES = "Liquid Pipes";

				// Token: 0x0400ABBE RID: 43966
				public static LocString GASPIPES = "Gas Pipes";

				// Token: 0x0400ABBF RID: 43967
				public static LocString SOLIDCONDUITS = "Conveyor Rails";

				// Token: 0x0400ABC0 RID: 43968
				public static LocString DIGPLACER = "Dig Orders";

				// Token: 0x0400ABC1 RID: 43969
				public static LocString CLEANANDCLEAR = "Sweep & Mop Orders";

				// Token: 0x0400ABC2 RID: 43970
				public static LocString ALL = "All";

				// Token: 0x0400ABC3 RID: 43971
				public static LocString HARVEST_WHEN_READY = "Enable Harvest";

				// Token: 0x0400ABC4 RID: 43972
				public static LocString DO_NOT_HARVEST = "Disable Harvest";

				// Token: 0x0400ABC5 RID: 43973
				public static LocString ATTACK = "Attack";

				// Token: 0x0400ABC6 RID: 43974
				public static LocString LOGIC = "Automation";

				// Token: 0x0400ABC7 RID: 43975
				public static LocString BACKWALL = "Background Buildings";

				// Token: 0x0400ABC8 RID: 43976
				public static LocString METAL = "Metal";

				// Token: 0x0400ABC9 RID: 43977
				public static LocString BUILDABLE = "Mineral";

				// Token: 0x0400ABCA RID: 43978
				public static LocString FILTER = "Filtration Medium";

				// Token: 0x0400ABCB RID: 43979
				public static LocString LIQUIFIABLE = "Liquefiable";

				// Token: 0x0400ABCC RID: 43980
				public static LocString LIQUID = "Liquid";

				// Token: 0x0400ABCD RID: 43981
				public static LocString GAS = "Gas";

				// Token: 0x0400ABCE RID: 43982
				public static LocString CONSUMABLEORE = "Consumable Ore";

				// Token: 0x0400ABCF RID: 43983
				public static LocString ORGANICS = "Organic";

				// Token: 0x0400ABD0 RID: 43984
				public static LocString FARMABLE = "Cultivable Soil";

				// Token: 0x0400ABD1 RID: 43985
				public static LocString BREATHABLE = "Breathable Gas";

				// Token: 0x0400ABD2 RID: 43986
				public static LocString UNBREATHABLE = "Unbreathable Gas";

				// Token: 0x0400ABD3 RID: 43987
				public static LocString AGRICULTURE = "Agriculture";

				// Token: 0x0400ABD4 RID: 43988
				public static LocString MISC = "Other";

				// Token: 0x0400ABD5 RID: 43989
				public static LocString ABSOLUTETEMPERATURE = "Temperature";

				// Token: 0x0400ABD6 RID: 43990
				public static LocString ADAPTIVETEMPERATURE = "Adapt. Temperature";

				// Token: 0x0400ABD7 RID: 43991
				public static LocString HEATFLOW = "Thermal Tolerance";

				// Token: 0x0400ABD8 RID: 43992
				public static LocString STATECHANGE = "State Change";

				// Token: 0x0400ABD9 RID: 43993
				public static LocString CONSTRUCTION = "Construction";

				// Token: 0x0400ABDA RID: 43994
				public static LocString DIG = "Digging";

				// Token: 0x0400ABDB RID: 43995
				public static LocString CLEAN = "Cleaning";

				// Token: 0x0400ABDC RID: 43996
				public static LocString OPERATE = "Duties";
			}
		}

		// Token: 0x02001DCA RID: 7626
		public class DETAILTABS
		{
			// Token: 0x0200271D RID: 10013
			public class STATS
			{
				// Token: 0x0400ABDD RID: 43997
				public static LocString NAME = "Skills";

				// Token: 0x0400ABDE RID: 43998
				public static LocString TOOLTIP = "View this Duplicant's attributes, traits, and daily stress";

				// Token: 0x0400ABDF RID: 43999
				public static LocString GROUPNAME_ATTRIBUTES = "ATTRIBUTES";

				// Token: 0x0400ABE0 RID: 44000
				public static LocString GROUPNAME_STRESS = "TODAY'S STRESS";

				// Token: 0x0400ABE1 RID: 44001
				public static LocString GROUPNAME_EXPECTATIONS = "EXPECTATIONS";

				// Token: 0x0400ABE2 RID: 44002
				public static LocString GROUPNAME_TRAITS = "TRAITS";
			}

			// Token: 0x0200271E RID: 10014
			public class SIMPLEINFO
			{
				// Token: 0x0400ABE3 RID: 44003
				public static LocString NAME = "Status";

				// Token: 0x0400ABE4 RID: 44004
				public static LocString TOOLTIP = "View the current status of the selected object";

				// Token: 0x0400ABE5 RID: 44005
				public static LocString GROUPNAME_STATUS = "STATUS";

				// Token: 0x0400ABE6 RID: 44006
				public static LocString GROUPNAME_DESCRIPTION = "INFORMATION";

				// Token: 0x0400ABE7 RID: 44007
				public static LocString GROUPNAME_CONDITION = "CONDITION";

				// Token: 0x0400ABE8 RID: 44008
				public static LocString GROUPNAME_REQUIREMENTS = "REQUIREMENTS";

				// Token: 0x0400ABE9 RID: 44009
				public static LocString GROUPNAME_EFFECTS = "EFFECTS";

				// Token: 0x0400ABEA RID: 44010
				public static LocString GROUPNAME_RESEARCH = "RESEARCH";

				// Token: 0x0400ABEB RID: 44011
				public static LocString GROUPNAME_LORE = "RECOVERED FILES";

				// Token: 0x0400ABEC RID: 44012
				public static LocString GROUPNAME_FERTILITY = "EGG CHANCES";

				// Token: 0x0400ABED RID: 44013
				public static LocString GROUPNAME_ROCKET = "ROCKETRY";

				// Token: 0x0400ABEE RID: 44014
				public static LocString GROUPNAME_CARGOBAY = "CARGO BAYS";

				// Token: 0x0400ABEF RID: 44015
				public static LocString GROUPNAME_ELEMENTS = "RESOURCES";

				// Token: 0x0400ABF0 RID: 44016
				public static LocString GROUPNAME_LIFE = "LIFEFORMS";

				// Token: 0x0400ABF1 RID: 44017
				public static LocString GROUPNAME_BIOMES = "BIOMES";

				// Token: 0x0400ABF2 RID: 44018
				public static LocString GROUPNAME_GEYSERS = "GEYSERS";

				// Token: 0x0400ABF3 RID: 44019
				public static LocString GROUPNAME_METEORSHOWERS = "METEOR SHOWERS";

				// Token: 0x0400ABF4 RID: 44020
				public static LocString GROUPNAME_WORLDTRAITS = "WORLD TRAITS";

				// Token: 0x0400ABF5 RID: 44021
				public static LocString GROUPNAME_CLUSTER_POI = "POINT OF INTEREST";

				// Token: 0x0400ABF6 RID: 44022
				public static LocString GROUPNAME_MOVABLE = "MOVING";

				// Token: 0x0400ABF7 RID: 44023
				public static LocString NO_METEORSHOWERS = "No meteor showers forecasted";

				// Token: 0x0400ABF8 RID: 44024
				public static LocString NO_GEYSERS = "No geysers detected";

				// Token: 0x0400ABF9 RID: 44025
				public static LocString UNKNOWN_GEYSERS = "Unknown Geysers ({num})";
			}

			// Token: 0x0200271F RID: 10015
			public class DETAILS
			{
				// Token: 0x0400ABFA RID: 44026
				public static LocString NAME = "Properties";

				// Token: 0x0400ABFB RID: 44027
				public static LocString MINION_NAME = "About";

				// Token: 0x0400ABFC RID: 44028
				public static LocString TOOLTIP = "More information";

				// Token: 0x0400ABFD RID: 44029
				public static LocString MINION_TOOLTIP = "More information";

				// Token: 0x0400ABFE RID: 44030
				public static LocString GROUPNAME_DETAILS = "DETAILS";

				// Token: 0x0400ABFF RID: 44031
				public static LocString GROUPNAME_CONTENTS = "CONTENTS";

				// Token: 0x0400AC00 RID: 44032
				public static LocString GROUPNAME_MINION_CONTENTS = "CARRIED ITEMS";

				// Token: 0x0400AC01 RID: 44033
				public static LocString STORAGE_EMPTY = "None";

				// Token: 0x0400AC02 RID: 44034
				public static LocString CONTENTS_MASS = "{0}: {1}";

				// Token: 0x0400AC03 RID: 44035
				public static LocString CONTENTS_TEMPERATURE = "{0} at {1}";

				// Token: 0x0400AC04 RID: 44036
				public static LocString CONTENTS_ROTTABLE = "\n • {0}";

				// Token: 0x0400AC05 RID: 44037
				public static LocString CONTENTS_DISEASED = "\n • {0}";

				// Token: 0x0400AC06 RID: 44038
				public static LocString NET_STRESS = "<b>Today's Net Stress: {0}%</b>";

				// Token: 0x02003147 RID: 12615
				public class RADIATIONABSORPTIONFACTOR
				{
					// Token: 0x0400C5BB RID: 50619
					public static LocString NAME = "Radiation Blocking: {0}";

					// Token: 0x0400C5BC RID: 50620
					public static LocString TOOLTIP = "This object will block approximately {0} of radiation.";
				}
			}

			// Token: 0x02002720 RID: 10016
			public class PERSONALITY
			{
				// Token: 0x0400AC07 RID: 44039
				public static LocString NAME = "Bio";

				// Token: 0x0400AC08 RID: 44040
				public static LocString TOOLTIP = "View this Duplicant's personality, resume, and amenities";

				// Token: 0x0400AC09 RID: 44041
				public static LocString GROUPNAME_BIO = "ABOUT";

				// Token: 0x0400AC0A RID: 44042
				public static LocString GROUPNAME_RESUME = "{0}'S RESUME";

				// Token: 0x02003148 RID: 12616
				public class RESUME
				{
					// Token: 0x0400C5BD RID: 50621
					public static LocString MASTERED_SKILLS = "<b><size=13>Learned Skills:</size></b>";

					// Token: 0x0400C5BE RID: 50622
					public static LocString MASTERED_SKILLS_TOOLTIP = string.Concat(new string[]
					{
						"All ",
						UI.PRE_KEYWORD,
						"Traits",
						UI.PST_KEYWORD,
						" and ",
						UI.PRE_KEYWORD,
						"Morale Needs",
						UI.PST_KEYWORD,
						" become permanent once a Duplicant has learned a new ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						"\n\n",
						BUILDINGS.PREFABS.RESETSKILLSSTATION.NAME,
						"s can be built from the ",
						UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
						" to completely reset a Duplicant's learned ",
						UI.PRE_KEYWORD,
						"Skills",
						UI.PST_KEYWORD,
						", refunding all ",
						UI.PRE_KEYWORD,
						"Skill Points",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C5BF RID: 50623
					public static LocString JOBTRAINING_TOOLTIP = string.Concat(new string[]
					{
						"{0} learned this ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						" while working as a {1}"
					});

					// Token: 0x0200340F RID: 13327
					public class APTITUDES
					{
						// Token: 0x0400CCA4 RID: 52388
						public static LocString NAME = "<b><size=13>Personal Interests:</size></b>";

						// Token: 0x0400CCA5 RID: 52389
						public static LocString TOOLTIP = "{0} enjoys these types of work";
					}

					// Token: 0x02003410 RID: 13328
					public class PERKS
					{
						// Token: 0x0400CCA6 RID: 52390
						public static LocString NAME = "<b><size=13>Skill Training:</size></b>";

						// Token: 0x0400CCA7 RID: 52391
						public static LocString TOOLTIP = "These are permanent skills {0} gained from learned skills";
					}

					// Token: 0x02003411 RID: 13329
					public class CURRENT_ROLE
					{
						// Token: 0x0400CCA8 RID: 52392
						public static LocString NAME = "<size=13><b>Current Job:</b> {0}</size>";

						// Token: 0x0400CCA9 RID: 52393
						public static LocString TOOLTIP = "{0} is currently working as a {1}";

						// Token: 0x0400CCAA RID: 52394
						public static LocString NOJOB_TOOLTIP = "This {0} is... \"between jobs\" at present";
					}

					// Token: 0x02003412 RID: 13330
					public class NO_MASTERED_SKILLS
					{
						// Token: 0x0400CCAB RID: 52395
						public static LocString NAME = "None";

						// Token: 0x0400CCAC RID: 52396
						public static LocString TOOLTIP = string.Concat(new string[]
						{
							"{0} has not learned any ",
							UI.PRE_KEYWORD,
							"Skills",
							UI.PST_KEYWORD,
							" yet"
						});
					}
				}

				// Token: 0x02003149 RID: 12617
				public class EQUIPMENT
				{
					// Token: 0x0400C5C0 RID: 50624
					public static LocString GROUPNAME_ROOMS = "AMENITIES";

					// Token: 0x0400C5C1 RID: 50625
					public static LocString GROUPNAME_OWNABLE = "EQUIPMENT";

					// Token: 0x0400C5C2 RID: 50626
					public static LocString NO_ASSIGNABLES = "None";

					// Token: 0x0400C5C3 RID: 50627
					public static LocString NO_ASSIGNABLES_TOOLTIP = "{0} has not been assigned any buildings of their own";

					// Token: 0x0400C5C4 RID: 50628
					public static LocString UNASSIGNED = "Unassigned";

					// Token: 0x0400C5C5 RID: 50629
					public static LocString UNASSIGNED_TOOLTIP = "This Duplicant has not been assigned a {0}";

					// Token: 0x0400C5C6 RID: 50630
					public static LocString ASSIGNED_TOOLTIP = "{2} has been assigned a {0}\n\nEffects: {1}";

					// Token: 0x0400C5C7 RID: 50631
					public static LocString NOEQUIPMENT = "None";

					// Token: 0x0400C5C8 RID: 50632
					public static LocString NOEQUIPMENT_TOOLTIP = "{0}'s wearing their Printday Suit and nothing more";
				}
			}

			// Token: 0x02002721 RID: 10017
			public class ENERGYCONSUMER
			{
				// Token: 0x0400AC0B RID: 44043
				public static LocString NAME = "Energy";

				// Token: 0x0400AC0C RID: 44044
				public static LocString TOOLTIP = "View how much power this building consumes";
			}

			// Token: 0x02002722 RID: 10018
			public class ENERGYWIRE
			{
				// Token: 0x0400AC0D RID: 44045
				public static LocString NAME = "Energy";

				// Token: 0x0400AC0E RID: 44046
				public static LocString TOOLTIP = "View this wire's network";
			}

			// Token: 0x02002723 RID: 10019
			public class ENERGYGENERATOR
			{
				// Token: 0x0400AC0F RID: 44047
				public static LocString NAME = "Energy";

				// Token: 0x0400AC10 RID: 44048
				public static LocString TOOLTIP = "Monitor the power this building is generating";

				// Token: 0x0400AC11 RID: 44049
				public static LocString CIRCUITOVERVIEW = "CIRCUIT OVERVIEW";

				// Token: 0x0400AC12 RID: 44050
				public static LocString GENERATORS = "POWER GENERATORS";

				// Token: 0x0400AC13 RID: 44051
				public static LocString CONSUMERS = "POWER CONSUMERS";

				// Token: 0x0400AC14 RID: 44052
				public static LocString BATTERIES = "BATTERIES";

				// Token: 0x0400AC15 RID: 44053
				public static LocString DISCONNECTED = "Not connected to an electrical circuit";

				// Token: 0x0400AC16 RID: 44054
				public static LocString NOGENERATORS = "No generators on this circuit";

				// Token: 0x0400AC17 RID: 44055
				public static LocString NOCONSUMERS = "No consumers on this circuit";

				// Token: 0x0400AC18 RID: 44056
				public static LocString NOBATTERIES = "No batteries on this circuit";

				// Token: 0x0400AC19 RID: 44057
				public static LocString AVAILABLE_JOULES = UI.FormatAsLink("Power", "POWER") + " stored: {0}";

				// Token: 0x0400AC1A RID: 44058
				public static LocString AVAILABLE_JOULES_TOOLTIP = "Amount of power stored in batteries";

				// Token: 0x0400AC1B RID: 44059
				public static LocString WATTAGE_GENERATED = UI.FormatAsLink("Power", "POWER") + " produced: {0}";

				// Token: 0x0400AC1C RID: 44060
				public static LocString WATTAGE_GENERATED_TOOLTIP = "The total amount of power generated by this circuit";

				// Token: 0x0400AC1D RID: 44061
				public static LocString WATTAGE_CONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

				// Token: 0x0400AC1E RID: 44062
				public static LocString WATTAGE_CONSUMED_TOOLTIP = "The total amount of power used by this circuit";

				// Token: 0x0400AC1F RID: 44063
				public static LocString POTENTIAL_WATTAGE_CONSUMED = "Potential power consumed: {0}";

				// Token: 0x0400AC20 RID: 44064
				public static LocString POTENTIAL_WATTAGE_CONSUMED_TOOLTIP = "The total amount of power that can be used by this circuit if all connected buildings are active";

				// Token: 0x0400AC21 RID: 44065
				public static LocString MAX_SAFE_WATTAGE = "Maximum safe wattage: {0}";

				// Token: 0x0400AC22 RID: 44066
				public static LocString MAX_SAFE_WATTAGE_TOOLTIP = "Exceeding this value will overload the circuit and can result in damage to wiring and buildings";
			}

			// Token: 0x02002724 RID: 10020
			public class DISEASE
			{
				// Token: 0x0400AC23 RID: 44067
				public static LocString NAME = "Germs";

				// Token: 0x0400AC24 RID: 44068
				public static LocString TOOLTIP = "View the disease risk presented by the selected object";

				// Token: 0x0400AC25 RID: 44069
				public static LocString DISEASE_SOURCE = "DISEASE SOURCE";

				// Token: 0x0400AC26 RID: 44070
				public static LocString IMMUNE_SYSTEM = "GERM HOST";

				// Token: 0x0400AC27 RID: 44071
				public static LocString CONTRACTION_RATES = "CONTRACTION RATES";

				// Token: 0x0400AC28 RID: 44072
				public static LocString CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400AC29 RID: 44073
				public static LocString NO_CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400AC2A RID: 44074
				public static LocString GERMS_INFO = "GERM LIFE CYCLE";

				// Token: 0x0400AC2B RID: 44075
				public static LocString INFECTION_INFO = "INFECTION DETAILS";

				// Token: 0x0400AC2C RID: 44076
				public static LocString DISEASE_INFO_POPUP_HEADER = "DISEASE INFO: {0}";

				// Token: 0x0400AC2D RID: 44077
				public static LocString DISEASE_INFO_POPUP_BUTTON = "FULL INFO";

				// Token: 0x0400AC2E RID: 44078
				public static LocString DISEASE_INFO_POPUP_TOOLTIP = "View detailed germ and infection info for {0}";

				// Token: 0x0200314A RID: 12618
				public class DETAILS
				{
					// Token: 0x0400C5C9 RID: 50633
					public static LocString NODISEASE = "No surface germs";

					// Token: 0x0400C5CA RID: 50634
					public static LocString NODISEASE_TOOLTIP = "There are no germs present on this object";

					// Token: 0x0400C5CB RID: 50635
					public static LocString DISEASE_AMOUNT = "{0}: {1}";

					// Token: 0x0400C5CC RID: 50636
					public static LocString DISEASE_AMOUNT_TOOLTIP = "{0} are present on the surface of the selected object";

					// Token: 0x0400C5CD RID: 50637
					public static LocString DEATH_FORMAT = "{0} dead/cycle";

					// Token: 0x0400C5CE RID: 50638
					public static LocString DEATH_FORMAT_TOOLTIP = "Germ count is being reduced by {0}/cycle";

					// Token: 0x0400C5CF RID: 50639
					public static LocString GROWTH_FORMAT = "{0} spawned/cycle";

					// Token: 0x0400C5D0 RID: 50640
					public static LocString GROWTH_FORMAT_TOOLTIP = "Germ count is being increased by {0}/cycle";

					// Token: 0x0400C5D1 RID: 50641
					public static LocString NEUTRAL_FORMAT = "No change";

					// Token: 0x0400C5D2 RID: 50642
					public static LocString NEUTRAL_FORMAT_TOOLTIP = "Germ count is static";

					// Token: 0x02003413 RID: 13331
					public class GROWTH_FACTORS
					{
						// Token: 0x0400CCAD RID: 52397
						public static LocString TITLE = "\nGrowth factors:";

						// Token: 0x0400CCAE RID: 52398
						public static LocString TOOLTIP = "These conditions are contributing to the multiplication of germs";

						// Token: 0x0400CCAF RID: 52399
						public static LocString RATE_OF_CHANGE = "Change rate: {0}";

						// Token: 0x0400CCB0 RID: 52400
						public static LocString RATE_OF_CHANGE_TOOLTIP = "Germ count is fluctuating at a rate of {0}";

						// Token: 0x0400CCB1 RID: 52401
						public static LocString HALF_LIFE_NEG = "Half life: {0}";

						// Token: 0x0400CCB2 RID: 52402
						public static LocString HALF_LIFE_NEG_TOOLTIP = "In {0} the germ count on this object will be halved";

						// Token: 0x0400CCB3 RID: 52403
						public static LocString HALF_LIFE_POS = "Doubling time: {0}";

						// Token: 0x0400CCB4 RID: 52404
						public static LocString HALF_LIFE_POS_TOOLTIP = "In {0} the germ count on this object will be doubled";

						// Token: 0x0400CCB5 RID: 52405
						public static LocString HALF_LIFE_NEUTRAL = "Static";

						// Token: 0x0400CCB6 RID: 52406
						public static LocString HALF_LIFE_NEUTRAL_TOOLTIP = "The germ count is neither increasing nor decreasing";

						// Token: 0x0200343F RID: 13375
						public class SUBSTRATE
						{
							// Token: 0x0400CCEF RID: 52463
							public static LocString GROW = "    • Growing on {0}: {1}";

							// Token: 0x0400CCF0 RID: 52464
							public static LocString GROW_TOOLTIP = "Contact with this substance is causing germs to multiply";

							// Token: 0x0400CCF1 RID: 52465
							public static LocString NEUTRAL = "    • No change on {0}";

							// Token: 0x0400CCF2 RID: 52466
							public static LocString NEUTRAL_TOOLTIP = "Contact with this substance has no effect on germ count";

							// Token: 0x0400CCF3 RID: 52467
							public static LocString DIE = "    • Dying on {0}: {1}";

							// Token: 0x0400CCF4 RID: 52468
							public static LocString DIE_TOOLTIP = "Contact with this substance is causing germs to die off";
						}

						// Token: 0x02003440 RID: 13376
						public class ENVIRONMENT
						{
							// Token: 0x0400CCF5 RID: 52469
							public static LocString TITLE = "    • Surrounded by {0}: {1}";

							// Token: 0x0400CCF6 RID: 52470
							public static LocString GROW_TOOLTIP = "This atmosphere is causing germs to multiply";

							// Token: 0x0400CCF7 RID: 52471
							public static LocString DIE_TOOLTIP = "This atmosphere is causing germs to die off";
						}

						// Token: 0x02003441 RID: 13377
						public class TEMPERATURE
						{
							// Token: 0x0400CCF8 RID: 52472
							public static LocString TITLE = "    • Current temperature {0}: {1}";

							// Token: 0x0400CCF9 RID: 52473
							public static LocString GROW_TOOLTIP = "This temperature is allowing germs to multiply";

							// Token: 0x0400CCFA RID: 52474
							public static LocString DIE_TOOLTIP = "This temperature is causing germs to die off";
						}

						// Token: 0x02003442 RID: 13378
						public class PRESSURE
						{
							// Token: 0x0400CCFB RID: 52475
							public static LocString TITLE = "    • Current pressure {0}: {1}";

							// Token: 0x0400CCFC RID: 52476
							public static LocString GROW_TOOLTIP = "Atmospheric pressure is causing germs to multiply";

							// Token: 0x0400CCFD RID: 52477
							public static LocString DIE_TOOLTIP = "Atmospheric pressure is causing germs to die off";
						}

						// Token: 0x02003443 RID: 13379
						public class RADIATION
						{
							// Token: 0x0400CCFE RID: 52478
							public static LocString TITLE = "    • Exposed to {0} Rads: {1}";

							// Token: 0x0400CCFF RID: 52479
							public static LocString DIE_TOOLTIP = "Radiation exposure is causing germs to die off";
						}

						// Token: 0x02003444 RID: 13380
						public class DYING_OFF
						{
							// Token: 0x0400CD00 RID: 52480
							public static LocString TITLE = "    • <b>Dying off: {0}</b>";

							// Token: 0x0400CD01 RID: 52481
							public static LocString TOOLTIP = "Low germ count in this area is causing germs to die rapidly\n\nFewer than {0} are on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}

						// Token: 0x02003445 RID: 13381
						public class OVERPOPULATED
						{
							// Token: 0x0400CD02 RID: 52482
							public static LocString TITLE = "    • <b>Overpopulated: {0}</b>";

							// Token: 0x0400CD03 RID: 52483
							public static LocString TOOLTIP = "Too many germs are present in this area, resulting in rapid die-off until the population stabilizes\n\nA maximum of {0} can be on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}
					}
				}
			}

			// Token: 0x02002725 RID: 10021
			public class NEEDS
			{
				// Token: 0x0400AC2F RID: 44079
				public static LocString NAME = "Stress";

				// Token: 0x0400AC30 RID: 44080
				public static LocString TOOLTIP = "View this Duplicant's psychological status";

				// Token: 0x0400AC31 RID: 44081
				public static LocString CURRENT_STRESS_LEVEL = "Current " + UI.FormatAsLink("Stress", "STRESS") + " Level: {0}";

				// Token: 0x0400AC32 RID: 44082
				public static LocString OVERVIEW = "Overview";

				// Token: 0x0400AC33 RID: 44083
				public static LocString STRESS_CREATORS = UI.FormatAsLink("Stress", "STRESS") + " Creators";

				// Token: 0x0400AC34 RID: 44084
				public static LocString STRESS_RELIEVERS = UI.FormatAsLink("Stress", "STRESS") + " Relievers";

				// Token: 0x0400AC35 RID: 44085
				public static LocString CURRENT_NEED_LEVEL = "Current Level: {0}";

				// Token: 0x0400AC36 RID: 44086
				public static LocString NEXT_NEED_LEVEL = "Next Level: {0}";
			}

			// Token: 0x02002726 RID: 10022
			public class EGG_CHANCES
			{
				// Token: 0x0400AC37 RID: 44087
				public static LocString CHANCE_FORMAT = "{0}: {1}";

				// Token: 0x0400AC38 RID: 44088
				public static LocString CHANCE_FORMAT_TOOLTIP = "This critter has a {1} chance of laying {0}s.\n\nThis probability increases when the creature:\n{2}";

				// Token: 0x0400AC39 RID: 44089
				public static LocString CHANCE_MOD_FORMAT = "    • {0}\n";

				// Token: 0x0400AC3A RID: 44090
				public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = "This critter has a {1} chance of laying {0}s.";
			}

			// Token: 0x02002727 RID: 10023
			public class BUILDING_CHORES
			{
				// Token: 0x0400AC3B RID: 44091
				public static LocString NAME = "Errands";

				// Token: 0x0400AC3C RID: 44092
				public static LocString TOOLTIP = "See what errands this building can perform and view its current queue";

				// Token: 0x0400AC3D RID: 44093
				public static LocString CHORE_TYPE_TOOLTIP = "Errand Type: {0}";

				// Token: 0x0400AC3E RID: 44094
				public static LocString AVAILABLE_CHORES = "AVAILABLE ERRANDS";

				// Token: 0x0400AC3F RID: 44095
				public static LocString DUPE_TOOLTIP_FAILED = "{Name} cannot currently {Errand}\n\nReason:\n{FailedPrecondition}";

				// Token: 0x0400AC40 RID: 44096
				public static LocString DUPE_TOOLTIP_SUCCEEDED = "{Description}\n\n{Errand}'s Type: {Groups}\n\n{Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n{Building} Priority: {BuildingPriority}\nAll {BestGroup} Errands: {TypePriority}\n\nTotal Priority: {TotalPriority}";

				// Token: 0x0400AC41 RID: 44097
				public static LocString DUPE_TOOLTIP_DESC_ACTIVE = "{Name} is currently busy: \"{Errand}\"";

				// Token: 0x0400AC42 RID: 44098
				public static LocString DUPE_TOOLTIP_DESC_INACTIVE = "\"{Errand}\" is #{Rank} on {Name}'s To Do list, after they finish their current errand";
			}

			// Token: 0x02002728 RID: 10024
			public class PROCESS_CONDITIONS
			{
				// Token: 0x0400AC43 RID: 44099
				public static LocString NAME = "LAUNCH CHECKLIST";

				// Token: 0x0400AC44 RID: 44100
				public static LocString ROCKETPREP = "Rocket Construction";

				// Token: 0x0400AC45 RID: 44101
				public static LocString ROCKETPREP_TOOLTIP = "It is recommended that all boxes on the Rocket Construction checklist be ticked before launching";

				// Token: 0x0400AC46 RID: 44102
				public static LocString ROCKETSTORAGE = "Cargo Manifest";

				// Token: 0x0400AC47 RID: 44103
				public static LocString ROCKETSTORAGE_TOOLTIP = "It is recommended that all boxes on the Cargo Manifest checklist be ticked before launching";

				// Token: 0x0400AC48 RID: 44104
				public static LocString ROCKETFLIGHT = "Flight Route";

				// Token: 0x0400AC49 RID: 44105
				public static LocString ROCKETFLIGHT_TOOLTIP = "A rocket requires a clear path to a set destination to conduct a mission";

				// Token: 0x0400AC4A RID: 44106
				public static LocString ROCKETBOARD = "Crew Manifest";

				// Token: 0x0400AC4B RID: 44107
				public static LocString ROCKETBOARD_TOOLTIP = "It is recommended that all boxes on the Crew Manifest checklist be ticked before launching";

				// Token: 0x0400AC4C RID: 44108
				public static LocString ALL = "Requirements";

				// Token: 0x0400AC4D RID: 44109
				public static LocString ALL_TOOLTIP = "These conditions must be fulfilled in order to launch a rocket mission";
			}
		}

		// Token: 0x02001DCB RID: 7627
		public class BUILDMENU
		{
			// Token: 0x0400881F RID: 34847
			public static LocString GRID_VIEW_TOGGLE_TOOLTIP = "Toggle Grid View";

			// Token: 0x04008820 RID: 34848
			public static LocString LIST_VIEW_TOGGLE_TOOLTIP = "Toggle List View";

			// Token: 0x04008821 RID: 34849
			public static LocString NO_SEARCH_RESULTS = "NO RESULTS FOUND";

			// Token: 0x04008822 RID: 34850
			public static LocString SEARCH_RESULTS_HEADER = "SEARCH RESULTS";

			// Token: 0x04008823 RID: 34851
			public static LocString SEARCH_TEXT_PLACEHOLDER = "Search all buildings...";

			// Token: 0x04008824 RID: 34852
			public static LocString CLEAR_SEARCH_TOOLTIP = "Clear search";
		}

		// Token: 0x02001DCC RID: 7628
		public class BUILDINGEFFECTS
		{
			// Token: 0x04008825 RID: 34853
			public static LocString OPERATIONREQUIREMENTS = "<b>Requirements:</b>";

			// Token: 0x04008826 RID: 34854
			public static LocString REQUIRESPOWER = UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x04008827 RID: 34855
			public static LocString REQUIRESELEMENT = "Supply of {0}";

			// Token: 0x04008828 RID: 34856
			public static LocString REQUIRESLIQUIDINPUT = UI.FormatAsLink("Liquid Intake Pipe", "LIQUIDPIPING");

			// Token: 0x04008829 RID: 34857
			public static LocString REQUIRESLIQUIDOUTPUT = UI.FormatAsLink("Liquid Output Pipe", "LIQUIDPIPING");

			// Token: 0x0400882A RID: 34858
			public static LocString REQUIRESLIQUIDOUTPUTS = "Two " + UI.FormatAsLink("Liquid Output Pipes", "LIQUIDPIPING");

			// Token: 0x0400882B RID: 34859
			public static LocString REQUIRESGASINPUT = UI.FormatAsLink("Gas Intake Pipe", "GASPIPING");

			// Token: 0x0400882C RID: 34860
			public static LocString REQUIRESGASOUTPUT = UI.FormatAsLink("Gas Output Pipe", "GASPIPING");

			// Token: 0x0400882D RID: 34861
			public static LocString REQUIRESGASOUTPUTS = "Two " + UI.FormatAsLink("Gas Output Pipes", "GASPIPING");

			// Token: 0x0400882E RID: 34862
			public static LocString REQUIRESMANUALOPERATION = "Duplicant operation";

			// Token: 0x0400882F RID: 34863
			public static LocString REQUIRESCREATIVITY = "Duplicant " + UI.FormatAsLink("Creativity", "ARTIST");

			// Token: 0x04008830 RID: 34864
			public static LocString REQUIRESPOWERGENERATOR = UI.FormatAsLink("Power", "POWER") + " generator";

			// Token: 0x04008831 RID: 34865
			public static LocString REQUIRESSEED = "1 Unplanted " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x04008832 RID: 34866
			public static LocString PREFERS_ROOM = "Preferred Room: {0}";

			// Token: 0x04008833 RID: 34867
			public static LocString REQUIRESROOM = "Dedicated Room: {0}";

			// Token: 0x04008834 RID: 34868
			public static LocString ALLOWS_FERTILIZER = "Plant " + UI.FormatAsLink("Fertilization", "WILTCONDITIONS");

			// Token: 0x04008835 RID: 34869
			public static LocString ALLOWS_IRRIGATION = "Plant " + UI.FormatAsLink("Liquid", "WILTCONDITIONS");

			// Token: 0x04008836 RID: 34870
			public static LocString ASSIGNEDDUPLICANT = "Duplicant assignment";

			// Token: 0x04008837 RID: 34871
			public static LocString CONSUMESANYELEMENT = "Any Element";

			// Token: 0x04008838 RID: 34872
			public static LocString ENABLESDOMESTICGROWTH = "Enables " + UI.FormatAsLink("Plant Domestication", "PLANTS");

			// Token: 0x04008839 RID: 34873
			public static LocString TRANSFORMER_INPUT_WIRE = "Input " + UI.FormatAsLink("Power Wire", "WIRE");

			// Token: 0x0400883A RID: 34874
			public static LocString TRANSFORMER_OUTPUT_WIRE = "Output " + UI.FormatAsLink("Power Wire", "WIRE") + " (Limited to {0})";

			// Token: 0x0400883B RID: 34875
			public static LocString OPERATIONEFFECTS = "<b>Effects:</b>";

			// Token: 0x0400883C RID: 34876
			public static LocString BATTERYCAPACITY = UI.FormatAsLink("Power", "POWER") + " capacity: {0}";

			// Token: 0x0400883D RID: 34877
			public static LocString BATTERYLEAK = UI.FormatAsLink("Power", "POWER") + " leak: {0}";

			// Token: 0x0400883E RID: 34878
			public static LocString STORAGECAPACITY = "Storage capacity: {0}";

			// Token: 0x0400883F RID: 34879
			public static LocString ELEMENTEMITTED_INPUTTEMP = "{0}: {1}";

			// Token: 0x04008840 RID: 34880
			public static LocString ELEMENTEMITTED_ENTITYTEMP = "{0}: {1}";

			// Token: 0x04008841 RID: 34881
			public static LocString ELEMENTEMITTED_MINORENTITYTEMP = "{0}: {1}";

			// Token: 0x04008842 RID: 34882
			public static LocString ELEMENTEMITTED_MINTEMP = "{0}: {1}";

			// Token: 0x04008843 RID: 34883
			public static LocString ELEMENTEMITTED_FIXEDTEMP = "{0}: {1}";

			// Token: 0x04008844 RID: 34884
			public static LocString ELEMENTCONSUMED = "{0}: {1}";

			// Token: 0x04008845 RID: 34885
			public static LocString ELEMENTEMITTED_TOILET = "{0}: {1} per use";

			// Token: 0x04008846 RID: 34886
			public static LocString ELEMENTEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x04008847 RID: 34887
			public static LocString DISEASEEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x04008848 RID: 34888
			public static LocString DISEASECONSUMEDPERUSE = "All Diseases: -{0} per use";

			// Token: 0x04008849 RID: 34889
			public static LocString ELEMENTCONSUMEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400884A RID: 34890
			public static LocString ENERGYCONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

			// Token: 0x0400884B RID: 34891
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + ": +{0}";

			// Token: 0x0400884C RID: 34892
			public static LocString HEATGENERATED = UI.FormatAsLink("Heat", "HEAT") + ": +{0}/s";

			// Token: 0x0400884D RID: 34893
			public static LocString HEATCONSUMED = UI.FormatAsLink("Heat", "HEAT") + ": -{0}/s";

			// Token: 0x0400884E RID: 34894
			public static LocString HEATER_TARGETTEMPERATURE = "Target " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400884F RID: 34895
			public static LocString HEATGENERATED_AIRCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x04008850 RID: 34896
			public static LocString HEATGENERATED_LIQUIDCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x04008851 RID: 34897
			public static LocString FABRICATES = "Fabricates";

			// Token: 0x04008852 RID: 34898
			public static LocString FABRICATEDITEM = "{1}";

			// Token: 0x04008853 RID: 34899
			public static LocString PROCESSES = "Refines:";

			// Token: 0x04008854 RID: 34900
			public static LocString PROCESSEDITEM = "{1} {0}";

			// Token: 0x04008855 RID: 34901
			public static LocString PLANTERBOX_PENTALTY = "Planter box penalty";

			// Token: 0x04008856 RID: 34902
			public static LocString DECORPROVIDED = UI.FormatAsLink("Decor", "DECOR") + ": {1} (Radius: {2} tiles)";

			// Token: 0x04008857 RID: 34903
			public static LocString OVERHEAT_TEMP = "Overheat " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x04008858 RID: 34904
			public static LocString MINIMUM_TEMP = "Freeze " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x04008859 RID: 34905
			public static LocString OVER_PRESSURE_MASS = "Overpressure: {0}";

			// Token: 0x0400885A RID: 34906
			public static LocString REFILLOXYGENTANK = "Refills Exosuit " + STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME;

			// Token: 0x0400885B RID: 34907
			public static LocString DUPLICANTMOVEMENTBOOST = "Runspeed: {0}";

			// Token: 0x0400885C RID: 34908
			public static LocString STRESSREDUCEDPERMINUTE = UI.FormatAsLink("Stress", "STRESS") + ": {0} per minute";

			// Token: 0x0400885D RID: 34909
			public static LocString REMOVESEFFECTSUBTITLE = "Cures";

			// Token: 0x0400885E RID: 34910
			public static LocString REMOVEDEFFECT = "{0}";

			// Token: 0x0400885F RID: 34911
			public static LocString ADDED_EFFECT = "Added Effect: {0}";

			// Token: 0x04008860 RID: 34912
			public static LocString GASCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x04008861 RID: 34913
			public static LocString LIQUIDCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x04008862 RID: 34914
			public static LocString MAX_WATTAGE = "Max " + UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x04008863 RID: 34915
			public static LocString MAX_BITS = UI.FormatAsLink("Bit", "LOGIC") + " Depth: {0}";

			// Token: 0x04008864 RID: 34916
			public static LocString RESEARCH_MATERIALS = "{0}: {1} per " + UI.FormatAsLink("Research", "RESEARCH") + " point";

			// Token: 0x04008865 RID: 34917
			public static LocString PRODUCES_RESEARCH_POINTS = "{0}";

			// Token: 0x04008866 RID: 34918
			public static LocString HIT_POINTS_PER_CYCLE = UI.FormatAsLink("Health", "Health") + " per cycle: {0}";

			// Token: 0x04008867 RID: 34919
			public static LocString KCAL_PER_CYCLE = UI.FormatAsLink("KCal", "FOOD") + " per cycle: {0}";

			// Token: 0x04008868 RID: 34920
			public static LocString REMOVES_DISEASE = "Kills germs";

			// Token: 0x04008869 RID: 34921
			public static LocString DOCTORING = "Doctoring";

			// Token: 0x0400886A RID: 34922
			public static LocString RECREATION = "Recreation";

			// Token: 0x0400886B RID: 34923
			public static LocString COOLANT = "Coolant: {1} {0}";

			// Token: 0x0400886C RID: 34924
			public static LocString REFINEMENT_ENERGY = "Heat: {0}";

			// Token: 0x0400886D RID: 34925
			public static LocString IMPROVED_BUILDINGS = "Improved Buildings";

			// Token: 0x0400886E RID: 34926
			public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

			// Token: 0x0400886F RID: 34927
			public static LocString GEYSER_PRODUCTION = "{0}: {1} at {2}";

			// Token: 0x04008870 RID: 34928
			public static LocString GEYSER_DISEASE = "Germs: {0}";

			// Token: 0x04008871 RID: 34929
			public static LocString GEYSER_PERIOD = "Eruption Period: {0} every {1}";

			// Token: 0x04008872 RID: 34930
			public static LocString GEYSER_YEAR_UNSTUDIED = "Active Period: (Requires Analysis)";

			// Token: 0x04008873 RID: 34931
			public static LocString GEYSER_YEAR_PERIOD = "Active Period: {0} every {1}";

			// Token: 0x04008874 RID: 34932
			public static LocString GEYSER_YEAR_NEXT_ACTIVE = "Next Activity: {0}";

			// Token: 0x04008875 RID: 34933
			public static LocString GEYSER_YEAR_NEXT_DORMANT = "Next Dormancy: {0}";

			// Token: 0x04008876 RID: 34934
			public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "Average Output: (Requires Analysis)";

			// Token: 0x04008877 RID: 34935
			public static LocString GEYSER_YEAR_AVR_OUTPUT = "Average Output: {0}";

			// Token: 0x04008878 RID: 34936
			public static LocString CAPTURE_METHOD_WRANGLE = "Capture Method: Wrangling";

			// Token: 0x04008879 RID: 34937
			public static LocString CAPTURE_METHOD_LURE = "Capture Method: Lures";

			// Token: 0x0400887A RID: 34938
			public static LocString CAPTURE_METHOD_TRAP = "Capture Method: Traps";

			// Token: 0x0400887B RID: 34939
			public static LocString DIET_HEADER = "Digestion:";

			// Token: 0x0400887C RID: 34940
			public static LocString DIET_CONSUMED = "    • Diet: {Foodlist}";

			// Token: 0x0400887D RID: 34941
			public static LocString DIET_STORED = "    • Stores: {Foodlist}";

			// Token: 0x0400887E RID: 34942
			public static LocString DIET_CONSUMED_ITEM = "{Food}: {Amount}";

			// Token: 0x0400887F RID: 34943
			public static LocString DIET_PRODUCED = "    • Excretion: {Items}";

			// Token: 0x04008880 RID: 34944
			public static LocString DIET_PRODUCED_ITEM = "{Item}: {Percent} of consumed mass";

			// Token: 0x04008881 RID: 34945
			public static LocString DIET_PRODUCED_ITEM_FROM_PLANT = "{Item}: {Amount} when properly fed";

			// Token: 0x04008882 RID: 34946
			public static LocString SCALE_GROWTH = "Shearable {Item}: {Amount} per {Time}";

			// Token: 0x04008883 RID: 34947
			public static LocString SCALE_GROWTH_ATMO = "Shearable {Item}: {Amount} per {Time} ({Atmosphere})";

			// Token: 0x04008884 RID: 34948
			public static LocString SCALE_GROWTH_TEMP = "Shearable {Item}: {Amount} per {Time} ({TempMin}-{TempMax})";

			// Token: 0x04008885 RID: 34949
			public static LocString ACCESS_CONTROL = "Duplicant Access Permissions";

			// Token: 0x04008886 RID: 34950
			public static LocString ROCKETRESTRICTION_HEADER = "Restriction Control:";

			// Token: 0x04008887 RID: 34951
			public static LocString ROCKETRESTRICTION_BUILDINGS = "    • Buildings: {buildinglist}";

			// Token: 0x04008888 RID: 34952
			public static LocString ITEM_TEMPERATURE_ADJUST = "Stored " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x04008889 RID: 34953
			public static LocString NOISE_CREATED = UI.FormatAsLink("Noise", "SOUND") + ": {0} dB (Radius: {1} tiles)";

			// Token: 0x0400888A RID: 34954
			public static LocString MESS_TABLE_SALT = "Table Salt: +{0}";

			// Token: 0x0400888B RID: 34955
			public static LocString ACTIVE_PARTICLE_CONSUMPTION = "Radbolts: {Rate}";

			// Token: 0x0400888C RID: 34956
			public static LocString PARTICLE_PORT_INPUT = "Radbolt Input Port";

			// Token: 0x0400888D RID: 34957
			public static LocString PARTICLE_PORT_OUTPUT = "Radbolt Output Port";

			// Token: 0x0400888E RID: 34958
			public static LocString IN_ORBIT_REQUIRED = "Active In Space";

			// Token: 0x02002729 RID: 10025
			public class TOOLTIPS
			{
				// Token: 0x0400AC4E RID: 44110
				public static LocString OPERATIONREQUIREMENTS = "All requirements must be met in order for this building to operate";

				// Token: 0x0400AC4F RID: 44111
				public static LocString REQUIRESPOWER = string.Concat(new string[]
				{
					"Must be connected to a power grid with at least ",
					UI.FormatAsNegativeRate("{0}"),
					" of available ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});

				// Token: 0x0400AC50 RID: 44112
				public static LocString REQUIRESELEMENT = string.Concat(new string[]
				{
					"Must receive deliveries of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" to function"
				});

				// Token: 0x0400AC51 RID: 44113
				public static LocString REQUIRESLIQUIDINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC52 RID: 44114
				public static LocString REQUIRESLIQUIDOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC53 RID: 44115
				public static LocString REQUIRESLIQUIDOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC54 RID: 44116
				public static LocString REQUIRESGASINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" from a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC55 RID: 44117
				public static LocString REQUIRESGASOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC56 RID: 44118
				public static LocString REQUIRESGASOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400AC57 RID: 44119
				public static LocString REQUIRESMANUALOPERATION = "A Duplicant must be present to run this building";

				// Token: 0x0400AC58 RID: 44120
				public static LocString REQUIRESCREATIVITY = "A Duplicant must work on this object to create " + UI.PRE_KEYWORD + "Art" + UI.PST_KEYWORD;

				// Token: 0x0400AC59 RID: 44121
				public static LocString REQUIRESPOWERGENERATOR = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" producing generator to function"
				});

				// Token: 0x0400AC5A RID: 44122
				public static LocString REQUIRESSEED = "Must receive a plant " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;

				// Token: 0x0400AC5B RID: 44123
				public static LocString PREFERS_ROOM = "This building gains additional effects or functionality when built inside a " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400AC5C RID: 44124
				public static LocString REQUIRESROOM = string.Concat(new string[]
				{
					"Must be built within a dedicated ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" will become a ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" after construction"
				});

				// Token: 0x0400AC5D RID: 44125
				public static LocString ALLOWS_FERTILIZER = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Fertilizer",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400AC5E RID: 44126
				public static LocString ALLOWS_IRRIGATION = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400AC5F RID: 44127
				public static LocString ALLOWS_IRRIGATION_PIPE = string.Concat(new string[]
				{
					"Allows irrigation ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" connection"
				});

				// Token: 0x0400AC60 RID: 44128
				public static LocString ASSIGNEDDUPLICANT = "This amenity may only be used by the Duplicant it is assigned to";

				// Token: 0x0400AC61 RID: 44129
				public static LocString OPERATIONEFFECTS = "The building will produce these effects when its requirements are met";

				// Token: 0x0400AC62 RID: 44130
				public static LocString BATTERYCAPACITY = string.Concat(new string[]
				{
					"Can hold <b>{0}</b> of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" when connected to a ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD
				});

				// Token: 0x0400AC63 RID: 44131
				public static LocString BATTERYLEAK = string.Concat(new string[]
				{
					UI.FormatAsNegativeRate("{0}"),
					" of this battery's charge will be lost as ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD
				});

				// Token: 0x0400AC64 RID: 44132
				public static LocString STORAGECAPACITY = "Holds up to <b>{0}</b> of material";

				// Token: 0x0400AC65 RID: 44133
				public static LocString ELEMENTEMITTED_INPUTTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the combined ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400AC66 RID: 44134
				public static LocString ELEMENTEMITTED_ENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the building at the time of production"
				});

				// Token: 0x0400AC67 RID: 44135
				public static LocString ELEMENTEMITTED_MINORENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the building is hotter."
				});

				// Token: 0x0400AC68 RID: 44136
				public static LocString ELEMENTEMITTED_MINTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the input materials are hotter."
				});

				// Token: 0x0400AC69 RID: 44137
				public static LocString ELEMENTEMITTED_FIXEDTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be produced at <b>{2}</b>."
				});

				// Token: 0x0400AC6A RID: 44138
				public static LocString ELEMENTCONSUMED = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use"
				});

				// Token: 0x0400AC6B RID: 44139
				public static LocString ELEMENTEMITTED_TOILET = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nDuplicant waste is emitted at <b>{2}</b>."
				});

				// Token: 0x0400AC6C RID: 44140
				public static LocString ELEMENTEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400AC6D RID: 44141
				public static LocString DISEASEEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400AC6E RID: 44142
				public static LocString DISEASECONSUMEDPERUSE = "Removes " + UI.FormatAsNegativeRate("{0}") + " per use";

				// Token: 0x0400AC6F RID: 44143
				public static LocString ELEMENTCONSUMEDPERUSE = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400AC70 RID: 44144
				public static LocString ENERGYCONSUMED = string.Concat(new string[]
				{
					"Draws ",
					UI.FormatAsNegativeRate("{0}"),
					" from the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400AC71 RID: 44145
				public static LocString ENERGYGENERATED = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{0}"),
					" for the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400AC72 RID: 44146
				public static LocString ENABLESDOMESTICGROWTH = string.Concat(new string[]
				{
					"Accelerates ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" growth and maturation"
				});

				// Token: 0x0400AC73 RID: 44147
				public static LocString HEATGENERATED = string.Concat(new string[]
				{
					"Generates ",
					UI.FormatAsPositiveRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change is affected by the material attributes of the heated substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400AC74 RID: 44148
				public static LocString HEATCONSUMED = string.Concat(new string[]
				{
					"Dissipates ",
					UI.FormatAsNegativeRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change can be affected by the material attributes of the cooled substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400AC75 RID: 44149
				public static LocString HEATER_TARGETTEMPERATURE = string.Concat(new string[]
				{
					"Stops heating when the surrounding average ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400AC76 RID: 44150
				public static LocString FABRICATES = "Fabrication is the production of items and equipment";

				// Token: 0x0400AC77 RID: 44151
				public static LocString PROCESSES = "Processes raw materials into refined materials";

				// Token: 0x0400AC78 RID: 44152
				public static LocString PROCESSEDITEM = "Refining this material produces " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400AC79 RID: 44153
				public static LocString PLANTERBOX_PENTALTY = "Plants grow more slowly when contained within boxes";

				// Token: 0x0400AC7A RID: 44154
				public static LocString DECORPROVIDED = string.Concat(new string[]
				{
					"Improves ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsPositiveModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400AC7B RID: 44155
				public static LocString DECORDECREASED = string.Concat(new string[]
				{
					"Decreases ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsNegativeModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400AC7C RID: 44156
				public static LocString OVERHEAT_TEMP = "Begins overheating at <b>{0}</b>";

				// Token: 0x0400AC7D RID: 44157
				public static LocString MINIMUM_TEMP = "Ceases to function when temperatures fall below <b>{0}</b>";

				// Token: 0x0400AC7E RID: 44158
				public static LocString OVER_PRESSURE_MASS = "Ceases to function when the surrounding mass is above <b>{0}</b>";

				// Token: 0x0400AC7F RID: 44159
				public static LocString REFILLOXYGENTANK = string.Concat(new string[]
				{
					"Refills ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" Oxygen tanks with ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" for reuse"
				});

				// Token: 0x0400AC80 RID: 44160
				public static LocString DUPLICANTMOVEMENTBOOST = "Duplicants walk <b>{0}</b> faster on this tile";

				// Token: 0x0400AC81 RID: 44161
				public static LocString STRESSREDUCEDPERMINUTE = string.Concat(new string[]
				{
					"Removes <b>{0}</b> of Duplicants' ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" for every uninterrupted minute of use"
				});

				// Token: 0x0400AC82 RID: 44162
				public static LocString REMOVESEFFECTSUBTITLE = "Use of this building will remove the listed effects";

				// Token: 0x0400AC83 RID: 44163
				public static LocString REMOVEDEFFECT = "{0}";

				// Token: 0x0400AC84 RID: 44164
				public static LocString ADDED_EFFECT = "Effect being applied:\n\n{0}\n{1}";

				// Token: 0x0400AC85 RID: 44165
				public static LocString GASCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400AC86 RID: 44166
				public static LocString LIQUIDCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400AC87 RID: 44167
				public static LocString MAX_WATTAGE = string.Concat(new string[]
				{
					"Drawing more than the maximum allowed ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400AC88 RID: 44168
				public static LocString MAX_BITS = string.Concat(new string[]
				{
					"Sending an ",
					UI.PRE_KEYWORD,
					"Automation Signal",
					UI.PST_KEYWORD,
					" with a higher ",
					UI.PRE_KEYWORD,
					"Bit Depth",
					UI.PST_KEYWORD,
					" than the connected ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400AC89 RID: 44169
				public static LocString RESEARCH_MATERIALS = string.Concat(new string[]
				{
					"This research station consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for each ",
					UI.PRE_KEYWORD,
					"Research Point",
					UI.PST_KEYWORD,
					" produced"
				});

				// Token: 0x0400AC8A RID: 44170
				public static LocString PRODUCES_RESEARCH_POINTS = string.Concat(new string[]
				{
					"Produces ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" research"
				});

				// Token: 0x0400AC8B RID: 44171
				public static LocString REMOVES_DISEASE = string.Concat(new string[]
				{
					"The cooking process kills all ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present in the ingredients, removing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" risk when eating the product"
				});

				// Token: 0x0400AC8C RID: 44172
				public static LocString DOCTORING = "Doctoring increases existing health benefits and can allow the treatment of otherwise stubborn " + UI.PRE_KEYWORD + "Diseases" + UI.PST_KEYWORD;

				// Token: 0x0400AC8D RID: 44173
				public static LocString RECREATION = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" during scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD
				});

				// Token: 0x0400AC8E RID: 44174
				public static LocString HEATGENERATED_AIRCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					"\n\nCooling 1",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.OXYGEN.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400AC8F RID: 44175
				public static LocString HEATGENERATED_LIQUIDCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					"\n\nCooling 10",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.WATER.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400AC90 RID: 44176
				public static LocString MOVEMENT_BONUS = "Increases the Runspeed of Duplicants";

				// Token: 0x0400AC91 RID: 44177
				public static LocString COOLANT = string.Concat(new string[]
				{
					"<b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" coolant is required to cool off an item produced by this building\n\nCoolant ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" increase is variable and dictated by the amount of energy needed to cool the produced item"
				});

				// Token: 0x0400AC92 RID: 44178
				public static LocString REFINEMENT_ENERGY_HAS_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nThis will raise the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the contained ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and heat the containing building"
				});

				// Token: 0x0400AC93 RID: 44179
				public static LocString REFINEMENT_ENERGY_NO_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nIf ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" is used for coolant, its ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will be raised by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and will heat the containing building"
				});

				// Token: 0x0400AC94 RID: 44180
				public static LocString IMPROVED_BUILDINGS = UI.PRE_KEYWORD + "Tune Ups" + UI.PST_KEYWORD + " will improve these buildings:";

				// Token: 0x0400AC95 RID: 44181
				public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

				// Token: 0x0400AC96 RID: 44182
				public static LocString GEYSER_PRODUCTION = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400AC97 RID: 44183
				public static LocString GEYSER_PRODUCTION_GEOTUNED = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400AC98 RID: 44184
				public static LocString GEYSER_PRODUCTION_GEOTUNED_COUNT = "<b>{0}</b> of <b>{1}</b> Geotuners targeting this geyser are amplifying it";

				// Token: 0x0400AC99 RID: 44185
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL = "Total geotuning: {0} {1}";

				// Token: 0x0400AC9A RID: 44186
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL_ROW_TITLE = "Geotuned ";

				// Token: 0x0400AC9B RID: 44187
				public static LocString GEYSER_DISEASE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " germs are present in the output of this geyser";

				// Token: 0x0400AC9C RID: 44188
				public static LocString GEYSER_PERIOD = "This geyser will produce for <b>{0}</b> of every <b>{1}</b>";

				// Token: 0x0400AC9D RID: 44189
				public static LocString GEYSER_YEAR_UNSTUDIED = "A researcher must analyze this geyser to determine its geoactive period";

				// Token: 0x0400AC9E RID: 44190
				public static LocString GEYSER_YEAR_PERIOD = "This geyser will be active for <b>{0}</b> out of every <b>{1}</b>\n\nIt will be dormant the rest of the time";

				// Token: 0x0400AC9F RID: 44191
				public static LocString GEYSER_YEAR_NEXT_ACTIVE = "This geyser will become active in <b>{0}</b>";

				// Token: 0x0400ACA0 RID: 44192
				public static LocString GEYSER_YEAR_NEXT_DORMANT = "This geyser will become dormant in <b>{0}</b>";

				// Token: 0x0400ACA1 RID: 44193
				public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "A researcher must analyze this geyser to determine its average output rate";

				// Token: 0x0400ACA2 RID: 44194
				public static LocString GEYSER_YEAR_AVR_OUTPUT = "This geyser emits an average of {average} of {element} during its lifetime\n\nThis includes its dormant period";

				// Token: 0x0400ACA3 RID: 44195
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE = "Total Geotuning ";

				// Token: 0x0400ACA4 RID: 44196
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW = "Geotuned ";

				// Token: 0x0400ACA5 RID: 44197
				public static LocString CAPTURE_METHOD_WRANGLE = string.Concat(new string[]
				{
					"This critter can be captured\n\nMark critters for capture using the ",
					UI.FormatAsTool("Wrangle Tool", global::Action.Capture),
					"\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400ACA6 RID: 44198
				public static LocString CAPTURE_METHOD_LURE = "This critter can be moved using an " + BUILDINGS.PREFABS.AIRBORNECREATURELURE.NAME;

				// Token: 0x0400ACA7 RID: 44199
				public static LocString CAPTURE_METHOD_TRAP = "This critter can be captured using a " + BUILDINGS.PREFABS.CREATURETRAP.NAME;

				// Token: 0x0400ACA8 RID: 44200
				public static LocString NOISE_POLLUTION_INCREASE = "Produces noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400ACA9 RID: 44201
				public static LocString NOISE_POLLUTION_DECREASE = "Dampens noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400ACAA RID: 44202
				public static LocString ITEM_TEMPERATURE_ADJUST = string.Concat(new string[]
				{
					"Stored items will reach a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{0}</b> over time"
				});

				// Token: 0x0400ACAB RID: 44203
				public static LocString DIET_HEADER = "Creatures will eat and digest only specific materials";

				// Token: 0x0400ACAC RID: 44204
				public static LocString DIET_CONSUMED = "This critter can typically consume these materials at the following rates:\n\n{Foodlist}";

				// Token: 0x0400ACAD RID: 44205
				public static LocString DIET_PRODUCED = "This critter will \"produce\" the following materials:\n\n{Items}";

				// Token: 0x0400ACAE RID: 44206
				public static LocString ROCKETRESTRICTION_HEADER = "Controls whether a building is operational within a rocket interior";

				// Token: 0x0400ACAF RID: 44207
				public static LocString ROCKETRESTRICTION_BUILDINGS = "This station controls the operational status of the following buildings:\n\n{buildinglist}";

				// Token: 0x0400ACB0 RID: 44208
				public static LocString SCALE_GROWTH = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400ACB1 RID: 44209
				public static LocString SCALE_GROWTH_ATMO = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be kept in ",
					UI.PRE_KEYWORD,
					"{Atmosphere}",
					UI.PST_KEYWORD,
					"-rich environments to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400ACB2 RID: 44210
				public static LocString SCALE_GROWTH_TEMP = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must eat food between {TempMin}-{TempMax} to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400ACB3 RID: 44211
				public static LocString MESS_TABLE_SALT = string.Concat(new string[]
				{
					"Duplicants gain ",
					UI.FormatAsPositiveModifier("+{0}"),
					" ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when using ",
					UI.PRE_KEYWORD,
					"Table Salt",
					UI.PST_KEYWORD,
					" with their food at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME
				});

				// Token: 0x0400ACB4 RID: 44212
				public static LocString ACCESS_CONTROL = "Settings to allow or restrict Duplicants from passing through the door.";

				// Token: 0x0400ACB5 RID: 44213
				public static LocString TRANSFORMER_INPUT_WIRE = string.Concat(new string[]
				{
					"Connect a ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" to the large ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" with any amount of ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400ACB6 RID: 44214
				public static LocString TRANSFORMER_OUTPUT_WIRE = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" provided by the the small ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" will be limited to {0}."
				});

				// Token: 0x0400ACB7 RID: 44215
				public static LocString FABRICATOR_INGREDIENTS = "Ingredients:\n{0}";

				// Token: 0x0400ACB8 RID: 44216
				public static LocString ACTIVE_PARTICLE_CONSUMPTION = string.Concat(new string[]
				{
					"This building requires ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" to function, consuming them at a rate of {Rate} while in use"
				});

				// Token: 0x0400ACB9 RID: 44217
				public static LocString PARTICLE_PORT_INPUT = "A Radbolt Port on this building allows it to receive " + UI.PRE_KEYWORD + "Radbolts" + UI.PST_KEYWORD;

				// Token: 0x0400ACBA RID: 44218
				public static LocString PARTICLE_PORT_OUTPUT = string.Concat(new string[]
				{
					"This building has a configurable Radbolt Port for ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" emission"
				});

				// Token: 0x0400ACBB RID: 44219
				public static LocString IN_ORBIT_REQUIRED = "This building is only operational while its parent rocket is in flight";
			}
		}

		// Token: 0x02001DCD RID: 7629
		public class LOGIC_PORTS
		{
			// Token: 0x0400888F RID: 34959
			public static LocString INPUT_PORTS = UI.FormatAsLink("Auto Inputs", "LOGIC");

			// Token: 0x04008890 RID: 34960
			public static LocString INPUT_PORTS_TOOLTIP = "Input ports change a state on this building when a signal is received";

			// Token: 0x04008891 RID: 34961
			public static LocString OUTPUT_PORTS = UI.FormatAsLink("Auto Outputs", "LOGIC");

			// Token: 0x04008892 RID: 34962
			public static LocString OUTPUT_PORTS_TOOLTIP = "Output ports send a signal when this building changes state";

			// Token: 0x04008893 RID: 34963
			public static LocString INPUT_PORT_TOOLTIP = "Input Behavior:\n• {0}\n• {1}";

			// Token: 0x04008894 RID: 34964
			public static LocString OUTPUT_PORT_TOOLTIP = "Output Behavior:\n• {0}\n• {1}";

			// Token: 0x04008895 RID: 34965
			public static LocString CONTROL_OPERATIONAL = "Enable/Disable";

			// Token: 0x04008896 RID: 34966
			public static LocString CONTROL_OPERATIONAL_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable building";

			// Token: 0x04008897 RID: 34967
			public static LocString CONTROL_OPERATIONAL_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable building";

			// Token: 0x04008898 RID: 34968
			public static LocString PORT_INPUT_DEFAULT_NAME = "INPUT";

			// Token: 0x04008899 RID: 34969
			public static LocString PORT_OUTPUT_DEFAULT_NAME = "OUTPUT";

			// Token: 0x0400889A RID: 34970
			public static LocString GATE_MULTI_INPUT_ONE_NAME = "INPUT A";

			// Token: 0x0400889B RID: 34971
			public static LocString GATE_MULTI_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400889C RID: 34972
			public static LocString GATE_MULTI_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400889D RID: 34973
			public static LocString GATE_MULTI_INPUT_TWO_NAME = "INPUT B";

			// Token: 0x0400889E RID: 34974
			public static LocString GATE_MULTI_INPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x0400889F RID: 34975
			public static LocString GATE_MULTI_INPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x040088A0 RID: 34976
			public static LocString GATE_MULTI_INPUT_THREE_NAME = "INPUT C";

			// Token: 0x040088A1 RID: 34977
			public static LocString GATE_MULTI_INPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x040088A2 RID: 34978
			public static LocString GATE_MULTI_INPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x040088A3 RID: 34979
			public static LocString GATE_MULTI_INPUT_FOUR_NAME = "INPUT D";

			// Token: 0x040088A4 RID: 34980
			public static LocString GATE_MULTI_INPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x040088A5 RID: 34981
			public static LocString GATE_MULTI_INPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x040088A6 RID: 34982
			public static LocString GATE_SINGLE_INPUT_ONE_NAME = "INPUT";

			// Token: 0x040088A7 RID: 34983
			public static LocString GATE_SINGLE_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x040088A8 RID: 34984
			public static LocString GATE_SINGLE_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x040088A9 RID: 34985
			public static LocString GATE_MULTI_OUTPUT_ONE_NAME = "OUTPUT A";

			// Token: 0x040088AA RID: 34986
			public static LocString GATE_MULTI_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x040088AB RID: 34987
			public static LocString GATE_MULTI_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x040088AC RID: 34988
			public static LocString GATE_MULTI_OUTPUT_TWO_NAME = "OUTPUT B";

			// Token: 0x040088AD RID: 34989
			public static LocString GATE_MULTI_OUTPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x040088AE RID: 34990
			public static LocString GATE_MULTI_OUTPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x040088AF RID: 34991
			public static LocString GATE_MULTI_OUTPUT_THREE_NAME = "OUTPUT C";

			// Token: 0x040088B0 RID: 34992
			public static LocString GATE_MULTI_OUTPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x040088B1 RID: 34993
			public static LocString GATE_MULTI_OUTPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x040088B2 RID: 34994
			public static LocString GATE_MULTI_OUTPUT_FOUR_NAME = "OUTPUT D";

			// Token: 0x040088B3 RID: 34995
			public static LocString GATE_MULTI_OUTPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x040088B4 RID: 34996
			public static LocString GATE_MULTI_OUTPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x040088B5 RID: 34997
			public static LocString GATE_SINGLE_OUTPUT_ONE_NAME = "OUTPUT";

			// Token: 0x040088B6 RID: 34998
			public static LocString GATE_SINGLE_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x040088B7 RID: 34999
			public static LocString GATE_SINGLE_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x040088B8 RID: 35000
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_NAME = "CONTROL A";

			// Token: 0x040088B9 RID: 35001
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x040088BA RID: 35002
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";

			// Token: 0x040088BB RID: 35003
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_NAME = "CONTROL B";

			// Token: 0x040088BC RID: 35004
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x040088BD RID: 35005
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";
		}

		// Token: 0x02001DCE RID: 7630
		public class GAMEOBJECTEFFECTS
		{
			// Token: 0x040088BE RID: 35006
			public static LocString CALORIES = "+{0}";

			// Token: 0x040088BF RID: 35007
			public static LocString FOOD_QUALITY = "Quality: {0}";

			// Token: 0x040088C0 RID: 35008
			public static LocString FOOD_MORALE = "Morale: {0}";

			// Token: 0x040088C1 RID: 35009
			public static LocString FORGAVEATTACKER = "Forgiveness";

			// Token: 0x040088C2 RID: 35010
			public static LocString COLDBREATHER = UI.FormatAsLink("Cooling Effect", "HEAT");

			// Token: 0x040088C3 RID: 35011
			public static LocString LIFECYCLETITLE = "Growth:";

			// Token: 0x040088C4 RID: 35012
			public static LocString GROWTHTIME_SIMPLE = "Life Cycle: {0}";

			// Token: 0x040088C5 RID: 35013
			public static LocString GROWTHTIME_REGROWTH = "Domestic growth: {0} / {1}";

			// Token: 0x040088C6 RID: 35014
			public static LocString GROWTHTIME = "Growth: {0}";

			// Token: 0x040088C7 RID: 35015
			public static LocString INITIALGROWTHTIME = "Initial Growth: {0}";

			// Token: 0x040088C8 RID: 35016
			public static LocString REGROWTHTIME = "Regrowth: {0}";

			// Token: 0x040088C9 RID: 35017
			public static LocString REQUIRES_LIGHT = UI.FormatAsLink("Light", "LIGHT") + ": {Lux}";

			// Token: 0x040088CA RID: 35018
			public static LocString REQUIRES_DARKNESS = UI.FormatAsLink("Darkness", "LIGHT");

			// Token: 0x040088CB RID: 35019
			public static LocString REQUIRESFERTILIZER = "{0}: {1}";

			// Token: 0x040088CC RID: 35020
			public static LocString IDEAL_FERTILIZER = "{0}: {1}";

			// Token: 0x040088CD RID: 35021
			public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

			// Token: 0x040088CE RID: 35022
			public static LocString ROTTEN = "Rotten";

			// Token: 0x040088CF RID: 35023
			public static LocString REQUIRES_ATMOSPHERE = UI.FormatAsLink("Atmosphere", "ATMOSPHERE") + ": {0}";

			// Token: 0x040088D0 RID: 35024
			public static LocString REQUIRES_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0} minimum";

			// Token: 0x040088D1 RID: 35025
			public static LocString IDEAL_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0}";

			// Token: 0x040088D2 RID: 35026
			public static LocString REQUIRES_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x040088D3 RID: 35027
			public static LocString IDEAL_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x040088D4 RID: 35028
			public static LocString REQUIRES_SUBMERSION = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Submersion";

			// Token: 0x040088D5 RID: 35029
			public static LocString FOOD_EFFECTS = "Effects:";

			// Token: 0x040088D6 RID: 35030
			public static LocString EMITS_LIGHT = UI.FormatAsLink("Light Range", "LIGHT") + ": {0} tiles";

			// Token: 0x040088D7 RID: 35031
			public static LocString EMITS_LIGHT_LUX = UI.FormatAsLink("Brightness", "LIGHT") + ": {0} Lux";

			// Token: 0x040088D8 RID: 35032
			public static LocString AMBIENT_RADIATION = "Ambient Radiation";

			// Token: 0x040088D9 RID: 35033
			public static LocString AMBIENT_RADIATION_FMT = "{minRads} - {maxRads}";

			// Token: 0x040088DA RID: 35034
			public static LocString AMBIENT_NO_MIN_RADIATION_FMT = "Less than {maxRads}";

			// Token: 0x040088DB RID: 35035
			public static LocString REQUIRES_NO_MIN_RADIATION = "Maximum " + UI.FormatAsLink("Radiation", "RADIATION") + ": {MaxRads}";

			// Token: 0x040088DC RID: 35036
			public static LocString REQUIRES_RADIATION = UI.FormatAsLink("Radiation", "RADIATION") + ": {MinRads} to {MaxRads}";

			// Token: 0x040088DD RID: 35037
			public static LocString MUTANT_STERILE = "Doesn't Drop " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x040088DE RID: 35038
			public static LocString DARKNESS = "Darkness";

			// Token: 0x040088DF RID: 35039
			public static LocString LIGHT = "Light";

			// Token: 0x040088E0 RID: 35040
			public static LocString SEED_PRODUCTION_DIG_ONLY = "Consumes 1 " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x040088E1 RID: 35041
			public static LocString SEED_PRODUCTION_HARVEST = "Harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x040088E2 RID: 35042
			public static LocString SEED_PRODUCTION_FINAL_HARVEST = "Final harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x040088E3 RID: 35043
			public static LocString SEED_PRODUCTION_FRUIT = "Fruit produces " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x040088E4 RID: 35044
			public static LocString SEED_REQUIREMENT_CEILING = "Plot Orientation: Downward";

			// Token: 0x040088E5 RID: 35045
			public static LocString SEED_REQUIREMENT_WALL = "Plot Orientation: Sideways";

			// Token: 0x040088E6 RID: 35046
			public static LocString REQUIRES_RECEPTACLE = "Farm Plot";

			// Token: 0x040088E7 RID: 35047
			public static LocString PLANT_MARK_FOR_HARVEST = "Autoharvest Enabled";

			// Token: 0x040088E8 RID: 35048
			public static LocString PLANT_DO_NOT_HARVEST = "Autoharvest Disabled";

			// Token: 0x0200272A RID: 10026
			public class INSULATED
			{
				// Token: 0x0400ACBC RID: 44220
				public static LocString NAME = "Insulated";

				// Token: 0x0400ACBD RID: 44221
				public static LocString TOOLTIP = "Proper insulation drastically reduces thermal conductivity";
			}

			// Token: 0x0200272B RID: 10027
			public class TOOLTIPS
			{
				// Token: 0x0400ACBE RID: 44222
				public static LocString CALORIES = "+{0}";

				// Token: 0x0400ACBF RID: 44223
				public static LocString FOOD_QUALITY = "Quality: {0}";

				// Token: 0x0400ACC0 RID: 44224
				public static LocString FOOD_MORALE = "Morale: {0}";

				// Token: 0x0400ACC1 RID: 44225
				public static LocString COLDBREATHER = "Lowers ambient air temperature";

				// Token: 0x0400ACC2 RID: 44226
				public static LocString GROWTHTIME_SIMPLE = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400ACC3 RID: 44227
				public static LocString GROWTHTIME_REGROWTH = "This plant initially takes <b>{0}</b> to grow, but only <b>{1}</b> to mature after first harvest";

				// Token: 0x0400ACC4 RID: 44228
				public static LocString GROWTHTIME = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400ACC5 RID: 44229
				public static LocString INITIALGROWTHTIME = "This plant takes <b>{0}</b> to mature again once replanted";

				// Token: 0x0400ACC6 RID: 44230
				public static LocString REGROWTHTIME = "This plant takes <b>{0}</b> to mature again once harvested";

				// Token: 0x0400ACC7 RID: 44231
				public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

				// Token: 0x0400ACC8 RID: 44232
				public static LocString REQUIRESFERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400ACC9 RID: 44233
				public static LocString IDEAL_FERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400ACCA RID: 44234
				public static LocString REQUIRES_LIGHT = string.Concat(new string[]
				{
					"This plant requires a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" source bathing it in at least {Lux}"
				});

				// Token: 0x0400ACCB RID: 44235
				public static LocString REQUIRES_DARKNESS = "This plant requires complete darkness";

				// Token: 0x0400ACCC RID: 44236
				public static LocString REQUIRES_ATMOSPHERE = "This plant must be submerged in one of the following gases: {0}";

				// Token: 0x0400ACCD RID: 44237
				public static LocString REQUIRES_ATMOSPHERE_LIQUID = "This plant must be submerged in one of the following liquids: {0}";

				// Token: 0x0400ACCE RID: 44238
				public static LocString REQUIRES_ATMOSPHERE_MIXED = "This plant must be submerged in one of the following gases or liquids: {0}";

				// Token: 0x0400ACCF RID: 44239
				public static LocString REQUIRES_PRESSURE = string.Concat(new string[]
				{
					"Ambient ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressure must be at least <b>{0}</b> for basic growth"
				});

				// Token: 0x0400ACD0 RID: 44240
				public static LocString IDEAL_PRESSURE = string.Concat(new string[]
				{
					"This plant requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressures above <b>{0}</b> for basic growth"
				});

				// Token: 0x0400ACD1 RID: 44241
				public static LocString REQUIRES_TEMPERATURE = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" must be between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400ACD2 RID: 44242
				public static LocString IDEAL_TEMPERATURE = string.Concat(new string[]
				{
					"This plant requires internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400ACD3 RID: 44243
				public static LocString REQUIRES_SUBMERSION = string.Concat(new string[]
				{
					"This plant must be fully submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400ACD4 RID: 44244
				public static LocString FOOD_EFFECTS = "Duplicants will gain the following effects from eating this food: {0}";

				// Token: 0x0400ACD5 RID: 44245
				public static LocString REQUIRES_RECEPTACLE = string.Concat(new string[]
				{
					"This plant must be housed in a ",
					UI.FormatAsLink("Planter Box", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tile", "FARMTILE"),
					", or ",
					UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM"),
					" farm to grow domestically"
				});

				// Token: 0x0400ACD6 RID: 44246
				public static LocString EMITS_LIGHT = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400ACD7 RID: 44247
				public static LocString EMITS_LIGHT_LUX = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400ACD8 RID: 44248
				public static LocString METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP = "Distribution of meteor types in this shower";

				// Token: 0x0400ACD9 RID: 44249
				public static LocString SEED_PRODUCTION_DIG_ONLY = "May be replanted, but will produce no further " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400ACDA RID: 44250
				public static LocString SEED_PRODUCTION_HARVEST = "Harvesting this plant will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400ACDB RID: 44251
				public static LocString SEED_PRODUCTION_FINAL_HARVEST = string.Concat(new string[]
				{
					"Yields new ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" on the final harvest of its life cycle"
				});

				// Token: 0x0400ACDC RID: 44252
				public static LocString SEED_PRODUCTION_FRUIT = "Consuming this plant's fruit will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400ACDD RID: 44253
				public static LocString SEED_REQUIREMENT_CEILING = "This seed must be planted in a downward facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400ACDE RID: 44254
				public static LocString SEED_REQUIREMENT_WALL = "This seed must be planted in a side facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400ACDF RID: 44255
				public static LocString REQUIRES_NO_MIN_RADIATION = "This plant will stop growing if exposed to more than {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400ACE0 RID: 44256
				public static LocString REQUIRES_RADIATION = "This plant will only grow if it has between {MinRads} and {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400ACE1 RID: 44257
				public static LocString MUTANT_SEED_TOOLTIP = "\n\nGrowing near its maximum radiation increases the chance of mutant seeds being produced";

				// Token: 0x0400ACE2 RID: 44258
				public static LocString MUTANT_STERILE = "This plant will not produce seeds of its own due to changes to its DNA";
			}

			// Token: 0x0200272C RID: 10028
			public class DAMAGE_POPS
			{
				// Token: 0x0400ACE3 RID: 44259
				public static LocString OVERHEAT = "Overheat Damage";

				// Token: 0x0400ACE4 RID: 44260
				public static LocString CORROSIVE_ELEMENT = "Corrosive Element Damage";

				// Token: 0x0400ACE5 RID: 44261
				public static LocString WRONG_ELEMENT = "Wrong Element Damage";

				// Token: 0x0400ACE6 RID: 44262
				public static LocString CIRCUIT_OVERLOADED = "Overload Damage";

				// Token: 0x0400ACE7 RID: 44263
				public static LocString LOGIC_CIRCUIT_OVERLOADED = "Signal Overload Damage";

				// Token: 0x0400ACE8 RID: 44264
				public static LocString LIQUID_PRESSURE = "Pressure Damage";

				// Token: 0x0400ACE9 RID: 44265
				public static LocString MINION_DESTRUCTION = "Tantrum Damage";

				// Token: 0x0400ACEA RID: 44266
				public static LocString CONDUIT_CONTENTS_FROZE = "Cold Damage";

				// Token: 0x0400ACEB RID: 44267
				public static LocString CONDUIT_CONTENTS_BOILED = "Heat Damage";

				// Token: 0x0400ACEC RID: 44268
				public static LocString MICROMETEORITE = "Micrometeorite Damage";

				// Token: 0x0400ACED RID: 44269
				public static LocString COMET = "Meteor Damage";

				// Token: 0x0400ACEE RID: 44270
				public static LocString ROCKET = "Rocket Thruster Damage";
			}
		}

		// Token: 0x02001DCF RID: 7631
		public class ASTEROIDCLOCK
		{
			// Token: 0x040088E9 RID: 35049
			public static LocString CYCLE = "Cycle";

			// Token: 0x040088EA RID: 35050
			public static LocString CYCLES_OLD = "This Colony is {0} Cycle(s) Old";

			// Token: 0x040088EB RID: 35051
			public static LocString TIME_PLAYED = "Time Played: {0} hours";

			// Token: 0x040088EC RID: 35052
			public static LocString SCHEDULE_BUTTON_TOOLTIP = "Manage Schedule";
		}

		// Token: 0x02001DD0 RID: 7632
		public class ENDOFDAYREPORT
		{
			// Token: 0x040088ED RID: 35053
			public static LocString REPORT_TITLE = "DAILY REPORTS";

			// Token: 0x040088EE RID: 35054
			public static LocString DAY_TITLE = "Cycle {0}";

			// Token: 0x040088EF RID: 35055
			public static LocString DAY_TITLE_TODAY = "Cycle {0} - Today";

			// Token: 0x040088F0 RID: 35056
			public static LocString DAY_TITLE_YESTERDAY = "Cycle {0} - Yesterday";

			// Token: 0x040088F1 RID: 35057
			public static LocString NOTIFICATION_TITLE = "Cycle {0} report ready";

			// Token: 0x040088F2 RID: 35058
			public static LocString NOTIFICATION_TOOLTIP = "The daily report for Cycle {0} is ready to view";

			// Token: 0x040088F3 RID: 35059
			public static LocString NEXT = "Next";

			// Token: 0x040088F4 RID: 35060
			public static LocString PREV = "Prev";

			// Token: 0x040088F5 RID: 35061
			public static LocString ADDED = "Added";

			// Token: 0x040088F6 RID: 35062
			public static LocString REMOVED = "Removed";

			// Token: 0x040088F7 RID: 35063
			public static LocString NET = "Net";

			// Token: 0x040088F8 RID: 35064
			public static LocString DUPLICANT_DETAILS_HEADER = "Duplicant Details:";

			// Token: 0x040088F9 RID: 35065
			public static LocString TIME_DETAILS_HEADER = "Total Time Details:";

			// Token: 0x040088FA RID: 35066
			public static LocString BASE_DETAILS_HEADER = "Base Details:";

			// Token: 0x040088FB RID: 35067
			public static LocString AVERAGE_TIME_DETAILS_HEADER = "Average Time Details:";

			// Token: 0x040088FC RID: 35068
			public static LocString MY_COLONY = "my colony";

			// Token: 0x040088FD RID: 35069
			public static LocString NONE = "None";

			// Token: 0x0200272D RID: 10029
			public class OXYGEN_CREATED
			{
				// Token: 0x0400ACEF RID: 44271
				public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN") + " Generation:";

				// Token: 0x0400ACF0 RID: 44272
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was produced by {1} over the course of the day";

				// Token: 0x0400ACF1 RID: 44273
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x0200272E RID: 10030
			public class CALORIES_CREATED
			{
				// Token: 0x0400ACF2 RID: 44274
				public static LocString NAME = "Calorie Generation:";

				// Token: 0x0400ACF3 RID: 44275
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was produced by {1} over the course of the day";

				// Token: 0x0400ACF4 RID: 44276
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x0200272F RID: 10031
			public class NUMBER_OF_DOMESTICATED_CRITTERS
			{
				// Token: 0x0400ACF5 RID: 44277
				public static LocString NAME = "Domesticated Critters:";

				// Token: 0x0400ACF6 RID: 44278
				public static LocString POSITIVE_TOOLTIP = "{0} domestic critters live in {1}";

				// Token: 0x0400ACF7 RID: 44279
				public static LocString NEGATIVE_TOOLTIP = "{0} domestic critters live in {1}";
			}

			// Token: 0x02002730 RID: 10032
			public class NUMBER_OF_WILD_CRITTERS
			{
				// Token: 0x0400ACF8 RID: 44280
				public static LocString NAME = "Wild Critters:";

				// Token: 0x0400ACF9 RID: 44281
				public static LocString POSITIVE_TOOLTIP = "{0} wild critters live in {1}";

				// Token: 0x0400ACFA RID: 44282
				public static LocString NEGATIVE_TOOLTIP = "{0} wild critters live in {1}";
			}

			// Token: 0x02002731 RID: 10033
			public class ROCKETS_IN_FLIGHT
			{
				// Token: 0x0400ACFB RID: 44283
				public static LocString NAME = "Rocket Missions Underway:";

				// Token: 0x0400ACFC RID: 44284
				public static LocString POSITIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";

				// Token: 0x0400ACFD RID: 44285
				public static LocString NEGATIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";
			}

			// Token: 0x02002732 RID: 10034
			public class STRESS_DELTA
			{
				// Token: 0x0400ACFE RID: 44286
				public static LocString NAME = UI.FormatAsLink("Stress", "STRESS") + " Change:";

				// Token: 0x0400ACFF RID: 44287
				public static LocString POSITIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " increased by a total of {0} for {1}";

				// Token: 0x0400AD00 RID: 44288
				public static LocString NEGATIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " decreased by a total of {0} for {1}";
			}

			// Token: 0x02002733 RID: 10035
			public class TRAVELTIMEWARNING
			{
				// Token: 0x0400AD01 RID: 44289
				public static LocString WARNING_TITLE = "Long Commutes";

				// Token: 0x0400AD02 RID: 44290
				public static LocString WARNING_MESSAGE = "My Duplicants are spending a significant amount of time traveling between their errands (> {0})";
			}

			// Token: 0x02002734 RID: 10036
			public class TRAVEL_TIME
			{
				// Token: 0x0400AD03 RID: 44291
				public static LocString NAME = "Travel Time:";

				// Token: 0x0400AD04 RID: 44292
				public static LocString POSITIVE_TOOLTIP = "On average, {1} spent {0} of their time traveling between tasks";
			}

			// Token: 0x02002735 RID: 10037
			public class WORK_TIME
			{
				// Token: 0x0400AD05 RID: 44293
				public static LocString NAME = "Work Time:";

				// Token: 0x0400AD06 RID: 44294
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent working";
			}

			// Token: 0x02002736 RID: 10038
			public class IDLE_TIME
			{
				// Token: 0x0400AD07 RID: 44295
				public static LocString NAME = "Idle Time:";

				// Token: 0x0400AD08 RID: 44296
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent idling";
			}

			// Token: 0x02002737 RID: 10039
			public class PERSONAL_TIME
			{
				// Token: 0x0400AD09 RID: 44297
				public static LocString NAME = "Personal Time:";

				// Token: 0x0400AD0A RID: 44298
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent tending to personal needs";
			}

			// Token: 0x02002738 RID: 10040
			public class ENERGY_USAGE
			{
				// Token: 0x0400AD0B RID: 44299
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Usage:";

				// Token: 0x0400AD0C RID: 44300
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was created by {1} over the course of the day";

				// Token: 0x0400AD0D RID: 44301
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002739 RID: 10041
			public class ENERGY_WASTED
			{
				// Token: 0x0400AD0E RID: 44302
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Wasted:";

				// Token: 0x0400AD0F RID: 44303
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was lost today due to battery runoff and overproduction in {1}";
			}

			// Token: 0x0200273A RID: 10042
			public class LEVEL_UP
			{
				// Token: 0x0400AD10 RID: 44304
				public static LocString NAME = "Skill Increases:";

				// Token: 0x0400AD11 RID: 44305
				public static LocString TOOLTIP = "Today {1} gained a total of {0} skill levels";
			}

			// Token: 0x0200273B RID: 10043
			public class TOILET_INCIDENT
			{
				// Token: 0x0400AD12 RID: 44306
				public static LocString NAME = "Restroom Accidents:";

				// Token: 0x0400AD13 RID: 44307
				public static LocString TOOLTIP = "{0} Duplicants couldn't quite reach the toilet in time today";
			}

			// Token: 0x0200273C RID: 10044
			public class DISEASE_ADDED
			{
				// Token: 0x0400AD14 RID: 44308
				public static LocString NAME = UI.FormatAsLink("Diseases", "DISEASE") + " Contracted:";

				// Token: 0x0400AD15 RID: 44309
				public static LocString POSITIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were contracted by {1}";

				// Token: 0x0400AD16 RID: 44310
				public static LocString NEGATIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were cured by {1}";
			}

			// Token: 0x0200273D RID: 10045
			public class CONTAMINATED_OXYGEN_FLATULENCE
			{
				// Token: 0x0400AD17 RID: 44311
				public static LocString NAME = UI.FormatAsLink("Flatulence", "CONTAMINATEDOXYGEN") + " Generation:";

				// Token: 0x0400AD18 RID: 44312
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400AD19 RID: 44313
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x0200273E RID: 10046
			public class CONTAMINATED_OXYGEN_TOILET
			{
				// Token: 0x0400AD1A RID: 44314
				public static LocString NAME = UI.FormatAsLink("Toilet Emissions: ", "CONTAMINATEDOXYGEN");

				// Token: 0x0400AD1B RID: 44315
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400AD1C RID: 44316
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x0200273F RID: 10047
			public class CONTAMINATED_OXYGEN_SUBLIMATION
			{
				// Token: 0x0400AD1D RID: 44317
				public static LocString NAME = UI.FormatAsLink("Sublimation", "CONTAMINATEDOXYGEN") + ":";

				// Token: 0x0400AD1E RID: 44318
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400AD1F RID: 44319
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002740 RID: 10048
			public class DISEASE_STATUS
			{
				// Token: 0x0400AD20 RID: 44320
				public static LocString NAME = "Disease Status:";

				// Token: 0x0400AD21 RID: 44321
				public static LocString TOOLTIP = "There are {0} covering {1}";
			}

			// Token: 0x02002741 RID: 10049
			public class CHORE_STATUS
			{
				// Token: 0x0400AD22 RID: 44322
				public static LocString NAME = "Errands:";

				// Token: 0x0400AD23 RID: 44323
				public static LocString POSITIVE_TOOLTIP = "{0} errands are queued for {1}";

				// Token: 0x0400AD24 RID: 44324
				public static LocString NEGATIVE_TOOLTIP = "{0} errands were completed over the course of the day by {1}";
			}

			// Token: 0x02002742 RID: 10050
			public class NOTES
			{
				// Token: 0x0400AD25 RID: 44325
				public static LocString NOTE_ENTRY_LINE_ITEM = "{0}\n{1}: {2}";

				// Token: 0x0400AD26 RID: 44326
				public static LocString BUTCHERED = "Butchered for {0}";

				// Token: 0x0400AD27 RID: 44327
				public static LocString BUTCHERED_CONTEXT = "Butchered";

				// Token: 0x0400AD28 RID: 44328
				public static LocString CRAFTED = "Crafted a {0}";

				// Token: 0x0400AD29 RID: 44329
				public static LocString CRAFTED_USED = "{0} used as ingredient";

				// Token: 0x0400AD2A RID: 44330
				public static LocString CRAFTED_CONTEXT = "Crafted";

				// Token: 0x0400AD2B RID: 44331
				public static LocString HARVESTED = "Harvested {0}";

				// Token: 0x0400AD2C RID: 44332
				public static LocString HARVESTED_CONTEXT = "Harvested";

				// Token: 0x0400AD2D RID: 44333
				public static LocString EATEN = "{0} eaten";

				// Token: 0x0400AD2E RID: 44334
				public static LocString ROTTED = "Rotten {0}";

				// Token: 0x0400AD2F RID: 44335
				public static LocString ROTTED_CONTEXT = "Rotted";

				// Token: 0x0400AD30 RID: 44336
				public static LocString GERMS = "On {0}";

				// Token: 0x0400AD31 RID: 44337
				public static LocString TIME_SPENT = "{0}";

				// Token: 0x0400AD32 RID: 44338
				public static LocString WORK_TIME = "{0}";

				// Token: 0x0400AD33 RID: 44339
				public static LocString PERSONAL_TIME = "{0}";

				// Token: 0x0400AD34 RID: 44340
				public static LocString FOODFIGHT_CONTEXT = "{0} ingested in food fight";
			}
		}

		// Token: 0x02001DD1 RID: 7633
		public static class SCHEDULEBLOCKTYPES
		{
			// Token: 0x02002743 RID: 10051
			public static class EAT
			{
				// Token: 0x0400AD35 RID: 44341
				public static LocString NAME = "Mealtime";

				// Token: 0x0400AD36 RID: 44342
				public static LocString DESCRIPTION = "EAT:\nDuring Mealtime Duplicants will head to their assigned mess halls and eat.";
			}

			// Token: 0x02002744 RID: 10052
			public static class SLEEP
			{
				// Token: 0x0400AD37 RID: 44343
				public static LocString NAME = "Sleep";

				// Token: 0x0400AD38 RID: 44344
				public static LocString DESCRIPTION = "SLEEP:\nWhen it's time to sleep, Duplicants will head to their assigned rooms and rest.";
			}

			// Token: 0x02002745 RID: 10053
			public static class WORK
			{
				// Token: 0x0400AD39 RID: 44345
				public static LocString NAME = "Work";

				// Token: 0x0400AD3A RID: 44346
				public static LocString DESCRIPTION = "WORK:\nDuring Work hours Duplicants will perform any pending errands in the colony.";
			}

			// Token: 0x02002746 RID: 10054
			public static class RECREATION
			{
				// Token: 0x0400AD3B RID: 44347
				public static LocString NAME = "Recreation";

				// Token: 0x0400AD3C RID: 44348
				public static LocString DESCRIPTION = "HAMMER TIME:\nDuring Hammer Time, Duplicants will relieve their " + UI.FormatAsLink("Stress", "STRESS") + " through dance. Please be aware that no matter how hard my Duplicants try, they will absolutely not be able to touch this.";
			}

			// Token: 0x02002747 RID: 10055
			public static class HYGIENE
			{
				// Token: 0x0400AD3D RID: 44349
				public static LocString NAME = "Hygiene";

				// Token: 0x0400AD3E RID: 44350
				public static LocString DESCRIPTION = "HYGIENE:\nDuring " + UI.FormatAsLink("Hygiene", "HYGIENE") + " hours Duplicants will head to their assigned washrooms to get cleaned up.";
			}
		}

		// Token: 0x02001DD2 RID: 7634
		public static class SCHEDULEGROUPS
		{
			// Token: 0x040088FE RID: 35070
			public static LocString TOOLTIP_FORMAT = "{0}\n\n{1}";

			// Token: 0x040088FF RID: 35071
			public static LocString MISSINGBLOCKS = "Warning: Scheduling Issues ({0})";

			// Token: 0x04008900 RID: 35072
			public static LocString NOTIME = "No {0} shifts allotted";

			// Token: 0x02002748 RID: 10056
			public static class HYGENE
			{
				// Token: 0x0400AD3F RID: 44351
				public static LocString NAME = "Bathtime";

				// Token: 0x0400AD40 RID: 44352
				public static LocString DESCRIPTION = "During Bathtime shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.\n\nOnce they're all caught up on personal hygiene, Duplicants will head back to work.";

				// Token: 0x0400AD41 RID: 44353
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Bathtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands."
				});
			}

			// Token: 0x02002749 RID: 10057
			public static class WORKTIME
			{
				// Token: 0x0400AD42 RID: 44354
				public static LocString NAME = "Work";

				// Token: 0x0400AD43 RID: 44355
				public static LocString DESCRIPTION = "During Work shifts my Duplicants must perform the errands I have placed for them throughout the colony.\n\nIt's important when scheduling to maintain a good work-life balance for my Duplicants to maintain their health and prevent Morale loss.";

				// Token: 0x0400AD44 RID: 44356
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Work",
					UI.PST_KEYWORD,
					" shifts my Duplicants must perform the errands I've placed for them throughout the colony."
				});
			}

			// Token: 0x0200274A RID: 10058
			public static class RECREATION
			{
				// Token: 0x0400AD45 RID: 44357
				public static LocString NAME = "Downtime";

				// Token: 0x0400AD46 RID: 44358
				public static LocString DESCRIPTION = "During Downtime my Duplicants they may do as they please.\n\nThis may include personal matters like bathroom visits or snacking, or they may choose to engage in leisure activities like socializing with friends.\n\nDowntime increases Duplicant Morale.";

				// Token: 0x0400AD47 RID: 44359
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants they may do as they please."
				});
			}

			// Token: 0x0200274B RID: 10059
			public static class SLEEP
			{
				// Token: 0x0400AD48 RID: 44360
				public static LocString NAME = "Bedtime";

				// Token: 0x0400AD49 RID: 44361
				public static LocString DESCRIPTION = "My Duplicants use Bedtime shifts to rest up after a hard day's work.\n\nScheduling too few bedtime shifts may prevent my Duplicants from regaining enough Stamina to make it through the following day.";

				// Token: 0x0400AD4A RID: 44362
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"My Duplicants use ",
					UI.PRE_KEYWORD,
					"Bedtime",
					UI.PST_KEYWORD,
					" shifts to rest up after a hard day's work."
				});
			}
		}

		// Token: 0x02001DD3 RID: 7635
		public class ELEMENTAL
		{
			// Token: 0x0200274C RID: 10060
			public class AGE
			{
				// Token: 0x0400AD4B RID: 44363
				public static LocString NAME = "Age: {0}";

				// Token: 0x0400AD4C RID: 44364
				public static LocString TOOLTIP = "The selected object is {0} cycles old";

				// Token: 0x0400AD4D RID: 44365
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x0400AD4E RID: 44366
				public static LocString UNKNOWN_TOOLTIP = "The age of the selected object is unknown";
			}

			// Token: 0x0200274D RID: 10061
			public class UPTIME
			{
				// Token: 0x0400AD4F RID: 44367
				public static LocString NAME = "Uptime:\n{0}{1}: {2}\n{0}{3}: {4}\n{0}{5}: {6}";

				// Token: 0x0400AD50 RID: 44368
				public static LocString THIS_CYCLE = "This Cycle";

				// Token: 0x0400AD51 RID: 44369
				public static LocString LAST_CYCLE = "Last Cycle";

				// Token: 0x0400AD52 RID: 44370
				public static LocString LAST_X_CYCLES = "Last {0} Cycles";
			}

			// Token: 0x0200274E RID: 10062
			public class PRIMARYELEMENT
			{
				// Token: 0x0400AD53 RID: 44371
				public static LocString NAME = "Primary Element: {0}";

				// Token: 0x0400AD54 RID: 44372
				public static LocString TOOLTIP = "The selected object is primarily composed of {0}";
			}

			// Token: 0x0200274F RID: 10063
			public class UNITS
			{
				// Token: 0x0400AD55 RID: 44373
				public static LocString NAME = "Stack Units: {0}";

				// Token: 0x0400AD56 RID: 44374
				public static LocString TOOLTIP = "This stack contains {0} units of {1}";
			}

			// Token: 0x02002750 RID: 10064
			public class MASS
			{
				// Token: 0x0400AD57 RID: 44375
				public static LocString NAME = "Mass: {0}";

				// Token: 0x0400AD58 RID: 44376
				public static LocString TOOLTIP = "The selected object has a mass of {0}";
			}

			// Token: 0x02002751 RID: 10065
			public class TEMPERATURE
			{
				// Token: 0x0400AD59 RID: 44377
				public static LocString NAME = "Temperature: {0}";

				// Token: 0x0400AD5A RID: 44378
				public static LocString TOOLTIP = "The selected object's current temperature is {0}";
			}

			// Token: 0x02002752 RID: 10066
			public class DISEASE
			{
				// Token: 0x0400AD5B RID: 44379
				public static LocString NAME = "Disease: {0}";

				// Token: 0x0400AD5C RID: 44380
				public static LocString TOOLTIP = "There are {0} on the selected object";
			}

			// Token: 0x02002753 RID: 10067
			public class SHC
			{
				// Token: 0x0400AD5D RID: 44381
				public static LocString NAME = "Specific Heat Capacity: {0}";

				// Token: 0x0400AD5E RID: 44382
				public static LocString TOOLTIP = "{SPECIFIC_HEAT_CAPACITY} is required to heat 1 g of the selected object by 1 {TEMPERATURE_UNIT}";
			}

			// Token: 0x02002754 RID: 10068
			public class THERMALCONDUCTIVITY
			{
				// Token: 0x0400AD5F RID: 44383
				public static LocString NAME = "Thermal Conductivity: {0}";

				// Token: 0x0400AD60 RID: 44384
				public static LocString TOOLTIP = "This object can conduct heat to other materials at a rate of {THERMAL_CONDUCTIVITY} W for each degree {TEMPERATURE_UNIT} difference\n\nBetween two objects, the rate of heat transfer will be determined by the object with the lowest Thermal Conductivity";

				// Token: 0x0200314B RID: 12619
				public class ADJECTIVES
				{
					// Token: 0x0400C5D3 RID: 50643
					public static LocString VALUE_WITH_ADJECTIVE = "{0} ({1})";

					// Token: 0x0400C5D4 RID: 50644
					public static LocString VERY_LOW_CONDUCTIVITY = "Highly Insulating";

					// Token: 0x0400C5D5 RID: 50645
					public static LocString LOW_CONDUCTIVITY = "Insulating";

					// Token: 0x0400C5D6 RID: 50646
					public static LocString MEDIUM_CONDUCTIVITY = "Conductive";

					// Token: 0x0400C5D7 RID: 50647
					public static LocString HIGH_CONDUCTIVITY = "Highly Conductive";

					// Token: 0x0400C5D8 RID: 50648
					public static LocString VERY_HIGH_CONDUCTIVITY = "Extremely Conductive";
				}
			}

			// Token: 0x02002755 RID: 10069
			public class CONDUCTIVITYBARRIER
			{
				// Token: 0x0400AD61 RID: 44385
				public static LocString NAME = "Insulation Thickness: {0}";

				// Token: 0x0400AD62 RID: 44386
				public static LocString TOOLTIP = "Thick insulation reduces an object's Thermal Conductivity";
			}

			// Token: 0x02002756 RID: 10070
			public class VAPOURIZATIONPOINT
			{
				// Token: 0x0400AD63 RID: 44387
				public static LocString NAME = "Vaporization Point: {0}";

				// Token: 0x0400AD64 RID: 44388
				public static LocString TOOLTIP = "The selected object will evaporate into a gas at {0}";
			}

			// Token: 0x02002757 RID: 10071
			public class MELTINGPOINT
			{
				// Token: 0x0400AD65 RID: 44389
				public static LocString NAME = "Melting Point: {0}";

				// Token: 0x0400AD66 RID: 44390
				public static LocString TOOLTIP = "The selected object will melt into a liquid at {0}";
			}

			// Token: 0x02002758 RID: 10072
			public class OVERHEATPOINT
			{
				// Token: 0x0400AD67 RID: 44391
				public static LocString NAME = "Overheat Modifier: {0}";

				// Token: 0x0400AD68 RID: 44392
				public static LocString TOOLTIP = "This building will overheat and take damage if its temperature reaches {0}\n\nBuilding with better building materials can increase overheat temperature";
			}

			// Token: 0x02002759 RID: 10073
			public class FREEZEPOINT
			{
				// Token: 0x0400AD69 RID: 44393
				public static LocString NAME = "Freeze Point: {0}";

				// Token: 0x0400AD6A RID: 44394
				public static LocString TOOLTIP = "The selected object will cool into a solid at {0}";
			}

			// Token: 0x0200275A RID: 10074
			public class DEWPOINT
			{
				// Token: 0x0400AD6B RID: 44395
				public static LocString NAME = "Condensation Point: {0}";

				// Token: 0x0400AD6C RID: 44396
				public static LocString TOOLTIP = "The selected object will condense into a liquid at {0}";
			}
		}

		// Token: 0x02001DD4 RID: 7636
		public class IMMIGRANTSCREEN
		{
			// Token: 0x04008901 RID: 35073
			public static LocString IMMIGRANTSCREENTITLE = "Select a Blueprint";

			// Token: 0x04008902 RID: 35074
			public static LocString PROCEEDBUTTON = "Print";

			// Token: 0x04008903 RID: 35075
			public static LocString CANCELBUTTON = "Cancel";

			// Token: 0x04008904 RID: 35076
			public static LocString REJECTALL = "Reject All";

			// Token: 0x04008905 RID: 35077
			public static LocString EMBARK = "EMBARK";

			// Token: 0x04008906 RID: 35078
			public static LocString SELECTDUPLICANTS = "Select {0} Duplicants";

			// Token: 0x04008907 RID: 35079
			public static LocString SELECTYOURCREW = "CHOOSE THREE DUPLICANTS TO BEGIN";

			// Token: 0x04008908 RID: 35080
			public static LocString SHUFFLE = "REROLL";

			// Token: 0x04008909 RID: 35081
			public static LocString SHUFFLETOOLTIP = "Reroll for a different Duplicant";

			// Token: 0x0400890A RID: 35082
			public static LocString BACK = "BACK";

			// Token: 0x0400890B RID: 35083
			public static LocString CONFIRMATIONTITLE = "Reject All Printables?";

			// Token: 0x0400890C RID: 35084
			public static LocString CONFIRMATIONBODY = "The Printing Pod will need time to recharge if I reject these Printables.";

			// Token: 0x0400890D RID: 35085
			public static LocString NAME_YOUR_COLONY = "NAME THE COLONY";

			// Token: 0x0400890E RID: 35086
			public static LocString CARE_PACKAGE_ELEMENT_QUANTITY = "{0} of {1}";

			// Token: 0x0400890F RID: 35087
			public static LocString CARE_PACKAGE_ELEMENT_COUNT = "{0} x {1}";

			// Token: 0x04008910 RID: 35088
			public static LocString CARE_PACKAGE_ELEMENT_COUNT_ONLY = "x {0}";

			// Token: 0x04008911 RID: 35089
			public static LocString CARE_PACKAGE_CURRENT_AMOUNT = "Available: {0}";

			// Token: 0x04008912 RID: 35090
			public static LocString DUPLICATE_COLONY_NAME = "A colony named \"{0}\" already exists";
		}

		// Token: 0x02001DD5 RID: 7637
		public class METERS
		{
			// Token: 0x0200275B RID: 10075
			public class HEALTH
			{
				// Token: 0x0400AD6D RID: 44397
				public static LocString TOOLTIP = "Health";
			}

			// Token: 0x0200275C RID: 10076
			public class BREATH
			{
				// Token: 0x0400AD6E RID: 44398
				public static LocString TOOLTIP = "Oxygen";
			}

			// Token: 0x0200275D RID: 10077
			public class FUEL
			{
				// Token: 0x0400AD6F RID: 44399
				public static LocString TOOLTIP = "Fuel";
			}

			// Token: 0x0200275E RID: 10078
			public class BATTERY
			{
				// Token: 0x0400AD70 RID: 44400
				public static LocString TOOLTIP = "Battery Charge";
			}
		}
	}
}
