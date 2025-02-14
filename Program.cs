using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class IG
{
    static void Main()
    {
        string username = "kullaniciadi";  // Instagram kullanıcı adını buraya yaz // Enter Instagram username here
        string password = "sifre";  // Instagram şifreni buraya yaz // Enter Instagram password here
        int unfollowCount = 500; // Kaç kişiyi takipten çıkaracağını belirle // Set how many people to unfollow

        // Chrome WebDriver'ı başlat // Start Chrome WebDriver
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();

        try
        {
            // Instagram giriş sayfasına git // Navigate to Instagram login page
            driver.Navigate().GoToUrl("https://www.instagram.com/accounts/login/");
            Thread.Sleep(5000);

            // Kullanıcı adı ve şifreyi gir // Enter username and password
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            Thread.Sleep(7000); // Giriş yapıldıktan sonra bekleme süresi // Wait after login

            // Profil sayfasına git // Navigate to profile page
            driver.Navigate().GoToUrl($"https://www.instagram.com/{username}/");
            Thread.Sleep(5000);

            // Following kısmına tıklayarak modal pencereyi aç // Click on "Following" to open modal window
            driver.FindElement(By.XPath("//a[contains(@href, 'following')]")).Click();
            Thread.Sleep(5000); // Modal açıldığında bekleme // Wait for modal to load

            // Takip edilenleri bul ve takipten çık // Find followed accounts and unfollow
            int count = 0;
            while (count < unfollowCount)
            {
                try
                {
                    // Modal pencerede "Following" veya "Takiptesin" butonunu bul // Find "Following" or "Takiptesin" button in modal
                    var buttons = driver.FindElements(By.XPath("//button//div[contains(text(), 'Following') or contains(text(), 'Takiptesin')]"));
                    
                    if (buttons.Count == 0)
                    {
                        Console.WriteLine("Takipten çıkarılacak kişi bulunamadı."); // No users found to unfollow
                        break;
                    }

                    buttons[0].Click(); // İlk butona tıkla // Click the first button
                    Thread.Sleep(500);

                    // Açılan menüde "Unfollow" veya "Takibi Bırak" butonunu bul ve tıkla // Find and click "Unfollow" or "Takibi Bırak" button in popup
                    var confirmButton = driver.FindElement(By.XPath("//button[contains(text(), 'Unfollow') or contains(text(), 'Takibi Bırak')"]));
                    confirmButton.Click();

                    count++;
                    Console.WriteLine($"{count}. kişi takipten çıkarıldı."); // {count} user unfollowed
                    Thread.Sleep(500); // Instagram'ın engellemesini önlemek için bekleme süresi // Wait to prevent Instagram blocking
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}"); // Error occurred
                }
            }
        }
        finally
        {
            Console.WriteLine("İşlem tamamlandı, tarayıcı kapatılıyor..."); // Process complete, closing browser
            driver.Quit();
        }
    }
}
