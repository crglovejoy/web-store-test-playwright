using Microsoft.Playwright;

namespace WebStoreTestPlaywright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BasicBaseSiteSanity : PageTest
    {
        private static string _baseUrl = null!;

        [OneTimeSetUp]
        public void LocalOneTimeSetUp() // happens once before any tests in this class are run
        {
            _baseUrl = Utilities.GetBaseUrl();
        }

        [Test]
        public async Task BasicBaseWebsiteSanity()
        {
            await Page.GotoAsync(_baseUrl);

            try
            {
                // Check the page title/text
                await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Homepage, WebstaurantStore" })).ToBeVisibleAsync();

                // Check the product search box and magnifying glass button are present.
                await Expect(Page.GetByRole(AriaRole.Combobox, new() { Name = "Search WebstaurantStore," })).ToBeVisibleAsync();
                await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Search WebstaurantStore" })).ToBeVisibleAsync();

                // Check the cart button is present.
                await Expect(Page.GetByTestId("cart-button")).ToBeVisibleAsync();

                // Check the flyout navigation menu is present and has the expected links.
                await Expect(Page.GetByTestId("flyout-nav")).ToMatchAriaSnapshotAsync(@"
                    - list:
                        - listitem:
                            - link ""Restaurant Equipment"" 
                        - listitem:
                            - link ""Refrigeration""
                        - listitem:
                            - link ""Smallwares""
                        - listitem:
                            - link ""Food & Beverage""
                        - listitem:
                            - link ""Tabletop""
                        - listitem:
                            - link ""Disposables""
                        - listitem:
                            - link ""Furniture""
                        - listitem:
                            - link ""Storage & Transport""
                        - listitem:
                            - link ""Janitorial""
                        - listitem:
                            - link ""Industrial""
                        - listitem:
                            - link ""Business Type"" ");

                // Check that the signup by email input box and Sign Up button are present in the footer.
                await Expect(Page.GetByTestId("footer-email-input")).ToBeVisibleAsync();
                await Expect(Page.GetByTestId("email-signup-button")).ToBeVisibleAsync();
            }
            catch
            {
                // If we've encountered an Expect/Asertion failure, take a screenshot of the page for debugging purposes.
                if (Page != null)
                {
                    await Page.ScreenshotAsync(new() { Path = "screenshot-basic-website-sanity-error.png", FullPage = true });
                }

                // throw the exception to ensure the test fails and the original error is reported.
                throw;
            }
        }
    }
}
