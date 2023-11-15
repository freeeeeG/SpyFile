using System;

// Token: 0x02000359 RID: 857
public struct LocToken
{
	// Token: 0x06001062 RID: 4194 RVA: 0x0005E19C File Offset: 0x0005C59C
	public LocToken(string token, string replaceWith)
	{
		this.m_token = token.ToLowerInvariant();
		if (this.m_token.Length > 0)
		{
			if (this.m_token[0] != '[')
			{
				this.m_token = "[" + this.m_token;
			}
			if (this.m_token[this.m_token.Length - 1] != ']')
			{
				this.m_token += "]";
			}
		}
		this.m_value = replaceWith;
	}

	// Token: 0x04000C67 RID: 3175
	public string m_token;

	// Token: 0x04000C68 RID: 3176
	public string m_value;
}
