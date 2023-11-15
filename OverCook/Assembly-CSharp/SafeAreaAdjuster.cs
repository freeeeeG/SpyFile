using System;
using UnityEngine;

// Token: 0x02000ADD RID: 2781
public class SafeAreaAdjuster : MonoBehaviour
{
	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06003842 RID: 14402 RVA: 0x00108D1D File Offset: 0x0010711D
	public bool Completed
	{
		get
		{
			return this.m_bCompleted;
		}
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x00108D25 File Offset: 0x00107125
	public static bool CanBeAdjusted()
	{
		return true;
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x00108D28 File Offset: 0x00107128
	public void Show()
	{
		this.m_buttonReleased = false;
		this.m_bCompleted = false;
		base.enabled = true;
		this.m_uiRect = base.gameObject.RequireComponent<RectTransform>();
		this.UpdateSafeAreaUISize();
		this.m_leftRightAxis = PlayerInputLookup.GetUIValue(PlayerInputLookup.LogicalValueID.MovementX, PlayerInputLookup.Player.One);
		this.m_upDownAxis = PlayerInputLookup.GetUIValue(PlayerInputLookup.LogicalValueID.MovementY, PlayerInputLookup.Player.One);
		this.m_confirmButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		this.m_metaGame = GameUtils.GetMetaGameProgress();
		this.m_widthOption = this.m_metaGame.GetOption(OptionsData.OptionType.SafeAreaX);
		this.m_heightOption = this.m_metaGame.GetOption(OptionsData.OptionType.SafeAreaY);
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x00108DB8 File Offset: 0x001071B8
	public void Hide()
	{
		this.m_bCompleted = true;
		base.enabled = false;
		if (this.m_metaGame != null)
		{
			IOption option = this.m_metaGame.GetOption(OptionsData.OptionType.HasSetSafeArea);
			option.SetOption(1);
			if (SafeAreaAdjuster.CanBeAdjusted())
			{
				SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
				saveManager.RegisterOnIdle(delegate
				{
					saveManager.SaveMetaProgress(null);
				});
			}
		}
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x00108E2C File Offset: 0x0010722C
	protected void Update()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		this.AdvanceAxis(this.m_widthOption, this.m_heightOption, this.m_leftRightAxis, this.m_upDownAxis, deltaTime, ref this.m_scrollAccumulator);
		this.UpdateSafeAreaUISize();
		if (this.m_buttonReleased && this.m_confirmButton.JustReleased())
		{
			this.Hide();
		}
		if (!this.m_buttonReleased && !this.m_confirmButton.IsDown())
		{
			this.m_buttonReleased = true;
		}
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x00108EB4 File Offset: 0x001072B4
	private void UpdateSafeAreaUISize()
	{
		float num = (1f - SafeAreaAdjuster.SafeAreaWidth) * 0.5f;
		float num2 = (1f - SafeAreaAdjuster.SafeAreaHeight) * 0.5f;
		this.m_uiRect.anchorMin = new Vector2(num, num2);
		this.m_uiRect.anchorMax = new Vector2(1f - num, 1f - num2);
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x00108F18 File Offset: 0x00107318
	private void AdvanceAxis(IOption _optionWidth, IOption _optionHeight, ILogicalValue _iLValueX, ILogicalValue _iLValueY, float _dt, ref Vector2 _cumulator)
	{
		float value = _iLValueX.GetValue();
		float value2 = _iLValueY.GetValue();
		int kerchunkedMove = this.GetKerchunkedMove(-value * _dt, ref _cumulator.x);
		_optionWidth.SetOption(_optionWidth.GetOption() + kerchunkedMove);
		int kerchunkedMove2 = this.GetKerchunkedMove(-value2 * _dt, ref _cumulator.y);
		_optionHeight.SetOption(_optionHeight.GetOption() + kerchunkedMove2);
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x00108F78 File Offset: 0x00107378
	private int GetKerchunkedMove(float _analogDiff, ref float _cumulator)
	{
		if (_cumulator * _analogDiff <= 0f)
		{
			_cumulator = _analogDiff;
		}
		else
		{
			_cumulator += _analogDiff;
		}
		float num;
		MathUtils.TruncModf(_cumulator, this.m_kerchunkRate, out num, out _cumulator);
		return (int)num;
	}

	// Token: 0x04002CF6 RID: 11510
	public static float SafeAreaWidth = 1f;

	// Token: 0x04002CF7 RID: 11511
	public static float SafeAreaHeight = 1f;

	// Token: 0x04002CF8 RID: 11512
	private bool m_bCompleted;

	// Token: 0x04002CF9 RID: 11513
	[SerializeField]
	private float m_kerchunkRate = 0.12f;

	// Token: 0x04002CFA RID: 11514
	private RectTransform m_uiRect;

	// Token: 0x04002CFB RID: 11515
	private MetaGameProgress m_metaGame;

	// Token: 0x04002CFC RID: 11516
	private IOption m_widthOption;

	// Token: 0x04002CFD RID: 11517
	private IOption m_heightOption;

	// Token: 0x04002CFE RID: 11518
	private ILogicalValue m_leftRightAxis;

	// Token: 0x04002CFF RID: 11519
	private ILogicalValue m_upDownAxis;

	// Token: 0x04002D00 RID: 11520
	private ILogicalButton m_confirmButton;

	// Token: 0x04002D01 RID: 11521
	private Vector2 m_scrollAccumulator;

	// Token: 0x04002D02 RID: 11522
	private bool m_buttonReleased;
}
