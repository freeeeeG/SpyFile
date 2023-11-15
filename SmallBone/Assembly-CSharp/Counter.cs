using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[Serializable]
public class Counter
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00003F66 File Offset: 0x00002166
	// (set) Token: 0x0600008B RID: 139 RVA: 0x00003F6E File Offset: 0x0000216E
	public int count { get; private set; }

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600008C RID: 140 RVA: 0x00003F78 File Offset: 0x00002178
	// (remove) Token: 0x0600008D RID: 141 RVA: 0x00003FB0 File Offset: 0x000021B0
	public event Action onArrival;

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600008E RID: 142 RVA: 0x00003FE5 File Offset: 0x000021E5
	private float time
	{
		get
		{
			if (this._time == null)
			{
				return Time.time;
			}
			return this._time.time;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00004000 File Offset: 0x00002200
	public void Initialize(ChronometerTime time)
	{
		this._time = time;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0000400C File Offset: 0x0000220C
	public void Count()
	{
		float num = this.time - this._lastCount;
		if (num < this.uniqueTime)
		{
			return;
		}
		if (num > this.refreshTime)
		{
			this.count = 0;
		}
		this._lastCount = this.time;
		int count = this.count;
		this.count = count + 1;
		if (this.count >= this.maxCount)
		{
			this.count = 0;
			Action action = this.onArrival;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x0400007D RID: 125
	public int maxCount;

	// Token: 0x0400007E RID: 126
	public float uniqueTime;

	// Token: 0x0400007F RID: 127
	public float refreshTime;

	// Token: 0x04000082 RID: 130
	private float _lastCount;

	// Token: 0x04000083 RID: 131
	private ChronometerTime _time;
}
