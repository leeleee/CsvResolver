namespace CsvResolver
{
    /// <summary>
    /// 从 CSV 文件读取到的一个分词。
    /// </summary>
    public class CsvToken
    {
        /// <summary>
        /// 使用给定的分词类型和分词值，创建并初始化一个
        /// <see cref="CsvToken"/>对象实例。
        /// </summary>
        /// <param name="tokenType">
        /// 当前分词值的类型，参考 <see cref="CsvTokenType"/>。
        /// </param>
        /// <param name="value">当前分词的值。</param>
        public CsvToken(CsvTokenType tokenType, string value)
        {
            this.TokenType = tokenType;
            this.Value = value;
        }

        /// <summary>
        /// 设置或者获取当前分词值的类型，参考
        /// <see cref="CsvTokenType"/>。
        /// </summary>
        public CsvTokenType TokenType { get; set; }

        /// <summary>
        /// 设置或获取当前分词的值。
        /// </summary>
        public string Value { get; set; }
    }
}