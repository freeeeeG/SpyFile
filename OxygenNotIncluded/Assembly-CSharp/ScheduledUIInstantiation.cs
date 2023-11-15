using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BEA RID: 3050
[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
	// Token: 0x06006080 RID: 24704 RVA: 0x0023AAB3 File Offset: 0x00238CB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.InstantiateOnAwake)
		{
			this.InstantiateElements(null);
			return;
		}
		Game.Instance.Subscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
	}

	// Token: 0x06006081 RID: 24705 RVA: 0x0023AAE8 File Offset: 0x00238CE8
	public void InstantiateElements(object data)
	{
		if (this.completed)
		{
			return;
		}
		this.completed = true;
		foreach (ScheduledUIInstantiation.Instantiation instantiation in this.UIElements)
		{
			foreach (GameObject gameObject in instantiation.prefabs)
			{
				if (DlcManager.IsContentActive(instantiation.RequiredDlcId))
				{
					Vector3 v = gameObject.rectTransform().anchoredPosition;
					GameObject gameObject2 = Util.KInstantiateUI(gameObject, instantiation.parent.gameObject, false);
					gameObject2.rectTransform().anchoredPosition = v;
					gameObject2.rectTransform().localScale = Vector3.one;
					this.instantiatedObjects.Add(gameObject2);
				}
			}
		}
		if (!this.InstantiateOnAwake)
		{
			base.Unsubscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
		}
	}

	// Token: 0x06006082 RID: 24706 RVA: 0x0023ABD0 File Offset: 0x00238DD0
	public T GetInstantiatedObject<T>() where T : Component
	{
		for (int i = 0; i < this.instantiatedObjects.Count; i++)
		{
			if (this.instantiatedObjects[i].GetComponent(typeof(T)) != null)
			{
				return this.instantiatedObjects[i].GetComponent(typeof(T)) as T;
			}
		}
		return default(T);
	}

	// Token: 0x040041BA RID: 16826
	public ScheduledUIInstantiation.Instantiation[] UIElements;

	// Token: 0x040041BB RID: 16827
	public bool InstantiateOnAwake;

	// Token: 0x040041BC RID: 16828
	public GameHashes InstantiationEvent = GameHashes.StartGameUser;

	// Token: 0x040041BD RID: 16829
	private bool completed;

	// Token: 0x040041BE RID: 16830
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Token: 0x02001B46 RID: 6982
	[Serializable]
	public struct Instantiation
	{
		// Token: 0x04007C52 RID: 31826
		public string Name;

		// Token: 0x04007C53 RID: 31827
		public string Comment;

		// Token: 0x04007C54 RID: 31828
		public GameObject[] prefabs;

		// Token: 0x04007C55 RID: 31829
		public Transform parent;

		// Token: 0x04007C56 RID: 31830
		public string RequiredDlcId;
	}
}
