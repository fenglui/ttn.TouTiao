using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ttn.TouTiao.Tests
{
    public abstract class BaseApiTest
    {
        private dynamic _appConfig;
        protected dynamic AppConfig
        {
            get
            {
                if (_appConfig == null)
                {

#if NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
                    var filePath = "../../../test.config";
#else
                    var filePath = "../../test.config";
#endif
                    if (File.Exists(filePath))
                    {
#if NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
                        var stream = new FileStream(filePath, FileMode.Open);
                        var doc = XDocument.Load(stream);
                        stream.Dispose();
#else
                        var doc = XDocument.Load(filePath);
#endif
                        _appConfig = new
                        {
                            AppId = doc.Root.Element("AppId").Value,
                            Secret = doc.Root.Element("Secret").Value,
                        };
                    }
                    else
                    {
                        throw new Exception(@"File test.config not found.
Add a file to your project
<Config>
  <AppId>yourAppId</AppId>
  <Secret>yourAppSecret</Secret>
</Config>
");
                    }
                }
                return _appConfig;
            }
        }

        protected string _appId
        {
            get { return AppConfig.AppId; }
        }

        protected string _appSecret
        {
            get { return AppConfig.Secret; }
        }
    }
}
