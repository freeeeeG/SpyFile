using System;
using UnityEngine;

// Token: 0x02000558 RID: 1368
[RequireComponent(typeof(Interactable))]
public class PushableObject : SessionInteractable
{
	// Token: 0x060019CD RID: 6605 RVA: 0x000816CC File Offset: 0x0007FACC
	private void Awake()
	{
		Rigidbody rigidbody = base.gameObject.RequireComponent<Rigidbody>();
		foreach (Rigidbody rigidbody2 in base.gameObject.RequestComponentsRecursive<Rigidbody>())
		{
			if (rigidbody2.gameObject != base.gameObject)
			{
				UnityEngine.Object.Destroy(rigidbody2);
			}
		}
		Collider[] array2 = base.gameObject.RequestComponentsRecursive<Collider>();
		for (int j = 0; j < array2.Length; j++)
		{
			Physics.IgnoreCollision(this.m_fakePlayerCollider, array2[j], true);
		}
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x0008175C File Offset: 0x0007FB5C
	public Transform GetAttachPoint(Transform child)
	{
		Transform result = base.transform;
		if (this.m_UseAttachPoints)
		{
			float num = float.MaxValue;
			for (int i = 0; i < this.m_AttachPoints.Length; i++)
			{
				IParentable parentable = this.m_AttachPoints[i].RequestInterface<IParentable>();
				if (parentable != null)
				{
					Transform attachPoint = parentable.GetAttachPoint(child.gameObject);
					float sqrMagnitude = (attachPoint.position - child.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						result = attachPoint;
						num = sqrMagnitude;
					}
				}
			}
		}
		else if (this.m_CentrePoint != null)
		{
			result = this.m_CentrePoint.transform;
		}
		return result;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x0008180C File Offset: 0x0007FC0C
	public bool IsAttached(Transform child)
	{
		if (this.m_UseAttachPoints)
		{
			for (int i = 0; i < this.m_AttachPoints.Length; i++)
			{
				IParentable parentable = this.m_AttachPoints[i].RequestInterface<IParentable>();
				if (parentable != null && child.parent == parentable.GetAttachPoint(child.gameObject))
				{
					return true;
				}
			}
		}
		else if (this.m_CentrePoint != null)
		{
			return child.parent == this.m_CentrePoint.transform;
		}
		return false;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x0008189D File Offset: 0x0007FC9D
	private void OnDestroy()
	{
		if (this.m_fakePlayerCollider != null)
		{
			UnityEngine.Object.Destroy(this.m_fakePlayerCollider);
		}
	}

	// Token: 0x04001480 RID: 5248
	[SerializeField]
	public GameObject m_CentrePoint;

	// Token: 0x04001481 RID: 5249
	[SerializeField]
	public bool m_UseAttachPoints = true;

	// Token: 0x04001482 RID: 5250
	[SerializeField]
	public GameObject[] m_AttachPoints = new GameObject[0];

	// Token: 0x04001483 RID: 5251
	[SerializeField]
	[HideInInspectorTest("m_UseAttachPoints", false)]
	public float m_idealPlayerDistance = 1.4f;

	// Token: 0x04001484 RID: 5252
	[SerializeField]
	[HideInInspectorTest("m_UseAttachPoints", false)]
	public float m_lerpPlayerSpeed = 6f;

	// Token: 0x04001485 RID: 5253
	[SerializeField]
	[AssignChildRecursive("PlayerCollisionEmulator", Editorbility.Editable)]
	public Collider m_fakePlayerCollider;

	// Token: 0x04001486 RID: 5254
	[SerializeField]
	public bool m_playDraggingAudio;

	// Token: 0x04001487 RID: 5255
	[SerializeField]
	[HideInInspectorTest("m_playDraggingAudio", true)]
	public GameLoopingAudioTag m_draggingAudioTag = GameLoopingAudioTag.COUNT;
}
