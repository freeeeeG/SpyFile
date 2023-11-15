using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000960 RID: 2400
	public class StreakAction : Action
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x0009947B File Offset: 0x0009767B
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._startMotion,
					this._motion,
					this._endMotion
				};
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060033B1 RID: 13233 RVA: 0x0009949E File Offset: 0x0009769E
		public Motion motion
		{
			get
			{
				return this._motion;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x000994A8 File Offset: 0x000976A8
		public override bool canUse
		{
			get
			{
				if (!base.cooldown.canUse)
				{
					return false;
				}
				if (this._owner.stunedOrFreezed)
				{
					return false;
				}
				if (!base.PassAllConstraints((this._startMotion != null) ? this._startMotion : this._motion))
				{
					return false;
				}
				Motion runningMotion = base.owner.runningMotion;
				if (runningMotion != null)
				{
					if (runningMotion == this._endMotion)
					{
						return false;
					}
					if (runningMotion == this._fullStreakEndMotion)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x0009952F File Offset: 0x0009772F
		private void CacheLookingDirection()
		{
			this._lookingDirection = base.owner.lookingDirection;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x00099542 File Offset: 0x00097742
		private void RestoreLookingDirection()
		{
			base.owner.lookingDirection = this._lookingDirection;
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x00099555 File Offset: 0x00097755
		private void Expire()
		{
			base.cooldown.streak.Expire();
			Action onEnd = this._onEnd;
			if (onEnd == null)
			{
				return;
			}
			onEnd();
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00099578 File Offset: 0x00097778
		protected override void Awake()
		{
			base.Awake();
			this._motion.Initialize(this);
			base.cooldown.Serialize();
			if (this._motion.blockLook && this._blockLook)
			{
				this._motion.onStart += this.RestoreLookingDirection;
			}
			else if (this._startMotion != null)
			{
				this._motion.onStart += delegate()
				{
					if (base.owner.runningMotion == this._startMotion)
					{
						this.RestoreLookingDirection();
					}
				};
			}
			this._motion.onEnd += this.OnMotionEnd;
			if (this._startMotion != null)
			{
				this._startMotion.Initialize(this);
				this._startMotion.onEnd += delegate()
				{
					base.DoMotion(this._motion);
				};
			}
			if (this._endMotion != null)
			{
				this._endMotion.Initialize(this);
				if (this._endMotion.blockLook && this._blockLook)
				{
					this._endMotion.onStart += this.RestoreLookingDirection;
				}
				this._endMotion.onEnd += this.Expire;
			}
			if (this._fullStreakEndMotion != null)
			{
				this._fullStreakEndMotion.Initialize(this);
				if (this._fullStreakEndMotion.blockLook && this._blockLook)
				{
					this._fullStreakEndMotion.onStart += this.RestoreLookingDirection;
				}
				this._fullStreakEndMotion.onEnd += this.Expire;
				return;
			}
			if (this._endMotion != null)
			{
				this._fullStreakEndMotion = this._endMotion;
			}
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x00099712 File Offset: 0x00097912
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x0009971C File Offset: 0x0009791C
		private void OnMotionEnd()
		{
			if (this._fullStreakEndMotion != null && base.cooldown.streak.count > 0 && base.cooldown.streak.remains == 0)
			{
				base.DoMotion(this._fullStreakEndMotion);
				return;
			}
			if (!this._endReserved && this._inputMethod == Action.InputMethod.TryStartIsPressed)
			{
				if (this._inputMethod == Action.InputMethod.TryStartIsPressed && base.cooldown.streak.remains > 0 && base.ConsumeCooldownIfNeeded())
				{
					base.DoMotion(this._motion);
				}
				return;
			}
			if (this._endMotion != null)
			{
				base.DoMotion(this._endMotion);
				return;
			}
			this.Expire();
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000997CC File Offset: 0x000979CC
		public override bool TryStart()
		{
			Motion runningMotion = this._owner.runningMotion;
			if (runningMotion != null && (runningMotion == this._endMotion || runningMotion == this._fullStreakEndMotion))
			{
				return false;
			}
			if (this.HandleWasPressed())
			{
				return true;
			}
			if ((this._startMotion != null && runningMotion == this._startMotion) || runningMotion == this._motion)
			{
				return false;
			}
			if (base.cooldown.streak.remains > 0)
			{
				return false;
			}
			if (!this.canUse)
			{
				return false;
			}
			if (!base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			this.CacheLookingDirection();
			if (this._startMotion != null)
			{
				base.DoAction(this._startMotion);
			}
			else
			{
				base.DoAction(this._motion);
			}
			this._startReserved = false;
			this._endReserved = false;
			return true;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000998A8 File Offset: 0x00097AA8
		private bool HandleWasPressed()
		{
			if (this._inputMethod != Action.InputMethod.TryStartWasPressed)
			{
				return false;
			}
			if (this._startReserved)
			{
				return false;
			}
			if (!base.cooldown.canUse)
			{
				return false;
			}
			if (base.cooldown.streak.remains == 0)
			{
				return false;
			}
			Motion runningMotion = this._owner.runningMotion;
			if (runningMotion == null)
			{
				return false;
			}
			if (runningMotion != this._motion)
			{
				return false;
			}
			if (this._mainInput.x == this._mainInput.y)
			{
				return false;
			}
			if (!MMMaths.Range(runningMotion.normalizedTime, this._mainInput))
			{
				return false;
			}
			if (MMMaths.Range(runningMotion.normalizedTime, this._mainCancel))
			{
				if (base.ConsumeCooldownIfNeeded())
				{
					base.DoMotion(this._motion);
				}
			}
			else
			{
				this._startReserved = true;
				base.StartCoroutine(this.CReservedAttack());
			}
			return true;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x00099981 File Offset: 0x00097B81
		private IEnumerator CReservedAttack()
		{
			while (this._owner.runningMotion == this._motion && this._startReserved)
			{
				yield return null;
				if (MMMaths.Range(this._owner.runningMotion.normalizedTime, this._mainCancel))
				{
					if (base.ConsumeCooldownIfNeeded())
					{
						base.DoMotion(this._motion);
						break;
					}
					break;
				}
			}
			this._startReserved = false;
			yield break;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x00099990 File Offset: 0x00097B90
		protected override void ForceEndProcess()
		{
			this._needForceEnd = false;
			Motion[] motions = this.motions;
			int i = 0;
			while (i < motions.Length)
			{
				Motion obj = motions[i];
				if (base.owner.motion.Equals(obj))
				{
					base.cooldown.streak.Expire();
					base.owner.CancelAction();
					Action onEnd = this._onEnd;
					if (onEnd == null)
					{
						return;
					}
					onEnd();
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000999FC File Offset: 0x00097BFC
		public override bool TryEnd()
		{
			if (this._inputMethod != Action.InputMethod.TryStartIsPressed)
			{
				return false;
			}
			this._endReserved = true;
			if (!this._cancelToEnd)
			{
				return false;
			}
			if (this._endMotion == null)
			{
				base.cooldown.streak.Expire();
				base.owner.CancelAction();
				Action onEnd = this._onEnd;
				if (onEnd != null)
				{
					onEnd();
				}
				return false;
			}
			base.DoMotion(this._endMotion);
			return true;
		}

		// Token: 0x040029ED RID: 10733
		[Subcomponent(true, typeof(Motion))]
		[HideInInspector]
		[SerializeField]
		private Motion _startMotion;

		// Token: 0x040029EE RID: 10734
		[MinMaxSlider(0f, 1f)]
		[HideInInspector]
		[SerializeField]
		private Vector2 _mainInput = new Vector2(0f, 1f);

		// Token: 0x040029EF RID: 10735
		[MinMaxSlider(0f, 1f)]
		[HideInInspector]
		[SerializeField]
		private Vector2 _mainCancel = new Vector2(0.9f, 1f);

		// Token: 0x040029F0 RID: 10736
		[HideInInspector]
		[SerializeField]
		private bool _blockLook = true;

		// Token: 0x040029F1 RID: 10737
		[HideInInspector]
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		private Motion _motion;

		// Token: 0x040029F2 RID: 10738
		[HideInInspector]
		[SerializeField]
		private bool _cancelToEnd;

		// Token: 0x040029F3 RID: 10739
		[HideInInspector]
		[Subcomponent(true, typeof(Motion))]
		[SerializeField]
		private Motion _endMotion;

		// Token: 0x040029F4 RID: 10740
		[SerializeField]
		[HideInInspector]
		[Subcomponent(true, typeof(Motion))]
		private Motion _fullStreakEndMotion;

		// Token: 0x040029F5 RID: 10741
		private bool _startReserved;

		// Token: 0x040029F6 RID: 10742
		private bool _endReserved;

		// Token: 0x040029F7 RID: 10743
		private Character.LookingDirection _lookingDirection;
	}
}
