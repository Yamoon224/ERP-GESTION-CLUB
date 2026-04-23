using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp.Views;

public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        TbDate.Text = $"Aujourd'hui : {DateTime.Now:dddd dd MMMM yyyy}";
        LoadStats();
    }

    private void LoadStats()
    {
        var db = App.Db;
        var clubs     = db.Clubs.Count();
        var joueurs   = db.Joueurs.Count();
        var matchs    = db.Matches.Count();
        var actualites = db.Actualites.Count();
        var produits  = db.Produits.Count();
        var billets   = db.Billets.Count();

        AddCard("Clubs",       clubs.ToString(),     Color.FromRgb(103, 58, 183));
        AddCard("Joueurs",     joueurs.ToString(),   Color.FromRgb(0, 150, 136));
        AddCard("Matchs",      matchs.ToString(),    Color.FromRgb(63, 81, 181));
        AddCard("Actualités",  actualites.ToString(),Color.FromRgb(255, 152, 0));
        AddCard("Produits",    produits.ToString(),  Color.FromRgb(76, 175, 80));
        AddCard("Billets",     billets.ToString(),   Color.FromRgb(244, 67, 54));

        var matches = db.Matches.Include(m => m.Club)
                                .OrderByDescending(m => m.DateMatch)
                                .Take(10)
                                .ToList();
        var vm = matches.Select(m => new {
            m.Club,
            m.Adversaire,
            m.DateMatch,
            Score = m.ScoreClub.HasValue ? $"{m.ScoreClub} - {m.ScoreAdversaire}" : "—",
            m.Statut
        });
        DgMatches.ItemsSource = vm.ToList();
    }

    private void AddCard(string label, string value, Color color)
    {
        var card = new Border
        {
            Width = 160, Height = 100,
            Margin = new Thickness(0, 0, 16, 16),
            CornerRadius = new CornerRadius(12),
            Background = new SolidColorBrush(color) { Opacity = 0.85 }
        };
        var sp = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(16) };
        sp.Children.Add(new TextBlock { Text = value, FontSize = 32, FontWeight = FontWeights.Bold, Foreground = Brushes.White });
        sp.Children.Add(new TextBlock { Text = label, FontSize = 13, Foreground = Brushes.White });
        card.Child = sp;
        CardsPanel.Children.Add(card);
    }
}
