using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
public class Personality : Resource
{
	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x0600401A RID: 16410 RVA: 0x0016702F File Offset: 0x0016522F
	public string description
	{
		get
		{
			return this.GetDescription();
		}
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x00167038 File Offset: 0x00165238
	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true)
	{
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x00167070 File Offset: 0x00165270
	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true)
	{
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x001670A8 File Offset: 0x001652A8
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion) : base(name_string_key, name)
	{
		this.nameStringKey = name_string_key;
		this.genderStringKey = Gender;
		this.personalityType = PersonalityType;
		this.stresstrait = StressTrait;
		this.joyTrait = JoyTrait;
		this.stickerType = StickerType;
		this.congenitaltrait = CongenitalTrait;
		this.unformattedDescription = description;
		this.headShape = headShape;
		this.mouth = mouth;
		this.neck = neck;
		this.eyes = eyes;
		this.hair = hair;
		this.body = body;
		this.belt = belt;
		this.cuff = cuff;
		this.foot = foot;
		this.hand = hand;
		this.pelvis = pelvis;
		this.leg = leg;
		this.startingMinion = isStartingMinion;
		this.outfitIds = new Dictionary<ClothingOutfitUtility.OutfitType, string>();
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x00167184 File Offset: 0x00165384
	public string GetDescription()
	{
		this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
		return this.unformattedDescription;
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x001671A8 File Offset: 0x001653A8
	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		Personality.StartingAttribute item = new Personality.StartingAttribute(attribute, value);
		this.attributes.Add(item);
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x001671C9 File Offset: 0x001653C9
	public void AddTrait(Trait trait)
	{
		this.traits.Add(trait);
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x001671D7 File Offset: 0x001653D7
	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		Db.Get().Permits.ClothingOutfits.SetDuplicantPersonalityOutfit(this.Id, outfit, outfitType);
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x001671F5 File Offset: 0x001653F5
	public void Internal_SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		if (outfit.HasValue)
		{
			this.outfitIds[outfitType] = outfit.Unwrap();
			return;
		}
		this.outfitIds.Remove(outfitType);
	}

	// Token: 0x06004023 RID: 16419 RVA: 0x00167221 File Offset: 0x00165421
	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.outfitIds.ContainsKey(outfitType))
		{
			return this.outfitIds[outfitType];
		}
		return null;
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x00167240 File Offset: 0x00165440
	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(this.nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string str;
		if (this.nameStringKey == "MIMA")
		{
			str = "Mi-Ma";
		}
		else
		{
			str = this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower();
		}
		return Assets.GetSprite("dreamIcon_" + str);
	}

	// Token: 0x040029C5 RID: 10693
	public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();

	// Token: 0x040029C6 RID: 10694
	public List<Trait> traits = new List<Trait>();

	// Token: 0x040029C7 RID: 10695
	public int headShape;

	// Token: 0x040029C8 RID: 10696
	public int mouth;

	// Token: 0x040029C9 RID: 10697
	public int neck;

	// Token: 0x040029CA RID: 10698
	public int eyes;

	// Token: 0x040029CB RID: 10699
	public int hair;

	// Token: 0x040029CC RID: 10700
	public int body;

	// Token: 0x040029CD RID: 10701
	public int belt;

	// Token: 0x040029CE RID: 10702
	public int cuff;

	// Token: 0x040029CF RID: 10703
	public int foot;

	// Token: 0x040029D0 RID: 10704
	public int hand;

	// Token: 0x040029D1 RID: 10705
	public int pelvis;

	// Token: 0x040029D2 RID: 10706
	public int leg;

	// Token: 0x040029D3 RID: 10707
	public Dictionary<ClothingOutfitUtility.OutfitType, string> outfitIds;

	// Token: 0x040029D4 RID: 10708
	public string nameStringKey;

	// Token: 0x040029D5 RID: 10709
	public string genderStringKey;

	// Token: 0x040029D6 RID: 10710
	public string personalityType;

	// Token: 0x040029D7 RID: 10711
	public string stresstrait;

	// Token: 0x040029D8 RID: 10712
	public string joyTrait;

	// Token: 0x040029D9 RID: 10713
	public string stickerType;

	// Token: 0x040029DA RID: 10714
	public string congenitaltrait;

	// Token: 0x040029DB RID: 10715
	public string unformattedDescription;

	// Token: 0x040029DC RID: 10716
	public bool startingMinion;

	// Token: 0x020016DF RID: 5855
	public class StartingAttribute
	{
		// Token: 0x06008CE1 RID: 36065 RVA: 0x00319FD0 File Offset: 0x003181D0
		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}

		// Token: 0x04006D40 RID: 27968
		public Klei.AI.Attribute attribute;

		// Token: 0x04006D41 RID: 27969
		public int value;
	}
}
