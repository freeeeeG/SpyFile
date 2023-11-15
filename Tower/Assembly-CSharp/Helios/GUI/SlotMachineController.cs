using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000E6 RID: 230
	public class SlotMachineController : MonoBehaviour
	{
		// Token: 0x06000356 RID: 854 RVA: 0x0000F017 File Offset: 0x0000D217
		private void OnEnable()
		{
			this._btnTurnSlotMachine.onClick.AddListener(new UnityAction(this.TurnSlotMachine));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F035 File Offset: 0x0000D235
		private void OnDisable()
		{
			this._btnTurnSlotMachine.onClick.RemoveAllListeners();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F048 File Offset: 0x0000D248
		private void TurnSlotMachine()
		{
			this._btnTurnSlotMachine.targetGraphic.gameObject.SetActive(false);
			this._imgTurnDownHandle.gameObject.SetActive(true);
			Animator[] animators = this._animators;
			for (int i = 0; i < animators.Length; i++)
			{
				animators[i].SetTrigger("Start");
			}
			float num = this._nbAnimationTime / 3f;
			base.Invoke("StopLeftSlotAnimation", num);
			base.Invoke("StopMiddleSlotAnimation", num * 2f);
			base.Invoke("StopRightSlotAnimation", this._nbAnimationTime);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000F0DA File Offset: 0x0000D2DA
		private void StopLeftSlotAnimation()
		{
			this._animators[0].SetTrigger("Stop");
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000F0EE File Offset: 0x0000D2EE
		private void StopMiddleSlotAnimation()
		{
			this._animators[1].SetTrigger("Stop");
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F102 File Offset: 0x0000D302
		private void StopRightSlotAnimation()
		{
			this._animators[2].SetTrigger("Stop");
			this._btnTurnSlotMachine.targetGraphic.gameObject.SetActive(true);
			this._imgTurnDownHandle.gameObject.SetActive(false);
		}

		// Token: 0x04000328 RID: 808
		[SerializeField]
		private Animator[] _animators;

		// Token: 0x04000329 RID: 809
		[SerializeField]
		private float _nbAnimationTime = 15f;

		// Token: 0x0400032A RID: 810
		[SerializeField]
		private Button _btnTurnSlotMachine;

		// Token: 0x0400032B RID: 811
		[SerializeField]
		private Image _imgTurnDownHandle;
	}
}
