using System;
using System.Collections.Generic;
using System.Text;

namespace WebStoreTestPlaywright
{
    public static class Utilities
    {
        public static string GetBaseUrl()
        {
            // Return the base URL for the application under test
            return TestContext.Parameters.Get("BaseUrl", "https://www.webstaurantstore.com/");
        }

        public static string GetBrowserChannel()
        {
            return TestContext.Parameters.Get("BrowserChannel", "chrome");
        }

        public static Boolean GetHeadlessBool()
        {
            return Boolean.Parse(TestContext.Parameters.Get("Headless", "true"));
        }
    }
}
