using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
[CreateAssetMenu]
public class ATextMessageInfo : ScriptableObject
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600015B RID: 347 RVA: 0x000070F4 File Offset: 0x000052F4
	public string nameKey
	{
		get
		{
			return this._nameKey;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600015C RID: 348 RVA: 0x000070FC File Offset: 0x000052FC
	public string messagesKey
	{
		get
		{
			return this._messagesKey;
		}
	}

	// Token: 0x0400011C RID: 284
	[SerializeField]
	private string _nameKey;

	// Token: 0x0400011D RID: 285
	[SerializeField]
	private string _messagesKey;
}
