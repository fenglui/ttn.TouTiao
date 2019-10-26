using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ttn.TouTiao.Tests
{
    [TestClass]
    public class AppsApiTest : BaseApiTest
    {
        [TestMethod]
        public void ValidGetAccessTokenTest()
        {
            var task = AppsApi.GetAccessTokenAsync(_appId, _appSecret);

            Assert.IsNotNull(task.Result);
            Assert.IsNotNull(task.Result.Response.access_token);
            Assert.IsTrue(task.Result.Response.expires_in > 0);
        }

        [TestMethod]
        public void NotValidGetAccessTokenTest()
        {
            string appid = "";
            string secret = "";
            var task = AppsApi.GetAccessTokenAsync(appid, secret);
            Assert.IsNotNull(task.Result);
            Assert.IsNotNull(task.Result.Error);
            Assert.IsNotNull(task.Result.Error.errmsg);
        }
    }
}
