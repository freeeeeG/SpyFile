using System;
using UnityEngine;

// Token: 0x020009FB RID: 2555
[ExecutionDependency(typeof(IFlowController))]
[RequireComponent(typeof(MapAvatarGroundCast))]
public class MapAvatarDynamicLandscapeParenting : MonoBehaviour
{
	// Token: 0x060031F1 RID: 12785 RVA: 0x000EA36C File Offset: 0x000E876C
	private void Awake()
	{
		MapAvatarGroundCast component = base.GetComponent<MapAvatarGroundCast>();
		if (component)
		{
			this.OnGroundChange(component.GetGroundCollider());
		}
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x000EA398 File Offset: 0x000E8798
	public void OnGroundChange(Collider _collider)
	{
		if (!base.enabled)
		{
			return;
		}
		if (_collider != null)
		{
			IParentable parentable = _collider.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
			if (parentable != null)
			{
				Transform attachPoint = parentable.GetAttachPoint(base.gameObject);
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
		else
		{
			base.transform.SetParent(null);
		}
	}
}
