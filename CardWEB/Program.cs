using Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapPost("/api/new-monster-card", CardCreator.CreateMonsterCard);
app.MapPost("/api/new-effect-card", CardCreator.CreateEffectCard);
app.MapPost("/api/new-game/{type}", Game.NewGame);
app.MapPost("/api/new-turn/{auto}", Game.NewTurn);
app.MapPost("/api/play", Game.Play);
app.MapPost("/api/attack/{card}/{target}", Game.Attack);
app.MapPost("/api/drop-card/{index}", Game.DropCard);
app.MapPost("/api/equip/{cardIndex}/{targetIndex}", Game.EquipPower);

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
