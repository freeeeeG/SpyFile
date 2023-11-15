using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000057 RID: 87
[CreateAssetMenu]
public class TextMessageInfo : ScriptableObject, IEnumerable<TextMessageInfo.Message>, IEnumerable
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007BE0 File Offset: 0x00005DE0
	public TextMessageInfo.Message[] messages
	{
		get
		{
			return this._messageKeys;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007BE8 File Offset: 0x00005DE8
	public string nameKey
	{
		get
		{
			return this._nameKey;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007BF0 File Offset: 0x00005DF0
	public string messageKey
	{
		get
		{
			return this._messageKey;
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00007BF8 File Offset: 0x00005DF8
	public IEnumerator<TextMessageInfo.Message> GetEnumerator()
	{
		return (IEnumerator<TextMessageInfo.Message>)this.messages.GetEnumerator();
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00007C0A File Offset: 0x00005E0A
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x04000168 RID: 360
	[SerializeField]
	private string _nameKey;

	// Token: 0x04000169 RID: 361
	[SerializeField]
	private string _messageKey;

	// Token: 0x0400016A RID: 362
	[SerializeField]
	private TextMessageInfo.Message[] _messageKeys;

	// Token: 0x02000058 RID: 88
	[Serializable]
	public class Message
	{
		// Token: 0x0400016B RID: 363
		public int startIndex;

		// Token: 0x0400016C RID: 364
		public int endIndex;
	}
}
