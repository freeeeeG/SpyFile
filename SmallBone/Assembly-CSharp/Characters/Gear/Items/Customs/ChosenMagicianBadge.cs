using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Items.Customs
{
	// Token: 0x02000902 RID: 2306
	public sealed class ChosenMagicianBadge : MonoBehaviour
	{
		// Token: 0x0600312D RID: 12589 RVA: 0x000933F4 File Offset: 0x000915F4
		private void Start()
		{
			float num = 0f;
			ChosenMagicianBadgeOrb[] orbs = this._orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].Initialize(num);
				num += 6.2831855f / (float)this._orbs.Length;
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x00093437 File Offset: 0x00091637
		private void OnEnable()
		{
			this._item.owner.onStartAction += this.HandleOnStartAction;
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x00093455 File Offset: 0x00091655
		private void OnDisable()
		{
			this._item.owner.onStartAction -= this.HandleOnStartAction;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x00093474 File Offset: 0x00091674
		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (action.type != Characters.Actions.Action.Type.Skill)
			{
				return;
			}
			this._fast = true;
			this._remainTime = this._fastTime;
			ChosenMagicianBadgeOrb[] orbs = this._orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].ChangeToFast();
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x0001913A File Offset: 0x0001733A
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000934BC File Offset: 0x000916BC
		private void Update()
		{
			this._remainTime -= Chronometer.global.deltaTime;
			base.transform.Rotate(Vector3.forward, (this._fast ? this._fastSpeed : this._normalSpeed) * Chronometer.global.deltaTime, Space.Self);
			if (this._remainTime <= 0f && this._fast)
			{
				this._fast = false;
				ChosenMagicianBadgeOrb[] orbs = this._orbs;
				for (int i = 0; i < orbs.Length; i++)
				{
					orbs[i].ChangeToNormal();
				}
			}
		}

		// Token: 0x0400287A RID: 10362
		[SerializeField]
		private Item _item;

		// Token: 0x0400287B RID: 10363
		[SerializeField]
		private float _radius = 10f;

		// Token: 0x0400287C RID: 10364
		[SerializeField]
		private float _fastTime;

		// Token: 0x0400287D RID: 10365
		[SerializeField]
		private float _normalSpeed = 100f;

		// Token: 0x0400287E RID: 10366
		[SerializeField]
		private float _fastSpeed = 300f;

		// Token: 0x0400287F RID: 10367
		[SerializeField]
		private ChosenMagicianBadgeOrb[] _orbs;

		// Token: 0x04002880 RID: 10368
		private float _remainTime;

		// Token: 0x04002881 RID: 10369
		private bool _fast;
	}
}
