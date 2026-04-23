using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;

namespace guinea_football_club_erp.Views;

public partial class MatchFormPage : Page
{
    private readonly Match? _match;

    public MatchFormPage(Match? match)
    {
        InitializeComponent();
        _match = match;

        CbClub.ItemsSource      = App.Db.Clubs.ToList();
        CbLieu.ItemsSource      = Enum.GetValues<LieuMatch>();
        CbCategorie.ItemsSource = Enum.GetValues<Categorie>();
        CbStatut.ItemsSource    = Enum.GetValues<StatutMatch>();

        if (match != null)
        {
            TbTitle.Text             = $"Modifier — vs {match.Adversaire}";
            CbClub.SelectedValue     = match.ClubId;
            TbAdversaire.Text        = match.Adversaire;
            TbDate.Text              = match.DateMatch;
            TbCompetition.Text       = match.Competition;
            TbStade.Text             = match.Stade ?? "";
            CbLieu.SelectedItem      = match.Lieu;
            CbCategorie.SelectedItem = match.Categorie;
            CbStatut.SelectedItem    = match.Statut;
            TbScoreClub.Text         = match.ScoreClub?.ToString() ?? "";
            TbScoreAdversaire.Text   = match.ScoreAdversaire?.ToString() ?? "";
            BtnDelete.Visibility     = Visibility.Visible;
        }
        else
        {
            TbTitle.Text = "Nouveau match";
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new MatchesPage());

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var db = App.Db;
        int? sClub = int.TryParse(TbScoreClub.Text, out int sc) ? sc : null;
        int? sAdv  = int.TryParse(TbScoreAdversaire.Text, out int sa) ? sa : null;

        if (_match != null)
        {
            var m = db.Matches.Find(_match.Id)!;
            m.ClubId         = (string)CbClub.SelectedValue;
            m.Adversaire     = TbAdversaire.Text;
            m.DateMatch      = TbDate.Text;
            m.Competition    = TbCompetition.Text;
            m.Stade          = TbStade.Text;
            m.Lieu           = (LieuMatch)CbLieu.SelectedItem;
            m.Categorie      = (Categorie)CbCategorie.SelectedItem;
            m.Statut         = (StatutMatch)CbStatut.SelectedItem;
            m.ScoreClub      = sClub;
            m.ScoreAdversaire= sAdv;
        }
        else
        {
            db.Matches.Add(new Match
            {
                ClubId          = (string)CbClub.SelectedValue,
                Adversaire      = TbAdversaire.Text,
                DateMatch       = TbDate.Text,
                Competition     = TbCompetition.Text,
                Stade           = TbStade.Text,
                Lieu            = (LieuMatch)CbLieu.SelectedItem,
                Categorie       = (Categorie)CbCategorie.SelectedItem,
                Statut          = (StatutMatch)CbStatut.SelectedItem,
                ScoreClub       = sClub,
                ScoreAdversaire = sAdv
            });
        }
        db.SaveChanges();
        NavigationService?.Navigate(new MatchesPage());
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show($"Supprimer ce match contre '{_match!.Adversaire}' ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            var m = App.Db.Matches.Find(_match.Id);
            if (m != null) App.Db.Matches.Remove(m);
            App.Db.SaveChanges();
            NavigationService?.Navigate(new MatchesPage());
        }
    }
}
