using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004D7 RID: 1239
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000973BC File Offset: 0x000955BC
	// (set) Token: 0x06001C4B RID: 7243 RVA: 0x000973C4 File Offset: 0x000955C4
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06001C4C RID: 7244 RVA: 0x000973CD File Offset: 0x000955CD
	// (set) Token: 0x06001C4D RID: 7245 RVA: 0x000973D5 File Offset: 0x000955D5
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06001C4E RID: 7246 RVA: 0x000973DE File Offset: 0x000955DE
	// (set) Token: 0x06001C4F RID: 7247 RVA: 0x000973E6 File Offset: 0x000955E6
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06001C50 RID: 7248 RVA: 0x000973EF File Offset: 0x000955EF
	public static void DestroyStatics()
	{
		MinionIdentity.maleNameList = null;
		MinionIdentity.femaleNameList = null;
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x00097400 File Offset: 0x00095600
	protected override void OnPrefabInit()
	{
		if (this.name == null)
		{
			this.name = MinionIdentity.ChooseRandomName();
		}
		if (GameClock.Instance != null)
		{
			this.arrivalTime = (float)GameClock.Instance.GetCycle();
		}
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			KAnimControllerBase kanimControllerBase = component;
			kanimControllerBase.OnUpdateBounds = (Action<Bounds>)Delegate.Combine(kanimControllerBase.OnUpdateBounds, new Action<Bounds>(this.OnUpdateBounds));
		}
		GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x0009747C File Offset: 0x0009567C
	protected override void OnSpawn()
	{
		if (this.addToIdentityList)
		{
			this.ValidateProxy();
			this.CleanupLimboMinions();
		}
		PathProber component = base.GetComponent<PathProber>();
		if (component != null)
		{
			component.SetGroupProber(MinionGroupProber.Get());
		}
		this.SetName(this.name);
		if (this.nameStringKey == null)
		{
			this.nameStringKey = this.name;
		}
		this.SetGender(this.gender);
		if (this.genderStringKey == null)
		{
			this.genderStringKey = "NB";
		}
		if (this.personalityResourceId == HashedString.Invalid)
		{
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey != null)
			{
				this.personalityResourceId = personalityFromNameStringKey.Id;
			}
		}
		if (this.addToIdentityList)
		{
			Components.MinionIdentities.Add(this);
			if (!base.gameObject.HasTag(GameTags.Dead))
			{
				Components.LiveMinionIdentities.Add(this);
				Game.Instance.Trigger(2144209314, this);
			}
		}
		SymbolOverrideController component2 = base.GetComponent<SymbolOverrideController>();
		if (component2 != null)
		{
			Accessorizer component3 = base.gameObject.GetComponent<Accessorizer>();
			if (component3 != null)
			{
				string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.HeadShape).symbol.hash).Replace("headshape", "cheek");
				component2.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(str), 1);
				component2.AddSymbolOverride("snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
				component2.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			}
		}
		this.voiceId = (this.voiceIdx + 1).ToString("D2");
		Prioritizable component4 = base.GetComponent<Prioritizable>();
		if (component4 != null)
		{
			component4.showIcon = false;
		}
		Pickupable component5 = base.GetComponent<Pickupable>();
		if (component5 != null)
		{
			component5.carryAnimOverride = Assets.GetAnim("anim_incapacitated_carrier_kanim");
		}
		this.ApplyCustomGameSettings();
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x0009770E File Offset: 0x0009590E
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x00097724 File Offset: 0x00095924
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with an invalid kpid! Attempting to recover...",
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x0009785D File Offset: 0x00095A5D
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x0009786A File Offset: 0x00095A6A
	public string GetVoiceId()
	{
		return this.voiceId;
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x00097872 File Offset: 0x00095A72
	public void SetName(string name)
	{
		this.name = name;
		if (this.selectable != null)
		{
			this.selectable.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000978B1 File Offset: 0x00095AB1
	public void SetStickerType(string stickerType)
	{
		this.stickerType = stickerType;
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000978BA File Offset: 0x00095ABA
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000978C3 File Offset: 0x00095AC3
	public void SetGender(string gender)
	{
		this.gender = gender;
		this.selectable.SetGender(gender);
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x000978D8 File Offset: 0x00095AD8
	public static string ChooseRandomName()
	{
		if (MinionIdentity.femaleNameList == null)
		{
			MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
			MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			return MinionIdentity.maleNameList.Next();
		}
		return MinionIdentity.femaleNameList.Next();
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00097938 File Offset: 0x00095B38
	protected override void OnCleanUp()
	{
		if (this.assignableProxy != null)
		{
			MinionAssignablesProxy minionAssignablesProxy = this.assignableProxy.Get();
			if (minionAssignablesProxy && minionAssignablesProxy.target == this)
			{
				Util.KDestroyGameObject(minionAssignablesProxy.gameObject);
			}
		}
		Components.MinionIdentities.Remove(this);
		Components.LiveMinionIdentities.Remove(this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x0009799B File Offset: 0x00095B9B
	private void OnUpdateBounds(Bounds bounds)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		component.offset = bounds.center;
		component.size = bounds.extents;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000979C8 File Offset: 0x00095BC8
	private void OnDied(object data)
	{
		this.GetSoleOwner().UnassignAll();
		this.GetEquipment().UnequipAll();
		Components.LiveMinionIdentities.Remove(this);
		Game.Instance.Trigger(-1523247426, this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00097A16 File Offset: 0x00095C16
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x00097A28 File Offset: 0x00095C28
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x00097A3A File Offset: 0x00095C3A
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x00097A4D File Offset: 0x00095C4D
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00097A5A File Offset: 0x00095C5A
	public Equipment GetEquipment()
	{
		return this.assignableProxy.Get().GetComponent<Equipment>();
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x00097A6C File Offset: 0x00095C6C
	public void Sim1000ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.navigator == null)
		{
			this.navigator = base.GetComponent<Navigator>();
		}
		if (this.navigator != null && !this.navigator.IsMoving())
		{
			return;
		}
		if (this.choreDriver == null)
		{
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}
		if (this.choreDriver != null)
		{
			Chore currentChore = this.choreDriver.GetCurrentChore();
			if (currentChore != null && currentChore is FetchAreaChore)
			{
				MinionResume component = base.GetComponent<MinionResume>();
				if (component != null)
				{
					component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
				}
			}
		}
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x00097B28 File Offset: 0x00095D28
	private void ApplyCustomGameSettings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
		if (currentQualitySetting.id == "Compromised")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Weak")
		{
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Strong")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Invincible")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 100000000f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
		if (currentQualitySetting2.id == "Doomed")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Pessimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Optimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Indomitable")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting3.id == "VeryHard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -1666.6666f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Hard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -833.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Easy")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 833.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Disabled")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
	}

	// Token: 0x04000F9D RID: 3997
	public const string HairAlwaysSymbol = "snapto_hair_always";

	// Token: 0x04000F9E RID: 3998
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04000F9F RID: 3999
	public int femaleVoiceCount;

	// Token: 0x04000FA0 RID: 4000
	public int maleVoiceCount;

	// Token: 0x04000FA1 RID: 4001
	[Serialize]
	private new string name;

	// Token: 0x04000FA2 RID: 4002
	[Serialize]
	public string gender;

	// Token: 0x04000FA6 RID: 4006
	[Serialize]
	public string stickerType;

	// Token: 0x04000FA7 RID: 4007
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x04000FA8 RID: 4008
	[Serialize]
	public int voiceIdx;

	// Token: 0x04000FA9 RID: 4009
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x04000FAA RID: 4010
	private Navigator navigator;

	// Token: 0x04000FAB RID: 4011
	private ChoreDriver choreDriver;

	// Token: 0x04000FAC RID: 4012
	public float timeLastSpoke;

	// Token: 0x04000FAD RID: 4013
	private string voiceId;

	// Token: 0x04000FAE RID: 4014
	private KAnimHashedString overrideExpression;

	// Token: 0x04000FAF RID: 4015
	private KAnimHashedString expression;

	// Token: 0x04000FB0 RID: 4016
	public bool addToIdentityList = true;

	// Token: 0x04000FB1 RID: 4017
	private static MinionIdentity.NameList maleNameList;

	// Token: 0x04000FB2 RID: 4018
	private static MinionIdentity.NameList femaleNameList;

	// Token: 0x04000FB3 RID: 4019
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, delegate(MinionIdentity component, object data)
	{
		component.OnDied(data);
	});

	// Token: 0x0200117C RID: 4476
	private class NameList
	{
		// Token: 0x060079C5 RID: 31173 RVA: 0x002DA1CC File Offset: 0x002D83CC
		public NameList(TextAsset file)
		{
			string[] array = file.text.Replace("  ", " ").Replace("\r\n", "\n").Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					' '
				});
				if (array2[array2.Length - 1] != "" && array2[array2.Length - 1] != null)
				{
					this.names.Add(array2[array2.Length - 1]);
				}
			}
			this.names.Shuffle<string>();
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x002DA27C File Offset: 0x002D847C
		public string Next()
		{
			List<string> list = this.names;
			int num = this.idx;
			this.idx = num + 1;
			return list[num % this.names.Count];
		}

		// Token: 0x04005C7A RID: 23674
		private List<string> names = new List<string>();

		// Token: 0x04005C7B RID: 23675
		private int idx;
	}
}
