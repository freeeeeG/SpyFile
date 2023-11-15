using System;
using System.Collections;
using System.Linq;
using Characters.Controllers;
using Characters.Gear;
using Characters.Gear.Weapons;
using Data;
using FX;
using FX.SpriteEffects;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007FE RID: 2046
	public sealed class WeaponInventory : MonoBehaviour, IAttackDamage
	{
		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060029A8 RID: 10664 RVA: 0x0007F604 File Offset: 0x0007D804
		// (remove) Token: 0x060029A9 RID: 10665 RVA: 0x0007F63C File Offset: 0x0007D83C
		public event Action onSwap;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x060029AA RID: 10666 RVA: 0x0007F674 File Offset: 0x0007D874
		// (remove) Token: 0x060029AB RID: 10667 RVA: 0x0007F6AC File Offset: 0x0007D8AC
		public event WeaponInventory.OnChangeDelegate onChanged;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060029AC RID: 10668 RVA: 0x0007F6E4 File Offset: 0x0007D8E4
		// (remove) Token: 0x060029AD RID: 10669 RVA: 0x0007F71C File Offset: 0x0007D91C
		public event Action onSwapReady;

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x0007F751 File Offset: 0x0007D951
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x0007F759 File Offset: 0x0007D959
		public int currentIndex { get; private set; } = -1;

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x0007F762 File Offset: 0x0007D962
		// (set) Token: 0x060029B1 RID: 10673 RVA: 0x0007F76A File Offset: 0x0007D96A
		public Weapon current { get; private set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0007F773 File Offset: 0x0007D973
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x0007F77B File Offset: 0x0007D97B
		public Weapon polymorphWeapon { get; private set; }

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0007F784 File Offset: 0x0007D984
		public Weapon polymorphOrCurrent
		{
			get
			{
				if (!(this.polymorphWeapon == null))
				{
					return this.polymorphWeapon;
				}
				return this.current;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x0007F7A4 File Offset: 0x0007D9A4
		public Weapon next
		{
			get
			{
				int num = this.currentIndex;
				for (;;)
				{
					num++;
					if (num == this.weapons.Length)
					{
						num = 0;
					}
					if (num == this.currentIndex)
					{
						break;
					}
					if (!(this.weapons[num] == null))
					{
						goto Block_3;
					}
				}
				return null;
				Block_3:
				return this.weapons[num];
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0007F7EC File Offset: 0x0007D9EC
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x0007F7F4 File Offset: 0x0007D9F4
		public IAttackDamage weaponAttackDamage { get; private set; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x0007F7FD File Offset: 0x0007D9FD
		public float amount
		{
			get
			{
				return this.weaponAttackDamage.amount;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x0007F80A File Offset: 0x0007DA0A
		public float reaminCooldownPercent
		{
			get
			{
				return this._remainCooldown / 8f;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x0007F818 File Offset: 0x0007DA18
		// (set) Token: 0x060029BB RID: 10683 RVA: 0x0007F820 File Offset: 0x0007DA20
		public bool swapReady { get; private set; } = true;

		// Token: 0x060029BC RID: 10684 RVA: 0x0007F829 File Offset: 0x0007DA29
		private void Awake()
		{
			this.EquipDefault();
			this._switchEffect = new EffectInfo(CommonResource.instance.swapEffect)
			{
				chronometer = this._character.chronometer.effect
			};
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x0007F85C File Offset: 0x0007DA5C
		private void EquipDefault()
		{
			int num = (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Castle) ? 0 : GameData.Generic.skinIndex;
			this._default = this._defaultSkins[num].Instantiate();
			this.ForceEquip(this._default);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x00033492 File Offset: 0x00031692
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x0007F8A8 File Offset: 0x0007DAA8
		private void Update()
		{
			if (this._remainCooldown > 0f)
			{
				this.swapReady = false;
				this._remainCooldown -= this._character.chronometer.master.deltaTime * this._character.stat.GetSwapCooldownSpeed();
				return;
			}
			if (!this.swapReady)
			{
				Action action = this.onSwapReady;
				if (action != null)
				{
					action();
				}
				this.swapReady = true;
			}
			this._remainCooldown = 0f;
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x0007F928 File Offset: 0x0007DB28
		private void StartUse(Weapon weapon)
		{
			weapon.gameObject.SetActive(true);
			this._character.CancelAction();
			this._character.InitializeActions();
			this._character.animationController.Initialize();
			this._character.animationController.ForceUpdate();
			this._character.collider.size = weapon.hitbox.size;
			this._character.collider.offset = weapon.hitbox.offset;
			this.weaponAttackDamage = weapon.GetComponent<IAttackDamage>();
			weapon.StartUse();
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x0007F9BF File Offset: 0x0007DBBF
		private void EndUse(Weapon weapon)
		{
			weapon.EndUse();
			this._character.stat.DetachValues(weapon.stat);
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x0007F9E0 File Offset: 0x0007DBE0
		public void UpdateSkin()
		{
			for (int i = 0; i < this.weapons.Length; i++)
			{
				Weapon weapon = this.weapons[i];
				if (!(weapon == null))
				{
					bool flag = false;
					foreach (Weapon weapon2 in this._defaultSkins)
					{
						if (weapon.name.Equals(weapon2.name, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this._default = this._defaultSkins[GameData.Generic.skinIndex].Instantiate();
						this.ForceEquipAt(this._default, i);
					}
				}
			}
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x0007FA78 File Offset: 0x0007DC78
		public bool NextWeapon(bool force = false)
		{
			if (!force && (this._remainCooldown > 0f || this._character.stunedOrFreezed))
			{
				return false;
			}
			for (int i = 1; i < this.weapons.Length; i++)
			{
				int num = (this.currentIndex + i) % this.weapons.Length;
				if (this.weapons[num] != null)
				{
					this.ChangeWeaponWithSwitchAction(num);
					this._remainCooldown = 8f;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x0007FAF0 File Offset: 0x0007DCF0
		private void ChangeWeapon(int index)
		{
			this.Unpolymorph();
			this.polymorphWeapon = null;
			this.current.gameObject.SetActive(false);
			this.current.EndUse();
			this.currentIndex = index;
			this.current = this.weapons[this.currentIndex];
			this.StartUse(this.current);
			Action action = this.onSwap;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x0007FB5C File Offset: 0x0007DD5C
		private void ChangeWeaponWithSwitchAction(int index)
		{
			Effects.SpritePoolObject spritePoolObject = Effects.sprite.Spawn();
			SpriteRenderer spriteRenderer = spritePoolObject.spriteRenderer;
			SpriteRenderer spriteRenderer2 = this.current.characterAnimation.spriteRenderer;
			spriteRenderer.sprite = spriteRenderer2.sprite;
			spriteRenderer.transform.localScale = spriteRenderer2.transform.lossyScale;
			spriteRenderer.transform.SetPositionAndRotation(spriteRenderer2.transform.position, spriteRenderer2.transform.rotation);
			spriteRenderer.flipX = spriteRenderer2.flipX;
			spriteRenderer.flipY = spriteRenderer2.flipY;
			spriteRenderer.sortingLayerID = spriteRenderer2.sortingLayerID;
			spriteRenderer.sortingOrder = spriteRenderer2.sortingOrder - 1;
			spriteRenderer.color = new Color(0.8352941f, 0.18039216f, 1f);
			spriteRenderer.sharedMaterial = MaterialResource.color;
			spritePoolObject.FadeOut(this._character.chronometer.effect, AnimationCurve.Linear(0f, 0f, 1f, 1f), 0.5f);
			this.ChangeWeapon(index);
			this.current.StartSwitchAction();
			this._switchEffect.Spawn(base.transform.position, 0f, 1f);
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0007FC90 File Offset: 0x0007DE90
		public void LoseAll()
		{
			this.Unpolymorph();
			this.current.EndUse();
			this.ChangeWeapon(0);
			for (int i = 1; i < this.weapons.Length; i++)
			{
				Weapon weapon = this.weapons[i];
				if (!(weapon == null))
				{
					this.Drop(weapon);
					WeaponInventory.OnChangeDelegate onChangeDelegate = this.onChanged;
					if (onChangeDelegate != null)
					{
						onChangeDelegate(weapon, null);
					}
					UnityEngine.Object.Destroy(weapon.gameObject);
				}
			}
			this._character.InitializeActions();
			this._character.animationController.Initialize();
			this._character.animationController.ForceUpdate();
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x0007FD2C File Offset: 0x0007DF2C
		public void Unequip(Weapon weapon)
		{
			int num = -1;
			for (int i = 0; i < this.weapons.Length; i++)
			{
				if (weapon == this.weapons[i])
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			this.Drop(weapon);
			this._character.InitializeActions();
			this._character.animationController.Initialize();
			this._character.animationController.ForceUpdate();
			this.weapons[num] = null;
			WeaponInventory.OnChangeDelegate onChangeDelegate = this.onChanged;
			if (onChangeDelegate == null)
			{
				return;
			}
			onChangeDelegate(weapon, null);
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x0007FDB4 File Offset: 0x0007DFB4
		private void Drop(Weapon weapon)
		{
			this.EndUse(weapon);
			weapon.state = Gear.State.Dropped;
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x0007FDC4 File Offset: 0x0007DFC4
		public Weapon Equip(Weapon weapon)
		{
			int index = this.currentIndex;
			for (int i = 0; i < this.weapons.Length; i++)
			{
				if (this.weapons[i] == null)
				{
					index = i;
					break;
				}
			}
			this._character.spriteEffectStack.Add(new EasedColorOverlay(int.MaxValue, Color.white, new Color(1f, 1f, 1f, 0f), new Curve(AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0.15f)));
			return this.EquipAt(weapon, index);
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x0007FE68 File Offset: 0x0007E068
		public Weapon EquipAt(Weapon weapon, int index)
		{
			this.Unpolymorph();
			for (int i = 0; i < this.weapons.Length; i++)
			{
				if (index != i && this.weapons[i] != null)
				{
					this.weapons[i].gameObject.SetActive(false);
					this.weapons[i].EndUse();
				}
			}
			Weapon weapon2 = this.weapons[index];
			if (weapon2 != null)
			{
				this.Drop(weapon2);
			}
			this.current = weapon;
			this.current.transform.parent = this._character.@base;
			this.current.transform.localPosition = Vector3.zero;
			this.current.transform.localScale = Vector3.one;
			this.StartUse(this.current);
			this.weapons[index] = weapon;
			this.currentIndex = index;
			WeaponInventory.OnChangeDelegate onChangeDelegate = this.onChanged;
			if (onChangeDelegate != null)
			{
				onChangeDelegate(weapon2, weapon);
			}
			return weapon2;
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x0007FF58 File Offset: 0x0007E158
		public void ForceEquip(Weapon weapon)
		{
			weapon.state = Gear.State.Equipped;
			int index = this.currentIndex;
			for (int i = 0; i < this.weapons.Length; i++)
			{
				if (this.weapons[i] == null)
				{
					index = i;
					break;
				}
			}
			this.ForceEquipAt(weapon, index);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x0007FFA4 File Offset: 0x0007E1A4
		public void ForceEquipAt(Weapon weapon, int index)
		{
			weapon.SetOwner(this._character);
			weapon.gameObject.SetActive(true);
			weapon.state = Gear.State.Equipped;
			Weapon weapon2 = this.weapons[index];
			this.EquipAt(weapon, index);
			if (weapon2 != null)
			{
				UnityEngine.Object.Destroy(weapon2.gameObject);
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x0007FFF8 File Offset: 0x0007E1F8
		public void Polymorph(Weapon target)
		{
			if (target == null)
			{
				Debug.Log("[WeaponInventory] Polymorph target is null.");
				return;
			}
			this.Unpolymorph();
			this.polymorphWeapon = target;
			this.current.gameObject.SetActive(false);
			this.EndUse(this.current);
			this.polymorphWeapon.SetOwner(this._character);
			this.polymorphWeapon.gameObject.SetActive(true);
			this.polymorphWeapon.state = Gear.State.Equipped;
			this.polymorphWeapon.transform.parent = this._character.@base;
			this.polymorphWeapon.transform.localPosition = Vector3.zero;
			this.polymorphWeapon.transform.localScale = Vector3.one;
			this.StartUse(this.polymorphWeapon);
			WeaponInventory.OnChangeDelegate onChangeDelegate = this.onChanged;
			if (onChangeDelegate == null)
			{
				return;
			}
			onChangeDelegate(this.current, this.polymorphWeapon);
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000800E0 File Offset: 0x0007E2E0
		public void Unpolymorph()
		{
			if (this.polymorphWeapon == null)
			{
				return;
			}
			Weapon polymorphWeapon = this.polymorphWeapon;
			this.polymorphWeapon = null;
			polymorphWeapon.gameObject.SetActive(false);
			this.EndUse(polymorphWeapon);
			polymorphWeapon.transform.parent = null;
			this.StartUse(this.current);
			WeaponInventory.OnChangeDelegate onChangeDelegate = this.onChanged;
			if (onChangeDelegate == null)
			{
				return;
			}
			onChangeDelegate(polymorphWeapon, this.current);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0008014C File Offset: 0x0007E34C
		public void ReduceSwapCooldown(float second)
		{
			this._remainCooldown -= second;
			if (this._remainCooldown < 0f)
			{
				this._remainCooldown = 0f;
			}
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x00080174 File Offset: 0x0007E374
		public void SetSwapCooldown(float second)
		{
			this._remainCooldown = second;
			if (this._remainCooldown < 0f)
			{
				this._remainCooldown = 0f;
			}
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x00080195 File Offset: 0x0007E395
		public void ResetSwapCooldown()
		{
			this._remainCooldown = 0f;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000801A4 File Offset: 0x0007E3A4
		public int GetCountByCategory(Weapon.Category category)
		{
			int num = 0;
			foreach (Weapon weapon in this.weapons)
			{
				if (!(weapon == null) && weapon.category == category)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000801E4 File Offset: 0x0007E3E4
		public int GetCountByRarity(Rarity rarity)
		{
			int num = 0;
			foreach (Weapon weapon in this.weapons)
			{
				if (!(weapon == null) && weapon.rarity == rarity)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x00080223 File Offset: 0x0007E423
		public void UpgradeCurrentWeapon()
		{
			if (this.current.rarity == Rarity.Legendary || this.current.name.Equals("Skul", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Log("각성할 수 없는 헤드입니다");
				return;
			}
			base.StartCoroutine(this.CUpgradeCurrentWeapon());
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00080263 File Offset: 0x0007E463
		public IEnumerator CUpgradeCurrentWeapon()
		{
			this.current.UnapplyAllSkillChanges();
			string[] skillKeys = (from skill in this.current.currentSkills
			select skill.key).ToArray<string>();
			yield return this.CWaitForUpgrade(skillKeys);
			yield break;
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x00080272 File Offset: 0x0007E472
		private IEnumerator CWaitForUpgrade(string[] skillKeys)
		{
			WeaponRequest request = this.current.nextLevelReference.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			Weapon weapon = Singleton<Service>.Instance.levelManager.DropWeapon(request, base.transform.position);
			this.current.destructible = false;
			weapon.SetSkills(skillKeys, true);
			this.ForceEquipAt(weapon, this.currentIndex);
			yield break;
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x00080288 File Offset: 0x0007E488
		public bool Has(string weaponKey)
		{
			for (int i = 0; i < this.weapons.Length; i++)
			{
				if (!(this.weapons[i] == null) && this.weapons[i].name.Equals(weaponKey, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040023B8 RID: 9144
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x040023B9 RID: 9145
		[SerializeField]
		[GetComponent]
		private PlayerInput _input;

		// Token: 0x040023BA RID: 9146
		[SerializeField]
		private Weapon[] _defaultSkins;

		// Token: 0x040023BB RID: 9147
		private const float _swapCooldown = 8f;

		// Token: 0x040023BD RID: 9149
		private float _remainCooldown;

		// Token: 0x040023BE RID: 9150
		public readonly Weapon[] weapons = new Weapon[2];

		// Token: 0x040023BF RID: 9151
		private EffectInfo _switchEffect;

		// Token: 0x040023C4 RID: 9156
		private Weapon _default;

		// Token: 0x020007FF RID: 2047
		// (Invoke) Token: 0x060029DA RID: 10714
		public delegate void OnChangeDelegate(Weapon old, Weapon @new);
	}
}
