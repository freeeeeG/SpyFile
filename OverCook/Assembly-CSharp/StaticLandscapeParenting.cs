using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class StaticLandscapeParenting : MonoBehaviour
{
	// Token: 0x0600162D RID: 5677 RVA: 0x0007603C File Offset: 0x0007443C
	private void Start()
	{
		Vector3 position = base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(position + Vector3.up, -Vector3.up), out raycastHit, 2f, this.m_landscapeMask))
		{
			GameObject gameObject = raycastHit.collider.gameObject;
			if (gameObject.transform != base.transform.parent)
			{
				this.AttachToGround(gameObject.transform);
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x000760C8 File Offset: 0x000744C8
	private void AttachToGround(Transform _target)
	{
		IParentable parentable = _target.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
		if (parentable != null)
		{
			Transform attachPoint = parentable.GetAttachPoint(_target.gameObject);
			if (base.transform.parent != attachPoint)
			{
				base.transform.SetParent(attachPoint, true);
			}
		}
		else
		{
			base.transform.SetParent(null);
		}
	}

	// Token: 0x040010C6 RID: 4294
	[SerializeField]
	private LayerMask m_landscapeMask;
}
