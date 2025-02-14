using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class IG
{
    static void Main()
    {
        string username = "kullaniciadi";  // Instagram kullanıcı adını buraya yaz
        string password = "sifre";  // Instagram şifreni buraya yaz
        int unfollowCount = 500; // Kaç kişiyi takipten çıkaracağını belirle

        // Chrome WebDriver'ı başlat
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            // Instagram giriş sayfasına git
            driver.Navigate().GoToUrl("https://www.instagram.com/accounts/login/");
            Thread.Sleep(5000);

            // Kullanıcı adı ve şifreyi gir
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            Thread.Sleep(7000); // Giriş yapıldıktan sonra bekleme süresi

            // Profil sayfasına git
            driver.Navigate().GoToUrl($"https://www.instagram.com/{username}/");
            Thread.Sleep(5000);

            // Following kısmına tıklayarak modal pencereyi aç
            driver.FindElement(By.XPath("//a[contains(@href, 'following')]")).Click();
            Thread.Sleep(5000); // Modal açıldığında bekleme

            // Takip edilenleri bul ve takipten çık
            int count = 0;
            while (count < unfollowCount)
            {
                try
                {
                    // Modal pencerede "Following" veya "Takiptesin" butonunu bul
                    var buttons = driver.FindElements(By.XPath("//button//div[contains(text(), 'Following') or contains(text(), 'Takiptesin')]"));
                    
                    if (buttons.Count == 0)
                    {
                        Console.WriteLine("Takipten çıkarılacak kişi bulunamadı.");
                        break;
                    }

                    buttons[0].Click(); // İlk butona tıkla
                    Thread.Sleep(500);

                    // Açılan menüde "Unfollow" veya "Takibi Bırak" butonunu bul ve tıkla
                    var confirmButton = driver.FindElement(By.XPath("//button[contains(text(), 'Unfollow') or contains(text(), 'Takibi Bırak')]"));
                    confirmButton.Click();

                    count++;
                    Console.WriteLine($"{count}. kişi takipten çıkarıldı.");
                    Thread.Sleep(500); // Instagram'ın engellemesini önlemek için bekleme süresi
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                }
            }
        }
        finally
        {
            Console.WriteLine("İşlem tamamlandı, tarayıcı kapatılıyor...");
            driver.Quit();
        }
    }
}
