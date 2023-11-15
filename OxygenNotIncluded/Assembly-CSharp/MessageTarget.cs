using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
	// Token: 0x06005B7F RID: 23423 RVA: 0x002190E4 File Offset: 0x002172E4
	public MessageTarget(KPrefabID prefab_id)
	{
		this.prefabId.Set(prefab_id);
		this.position = prefab_id.transform.GetPosition();
		this.name = "Unknown";
		KSelectable component = prefab_id.GetComponent<KSelectable>();
		if (component != null)
		{
			this.name = component.GetName();
		}
		prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x0021915E File Offset: 0x0021735E
	public Vector3 GetPosition()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetPosition();
		}
		return this.position;
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x0021918F File Offset: 0x0021738F
	public KSelectable GetSelectable()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetComponent<KSelectable>();
		}
		return null;
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x002191BB File Offset: 0x002173BB
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x002191C4 File Offset: 0x002173C4
	private void OnAbsorbedBy(object data)
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		this.prefabId.Set(component);
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x00219238 File Offset: 0x00217438
	public void OnCleanUp()
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
			this.prefabId.Set(null);
		}
	}

	// Token: 0x04003DAF RID: 15791
	[Serialize]
	private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

	// Token: 0x04003DB0 RID: 15792
	[Serialize]
	private Vector3 position;

	// Token: 0x04003DB1 RID: 15793
	[Serialize]
	private string name;
}
