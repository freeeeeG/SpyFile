using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class ClientCannon : ClientSynchroniserBase
{
	// Token: 0x06001440 RID: 5184 RVA: 0x0006E8E8 File Offset: 0x0006CCE8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = (Cannon)synchronisedObject;
		this.m_playerHandler = this.m_cannon.gameObject.RequireComponent<ClientCannonPlayerHandler>();
		this.m_handlers = base.GetComponents<IClientCannonHandler>();
		this.m_pilotRotation = base.gameObject.RequestComponentRecursive<PilotRotation>();
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x0006E93B File Offset: 0x0006CD3B
	public override EntityType GetEntityType()
	{
		return EntityType.Cannon;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0006E940 File Offset: 0x0006CD40
	public void Load(GameObject _obj)
	{
		this.m_loadedObject = _obj;
		this.m_exitPosition = _obj.transform.position;
		this.m_exitRotation = _obj.transform.rotation;
		IClientCannonHandler handler = this.GetHandler(_obj);
		if (handler != null)
		{
			handler.Load(_obj);
		}
		_obj.transform.position = this.m_cannon.m_attachPoint.position;
		_obj.transform.rotation = this.m_cannon.m_attachPoint.rotation;
		_obj.transform.SetParent(this.m_cannon.m_attachPoint, true);
		if (this.m_onLoadedCallback != null)
		{
			this.m_onLoadedCallback(_obj);
		}
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x0006E9F0 File Offset: 0x0006CDF0
	public IClientCannonHandler GetHandler(GameObject _obj)
	{
		for (int i = 0; i < this.m_handlers.Length; i++)
		{
			if (this.m_handlers[i].CanHandle(_obj))
			{
				return this.m_handlers[i];
			}
		}
		return null;
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0006EA34 File Offset: 0x0006CE34
	public IEnumerator Unload(GameObject _obj, Vector3 _exitPosition, Quaternion _exitRotation)
	{
		if (_obj != null)
		{
			_obj.transform.position = _exitPosition;
			_obj.transform.rotation = _exitRotation;
			_obj.transform.SetParent(null, true);
		}
		this.m_cannon.EndCannonRoutine(_obj);
		IClientCannonHandler handler = this.GetHandler(_obj);
		if (handler != null)
		{
			IEnumerator exit = handler.ExitCannonRoutine(_obj, _exitPosition, _exitRotation);
			while (exit.MoveNext())
			{
				yield return null;
			}
		}
		if (this.m_onUnloadedCallback != null)
		{
			this.m_onUnloadedCallback(_obj);
		}
		yield break;
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x0006EA64 File Offset: 0x0006CE64
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		for (int i = 0; i < this.m_launches.Count; i++)
		{
			if (this.m_launches[i] == null || !this.m_launches[i].MoveNext())
			{
				this.m_launches.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0006EAC8 File Offset: 0x0006CEC8
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.m_message = (CannonMessage)serialisable;
		CannonMessage.CannonState state = this.m_message.m_state;
		if (state != CannonMessage.CannonState.Launched)
		{
			if (state != CannonMessage.CannonState.Load)
			{
				if (state == CannonMessage.CannonState.Unload)
				{
					this.m_launches.Add(this.Unload(this.m_message.m_loadedObject, this.m_cannon.m_exitPoint.position, this.m_cannon.m_exitPoint.rotation));
				}
			}
			else
			{
				this.Load(this.m_message.m_loadedObject);
			}
		}
		else
		{
			Vector3 eulerAngles = this.m_pilotRotation.m_transformToRotate.eulerAngles;
			eulerAngles.y = this.m_message.m_angle;
			this.m_pilotRotation.m_transformToRotate.eulerAngles = eulerAngles;
			this.m_launches.Add(this.LaunchProjectile(this.m_message.m_loadedObject, this.m_cannon.m_target));
		}
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0006EBC0 File Offset: 0x0006CFC0
	private IEnumerator LaunchProjectile(GameObject _objectToLaunch, Transform _target)
	{
		IClientCannonHandler handler = this.GetHandler(_objectToLaunch);
		if (handler != null)
		{
			handler.Launch(_objectToLaunch);
		}
		_objectToLaunch.transform.SetParent(null, true);
		if (this.m_onLaunchedCallback != null)
		{
			this.m_onLaunchedCallback(_objectToLaunch);
		}
		IEnumerator animation = this.m_cannon.m_animation.Run(_objectToLaunch, _target);
		while (animation.MoveNext())
		{
			yield return null;
		}
		if (handler != null)
		{
			handler.Land(_objectToLaunch);
		}
		this.m_cannon.EndCannonRoutine(_objectToLaunch);
		if (handler != null)
		{
			IEnumerator exit = handler.ExitCannonRoutine(_objectToLaunch, _target.position, _target.rotation);
			while (exit.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x0006EBE9 File Offset: 0x0006CFE9
	public void RegisterOnLoadedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onLoadedCallback = (VoidGeneric<GameObject>)Delegate.Combine(this.m_onLoadedCallback, _callback);
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x0006EC02 File Offset: 0x0006D002
	public void UnregisterOnLoadedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onLoadedCallback = (VoidGeneric<GameObject>)Delegate.Remove(this.m_onLoadedCallback, _callback);
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x0006EC1B File Offset: 0x0006D01B
	public void RegisterOnUnloadedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onUnloadedCallback = (VoidGeneric<GameObject>)Delegate.Combine(this.m_onUnloadedCallback, _callback);
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x0006EC34 File Offset: 0x0006D034
	public void UnregisterOnUnloadedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onUnloadedCallback = (VoidGeneric<GameObject>)Delegate.Remove(this.m_onUnloadedCallback, _callback);
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x0006EC4D File Offset: 0x0006D04D
	public void RegisterOnLaunchedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onLaunchedCallback = (VoidGeneric<GameObject>)Delegate.Combine(this.m_onLaunchedCallback, _callback);
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x0006EC66 File Offset: 0x0006D066
	public void UnregisterOnLaunchedCallback(VoidGeneric<GameObject> _callback)
	{
		this.m_onLaunchedCallback = (VoidGeneric<GameObject>)Delegate.Remove(this.m_onLaunchedCallback, _callback);
	}

	// Token: 0x04000FB9 RID: 4025
	private Cannon m_cannon;

	// Token: 0x04000FBA RID: 4026
	private ClientCannonPlayerHandler m_playerHandler;

	// Token: 0x04000FBB RID: 4027
	private PilotRotation m_pilotRotation;

	// Token: 0x04000FBC RID: 4028
	private CannonMessage m_message = new CannonMessage();

	// Token: 0x04000FBD RID: 4029
	private GameObject m_loadedObject;

	// Token: 0x04000FBE RID: 4030
	private Vector3 m_exitPosition;

	// Token: 0x04000FBF RID: 4031
	private Quaternion m_exitRotation;

	// Token: 0x04000FC0 RID: 4032
	private List<IEnumerator> m_launches = new List<IEnumerator>();

	// Token: 0x04000FC1 RID: 4033
	private IClientCannonHandler[] m_handlers;

	// Token: 0x04000FC2 RID: 4034
	private VoidGeneric<GameObject> m_onLoadedCallback;

	// Token: 0x04000FC3 RID: 4035
	private VoidGeneric<GameObject> m_onUnloadedCallback;

	// Token: 0x04000FC4 RID: 4036
	private VoidGeneric<GameObject> m_onLaunchedCallback;
}
