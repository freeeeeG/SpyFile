using System;

namespace flanne
{
	// Token: 0x0200006D RID: 109
	public class BoolToggle
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060004A6 RID: 1190 RVA: 0x00017A10 File Offset: 0x00015C10
		// (remove) Token: 0x060004A7 RID: 1191 RVA: 0x00017A48 File Offset: 0x00015C48
		public event EventHandler<bool> ToggleEvent;

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00017A7D File Offset: 0x00015C7D
		public bool value
		{
			get
			{
				if (this._flip > 0)
				{
					return !this.defaultValue;
				}
				return this.defaultValue;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00017A98 File Offset: 0x00015C98
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00017AA0 File Offset: 0x00015CA0
		public bool defaultValue { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00017AA9 File Offset: 0x00015CA9
		public int flips
		{
			get
			{
				return this._flip;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00017AB1 File Offset: 0x00015CB1
		public BoolToggle(bool b)
		{
			this.defaultValue = b;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00017AC0 File Offset: 0x00015CC0
		public void Flip()
		{
			this._flip++;
			if (this.ToggleEvent != null)
			{
				this.ToggleEvent(this, this.value);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00017AEA File Offset: 0x00015CEA
		public void UnFlip()
		{
			this._flip--;
			if (this.ToggleEvent != null)
			{
				this.ToggleEvent(this, this.value);
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00017AA9 File Offset: 0x00015CA9
		public int GetFlipAmount()
		{
			return this._flip;
		}

		// Token: 0x040002A4 RID: 676
		private int _flip;
	}
}
