using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000953 RID: 2387
	public class PowerbombChainAction : Action
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003371 RID: 13169 RVA: 0x00098ADF File Offset: 0x00096CDF
		public override Motion[] motions
		{
			get
			{
				return this._motions.components.Concat(this._landingMotions.components).ToArray<Motion>();
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003372 RID: 13170 RVA: 0x000950CB File Offset: 0x000932CB
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.motions[0]);
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x00098B04 File Offset: 0x00096D04
		protected override void Awake()
		{
			PowerbombChainAction.<>c__DisplayClass9_0 CS$<>8__locals1 = new PowerbombChainAction.<>c__DisplayClass9_0();
			CS$<>8__locals1.<>4__this = this;
			base.Awake();
			CS$<>8__locals1.blockLookBefore = false;
			CS$<>8__locals1.<Awake>g__JoinMotion|0(this._motions);
			CS$<>8__locals1.<Awake>g__JoinMotion|0(this._landingMotions);
			if (this._motions.components.Length == 0)
			{
				return;
			}
			Motion motion = this._motions.components[this._motions.components.Length - 1];
			motion.onStart += delegate()
			{
				CS$<>8__locals1.<>4__this.owner.movement.onGrounded += CS$<>8__locals1.<>4__this.OnGrounded;
			};
			motion.onEnd += delegate()
			{
				CS$<>8__locals1.<>4__this.owner.movement.onGrounded -= CS$<>8__locals1.<>4__this.OnGrounded;
			};
			motion.onCancel += delegate()
			{
				CS$<>8__locals1.<>4__this.owner.movement.onGrounded -= CS$<>8__locals1.<>4__this.OnGrounded;
			};
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00098BA2 File Offset: 0x00096DA2
		private void OnDisable()
		{
			base.owner.movement.onGrounded -= this.OnGrounded;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00098BC0 File Offset: 0x00096DC0
		private void OnGrounded()
		{
			base.StopAllCoroutines();
			base.DoMotion(this._landingMotions.components[0]);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00098BDC File Offset: 0x00096DDC
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x00098C14 File Offset: 0x00096E14
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			if (base.owner.movement.isGrounded && this._doLandingMotionIfGrounded)
			{
				this._lookingDirection = base.owner.lookingDirection;
				base.DoAction(this._landingMotions.components[0]);
			}
			else
			{
				base.DoAction(this._motions.components[0]);
				base.StopAllCoroutines();
				base.StartCoroutine(this.CExtraGroundCheck());
			}
			return true;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x00098CA7 File Offset: 0x00096EA7
		private IEnumerator CExtraGroundCheck()
		{
			float speedMultiplier = base.GetSpeedMultiplier(this._motions.components[0]);
			yield return Chronometer.global.WaitForSeconds(this._motionTimeout / speedMultiplier);
			for (;;)
			{
				if (!this._motions.components.Any((Motion m) => m.running))
				{
					goto Block_3;
				}
				if (base.owner.movement.isGrounded)
				{
					break;
				}
				yield return null;
			}
			base.DoMotion(this._landingMotions.components[0]);
			yield break;
			Block_3:
			yield break;
		}

		// Token: 0x040029CA RID: 10698
		[SerializeField]
		[Tooltip("이 옵션을 활성화하면 땅에서 사용할 경우 즉시 landing motion이 발동됩니다.")]
		private bool _doLandingMotionIfGrounded;

		// Token: 0x040029CB RID: 10699
		[SerializeField]
		[Tooltip("이 시간이 지난 후부터 땅에 있는 지 검사하여 땅에 있을 경우 강제로 landing motion을 실행시킵니다. 넉백이나 프레임 드랍 등으로 인해 landing motion이 실행되지 않는 경우를 방지하기 위해서이며, motion에 의해 캐릭터가 땅에서 떨어져 공중으로 뜨기 위한 시간 정도로 짧게 주는 것이 좋습니다. 보통 기본값인 0.1초로 충분합니다.")]
		private float _motionTimeout = 0.1f;

		// Token: 0x040029CC RID: 10700
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion.Subcomponents _motions;

		// Token: 0x040029CD RID: 10701
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion.Subcomponents _landingMotions;

		// Token: 0x040029CE RID: 10702
		private Character.LookingDirection _lookingDirection;
	}
}
