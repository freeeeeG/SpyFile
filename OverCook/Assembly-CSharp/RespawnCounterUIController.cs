using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B49 RID: 2889
public class RespawnCounterUIController : HoverIconUIController
{
	// Token: 0x06003ABB RID: 15035 RVA: 0x00117D51 File Offset: 0x00116151
	private void Start()
	{
		this.m_background = base.gameObject.RequestComponent<Image>();
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x00117D64 File Offset: 0x00116164
	public void SetTarget(GameObject _target)
	{
		this.m_target = _target;
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x00117D6D File Offset: 0x0011616D
	public void SetCountdown(float _countDown)
	{
		this.m_countDown = _countDown;
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x00117D76 File Offset: 0x00116176
	public void SetTeam(TeamID _team)
	{
		this.m_team = _team;
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x00117D7F File Offset: 0x0011617F
	public void SetPlayerNum(int _num)
	{
		this.m_playerNum = _num;
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x00117D88 File Offset: 0x00116188
	protected override void Awake()
	{
		base.Awake();
		this.m_originalText = this.m_textUI.text;
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x00117DA4 File Offset: 0x001161A4
	public override void LateUpdate()
	{
		this.m_countDown -= TimeManager.GetDeltaTime((!(this.m_target != null)) ? base.gameObject : this.m_target);
		int num = Mathf.Max(Mathf.CeilToInt(this.m_countDown), 0);
		this.m_textUI.text = this.m_originalText.Replace(this.m_matchString, num.ToString());
		if (this.m_background != null)
		{
			if (this.m_team == TeamID.None)
			{
				this.m_background.sprite = this.m_backgrounds[this.m_playerNum];
			}
			else
			{
				this.m_background.sprite = this.m_backgrounds[(int)(TeamID.Two - this.m_team)];
			}
		}
		base.LateUpdate();
	}

	// Token: 0x04002F96 RID: 12182
	[SerializeField]
	private Text m_textUI;

	// Token: 0x04002F97 RID: 12183
	[SerializeField]
	private string m_matchString;

	// Token: 0x04002F98 RID: 12184
	private Image m_background;

	// Token: 0x04002F99 RID: 12185
	private TeamID m_team;

	// Token: 0x04002F9A RID: 12186
	private int m_playerNum;

	// Token: 0x04002F9B RID: 12187
	[SerializeField]
	private Sprite[] m_backgrounds;

	// Token: 0x04002F9C RID: 12188
	private string m_originalText;

	// Token: 0x04002F9D RID: 12189
	private float m_countDown;

	// Token: 0x04002F9E RID: 12190
	private GameObject m_target;
}
