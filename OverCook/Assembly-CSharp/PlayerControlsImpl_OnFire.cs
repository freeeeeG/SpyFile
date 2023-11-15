using System;
using UnityEngine;

// Token: 0x02000A15 RID: 2581
public class PlayerControlsImpl_OnFire : MonoBehaviour, IPlayerControlsImpl
{
	// Token: 0x0600332D RID: 13101 RVA: 0x000F03DE File Offset: 0x000EE7DE
	private void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x000F03E7 File Offset: 0x000EE7E7
	public void Init(PlayerControls _controls)
	{
		this.m_controls = _controls;
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x000F03F0 File Offset: 0x000EE7F0
	public void Enable()
	{
		if (this.m_iCarrier.InspectCarriedItem() != null)
		{
			Vector2 normalized = base.gameObject.transform.forward.XZ().normalized;
			PlayerControlsHelper.PlaceHeldItem_Client(this.m_controls);
		}
		base.enabled = true;
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x000F0444 File Offset: 0x000EE844
	public void Update_Impl()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		this.Update_Movement(deltaTime);
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x000F0464 File Offset: 0x000EE864
	public void Disable()
	{
		base.enabled = false;
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x000F0470 File Offset: 0x000EE870
	private void Update_Movement(float _deltaTime)
	{
		PlayerControlsHelper.TurnTowardsControlAxis(ref this.m_controlAxisData, this.m_controls, base.gameObject, _deltaTime);
		float runSpeed = this.m_controls.Movement.RunSpeed;
		base.gameObject.GetComponent<Rigidbody>().velocity = this.m_controls.MovementScale * base.gameObject.transform.forward * runSpeed;
	}

	// Token: 0x0400291A RID: 10522
	private PlayerControls m_controls;

	// Token: 0x0400291B RID: 10523
	private ICarrier m_iCarrier;

	// Token: 0x0400291C RID: 10524
	private PlayerControlsHelper.ControlAxisData m_controlAxisData = default(PlayerControlsHelper.ControlAxisData);
}
