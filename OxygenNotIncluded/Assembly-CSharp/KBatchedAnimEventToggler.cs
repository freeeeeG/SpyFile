using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044D RID: 1101
[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
	// Token: 0x06001805 RID: 6149 RVA: 0x0007BFB4 File Offset: 0x0007A1B4
	protected override void OnPrefabInit()
	{
		Vector3 position = this.eventSource.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		int layer = LayerMask.NameToLayer("Default");
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.transform.SetPosition(position);
			entry.controller.SetLayer(layer);
			entry.controller.gameObject.SetActive(false);
		}
		int hash = Hash.SDBMLower(this.enableEvent);
		int hash2 = Hash.SDBMLower(this.disableEvent);
		base.Subscribe(this.eventSource, hash, new Action<object>(this.Enable));
		base.Subscribe(this.eventSource, hash2, new Action<object>(this.Disable));
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x0007C0A4 File Offset: 0x0007A2A4
	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0007C0B4 File Offset: 0x0007A2B4
	private void Enable(object data)
	{
		this.StopAll();
		HashedString context = this.animEventHandler.GetContext();
		if (!context.IsValid)
		{
			return;
		}
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			if (entry.context == context)
			{
				entry.controller.gameObject.SetActive(true);
				entry.controller.Play(entry.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0007C15C File Offset: 0x0007A35C
	private void Disable(object data)
	{
		this.StopAll();
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0007C164 File Offset: 0x0007A364
	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000D3C RID: 3388
	[SerializeField]
	public GameObject eventSource;

	// Token: 0x04000D3D RID: 3389
	[SerializeField]
	public string enableEvent;

	// Token: 0x04000D3E RID: 3390
	[SerializeField]
	public string disableEvent;

	// Token: 0x04000D3F RID: 3391
	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	// Token: 0x04000D40 RID: 3392
	private AnimEventHandler animEventHandler;

	// Token: 0x020010D7 RID: 4311
	[Serializable]
	public struct Entry
	{
		// Token: 0x04005A3B RID: 23099
		public string anim;

		// Token: 0x04005A3C RID: 23100
		public HashedString context;

		// Token: 0x04005A3D RID: 23101
		public KBatchedAnimController controller;
	}
}
