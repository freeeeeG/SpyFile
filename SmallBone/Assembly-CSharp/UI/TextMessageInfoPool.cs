using System;
using UnityEngine;

namespace UI
{
	// Token: 0x02000385 RID: 901
	[CreateAssetMenu]
	public class TextMessageInfoPool : TextMessageInfo
	{
		// Token: 0x06001078 RID: 4216 RVA: 0x00030BBB File Offset: 0x0002EDBB
		public TextMessageInfo.Message GetRandomText()
		{
			return base.messages[UnityEngine.Random.Range(0, base.messages.Length - 1)];
		}
	}
}
