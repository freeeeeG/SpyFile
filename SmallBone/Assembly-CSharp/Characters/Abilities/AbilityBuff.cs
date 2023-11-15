using System;
using System.Runtime.CompilerServices;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities
{
	// Token: 0x02000994 RID: 2452
	public class AbilityBuff : MonoBehaviour, IAbility, IAbilityInstance
	{
		// Token: 0x14000092 RID: 146
		// (add) Token: 0x0600349A RID: 13466 RVA: 0x0009B268 File Offset: 0x00099468
		// (remove) Token: 0x0600349B RID: 13467 RVA: 0x0009B2A0 File Offset: 0x000994A0
		public event Action onSold;

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x0009B2D5 File Offset: 0x000994D5
		protected string _keyBase
		{
			get
			{
				return "abilityBuff/" + base.name;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x0009B2E7 File Offset: 0x000994E7
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600349E RID: 13470 RVA: 0x0009B2FE File Offset: 0x000994FE
		public string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x0009B315 File Offset: 0x00099515
		public Rarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060034A0 RID: 13472 RVA: 0x0009B31D File Offset: 0x0009951D
		// (set) Token: 0x060034A1 RID: 13473 RVA: 0x0009B325 File Offset: 0x00099525
		public int price { get; set; }

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060034A2 RID: 13474 RVA: 0x0009B32E File Offset: 0x0009952E
		// (set) Token: 0x060034A3 RID: 13475 RVA: 0x0009B336 File Offset: 0x00099536
		public Character owner { get; private set; }

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060034A4 RID: 13476 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060034A5 RID: 13477 RVA: 0x0009B33F File Offset: 0x0009953F
		// (set) Token: 0x060034A6 RID: 13478 RVA: 0x0009B347 File Offset: 0x00099547
		public float remainTime { get; set; }

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060034A7 RID: 13479 RVA: 0x0009B350 File Offset: 0x00099550
		// (set) Token: 0x060034A8 RID: 13480 RVA: 0x0009B358 File Offset: 0x00099558
		public bool attached { get; private set; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060034A9 RID: 13481 RVA: 0x0009B361 File Offset: 0x00099561
		public Sprite icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060034AB RID: 13483 RVA: 0x0009B369 File Offset: 0x00099569
		public int iconStacks
		{
			get
			{
				if (this._stackable != null)
				{
					return (int)this._stackable.stack;
				}
				return this.remainMaps;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x0009B386 File Offset: 0x00099586
		// (set) Token: 0x060034AE RID: 13486 RVA: 0x0009B38E File Offset: 0x0009958E
		public float duration { get; set; }

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060034AF RID: 13487 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060034B0 RID: 13488 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060034B1 RID: 13489 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060034B2 RID: 13490 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060034B3 RID: 13491 RVA: 0x0009B397 File Offset: 0x00099597
		// (set) Token: 0x060034B4 RID: 13492 RVA: 0x0009B3B2 File Offset: 0x000995B2
		public float stack
		{
			get
			{
				if (this._stackable != null)
				{
					return this._stackable.stack;
				}
				return 0f;
			}
			set
			{
				if (this._stackable == null)
				{
					return;
				}
				this._stackable.stack = value;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x0009B3C9 File Offset: 0x000995C9
		public bool stackable
		{
			get
			{
				return this._stackable != null;
			}
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x0009B3D4 File Offset: 0x000995D4
		private void Awake()
		{
			this._stackable = base.GetComponentInChildren<IStackable>();
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x0009B3E2 File Offset: 0x000995E2
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.OnMapChagned;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x0009B408 File Offset: 0x00099608
		public void Attach(Character character)
		{
			this._display.gameObject.SetActive(false);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._lootSound, base.transform.position);
			this.owner = character;
			base.transform.parent = this.owner.transform;
			base.transform.localPosition = Vector3.zero;
			if (this._abilityAttacher.components.Length == 0)
			{
				return;
			}
			this._abilityAttacher.Initialize(this.owner);
			if (this.stackable)
			{
				foreach (AbilityBuff abilityBuff in this.owner.ability.GetInstancesByInstanceType<AbilityBuff>())
				{
					if (abilityBuff.name.Equals(base.name, StringComparison.OrdinalIgnoreCase))
					{
						AbilityBuff abilityBuff2 = abilityBuff;
						float stack = abilityBuff2.stack;
						abilityBuff2.stack = stack + 1f;
						return;
					}
				}
			}
			this.owner.ability.Add(this);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x0009B51C File Offset: 0x0009971C
		public void Loot(Character character)
		{
			this.Attach(character);
			this.owner.health.PercentHeal((float)this._healingPercent * 0.01f);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x0009B544 File Offset: 0x00099744
		private void OnMapChagned(Map old, Map @new)
		{
			if (@new.waveContainer.enemyWaves.Length == 0)
			{
				return;
			}
			this.remainMaps--;
			if (this.remainMaps == 0)
			{
				this.owner.ability.Remove(this);
				this._abilityAttacher.StopAttach();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x0009B59E File Offset: 0x0009979E
		public void Refresh()
		{
			this.remainTime = this.duration;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x0009B5AC File Offset: 0x000997AC
		public void Attach()
		{
			this.attached = true;
			this.remainMaps = this._durationMaps;
			this._loopEffectInstance = ((this._loopEffect == null) ? null : this._loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.OnMapChagned;
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x0009B62E File Offset: 0x0009982E
		public void Detach()
		{
			this.attached = false;
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
			}
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.OnMapChagned;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x0009B66C File Offset: 0x0009986C
		public void Initialize()
		{
			this._display.Initialize(this);
			this._display.price = this.price;
			this._display.onLoot += this.Loot;
			this._display.onLoot += delegate(Character c)
			{
				Action action = this.onSold;
				if (action == null)
				{
					return;
				}
				action();
			};
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0009B6C4 File Offset: 0x000998C4
		public override string ToString()
		{
			return string.Format("{0}|{1}|{2}", base.name, this.remainMaps, this.stack);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0009B6EC File Offset: 0x000998EC
		[return: TupleElementNames(new string[]
		{
			"name",
			"durationMaps",
			"stack"
		})]
		public static ValueTuple<string, int, int> Parse(string text)
		{
			string[] array = text.Split(new char[]
			{
				'|'
			});
			return new ValueTuple<string, int, int>(array[0], int.Parse(array[1]), int.Parse(array[2]));
		}

		// Token: 0x04002A73 RID: 10867
		[SerializeField]
		private AbilityBuffDisplay _display;

		// Token: 0x04002A74 RID: 10868
		[SerializeField]
		private Rarity _rarity;

		// Token: 0x04002A75 RID: 10869
		[SerializeField]
		[FormerlySerializedAs("_healAmount")]
		private int _healingPercent;

		// Token: 0x04002A76 RID: 10870
		[SerializeField]
		private int _durationMaps = 3;

		// Token: 0x04002A77 RID: 10871
		[SerializeField]
		private Sprite _icon;

		// Token: 0x04002A78 RID: 10872
		[SerializeField]
		private SoundInfo _lootSound;

		// Token: 0x04002A79 RID: 10873
		[SerializeField]
		private EffectInfo _loopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002A7A RID: 10874
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher.Subcomponents _abilityAttacher;

		// Token: 0x04002A7B RID: 10875
		[NonSerialized]
		public int remainMaps;

		// Token: 0x04002A7C RID: 10876
		private EffectPoolInstance _loopEffectInstance;

		// Token: 0x04002A7D RID: 10877
		private IStackable _stackable;

		// Token: 0x04002A7E RID: 10878
		private const string _prefix = "abilityBuff";
	}
}
