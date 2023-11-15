using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class ServerAttachmentCatchingProxy : ServerSynchroniserBase
{
	// Token: 0x0600135E RID: 4958 RVA: 0x0006C180 File Offset: 0x0006A580
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_iHandleCatches = base.gameObject.RequestInterfaces<IHandleCatch>();
		this.m_triggerRecorder = base.gameObject.RequestComponent<TriggerRecorder>();
		if (this.m_triggerRecorder == null)
		{
			this.m_triggerRecorder = base.gameObject.AddComponent<TriggerRecorder>();
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0006C1D8 File Offset: 0x0006A5D8
	public void RegisterUncatchableItemCallback(GenericVoid<GameObject, Vector2> _callback)
	{
		this.m_uncatchableItemCallback = (GenericVoid<GameObject, Vector2>)Delegate.Combine(this.m_uncatchableItemCallback, _callback);
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x0006C1F1 File Offset: 0x0006A5F1
	public void UnRegisterUncatchableItemCallback(GenericVoid<GameObject, Vector2> _callback)
	{
		this.m_uncatchableItemCallback = (GenericVoid<GameObject, Vector2>)Delegate.Remove(this.m_uncatchableItemCallback, _callback);
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0006C20C File Offset: 0x0006A60C
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_triggerRecorder != null)
		{
			List<Collider> recentCollisions = this.m_triggerRecorder.GetRecentCollisions();
			for (int i = 0; i < recentCollisions.Count; i++)
			{
				if (!(recentCollisions[i] == null) && recentCollisions[i].enabled && !(recentCollisions[i].gameObject == null) && recentCollisions[i].gameObject.activeSelf)
				{
					IAttachment component = recentCollisions[i].gameObject.GetComponent<IAttachment>();
					if (component != null && !component.IsAttached())
					{
						this.AttemptToCatch(component.AccessGameObject());
					}
				}
			}
		}
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x0006C2D8 File Offset: 0x0006A6D8
	private void AttemptToCatch(GameObject _object)
	{
		ICatchable catchable = _object.RequestInterface<ICatchable>();
		if (catchable != null)
		{
			IHandleCatch controllingCatchingHandler = this.GetControllingCatchingHandler();
			if (controllingCatchingHandler != null)
			{
				Vector2 directionXZ = (_object.transform.position - base.transform.position).SafeNormalised(base.transform.forward).XZ();
				if (controllingCatchingHandler.CanHandleCatch(catchable, directionXZ))
				{
					controllingCatchingHandler.HandleCatch(catchable, directionXZ);
				}
			}
		}
		else
		{
			Vector2 param = (_object.transform.position - base.transform.position).SafeNormalised(base.transform.forward).XZ();
			if (this.m_uncatchableItemCallback != null)
			{
				this.m_uncatchableItemCallback(_object, param);
			}
		}
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0006C398 File Offset: 0x0006A798
	public void SetHandleCatchingReferree(IHandleCatch _iHandleCatch)
	{
		this.m_iHandleCatchReferree = _iHandleCatch;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0006C3A1 File Offset: 0x0006A7A1
	public IHandleCatch GetHandleCatchingReferree()
	{
		return this.m_iHandleCatchReferree;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0006C3AC File Offset: 0x0006A7AC
	public IHandleCatch GetControllingCatchingHandler()
	{
		if (this.m_iHandleCatchReferree != null)
		{
			return this.m_iHandleCatchReferree;
		}
		IHandleCatch handleCatch = null;
		for (int i = 0; i < this.m_iHandleCatches.Length; i++)
		{
			IHandleCatch handleCatch2 = this.m_iHandleCatches[i];
			if ((!(handleCatch2 is MonoBehaviour) || (handleCatch2 as MonoBehaviour).enabled) && (handleCatch == null || handleCatch2.GetCatchingPriority() > handleCatch.GetCatchingPriority()))
			{
				handleCatch = handleCatch2;
			}
		}
		return handleCatch;
	}

	// Token: 0x04000F3D RID: 3901
	private TriggerRecorder m_triggerRecorder;

	// Token: 0x04000F3E RID: 3902
	private IHandleCatch[] m_iHandleCatches = new IHandleCatch[0];

	// Token: 0x04000F3F RID: 3903
	private IHandleCatch m_iHandleCatchReferree;

	// Token: 0x04000F40 RID: 3904
	private GenericVoid<GameObject, Vector2> m_uncatchableItemCallback = delegate(GameObject _object, Vector2 _directionXZ)
	{
	};
}
