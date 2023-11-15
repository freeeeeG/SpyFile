using System;

// Token: 0x02000206 RID: 518
public interface IReassignable<T> where T : ILogicalElement
{
	// Token: 0x060008A8 RID: 2216
	void Reassign(T _childNode);
}
