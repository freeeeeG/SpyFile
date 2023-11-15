using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B33 RID: 2867
public class LivesUIController : UIControllerBase
{
	// Token: 0x06003A20 RID: 14880 RVA: 0x00114B2F File Offset: 0x00112F2F
	public int GetLives()
	{
		return this.m_lives;
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x00114B38 File Offset: 0x00112F38
	public void SetLives(int _numLives)
	{
		this.m_lives = _numLives;
		for (int i = 0; i < this.m_lifeImages.Length; i++)
		{
			this.m_lifeImages[i].gameObject.SetActive(i < this.m_lives);
		}
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x00114B80 File Offset: 0x00112F80
	private void Start()
	{
		this.SetLives(this.m_lives);
	}

	// Token: 0x04002F13 RID: 12051
	[SerializeField]
	private Image[] m_lifeImages = new Image[0];

	// Token: 0x04002F14 RID: 12052
	[SerializeField]
	private int m_lives;
}
