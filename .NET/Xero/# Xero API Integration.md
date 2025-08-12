# API Integeration

❌ Why You Cannot Login to Xero Using Username and Password (to get a token)
Xero does not allow direct login with username/password via API because:

✅ Xero uses OAuth 2.0, and:
OAuth 2.0 is the only supported authentication method for accessing Xero data via APIs.

Usernames and passwords are only used on Xero's own secure login page, never in your app.

Username/passwords are sensitive. If you send them over the internet via your app, they can be stolen.
✅ OAuth gives limited access	OAuth tokens can be scoped and expired — safer than giving full account access.

# Alternate
✅ Use OAuth 2.0 — This Is the Login
When integrating with Xero APIs, you are not supposed to log in with credentials like a normal form. Instead, OAuth 2.0 is the login method for APIs.

==================================================================================
# Company or Application URL
This is for informational purposes only, but it should be a valid base URL of your app.

Since you're working locally:

Use:

https://localhost:7183

Later, when deployed, change it to:

https://yourdomain.com

==================================================================================
# Redirect URL
The redirect URI endpoint is a special API endpoint in your application where Xero will redirect the user after they log in and send you an authorization code.

This is the endpoint you must create in your Web API to receive that code, and then exchange it for an access token (so you can call Xero APIs).

==================================================================================
You need to create an count in Xero 
https://www.xero.com/signup/developers/

✅ 1. Register Your App on Xero Developer Portal
Go to: https://developer.xero.com/myapps

Create a new app.

You’ll get:
Client ID
Client Secret
==================================================================================
✅ 1. What You Need for Login to Work
Required Item	Description
✅ Client ID	From Xero Developer Portal
✅ Client Secret	From Xero Developer Portal
✅ Redirect URI	This is an endpoint in your API (e.g., https://localhost:7183/api/login/callback) that handles Xero’s response
✅ Scopes	Permissions your app needs. Common: openid profile email accounting.transactions offline_access
✅ Login Controller	To initiate login and handle token exchange

You must set a Redirect URI (e.g., https://localhost:5001/callback)

==================================================================================

✅ 2. Add These Values to appsettings.json
In your .NET Web API project:

{
  "Xero": {
    "ClientId": "YOUR_CLIENT_ID",
    "ClientSecret": "YOUR_CLIENT_SECRET",
    "RedirectUri": "https://localhost:7183/api/login/callback",
    "Scope": "openid profile email accounting.transactions offline_access"
  }
}

==================================================================================
 public class LogInControler : ControllerBase
 {
     private readonly IConfiguration _config;
     private readonly IHttpClientFactory _httpClientFactory;
     private readonly IXeroService _XeroService;

     public LogInControler(IConfiguration config, IHttpClientFactory httpClientFactory, IXeroService XeroService)
     {
         _config = config;
         _httpClientFactory = httpClientFactory;
         _XeroService = XeroService;
     }

     // STEP 1: Login Endpoint to redirect to Xero
     [HttpGet]
     public IActionResult Login()
     {
         
         var scopes = "openid profile email offline_access accounting.settings.read";
         var authUrl = $"https://login.xero.com/identity/connect/authorize?response_type=code&client_id={_config["Xero:ClientId"]}&redirect_uri={_config["Xero:RedirectUri"]}&scope={Uri.EscapeDataString(scopes)}&state=xyz";
         // Return as plain text or JSON
         return Ok(new { authUrl });
     }


     [HttpGet("callback")]
     public async Task<IActionResult> Callback([FromQuery] string code)
     {
         var client = _httpClientFactory.CreateClient();
         var request = new HttpRequestMessage(HttpMethod.Post, "https://identity.xero.com/connect/token");

         var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config["Xero:ClientId"]}:{_config["Xero:ClientSecret"]}"));

         request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
         request.Content = new FormUrlEncodedContent(new[]
         {
         new KeyValuePair<string, string>("grant_type", "authorization_code"),
         new KeyValuePair<string, string>("code", code),
         new KeyValuePair<string, string>("redirect_uri", _config["Xero:RedirectUri"])
     });

         var response = await client.SendAsync(request);
         var content = await response.Content.ReadAsStringAsync();

         // Optional: Store tokens in DB/session/cache
         return Ok(JsonDocument.Parse(content));
     }



     [HttpGet("get-tenants")]
     public async Task<IActionResult> GetTenants([FromHeader(Name = "access_token")] string accessToken)
     {
         var client = _httpClientFactory.CreateClient();
         var request = new HttpRequestMessage(HttpMethod.Get, "https://api.xero.com/connections");
         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

         var response = await client.SendAsync(request);
         var content = await response.Content.ReadAsStringAsync();

         if (!response.IsSuccessStatusCode)
             return StatusCode((int)response.StatusCode, content);

         return Ok(JsonDocument.Parse(content));
     }



 }


When login you will get a link which when you open will ask you wheater to Aloow or Not.When you allow it will Give you token