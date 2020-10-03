namespace CsvResolver
{
    /// <summary>
    /// 从 CSV 文件中读取到的分词类型。
    /// </summary>
    public enum CsvTokenType : byte
    {
        /// <summary>
        /// 默认值，通常标志着还未读取。
        /// </summary>
        Unknow,
        /// <summary>
        /// 读取到一个字段。
        /// </summary>
        Record,
        /// <summary>
        /// 标志着读取到了一条记录的最后一个字段。
        /// </summary>
        EndRecord,
        /// <summary>
        /// 标志着读取到了文件的结尾。
        /// </summary>
        Eof,
    }
}