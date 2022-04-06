using Microsoft.AspNetCore;

using Web;

WebHost.CreateDefaultBuilder()
       .UseStartup<Startup>()
       .Build()
       .Run();
