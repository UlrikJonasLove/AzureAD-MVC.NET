using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// GL�M INTE ATT TA BORT INFON
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
  {
      options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.Authority = "https://login.microsoftonline.com/[YOUR_TENANT_ID]/v2.0";
      options.ClientId = "[YOUR_CLIENT_ID]";
      options.ResponseType = "code";
      options.SaveTokens = true;
      options.ClientSecret = "[YOUR_CLIENT_SECRET_VALUE]"; // the value of the client secret
      options.Scope.Add("api://[API_KEY]/AdminAccess");
      options.TokenValidationParameters = new TokenValidationParameters
      {
          NameClaimType = "name" //this will display the name when logged in
      };
  });
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
