using Microsoft.EntityFrameworkCore;
using GuineaFootballClub.Erp.Models;

namespace GuineaFootballClub.Erp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<ClubStat> ClubStats => Set<ClubStat>();
    public DbSet<Joueur> Joueurs => Set<Joueur>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Actualite> Actualites => Set<Actualite>();
    public DbSet<Palmares> Palmares => Set<Palmares>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Produit> Produits => Set<Produit>();
    public DbSet<Billet> Billets => Set<Billet>();
    public DbSet<ClassementEntree> Classements => Set<ClassementEntree>();
    public DbSet<User> Users => Set<User>();
}
