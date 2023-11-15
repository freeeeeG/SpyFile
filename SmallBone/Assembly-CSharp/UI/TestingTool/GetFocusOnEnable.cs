using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x02000404 RID: 1028
	[RequireComponent(typeof(Selectable))]
	public class GetFocusOnEnable : MonoBehaviour
	{
		// Token: 0x06001367 RID: 4967 RVA: 0x0003AB00 File Offset: 0x00038D00
		private void OnEnable()
		{
			base.StartCoroutine(this.GetFocus());
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0003AB0F File Offset: 0x00038D0F
		private IEnumerator GetFocus()
		{
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			EventSystem.current.SetSelectedGameObject(base.gameObject);
			Selectable component = base.GetComponent<Selectable>();
			typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component, new object[]
			{
				3,
				true
			});
			yield break;
		}
	}
}
