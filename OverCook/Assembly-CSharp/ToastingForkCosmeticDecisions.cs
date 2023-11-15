using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class ToastingForkCosmeticDecisions : MonoBehaviour
{
	// Token: 0x060012AE RID: 4782 RVA: 0x00068CFC File Offset: 0x000670FC
	public void ApplyPositionOffsetToChilden(Vector3 _offset, ref Dictionary<string, Vector3> _originalPositions)
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			_originalPositions.SafeAdd(child.name, child.localPosition);
			child.localPosition += _offset;
		}
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x00068D5C File Offset: 0x0006715C
	public void RestorePositionsToChildren(ref Dictionary<string, Vector3> _originalPositions)
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Vector3 localPosition = _originalPositions.SafeGet(child.name, child.localPosition);
			child.localPosition = localPosition;
		}
	}

	// Token: 0x04000EA8 RID: 3752
	[SerializeField]
	[ReadOnly]
	public float m_surfaceAttachedOffset = -0.48f;
}
