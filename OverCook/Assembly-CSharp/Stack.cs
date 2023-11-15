using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056B RID: 1387
[RequireComponent(typeof(BoxCollider))]
public class Stack : MonoBehaviour, IParentable
{
	// Token: 0x06001A19 RID: 6681 RVA: 0x00082BB4 File Offset: 0x00080FB4
	protected virtual void Awake()
	{
		if (this.m_AttachPoint == null)
		{
			GameObject gameObject = new GameObject("StackAttachment");
			this.m_AttachPoint = gameObject.transform;
			this.m_AttachPoint.SetParent(base.transform);
			this.m_AttachPoint.localPosition = default(Vector3);
			this.m_AttachPoint.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x00082C20 File Offset: 0x00081020
	public void RefreshStackTransforms(ref List<GameObject> _objects)
	{
		_objects.RemoveAll((GameObject x) => x == null);
		float num = 0f;
		for (int i = 0; i < _objects.Count; i++)
		{
			GameObject gameObject = _objects[i];
			Transform transform = gameObject.transform;
			transform.localRotation = Quaternion.identity;
			transform.localPosition = new Vector3(0f, num, 0f);
			num += this.m_heightOffset;
		}
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x00082CAC File Offset: 0x000810AC
	public void RefreshStackCollider(ref BoxCollider _collider, int _size)
	{
		float num = (float)_size * this.m_heightOffset;
		_collider.size = _collider.size.WithY(num);
		_collider.center = _collider.center.WithY(0.5f * num);
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x00082CF1 File Offset: 0x000810F1
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_AttachPoint;
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x00082CF9 File Offset: 0x000810F9
	public bool HasClientSidePrediction()
	{
		return false;
	}

	// Token: 0x040014BC RID: 5308
	[SerializeField]
	public string m_itemTag;

	// Token: 0x040014BD RID: 5309
	[SerializeField]
	public float m_heightOffset;

	// Token: 0x040014BE RID: 5310
	private Transform m_AttachPoint;
}
