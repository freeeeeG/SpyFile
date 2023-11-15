using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E04 RID: 3588
	public class ModifierInstance<ModifierType> : IStateMachineTarget
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06006E10 RID: 28176 RVA: 0x002B5E54 File Offset: 0x002B4054
		// (set) Token: 0x06006E11 RID: 28177 RVA: 0x002B5E5C File Offset: 0x002B405C
		public GameObject gameObject { get; private set; }

		// Token: 0x06006E12 RID: 28178 RVA: 0x002B5E65 File Offset: 0x002B4065
		public ModifierInstance(GameObject game_object, ModifierType modifier)
		{
			this.gameObject = game_object;
			this.modifier = modifier;
		}

		// Token: 0x06006E13 RID: 28179 RVA: 0x002B5E7B File Offset: 0x002B407B
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x002B5E88 File Offset: 0x002B4088
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x002B5E9C File Offset: 0x002B409C
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x002B5EB0 File Offset: 0x002B40B0
		public void Unsubscribe(int id)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x002B5EC3 File Offset: 0x002B40C3
		public void Trigger(int hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06006E18 RID: 28184 RVA: 0x002B5ED7 File Offset: 0x002B40D7
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06006E19 RID: 28185 RVA: 0x002B5EE4 File Offset: 0x002B40E4
		public bool isNull
		{
			get
			{
				return this.gameObject == null;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06006E1A RID: 28186 RVA: 0x002B5EF2 File Offset: 0x002B40F2
		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x002B5EFF File Offset: 0x002B40FF
		public virtual void OnCleanUp()
		{
		}

		// Token: 0x04005289 RID: 21129
		public ModifierType modifier;
	}
}
