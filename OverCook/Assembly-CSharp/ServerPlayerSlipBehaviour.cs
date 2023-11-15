using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
public class ServerPlayerSlipBehaviour : ServerSynchroniserBase
{
	// Token: 0x06003362 RID: 13154 RVA: 0x000F2025 File Offset: 0x000F0425
	public override EntityType GetEntityType()
	{
		return EntityType.PlayerSlip;
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x000F2029 File Offset: 0x000F0429
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_slipBehaviour = (PlayerSlipBehaviour)synchronisedObject;
		this.m_playerControls = base.gameObject.RequireComponent<PlayerControls>();
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x000F204F File Offset: 0x000F044F
	public void Slip(ServerSlipCollider _surface)
	{
		base.StartCoroutine(this.SlipCoroutine(_surface));
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x000F2060 File Offset: 0x000F0460
	private IEnumerator SlipCoroutine(ServerSlipCollider _surface)
	{
		this.m_data.Initialise(PlayerSlipMessage.MsgType.Slip);
		this.SendServerEvent(this.m_data);
		this.DisablePlayer(this.m_playerControls);
		IEnumerator waitRoutine = CoroutineUtils.TimerRoutine(this.m_slipBehaviour.m_fallTime, base.gameObject.layer);
		while (waitRoutine.MoveNext())
		{
			yield return null;
		}
		waitRoutine = CoroutineUtils.TimerRoutine(this.m_slipBehaviour.m_downTime, base.gameObject.layer);
		while (waitRoutine.MoveNext())
		{
			yield return null;
		}
		this.m_data.Initialise(PlayerSlipMessage.MsgType.Stand);
		this.SendServerEvent(this.m_data);
		waitRoutine = CoroutineUtils.TimerRoutine(this.m_slipBehaviour.m_standTime, base.gameObject.layer);
		while (waitRoutine.MoveNext())
		{
			yield return null;
		}
		this.ReenablePlayer(this.m_playerControls);
		this.m_data.Initialise(PlayerSlipMessage.MsgType.Finished);
		this.SendServerEvent(this.m_data);
		yield break;
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x000F207C File Offset: 0x000F047C
	private void DisablePlayer(PlayerControls _controls)
	{
		Rigidbody rigidbody = _controls.gameObject.RequireComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		Collider collider = _controls.gameObject.RequireComponent<Collider>();
		collider.enabled = false;
		this.m_playerControls.enabled = false;
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x000F20C8 File Offset: 0x000F04C8
	private void ReenablePlayer(PlayerControls _controls)
	{
		Rigidbody rigidbody = _controls.gameObject.RequireComponent<Rigidbody>();
		rigidbody.isKinematic = false;
		Collider collider = _controls.gameObject.RequireComponent<Collider>();
		collider.enabled = true;
		this.m_playerControls.enabled = true;
	}

	// Token: 0x0400295A RID: 10586
	private PlayerSlipBehaviour m_slipBehaviour;

	// Token: 0x0400295B RID: 10587
	private PlayerSlipMessage m_data = new PlayerSlipMessage();

	// Token: 0x0400295C RID: 10588
	private PlayerControls m_playerControls;
}
