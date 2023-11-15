using System;
using UnityEngine;

// Token: 0x0200082D RID: 2093
[AddComponentMenu("KMonoBehaviour/scripts/SelectMarker")]
public class SelectMarker : KMonoBehaviour
{
	// Token: 0x06003CC6 RID: 15558 RVA: 0x00150D19 File Offset: 0x0014EF19
	public void SetTargetTransform(Transform target_transform)
	{
		this.targetTransform = target_transform;
		this.LateUpdate();
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x00150D28 File Offset: 0x0014EF28
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.targetTransform.GetPosition();
		KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
		if (component != null)
		{
			position.x = component.bounds.center.x;
			position.y = component.bounds.center.y + component.bounds.size.y / 2f + 0.1f;
		}
		else
		{
			position.y += 2f;
		}
		Vector3 b = new Vector3(0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0f);
		base.transform.SetPosition(position + b);
	}

	// Token: 0x040027B6 RID: 10166
	public float animationOffset = 0.1f;

	// Token: 0x040027B7 RID: 10167
	private Transform targetTransform;
}
