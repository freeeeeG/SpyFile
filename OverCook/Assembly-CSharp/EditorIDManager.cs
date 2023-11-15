using System;
using UnityEngine;

// Token: 0x02000BA0 RID: 2976
[ExecuteInEditMode]
public class EditorIDManager : Manager
{
	// Token: 0x06003CFE RID: 15614 RVA: 0x00123743 File Offset: 0x00121B43
	protected virtual void Awake()
	{
		if (this.m_idStore == null)
		{
			this.m_idStore = UnityEngine.Object.FindObjectOfType<EditorIDStore>();
		}
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00123761 File Offset: 0x00121B61
	public uint GetIDForObject(GameObject _object)
	{
		this.EnsureIDStoreAcquired();
		return this.m_idStore.GetIDForObject(_object);
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00123775 File Offset: 0x00121B75
	protected void EnsureIDStoreAcquired()
	{
		if (this.m_idStore == null)
		{
			this.m_idStore = UnityEngine.Object.FindObjectOfType<EditorIDStore>();
		}
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00123794 File Offset: 0x00121B94
	public static uint GetUniqueID(GameObject _object)
	{
		EditorIDManager editorIDManager = GameUtils.RequireManager<EditorIDManager>();
		if (editorIDManager == null)
		{
			return 0U;
		}
		return editorIDManager.GetIDForObject(_object);
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x001237BC File Offset: 0x00121BBC
	protected virtual void OnDestroy()
	{
		if (this.m_idStore != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_idStore);
		}
	}

	// Token: 0x04003112 RID: 12562
	[SerializeField]
	[ReadOnly]
	private EditorIDStore m_idStore;
}
