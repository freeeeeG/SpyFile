using System;
using Characters.Abilities.Statuses;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006CB RID: 1739
	public class CharacterStatus : MonoBehaviour
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x060022EF RID: 8943 RVA: 0x00069620 File Offset: 0x00067820
		// (remove) Token: 0x060022F0 RID: 8944 RVA: 0x00069658 File Offset: 0x00067858
		public event CharacterStatus.OnTimeDelegate onApplyBleed;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x060022F1 RID: 8945 RVA: 0x00069690 File Offset: 0x00067890
		// (remove) Token: 0x060022F2 RID: 8946 RVA: 0x000696C8 File Offset: 0x000678C8
		public event CharacterStatus.OnTimeDelegate onApplyWound;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x060022F3 RID: 8947 RVA: 0x00069700 File Offset: 0x00067900
		// (remove) Token: 0x060022F4 RID: 8948 RVA: 0x00069738 File Offset: 0x00067938
		public event CharacterStatus.OnTimeDelegate onApplyFreeze;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x060022F5 RID: 8949 RVA: 0x00069770 File Offset: 0x00067970
		// (remove) Token: 0x060022F6 RID: 8950 RVA: 0x000697A8 File Offset: 0x000679A8
		public event CharacterStatus.OnTimeDelegate onRefreshFreeze;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x060022F7 RID: 8951 RVA: 0x000697E0 File Offset: 0x000679E0
		// (remove) Token: 0x060022F8 RID: 8952 RVA: 0x00069818 File Offset: 0x00067A18
		public event CharacterStatus.OnTimeDelegate onReleaseFreeze;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x060022F9 RID: 8953 RVA: 0x00069850 File Offset: 0x00067A50
		// (remove) Token: 0x060022FA RID: 8954 RVA: 0x00069888 File Offset: 0x00067A88
		public event CharacterStatus.OnTimeDelegate onApplyBurn;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060022FB RID: 8955 RVA: 0x000698C0 File Offset: 0x00067AC0
		// (remove) Token: 0x060022FC RID: 8956 RVA: 0x000698F8 File Offset: 0x00067AF8
		public event CharacterStatus.OnTimeDelegate onRefreshBurn;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060022FD RID: 8957 RVA: 0x00069930 File Offset: 0x00067B30
		// (remove) Token: 0x060022FE RID: 8958 RVA: 0x00069968 File Offset: 0x00067B68
		public event CharacterStatus.OnTimeDelegate onReleaseBurn;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060022FF RID: 8959 RVA: 0x000699A0 File Offset: 0x00067BA0
		// (remove) Token: 0x06002300 RID: 8960 RVA: 0x000699D8 File Offset: 0x00067BD8
		public event CharacterStatus.OnTimeDelegate onGaveBurnDamage;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06002301 RID: 8961 RVA: 0x00069A10 File Offset: 0x00067C10
		// (remove) Token: 0x06002302 RID: 8962 RVA: 0x00069A48 File Offset: 0x00067C48
		public event CharacterStatus.OnTimeDelegate onGaveEmberDamage;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06002303 RID: 8963 RVA: 0x00069A80 File Offset: 0x00067C80
		// (remove) Token: 0x06002304 RID: 8964 RVA: 0x00069AB8 File Offset: 0x00067CB8
		public event CharacterStatus.OnTimeDelegate onApplyPoison;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002305 RID: 8965 RVA: 0x00069AF0 File Offset: 0x00067CF0
		// (remove) Token: 0x06002306 RID: 8966 RVA: 0x00069B28 File Offset: 0x00067D28
		public event CharacterStatus.OnTimeDelegate onRefreshPoison;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06002307 RID: 8967 RVA: 0x00069B60 File Offset: 0x00067D60
		// (remove) Token: 0x06002308 RID: 8968 RVA: 0x00069B98 File Offset: 0x00067D98
		public event CharacterStatus.OnTimeDelegate onReleasePoison;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002309 RID: 8969 RVA: 0x00069BD0 File Offset: 0x00067DD0
		// (remove) Token: 0x0600230A RID: 8970 RVA: 0x00069C08 File Offset: 0x00067E08
		public event CharacterStatus.OnTimeDelegate onApplyStun;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x0600230B RID: 8971 RVA: 0x00069C40 File Offset: 0x00067E40
		// (remove) Token: 0x0600230C RID: 8972 RVA: 0x00069C78 File Offset: 0x00067E78
		public event CharacterStatus.OnTimeDelegate onRefreshStun;

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600230D RID: 8973 RVA: 0x00069CB0 File Offset: 0x00067EB0
		// (remove) Token: 0x0600230E RID: 8974 RVA: 0x00069CE8 File Offset: 0x00067EE8
		public event CharacterStatus.OnTimeDelegate onReleaseStun;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x0600230F RID: 8975 RVA: 0x00069D20 File Offset: 0x00067F20
		// (remove) Token: 0x06002310 RID: 8976 RVA: 0x00069D58 File Offset: 0x00067F58
		public event CharacterStatus.OnApplyDelegate onApply;

		// Token: 0x1700073B RID: 1851
		public int this[CharacterStatus.Kind kind]
		{
			get
			{
				return this._statuses[kind].currentGrade;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x00069DA0 File Offset: 0x00067FA0
		public bool stuned
		{
			get
			{
				return !this.stun.expired;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x00069DB0 File Offset: 0x00067FB0
		public bool freezed
		{
			get
			{
				return !this.freeze.expired;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x00069DC0 File Offset: 0x00067FC0
		public bool burning
		{
			get
			{
				return !this.burn.expired;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x00069DD0 File Offset: 0x00067FD0
		public bool wounded
		{
			get
			{
				return !this.wound.expired;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x00069DE0 File Offset: 0x00067FE0
		public bool poisoned
		{
			get
			{
				return !this.poison.expired;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x00069DF0 File Offset: 0x00067FF0
		public bool unmovable
		{
			get
			{
				return !this.unmoving.expired;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x00069E00 File Offset: 0x00068000
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x00069E08 File Offset: 0x00068008
		public Stun stun { get; private set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x00069E11 File Offset: 0x00068011
		// (set) Token: 0x0600231B RID: 8987 RVA: 0x00069E19 File Offset: 0x00068019
		public Burn burn { get; private set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x00069E22 File Offset: 0x00068022
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x00069E2A File Offset: 0x0006802A
		public Wound wound { get; private set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x00069E33 File Offset: 0x00068033
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x00069E3B File Offset: 0x0006803B
		public Freeze freeze { get; private set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x00069E44 File Offset: 0x00068044
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x00069E4C File Offset: 0x0006804C
		public Poison poison { get; private set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x00069E55 File Offset: 0x00068055
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x00069E5D File Offset: 0x0006805D
		public Unmoving unmoving { get; private set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x00069E66 File Offset: 0x00068066
		public bool hasAny
		{
			get
			{
				return this.stuned || this.freezed || this.burning || this.wounded || this.poisoned;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x00069E90 File Offset: 0x00068090
		// (set) Token: 0x06002326 RID: 8998 RVA: 0x00069E98 File Offset: 0x00068098
		public bool giveStoppingPowerOnPoison { get; set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x00069EA1 File Offset: 0x000680A1
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x00069EA9 File Offset: 0x000680A9
		public bool canBleedCritical { get; set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x00069EB2 File Offset: 0x000680B2
		// (set) Token: 0x0600232A RID: 9002 RVA: 0x00069EBA File Offset: 0x000680BA
		public int freezeMaxHitStack
		{
			get
			{
				return this._freezeMaxStack;
			}
			set
			{
				this._freezeMaxStack = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x00069EC3 File Offset: 0x000680C3
		// (set) Token: 0x0600232C RID: 9004 RVA: 0x00069ECB File Offset: 0x000680CB
		public EnumArray<CharacterStatus.Kind, SumFloat> durationMultiplier { get; set; } = new EnumArray<CharacterStatus.Kind, SumFloat>(new SumFloat[]
		{
			new SumFloat(1f),
			new SumFloat(1f),
			new SumFloat(1f),
			new SumFloat(1f),
			new SumFloat(1f),
			new SumFloat(1f),
			new SumFloat(1f)
		});

		// Token: 0x0600232D RID: 9005 RVA: 0x00069ED4 File Offset: 0x000680D4
		public bool isLockStatus(CharacterStatus.Kind kind)
		{
			return kind == CharacterStatus.Kind.Stun || kind == CharacterStatus.Kind.Freeze;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x00069EE0 File Offset: 0x000680E0
		private void Awake()
		{
			this.stun = new Stun(this._character);
			this.stun.Initialize();
			this.freeze = new Freeze(this._character);
			this.freeze.Initialize();
			this.burn = new Burn(this._character);
			this.burn.Initialize();
			this.wound = new Wound(this._character);
			this.wound.Initialize();
			this.poison = new Poison(this._character);
			this.poison.Initialize();
			this.unmoving = new Unmoving(this._character);
			this.unmoving.Initialize();
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x00069F98 File Offset: 0x00068198
		public bool Apply(Character attacker, CharacterStatus.ApplyInfo applyInfo)
		{
			if (this._character.invulnerable.value)
			{
				return false;
			}
			if (this.unstoppable.value && this.isLockStatus(applyInfo.kind))
			{
				Vector2 v = MMMaths.RandomPointWithinBounds(this._character.collider.bounds);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString("floating/unstoppable"), v, "#a3a3a3");
				return false;
			}
			bool flag = false;
			switch (applyInfo.kind)
			{
			case CharacterStatus.Kind.Stun:
				flag = this.ApplyStun(attacker);
				break;
			case CharacterStatus.Kind.Freeze:
				flag = this.ApplyFreeze(attacker);
				break;
			case CharacterStatus.Kind.Burn:
				flag = this.ApplyBurn(attacker);
				break;
			case CharacterStatus.Kind.Wound:
				flag = this.ApplyWound(attacker);
				break;
			case CharacterStatus.Kind.Poison:
				flag = this.ApplyPoison(attacker);
				break;
			case CharacterStatus.Kind.Unmoving:
				flag = this.ApplyUnmoving(attacker);
				break;
			}
			if (flag)
			{
				CharacterStatus.OnApplyDelegate onApplyDelegate = this.onApply;
				if (onApplyDelegate != null)
				{
					onApplyDelegate(attacker, this._character, applyInfo);
				}
			}
			return flag;
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0006A090 File Offset: 0x00068290
		public void RemoveStun()
		{
			this._character.ability.Remove(this.stun);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0006A0AC File Offset: 0x000682AC
		public void RemoveAllStatus()
		{
			this._character.ability.Remove(this.stun);
			this._character.ability.Remove(this.freeze);
			this._character.ability.Remove(this.burn);
			this._character.ability.Remove(this.wound);
			this._character.ability.Remove(this.poison);
			this._character.ability.Remove(this.unmoving);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0006A143 File Offset: 0x00068343
		private bool ApplyUnmoving(Character attacker)
		{
			this.unmoving.attacker = attacker;
			this._character.ability.Add(this.unmoving);
			return true;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0006A16C File Offset: 0x0006836C
		private bool ApplyStun(Character attacker)
		{
			this.stun.attacker = attacker;
			if (attacker.status != null)
			{
				this.stun.durationMultiplier = attacker.status.durationMultiplier[CharacterStatus.Kind.Stun].total;
				this.stun.onAttached = attacker.status.onApplyStun;
				this.stun.onRefreshed = attacker.status.onRefreshStun;
				this.stun.onDetached = attacker.status.onReleaseStun;
			}
			this._character.ability.Add(this.stun);
			return true;
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0006A210 File Offset: 0x00068410
		private bool ApplyFreeze(Character attacker)
		{
			this.freeze.attacker = attacker;
			if (attacker.status != null)
			{
				this.freeze.durationMultiplier = attacker.status.durationMultiplier[CharacterStatus.Kind.Freeze].total;
				this.freeze.hitStack = attacker.status.freezeMaxHitStack;
				this.freeze.onAttached = attacker.status.onApplyFreeze;
				this.freeze.onRefreshed = attacker.status.onRefreshFreeze;
				this.freeze.onDetached = attacker.status.onReleaseFreeze;
			}
			this._character.ability.Add(this.freeze);
			return true;
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0006A2C8 File Offset: 0x000684C8
		private bool ApplyBurn(Character attacker)
		{
			this.burn.attacker = attacker;
			if (attacker.status != null)
			{
				this.burn.durationMultiplier = attacker.status.durationMultiplier[CharacterStatus.Kind.Burn].total;
				this.burn.onAttached = attacker.status.onApplyBurn;
				this.burn.onRefreshed = attacker.status.onRefreshBurn;
				this.burn.onDetached = attacker.status.onReleaseBurn;
				this.burn.onTookBurnDamage = attacker.status.onGaveBurnDamage;
				this.burn.onTookEmberDamage = attacker.status.onGaveEmberDamage;
			}
			this._character.ability.Add(this.burn);
			return true;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0006A39C File Offset: 0x0006859C
		private bool ApplyWound(Character attacker)
		{
			this.wound.attacker = attacker;
			if (attacker.status != null)
			{
				this.wound.critical = attacker.status.canBleedCritical;
				this.wound.onAttached = attacker.status.onApplyWound;
				this.wound.onDetached = attacker.status.onApplyBleed;
			}
			this._character.ability.Add(this.wound);
			return true;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0006A420 File Offset: 0x00068620
		private bool ApplyPoison(Character attacker)
		{
			this.poison.attacker = attacker;
			if (attacker.status != null)
			{
				this.poison.durationMultiplier = attacker.status.durationMultiplier[CharacterStatus.Kind.Poison].total;
				this.poison.stoppingPower = attacker.status.giveStoppingPowerOnPoison;
				this.poison.onAttached = attacker.status.onApplyPoison;
				this.poison.onRefreshed = attacker.status.onRefreshPoison;
				this.poison.onDetached = attacker.status.onReleasePoison;
			}
			this._character.ability.Add(this.poison);
			return true;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0006A4D8 File Offset: 0x000686D8
		public bool IsApplying(CharacterStatus.Kind kind)
		{
			switch (kind)
			{
			case CharacterStatus.Kind.Stun:
				return this.stuned && this.stun.remainTime < this.stun.duration;
			case CharacterStatus.Kind.Freeze:
				return this.freezed && this.freeze.remainTime < this.freeze.duration;
			case CharacterStatus.Kind.Burn:
				return this.burning && this.burn.remainTime < this.burn.duration;
			case CharacterStatus.Kind.Wound:
				return this.wounded && this.wound.attached;
			case CharacterStatus.Kind.Poison:
				return this.poisoned && this.poison.remainTime < this.poison.duration;
			case CharacterStatus.Kind.Unmoving:
				return this.unmovable;
			default:
				return false;
			}
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0006A5B4 File Offset: 0x000687B4
		public bool IsApplying(EnumArray<CharacterStatus.Kind, bool> enumArray)
		{
			for (int i = 0; i < enumArray.Count; i++)
			{
				if (enumArray.Array[i] && this.IsApplying(enumArray.Keys[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0006A5F4 File Offset: 0x000687F4
		public void Register(CharacterStatus.Kind kind, CharacterStatus.Timing timing, CharacterStatus.OnTimeDelegate invoke)
		{
			switch (kind)
			{
			case CharacterStatus.Kind.Stun:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyStun += invoke;
					this.onRefreshStun += invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshStun += invoke;
					return;
				}
				this.onReleaseStun += invoke;
				return;
			case CharacterStatus.Kind.Freeze:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyFreeze += invoke;
					this.onRefreshFreeze += invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshFreeze += invoke;
					return;
				}
				this.onReleaseFreeze += invoke;
				return;
			case CharacterStatus.Kind.Burn:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyBurn += invoke;
					this.onRefreshStun += invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshBurn += invoke;
					return;
				}
				this.onReleaseBurn += invoke;
				return;
			case CharacterStatus.Kind.Wound:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyWound += invoke;
					return;
				}
				this.onApplyBleed += invoke;
				return;
			case CharacterStatus.Kind.Poison:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyPoison += invoke;
					this.onRefreshPoison += invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshPoison += invoke;
					return;
				}
				this.onReleasePoison += invoke;
				return;
			default:
				Debug.Log(string.Format("{0} 미구현", kind));
				return;
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0006A6E0 File Offset: 0x000688E0
		public void Unregister(CharacterStatus.Kind kind, CharacterStatus.Timing timing, CharacterStatus.OnTimeDelegate invoke)
		{
			switch (kind)
			{
			case CharacterStatus.Kind.Stun:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyStun -= invoke;
					this.onRefreshStun -= invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshStun -= invoke;
					return;
				}
				this.onReleaseStun -= invoke;
				return;
			case CharacterStatus.Kind.Freeze:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyFreeze -= invoke;
					this.onRefreshFreeze -= invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshFreeze -= invoke;
					return;
				}
				this.onReleaseFreeze -= invoke;
				return;
			case CharacterStatus.Kind.Burn:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyBurn -= invoke;
					this.onRefreshBurn -= invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshBurn -= invoke;
					return;
				}
				this.onReleaseBurn -= invoke;
				return;
			case CharacterStatus.Kind.Wound:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyWound -= invoke;
					return;
				}
				this.onApplyBleed -= invoke;
				return;
			case CharacterStatus.Kind.Poison:
				if (timing == CharacterStatus.Timing.Apply)
				{
					this.onApplyPoison -= invoke;
					this.onRefreshPoison -= invoke;
					return;
				}
				if (timing == CharacterStatus.Timing.Refresh)
				{
					this.onRefreshPoison -= invoke;
					return;
				}
				this.onReleasePoison -= invoke;
				return;
			default:
				Debug.Log(string.Format("{0} 미구현", kind));
				return;
			}
		}

		// Token: 0x04001DC9 RID: 7625
		public static readonly string AttackKeyBurn = "burn";

		// Token: 0x04001DCA RID: 7626
		public static readonly string AttackKeyEmber = "ember";

		// Token: 0x04001DCB RID: 7627
		public static readonly string AttackKeyPoison = "poison";

		// Token: 0x04001DCC RID: 7628
		public static readonly string AttackKeyFreeze = "freeze";

		// Token: 0x04001DCD RID: 7629
		public static readonly string AttackKeyBleed = "bleed";

		// Token: 0x04001DDF RID: 7647
		public readonly SumInt gradeBonuses = new SumInt(0);

		// Token: 0x04001DE0 RID: 7648
		public readonly TrueOnlyLogicalSumList unstoppable = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001DE1 RID: 7649
		private const string _unstoppableFloatingTextKey = "floating/unstoppable";

		// Token: 0x04001DE2 RID: 7650
		private const string _unstoppableFloatingTextColor = "#a3a3a3";

		// Token: 0x04001DE3 RID: 7651
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001DE4 RID: 7652
		private readonly EnumArray<CharacterStatus.Kind, CharacterStatus.Status> _statuses = new EnumArray<CharacterStatus.Kind, CharacterStatus.Status>();

		// Token: 0x04001DEE RID: 7662
		private int _freezeMaxStack = 1;

		// Token: 0x020006CC RID: 1740
		public enum Timing
		{
			// Token: 0x04001DF0 RID: 7664
			Apply,
			// Token: 0x04001DF1 RID: 7665
			Refresh,
			// Token: 0x04001DF2 RID: 7666
			Release
		}

		// Token: 0x020006CD RID: 1741
		private class Grades
		{
			// Token: 0x1400004F RID: 79
			// (add) Token: 0x0600233E RID: 9022 RVA: 0x0006A8AC File Offset: 0x00068AAC
			// (remove) Token: 0x0600233F RID: 9023 RVA: 0x0006A8E4 File Offset: 0x00068AE4
			internal event CharacterStatus.Grades.OnChanged onChanged;

			// Token: 0x1700074D RID: 1869
			// (get) Token: 0x06002340 RID: 9024 RVA: 0x0006A919 File Offset: 0x00068B19
			// (set) Token: 0x06002341 RID: 9025 RVA: 0x0006A921 File Offset: 0x00068B21
			internal int max { get; private set; }

			// Token: 0x06002342 RID: 9026 RVA: 0x0006A92A File Offset: 0x00068B2A
			internal Grades(int maxGrade)
			{
				this._grades = new int[maxGrade];
				this.max = -1;
			}

			// Token: 0x06002343 RID: 9027 RVA: 0x0006A945 File Offset: 0x00068B45
			internal void Attach(int grade)
			{
				this._grades[grade]++;
				if (grade > this.max)
				{
					CharacterStatus.Grades.OnChanged onChanged = this.onChanged;
					if (onChanged != null)
					{
						onChanged(this.max + 1, grade + 1);
					}
					this.max = grade;
				}
			}

			// Token: 0x06002344 RID: 9028 RVA: 0x0006A984 File Offset: 0x00068B84
			internal void Detach(int grade)
			{
				this._grades[grade]--;
				if (grade == this.max && this._grades[grade] == 0)
				{
					for (int i = grade - 1; i >= 0; i--)
					{
						if (this._grades[i] > 0)
						{
							CharacterStatus.Grades.OnChanged onChanged = this.onChanged;
							if (onChanged != null)
							{
								onChanged(this.max + 1, i + 1);
							}
							this.max = i;
							return;
						}
					}
					CharacterStatus.Grades.OnChanged onChanged2 = this.onChanged;
					if (onChanged2 != null)
					{
						onChanged2(this.max + 1, 0);
					}
					this.max = -1;
				}
			}

			// Token: 0x04001DF4 RID: 7668
			private readonly int[] _grades;

			// Token: 0x020006CE RID: 1742
			// (Invoke) Token: 0x06002346 RID: 9030
			internal delegate void OnChanged(int old, int @new);
		}

		// Token: 0x020006CF RID: 1743
		private abstract class Status
		{
			// Token: 0x1700074E RID: 1870
			// (get) Token: 0x06002349 RID: 9033 RVA: 0x0006AA13 File Offset: 0x00068C13
			// (set) Token: 0x0600234A RID: 9034 RVA: 0x0006AA1B File Offset: 0x00068C1B
			protected internal int currentGrade { get; protected set; }

			// Token: 0x0600234B RID: 9035 RVA: 0x0006AA24 File Offset: 0x00068C24
			internal Status(CharacterStatus characterStatus)
			{
				this._characterStatus = characterStatus;
				this._character = characterStatus._character;
			}

			// Token: 0x0600234C RID: 9036
			internal abstract void Apply(int grade);

			// Token: 0x0600234D RID: 9037
			internal abstract void Stop();

			// Token: 0x04001DF6 RID: 7670
			protected readonly CharacterStatus _characterStatus;

			// Token: 0x04001DF7 RID: 7671
			protected readonly Character _character;
		}

		// Token: 0x020006D0 RID: 1744
		[Serializable]
		public class ApplyInfo
		{
			// Token: 0x0600234E RID: 9038 RVA: 0x0006AA3F File Offset: 0x00068C3F
			public ApplyInfo(CharacterStatus.Kind kind)
			{
				this.kind = kind;
			}

			// Token: 0x04001DF9 RID: 7673
			public CharacterStatus.Kind kind;
		}

		// Token: 0x020006D1 RID: 1745
		public enum Kind
		{
			// Token: 0x04001DFB RID: 7675
			Stun,
			// Token: 0x04001DFC RID: 7676
			Freeze,
			// Token: 0x04001DFD RID: 7677
			Burn,
			// Token: 0x04001DFE RID: 7678
			Wound,
			// Token: 0x04001DFF RID: 7679
			Poison,
			// Token: 0x04001E00 RID: 7680
			Unmoving
		}

		// Token: 0x020006D2 RID: 1746
		// (Invoke) Token: 0x06002350 RID: 9040
		public delegate void OnTimeDelegate(Character attacker, Character target);

		// Token: 0x020006D3 RID: 1747
		// (Invoke) Token: 0x06002354 RID: 9044
		public delegate void OnApplyDelegate(Character attacker, Character target, CharacterStatus.ApplyInfo applyInfo);
	}
}
