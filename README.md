# 遊艇網站與後台管理系統 | Yacht Website

本專案為一個功能完整的船舶展示與後台管理網站，基於 ASP.NET Web Forms 技術開發。主要目標是為遊艇公司打造一個專業的官網，並提供一個強大的後台系統，讓管理員可以輕鬆管理網站內容。
## ✨ 核心功能 (Key Features)

### 前台 (Front-End)
- **遊艇型錄**：提供遊艇列表及詳細規格介紹頁面，包含多圖輪播、規格表與檔案下載。
- **最新消息**：發布公司與產業新聞。
- **全球經銷商**：以列表方式呈現全球銷售據點。
- **聯絡我們**：整合 Google reCAPTCHA v2 驗證的聯絡表單，並透過 SMTP 自動發送信件給管理員與使用者。
- **公司資訊**：靜態頁面（如關於我們、認證資訊等）皆可由後台動態管理。

### 後台 (Back-End)（使用 SB Admin 2 模板）
- **安全登入機制**：管理員帳號登入與權限控管。
- **遊艇管理**：對遊艇資料進行完整的 CRUD (新增、讀取、修改、刪除) 操作，包含圖片與附件上傳。
- **內容管理**：輕鬆管理最新消息、公司資訊與代理商資料。
- **管理員權限控制**：設有 `SuperAdmin` 角色，具備管理其他後台使用者的最高權限。

## 🛠️ 技術棧 (Technology Stack)

| 類別 | 技術 |
| :--- | :--- |
| **後端** | ASP.NET Web Forms (.NET Framework 4.7.2), C# |
| **前端** | HTML, CSS, JavaScript |
| **資料庫** | MS SQL Server, SSMS |
| **資料存取** | ADO.NET (手動操作 `SqlConnection`, `SqlCommand`, etc.) |
| **框架/模板**| Bootstrap 3 (前台), SB Admin 2 (後台) |
| **第三方服務**| Google reCAPTCHA v2, SMTP (for mailing) |
| **編輯器** | CKEditor (用於新聞與內容的所見即所得編輯) |


## 🚀 如何在本機運行 (Getting Started)

請依照以下步驟設定您的本機開發環境。

### **安裝步驟 (Installation)**

1.  **Clone 專案**
    ```bash
    git clone [https://github.com/kc34524/TayanaYacht.git](https://github.com/kc34524/TayanaYacht.git)
    cd TayanaYacht
    ```
2.  **建立資料庫**
    - 開啟 SSMS，建立一個新的資料庫。
    - 執行專案中提供的 `.sql` 檔案以建立所有需要的資料表與預設資料。

3.  **設定環境變數**
    - 使用 Visual Studio 開啟 `.sln` 專案檔。
    - **設定資料庫連線字串**：開啟 `Web.config` 檔案，找到 `<connectionStrings>` 區塊，並填入您的 SQL Server 連線資訊。
      ```xml
      <connectionStrings>
        <add name="MyDb" connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=YOUR_DB_NAME;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD" providerName="System.Data.SqlClient" />
      </connectionStrings>
      ```
    - **設定 Google reCAPTCHA**：同樣在 `Web.config`，於 `<appSettings>` 區塊填入您的金鑰。
      ```xml
      <appSettings>
        <add key="RecaptchaSiteKey" value="YOUR_RECAPTCHA_SITE_KEY" />
        <add key="RecaptchaSecretKey" value="YOUR_RECAPTCHA_SECRET_KEY" />
      </appSettings>
      ```
    - **設定 SMTP 寄件資訊**：開啟 `Contact.aspx.cs` 檔案，填入您的寄件 Email 帳號、密碼與收件者。
      ```csharp
      // Contact.aspx.cs
      smtp.Credentials = new NetworkCredential("Your_Email@gmail.com", "Your_Google_App_Password"); // 建議使用 Google 應用程式密碼
      
      // --- 寄送第一封信 (給管理員) ---
      MailMessage mailtoAdmin = new MailMessage();
      mailtoAdmin.From = new MailAddress("Your_Email@gmail.com");
      mailtoAdmin.To.Add("Admin_Receive_Email@example.com"); 

      // --- 寄送第二封信 (給使用者) ---
      MailMessage mailToUser = new MailMessage();
      mailToUser.From = new MailAddress("Your_Email@gmail.com", "Yachts"); // 可以加上寄件人名稱

      ```

4.  **啟動專案**
    - 在 Visual Studio 中，按下 `F5` 或點擊 "IIS Express" 按鈕來執行專案。



## 📁 專案結構 (Project Structure)
```
/TayanaYacht
├── Admin/              # 後台管理頁面 (ASPX, CS)
├── assers/             # 後台模板 (SB Admin 2) 靜態資源
├── Front_Assets/       # 前台靜態資源 (CSS, JS, Images)
├── Upload/             # 使用者上傳的圖片與檔案 (Dealers, News, Yachts)
├── Web.config          # 核心設定檔 (已移除敏感資訊)
└── (根目錄)             # 前台頁面 (ASPX, CS)
```
## 🧠 挑戰與學習 (Challenges & Learnings)

在這個專案中，我遇到並克服了以下挑戰，從中獲益良多：

* **純手工 ADO.NET 操作**：在未使用 ORM 的情況下，我學習到如何精準控制 SQL 指令、管理資料庫連線池以優化效能，這讓我對底層資料庫互動有更深刻的理解。
* **狀態管理**：深入研究了 ASP.NET Web Forms 的生命週期與 `ViewState`、`Session`、`QueryString` 等不同狀態管理機制的適用場景，並在後台權限判斷與前台資料傳遞中進行了實踐。
* **檔案上傳與路徑管理**：實作了後台圖片與檔案上傳功能，並學習如何安全地處理檔案名稱與儲存路徑，避免潛在的安全風險和路徑混亂問題。
* **第三方 API 整合**：成功整合了 Google reCAPTCHA v2 和 SMTP 服務。在處理 SMTP 時，學習了如何使用 `NetworkCredential` 及 Google 的「應用程式密碼」來安全地進行身分驗證。
* **綜合技能提升**：透過這個專案，我進一步掌握了前後台功能之間的設計關係，例如如何以前台需求反推後台欄位與資料結構；同時也熟悉了 Git 分支操作與合併流程、專案資料夾結構規劃，並實作了基本的使用者權限與後台管理系統。


## 📝 專案狀態 (Project Status)

本專案已完成核心功能開發與測試，並合併至 `main` 分支。目前僅作為個人作品集展示用途，暫無後續維護計畫。

## 👤 作者 (Author)

**Kelly C.**
* GitHub: [@kc34524](https://github.com/kc34524)
