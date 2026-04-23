using System.Windows;
using GuineaFootballClub.Erp.Data;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp;

public partial class App : Application
{
    public static AppDbContext Db { get; private set; } = null!;
    public static User CurrentUser { get; set; } = null!;

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

        // Créer le compte admin par défaut si aucun utilisateur n'existe
        if (!Db.Users.Any())
        {
            var (hash, salt) = PasswordHelper.Hash("admin");
            Db.Users.Add(new User
            {
                Username     = "admin",
                PasswordHash = hash,
                PasswordSalt = salt,
                NomComplet   = "Administrateur",
                Role         = RoleUser.Admin,
                EstActif     = true
            });
            Db.SaveChanges();
        }

        // Afficher la fenêtre de connexion
        var login = new LoginWindow();
        if (login.ShowDialog() != true)
        {
            Shutdown();
            return;
        }
        CurrentUser = login.AuthenticatedUser!;

        var main = new MainWindow();
        main.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Db?.Dispose();
        base.OnExit(e);
    }
}

