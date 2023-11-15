using System;
using UnityEngine;

// Token: 0x02000B55 RID: 2901
public class TimedInputUIController : UIControllerBase
{
	// Token: 0x06003AF3 RID: 15091 RVA: 0x00118F70 File Offset: 0x00117370
	public void SetDisplayInput(TimedLogicalButton _button, float _targetTime)
	{
		this.m_button = _button;
		this.m_targetTime = _targetTime;
		this.Update();
	}

	// Token: 0x06003AF4 RID: 15092 RVA: 0x00118F86 File Offset: 0x00117386
	protected void OnEnable()
	{
		this.UpdateProgress();
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x00118F8E File Offset: 0x0011738E
	public void Update()
	{
		this.UpdateProgress();
	}

	// Token: 0x06003AF6 RID: 15094 RVA: 0x00118F98 File Offset: 0x00117398
	private void UpdateProgress()
	{
		if (this.m_button == null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		float filledAmount = Mathf.Clamp01(this.m_button.GetHeldTimeLength() / this.m_targetTime);
		this.m_progressBar.SetFilledAmount(filledAmount);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04002FF2 RID: 12274
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private T17FilledImage m_progressBar;

	// Token: 0x04002FF3 RID: 12275
	private ILogicalButton m_button;

	// Token: 0x04002FF4 RID: 12276
	private float m_targetTime;
}
