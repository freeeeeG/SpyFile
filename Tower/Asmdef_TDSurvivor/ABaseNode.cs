using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public abstract class ABaseNode : MonoBehaviour, IMapElement
{
	// Token: 0x0600037D RID: 893 RVA: 0x0000E36B File Offset: 0x0000C56B
	public virtual string GetName()
	{
		return "Boss";
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0000E372 File Offset: 0x0000C572
	public virtual void OnElementSelected()
	{
		throw new NotImplementedException("This functionality is not yet implemented.");
	}

	// Token: 0x040003AC RID: 940
	[SerializeField]
	protected eMapNodeState state;
}
