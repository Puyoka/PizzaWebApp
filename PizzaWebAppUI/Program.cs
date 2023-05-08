using Blazored.Modal;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazoredModal();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddBlazoredToast();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim("jobTitle", "Admin");
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Worker", policy =>
    {
        policy.RequireClaim("jobTitle", "Worker");
    });
});

builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
builder.Services.AddSingleton<IOrderStatusData, MongoOrderStatusData>();
builder.Services.AddTransient<ICustomerData, MongoCustomerData>();
builder.Services.AddTransient<IIngredientData, MongoIngredientData>();
builder.Services.AddTransient<IOrderData, MongoOrderData>();
builder.Services.AddTransient<IProductData, MongoProductData>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//authe,autho
app.UseAuthentication();
app.UseAuthorization();

app.UseRewriter(new RewriteOptions().Add(
   context =>
   {
       if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
       {
           context.HttpContext.Response.Redirect("/");
       }
   }));


app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
