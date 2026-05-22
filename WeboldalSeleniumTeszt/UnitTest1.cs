using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WeboldalSeleniumTeszt;

public class Tests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
    }

    [Test]
    public void Teljes_KonyvtarOldal_UI_UX_Teszt()
    {
        // 1. Az index.html betöltése 
        _driver.Navigate().GoToUrl("file:///C:/Users/fabia/source/repos/Gyakorlatok4/WeboldalSeleniumTeszt/index.html");

        // 2. UI/UX teszt: Fejléc és Logó ellenőrzése
        var logo = _driver.FindElement(By.ClassName("logo"));
        Assert.That(logo.Text, Contains.Substring("KönyvOázis"), "A főoldal logója nem megfelelő!");

        // 3. UX Görgetés Teszt: rákattintunk a menüben a 'Fiókom' gombra, hogy leugorjon az oldal az űrlaphoz
        var fiokMenuLink = _driver.FindElement(By.XPath("//a[@href='#fiok']"));
        fiokMenuLink.Click();
        Thread.Sleep(1000); // Kivárjuk, amíg a smooth scroll leér

        // 4. Input elemek megkeresése
        var userField = _driver.FindElement(By.Id("user"));
        var passField = _driver.FindElement(By.Id("pass"));
        var loginBtn = _driver.FindElement(By.Id("login-btn"));

        // 5. Negatív teszt: Hibás adatok beküldése
        userField.SendKeys("hibas_felhasznalo");
        passField.SendKeys("rossz_jelszo");
        loginBtn.Click();

        // Ellenőrizzük, megjelent-e a piros riasztási ablak
        var errorMsg = _driver.FindElement(By.Id("error-msg"));
        Assert.That(errorMsg.Displayed, Is.True, "Nem ugrott fel a hibaüzenet panel!");

        // 6. Pozitív teszt: Helyes adatok beírása
        userField.Clear();
        userField.SendKeys("admin");

        passField.Clear();
        passField.SendKeys("titok");

        loginBtn.Click();

        // Ellenőrizzük, hogy a cím átváltott-e a sikeres üzenetre
        var title = _driver.FindElement(By.Id("page-title"));
        Assert.That(title.Text, Is.EqualTo("Sikeresen bent vagy!"));

        // 4 másodperc élvezet
        Thread.Sleep(4000);
    }

    [TearDown]
    public void Teardown()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
