using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public abstract class Wave : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060001C7 RID: 455 RVA: 0x000085B0 File Offset: 0x000067B0
	// (remove) Token: 0x060001C8 RID: 456 RVA: 0x000085C9 File Offset: 0x000067C9
	public event Action onClear
	{
		add
		{
			this._onClear = (Action)Delegate.Combine(this._onClear, value);
		}
		remove
		{
			this._onClear = (Action)Delegate.Remove(this._onClear, value);
		}
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060001C9 RID: 457 RVA: 0x000085E2 File Offset: 0x000067E2
	// (remove) Token: 0x060001CA RID: 458 RVA: 0x000085FB File Offset: 0x000067FB
	public event Action onSpawn
	{
		add
		{
			this._onSpawn = (Action)Delegate.Combine(this._onSpawn, value);
		}
		remove
		{
			this._onSpawn = (Action)Delegate.Remove(this._onSpawn, value);
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060001CB RID: 459 RVA: 0x00008614 File Offset: 0x00006814
	// (set) Token: 0x060001CC RID: 460 RVA: 0x0000861C File Offset: 0x0000681C
	public Wave.State state { get; protected set; }

	// Token: 0x060001CD RID: 461
	public abstract void Initialize();

	// Token: 0x060001CE RID: 462 RVA: 0x00008625 File Offset: 0x00006825
	public void Stop()
	{
		this.state = Wave.State.Stopped;
		base.StopAllCoroutines();
	}

	// Token: 0x04000192 RID: 402
	protected Action _onClear;

	// Token: 0x04000193 RID: 403
	protected Action _onSpawn;

	// Token: 0x02000061 RID: 97
	public enum State
	{
		// Token: 0x04000196 RID: 406
		Waiting,
		// Token: 0x04000197 RID: 407
		Spawned,
		// Token: 0x04000198 RID: 408
		Cleared,
		// Token: 0x04000199 RID: 409
		Stopped
	}
}
