using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BE4 RID: 3044
public class ServerWorldMapInfoPopup : ServerSynchroniserBase
{
	// Token: 0x06003E3A RID: 15930 RVA: 0x00129E4E File Offset: 0x0012824E
	public override EntityType GetEntityType()
	{
		return EntityType.WorldPopup;
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x00129E52 File Offset: 0x00128252
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_popupInfo = (synchronisedObject as WorldMapInfoPopup);
		this.m_continueButton = PlayerInputLookup.GetButton(this.m_popupInfo.m_button, PlayerInputLookup.Player.One);
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x00129E7E File Offset: 0x0012827E
	protected override void OnEnable()
	{
		base.OnEnable();
		base.StartCoroutine(this.PopupRoutine());
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x00129E94 File Offset: 0x00128294
	public IEnumerator PopupRoutine()
	{
		IEnumerator wait = null;
		if (this.m_popupInfo.m_autoCancel)
		{
			wait = CoroutineUtils.TimerRoutine(this.m_popupInfo.m_autoCancelTime, base.gameObject.layer);
		}
		while (wait == null || wait.MoveNext())
		{
			if (this.m_continueButton.JustPressed())
			{
				break;
			}
			yield return null;
		}
		this.SendServerEvent(this.m_message);
		yield break;
	}

	// Token: 0x040031F8 RID: 12792
	private WorldMapInfoPopup m_popupInfo;

	// Token: 0x040031F9 RID: 12793
	private WorldMapInfoPopupMessage m_message = new WorldMapInfoPopupMessage();

	// Token: 0x040031FA RID: 12794
	private ILogicalButton m_continueButton;
}
