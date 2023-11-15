using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D12 RID: 3346
	public class Personalities : ResourceSet<Personality>
	{
		// Token: 0x060069DE RID: 27102 RVA: 0x00291144 File Offset: 0x0028F344
		public Personalities()
		{
			foreach (Personalities.PersonalityInfo personalityInfo in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
			{
				Personality resource = new Personality(personalityInfo.Name.ToUpper(), Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", personalityInfo.Name.ToUpper())), personalityInfo.Gender.ToUpper(), personalityInfo.PersonalityType, personalityInfo.StressTrait, personalityInfo.JoyTrait, personalityInfo.StickerType, personalityInfo.CongenitalTrait, personalityInfo.HeadShape, personalityInfo.Mouth, personalityInfo.Neck, personalityInfo.Eyes, personalityInfo.Hair, personalityInfo.Body, personalityInfo.Belt, personalityInfo.Cuff, personalityInfo.Foot, personalityInfo.Hand, personalityInfo.Pelvis, personalityInfo.Leg, Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", personalityInfo.Name.ToUpper())), personalityInfo.ValidStarter);
				base.Add(resource);
			}
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x00291248 File Offset: 0x0028F448
		private void AddTrait(Personality personality, string trait_name)
		{
			Trait trait = Db.Get().traits.TryGet(trait_name);
			if (trait != null)
			{
				personality.AddTrait(trait);
			}
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x00291270 File Offset: 0x0028F470
		private void SetAttribute(Personality personality, string attribute_name, int value)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
			if (attribute == null)
			{
				Debug.LogWarning("Attribute does not exist: " + attribute_name);
				return;
			}
			personality.SetAttribute(attribute, value);
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x002912AA File Offset: 0x0028F4AA
		public List<Personality> GetStartingPersonalities()
		{
			return this.resources.FindAll((Personality x) => x.startingMinion);
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x002912D8 File Offset: 0x0028F4D8
		public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.resources.FindAll((Personality x) => (!onlyStartingMinions || x.startingMinion) && (!onlyEnabledMinions || !x.Disabled));
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x00291310 File Offset: 0x0028F510
		public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x00291320 File Offset: 0x0028F520
		public Personality GetPersonalityFromNameStringKey(string name_string_key)
		{
			foreach (Personality personality in Db.Get().Personalities.resources)
			{
				if (personality.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
				{
					return personality;
				}
			}
			return null;
		}

		// Token: 0x02001C34 RID: 7220
		public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
		{
			// Token: 0x06009CB0 RID: 40112 RVA: 0x0034EBB7 File Offset: 0x0034CDB7
			public PersonalityLoader() : base(Assets.instance.personalitiesFile)
			{
			}

			// Token: 0x06009CB1 RID: 40113 RVA: 0x0034EBC9 File Offset: 0x0034CDC9
			public override void Run()
			{
				base.Run();
			}
		}

		// Token: 0x02001C35 RID: 7221
		public class PersonalityInfo : Resource
		{
			// Token: 0x0400800A RID: 32778
			public int HeadShape;

			// Token: 0x0400800B RID: 32779
			public int Mouth;

			// Token: 0x0400800C RID: 32780
			public int Neck;

			// Token: 0x0400800D RID: 32781
			public int Eyes;

			// Token: 0x0400800E RID: 32782
			public int Hair;

			// Token: 0x0400800F RID: 32783
			public int Body;

			// Token: 0x04008010 RID: 32784
			public int Belt;

			// Token: 0x04008011 RID: 32785
			public int Cuff;

			// Token: 0x04008012 RID: 32786
			public int Foot;

			// Token: 0x04008013 RID: 32787
			public int Hand;

			// Token: 0x04008014 RID: 32788
			public int Pelvis;

			// Token: 0x04008015 RID: 32789
			public int Leg;

			// Token: 0x04008016 RID: 32790
			public string Gender;

			// Token: 0x04008017 RID: 32791
			public string PersonalityType;

			// Token: 0x04008018 RID: 32792
			public string StressTrait;

			// Token: 0x04008019 RID: 32793
			public string JoyTrait;

			// Token: 0x0400801A RID: 32794
			public string StickerType;

			// Token: 0x0400801B RID: 32795
			public string CongenitalTrait;

			// Token: 0x0400801C RID: 32796
			public string Design;

			// Token: 0x0400801D RID: 32797
			public bool ValidStarter;
		}
	}
}
