using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class GlobalTransformHolder : MonoBehaviour
{
	// Token: 0x0600012F RID: 303 RVA: 0x00006BA0 File Offset: 0x00004DA0
	private void Awake()
	{
		int childCount = base.transform.childCount;
		this._children = new Transform[childCount];
		this._originalPositions = new Vector3[childCount];
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			this._children[i] = child;
			this._originalPositions[i] = child.localPosition;
			child.parent = null;
		}
		base.transform.DetachChildren();
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00006C18 File Offset: 0x00004E18
	private void OnDestroy()
	{
		Transform[] children = this._children;
		for (int i = 0; i < children.Length; i++)
		{
			UnityEngine.Object.Destroy(children[i].gameObject);
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00006C48 File Offset: 0x00004E48
	public void ResetChildrenToLocal()
	{
		for (int i = 0; i < this._originalPositions.Length; i++)
		{
			this._children[i].position = base.transform.TransformPoint(this._originalPositions[i]);
		}
	}

	// Token: 0x04000103 RID: 259
	private Transform[] _children;

	// Token: 0x04000104 RID: 260
	private Vector3[] _originalPositions;
}
