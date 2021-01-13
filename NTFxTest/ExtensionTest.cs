using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using SinGooCMS.Utility.Extension;
using System.Text;

namespace NTFxTest
{
    [TestClass]
    public class ExtensionTest
    {
        [TestMethod]
        public void TestJson()
        {
            var user = new Student() { UserName="jsonlee",Age=18 };
            var json = user.ToJson();
            Console.WriteLine("user to json:" + json);
            var user2 = json.JsonToObject<Student>();
            Assert.AreEqual("jsonlee", user2.UserName);
        }

        [TestMethod]
        public void TestJosnToAnanymous()
        {
            var json = "{\"UserName\":\"jsonlee\",\"Age\":18}";
            var model = new { UserName = "", Age = 0 };
            var obj = json.JsonToAnonymousObject(model);
            Assert.AreEqual("jsonlee", obj.UserName);
        }

        [TestMethod]
        public void TestXml()
        {
            var user = new Student() { UserName = "jsonlee", Age = 18 };
            var xml = user.ToXml();
            Console.WriteLine("user to json:" + xml);
            var user2 = xml.XmlToObject<Student>();
            Assert.AreEqual("jsonlee", user2.UserName);
        }

        [TestMethod]
        public void TestXmlToObj()
        {
            string xml = @"<?xml version='1.0' encoding='utf - 8'?>
                          <Student>  
                            <UserName>jsonlee</UserName>  
                            <Age>18</Age>
                          </Student> ";

            var obj = xml.XmlToObject<Student>();
            Assert.AreEqual(obj.UserName, "jsonlee");
        }

        [TestMethod]
        public void TestDate()
        {
            var now = DateTime.Now;
            Console.WriteLine($"��һ��{now.GetMonday()} ���գ�{now.GetSunday()}");
            Console.WriteLine($"�³���{now.GetMonthFirstDay()} ��β��{now.GetMonthLastDay()}");
            Console.WriteLine($"������{now.GetQuarterFirstDay()} ��β��{now.GetQuarterLastDay()}");
            Console.WriteLine($"�����{now.GetYearFirstDay()} ��β��{now.GetYearLastDay()}");
            Console.WriteLine($"����ڼ��ܣ�{now.WeekOfYear()}");
            Console.WriteLine($"��ʽ����{now.ToFormatDate()}");
            Console.WriteLine($"��������{now.GetTotalSeconds()}");
            Console.WriteLine($"��ǰ�й�ʱ����{now.GetCNHour().DIZHI} - {now.GetCNHour().CNHOUR}");
        }

        [TestMethod]
        public void ConvertTest()
        {
            var a = "0";
            var b = "true";

            //Assert.AreEqual(0, a.To<int>());
            Assert.AreEqual(true, b.To<bool>());
        }

        [TestMethod]
        public void ValidTest()
        {
            Assert.AreEqual(true,"16826375@qq.com".IsEmail());
            Assert.AreEqual(true, " ".IsNullOrEmpty());
        }

        [TestMethod]
        public void StringTest()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("123ת����ң�{0} \r\n",123.9m.ToRMB());
            builder.AppendFormat("123456789ת���룺{0} \r\n", "123456789".Mask());

            Console.WriteLine(builder.ToString());
        }
    }

    public class Student
    {
        public string UserName { get; set; }
        public int Age { get; set; }
    }
}