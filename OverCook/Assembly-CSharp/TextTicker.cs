using System;
using UnityEngine;

// Token: 0x02000B1C RID: 2844
public class TextTicker : MonoBehaviour
{
	// Token: 0x06003998 RID: 14744 RVA: 0x00111348 File Offset: 0x0010F748
	private void Update()
	{
		if (this.m_Text != null)
		{
			float time = Time.time;
			if (this.m_CurrentTickTimer + this.m_TickLength < time)
			{
				this.m_TickCount++;
				if (this.m_TickCount > this.m_MaxTickCount)
				{
					this.m_TickCount = 0;
				}
				string text = string.Empty;
				for (int i = 0; i < this.m_TickCount; i++)
				{
					text += this.m_TickerCharacter;
				}
				this.m_Text.SetNonLocalizedText(text);
				this.m_CurrentTickTimer = time;
			}
		}
	}

	// Token: 0x04002E43 RID: 11843
	public T17Text m_Text;

	// Token: 0x04002E44 RID: 11844
	public char m_TickerCharacter = '.';

	// Token: 0x04002E45 RID: 11845
	private int m_TickCount;

	// Token: 0x04002E46 RID: 11846
	public int m_MaxTickCount;

	// Token: 0x04002E47 RID: 11847
	private float m_CurrentTickTimer;

	// Token: 0x04002E48 RID: 11848
	public float m_TickLength;
}
