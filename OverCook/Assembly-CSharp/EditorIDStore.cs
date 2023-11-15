using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BA1 RID: 2977
public class EditorIDStore : MonoBehaviour
{
	// Token: 0x06003D04 RID: 15620 RVA: 0x001237F0 File Offset: 0x00121BF0
	public uint GetIDForObject(GameObject _object)
	{
		uint result = 0U;
		EditorIDStore.IDMap idmap = this.m_allAssignedIDs.Find((EditorIDStore.IDMap x) => x.gameObject == _object);
		if (idmap != null)
		{
			result = idmap.id;
		}
		return result;
	}

	// Token: 0x04003113 RID: 12563
	[SerializeField]
	private List<EditorIDStore.IDMap> m_allAssignedIDs = new List<EditorIDStore.IDMap>();

	// Token: 0x04003114 RID: 12564
	[SerializeField]
	private uint m_maxAssignedID;

	// Token: 0x02000BA2 RID: 2978
	[Serializable]
	private class IDMap
	{
		// Token: 0x06003D05 RID: 15621 RVA: 0x00123832 File Offset: 0x00121C32
		public IDMap(GameObject _object, uint _id)
		{
			this.gameObject = _object;
			this.id = _id;
		}

		// Token: 0x04003115 RID: 12565
		public uint id;

		// Token: 0x04003116 RID: 12566
		public GameObject gameObject;
	}
}
