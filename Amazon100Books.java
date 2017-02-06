import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

public class AmazonBookList {

	public static void main(String[] args){	
				
		System.setProperty("webdriver.chrome.driver", "C:/Users/snobl/Downloads/chromedriver_win32/chromedriver.exe");
		
		WebDriver driver = new ChromeDriver();
		
		driver.get("https://www.amazon.co.uk/s/ref=lp_4656884031_il_ti_stripbooks?rh=n%3A266239%2Cn%3A%21425568031%2Cn%3A%21425570031%2Cn%3A4656884031&ie=UTF8&qid=1486404507&lo=stripbooks");
		driver.manage().window().maximize();
		
		for (int i = 0; i < 60; i++) {
			String s = driver.findElement(By.xpath("//*[@id='result_"+i+"']/div/div[3]/div[1]/a/h2")).getText();			
			System.err.println(s);
		}
		
		driver.get("https://www.amazon.co.uk/s/ref=lp_4656884031_pg_2/254-0075569-9969617?rh=n%3A266239%2Cn%3A%21425568031%2Cn%3A%21425570031%2Cn%3A4656884031&page=2&ie=UTF8&qid=1486404689&lo=stripbooks");
		
		for (int i = 60; i < 100; i++) {
			String s = driver.findElement(By.xpath("//*[@id='result_"+i+"']/div/div[3]/div[1]/a/h2")).getText();
			System.err.println(s);
		}	
	}
}
