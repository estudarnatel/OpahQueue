using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpahQueue.Data;
using OpahQueue.Helpers;
using OpahQueue.Services;
var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the DI container.
builder.Services.AddCors();
builder.Services.AddControllers();
// builder.Services.AddControllers().AddNewtonsoftJson(); //PESQUISAR COMO USAR DEPOIS
// configure strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// configure DI for application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<DBContext>(opt =>    
    // opt.UseInMemoryDatabase("TodoList"));
    // opt.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;"));
    opt.UseSqlServer(connString));    
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
// configure DI for application services
// builder.Services.AddDbContext<DBContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext") ?? throw new InvalidOperationException("Connection string 'DBContext' not found.")));
