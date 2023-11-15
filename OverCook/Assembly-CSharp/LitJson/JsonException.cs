using System;

namespace LitJson
{
	// Token: 0x0200023E RID: 574
	public class JsonException : ApplicationException
	{
		// Token: 0x06000A08 RID: 2568 RVA: 0x00037396 File Offset: 0x00035796
		public JsonException()
		{
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003739E File Offset: 0x0003579E
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000373B6 File Offset: 0x000357B6
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000373CF File Offset: 0x000357CF
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000373E8 File Offset: 0x000357E8
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00037402 File Offset: 0x00035802
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003740B File Offset: 0x0003580B
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
