using System;
using UnityEngine;

// Token: 0x02000AD2 RID: 2770
public class KeyboardRebindElementSet : MonoBehaviour
{
	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06003804 RID: 14340 RVA: 0x00107E39 File Offset: 0x00106239
	public int ElementCount
	{
		get
		{
			return (this.m_Elements == null) ? 0 : this.m_Elements.Length;
		}
	}

	// Token: 0x170003CE RID: 974
	public KeyboardRebindElement this[int i]
	{
		get
		{
			return (this.m_Elements == null) ? null : this.m_Elements[i];
		}
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x00107E6F File Offset: 0x0010626F
	private void Awake()
	{
		this.m_Elements = base.GetComponentsInChildren<KeyboardRebindElement>();
	}

	// Token: 0x04002CC2 RID: 11458
	private KeyboardRebindElement[] m_Elements;
}
