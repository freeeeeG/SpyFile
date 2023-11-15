using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D27 RID: 3367
	public class TechItems : ResourceSet<TechItem>
	{
		// Token: 0x06006A23 RID: 27171 RVA: 0x00295499 File Offset: 0x00293699
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x002954A8 File Offset: 0x002936A8
		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), DlcManager.AVAILABLE_ALL_VERSIONS);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.AVAILABLE_EXPANSION1_ONLY);
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x00295710 File Offset: 0x00293910
		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x00295729 File Offset: 0x00293929
		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x00295744 File Offset: 0x00293944
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds)
		{
			if (!DlcManager.IsDlcListValidForCurrentContent(DLCIds))
			{
				return null;
			}
			if (base.TryGet(id) != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Tried adding a tech item called",
					id,
					name,
					"but it was already added!"
				});
				return base.Get(id);
			}
			Tech techFromItemID = this.GetTechFromItemID(id);
			if (techFromItemID == null)
			{
				return null;
			}
			TechItem techItem = new TechItem(id, this, name, description, getUISprite, techFromItemID.Id, DLCIds);
			base.Add(techItem);
			techFromItemID.unlockedItems.Add(techItem);
			return techItem;
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x002957C8 File Offset: 0x002939C8
		public bool IsTechItemComplete(string id)
		{
			bool result = true;
			foreach (TechItem techItem in this.resources)
			{
				if (techItem.Id == id)
				{
					result = techItem.IsComplete();
					break;
				}
			}
			return result;
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x00295830 File Offset: 0x00293A30
		private Tech GetTechFromItemID(string itemId)
		{
			if (Db.Get().Techs == null)
			{
				return null;
			}
			return Db.Get().Techs.TryGetTechForTechItem(itemId);
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x00295850 File Offset: 0x00293A50
		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		// Token: 0x04004D43 RID: 19779
		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		// Token: 0x04004D44 RID: 19780
		public TechItem automationOverlay;

		// Token: 0x04004D45 RID: 19781
		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		// Token: 0x04004D46 RID: 19782
		public TechItem suitsOverlay;

		// Token: 0x04004D47 RID: 19783
		public const string JET_SUIT_ID = "JetSuit";

		// Token: 0x04004D48 RID: 19784
		public TechItem jetSuit;

		// Token: 0x04004D49 RID: 19785
		public const string ATMO_SUIT_ID = "AtmoSuit";

		// Token: 0x04004D4A RID: 19786
		public TechItem atmoSuit;

		// Token: 0x04004D4B RID: 19787
		public const string OXYGEN_MASK_ID = "OxygenMask";

		// Token: 0x04004D4C RID: 19788
		public TechItem oxygenMask;

		// Token: 0x04004D4D RID: 19789
		public const string LEAD_SUIT_ID = "LeadSuit";

		// Token: 0x04004D4E RID: 19790
		public TechItem leadSuit;

		// Token: 0x04004D4F RID: 19791
		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		// Token: 0x04004D50 RID: 19792
		public TechItem betaResearchPoint;

		// Token: 0x04004D51 RID: 19793
		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		// Token: 0x04004D52 RID: 19794
		public TechItem gammaResearchPoint;

		// Token: 0x04004D53 RID: 19795
		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		// Token: 0x04004D54 RID: 19796
		public TechItem deltaResearchPoint;

		// Token: 0x04004D55 RID: 19797
		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		// Token: 0x04004D56 RID: 19798
		public TechItem orbitalResearchPoint;

		// Token: 0x04004D57 RID: 19799
		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		// Token: 0x04004D58 RID: 19800
		public TechItem conveyorOverlay;
	}
}
