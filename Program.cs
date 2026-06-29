using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModuleHelpDeskTimesheet.Consumers;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── Authentication — accepte tokens ERP frontend ET n8n-service ─────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("erpApplication", options =>
    {
        options.Authority = "https://authentik.itanis.tn/application/o/erp-application/";
        options.Audience = "BGnXFXMepfj4wh0AVli40YPWPjTFs9SgBxf1Udxk";
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    })
    .AddJwtBearer("n8nService", options =>
    {
        options.Authority = "https://authentik.itanis.tn/application/o/n8nservice/";
        options.Audience = "rzIkG93Do9dvuzvxcnLTFK87Qz2COItxuqe6p7Yb";
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    var defaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(
            "erpApplication", "n8nService")
        .RequireAuthenticatedUser()
        .Build();
    options.DefaultPolicy = defaultPolicy;
    options.FallbackPolicy = defaultPolicy;
});

var connectionString = builder.Configuration.GetConnectionString("TimesheetConnection");
builder.Services.AddDbContext<TimesheetDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeclarationTempsSyncConsumer>();
    x.AddConsumer<AgentSyncConsumer>();
    x.AddConsumer<TicketSyncConsumer>();
    x.AddConsumer<TicketCollaborateurSyncConsumer>();
    x.AddConsumer<TimesheetSessionConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("51.254.133.231", 31672, "/", h =>
        {
            h.Username("admin");
            h.Password("rabbitMQ-dev");
        });

        cfg.ReceiveEndpoint("timesheet-declaration-temps-sync", e =>
        {
            e.ConfigureConsumer<DeclarationTempsSyncConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("timesheet-agent-sync", e =>  
        {
            e.ConfigureConsumer<AgentSyncConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("timesheet-ticket-sync", e =>
        {
            e.ConfigureConsumer<TicketSyncConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("timesheet-ticket-collaborateur-sync", e =>
        {
            e.ConfigureConsumer<TicketCollaborateurSyncConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("timesheet-session-sync", e =>
        {
            e.ConfigureConsumer<TimesheetSessionConsumer>(ctx);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();           // Valide le token Authentik
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Démarrage du Module Timesheet sur le serveur distant...");

app.Run();
