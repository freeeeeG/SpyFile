using System;
using UnityEngine;

// Token: 0x02000835 RID: 2101
public readonly struct JoyResponseOutfitTarget
{
	// Token: 0x06003D15 RID: 15637 RVA: 0x001532C5 File Offset: 0x001514C5
	public JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x001532CE File Offset: 0x001514CE
	public Option<string> ReadFacadeId()
	{
		return this.impl.ReadFacadeId();
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x001532DB File Offset: 0x001514DB
	public void WriteFacadeId(Option<string> facadeId)
	{
		this.impl.WriteFacadeId(facadeId);
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x001532E9 File Offset: 0x001514E9
	public string GetMinionName()
	{
		return this.impl.GetMinionName();
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x001532F6 File Offset: 0x001514F6
	public Personality GetPersonality()
	{
		return this.impl.GetPersonality();
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x00153303 File Offset: 0x00151503
	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x00153315 File Offset: 0x00151515
	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.PersonalityTarget(personality));
	}

	// Token: 0x040027DF RID: 10207
	private readonly JoyResponseOutfitTarget.Implementation impl;

	// Token: 0x02001601 RID: 5633
	public interface Implementation
	{
		// Token: 0x0600890F RID: 35087
		Option<string> ReadFacadeId();

		// Token: 0x06008910 RID: 35088
		void WriteFacadeId(Option<string> permitId);

		// Token: 0x06008911 RID: 35089
		string GetMinionName();

		// Token: 0x06008912 RID: 35090
		Personality GetPersonality();
	}

	// Token: 0x02001602 RID: 5634
	public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06008913 RID: 35091 RVA: 0x0031079C File Offset: 0x0030E99C
		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06008914 RID: 35092 RVA: 0x003107B1 File Offset: 0x0030E9B1
		public string GetMinionName()
		{
			return this.minionInstance.GetProperName();
		}

		// Token: 0x06008915 RID: 35093 RVA: 0x003107BE File Offset: 0x0030E9BE
		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		// Token: 0x06008916 RID: 35094 RVA: 0x003107DF File Offset: 0x0030E9DF
		public Option<string> ReadFacadeId()
		{
			return this.wearableAccessorizer.GetJoyResponseId();
		}

		// Token: 0x06008917 RID: 35095 RVA: 0x003107EC File Offset: 0x0030E9EC
		public void WriteFacadeId(Option<string> permitId)
		{
			this.wearableAccessorizer.SetJoyResponseId(permitId);
		}

		// Token: 0x04006A2E RID: 27182
		public readonly GameObject minionInstance;

		// Token: 0x04006A2F RID: 27183
		public readonly WearableAccessorizer wearableAccessorizer;
	}

	// Token: 0x02001603 RID: 5635
	public readonly struct PersonalityTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06008918 RID: 35096 RVA: 0x003107FA File Offset: 0x0030E9FA
		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		// Token: 0x06008919 RID: 35097 RVA: 0x00310803 File Offset: 0x0030EA03
		public string GetMinionName()
		{
			return this.personality.Name;
		}

		// Token: 0x0600891A RID: 35098 RVA: 0x00310810 File Offset: 0x0030EA10
		public Personality GetPersonality()
		{
			return this.personality;
		}

		// Token: 0x0600891B RID: 35099 RVA: 0x00310818 File Offset: 0x0030EA18
		public Option<string> ReadFacadeId()
		{
			return this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		// Token: 0x0600891C RID: 35100 RVA: 0x0031082B File Offset: 0x0030EA2B
		public void WriteFacadeId(Option<string> facadeId)
		{
			this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}

		// Token: 0x04006A30 RID: 27184
		public readonly Personality personality;
	}
}
