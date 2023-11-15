using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
public class CheatSequenceDetector : MonoBehaviour
{
	// Token: 0x060021AC RID: 8620 RVA: 0x000A2F24 File Offset: 0x000A1324
	private static void _AOT()
	{
		KeyValuePair<int, CheatSequenceDetector.Sequence> keyValuePair = default(KeyValuePair<int, CheatSequenceDetector.Sequence>);
		Generic<float, CheatSequenceDetector.Sequence> generic = (CheatSequenceDetector.Sequence s) => 0f;
		if (keyValuePair.Key == 0)
		{
		}
		if (generic != null)
		{
		}
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000A2F69 File Offset: 0x000A1369
	private void AvoidWarnings()
	{
		if (this.m_sequences != null)
		{
		}
		if (this.m_buttonRingBuffer != null)
		{
		}
		if (this.m_buttonRingBufferCaret == 0)
		{
		}
		if (this.m_buttons != null)
		{
		}
	}

	// Token: 0x040019D5 RID: 6613
	[SerializeField]
	private CheatSequenceDetector.Sequence[] m_sequences = new CheatSequenceDetector.Sequence[0];

	// Token: 0x040019D6 RID: 6614
	private ControlPadInput.Button[] m_buttonRingBuffer;

	// Token: 0x040019D7 RID: 6615
	private int m_buttonRingBufferCaret;

	// Token: 0x040019D8 RID: 6616
	private Dictionary<ControlPadInput.Button, ILogicalButton> m_buttons = new Dictionary<ControlPadInput.Button, ILogicalButton>();

	// Token: 0x020006F1 RID: 1777
	[Serializable]
	private class Sequence
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060021B0 RID: 8624 RVA: 0x000A2FF0 File Offset: 0x000A13F0
		// (remove) Token: 0x060021B1 RID: 8625 RVA: 0x000A3028 File Offset: 0x000A1428
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VoidGeneric<int> OnProgressChange = delegate(int _)
		{
		};

		// Token: 0x060021B2 RID: 8626 RVA: 0x000A305E File Offset: 0x000A145E
		public void SetProgressChange(int _value)
		{
			this.OnProgressChange(_value);
		}

		// Token: 0x040019DA RID: 6618
		public ControlPadInput.Button[] Definition = new ControlPadInput.Button[0];

		// Token: 0x040019DB RID: 6619
		public string Message = string.Empty;
	}
}
