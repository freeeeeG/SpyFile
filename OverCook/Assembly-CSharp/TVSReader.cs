using System;
using System.Text;

// Token: 0x0200035C RID: 860
public class TVSReader
{
	// Token: 0x06001070 RID: 4208 RVA: 0x0005EC44 File Offset: 0x0005D044
	public TVSReader(byte[] bytes)
	{
		this.m_Data = bytes;
		this.m_Pos = 0;
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x0005EC5C File Offset: 0x0005D05C
	private string ReadLine()
	{
		string text = string.Empty;
		int i;
		for (i = this.m_Pos; i < this.m_Data.Length; i++)
		{
			if (this.m_Data[i] == 10 || this.m_Data[i] == 13)
			{
				break;
			}
		}
		text = Encoding.UTF8.GetString(this.m_Data, this.m_Pos, i - this.m_Pos);
		this.m_Pos = i + 1;
		if (this.m_Pos < this.m_Data.Length && (this.m_Data[this.m_Pos] == 10 || this.m_Data[this.m_Pos] == 13))
		{
			this.m_Pos++;
		}
		return text.Replace("\\n", "\n");
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0005ED34 File Offset: 0x0005D134
	public string[] ReadRow()
	{
		string[] result = null;
		char[] separator = new char[]
		{
			'\t'
		};
		if (this.m_Pos < this.m_Data.Length)
		{
			string text = this.ReadLine();
			result = text.Split(separator, StringSplitOptions.None);
		}
		return result;
	}

	// Token: 0x04000C83 RID: 3203
	private byte[] m_Data;

	// Token: 0x04000C84 RID: 3204
	private int m_Pos;
}
