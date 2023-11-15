using System;
using System.Collections;
using System.Linq;
using Characters.Abilities.Constraints;
using Characters.Operations;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Spirits
{
	// Token: 0x02000B80 RID: 2944
	public class Spirit : AbilityAttacher, IAbility, IAbilityInstance
	{
		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06003BDA RID: 15322 RVA: 0x000B090C File Offset: 0x000AEB0C
		// (set) Token: 0x06003BDB RID: 15323 RVA: 0x000B0914 File Offset: 0x000AEB14
		public float remainTime { get; set; }

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x000B091D File Offset: 0x000AEB1D
		public Sprite icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x000B0925 File Offset: 0x000AEB25
		public virtual float iconFillAmount
		{
			get
			{
				if (!this._waitForTarget)
				{
					return this._remainTimeToNextAttack / this._attackInterval;
				}
				return 0f;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool iconFillFlipped
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x00018EC5 File Offset: 0x000170C5
		public virtual int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x000B0942 File Offset: 0x000AEB42
		public float duration
		{
			get
			{
				return float.PositiveInfinity;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x000B094C File Offset: 0x000AEB4C
		public virtual void UpdateTime(float deltaTime)
		{
			this._remainTimeToNextAttack -= deltaTime * (float)base.owner.stat.GetFinal(Stat.Kind.SpiritAttackCooldownSpeed);
			if (this._remainTimeToNextAttack <= 0f)
			{
				if (!this.FindTarget())
				{
					this._waitForTarget = true;
					this._remainTimeToNextAttack = 0.5f;
					return;
				}
				base.StartCoroutine(this.CTimer());
				this._waitForTarget = false;
				this._remainTimeToNextAttack += this._attackInterval;
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x000B09D0 File Offset: 0x000AEBD0
		protected virtual void Awake()
		{
			this._remainTimeToNextAttack = this._attackInterval;
			this._operationInfos = (from operation in this._operations.components
			orderby operation.timeToTrigger
			select operation).ToArray<OperationInfo>();
			for (int i = 0; i < this._operationInfos.Length; i++)
			{
				this._operationInfos[i].operation.Initialize();
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x000B0A48 File Offset: 0x000AEC48
		private void OnEnable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.ResetPosition;
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x000B0A65 File Offset: 0x000AEC65
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.ResetPosition;
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x000B0A8A File Offset: 0x000AEC8A
		private void ResetPosition()
		{
			this._position = this.targetPosition.transform.position;
			base.transform.position = this._position;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x000B0AB4 File Offset: 0x000AECB4
		private void Update()
		{
			this._position = Vector3.Lerp(this._position, this.targetPosition.position, base.owner.chronometer.master.deltaTime * this._trackSpeed);
			base.transform.localScale = ((this.targetPosition.position.x - this._position.x < 0f) ? Spirit._leftScale : Spirit._rightScale);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x000B0B33 File Offset: 0x000AED33
		private void LateUpdate()
		{
			if (!this.attached)
			{
				return;
			}
			base.transform.position = this._position;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x000B0B50 File Offset: 0x000AED50
		private bool FindTarget()
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(base.owner.gameObject));
			this._detectRange.enabled = true;
			this._overlapper.OverlapCollider(this._detectRange);
			return this._overlapper.results.GetComponents(true).Count != 0;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x000B0BBC File Offset: 0x000AEDBC
		private IEnumerator CTimer()
		{
			int operationIndex = 0;
			float time = 0f;
			for (;;)
			{
				if (operationIndex >= this._operationInfos.Length || time < this._operationInfos[operationIndex].timeToTrigger)
				{
					yield return null;
					time += base.owner.chronometer.animation.deltaTime;
				}
				else
				{
					this._operationInfos[operationIndex].operation.Run(base.owner);
					int num = operationIndex;
					operationIndex = num + 1;
				}
			}
			yield break;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x000B0BCB File Offset: 0x000AEDCB
		private void OnMapChanged(Map old, Map @new)
		{
			if (this.targetPosition != null)
			{
				this._position = this.targetPosition.position;
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00002191 File Offset: 0x00000391
		public override void OnIntialize()
		{
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x000B0BEC File Offset: 0x000AEDEC
		public override void StartAttach()
		{
			this._waitForTarget = false;
			if (base.owner != null)
			{
				base.owner.ability.Add(this);
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x000B0C15 File Offset: 0x000AEE15
		public override void StopAttach()
		{
			if (base.owner != null)
			{
				base.owner.ability.Remove(this);
			}
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x000B0C37 File Offset: 0x000AEE37
		public void Attach()
		{
			base.owner.playerComponents.inventory.item.AttachSpirit(this);
			this.ResetPosition();
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.OnMapChanged;
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x000B0C78 File Offset: 0x000AEE78
		public void Detach()
		{
			base.owner.playerComponents.inventory.item.DetachSpirit(this);
			base.transform.localPosition = Vector3.zero;
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.OnMapChanged;
		}

		// Token: 0x04002F0E RID: 12046
		private static readonly Vector3 _leftScale = new Vector3(-1f, 1f, 1f);

		// Token: 0x04002F0F RID: 12047
		private static readonly Vector3 _rightScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04002F10 RID: 12048
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x04002F11 RID: 12049
		[SerializeField]
		private Collider2D _detectRange;

		// Token: 0x04002F12 RID: 12050
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04002F13 RID: 12051
		private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x04002F14 RID: 12052
		[Space]
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04002F15 RID: 12053
		[SerializeField]
		private Sprite _icon;

		// Token: 0x04002F16 RID: 12054
		[Space]
		[SerializeField]
		private float _trackSpeed = 2.5f;

		// Token: 0x04002F17 RID: 12055
		[SerializeField]
		protected float _attackInterval = 6f;

		// Token: 0x04002F18 RID: 12056
		[Space]
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04002F19 RID: 12057
		private OperationInfo[] _operationInfos;

		// Token: 0x04002F1A RID: 12058
		private float _remainTimeToNextAttack;

		// Token: 0x04002F1B RID: 12059
		private bool _waitForTarget;

		// Token: 0x04002F1C RID: 12060
		private Vector3 _position;

		// Token: 0x04002F1D RID: 12061
		[NonSerialized]
		public Transform targetPosition;
	}
}
