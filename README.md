<div align="center">

# ⚽ Guinea Football Club — ERP

**Application de gestion interne (ERP) pour les clubs de football guinéens**

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)
![WPF](https://img.shields.io/badge/WPF-Windows-0078D4?style=flat-square&logo=windows)
![Material Design](https://img.shields.io/badge/Material_Design-5.1-757575?style=flat-square&logo=materialdesign)
![EF Core](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat-square)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=flat-square&logo=sqlite)

</div>

---

## 📋 Description

**Guinea Football Club ERP** est une application bureau Windows qui centralise la gestion quotidienne d'un ou plusieurs clubs de football. Elle couvre l'ensemble des besoins opérationnels : effectif, compétitions, communication, boutique et billetterie, le tout dans une interface moderne **Material Design** avec thème clair, tableaux paginés et recherche instantanée.

---

## ✨ Fonctionnalités

| Module | Description |
|---|---|
| 📊 **Tableau de bord** | Vue synthétique des indicateurs clés et des derniers matchs |
| 🏙️ **Clubs** | Gestion des clubs (identité, ville, couleurs, logo) |
| 👤 **Joueurs** | Effectif complet par catégorie (Cadets · Juniors · Seniors) |
| ⚽ **Matchs** | Calendrier, scores, compétitions et statuts |
| 📰 **Actualités** | Articles publiables avec catégorisation |
| 🛍️ **Boutique** | Catalogue produits, stock, soldes et nouveautés |
| 🏟️ **Billets** | Gestion des places par type (Tribune · VIP · Pelouse · Loge) |
| 🏆 **Classement** | Tableau de classement par compétition et saison |
| 🥇 **Palmarès** | Historique des titres et distinctions |
| 📷 **Photos** | Galerie multimédia par club et catégorie |

Chaque module dispose de :
- **Recherche instantanée** multi-critères
- **Pagination** (15 lignes par page)
- **Formulaire d'édition** (ajout / modification)
- Tableau avec **bord arrondi et ombre portée**

---

## 🛠️ Stack technique

| Composant | Technologie |
|---|---|
| Framework | .NET 9 — WPF |
| UI Library | [Material Design In XAML Toolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) v5.1 |
| ORM | Entity Framework Core 9 |
| Base de données | SQLite |
| Langage | C# 13 |

---

## 🚀 Démarrage rapide

### Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10 / 11
- La base de données SQLite du projet API : [`guinea_football_club_api`](https://github.com/votre-username/guinea_football_club_api)

### Installation

```bash
git clone https://github.com/votre-username/guinea_football_club_erp.git
cd guinea_football_club_erp
dotnet restore
dotnet run
```

### Configuration

Par défaut, l'application cherche la base de données ici :

```
C:\laragon\www\guinea_football_club_api\guinea_football_club.db
```

Pour changer le chemin, modifier `App.xaml.cs` :

```csharp
var dbPath = System.IO.Path.Combine(
    @"VOTRE\CHEMIN\VERS\L'API",
    "guinea_football_club.db");
```

---

## 📁 Structure du projet

```
guinea_football_club_erp/
├── App.xaml                    # Thème Material Design (Light / DeepPurple)
├── App.xaml.cs                 # Point d'entrée, DbContext global (App.Db)
├── MainWindow.xaml             # Sidebar de navigation + Frame principale
├── MainWindow.xaml.cs          # Logique de navigation entre pages
│
├── Models/
│   └── Models.cs               # Entités EF Core (Club, Joueur, Match, …)
│
├── Data/
│   └── AppDbContext.cs         # Contexte SQLite Entity Framework Core
│
└── Views/                      # Une page XAML + CS par module
    ├── DashboardPage           # Tableau de bord avec stats et derniers matchs
    ├── ClubsPage / ClubFormPage
    ├── JoueursPage / JoueurFormPage
    ├── MatchesPage / MatchFormPage
    ├── ActualitesPage / ActualiteFormPage
    ├── BoutiquePage / ProduitFormPage
    ├── BilletsPage / BilletFormPage
    ├── ClassementPage / ClassementFormPage
    ├── PalmaresPage / PalmaresFormPage
    └── PhotosPage / PhotoFormPage
```

---

## 🗃️ Modèle de données

```
Club ──┬── Joueur
       ├── Match
       ├── Actualite
       ├── Palmares
       ├── Photo
       ├── Produit
       ├── Billet ── Match
       ├── ClassementEntree
       └── ClubStat
```

---

## 📸 Aperçu

> Interface claire, sidebar violet Material Design, tableaux avec ombre portée et pagination.

---

## 📄 Licence

Ce projet est sous licence **MIT**.

---

<div align="center">
  <sub>Développé pour la digitalisation du football guinéen 🇬🇳</sub>
</div>
