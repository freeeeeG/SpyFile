using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public abstract class KitchenLoaderManager : Manager
{
	// Token: 0x0600181D RID: 6173 RVA: 0x0007978C File Offset: 0x00077B8C
	protected virtual void Awake()
	{
		KitchenLoaderManager.s_Instance = this;
		if (this.m_ceilingHeight.HasValue)
		{
			GameObject gameObject = GameObjectUtils.CreateOnParent(null, "Ceiling");
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			Transform transform = gameObject.transform;
			transform.localScale = new Vector3(50f, 1f, 50f);
			Camera main = Camera.main;
			transform.position = main.transform.position.WithY(this.m_ceilingHeight.Value + 0.5f);
		}
		this.m_MultiplayerController = GameUtils.RequestManagerInterface<MultiplayerController>();
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x0007982B File Offset: 0x00077C2B
	private void Update()
	{
		if (!this.m_bStarted && ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
		{
			this.m_MultiplayerController.StartKitchen();
			this.m_bStarted = true;
		}
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x0007985A File Offset: 0x00077C5A
	private void OnDestroy()
	{
		this.m_MultiplayerController.StopKitchen();
	}

	// Token: 0x06001820 RID: 6176
	public abstract void AssignChefEntities(FastList<User> users);

	// Token: 0x0400136A RID: 4970
	[SerializeField]
	private OptionalFloat m_ceilingHeight;

	// Token: 0x0400136B RID: 4971
	public static KitchenLoaderManager s_Instance;

	// Token: 0x0400136C RID: 4972
	private MultiplayerController m_MultiplayerController;

	// Token: 0x0400136D RID: 4973
	private bool m_bStarted;
}
