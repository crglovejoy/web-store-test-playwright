using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebStoreTestPlaywright
{
    public class AvoidBotClass : PlaywrightTest
    {
        private string _baseUrl = null!;
        protected string BaseUrl => _baseUrl;
        private IPage _page = null!;
        protected IPage Page => _page;
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IBrowserContext _context = null!;
        private static readonly Random _rnd = new();

        [OneTimeSetUp]
        public async Task BaseOneTimeSetUp()
        {
            _baseUrl = Utilities.GetBaseUrl();

            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Channel = Utilities.GetBrowserChannel(),
                Headless = Utilities.GetHeadlessBool()
            });

            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });

            await _context.AddInitScriptAsync(@"
                Object.defineProperty(navigator, 'webdriver', { get: () => undefined });
                window.chrome = { runtime: {} };
            ");
        }

        [SetUp]
        public async Task BaseSetup()
        {
            _page = await _context.NewPageAsync();
        }

        [TearDown]
        public async Task BaseTearDown()
        {
            if (_page != null) await Page.CloseAsync();
        }

        [OneTimeTearDown]
        public async Task BaseOneTimeTearDown()
        {
            if (_context != null) await _context.CloseAsync();
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
        }

        protected static float GetRandomThinkTime()
        {
            return (float)_rnd.Next(2000, 4000);
        }
    }
}
