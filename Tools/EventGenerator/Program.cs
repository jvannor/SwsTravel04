using DotNetEnv;
using Microsoft.Data.SqlClient;
using System.Threading;

Env.Load();
var connecctionString = Environment.GetEnvironmentVariable("ConnectionString");
var interval = TimeSpan.Parse(Environment.GetEnvironmentVariable("Interval"));
var eventCode = Environment.GetEnvironmentVariable("EventCode");

using (var conn = new SqlConnection(connecctionString))
{
    var cmd = conn.CreateCommand();
    cmd.CommandText = $"INSERT INTO [EVENT] (EventCode) VALUES ('{eventCode}')";
    
    conn.Open();
    for (int i=0; i<int.MaxValue; i++)
    {
        Console.Write($"Sending event {i}...  ");
        var count = cmd.ExecuteNonQuery();
        Console.WriteLine("Done!");
        
        Thread.Sleep(interval);
    }
}
