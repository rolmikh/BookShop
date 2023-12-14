using Book_Shop_WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestPassword
{
    [TestClass]
    public class UnitTest1
    {
        Registration registration = new Registration();
        
        [TestMethod]
        public void isValidPassword_Password_Short_false()
        {
            string password = "a";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_Long_false()
        {
            string password = "aaaaaaaaaaaaaaaaaaaaaa";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_Number_false()
        {
            string password = "12345678";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_NoLower_false()
        {
            string password = "AAAAAAAA";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_NoHigher_false()
        {
            string password = "aaaaaaaa";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_Special_false()
        {
            string password = "*()^&$#";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void isValidPassword_Password_Short_true()
        {
            string password = "SgyuytyrtD";

            bool res = registration.IsValidPassword(password);
            Assert.IsTrue(res);

        }
    }
}
