using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TestCntrl.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<ScenarioService>();
builder.Services.AddSingleton<InterfaceService>();
builder.Services.AddSingleton<PositionService>();
builder.Services.AddSingleton<VendorService>();
builder.Services.AddSingleton<VendorFEService>();
builder.Services.AddSingleton<GenerateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
