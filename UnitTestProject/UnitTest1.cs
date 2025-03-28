using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegTest.Pages;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class AuthTestUser
    {
        [TestMethod]
        public void AuthTestPositive1()
        {
            var Page = new Login();
            Assert.IsTrue(Page.Authenticate("admin", "adminBD"));
        }
        [TestMethod]
        public void AuthTestNegative1()
        {
            var Page = new Login();
            Assert.IsFalse(Page.Authenticate("admin", "admin12345"));
        }
        [TestMethod]
        public void AuthTestNegative2()
        {
            var Page = new Login();
            Assert.IsFalse(Page.Authenticate("flokin98", "adminBD"));
        }
        [TestMethod]
        public void AuthTestPositive2()
        {
            var Page = new Login();
            Assert.IsTrue(Page.Authenticate("Admin", "adminBD"));
        }
        [TestMethod]
        public void AuthTestNegative3()
        {
            var Page = new Login();
            Assert.IsFalse(Page.Authenticate(" ", " "));
        }
    }
    [TestClass]
    public class RegTestUser
    {
        [TestMethod]
        public void RegTestPositive1()
        {
            var Page = new Register();
            Assert.IsTrue(Page.RegisterUser("Фокин", "Евгений", "FokinE", "fokin@cklad.ru", "+79777129672", "parol6453"));
        }
        [TestMethod] 
        public void RegTestNegative1()
        {
            var Page = new Register();
            Assert.IsFalse(Page.RegisterUser("", "", "", "", "", ""));
        }
        [TestMethod]
        public void RegTestNegative2()
        {
            var Page = new Register();
            Assert.IsFalse(Page.RegisterUser("Мария", "Окатая", "MariaO", "maria@clkad.ru", "89763246", "gdyehn2463"));
        }
        [TestMethod] 
        public void RegTestNegative3()
        {
            var Page = new Register();
            Assert.IsFalse(Page.RegisterUser("Иван", "Меморский", "IvanM", "Ivancklad.r", "+79653569842", "jfueg2654"));
        }
        [TestMethod] 
        public void RegTestNegative4()
        {
            var Page = new Register();
            Assert.IsTrue(Page.RegisterUser("Л", "П", "test", "hj@h.t", "+79983738623", "123456"));
        }
    }
}
