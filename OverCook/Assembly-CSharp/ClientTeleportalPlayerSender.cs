using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class ClientTeleportalPlayerSender : ClientBaseTeleportalSender
{
	// Token: 0x06001C7A RID: 7290 RVA: 0x0008B0F3 File Offset: 0x000894F3
	protected override void Awake()
	{
		base.Awake();
		this.m_sender = base.gameObject.RequireComponent<TeleportalPlayerSender>();
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x0008B10C File Offset: 0x0008950C
	protected override IEnumerator TeleportRoutine(ClientTeleportal _entrancePortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object)
	{
		GameObject root = ((MonoBehaviour)_object).gameObject;
		PlayerControls controls = root.RequireComponent<PlayerControls>();
		root = controls.gameObject;
		controls.enabled = false;
		controls.Motion.SetKinematic(true);
		OvercookedAchievementManager achievements = GameUtils.RequestManager<OvercookedAchievementManager>();
		PlayerIDProvider provider = root.RequestComponent<PlayerIDProvider>();
		if (achievements != null && provider != null)
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(provider.GetID());
			achievements.IncStat(6, 1f, padForPlayer);
		}
		Collider collider = root.RequireComponent<Collider>();
		collider.enabled = false;
		DynamicLandscapeParenting dynamicParenting = root.RequestComponent<DynamicLandscapeParenting>();
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = false;
		}
		ClientWorldObjectSynchroniser synchroniser = root.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			synchroniser.Pause();
		}
		Transform attachPoint = this.m_sender.GetAttachPoint(root);
		root.transform.SetParent(attachPoint, true);
		IEnumerator routine = this.m_sender.m_SenderAnimation.Run(root, attachPoint, default(Vector3));
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_sender.OnAnimationFinished("Teleport");
		root.transform.SetParent(null, true);
		root.transform.rotation = Quaternion.identity;
		Animator mesh = root.RequestComponentRecursive<Animator>();
		if (mesh != null)
		{
			mesh.gameObject.SetActive(true);
		}
		ParticleSystem pfx = root.RequestComponentRecursive<ParticleSystem>();
		if (pfx != null)
		{
			pfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		Animator anim = root.GetComponentInChildren<Animator>(true);
		anim.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04001644 RID: 5700
	private TeleportalPlayerSender m_sender;

	// Token: 0x04001645 RID: 5701
	private static readonly int m_iTeleport = Animator.StringToHash("Teleport");

	// Token: 0x04001646 RID: 5702
	private static readonly int m_iIsFinished = Animator.StringToHash("IsFinished");

	// Token: 0x04001647 RID: 5703
	private static readonly int m_iReset = Animator.StringToHash("Reset");
}
