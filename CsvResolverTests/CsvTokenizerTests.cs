using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvResolver.Tests
{
    [TestClass()]
    public class CsvTokenizerTests
    {
        [TestMethod()]
        public void CsvTokenizerTest_ConstructorArgumentNull()
        {
            ArgumentNullException ane = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new CsvTokenizer(null);
            });
        }

        [TestMethod]
        public void CsvTokenizerTest_Disposed()
        {
            StringReader reader = new StringReader("");
            CsvTokenizer tokenizer = new CsvTokenizer(reader);

            tokenizer.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                reader.Read();
            });

            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                tokenizer.NextToken();
            });
        }

        [TestMethod]
        public void CsvTokenizerTest_OneLineNormal()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader(@"aaa,bbb,ccc"));
            List<string> records = new List<string>();

            CsvToken token;
            while ((token = tokenizer.NextToken()).TokenType != CsvTokenType.Eof)
            {
                records.Add(token.Value);
            }
            records.Add(token.Value);

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("aaa", records[0]);
            Assert.AreEqual("bbb", records[1]);
            Assert.AreEqual("ccc", records[2]);
        }

        [TestMethod]
        public void CsvTokenizerTest_DoubleQouteOnRecord()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader("aaa\""));

            CsvToken token = tokenizer.NextToken();

            Assert.AreEqual(CsvTokenType.Eof, token.TokenType);
            Assert.AreEqual("aaa\"", token.Value);
        }

        [TestMethod]
        public void CsvTokenizerTest_DoubleQouteInDoubleQoutedValues()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader("aaa\",\"b\"\"\",ccc"));
            List<string> records = new List<string>();

            CsvToken token;
            while ((token = tokenizer.NextToken()).TokenType != CsvTokenType.Eof)
            {
                records.Add(token.Value);
            }
            records.Add(token.Value);

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("aaa\"", records[0]);
            Assert.AreEqual("b\"", records[1]);
            Assert.AreEqual("ccc", records[2]);
        }


        [TestMethod]
        public void CsvTokenizerTest_LRLFInDoubleQoutedValues()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader("aaa\",\"b\r\n\"\"\",ccc"));
            List<string> records = new List<string>();

            CsvToken token;
            while ((token = tokenizer.NextToken()).TokenType != CsvTokenType.Eof)
            {
                records.Add(token.Value);
            }
            records.Add(token.Value);

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("aaa\"", records[0]);
            Assert.AreEqual("b\r\n\"", records[1]);
            Assert.AreEqual("ccc", records[2]);
        }

        [TestMethod]
        public void CsvTokenizerTest_CommaInDoubleQoutedValues()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader("aaa\",\"b,\r\n\"\"\",ccc"));
            List<string> records = new List<string>();

            CsvToken token;
            while ((token = tokenizer.NextToken()).TokenType != CsvTokenType.Eof)
            {
                records.Add(token.Value);
            }
            records.Add(token.Value);

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("aaa\"", records[0]);
            Assert.AreEqual("b,\r\n\"", records[1]);
            Assert.AreEqual("ccc", records[2]);
        }

        [TestMethod]
        public void CsvTokenizerTest_Emoji()
        {
            using CsvTokenizer tokenizer = new CsvTokenizer(new StringReader("🍀aaa,b🍀bb,ccc🍀"));
            List<string> records = new List<string>();

            CsvToken token;
            while ((token = tokenizer.NextToken()).TokenType != CsvTokenType.Eof)
            {
                records.Add(token.Value);
            }
            records.Add(token.Value);

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("🍀aaa", records[0]);
            Assert.AreEqual("b🍀bb", records[1]);
            Assert.AreEqual("ccc🍀", records[2]);
        }
    }
}