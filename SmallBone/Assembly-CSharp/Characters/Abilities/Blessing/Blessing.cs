using System;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Blessing
{
	// Token: 0x02000D0E RID: 3342
	public class Blessing : MonoBehaviour, IAbility, IAbilityInstance
	{
		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x000C4679 File Offset: 0x000C2879
		public float duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06004358 RID: 17240 RVA: 0x000C4681 File Offset: 0x000C2881
		// (set) Token: 0x06004359 RID: 17241 RVA: 0x000C4689 File Offset: 0x000C2889
		public int iconPriority { get; set; }

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x0600435A RID: 17242 RVA: 0x000C4692 File Offset: 0x000C2892
		// (set) Token: 0x0600435B RID: 17243 RVA: 0x000C469A File Offset: 0x000C289A
		public bool removeOnSwapWeapon { get; set; }

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x0600435C RID: 17244 RVA: 0x000C46A3 File Offset: 0x000C28A3
		// (set) Token: 0x0600435D RID: 17245 RVA: 0x000C46AB File Offset: 0x000C28AB
		public Character owner { get; private set; }

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x0600435F RID: 17247 RVA: 0x000C46B4 File Offset: 0x000C28B4
		// (set) Token: 0x06004360 RID: 17248 RVA: 0x000C46BC File Offset: 0x000C28BC
		public float remainTime
		{
			get
			{
				return this._remainTime;
			}
			set
			{
				this._remainTime = value;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004361 RID: 17249 RVA: 0x000C46C5 File Offset: 0x000C28C5
		// (set) Token: 0x06004362 RID: 17250 RVA: 0x000C46CD File Offset: 0x000C28CD
		public bool attached { get; private set; }

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004363 RID: 17251 RVA: 0x000C46D6 File Offset: 0x000C28D6
		public Sprite icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004364 RID: 17252 RVA: 0x000C46DE File Offset: 0x000C28DE
		public float iconFillAmount
		{
			get
			{
				return 1f - this._remainTime / this._duration;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06004365 RID: 17253 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004366 RID: 17254 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x000C46F3 File Offset: 0x000C28F3
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x000C4705 File Offset: 0x000C2905
		public AnimationClip clip
		{
			get
			{
				return this._holyGrail;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x000C470D File Offset: 0x000C290D
		public string activatedNameKey
		{
			get
			{
				return this._activatedNameKey;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x000C4715 File Offset: 0x000C2915
		public string activatedChatKey
		{
			get
			{
				return this._activatedChatKey;
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x000C4720 File Offset: 0x000C2920
		public void Apply(Character target)
		{
			Vector2 v = new Vector2(target.collider.bounds.center.x, target.collider.bounds.max.y + 0.5f);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(this._floatingTextkey), v, "#F2F2F2");
			this.owner = target;
			base.transform.parent = this.owner.transform;
			base.transform.localPosition = Vector3.zero;
			this._abilityAttacher.Initialize(this.owner);
			this.owner.ability.Add(this);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x000C47DF File Offset: 0x000C29DF
		public void Attach()
		{
			this.attached = true;
			this.remainTime = this.duration;
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x000C47FF File Offset: 0x000C29FF
		public void Refresh()
		{
			this.remainTime = this.duration;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x000C480D File Offset: 0x000C2A0D
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x000C481D File Offset: 0x000C2A1D
		public void Detach()
		{
			this.attached = false;
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x000C4831 File Offset: 0x000C2A31
		private void OnDestroy()
		{
			this._icon = null;
			this._holyGrail = null;
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x0400337E RID: 13182
		[SerializeField]
		[Header("임시")]
		private string _notificationText;

		// Token: 0x0400337F RID: 13183
		[SerializeField]
		private string _floatingTextkey;

		// Token: 0x04003380 RID: 13184
		[SerializeField]
		private string _activatedNameKey;

		// Token: 0x04003381 RID: 13185
		[SerializeField]
		private string _activatedChatKey;

		// Token: 0x04003382 RID: 13186
		[SerializeField]
		private AnimationClip _holyGrail;

		// Token: 0x04003383 RID: 13187
		[SerializeField]
		private Sprite _icon;

		// Token: 0x04003384 RID: 13188
		[SerializeField]
		private float _duration;

		// Token: 0x04003385 RID: 13189
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher.Subcomponents _abilityAttacher;

		// Token: 0x04003386 RID: 13190
		private float _remainTime;
	}
}
