using System;
using System.Collections.Generic;
using System.Text;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Operations;
using Data;
using Scenes;
using UI.Hud;
using UnityEngine;

namespace Characters.Abilities.Weapons.DavyJones
{
	// Token: 0x02000C1A RID: 3098
	[Serializable]
	public sealed class DavyJonesPassive : Ability, IDavyJonesCannonBallSave, IDavyJonesCannonBallCollection
	{
		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x000B8839 File Offset: 0x000B6A39
		public bool isEmpty
		{
			get
			{
				return this._magazine.Count == 0;
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x000B884C File Offset: 0x000B6A4C
		public CannonBallType? Top()
		{
			if (this._magazine.Count == 0)
			{
				return null;
			}
			return new CannonBallType?(this._magazine.Peek());
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x000B8880 File Offset: 0x000B6A80
		public void Push(CannonBallType cannon, int count)
		{
			bool flag = Convert.ToInt32(cannon) % 2 == 1;
			if (this._reloadCount == this._enhanceCycle - 1)
			{
				this._onBeforeEnhanceReload.Run(this._owner);
			}
			if (this._reloadCount >= this._enhanceCycle && !flag)
			{
				this._onEnhanceReload.Run(this._owner);
				for (int i = 0; i < count; i++)
				{
					this.Push(Convert.ToInt32(cannon) + 1);
				}
				this._reloadCount = 1;
				return;
			}
			for (int j = 0; j < count; j++)
			{
				this.Push(cannon);
			}
			this._reloadCount++;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x000B8929 File Offset: 0x000B6B29
		private void Push(CannonBallType cannon)
		{
			if (this._magazine.Count >= 4)
			{
				this._magazine.Dequeue();
			}
			this._magazine.Enqueue(cannon);
			this._emptyMagazineSign.SetActive(false);
			this.UpdateHUD();
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x000B8964 File Offset: 0x000B6B64
		private void Push(int index)
		{
			foreach (object obj in Enum.GetValues(typeof(CannonBallType)))
			{
				CannonBallType cannonBallType = (CannonBallType)obj;
				if (Convert.ToInt32(cannonBallType) == index)
				{
					this.Push(cannonBallType);
					break;
				}
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x000B89D8 File Offset: 0x000B6BD8
		public void Pop()
		{
			if (this._magazine.Count > 0)
			{
				this._magazine.Dequeue();
				int count = this._magazine.Count;
				if (count != 0)
				{
					if (count != 1)
					{
						this._onMagazineCount2or3.Run(this._owner);
					}
					else
					{
						this._onMagazineCount1.Run(this._owner);
					}
				}
				else
				{
					this._emptyMagazineSign.SetActive(true);
					this._onMagazineCount0.Run(this._owner);
				}
			}
			this.UpdateHUD();
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x000B8A60 File Offset: 0x000B6C60
		private void UpdateHUD()
		{
			DavyJonesHud davyJonesHud = Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud;
			davyJonesHud.HideAllCannonBall();
			int num = 0;
			foreach (CannonBallType type in this._magazine)
			{
				davyJonesHud.SetCannonBall(num, type);
				num++;
			}
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x000B8AD8 File Offset: 0x000B6CD8
		public override void Initialize()
		{
			base.Initialize();
			this._magazine = new Queue<CannonBallType>(4);
			GameData.Save instance = GameData.Save.instance;
			if (instance.currentWeapon.Equals(this._weapon.name))
			{
				return;
			}
			if (instance.nextWeapon.Equals(this._weapon.name))
			{
				return;
			}
			this.Push(CannonBallType.Normal, 2);
			for (int i = 0; i < this._weapon.currentSkills.Count; i++)
			{
				SkillInfo skillInfo = this._weapon.currentSkills[i];
				for (int j = 0; j < this._skillInfoCannonBallMaps.Length; j++)
				{
					if (this._skillInfoCannonBallMaps[j]._skillInfo.key == skillInfo.key)
					{
						this.Push(this._skillInfoCannonBallMaps[j]._cannonBallType);
						break;
					}
				}
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x000B8BB3 File Offset: 0x000B6DB3
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return new DavyJonesPassive.Instance(owner, this);
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x000B8BC4 File Offset: 0x000B6DC4
		public float MakeSaveData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this._magazine.Count);
			CannonBallType[] array = this._magazine.ToArray();
			for (int i = 0; i < 4; i++)
			{
				if (i < array.Length)
				{
					stringBuilder.Append(Convert.ToInt32(array[i]));
				}
				else
				{
					stringBuilder.Append(0);
				}
			}
			return (float)Convert.ToInt32(stringBuilder.ToString());
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x000B8C34 File Offset: 0x000B6E34
		public void Load(float data)
		{
			string message = Convert.ToString((int)data);
			Debug.Log(message);
		}

		// Token: 0x040030FA RID: 12538
		[SerializeField]
		private GameObject _emptyMagazineSign;

		// Token: 0x040030FB RID: 12539
		[SerializeField]
		private int _enhanceCycle = 4;

		// Token: 0x040030FC RID: 12540
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x040030FD RID: 12541
		[SerializeField]
		private Characters.Actions.Action[] _cannonBallActions;

		// Token: 0x040030FE RID: 12542
		[SerializeField]
		private DavyJonesPassive.SkillInfoCannonBallMap[] _skillInfoCannonBallMaps;

		// Token: 0x040030FF RID: 12543
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		[Header("강화 관련")]
		private CharacterOperation.Subcomponents _onBeforeEnhanceReload;

		// Token: 0x04003100 RID: 12544
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _onEnhanceReload;

		// Token: 0x04003101 RID: 12545
		[Header("탄창 관련")]
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onMagazineCount0;

		// Token: 0x04003102 RID: 12546
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onMagazineCount1;

		// Token: 0x04003103 RID: 12547
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onMagazineCount2or3;

		// Token: 0x04003104 RID: 12548
		private Character _owner;

		// Token: 0x04003105 RID: 12549
		private int _reloadCount;

		// Token: 0x04003106 RID: 12550
		private const int _maxCount = 4;

		// Token: 0x04003107 RID: 12551
		private Queue<CannonBallType> _magazine;

		// Token: 0x02000C1B RID: 3099
		public class Instance : AbilityInstance<DavyJonesPassive>
		{
			// Token: 0x17000D75 RID: 3445
			// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x000B8C5E File Offset: 0x000B6E5E
			public override Sprite icon
			{
				get
				{
					if (this.iconStacks != 0)
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x17000D76 RID: 3446
			// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x000B8C70 File Offset: 0x000B6E70
			public override int iconStacks
			{
				get
				{
					return this.ability._reloadCount - 1;
				}
			}

			// Token: 0x06003FA7 RID: 16295 RVA: 0x000B8C7F File Offset: 0x000B6E7F
			public Instance(Character owner, DavyJonesPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003FA8 RID: 16296 RVA: 0x000B8C89 File Offset: 0x000B6E89
			protected override void OnAttach()
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud.ShowHUD();
				this.ability.UpdateHUD();
				this.PushSwapBonusCannonBall();
			}

			// Token: 0x06003FA9 RID: 16297 RVA: 0x000B8CB5 File Offset: 0x000B6EB5
			protected override void OnDetach()
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.davyJonesHud.HideAll();
			}

			// Token: 0x06003FAA RID: 16298 RVA: 0x000B8CD0 File Offset: 0x000B6ED0
			private void PushSwapBonusCannonBall()
			{
				if (this.ability._magazine.Count <= 2)
				{
					this.ability.Push(CannonBallType.Normal, 2);
				}
			}
		}

		// Token: 0x02000C1C RID: 3100
		[Serializable]
		private struct SkillInfoCannonBallMap
		{
			// Token: 0x04003108 RID: 12552
			[SerializeField]
			internal CannonBallType _cannonBallType;

			// Token: 0x04003109 RID: 12553
			[SerializeField]
			internal SkillInfo _skillInfo;
		}
	}
}
