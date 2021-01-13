using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using SinGooCMS.Utility.Extension;

namespace SinGooCMS.Utility
{
    /// <summary>
    /// �ַ�������
    /// </summary>
    public static class StringUtils
    {
        #region ��ȡ����ƴ���ĵ�һ����ĸ

        /// <summary>
        /// ��ȡ����ƴ��������ĸ
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpellFirst(string strText)
        {
            var builder = new StringBuilder();
            strText.ToCharArray().ForEach((p) =>
            {
                builder.Append(GetChineseSpell(p.ToString()).Substring(0, 1));
            });

            return builder.ToString();
        }

        /// <summary>
        /// ��ȡ����ƴ��
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            string py = "";
            var builder = new StringBuilder();
            strText.ToCharArray().ForEach((p) =>
            {
                if (!ChineseChar.IsValidChar(p))
                    builder.Append(p.ToString()); //������Ч�ĺ��֣�ԭ�����
                else
                {
                    var chineseChar = new ChineseChar(p);
                    if (chineseChar.PinyinCount > 0)
                    {
                        py = chineseChar.Pinyins[0].ToString();
                        builder.Append(py.Substring(0, py.Length - 1));
                    }
                    else
                        builder.Append(p.ToString());
                }
            });

            return builder.ToString();
        }

        #endregion

        #region ��ת��

        /// <summary> 
        /// ����ת��Ϊ����
        /// </summary> 
        /// <param name="str">������</param> 
        /// <returns>������</returns> 
        public static string GetTraditional(string str)
        {
            return ChineseConverter.Convert(str, ChineseConversionDirection.SimplifiedToTraditional);//ת����
        }

        /// <summary> 
        /// ����ת��Ϊ����
        /// </summary> 
        /// <param name="str">������</param> 
        /// <returns>������</returns> 
        public static string GetSimplified(string str)
        {
            return ChineseConverter.Convert(str, ChineseConversionDirection.TraditionalToSimplified);//ת����
        }

        #endregion

        #region ��ȡ�ַ���

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="len"></param>
        /// <param name="appendString"></param>
        /// <returns></returns>
        public static string Cut(string sourceStr, int len, string appendString = "")
        {
            if (sourceStr.Length >= len)
                return sourceStr.Substring(0, len) + appendString;

            return sourceStr;
        }

        /// <summary>
        /// ��ȡ�м�һ���ַ���
        /// </summary>
        /// <param name="source"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Cut(string source, string begin, string end)
        {
            string sub;
            sub = source.Substring(source.IndexOf(begin) + begin.Length);
            sub = sub.Substring(0, sub.IndexOf(end));
            return sub;
        }

        /// <summary>
        /// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str) =>
            Encoding.Default.GetBytes(str).Length;

        #endregion

        #region �����ַ�        

        /// <summary>
        /// ѹ��HTML
        /// </summary>
        /// <returns></returns>
        public static string Compress(string strHTML)
        {
            strHTML = Regex.Replace(strHTML, @">\s+\r", ">");
            strHTML = Regex.Replace(strHTML, @">\s+\n", ">");
            strHTML = Regex.Replace(strHTML, @">\s+<", "><");

            return strHTML;
        }

        /// <summary>
        /// �滻sql����е����������
        /// </summary>
        public static string ChkSQL(string str)
        {
            return (str == null) ? "" : str.Replace("'", "''");
        }

        #endregion

        #region ����ָ�

        /// <summary>
        /// ����������ʽģʽ�����λ�ò�������ַ�����
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Split(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            return regex.Split(input);
        }

        #endregion

        #region �����

        /// <summary>
        /// ����һ���µ��ļ���
        /// </summary>
        /// <returns></returns>
        public static string GetNewFileName() =>
            DateTime.Now.ToString("yyyyMMddffff") + GetRandomString(8);

        ///<summary>
        ///��������ַ��� 
        ///</summary>
        ///<param name="length">Ŀ���ַ����ĳ���</param>
        ///<param name="custom">Ҫ�������Զ����ַ���ֱ������Ҫ�������ַ��б�</param>
        ///<param name="useNum">�Ƿ�������֣�1=������Ĭ��Ϊ����</param>
        ///<param name="useLow">�Ƿ����Сд��ĸ��1=������Ĭ��Ϊ����</param>
        ///<param name="useUpp">�Ƿ������д��ĸ��1=������Ĭ��Ϊ����</param>
        ///<param name="useSpe">�Ƿ���������ַ���1=������Ĭ��Ϊ������</param>        
        ///<returns>ָ�����ȵ�����ַ���</returns>
        public static string GetRandomString(int length = 10, string custom = "", bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            var builder = new StringBuilder();
            if (!custom.IsNullOrEmpty())
                builder.Append(custom);

            if (useNum == true) { builder.Append("0123456789"); }
            if (useLow == true) { builder.Append("abcdefghijklmnopqrstuvwxyz"); }
            if (useUpp == true) { builder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
            if (useSpe == true) { builder.Append("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"); }
            string str = builder.ToString();

            var builder2 = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder2.Append(str.Substring(r.Next(0, str.Length - 1), 1));
            }
            return builder2.ToString();
        }

        /// <summary>
        /// ����GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
        #endregion

        #region ���ط���ǰ׺

        /// <summary>
        /// ���ؿո�
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static string GetSpaceChar(int depth)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                builder.Append(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// ���ط���ǰ׺
        /// </summary>
        /// <param name="intDept"></param>
        /// <param name="boolIsEnd"></param>
        /// <returns></returns>
        public static string GetCatePrefix(int intDept, bool boolIsEnd)
        {
            char ch = '\x00a0';
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < intDept; i++)
            {
                builder.Append(ch);
            }
            if (boolIsEnd)
                builder.Append("��");
            else
                builder.Append("��");

            return builder.ToString();
        }
        #endregion                

        #region ���ɱ��
        /// <summary>
        /// ���ɱ��${year}${month}${day}${hour}${minute}${second}${millisecond}${rnd}
        /// </summary>
        /// <param name="SNFormat"></param>
        /// <returns></returns>
        public static string GenerateSN(string SNFormat)
        {
            return SNFormat.Replace("${year}", System.DateTime.Now.ToString("yyyy")).Replace("${month}", System.DateTime.Now.ToString("MM"))
                .Replace("${day}", System.DateTime.Now.ToString("dd")).Replace("${hour}", System.DateTime.Now.ToString("HH"))
                .Replace("${minute}", System.DateTime.Now.ToString("mm")).Replace("${second}", System.DateTime.Now.ToString("ss"))
                .Replace("${millisecond}", System.DateTime.Now.ToString("ffff")).Replace("${rnd}", StringUtils.GetRandomString(3));
        }
        #endregion

        #region ����uri��ַ

        /// <summary>
        /// ����uri��ַ
        /// </summary>
        /// <param name="parameters">����</param>
        /// <param name="protocol">Э�� http:// https:// ftp://</param>
        /// <returns></returns>
        public static Uri GenerateUri(string[] parameters, string protocol = "http://")
        {
            if (protocol.IndexOf("://") == -1)
                protocol += "://";

            var builder = new StringBuilder(protocol);
            for (var i = 0; i < parameters.Length; i++)
            {
                if (i == parameters.Length - 1)
                    builder.Append(parameters[i].Replace("//", "/").TrimStart('/').TrimEnd('/'));
                else
                    builder.Append(parameters[i].Replace("//", "/").TrimStart('/').TrimEnd('/') + "/");
            }

            return new Uri(builder.ToString());
        }

        #endregion
    }
}