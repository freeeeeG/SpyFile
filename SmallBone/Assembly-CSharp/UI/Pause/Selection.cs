using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x02000427 RID: 1063
	public class Selection : Selectable
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600141B RID: 5147 RVA: 0x0003D9A4 File Offset: 0x0003BBA4
		// (remove) Token: 0x0600141C RID: 5148 RVA: 0x0003D9DC File Offset: 0x0003BBDC
		public event Action<int> onValueChanged;

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0003DA11 File Offset: 0x0003BC11
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x0003DA19 File Offset: 0x0003BC19
		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this.SetValueWithoutNotify(value);
				Action<int> action = this.onValueChanged;
				if (action == null)
				{
					return;
				}
				action(this._value);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x0003DA38 File Offset: 0x0003BC38
		public string text
		{
			get
			{
				return this._texts[this.value];
			}
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0003DA4B File Offset: 0x0003BC4B
		protected override void Awake()
		{
			base.Awake();
			this._left.onPointerDown = new Action(this.MoveLeft);
			this._right.onPointerDown = new Action(this.MoveRight);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0003DA84 File Offset: 0x0003BC84
		private void ValidateValue()
		{
			if (this._value < 0)
			{
				this._value = this._texts.Count - 1;
				return;
			}
			if (this._value >= this._texts.Count)
			{
				this._value %= this._texts.Count;
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0003DAD9 File Offset: 0x0003BCD9
		public void SetTexts(IList<string> texts)
		{
			this._texts = texts;
			this.ValidateValue();
			this._text.text = this._texts[this._value];
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0003DB04 File Offset: 0x0003BD04
		public void SetValueWithoutNotify(int value)
		{
			this._value = value;
			this.ValidateValue();
			this._text.text = this._texts[this._value];
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0003DB30 File Offset: 0x0003BD30
		public override void OnMove(AxisEventData eventData)
		{
			MoveDirection moveDir = eventData.moveDir;
			if (moveDir == MoveDirection.Left)
			{
				this.MoveLeft();
				return;
			}
			if (moveDir != MoveDirection.Right)
			{
				base.OnMove(eventData);
				return;
			}
			this.MoveRight();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0003DB64 File Offset: 0x0003BD64
		public void MoveLeft()
		{
			int value = this.value;
			this.value = value - 1;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0003DB84 File Offset: 0x0003BD84
		public void MoveRight()
		{
			int value = this.value;
			this.value = value + 1;
		}

		// Token: 0x04001118 RID: 4376
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04001119 RID: 4377
		[SerializeField]
		private PointerDownHandler _left;

		// Token: 0x0400111A RID: 4378
		[SerializeField]
		private PointerDownHandler _right;

		// Token: 0x0400111B RID: 4379
		private IList<string> _texts;

		// Token: 0x0400111C RID: 4380
		private int _value;
	}
}
