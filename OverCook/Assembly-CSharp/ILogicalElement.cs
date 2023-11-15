using System;

// Token: 0x020001EF RID: 495
public interface ILogicalElement
{
	// Token: 0x06000835 RID: 2101
	void GetLogicTreeData(out AcyclicGraph<ILogicalElement, LogicalLinkInfo> _graph, out AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node _head);
}
