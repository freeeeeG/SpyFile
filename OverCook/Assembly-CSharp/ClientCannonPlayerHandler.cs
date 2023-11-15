using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class ClientCannonPlayerHandler : ClientSynchroniserBase, IClientCannonHandler
{
	// Token: 0x0600145F RID: 5215 RVA: 0x0006F05D File Offset: 0x0006D45D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = synchronisedObject.gameObject.RequireComponent<ClientCannon>();
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x0006F078 File Offset: 0x0006D478
	public override void UpdateSynchronising()
	{
		if (this.m_controls != null && !this.m_inCannon && this.m_playerIdProvider.IsLocallyControlled())
		{
			if (!this.m_controls.GetDirectlyUnderPlayerControl())
			{
				this.m_controls.ThrowIndicator.Hide();
			}
			else
			{
				this.m_controls.ThrowIndicator.Show(false);
			}
		}
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x0006F0E8 File Offset: 0x0006D4E8
	public void Load(GameObject _player)
	{
		this.m_controls = _player.RequireComponent<PlayerControls>();
		this.m_controls.AllowSwitchingWhenDisabled = true;
		this.m_controls.ThrowIndicator.Hide();
		this.m_playerIdProvider = _player.RequireComponent<PlayerIDProvider>();
		this.m_inCannon = true;
		DynamicLandscapeParenting dynamicLandscapeParenting = _player.RequestComponent<DynamicLandscapeParenting>();
		if (dynamicLandscapeParenting != null)
		{
			dynamicLandscapeParenting.enabled = false;
		}
		ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = _player.RequestComponent<ClientWorldObjectSynchroniser>();
		if (clientWorldObjectSynchroniser != null)
		{
			clientWorldObjectSynchroniser.Pause();
		}
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x0006F164 File Offset: 0x0006D564
	public IEnumerator ExitCannonRoutine(GameObject _player, Vector3 _exitPosition, Quaternion _exitRotation)
	{
		this.m_inCannon = false;
		this.m_controls.AllowSwitchingWhenDisabled = false;
		this.m_controls = null;
		this.m_playerIdProvider = null;
		if (_player != null)
		{
			DynamicLandscapeParenting dynamicParenting = _player.RequestComponent<DynamicLandscapeParenting>();
			if (dynamicParenting != null)
			{
				dynamicParenting.enabled = true;
			}
			yield return null;
		}
		if (_player != null)
		{
			ClientWorldObjectSynchroniser synchroniser = _player.RequireComponent<ClientWorldObjectSynchroniser>();
			while (synchroniser != null && !synchroniser.IsReadyToResume())
			{
				yield return null;
			}
			if (synchroniser != null)
			{
				synchroniser.Resume();
			}
		}
		yield break;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x0006F186 File Offset: 0x0006D586
	public bool CanHandle(GameObject _obj)
	{
		return !(_obj == null) && _obj.GetComponent<PlayerControls>() != null;
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x0006F1A4 File Offset: 0x0006D5A4
	public void Launch(GameObject _obj)
	{
		if (_obj != null)
		{
			this.m_inCannon = false;
			this.m_controls.enabled = false;
			this.m_controls.Motion.SetKinematic(true);
			Collider collider = _obj.RequireComponent<Collider>();
			collider.enabled = false;
		}
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x0006F1F0 File Offset: 0x0006D5F0
	public void Land(GameObject _obj)
	{
		if (_obj != null)
		{
			this.m_controls.enabled = true;
			if (this.m_controls.GetComponent<PlayerIDProvider>().IsLocallyControlled())
			{
				this.m_controls.Motion.SetKinematic(false);
			}
			Collider collider = _obj.RequireComponent<Collider>();
			collider.enabled = true;
			this.m_controls.GetComponent<ClientPlayerControlsImpl_Default>().ApplyImpact(this.m_controls.transform.forward.XZ() * 2f, 0.2f);
		}
	}

	// Token: 0x04000FC6 RID: 4038
	private ClientCannon m_cannon;

	// Token: 0x04000FC7 RID: 4039
	private PlayerControls m_controls;

	// Token: 0x04000FC8 RID: 4040
	private PlayerIDProvider m_playerIdProvider;

	// Token: 0x04000FC9 RID: 4041
	private bool m_inCannon;
}
