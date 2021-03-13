using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using NUnit.Framework;
using SinGooCMS.Utility;
using SinGooCMS.Utility.Extension;

namespace CoreTest
{
    public class ExtensionTest
    {
        [Test]
        public void TestJson()
        {
            var user = new StudentInfo() { UserName = "jsonlee", Age = 18 };
            var json = user.ToJson();
            Console.WriteLine("user to json:" + json);
            var user2 = json.JsonToObject<StudentInfo>();
            Assert.AreEqual("jsonlee", user2.UserName);
        }

        [Test]
        public void TestJosnToAnanymous()
        {
            var json = "{\"UserName\":\"jsonlee\",\"Age\":18}";
            //�����࣬��̬��
            var model = new { UserName = "", Age = 0 };
            var obj = json.JsonToAnonymousObject(model);
            Assert.AreEqual("jsonlee", obj.UserName);
        }

        [Test]
        public void TestXml()
        {
            var users = new List<StudentInfo>() {
                new StudentInfo() { UserName = "jsonlee", Age = 18 },
                new StudentInfo() { UserName = "jsonlee2", Age = 22 }
            };
            var xml = users.ToXml();
            Console.WriteLine("user to json:" + xml);
            var users2 = xml.XmlToObject<List<StudentInfo>>();
            Assert.AreEqual("jsonlee", users2[0].UserName);

        }

        [Test]
        public void TestXmlToObj()
        {
            //�����list,��Ŀ¼�������ģ�ArrayOfStudent
            string xml = @"<?xml version='1.0' encoding='utf - 8'?>
                          <StudentInfo>
                            <score>100</score>
                            <AutoID>1</AutoID>
                            <UserName>jsonlee</UserName>
                            <Age>18</Age>
                            <Birthday>0001-01-01T00:00:00</Birthday>
                            <AutoTimeStamp>0001-01-01T00:00:00</AutoTimeStamp>
                          </StudentInfo> ";

            var obj = xml.XmlToObject<StudentInfo>();
            Assert.AreEqual(obj.UserName, "jsonlee");
        }

        [Test]
        public void TestDate()
        {
            var now = DateTime.Now;
            Console.WriteLine($"��һ��{now.GetMonday()} ���գ�{now.GetSunday()}");
            Console.WriteLine($"�����ܵ�һ�죺{now.GetWeekFirstDay(false)} �����ܵ����һ�죺{now.GetWeekLastDay(false)}");
            Console.WriteLine($"�й��ܵ�һ�죺{now.GetWeekFirstDay()} �й��ܵ����һ�죺{now.GetWeekLastDay()}");
            Console.WriteLine($"�³���{now.GetMonthFirstDay()} ��β��{now.GetMonthLastDay()}");
            Console.WriteLine($"������{now.GetQuarterFirstDay()} ��β��{now.GetQuarterLastDay()}");
            Console.WriteLine($"�����{now.GetYearFirstDay()} ��β��{now.GetYearLastDay()}");

            //ע�� 2020, 12, 31 �� 2021, 1, 1��ͬһ�ܣ���Ϊ�����ˣ��ֳ���2�����
            var date1 = new DateTime(2020, 12, 31);
            Console.WriteLine($"����ڼ��ܣ�{date1.WeekOfYear()}"); //����ڼ��ܣ�53
            var date2 = new DateTime(2021, 1, 1);
            Console.WriteLine($"����ڼ��ܣ�{date2.WeekOfYear()}"); //����ڼ��ܣ�1

            Console.WriteLine($"2020-12-31�ڼ��ܣ�{date1.WeekOfYear(true)}��2021-1-1�ڼ��ܣ�{date2.WeekOfYear(true)}"); //����ڼ��ܣ�1

            Console.WriteLine($"��ʽ����{now.ToFormatDate()}");
            Console.WriteLine($"��������{now.GetUnixTimeSeconds()}");
            Console.WriteLine($"��ǰ�й�ʱ����{now.GetCNHour().DIZHI} - {now.GetCNHour().CNHOUR}");
        }

