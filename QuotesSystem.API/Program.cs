using MySql.Data.MySqlClient;
using MySqlConnector;
using QuotesSystem.Infrastructure.Abstract;
using QuotesSystem.Infrastructure.Concrete;
using QuotesSystem.Repository.Abstract;
using QuotesSystem.Repository.Concrete;

namespace QuotesSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // My SQL Connection 
            builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Customer")!);
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ITestRepository, TestRepository>();
            builder.Services.AddTransient<ITestService, TestService>();
            builder.Services.AddTransient<IAssociateRepository, AssociateRepository>();
            builder.Services.AddTransient<IAssociateService, AssociateService>();
            builder.Services.AddTransient<IAdministrationRepository, AdministrationRepository>();
            builder.Services.AddTransient<IAdministrationService, AdministrationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
