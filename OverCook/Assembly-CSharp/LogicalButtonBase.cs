using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public abstract class LogicalButtonBase : ILogicalButton, ILogicalElement
{
	// Token: 0x06000838 RID: 2104 RVA: 0x00031F20 File Offset: 0x00030320
	public bool JustPressed()
	{
		bool flag = this.HasUnclaimedPressEvent();
		if (flag)
		{
			this.ClaimPressEvent();
		}
		return flag;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00031F44 File Offset: 0x00030344
	public bool JustReleased()
	{
		bool flag = this.HasUnclaimedReleaseEvent();
		if (flag)
		{
			this.ClaimReleaseEvent();
		}
		return flag;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00031F68 File Offset: 0x00030368
	public bool HasUnclaimedPressEvent()
	{
		bool flag = this.IsDown();
		this.Update(flag);
		return flag && !this.m_pressClaimed;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00031F95 File Offset: 0x00030395
	public void ClaimPressEvent()
	{
		this.m_pressClaimed = true;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00031FA0 File Offset: 0x000303A0
	public bool HasUnclaimedReleaseEvent()
	{
		bool flag = this.IsDown();
		this.Update(flag);
		return !flag && !this.m_releaseClaimed;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00031FCD File Offset: 0x000303CD
	public void ClaimReleaseEvent()
	{
		this.m_releaseClaimed = true;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00031FD8 File Offset: 0x000303D8
	public float GetHeldTimeLength()
	{
		bool flag = this.IsDown();
		this.Update(flag);
		if (flag)
		{
			this.m_buttonDownLength = Time.time - this.m_buttonDownTime;
		}
		return this.m_buttonDownLength;
	}

	// Token: 0x0600083F RID: 2111
	public abstract bool IsDown();

	// Token: 0x06000840 RID: 2112
	public abstract void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _graph, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head);

	// Token: 0x06000841 RID: 2113 RVA: 0x00032011 File Offset: 0x00030411
	protected virtual bool CanProcessInput()
	{
		return Application.isFocused;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00032018 File Offset: 0x00030418
	protected void Update(bool _isDown)
	{
		this.m_pressClaimed = (this.m_pressClaimed && _isDown);
		this.m_releaseClaimed = (this.m_releaseClaimed && !_isDown);
		if (_isDown && !this.m_down)
		{
			this.m_buttonDownTime = Time.time;
		}
		else if (_isDown && !this.m_down)
		{
			this.m_buttonDownLength = Time.time - this.m_buttonDownTime;
		}
		this.m_down = _isDown;
		if (!this.CanProcessInput())
		{
			this.ClaimPressEvent();
			this.ClaimReleaseEvent();
		}
	}

	// Token: 0x040006FD RID: 1789
	private bool m_pressClaimed = true;

	// Token: 0x040006FE RID: 1790
	private bool m_releaseClaimed = true;

	// Token: 0x040006FF RID: 1791
	private float m_buttonDownTime;

	// Token: 0x04000700 RID: 1792
	private float m_buttonDownLength;

	// Token: 0x04000701 RID: 1793
	private bool m_down;
}
