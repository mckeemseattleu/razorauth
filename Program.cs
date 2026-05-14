using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;

// TODO 1. Make sure you use your own GOOGLE CLIENT ID and SECRET from Google Developer Console
// TODO 2. 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// ✅ Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "749302541068-ostnpk68bn32j5adeipr46rktj08rgs1.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.Scope.Add("email");
    options.Scope.Add("profile");

    // ✅ Map email claim explicitly (important)
    options.ClaimActions.MapJsonKey("email", "email");


});





builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Map Razor Pages
app.MapRazorPages();

// ✅ Login endpoint
app.MapGet("/login", async context =>
{
    await context.ChallengeAsync("Google", new AuthenticationProperties
    {
        RedirectUri = "/profile"
    });
});

// ✅ Logout endpoint
app.MapGet("/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.Run();