using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp.Views;

public partial class BoutiquePage : Page
{
    private List<Produit> _all = [];
    private List<Produit> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public BoutiquePage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Produits.Include(p => p.Club).ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(p => p.NomFr.ToLower().Contains(q)
                            || p.Categorie.ToString().ToLower().Contains(q)
                            || (p.Club?.Acronyme?.ToLower().Contains(q) ?? false)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgProduits.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} entrée{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgProduits.SelectedItem is Produit p)
            NavigationService?.Navigate(new ProduitFormPage(p));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new ProduitFormPage(null));
}
