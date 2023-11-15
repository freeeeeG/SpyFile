using System;
using InControl;
using UnityEngine;

namespace UI.TestingTool
{
	// Token: 0x02000413 RID: 1043
	public class ScrollByRightStick : MonoBehaviour
	{
		// Token: 0x060013CB RID: 5067 RVA: 0x0003C663 File Offset: 0x0003A863
		private void Update()
		{
			this._container.localPosition += new Vector3(0f, -InputManager.ActiveDevice.RightStickY.Value * 100f, 0f);
		}

		// Token: 0x040010CB RID: 4299
		[SerializeField]
		private Transform _container;
	}
}
