using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FD RID: 509
[AddComponentMenu("Scripts/Core/Input/DebugInputDisplay")]
public class DebugInputDisplay : MonoBehaviour
{
	// Token: 0x06000890 RID: 2192 RVA: 0x00034284 File Offset: 0x00032684
	private void OnGUI()
	{
		if (!this.m_displayDebug)
		{
			return;
		}
		Rect rect = new Rect(0f, 0f, 100f, 22f);
		int num = (this.m_inputProvider != DebugInputDisplay.InputProvider.DirectInput) ? ((this.m_inputProvider != DebugInputDisplay.InputProvider.NX) ? 4 : 9) : 11;
		for (int i = Mathf.Min(this.m_startIndex, num - 1); i < num; i++)
		{
			ControlPadInput.PadNum pad = (ControlPadInput.PadNum)i;
			if (this.m_inputProvider != DebugInputDisplay.InputProvider.NX)
			{
				this.PrintButtons<ControlPadInput.Button>(new ControlPadInput.Button?(ControlPadInput.Button.Invalid), pad, ref rect);
				this.PrintValues<ControlPadInput.Value>(new ControlPadInput.Value?(ControlPadInput.Value.Invalid), pad, ref rect);
			}
			rect.x += rect.width;
			rect.y = 0f;
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00034350 File Offset: 0x00032750
	private void PrintButtons<T>(T? ignore, ControlPadInput.PadNum pad, ref Rect rect) where T : struct, IComparable
	{
		IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				T t = (T)((object)obj);
				if (ignore != null)
				{
					T value = ignore.Value;
					if (value.Equals(t))
					{
						continue;
					}
				}
				bool flag = this.IsDown<T>(pad, t);
				GUI.TextField(rect, string.Format("{0}:{1}", t.ToString(), flag));
				rect.y += rect.height;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0003442C File Offset: 0x0003282C
	private void PrintValues<T>(T? ignore, ControlPadInput.PadNum pad, ref Rect rect) where T : struct, IComparable
	{
		IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				T t = (T)((object)obj);
				if (ignore != null)
				{
					T value = ignore.Value;
					if (value.Equals(t))
					{
						continue;
					}
				}
				float value2 = this.GetValue<T>(pad, t);
				GUI.TextField(rect, string.Format("{0}:{1}", t.ToString(), value2));
				rect.y += rect.height;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00034508 File Offset: 0x00032908
	private bool IsDown<T>(ControlPadInput.PadNum _pad, T _button)
	{
		if (typeof(T) != typeof(ControlPadInput.Button))
		{
			return false;
		}
		ControlPadInput.Button button = (ControlPadInput.Button)((object)_button);
		DebugInputDisplay.InputProvider inputProvider = this.m_inputProvider;
		if (inputProvider != DebugInputDisplay.InputProvider.DirectInput)
		{
			return inputProvider == DebugInputDisplay.InputProvider.XInput && Singleton<XInputProvider>.Get().IsDown(_pad, button);
		}
		return Singleton<DirectInputProvider>.Get().IsDown(_pad, button);
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00034570 File Offset: 0x00032970
	private float GetValue<T>(ControlPadInput.PadNum _pad, T _value)
	{
		if (typeof(T) != typeof(ControlPadInput.Value))
		{
			return 0f;
		}
		ControlPadInput.Value value = (ControlPadInput.Value)((object)_value);
		DebugInputDisplay.InputProvider inputProvider = this.m_inputProvider;
		if (inputProvider == DebugInputDisplay.InputProvider.DirectInput)
		{
			return Singleton<DirectInputProvider>.Get().GetValue(_pad, value);
		}
		if (inputProvider != DebugInputDisplay.InputProvider.XInput)
		{
			return 0f;
		}
		return Singleton<XInputProvider>.Get().GetValue(_pad, value);
	}

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private bool m_displayDebug;

	// Token: 0x0400078D RID: 1933
	[SerializeField]
	private int m_startIndex;

	// Token: 0x0400078E RID: 1934
	[SerializeField]
	private DebugInputDisplay.InputProvider m_inputProvider;

	// Token: 0x020001FE RID: 510
	private enum InputProvider
	{
		// Token: 0x04000790 RID: 1936
		DirectInput,
		// Token: 0x04000791 RID: 1937
		XInput,
		// Token: 0x04000792 RID: 1938
		NX
	}
}
