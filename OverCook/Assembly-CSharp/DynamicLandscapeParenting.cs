using System;
using UnityEngine;

// Token: 0x020009DE RID: 2526
[ExecutionDependency(typeof(IFlowController))]
[RequireComponent(typeof(GroundCast))]
public class DynamicLandscapeParenting : MonoBehaviour
{
	// Token: 0x0600315C RID: 12636 RVA: 0x000E7619 File Offset: 0x000E5A19
	public void SetEnabled(bool bEnabled)
	{
		this.m_bEnabled = bEnabled;
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x000E7624 File Offset: 0x000E5A24
	private void Awake()
	{
		if (GameUtils.GetLevelConfig().m_disableDynamicParenting)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			GroundCast component = base.GetComponent<GroundCast>();
			if (component)
			{
				this.OnGroundChange(component.GetGroundCollider());
			}
		}
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x000E766C File Offset: 0x000E5A6C
	public void OnGroundChange(Collider _collider)
	{
		if (!base.enabled || !this.m_bEnabled)
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

	// Token: 0x040027A4 RID: 10148
	private bool m_bEnabled = true;
}
