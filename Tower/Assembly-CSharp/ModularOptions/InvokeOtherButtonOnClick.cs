using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200007E RID: 126
	[AddComponentMenu("Modular Options/Button/Invoke Other Button")]
	[RequireComponent(typeof(Button))]
	public class InvokeOtherButtonOnClick : MonoBehaviour
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00008398 File Offset: 0x00006598
		private void Awake()
		{
			if (this.buttonToInvoke.gameObject == base.gameObject)
			{
				Debug.LogWarning("Invocation loop prevented. Don't reference a button on the same GameObject.", this);
				return;
			}
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.buttonToInvoke.onClick.Invoke();
			});
		}

		// Token: 0x040001B3 RID: 435
		public Button buttonToInvoke;
	}
}
