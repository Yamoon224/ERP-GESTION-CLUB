namespace GuineaFootballClub.Erp.Models;

public enum Categorie { Cadets, Juniors, Seniors }
public enum StatutMatch { Planifie, EnCours, Termine, Annule }
public enum LieuMatch { Domicile, Exterieur }
public enum TypePlace { Tribune, Pelouse, VIP, Loge }
public enum CategorieProduit { Jerseys, Accessories, Bags }

public class Club
{
    public string Id { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Acronyme { get; set; } = string.Empty;
    public string Fondation { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string CouleurPrimaire { get; set; } = "#1976D2";
    public string CouleurSecondaire { get; set; } = "#FFFFFF";

    public ICollection<Joueur> Joueurs { get; set; } = [];
    public ICollection<Match> Matches { get; set; } = [];
    public ICollection<Actualite> Actualites { get; set; } = [];
    public ICollection<Palmares> Palmares { get; set; } = [];
    public ICollection<Photo> Photos { get; set; } = [];
    public ICollection<Produit> Produits { get; set; } = [];
    public ICollection<Billet> Billets { get; set; } = [];
    public ICollection<ClubStat> Stats { get; set; } = [];
    public ICollection<ClassementEntree> Classements { get; set; } = [];
}

public class ClubStat
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class Joueur
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public string Prenom { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string DateNaissance { get; set; } = string.Empty;
    public string Poste { get; set; } = string.Empty;
    public string Taille { get; set; } = string.Empty;
    public Categorie Categorie { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
}

public class Match
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string DateMatch { get; set; } = string.Empty;
    public string Adversaire { get; set; } = string.Empty;
    public LieuMatch Lieu { get; set; }
    public Categorie Categorie { get; set; }
    public int? ScoreClub { get; set; }
    public int? ScoreAdversaire { get; set; }
    public string Competition { get; set; } = string.Empty;
    public StatutMatch Statut { get; set; }
    public string? Stade { get; set; }
}

public class Actualite
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string Titre { get; set; } = string.Empty;
    public string Contenu { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime DatePublication { get; set; } = DateTime.UtcNow;
    public string CategorieArticle { get; set; } = "Général";
    public bool EstPublie { get; set; } = true;
}

public class Palmares
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string Competition { get; set; } = string.Empty;
    public int Annee { get; set; }
    public string Rang { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class Photo
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string? Legende { get; set; }
    public DateTime DatePublication { get; set; } = DateTime.UtcNow;
    public string? CategoriePhoto { get; set; }
}

public class Produit
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string NomFr { get; set; } = string.Empty;
    public string NomEn { get; set; } = string.Empty;
    public CategorieProduit Categorie { get; set; }
    public string Prix { get; set; } = string.Empty;
    public string? Image { get; set; }
    public bool EstNouveau { get; set; }
    public bool EstEnSolde { get; set; }
    public string? AncienPrix { get; set; }
    public double Note { get; set; }
    public int NombreAvis { get; set; }
    public int Stock { get; set; } = 100;
}

public class Billet
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public int MatchId { get; set; }
    public Match Match { get; set; } = null!;
    public TypePlace TypePlace { get; set; }
    public string Prix { get; set; } = string.Empty;
    public int NombreDisponible { get; set; }
    public int NombreTotal { get; set; }
}

public class ClassementEntree
{
    public int Id { get; set; }
    public string ClubId { get; set; } = string.Empty;
    public Club Club { get; set; } = null!;
    public string Competition { get; set; } = string.Empty;
    public Categorie Categorie { get; set; }
    public string Saison { get; set; } = string.Empty;
    public int Position { get; set; }
    public int Joues { get; set; }
    public int Victoires { get; set; }
    public int Nuls { get; set; }
    public int Defaites { get; set; }
    public int ButsPour { get; set; }
    public int ButsContre { get; set; }
    public int Points { get; set; }
}

public enum RoleUser { Admin, Gestionnaire, Lecteur }

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string NomComplet { get; set; } = string.Empty;
    public string? Email { get; set; }
    public RoleUser Role { get; set; } = RoleUser.Gestionnaire;
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
}
