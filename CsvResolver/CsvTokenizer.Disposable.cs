using System;

namespace CsvResolver
{
    partial class CsvTokenizer : IDisposable
    {
        public void Dispose()
        {
            ((IDisposable)reader).Dispose();
        }
    }
}
