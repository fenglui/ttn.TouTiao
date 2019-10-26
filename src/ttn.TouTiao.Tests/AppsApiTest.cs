using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ttn.TouTiao.Tests
{
    [TestClass]
    public class AppsApiTest : BaseApiTest
    {
        [TestMethod]
        public void ValidGetAccessTokenTest()
        {
            string access_token = null;

            var task = AppsApi.GetAccessTokenAsync(_appId, _appSecret);

            Assert.IsNotNull(task);
            Assert.IsNotNull(task.Result.Response.access_token);
            access_token = task.Result.Response.access_token;
            Assert.IsTrue(task.Result.Response.expires_in > 0);

            Console.WriteLine(access_token);
        }

        [TestMethod]
        public void NotValidGetAccessTokenTest()
        {
            string appid = "";
            string secret = "";

            Task.Run(async () => {
                var task = await AppsApi.GetAccessTokenAsync(appid, secret);
                Assert.IsNotNull(task);
                Assert.IsNotNull(task.Error);
                Assert.IsNotNull(task.Error.errmsg);
            });
        }

        [TestMethod]
        public void CreateQRCodeAsyncTest()
        {
            string access_token = null;

            var task = AppsApi.GetAccessTokenAsync(_appId, _appSecret);

            Assert.IsNotNull(task);
            Assert.IsNotNull(task.Result.Response.access_token);
            access_token = task.Result.Response.access_token;
            Assert.IsTrue(task.Result.Response.expires_in > 0);
            var qr = AppsApi.CreateQRCodeAsync(access_token);
            Assert.IsNotNull(qr);
            Assert.IsNotNull(qr.Result);
            Assert.IsNotNull(qr.Result.Response);
        }
    }
}
