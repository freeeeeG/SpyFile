using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public abstract class IGameSystem : MonoBehaviour
{
	// Token: 0x060006A7 RID: 1703 RVA: 0x000125C5 File Offset: 0x000107C5
	public virtual void Initialize()
	{
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000125C7 File Offset: 0x000107C7
	public virtual void Release()
	{
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x000125C9 File Offset: 0x000107C9
	public virtual void GameUpdate()
	{
	}
}
