using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters.Abilities;
using Characters.Actions;
using Characters.Controllers;
using Characters.Gear.Weapons.Gauges;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x0200082F RID: 2095
	[RequireComponent(typeof(AttackDamage))]
	public sealed class Weapon : Gear
	{
		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06002B54 RID: 11092 RVA: 0x00085458 File Offset: 0x00083658
		// (remove) Token: 0x06002B55 RID: 11093 RVA: 0x00085490 File Offset: 0x00083690
		public event Action<Characters.Actions.Action> onStartAction;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06002B56 RID: 11094 RVA: 0x000854C8 File Offset: 0x000836C8
		// (remove) Token: 0x06002B57 RID: 11095 RVA: 0x00085500 File Offset: 0x00083700
		public event Action<Characters.Actions.Action> onEndAction;

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Weapon;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000147BD File Offset: 0x000129BD
		public override GameData.Currency.Type currencyTypeByDiscard
		{
			get
			{
				return GameData.Currency.Type.Bone;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002B5A RID: 11098 RVA: 0x00085535 File Offset: 0x00083735
		public override int currencyByDiscard
		{
			get
			{
				if (base.dropped.price <= 0 && this.destructible)
				{
					return Settings.instance.bonesByDiscard[base.rarity];
				}
				return 0;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x00085564 File Offset: 0x00083764
		protected override string _prefix
		{
			get
			{
				return "weapon";
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x0008556B File Offset: 0x0008376B
		public Weapon.Category category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x00085573 File Offset: 0x00083773
		public float customWidth
		{
			get
			{
				return this._customWidth;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x0008557B File Offset: 0x0008377B
		public Gauge gauge
		{
			get
			{
				return this._gauge;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x00085583 File Offset: 0x00083783
		public string categoryDisplayName
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					"label",
					this._prefix,
					"Category",
					this._category
				}));
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x000855C1 File Offset: 0x000837C1
		public string activeName
		{
			get
			{
				return Localization.GetLocalizedString(base._keyBase + "/active/name");
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000855D8 File Offset: 0x000837D8
		public string activeDescription
		{
			get
			{
				return Localization.GetLocalizedString(base._keyBase + "/active/desc");
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000855EF File Offset: 0x000837EF
		public BoxCollider2D hitbox
		{
			get
			{
				return this._hitbox;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x000855F7 File Offset: 0x000837F7
		// (set) Token: 0x06002B64 RID: 11108 RVA: 0x000855FF File Offset: 0x000837FF
		public SkillInfo[] skills { get; private set; }

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x00085608 File Offset: 0x00083808
		// (set) Token: 0x06002B66 RID: 11110 RVA: 0x00085610 File Offset: 0x00083810
		public List<SkillInfo> currentSkills { get; private set; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x00085619 File Offset: 0x00083819
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x00085621 File Offset: 0x00083821
		public CharacterAnimation characterAnimation { get; private set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x0008562A File Offset: 0x0008382A
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x00085632 File Offset: 0x00083832
		public Sprite mainIcon { get; private set; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0008563B File Offset: 0x0008383B
		// (set) Token: 0x06002B6C RID: 11116 RVA: 0x00085643 File Offset: 0x00083843
		public Sprite subIcon { get; private set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x0008564C File Offset: 0x0008384C
		public bool upgradable
		{
			get
			{
				return !string.IsNullOrEmpty(this.nextLevelReference.name);
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x00085661 File Offset: 0x00083861
		protected override void Awake()
		{
			base.Awake();
			Singleton<Service>.Instance.gearManager.RegisterWeaponInstance(this);
			this.InitializeSkills();
			this.AttachMinimapAgent();
			this.GetChildActions();
			this.characterAnimation = base.equipped.GetComponent<CharacterAnimation>();
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x0008569C File Offset: 0x0008389C
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.UnregisterWeaponInstance(this);
			this._abilityAttacher.StopAttach();
			this._passiveAbilityAttacher.StopAttach();
			Action<Gear> onDiscard = this._onDiscard;
			if (onDiscard != null)
			{
				onDiscard(this);
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (levelManager.player == null || !levelManager.player.liveAndActive)
			{
				return;
			}
			if (this.destructible)
			{
				GameData.Progress.encounterWeaponCount++;
				PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, base.transform.position);
				Collider2D component = base.dropped.GetComponent<Collider2D>();
				if (component == null)
				{
					Weapon.Assets.destroyWeapon.Spawn(base.transform.position, 0f, 1f);
				}
				else
				{
					Weapon.Assets.destroyWeapon.Spawn(component.bounds.center, 0f, 1f);
				}
			}
			if (this.currencyByDiscard == 0)
			{
				return;
			}
			int count = 1;
			if (this.currencyByDiscard > 0)
			{
				switch (base.rarity)
				{
				case Rarity.Common:
					count = 4;
					break;
				case Rarity.Rare:
					count = 7;
					break;
				case Rarity.Unique:
					count = 13;
					break;
				case Rarity.Legendary:
					count = 20;
					break;
				}
			}
			levelManager.DropBone(this.currencyByDiscard, count);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000857F4 File Offset: 0x000839F4
		private void GetChildActions()
		{
			new List<Characters.Actions.Action>();
			new List<Characters.Actions.Action>();
			new List<Characters.Actions.Action>();
			new List<Characters.Actions.Action>();
			Characters.Actions.Action[] componentsInChildren = base.GetComponentsInChildren<Characters.Actions.Action>(true);
			EnumArray<Characters.Actions.Action.Type, List<Characters.Actions.Action>> enumArray = new EnumArray<Characters.Actions.Action.Type, List<Characters.Actions.Action>>();
			for (int i = 0; i < enumArray.Keys.Count; i++)
			{
				enumArray.Array[i] = new List<Characters.Actions.Action>();
			}
			Characters.Actions.Action[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				Characters.Actions.Action action = array[j];
				action.onStart += delegate()
				{
					Action<Characters.Actions.Action> action = this.onStartAction;
					if (action == null)
					{
						return;
					}
					action(action);
				};
				action.onEnd += delegate()
				{
					Action<Characters.Actions.Action> action = this.onEndAction;
					if (action == null)
					{
						return;
					}
					action(action);
				};
				enumArray[action.type].Add(action);
			}
			for (int k = 0; k < enumArray.Keys.Count; k++)
			{
				this.actionsByType.Array[k] = enumArray.Array[k].ToArray();
			}
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x00085900 File Offset: 0x00083B00
		private void AttachMinimapAgent()
		{
			GameObject gameObject = new GameObject("MinimapAgent");
			gameObject.transform.parent = base.equipped.transform;
			SpriteRenderer component = base.equipped.GetComponent<SpriteRenderer>();
			Bounds bounds = this._hitbox.bounds;
			bounds.Expand(0.33f);
			MinimapAgentGenerator.Generate(gameObject, bounds, Color.yellow, component);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0008595E File Offset: 0x00083B5E
		private void InitializeSkills()
		{
			this.skills = (from skill in base.GetComponentsInChildren<SkillInfo>(true)
			where skill.gameObject.activeSelf
			select skill).ToArray<SkillInfo>();
			this.RandomizeSkills();
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x0008599C File Offset: 0x00083B9C
		private void RandomizeSkills()
		{
			this.SetCurrentSkills();
			this.SetSkillButtons();
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000859AC File Offset: 0x00083BAC
		public void ApplyAllSkillChanges()
		{
			for (int i = 0; i < this._skillChangeMap.originals.Count; i++)
			{
				int num = this.currentSkills.IndexOf(this._skillChangeMap.originals[i]);
				if (num != -1)
				{
					this.ChangeSkill(num, this._skillChangeMap.news[i], true);
				}
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x00085A10 File Offset: 0x00083C10
		public void UnapplyAllSkillChanges()
		{
			for (int i = 0; i < this._skillChangeMap.originals.Count; i++)
			{
				int num = this.currentSkills.IndexOf(this._skillChangeMap.news[i]);
				if (num != -1)
				{
					this.ChangeSkill(num, this._skillChangeMap.originals[i], true);
				}
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x00085A74 File Offset: 0x00083C74
		public SkillInfo GetSkillWithoutSkillChanges(int index)
		{
			SkillInfo skillInfo = this.currentSkills[index];
			int num = this._skillChangeMap.news.IndexOf(skillInfo);
			if (num == -1)
			{
				return skillInfo;
			}
			return this._skillChangeMap.originals[num];
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x00085AB8 File Offset: 0x00083CB8
		public void RerollSkills()
		{
			if (this.currentSkills.Count == this.skills.Length)
			{
				return;
			}
			this.UnapplyAllSkillChanges();
			ILookup<bool, SkillInfo> lookup = this.skills.ToLookup((SkillInfo info) => info.hasAlways);
			SkillInfo[] array = lookup[true].ToArray<SkillInfo>();
			List<SkillInfo> excepts = new List<SkillInfo>(from info in this.currentSkills
			where !info.hasAlways
			select info);
			if (excepts.Count > 1)
			{
				excepts.RemoveAt(excepts.RandomIndex<SkillInfo>());
			}
			List<SkillInfo> from = (from info in lookup[false]
			where info.weight > 0 && !excepts.Contains(info)
			select info).ToList<SkillInfo>();
			int i;
			for (i = 0; i < array.Length; i++)
			{
				this.currentSkills[i] = array[i];
			}
			for (int j = i; j < this._skillSlots; j++)
			{
				SkillInfo value = SkillInfo.WeightedRandomPop(from);
				this.currentSkills[j] = value;
			}
			this.ApplyAllSkillChanges();
			this.SetSkillButtons();
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x00085BEC File Offset: 0x00083DEC
		public void SetSkills(string[] skillKeys, bool ignoreLevel = true)
		{
			ILookup<bool, SkillInfo> lookup = this.skills.ToLookup((SkillInfo info) => info.hasAlways);
			SkillInfo[] array = lookup[true].ToArray<SkillInfo>();
			List<SkillInfo> list = lookup[false].ToList<SkillInfo>();
			List<string> list2 = (from skill in list
			select skill.key).ToList<string>();
			if (ignoreLevel)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					string text = list2[i];
					int num = text.IndexOf('_');
					if (num >= 0)
					{
						list2[i] = text.Substring(0, num);
					}
				}
				for (int j = 0; j < skillKeys.Length; j++)
				{
					string text2 = skillKeys[j];
					int num2 = text2.IndexOf('_');
					if (num2 >= 0)
					{
						skillKeys[j] = text2.Substring(0, num2);
					}
				}
			}
			int k;
			for (k = 0; k < array.Length; k++)
			{
				this.currentSkills[k] = array[k];
			}
			foreach (string value in skillKeys)
			{
				int num3 = 0;
				while (num3 < list2.Count && k < this._skillSlots)
				{
					if (list2[num3].Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						this.currentSkills[k] = list[num3];
						list.RemoveAt(num3);
						list2.RemoveAt(num3);
						k++;
						break;
					}
					num3++;
				}
			}
			for (int m = k; m < this._skillSlots; m++)
			{
				SkillInfo value2 = SkillInfo.WeightedRandomPop(list);
				this.currentSkills[m] = value2;
			}
			this.SetSkillButtons();
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x00085DA8 File Offset: 0x00083FA8
		private void SetCurrentSkills()
		{
			if (this.skills.Length < this._skillSlots)
			{
				Debug.LogError("Skill count is less than skill slots of the weapon.");
				return;
			}
			SkillInfo[] skills = this.skills;
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i].Initialize();
			}
			ILookup<bool, SkillInfo> lookup = this.skills.ToLookup((SkillInfo info) => info.hasAlways);
			SkillInfo[] array = lookup[true].ToArray<SkillInfo>();
			List<SkillInfo> list = (from info in lookup[false]
			where info.weight > 0
			select info).ToList<SkillInfo>();
			if (array.Length + list.Count == this._skillSlots)
			{
				this.currentSkills = new List<SkillInfo>(array);
				this.currentSkills.AddRange(list);
				return;
			}
			this.currentSkills = new List<SkillInfo>(new SkillInfo[this._skillSlots]);
			int j;
			for (j = 0; j < array.Length; j++)
			{
				this.currentSkills[j] = array[j];
			}
			for (int k = j; k < this._skillSlots; k++)
			{
				SkillInfo value = SkillInfo.WeightedRandomPop(list);
				this.currentSkills[k] = value;
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x00085EE4 File Offset: 0x000840E4
		public void SetSkillButtons()
		{
			SkillInfo[] skills = this.skills;
			for (int i = 0; i < skills.Length; i++)
			{
				skills[i].action.button = Button.None;
			}
			this.<SetSkillButtons>g__SetActionButtonAt|75_0(0, Button.Skill);
			this.<SetSkillButtons>g__SetActionButtonAt|75_0(1, Button.Skill2);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x00085F30 File Offset: 0x00084130
		public override void Initialize()
		{
			base.Initialize();
			this.mainIcon = GearResource.instance.GetWeaponHudMainIcon(base.name);
			this.subIcon = GearResource.instance.GetWeaponHudSubIcon(base.name);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x00085F64 File Offset: 0x00084164
		public void StartUse()
		{
			base.owner.stat.AttachValues(base.stat);
			this._abilityAttacher.StartAttach();
			this._passiveAbilityAttacher.StartAttach();
			foreach (SkillInfo skillInfo in this.currentSkills)
			{
				skillInfo.gameObject.SetActive(true);
			}
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x00085FE8 File Offset: 0x000841E8
		public void EndUse()
		{
			base.owner.stat.DetachValues(base.stat);
			this._abilityAttacher.StopAttach();
			foreach (SkillInfo skillInfo in this.currentSkills)
			{
				skillInfo.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x00086060 File Offset: 0x00084260
		public void StartSwitchAction()
		{
			Characters.Actions.Action[] array = this.actionsByType[Characters.Actions.Action.Type.Swap];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TryStart())
				{
					return;
				}
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x00086093 File Offset: 0x00084293
		protected override void OnLoot(Character character)
		{
			this.SetOwner(character);
			base.state = Gear.State.Equipped;
			character.playerComponents.inventory.weapon.Equip(this);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000860BA File Offset: 0x000842BA
		public void SetOwner(Character character)
		{
			base.owner = character;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000860C3 File Offset: 0x000842C3
		protected override void OnEquipped()
		{
			base.OnEquipped();
			this._abilityAttacher.Initialize(base.owner);
			this._passiveAbilityAttacher.Initialize(base.owner);
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000860ED File Offset: 0x000842ED
		protected override void OnDropped()
		{
			base.OnDropped();
			this._passiveAbilityAttacher.StopAttach();
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x00086100 File Offset: 0x00084300
		public Weapon Instantiate()
		{
			Weapon weapon = UnityEngine.Object.Instantiate<Weapon>(this);
			weapon.name = base.name;
			weapon.Initialize();
			return weapon;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x0008611C File Offset: 0x0008431C
		public void RemoveSkill(string key)
		{
			for (int i = 0; i < this.currentSkills.Count; i++)
			{
				if (key.Equals(this.currentSkills[i].key, StringComparison.InvariantCultureIgnoreCase))
				{
					this.RemoveSkill(i);
					return;
				}
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x00086161 File Offset: 0x00084361
		public void RemoveSkill(int index)
		{
			this.currentSkills[index].gameObject.SetActive(false);
			this.currentSkills.RemoveAt(index);
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x00086186 File Offset: 0x00084386
		public void SwapSkillOrder()
		{
			if (this.currentSkills.Count < 2)
			{
				return;
			}
			this.currentSkills.Swap(0, 1);
			this.SetSkillButtons();
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000861AC File Offset: 0x000843AC
		public void AttachSkillChange(SkillInfo original, SkillInfo @new, bool copyCooldown = false)
		{
			this._skillChangeMap.Add(original, @new);
			int targetSkillIndex = this.currentSkills.IndexOf(original);
			this.ChangeSkill(targetSkillIndex, @new, copyCooldown);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000861DC File Offset: 0x000843DC
		public void AttachSkillChanges(SkillInfo[] originals, SkillInfo[] news, bool copyCooldown = false)
		{
			for (int i = 0; i < originals.Length; i++)
			{
				this._skillChangeMap.Add(originals[i], news[i]);
				int num = this.currentSkills.IndexOf(originals[i]);
				if (num != -1)
				{
					this.ChangeSkill(num, news[i], copyCooldown);
				}
			}
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00086228 File Offset: 0x00084428
		public void DetachSkillChange(SkillInfo original, SkillInfo @new, bool copyCooldown = false)
		{
			if (!this._skillChangeMap.Remove(original, @new))
			{
				return;
			}
			int targetSkillIndex = this.currentSkills.IndexOf(@new);
			this.ChangeSkill(targetSkillIndex, original, copyCooldown);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x0008625C File Offset: 0x0008445C
		public void DetachSkillChanges(SkillInfo[] originals, SkillInfo[] news, bool copyCooldown = false)
		{
			for (int i = 0; i < originals.Length; i++)
			{
				if (this._skillChangeMap.Remove(originals[i], news[i]))
				{
					int num = this.currentSkills.IndexOf(news[i]);
					if (num != -1)
					{
						this.ChangeSkill(num, originals[i], copyCooldown);
					}
				}
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000862A8 File Offset: 0x000844A8
		private void ChangeSkill(int targetSkillIndex, SkillInfo newSkill, bool copyCooldown = false)
		{
			if (targetSkillIndex < 0 || targetSkillIndex >= this.currentSkills.Count)
			{
				return;
			}
			SkillInfo skillInfo = this.currentSkills[targetSkillIndex];
			this.currentSkills[targetSkillIndex] = newSkill;
			Button button = skillInfo.action.button;
			skillInfo.action.button = newSkill.action.button;
			newSkill.action.button = button;
			if (!copyCooldown)
			{
				return;
			}
			newSkill.action.cooldown.CopyCooldown(skillInfo.action.cooldown);
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00086330 File Offset: 0x00084530
		public void ChangeAction(Characters.Actions.Action targetAction, Characters.Actions.Action newAction)
		{
			UnityEngine.Object component = targetAction.GetComponent<SkillInfo>();
			SkillInfo component2 = newAction.GetComponent<SkillInfo>();
			if (component != null && component2 != null)
			{
				Debug.LogError("Please use ChangeSkill for action that has skill info.");
				return;
			}
			Button button = targetAction.button;
			targetAction.button = newAction.button;
			newAction.button = button;
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000863A8 File Offset: 0x000845A8
		[CompilerGenerated]
		private void <SetSkillButtons>g__SetActionButtonAt|75_0(int index, Button button)
		{
			if (index >= this.currentSkills.Count)
			{
				return;
			}
			Characters.Actions.Action component = this.currentSkills[index].GetComponent<Characters.Actions.Action>();
			if (component == null)
			{
				return;
			}
			component.button = button;
		}

		// Token: 0x040024DB RID: 9435
		public readonly EnumArray<Characters.Actions.Action.Type, Characters.Actions.Action[]> actionsByType = new EnumArray<Characters.Actions.Action.Type, Characters.Actions.Action[]>();

		// Token: 0x040024DC RID: 9436
		[SerializeField]
		private BoxCollider2D _hitbox;

		// Token: 0x040024DD RID: 9437
		[SerializeField]
		private Weapon.Category _category;

		// Token: 0x040024DE RID: 9438
		[Range(0f, 2f)]
		[SerializeField]
		private int _skillSlots = 1;

		// Token: 0x040024DF RID: 9439
		[Information("0이면 콜라이더 크기 따라감", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _customWidth;

		// Token: 0x040024E0 RID: 9440
		[SerializeField]
		private Gauge _gauge;

		// Token: 0x040024E1 RID: 9441
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher.Subcomponents _abilityAttacher;

		// Token: 0x040024E2 RID: 9442
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher.Subcomponents _passiveAbilityAttacher;

		// Token: 0x040024E5 RID: 9445
		private readonly Weapon.SkillChangeMap _skillChangeMap = new Weapon.SkillChangeMap();

		// Token: 0x040024E9 RID: 9449
		public WeaponReference nextLevelReference;

		// Token: 0x02000830 RID: 2096
		private class Assets
		{
			// Token: 0x040024EA RID: 9450
			internal static EffectInfo destroyWeapon = new EffectInfo(CommonResource.instance.destroyWeapon);
		}

		// Token: 0x02000831 RID: 2097
		public enum Category
		{
			// Token: 0x040024EC RID: 9452
			Balance,
			// Token: 0x040024ED RID: 9453
			Power,
			// Token: 0x040024EE RID: 9454
			Speed,
			// Token: 0x040024EF RID: 9455
			Ranged
		}

		// Token: 0x02000832 RID: 2098
		public class SkillChangeMap
		{
			// Token: 0x06002B91 RID: 11153 RVA: 0x000863FD File Offset: 0x000845FD
			public void Add(SkillInfo original, SkillInfo @new)
			{
				this.originals.Add(original);
				this.news.Add(@new);
			}

			// Token: 0x06002B92 RID: 11154 RVA: 0x00086418 File Offset: 0x00084618
			public bool Remove(SkillInfo original, SkillInfo @new)
			{
				for (int i = 0; i < this.originals.Count; i++)
				{
					if (this.originals[i] == original && this.news[i] == @new)
					{
						this.originals.RemoveAt(i);
						this.news.RemoveAt(i);
						return true;
					}
				}
				return false;
			}

			// Token: 0x040024F0 RID: 9456
			public readonly List<SkillInfo> originals = new List<SkillInfo>();

			// Token: 0x040024F1 RID: 9457
			public readonly List<SkillInfo> news = new List<SkillInfo>();
		}
	}
}
