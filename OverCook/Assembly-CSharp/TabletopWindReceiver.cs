using System;
using UnityEngine;

// Token: 0x0200059B RID: 1435
public class TabletopWindReceiver : WindAccumulator
{
	// Token: 0x06001B47 RID: 6983 RVA: 0x000875D9 File Offset: 0x000859D9
	protected override void Start()
	{
		this.m_attachStation = base.gameObject.RequestComponent<ServerAttachStation>();
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000875EC File Offset: 0x000859EC
	protected override void Update()
	{
		base.Update();
		if (this.m_attachStation != null)
		{
			Collider collider = this.m_attachStation.gameObject.RequireComponent<Collider>();
			collider.enabled = !this.IsTooMuchWind(base.GetVelocity());
			GameObject gameObject = this.m_attachStation.InspectItem();
			if (gameObject != null && this.ShouldDetach(gameObject))
			{
				this.Detach();
			}
		}
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x00087660 File Offset: 0x00085A60
	protected void Detach()
	{
		this.m_attachStation.TakeItem();
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x0008766E File Offset: 0x00085A6E
	protected virtual bool ShouldDetach(GameObject _object)
	{
		return this.IsTooMuchWind(base.GetVelocity()) && _object.RequestInterface<IWindReceiver>() != null;
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x00087690 File Offset: 0x00085A90
	protected bool IsTooMuchWind(Vector3 _velocity)
	{
		float num = this.m_minDetachForce * this.m_minDetachForce;
		return _velocity.sqrMagnitude > num;
	}

	// Token: 0x0400156F RID: 5487
	[SerializeField]
	private float m_minDetachForce;

	// Token: 0x04001570 RID: 5488
	private ServerAttachStation m_attachStation;
}
