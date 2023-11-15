using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000A1 RID: 161
	[ExecuteInEditMode]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_unity_reference_helper.php")]
	public class UnityReferenceHelper : MonoBehaviour
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x0002D3B3 File Offset: 0x0002B5B3
		public string GetGUID()
		{
			return this.guid;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002D3BB File Offset: 0x0002B5BB
		public void Awake()
		{
			this.Reset();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002D3C4 File Offset: 0x0002B5C4
		public void Reset()
		{
			if (string.IsNullOrEmpty(this.guid))
			{
				this.guid = Guid.NewGuid().ToString();
				Debug.Log("Created new GUID - " + this.guid, this);
				return;
			}
			if (base.gameObject.scene.name != null)
			{
				foreach (UnityReferenceHelper unityReferenceHelper in Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[])
				{
					if (unityReferenceHelper != this && this.guid == unityReferenceHelper.guid)
					{
						this.guid = Guid.NewGuid().ToString();
						Debug.Log("Created new GUID - " + this.guid, this);
						return;
					}
				}
			}
		}

		// Token: 0x04000439 RID: 1081
		[HideInInspector]
		[SerializeField]
		private string guid;
	}
}