        [Test]
        public void ConvertTest()
        {
            var builder = new StringBuilder();
            builder.Append($"{"100"}תLong:{"100".ToLong()}\r\n");
            builder.Append($"{"100"}תInt:{"100".ToInt()}\r\n");
            builder.Append($"{"100"}תShort:{"100".ToShort()}\r\n");
            builder.Append($"{"100"}תByte:{"100".ToByte()}\r\n");

            builder.Append($"{"100.89"}תFloat:{"100.89".ToFloat()}\r\n");
            builder.Append($"{"100.89"}תDouble:{"100.89".ToDouble()}\r\n");
            builder.Append($"{"100.89"}תDecimal:{"100.89".ToDecimal()}\r\n");

            builder.Append($"{"100.89"}תint:{"100.89".ToInt()}\r\n");
            builder.Append($"{100.89m}תbyte:{100.89m.ToByte()}\r\n");
            builder.Append($"{100.89m}תshort:{100.89m.ToShort()}\r\n");
            builder.Append($"{100.89m}תint:{100.89m.ToInt()}\r\n");
            builder.Append($"{100.89m}תlong:{100.89m.ToLong()}\r\n");

            builder.Append($"{"true"}תbool:{"true".ToBool()}\r\n");

            builder.Append($"{"2020-12-31"}תdatetime:{"2020-12-31".ToDateTime()}\r\n");
            builder.Append($"{"2020-12-31 12:00"}תdatetime:{"2020-12-31 12:00".ToDateTime()}\r\n");

            Console.WriteLine(builder.ToString());

            var d = 18.5m;
            int e = d.To<int>(); //ת����ȷ ����18.9��ת��19
            Assert.AreEqual(18, e);

            int f = d.ToInt(); //ת����ȷ
            Assert.AreEqual(18, f);
        }

        [Test]
        public void ObjTest()
        {
            var user = new StudentInfo() { UserName = "jsonlee", Age = 18 };
            var user2 = user.DeepClone();
            Assert.AreEqual(user.UserName, user2.UserName);
            Assert.AreEqual(false, user2.ReferenceEquals(user)); //���copy�������������ò�ͬ���ڴ�ռ�
        }

        [Test]
        public void ValidTest()
        {
            var builder = new StringBuilder();
            builder.Append($"��֤����\r\n");
            builder.Append($" 123 is int:{"123".IsInt()}\r\n");
            builder.Append($" 123.5 is int:{"123.5".IsInt()}\r\n");
            builder.Append($" 123 is decimal:{"123".IsDecimal()}\r\n");
            builder.Append($" 123.5 is decimal:{"123.5".IsDecimal()}\r\n");

            builder.Append($" 16826375@163.com is email:{"16826375@163.com".IsEmail()}\r\n");
            builder.Append($" 17788760902 is mobile:{"17788760902".IsMobile()}\r\n");

            builder.Append($" jsonlee is en:{"jsonlee".IsEn()}\r\n");
            builder.Append($" ���� is cn:{"����".IsZHCN()}\r\n");

            builder.Append($" http://www.singoo.top is url:{"http://www.singoo.top".IsUrl()}\r\n");
            builder.Append($" http://www.singoo.top/article/news?p=1 is url:{"http://www.singoo.top/article/news?p=1".IsUrl()}\r\n");

            Console.WriteLine(builder.ToString());
        }

        [Test]
        public void StringTest()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("123ת����ң�{0} \r\n", 123.9m.ToRMB());
            builder.AppendFormat("123456789ת���룺{0} \r\n", "123456789".Mask());

            string urlEncode = "http://www.singoo.top/?a=���".AsEncodeUrl();
            builder.AppendFormat("http://www.singoo.top/?a=��� URL���룺{0} \r\n", urlEncode);
            builder.AppendFormat("{0} URL���룺{1} \r\n", urlEncode, urlEncode.AsDecodeUrl());

            Console.WriteLine(builder.ToString());

            var dict = new Dictionary<string, string>();
            dict.Add("username", "����");
            dict.Add("age", "18");
            Console.WriteLine("url:" + dict.ToUrlSearch());

            var urlText = "username=%E5%88%98%E5%A4%87&age=18";
            Console.WriteLine("username:" + urlText.ToUrlDictionary()["username"]);
        }

        [Test]
        public void SpliterTest()
        {
            var str = "1,22,3,44,55";
            var arr = str.ToIntArray();
            Assert.AreEqual(5, arr.Length);

            var str2 = "true,false,true,true";
            var arr2 = str2.ToSpliterArray<bool>();
            Assert.AreEqual(true, arr2[3]);
        }
    }
}