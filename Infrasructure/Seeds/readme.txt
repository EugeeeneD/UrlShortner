// that is for seeding roles
//builder.Services.AddScoped<IDbContextSeed, SeedRole>();

//var app = builder.Build();

// that is for seeding roles
// should be run once
//using var scope = app.Services.CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<UrlDbContext>();
//await dbContext.Database.MigrateAsync();

// Seed the database with roles
//var dbContextSeed = scope.ServiceProvider.GetRequiredService<IDbContextSeed>();
//await dbContextSeed.SeedAsync(dbContext);
