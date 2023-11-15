using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000262 RID: 610
public class AcyclicGraphEnumerator<NodeContents, LinkContents> : IEnumerator where NodeContents : class where LinkContents : class
{
	// Token: 0x06000B3D RID: 2877 RVA: 0x0003C4BD File Offset: 0x0003A8BD
	public AcyclicGraphEnumerator(List<AcyclicGraph<NodeContents, LinkContents>.Node> _list)
	{
		this.m_list = _list;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0003C4D3 File Offset: 0x0003A8D3
	public bool MoveNext()
	{
		this.position++;
		return this.position < this.m_list.Count;
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0003C4F6 File Offset: 0x0003A8F6
	public void Reset()
	{
		this.position = -1;
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0003C4FF File Offset: 0x0003A8FF
	object IEnumerator.Current
	{
		get
		{
			return this.Current;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0003C50C File Offset: 0x0003A90C
	public NodeContents Current
	{
		get
		{
			NodeContents value;
			try
			{
				value = this.m_list[this.position].m_value;
			}
			catch (IndexOutOfRangeException)
			{
				throw new InvalidOperationException();
			}
			return value;
		}
	}

	// Token: 0x040008D0 RID: 2256
	public List<AcyclicGraph<NodeContents, LinkContents>.Node> m_list;

	// Token: 0x040008D1 RID: 2257
	private int position = -1;
}
