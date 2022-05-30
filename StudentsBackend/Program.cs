using StudentsRegistrations.DB;
using StudentsRegistrations.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StudentStoreDbConfig>(
    builder.Configuration.GetSection("StudentStoreDbConfig"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        policy => policy.AllowAnyOrigin().
            AllowAnyMethod().AllowAnyHeader()
        );

});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbClient, DbClient>();
builder.Services.AddTransient<IStudentServices, StudentServices>();
// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
