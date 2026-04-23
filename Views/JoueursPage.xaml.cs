using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp.Views;

public partial class JoueursPage : Page
{
    private List<Joueur> _all = [];
    private List<Joueur> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public JoueursPage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Joueurs.Include(j => j.Club).OrderBy(j => j.Nom).ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(j => j.Nom.ToLower().Contains(q)
                            || j.Prenom.ToLower().Contains(q)
                            || j.Poste.ToLower().Contains(q)
                            || (j.Club?.Nom?.ToLower().Contains(q) ?? false)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgJoueurs.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} entrée{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void DgJoueurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgJoueurs.SelectedItem is Joueur j)
            NavigationService?.Navigate(new JoueurFormPage(j));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new JoueurFormPage(null));
}
