using System;

// Token: 0x0200020B RID: 523
public class TimedLogicalButton : ILogicalButton, ILogicalElement
{
	// Token: 0x060008BE RID: 2238 RVA: 0x00034AE9 File Offset: 0x00032EE9
	public TimedLogicalButton(ILogicalButton _baseButton, TimedLogicalButton.Condition _condition, float _requiredDownTime)
	{
		this.m_buttonBase = _baseButton;
		this.m_requiredDownTime = _requiredDownTime;
		this.m_condition = _condition;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00034B06 File Offset: 0x00032F06
	public TimedLogicalButton()
	{
		this.m_requiredDownTime = 0f;
		this.m_condition = TimedLogicalButton.Condition.HeldLonger;
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00034B20 File Offset: 0x00032F20
	public bool JustPressed()
	{
		bool flag = this.HasUnclaimedPressEvent();
		if (flag)
		{
			this.ClaimPressEvent();
		}
		return flag;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00034B44 File Offset: 0x00032F44
	public bool JustReleased()
	{
		bool flag = this.HasUnclaimedReleaseEvent();
		if (flag)
		{
			this.ClaimReleaseEvent();
		}
		return flag;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00034B65 File Offset: 0x00032F65
	public virtual void ClaimPressEvent()
	{
		if (this.m_condition == TimedLogicalButton.Condition.HeldShorter)
		{
			this.m_buttonBase.ClaimReleaseEvent();
		}
		this.m_buttonBase.ClaimPressEvent();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00034B88 File Offset: 0x00032F88
	public virtual bool HasUnclaimedPressEvent()
	{
		bool flag = this.m_buttonBase.HasUnclaimedReleaseEvent();
		bool flag2 = this.m_buttonBase.HasUnclaimedPressEvent();
		float heldTimeLength = this.m_buttonBase.GetHeldTimeLength();
		TimedLogicalButton.Condition condition = this.m_condition;
		bool result;
		if (condition != TimedLogicalButton.Condition.HeldShorter)
		{
			result = (condition == TimedLogicalButton.Condition.HeldLonger && heldTimeLength >= this.m_requiredDownTime && flag2);
		}
		else
		{
			result = (heldTimeLength < this.m_requiredDownTime && flag);
		}
		return result;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00034C14 File Offset: 0x00033014
	public virtual bool HasUnclaimedReleaseEvent()
	{
		bool flag = this.m_buttonBase.HasUnclaimedReleaseEvent();
		float heldTimeLength = this.m_buttonBase.GetHeldTimeLength();
		TimedLogicalButton.Condition condition = this.m_condition;
		bool result;
		if (condition != TimedLogicalButton.Condition.HeldShorter)
		{
			result = (condition == TimedLogicalButton.Condition.HeldLonger && heldTimeLength >= this.m_requiredDownTime && flag);
		}
		else
		{
			result = (heldTimeLength < this.m_requiredDownTime && flag);
		}
		return result;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00034C90 File Offset: 0x00033090
	public virtual void ClaimReleaseEvent()
	{
		this.m_buttonBase.ClaimReleaseEvent();
		if (this.m_condition == TimedLogicalButton.Condition.HeldShorter)
		{
			this.m_buttonBase.ClaimPressEvent();
		}
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00034CB4 File Offset: 0x000330B4
	public virtual bool IsDown()
	{
		if (this.m_condition == TimedLogicalButton.Condition.HeldLonger)
		{
			float heldTimeLength = this.m_buttonBase.GetHeldTimeLength();
			return heldTimeLength >= this.m_requiredDownTime && this.m_buttonBase.IsDown();
		}
		return false;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00034CF8 File Offset: 0x000330F8
	public virtual float GetHeldTimeLength()
	{
		bool flag = this.m_buttonBase.IsDown();
		if (flag)
		{
			return this.m_buttonBase.GetHeldTimeLength();
		}
		return 0f;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00034D28 File Offset: 0x00033128
	public void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _tree, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head)
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		this.m_buttonBase.GetLogicTreeData(out _tree, out node);
		_tree.AddLink(this, node.m_value, new LogicalLinkInfo());
		_head = _tree.GetNode(this);
	}

	// Token: 0x040007AA RID: 1962
	protected float m_downTime;

	// Token: 0x040007AB RID: 1963
	protected float m_requiredDownTime;

	// Token: 0x040007AC RID: 1964
	protected TimedLogicalButton.Condition m_condition;

	// Token: 0x040007AD RID: 1965
	protected ILogicalButton m_buttonBase;

	// Token: 0x0200020C RID: 524
	public enum Condition
	{
		// Token: 0x040007AF RID: 1967
		HeldShorter,
		// Token: 0x040007B0 RID: 1968
		HeldLonger
	}
}
