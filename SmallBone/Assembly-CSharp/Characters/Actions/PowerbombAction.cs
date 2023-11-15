using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000951 RID: 2385
	public class PowerbombAction : Action
	{
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600335C RID: 13148 RVA: 0x00098814 File Offset: 0x00096A14
		public Motion motion
		{
			get
			{
				return this._motion;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600335D RID: 13149 RVA: 0x0009881C File Offset: 0x00096A1C
		public Motion landingMotion
		{
			get
			{
				return this._landingMotion;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600335E RID: 13150 RVA: 0x00098824 File Offset: 0x00096A24
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._motion,
					this._landingMotion
				};
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x0009883E File Offset: 0x00096A3E
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._motion);
			}
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x00098868 File Offset: 0x00096A68
		protected override void Awake()
		{
			base.Awake();
			this._motion.onStart += delegate()
			{
				this._lookingDirection = base.owner.lookingDirection;
				base.owner.movement.onGrounded += this.OnGrounded;
			};
			this._motion.onEnd += delegate()
			{
				base.owner.movement.onGrounded -= this.OnGrounded;
			};
			this._motion.onCancel += delegate()
			{
				base.owner.movement.onGrounded -= this.OnGrounded;
			};
			this._landingMotion.onStart += delegate()
			{
				base.owner.lookingDirection = this._lookingDirection;
			};
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000988D7 File Offset: 0x00096AD7
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._motion.Initialize(this);
			this._landingMotion.Initialize(this);
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000988F8 File Offset: 0x00096AF8
		private void OnDisable()
		{
			base.owner.movement.onGrounded -= this.OnGrounded;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00098916 File Offset: 0x00096B16
		private void OnGrounded()
		{
			base.StopAllCoroutines();
			base.DoMotion(this._landingMotion);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x0009892C File Offset: 0x00096B2C
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			if (base.owner.movement.isGrounded && this._doLandingMotionIfGrounded)
			{
				this._lookingDirection = base.owner.lookingDirection;
				base.DoAction(this._landingMotion);
			}
			else
			{
				base.DoAction(this._motion);
				base.StopAllCoroutines();
				base.StartCoroutine(this.CExtraGroundCheck());
			}
			return true;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x000989A4 File Offset: 0x00096BA4
		private IEnumerator CExtraGroundCheck()
		{
			float speedMultiplier = base.GetSpeedMultiplier(this._motion);
			if (this._motionTimeout > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(this._motionTimeout / speedMultiplier);
			}
			while (this._motion.running)
			{
				if (base.owner.movement.isGrounded)
				{
					base.DoMotion(this._landingMotion);
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x040029C2 RID: 10690
		[SerializeField]
		[Tooltip("이 옵션을 활성화하면 땅에서 사용할 경우 즉시 landing motion이 발동됩니다.")]
		private bool _doLandingMotionIfGrounded;

		// Token: 0x040029C3 RID: 10691
		[SerializeField]
		[Tooltip("이 시간이 지난 후부터 땅에 있는 지 검사하여 땅에 있을 경우 강제로 landing motion을 실행시킵니다. 넉백이나 프레임 드랍 등으로 인해 landing motion이 실행되지 않는 경우를 방지하기 위해서이며, motion에 의해 캐릭터가 땅에서 떨어져 공중으로 뜨기 위한 시간 정도로 짧게 주는 것이 좋습니다. 보통 기본값인 0.1초로 충분합니다.")]
		private float _motionTimeout = 0.1f;

		// Token: 0x040029C4 RID: 10692
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _motion;

		// Token: 0x040029C5 RID: 10693
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _landingMotion;

		// Token: 0x040029C6 RID: 10694
		private Character.LookingDirection _lookingDirection;
	}
}
