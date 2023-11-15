using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BF2 RID: 3058
public class ClientWorldMapSwitch : ClientSynchroniserBase, IClientMapSelectable
{
	// Token: 0x06003E6A RID: 15978 RVA: 0x0012AD48 File Offset: 0x00129148
	protected override void OnDestroy()
	{
		if (this.m_MapOptimizer != null)
		{
			this.m_MapOptimizer.UnRegisterOnSwitchFlipBegin(new CallbackVoid(this.UpdateVisuals));
			this.m_MapOptimizer.UnRegisterOnSwitchFlipEnd(new CallbackVoid(this.UpdateVisuals));
			this.m_MapOptimizer.UnRegisterOnSwitchFlipEnd(new CallbackVoid(this.RevealSwitch));
		}
		base.OnDestroy();
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x0012ADB4 File Offset: 0x001291B4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (WorldMapSwitch)synchronisedObject;
		this.m_MapOptimizer = base.gameObject.RequireComponentRecursive<WorldMapSwitchOptimizer>();
		if (this.m_MapOptimizer != null)
		{
			this.m_MapOptimizer.RegisterOnSwitchFlipBegin(new CallbackVoid(this.UpdateVisuals));
			this.m_MapOptimizer.RegisterOnSwitchFlipEnd(new CallbackVoid(this.UpdateVisuals));
			this.m_MapOptimizer.RegisterOnSwitchFlipEnd(new CallbackVoid(this.RevealSwitch));
		}
		this.UpdateVisuals();
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x0012AE3A File Offset: 0x0012923A
	public void AvatarEnteringSelectable(MapAvatarControls _avatar)
	{
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x0012AE3C File Offset: 0x0012923C
	public void AvatarLeavingSelectable(MapAvatarControls _avatar)
	{
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x0012AE40 File Offset: 0x00129240
	public void SetVisualsToUnPressed()
	{
		if (this.m_bVisualsPressed)
		{
			this.m_bVisualsPressed = false;
			if (this.m_baseObject.m_PadMesh != null)
			{
				Vector3 position = this.m_baseObject.m_PadMesh.transform.position;
				position.y += this.m_baseObject.m_PadPressDistance;
				this.m_baseObject.m_PadMesh.transform.position = position;
			}
			if (this.m_baseObject.m_PadGlowMesh != null && this.m_baseObject.m_PadGlowMesh.material != null)
			{
				this.m_baseObject.m_PadGlowMesh.material.SetFloat("_Multiplier", 0f);
			}
		}
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0012AF0C File Offset: 0x0012930C
	public void SetVisualsToPressed()
	{
		if (!this.m_bVisualsPressed)
		{
			this.m_bVisualsPressed = true;
			if (this.m_baseObject.m_PadMesh != null)
			{
				Vector3 position = this.m_baseObject.m_PadMesh.transform.position;
				position.y -= this.m_baseObject.m_PadPressDistance;
				this.m_baseObject.m_PadMesh.transform.position = position;
			}
			if (this.m_baseObject.m_PadGlowMesh != null && this.m_baseObject.m_PadGlowMesh.material != null)
			{
				this.m_baseObject.m_PadGlowMesh.material.SetFloat("_Multiplier", 1f);
			}
		}
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0012AFD6 File Offset: 0x001293D6
	private void UpdateVisuals()
	{
		if (this.m_baseObject.IsSwitchActivated())
		{
			this.SetVisualsToPressed();
		}
		else
		{
			this.SetVisualsToUnPressed();
		}
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x0012AFFC File Offset: 0x001293FC
	private void RevealSwitch()
	{
		if (!this.m_baseObject.IsFlipped())
		{
			return;
		}
		GameSession gameSession = GameUtils.GetGameSession();
		int switchID = this.m_baseObject.m_switchOwnerData.m_switchMapNode.SwitchID;
		GameProgress.GameProgressData.SwitchState switchState = gameSession.Progress.GetSwitchState(switchID);
		if (switchState.SwitchId == -1)
		{
			gameSession.Progress.RecordSwitchRevealed(switchID);
		}
	}

	// Token: 0x04003227 RID: 12839
	private WorldMapSwitch m_baseObject;

	// Token: 0x04003228 RID: 12840
	private bool m_bVisualsPressed;

	// Token: 0x04003229 RID: 12841
	private WorldMapSwitchOptimizer m_MapOptimizer;
}
