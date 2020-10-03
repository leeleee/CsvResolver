using System;
using System.IO;
using System.Text;

namespace CsvResolver
{
/// <summary>
/// 将字符流转换为标记流，只支持向前读取。
/// </summary>
public partial class CsvTokenizer
{
    private readonly TextReader reader;

    /// <summary>
    /// 使用指定的字符流读取器，创建并初始化一个
    /// <see cref="CsvTokenizer"/>对象实例。
    /// </summary>
    /// <param name="reader">字符流读取器。</param>
    public CsvTokenizer(TextReader reader)
    {
        this.reader = reader
            ?? throw new ArgumentNullException(nameof(reader));
    }

    /// <summary>
    /// 从构造传入的字符输入流，读取下一个记录。
    /// </summary>
    /// <returns>下一个记录信息。</returns>
    public CsvToken NextToken()
    {
        int ch;
        StringBuilder buff = new StringBuilder();
        CsvTokenType tokenType = CsvTokenType.Unknow;
        while ((ch = reader.Read()) != -1)
        {
            switch (ch)
            {
                case ',':
                    tokenType = CsvTokenType.Record;
                    goto ret;
                case '"':
                    if (buff.Length <= 0)
                    {
                        this.ReadString(buff);
                        continue;
                    }
                    break;
                case '\r':
                    if (reader.Peek() == '\n')
                    {
                        // skip '\n'
                        reader.Read();
                        tokenType = CsvTokenType.EndRecord;
                        goto ret;
                    }
                    break;
                default:
                    break;
            }
            buff.Append((char)ch);
        }

        if (ch == -1)
        {
            tokenType = CsvTokenType.Eof;
        }
    ret:
        return new CsvToken(tokenType, buff.ToString());
    }

    private void ReadString(StringBuilder buff)
    {
        int ch;
        while ((ch = reader.Read()) != -1)
        {
            switch (ch)
            {
                case '"':
                    if (reader.Peek() == '"')
                    {
                        // skip next double-qoutes
                        reader.Read();
                        break;
                    }
                    return;
                default:
                    break;
            }
            buff.Append((char)ch);
        }
    }
}
}
