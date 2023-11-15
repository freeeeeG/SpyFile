using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class DebugMenuButton : MonoBehaviour
{
	// Token: 0x06001329 RID: 4905 RVA: 0x0006BAD1 File Offset: 0x00069ED1
	public void SetName(string name)
	{
		this.m_nameText.text = name;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0006BADF File Offset: 0x00069EDF
	public void SetStatus(bool value)
	{
		this.m_status = value;
		this.m_statusText.text = this.m_status.ToString();
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x0006BB04 File Offset: 0x00069F04
	public void Clicked()
	{
	}

	// Token: 0x04000F0E RID: 3854
	[SerializeField]
	private T17Text m_nameText;

	// Token: 0x04000F0F RID: 3855
	[SerializeField]
	private T17Text m_statusText;

	// Token: 0x04000F10 RID: 3856
	private bool m_status;
}
