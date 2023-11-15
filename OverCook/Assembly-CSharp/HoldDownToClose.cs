using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A9B RID: 2715
public class HoldDownToClose : MonoBehaviour
{
	// Token: 0x060035B0 RID: 13744 RVA: 0x000FB0DD File Offset: 0x000F94DD
	private void Awake()
	{
		this.m_Button = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PlayerInputLookup.Player.One);
		if (this.m_FillImage != null)
		{
			this.m_FillImage.fillAmount = 0f;
		}
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x000FB110 File Offset: 0x000F9510
	private void Update()
	{
		if (this.m_Button.IsDown())
		{
			this.m_Button.ClaimPressEvent();
			this.m_HoldDownTimer += Time.deltaTime;
		}
		else
		{
			this.m_HoldDownTimer = 0f;
		}
		if (this.m_FillImage != null)
		{
			this.m_FillImage.fillAmount = this.m_HoldDownTimer / this.m_TimeToHoldDown;
		}
		if (this.m_HoldDownTimer >= this.m_TimeToHoldDown)
		{
			UnityEngine.Object.Destroy(this.m_ObjectToKill);
		}
	}

	// Token: 0x04002B34 RID: 11060
	[SerializeField]
	private GameObject m_ObjectToKill;

	// Token: 0x04002B35 RID: 11061
	[SerializeField]
	private float m_TimeToHoldDown = 1f;

	// Token: 0x04002B36 RID: 11062
	[SerializeField]
	private Image m_FillImage;

	// Token: 0x04002B37 RID: 11063
	private float m_HoldDownTimer;

	// Token: 0x04002B38 RID: 11064
	private ILogicalButton m_Button;
}
