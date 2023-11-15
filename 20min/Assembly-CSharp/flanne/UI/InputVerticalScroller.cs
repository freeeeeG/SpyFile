using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000225 RID: 549
	[RequireComponent(typeof(Scrollbar))]
	public class InputVerticalScroller : MonoBehaviour
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x0002CB2B File Offset: 0x0002AD2B
		private void OnInput(InputAction.CallbackContext context)
		{
			this.scrollVec = context.ReadValue<Vector2>().y;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002CB3F File Offset: 0x0002AD3F
		private void Start()
		{
			this.scrollbar = base.GetComponent<Scrollbar>();
			this.inputs.FindAction("UI/Move", false).performed += this.OnInput;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0002CB6F File Offset: 0x0002AD6F
		private void OnDestroy()
		{
			this.inputs.FindAction("UI/Move", false).performed -= this.OnInput;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0002CB94 File Offset: 0x0002AD94
		private void Update()
		{
			float num = this.scrollVec * this.scrollSpeed * Time.unscaledDeltaTime;
			this.scrollbar.value += num;
		}

		// Token: 0x0400087E RID: 2174
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x0400087F RID: 2175
		[SerializeField]
		private float scrollSpeed = 1f;

		// Token: 0x04000880 RID: 2176
		private Scrollbar scrollbar;

		// Token: 0x04000881 RID: 2177
		private float scrollVec;
	}
}
