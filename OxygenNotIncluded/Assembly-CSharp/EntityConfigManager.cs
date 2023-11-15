using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000794 RID: 1940
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
	// Token: 0x060035F8 RID: 13816 RVA: 0x001239FF File Offset: 0x00121BFF
	public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x00123A07 File Offset: 0x00121C07
	protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x00123A10 File Offset: 0x00121C10
	private static int GetSortOrder(Type type)
	{
		foreach (Attribute attribute in type.GetCustomAttributes(true))
		{
			if (attribute.GetType() == typeof(EntityConfigOrder))
			{
				return (attribute as EntityConfigOrder).sortOrder;
			}
		}
		return 0;
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x00123A60 File Offset: 0x00121C60
	public void LoadGeneratedEntities(List<Type> types)
	{
		Type typeFromHandle = typeof(IEntityConfig);
		Type typeFromHandle2 = typeof(IMultiEntityConfig);
		List<EntityConfigManager.ConfigEntry> list = new List<EntityConfigManager.ConfigEntry>();
		foreach (Type type in types)
		{
			if ((typeFromHandle.IsAssignableFrom(type) || typeFromHandle2.IsAssignableFrom(type)) && !type.IsAbstract && !type.IsInterface)
			{
				int sortOrder = EntityConfigManager.GetSortOrder(type);
				EntityConfigManager.ConfigEntry item = new EntityConfigManager.ConfigEntry
				{
					type = type,
					sortOrder = sortOrder
				};
				list.Add(item);
			}
		}
		list.Sort((EntityConfigManager.ConfigEntry x, EntityConfigManager.ConfigEntry y) => x.sortOrder.CompareTo(y.sortOrder));
		foreach (EntityConfigManager.ConfigEntry configEntry in list)
		{
			object obj = Activator.CreateInstance(configEntry.type);
			if (obj is IEntityConfig && DlcManager.IsDlcListValidForCurrentContent((obj as IEntityConfig).GetDlcIds()))
			{
				this.RegisterEntity(obj as IEntityConfig);
			}
			if (obj is IMultiEntityConfig)
			{
				this.RegisterEntities(obj as IMultiEntityConfig);
			}
		}
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x00123BC0 File Offset: 0x00121DC0
	public void RegisterEntity(IEntityConfig config)
	{
		KPrefabID component = config.CreatePrefab().GetComponent<KPrefabID>();
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x00123BF8 File Offset: 0x00121DF8
	public void RegisterEntities(IMultiEntityConfig config)
	{
		foreach (GameObject gameObject in config.CreatePrefabs())
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			component.prefabInitFn += config.OnPrefabInit;
			component.prefabSpawnFn += config.OnSpawn;
			Assets.AddPrefab(component);
		}
	}

	// Token: 0x040020EF RID: 8431
	public static EntityConfigManager Instance;

	// Token: 0x02001516 RID: 5398
	private struct ConfigEntry
	{
		// Token: 0x04006746 RID: 26438
		public Type type;

		// Token: 0x04006747 RID: 26439
		public int sortOrder;
	}
}
