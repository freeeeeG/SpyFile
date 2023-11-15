using System;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
public class ScoreBoundaryStar : MonoBehaviour
{
	// Token: 0x170003E2 RID: 994
	// (set) Token: 0x060038DA RID: 14554 RVA: 0x0010C85B File Offset: 0x0010AC5B
	public int Score
	{
		set
		{
			if (this.m_scoreText != null)
			{
				this.m_scoreText.SetNonLocalizedText(value.ToString());
			}
		}
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x0010C886 File Offset: 0x0010AC86
	public void SetUnlocked(bool _isUnlocked)
	{
		if (this.m_completedStar != null)
		{
			this.m_completedStar.SetActive(_isUnlocked);
		}
	}

	// Token: 0x04002D98 RID: 11672
	[SerializeField]
	[StarNumber]
	public int m_star;

	// Token: 0x04002D99 RID: 11673
	[SerializeField]
	[AssignComponentRecursive(Editorbility.Editable)]
	private T17Text m_scoreText;

	// Token: 0x04002D9A RID: 11674
	[SerializeField]
	[AssignChildRecursive("Completed_Star", Editorbility.Editable)]
	public GameObject m_completedStar;
}
