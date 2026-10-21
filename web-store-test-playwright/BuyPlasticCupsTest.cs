using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace WebStoreTestPlaywright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BuyPlasticCupsTest : AvoidBotClass
    {

        [Test]
        public async Task BuyPlasticCups()
        {
            await Page.GotoAsync(BaseUrl);

            try
            {
                // Go to the Disposable tab
                await Expect(Page.GetByTestId("nav-full-data-items").GetByTestId("category-item-anchor-Disposables")).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByTestId("nav-full-data-items").GetByTestId("category-item-anchor-Disposables").ClickAsync();
              
                // Go to Plastic Cups and Lids link
                await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Plastic Cups and Lids" })).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByRole(AriaRole.Link, new() { Name = "Plastic Cups and Lids" }).ClickAsync();

                // Go to disposable plastic cups
                await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Disposable Plastic Cups Shop" })).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByRole(AriaRole.Link, new() { Name = "Disposable Plastic Cups Shop" }).ClickAsync();

                // Select the 16oz cup size option
                await Expect(Page.GetByTestId("topper-item-12600")).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByTestId("topper-item-12600").ClickAsync();

                // Select the Solo Ultra Clear cup options
                await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Case Customizable 1 Option Solo Ultra Clear™ TP16D 16 oz. Customizable Clear" })).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByRole(AriaRole.Link, new() { Name = "Case Customizable 1 Option Solo Ultra Clear™ TP16D 16 oz. Customizable Clear" }).ClickAsync();

                // Validate that the default quantity next to the add to cart button is set to 1
                await Expect(Page.GetByTestId("product-detail-heading")).ToBeVisibleAsync();
                await Expect(Page.GetByTestId("product-detail-heading")).ToMatchAriaSnapshotAsync("- heading /Solo Ultra Clear™ TP16D \\d+ oz\\. Customizable Clear PET Plastic Squat Cold Cup - \\d+,\\d+\\/Case/ [level=1]");
                await Expect(Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true })).ToBeVisibleAsync();
                await Expect(Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true })).ToHaveValueAsync("1");

                // Change the quantity to 3 next to the add to cart button.
                await Expect(Page.GetByTestId("atc-button")).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true }).ClickAsync();
                await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true }).PressAsync("ArrowRight");
                await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true }).FillAsync("3");

                // Add the order to the shopping cart
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByTestId("atc-button").ClickAsync();

                // Go to the shopping cart and validate that what we ordered is present and that the quantity is 3
                await Expect(Page.GetByTestId("cart-button")).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByTestId("cart-button").ClickAsync();
                await Expect(Page.GetByLabel("cart items").GetByRole(AriaRole.Link, new() { Name = "Solo Ultra Clear™ TP16D 16 oz" })).ToBeVisibleAsync();
                await Expect(Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true })).ToBeVisibleAsync();
                await Expect(Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Quantity", Exact = true })).ToHaveValueAsync("3");

                // Empty the shopping cart and validate it is empty
                await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Empty Cart" })).ToBeVisibleAsync();
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await Page.GetByRole(AriaRole.Button, new() { Name = "Empty Cart" }).ClickAsync();
                // Handle the modal that appears to confirm the intent to empty the cart.
                await Expect(Page.Locator("header").Filter(new() { HasText = "Empty Cart" })).ToBeVisibleAsync();
                await Page.GetByText("Are you sure you want to").ClickAsync();
                await Expect(Page.GetByTestId("modal-footer")).ToBeVisibleAsync();
                var modal = Page.GetByTestId("modal-footer");
                await Page.WaitForTimeoutAsync(GetRandomThinkTime());
                await modal.GetByText("Empty Cart").ClickAsync();
                await Expect(modal).ToBeHiddenAsync();
                await Expect(Page.GetByText("Your cart is empty.")).ToBeVisibleAsync();
            }
            catch
            {
                // If we've encountered an Expect/Asertion failure, take a screenshot of the page for debugging purposes.
                if (Page != null)
                {
                    await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "BuyPlasticCupsTest_Failure.png", FullPage = true });
                }

                throw; // rethrow the exception to mark the test as failed
            }
        }
    }
}
