using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class ClientTerminalCosmeticDecisions : ClientSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600129A RID: 4762 RVA: 0x000686F8 File Offset: 0x00066AF8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_decisions = (TerminalCosmeticDecisions)synchronisedObject;
		this.m_clientTerminal = base.gameObject.RequireComponent<ClientTerminal>();
		this.m_interactable = base.gameObject.RequireComponent<Interactable>();
		GameObject gameObject = this.m_decisions.MoveIconPrefab.gameObject;
		GameObject obj = GameUtils.InstantiateHoverIconUIController(gameObject, base.transform, "HoverIconCanvas", this.m_decisions.Iconoffset);
		this.m_moveUI = obj.RequireComponent<HoverIconUIController>();
		this.SetIconVisible(false);
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0006877C File Offset: 0x00066B7C
	protected void Update()
	{
		if (this.m_decisions)
		{
			this.SetIconVisible(this.m_clientTerminal.HasSession);
			if (this.m_clientTerminal.HasSession)
			{
				this.SetMaterialOnBits(this.m_decisions.InUseMaterial);
			}
			else if (this.m_interactable.enabled)
			{
				this.SetMaterialOnBits(this.m_decisions.ActiveMaterial);
			}
			else
			{
				this.SetMaterialOnBits(this.m_decisions.DisabledMaterial);
			}
			Vector2 cosmeticJoystickInput = this.m_clientTerminal.CosmeticJoystickInput;
			Vector3 euler = this.m_decisions.JoystickMaxAngle * new Vector3(cosmeticJoystickInput.y, 0f, -cosmeticJoystickInput.x);
			this.m_decisions.ShaftTransform.rotation = Quaternion.Euler(euler);
			bool flag = this.m_clientTerminal.GetTerminal().m_pilotableObject.HasMoved();
			if (!this.m_decisions.IsPlaying && flag)
			{
				GameUtils.StartAudio(this.m_decisions.Moving, this, base.gameObject.layer);
				this.m_decisions.IsPlaying = true;
			}
			else if (this.m_decisions.IsPlaying && !flag)
			{
				GameUtils.StopAudio(this.m_decisions.Moving, this);
				this.m_decisions.IsPlaying = false;
			}
		}
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x000688E0 File Offset: 0x00066CE0
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_decisions.IsPlaying)
		{
			GameUtils.StopAudio(this.m_decisions.Moving, this);
			this.m_decisions.IsPlaying = false;
		}
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x00068915 File Offset: 0x00066D15
	private void SetIconVisible(bool _visible)
	{
		if (this.m_uiVisible != _visible)
		{
			this.m_uiVisible = _visible;
			this.m_moveUI.SetVisibility(_visible);
		}
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x00068938 File Offset: 0x00066D38
	private void SetMaterialOnBits(Material _material)
	{
		for (int i = 0; i < this.m_decisions.ColouredMeshes.Length; i++)
		{
			this.m_decisions.ColouredMeshes[i].material = _material;
		}
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x00068978 File Offset: 0x00066D78
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_interactable.m_onInteractStartedTrigger)
		{
			GameUtils.TriggerAudio(this.m_decisions.StartMoving, base.gameObject.layer);
		}
		else if (_trigger == this.m_interactable.m_onInteractEndedTrigger)
		{
			GameUtils.TriggerAudio(this.m_decisions.StopMoving, base.gameObject.layer);
		}
	}

	// Token: 0x04000E8E RID: 3726
	private TerminalCosmeticDecisions m_decisions;

	// Token: 0x04000E8F RID: 3727
	private HoverIconUIController m_moveUI;

	// Token: 0x04000E90 RID: 3728
	private bool m_uiVisible = true;

	// Token: 0x04000E91 RID: 3729
	private ClientTerminal m_clientTerminal;

	// Token: 0x04000E92 RID: 3730
	private Interactable m_interactable;
}
