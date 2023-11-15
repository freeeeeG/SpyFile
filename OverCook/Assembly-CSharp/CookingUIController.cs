using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B25 RID: 2853
public class CookingUIController : HoverIconUIController
{
	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x060039BF RID: 14783 RVA: 0x001122AA File Offset: 0x001106AA
	public CookingUIController.State CurrentState
	{
		get
		{
			return this.m_currentState;
		}
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x001122B4 File Offset: 0x001106B4
	public void SetState(CookingUIController.State _state)
	{
		if (_state == this.m_currentState)
		{
			return;
		}
		this.m_TickAnimation = null;
		this.m_tick.gameObject.SetActive(false);
		switch (_state)
		{
		case CookingUIController.State.Idle:
			this.DoIdleState();
			break;
		case CookingUIController.State.Progressing:
			this.m_progressBar.gameObject.SetActive(true);
			this.m_warningIcon.gameObject.SetActive(false);
			break;
		case CookingUIController.State.Completed:
			this.m_progressBar.gameObject.SetActive(false);
			this.m_warningIcon.gameObject.SetActive(false);
			if (this.m_currentState == CookingUIController.State.Progressing)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.ImCooked, base.gameObject.layer);
			}
			this.m_TickAnimation = this.RunTickAnimation();
			break;
		case CookingUIController.State.OverDoing:
			this.m_progressBar.gameObject.SetActive(false);
			this.m_warningIcon.gameObject.SetActive(true);
			break;
		case CookingUIController.State.Ruined:
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

	// Token: 0x060039C1 RID: 14785 RVA: 0x001123FC File Offset: 0x001107FC
	private void OnDisable()
	{
		if (this.m_TickAnimation != null)
		{
			this.m_TickAnimation = null;
			this.m_tick.gameObject.SetActive(false);
		}
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x00112421 File Offset: 0x00110821
	public void Update()
	{
		if (this.m_TickAnimation != null && !this.m_TickAnimation.MoveNext())
		{
			this.m_TickAnimation = null;
		}
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x00112448 File Offset: 0x00110848
	private IEnumerator RunTickAnimation()
	{
		this.m_tick.gameObject.SetActive(true);
		Color c = this.m_tick.color;
		for (float timer = 0f; timer < this.m_tickAnimationLength; timer += TimeManager.GetDeltaTime(base.gameObject))
		{
			c.a = 0.5f * (1f - Mathf.Cos(6.2831855f * Mathf.Clamp01(timer / this.m_tickAnimationLength)));
			this.m_tick.color = c;
			yield return null;
		}
		this.m_tick.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x00112463 File Offset: 0x00110863
	public void SetProgress(float _value)
	{
		this.m_progressBar.SetValue(_value);
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x00112474 File Offset: 0x00110874
	public void SetOverDoingAmount(float _value)
	{
		int num = (int)Mathf.Round(MathUtils.ClampedRemap(_value, 0f, 1f, -0.49f, (float)this.m_pulseMultipliers.Length - 0.51f));
		this.m_warningIcon.SetPulseSpeedMultiplier(this.m_pulseMultipliers[num]);
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x001124BF File Offset: 0x001108BF
	protected override void Awake()
	{
		base.Awake();
		this.SetState(CookingUIController.State.Idle);
		this.DoIdleState();
		this.m_warningIcon.SetPulseCallback(new CallbackVoid(this.OnOverDoingPulse));
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x001124EB File Offset: 0x001108EB
	private void OnOverDoingPulse()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.CookingWarning, base.gameObject.layer);
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x00112500 File Offset: 0x00110900
	private void DoIdleState()
	{
		this.m_progressBar.gameObject.SetActive(false);
		this.m_warningIcon.gameObject.SetActive(false);
		this.m_tick.gameObject.SetActive(false);
	}

	// Token: 0x04002E88 RID: 11912
	[SerializeField]
	private CookingUIController.State m_currentState;

	// Token: 0x04002E89 RID: 11913
	[SerializeField]
	private ProgressBarUI m_progressBar;

	// Token: 0x04002E8A RID: 11914
	[SerializeField]
	private ImagePulser m_warningIcon;

	// Token: 0x04002E8B RID: 11915
	[SerializeField]
	private Image m_tick;

	// Token: 0x04002E8C RID: 11916
	[SerializeField]
	private float[] m_pulseMultipliers = new float[]
	{
		1f
	};

	// Token: 0x04002E8D RID: 11917
	[SerializeField]
	private float m_tickAnimationLength;

	// Token: 0x04002E8E RID: 11918
	private IEnumerator m_TickAnimation;

	// Token: 0x02000B26 RID: 2854
	public enum State
	{
		// Token: 0x04002E90 RID: 11920
		Idle,
		// Token: 0x04002E91 RID: 11921
		Progressing,
		// Token: 0x04002E92 RID: 11922
		Completed,
		// Token: 0x04002E93 RID: 11923
		OverDoing,
		// Token: 0x04002E94 RID: 11924
		Ruined
	}
}
