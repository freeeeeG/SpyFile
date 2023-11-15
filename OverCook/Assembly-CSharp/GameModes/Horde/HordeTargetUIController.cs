using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007C0 RID: 1984
	public class HordeTargetUIController : HoverIconUIController
	{
		// Token: 0x06002602 RID: 9730 RVA: 0x000B47E4 File Offset: 0x000B2BE4
		protected override void Awake()
		{
			base.Awake();
			this.SetState(HordeTargetUIController.State.Idle);
			this.m_progressBar.gameObject.SetActive(false);
			this.m_warningIcon.gameObject.SetActive(false);
			this.m_warningIcon.SetPulseCallback(new CallbackVoid(this.OnWarningPulse));
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x000B4837 File Offset: 0x000B2C37
		public HordeTargetUIController.State CurrentState
		{
			get
			{
				return this.m_currentState;
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000B4840 File Offset: 0x000B2C40
		public void SetState(HordeTargetUIController.State _state)
		{
			if (_state == this.m_currentState)
			{
				return;
			}
			switch (_state)
			{
			case HordeTargetUIController.State.Idle:
				this.m_progressBar.gameObject.SetActive(false);
				this.m_warningIcon.gameObject.SetActive(false);
				break;
			case HordeTargetUIController.State.Repairing:
				this.m_progressBar.gameObject.SetActive(true);
				this.m_warningIcon.gameObject.SetActive(false);
				break;
			case HordeTargetUIController.State.UnderAttack:
				this.m_progressBar.gameObject.SetActive(false);
				this.m_warningIcon.gameObject.SetActive(true);
				break;
			case HordeTargetUIController.State.Broken:
				this.m_progressBar.gameObject.SetActive(false);
				this.m_warningIcon.gameObject.SetActive(false);
				break;
			}
			this.m_currentState = _state;
			UI_Move ui_Move = base.gameObject.RequireComponent<UI_Move>();
			if (ui_Move != null)
			{
				ui_Move.UpdateGraphics();
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000B4938 File Offset: 0x000B2D38
		public void SetProgress(float _value)
		{
			this.m_progressBar.SetValue(_value);
			float num = _value - this.m_progressAnticipation;
			if (num <= this.m_warningIconThreshold)
			{
				int num2 = (int)Mathf.Round(MathUtils.ClampedRemap(num, 0f, this.m_warningIconThreshold, (float)this.m_pulseMultipliers.Length - 0.51f, -0.49f));
				this.m_warningIcon.SetPulseSpeedMultiplier(this.m_pulseMultipliers[num2]);
				this.m_warningIcon.gameObject.SetActive(this.m_currentState == HordeTargetUIController.State.UnderAttack);
			}
			else
			{
				this.m_warningIcon.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000B49D4 File Offset: 0x000B2DD4
		public void SetProgressWarningAnticipation(float _value)
		{
			this.m_progressAnticipation = _value;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000B49DD File Offset: 0x000B2DDD
		private void OnWarningPulse()
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.CookingWarning, base.gameObject.layer);
		}

		// Token: 0x04001DFB RID: 7675
		[SerializeField]
		private HordeTargetUIController.State m_currentState;

		// Token: 0x04001DFC RID: 7676
		[SerializeField]
		private ProgressBarUI m_progressBar;

		// Token: 0x04001DFD RID: 7677
		[SerializeField]
		private ImagePulser m_warningIcon;

		// Token: 0x04001DFE RID: 7678
		[SerializeField]
		private float m_warningIconThreshold = 1f;

		// Token: 0x04001DFF RID: 7679
		[SerializeField]
		private float[] m_pulseMultipliers = new float[]
		{
			1f
		};

		// Token: 0x04001E00 RID: 7680
		private float m_progressAnticipation;

		// Token: 0x020007C1 RID: 1985
		public enum State
		{
			// Token: 0x04001E02 RID: 7682
			Idle,
			// Token: 0x04001E03 RID: 7683
			Repairing,
			// Token: 0x04001E04 RID: 7684
			UnderAttack,
			// Token: 0x04001E05 RID: 7685
			Broken
		}
	}
}
