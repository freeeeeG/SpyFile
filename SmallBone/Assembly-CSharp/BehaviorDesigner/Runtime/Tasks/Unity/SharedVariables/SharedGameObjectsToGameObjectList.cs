using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001548 RID: 5448
	[TaskDescription("Sets the SharedGameObjectList values from the GameObjects. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SharedGameObjectsToGameObjectList : Action
	{
		// Token: 0x06006933 RID: 26931 RVA: 0x0012F199 File Offset: 0x0012D399
		public override void OnAwake()
		{
			this.storedGameObjectList.Value = new List<GameObject>();
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x0012F1AC File Offset: 0x0012D3AC
		public override TaskStatus OnUpdate()
		{
			if (this.gameObjects == null || this.gameObjects.Length == 0)
			{
				return TaskStatus.Failure;
			}
			this.storedGameObjectList.Value.Clear();
			for (int i = 0; i < this.gameObjects.Length; i++)
			{
				this.storedGameObjectList.Value.Add(this.gameObjects[i].Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x0012F20D File Offset: 0x0012D40D
		public override void OnReset()
		{
			this.gameObjects = null;
			this.storedGameObjectList = null;
		}

		// Token: 0x04005502 RID: 21762
		[Tooltip("The GameObjects value")]
		public SharedGameObject[] gameObjects;

		// Token: 0x04005503 RID: 21763
		[Tooltip("The SharedTransformList to set")]
		[RequiredField]
		public SharedGameObjectList storedGameObjectList;
	}
}
