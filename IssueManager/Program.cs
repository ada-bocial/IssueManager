using IssueManager.Application.DTOs.Issue;
using IssueManager.Application.DTOs.Project;
using IssueManager.Application.DTOs.User;
using IssueManager.Application.IServices;
using IssueManager.Application.Services;
using IssueManager.Application.Utils;
using IssueManager.Domain.IRepositories;
using IssueManager.Infrastructure;
using IssueManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddOpenApi();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
        builder.Services.AddDbContext<ManagerDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration["Database:SqlLiteConnectionString"]);
        });

        builder.Services.AddScoped<IIssueRepository, IssueRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

        builder.Services.AddScoped<IIssueService, IssueService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IAppUserService, AppUserService>();
        var secret = "660a44324e36fb9baeeb50f0389e8cf501b7a9b4a7517e9736dd7d3aa51db52d586f1cdf889a63ed4c242420284ea962602fdc11e18c5b9589b646dcb6593038e3ce97557939dd7da165dd18acbe65f2aefcefc177e78c78499828f9fa42d88b1cb6fc6ae060c1930d691df8e490e0b2a250c115e48477fa54141628a18ca12c7848f7a14f76647c72da9d9b6a4747471a218d555a147f3f8339f7fbd1e9976e3b38212192d4146867bccc89b1b417910cc45ede542d3a7f29de4c39a28e71e6f12090574bd8a9d5f62b298d15bd73a946c9151e4a4a2cebf89ba98732782335020823bca7d778b1178b9f33730bf16d416020caaa17130d07d45736999c8529ce0250f5c6803aa5422ea283c7cfd1be4ceb6faf9e32a63b0da4f6e4a4d5e2ee60d8a6fe9e85d80c5a64b497e8e49cc7c75057979a524a7a52b19c57cd477ef6c8373f0181aad916b8854def796cc2d1ddcae6d47e101e18e8895315101feff72ce485a5f31dd4b3c8f0f7e7566564834752b1e1e12cf0dd67491676910d38bb58dedd26412665d4a27a0a18caa5c8df6e0c85d83e3f4cec43b91d0829b131d6a834f5965972acfdf87f0e5bca09d0bb7c47b000a671732b939a8ea9c109db1fe65584d63c393f6ba9a243bcb6e8e104e57a51e147bd603fb5651711a5e680e27360ae05cffcfdfcea9230de2d59b0dde049b829a0f7efda2437d7174011e543";


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = "http://localhost:7196";
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "defaultIssuer",
                ValidAudience = "defaultAudience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
            };
        });

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
        });
        builder.Services.AddAuthorization();
        var app = builder.Build();
        
        
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Manager Api");
                
            });
            app.MapGet("/", async context =>
            {
                await Task.Run(() => context.Response.Redirect("./swagger/index.html", permanent: false));
            });
        }
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseAuthorization();


        app.MapPost("/login", async (LoginDto dto, IAppUserService userService) =>
        {
            var result = await userService.LoginAsync(dto);
            if (result == null)
                return Results.NotFound();
            return Results.Ok(result);
        }).WithName("Login");
        #region Project Endpoints

        app.MapGet("/project", (IProjectService projectService) =>
        {
            return projectService.GetAllProjects();
        }).RequireAuthorization(c => c.RequireRole("Developer","Admin"))
          .WithName("Get projects list");
        app.MapGet("/project/{id}", async (int id, IProjectService projectService) =>
        {
            return await projectService.GetProjectDetailsAsync(id);
        })
        .RequireAuthorization(c => c.RequireRole("Developer", "Admin"))
        .WithName("Get project details");
        app.MapPost("/project", async (NewProjectDto dto, IProjectService projectService) =>
        {
            return await projectService.CreateProjectAsync(dto);
        })
        .RequireAuthorization(c => c.RequireRole("Admin"))
        .WithName("Add project");
        app.MapPut("/project", async (UpdateProjectDto dto, IProjectService projectService) =>
        {
            return await projectService.UpdateProjectAsync(dto);
        })
        .RequireAuthorization(c => c.RequireRole("Admin"))
        .WithName("Update project");
        app.MapDelete("/project/{id}", async (int id, bool isForced, IProjectService projectService) =>
        {
            if(await projectService.DeleteProjectAsync(id, isForced))
                return Results.Ok();
            return Results.BadRequest("Cannot delete project. Consider using force delete with dependencies");
        })
        .RequireAuthorization(c => c.RequireRole("Admin"))
        .WithName("Delete project");
        #endregion

        #region Issue Endpoints

        app.MapGet("/issue", (int projectId, IIssueService issueService) =>
        {
            return issueService.GetProjectIssuesAsync(projectId);
        })
        .RequireAuthorization(c => c.RequireRole("Developer", "Admin"))
        .WithName("Get issues list for project");
        app.MapGet("/issue/{id}", (int id, IIssueService issueService) =>
        {
            return issueService.GetIssueDetails(id);
        })
        .RequireAuthorization(c => c.RequireRole("Developer", "Admin"))
        .WithName("Get issue details");
        app.MapPost("/issue", async (NewIssueDto dto, IIssueService issueService) =>
        {
            return await issueService.CreateIssueAsync(dto);
        })
        .RequireAuthorization(c => c.RequireRole("Admin"))
        .WithName("Add issue");
        app.MapPut("/issue", async (UpdateIssueDto dto, IIssueService issueService) =>
        {
            return await issueService.UpdateIssueAsync(dto);
        })
        .RequireAuthorization(c => c.RequireRole("Developer", "Admin"))
        .WithName("Update issue");
        app.MapDelete("/issue/{id}", async (int id, IIssueService issueService) =>
        {
            if (await issueService.DeleteIssueAsync(id))
                return Results.Ok();
            return Results.BadRequest("Cannot delete issue");
        })
        .RequireAuthorization(c => c.RequireRole("Admin"))
        .WithName("Delete issue");
        app.Run();
        #endregion
    }



    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
            {
                var requirements = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", 
                        In = ParameterLocation.Header,
                        BearerFormat = "Json Web Token"
                    }
                };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = requirements;
                foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
                {
                    operation.Value.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
                    });
                }
            }
        }
    }
}

