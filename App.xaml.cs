using System.Windows;
using GuineaFootballClub.Erp.Data;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp;

public partial class App : Application
{
    public static AppDbContext Db { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var dbPath = System.IO.Path.Combine(
            @"C:\laragon\www\guinea_football_club_api",
            "guinea_football_club.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        Db = new AppDbContext(options);
        Db.Database.EnsureCreated();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Db.Dispose();
        base.OnExit(e);
    }
}

