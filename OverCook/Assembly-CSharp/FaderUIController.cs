using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B2A RID: 2858
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class FaderUIController : UIControllerBase
{
	// Token: 0x060039DE RID: 14814 RVA: 0x00113494 File Offset: 0x00111894
	private void Awake()
	{
		Color color = this.m_image.color;
		color.a = ((!this.m_startOpaque) ? 0f : 1f);
		this.m_image.color = color;
		Animator animator = base.gameObject.RequireComponent<Animator>();
		animator.SetBool(FaderUIController.m_iStartOpaque, this.m_startOpaque);
		if (this.m_fadeClearOnStart)
		{
			animator.SetTrigger(FaderUIController.m_iFadeIn);
		}
	}

	// Token: 0x04002EC2 RID: 11970
	[SerializeField]
	private bool m_startOpaque;

	// Token: 0x04002EC3 RID: 11971
	[SerializeField]
	private bool m_fadeClearOnStart;

	// Token: 0x04002EC4 RID: 11972
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Image m_image;

	// Token: 0x04002EC5 RID: 11973
	private static readonly int m_iStartOpaque = Animator.StringToHash("StartOpaque");

	// Token: 0x04002EC6 RID: 11974
	private static readonly int m_iFadeIn = Animator.StringToHash("FadeIn");
}
