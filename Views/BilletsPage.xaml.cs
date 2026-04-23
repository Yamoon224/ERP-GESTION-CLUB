using System.Windows;
using System.Windows.Controls;
using GuineaFootballClub.Erp.Models;
using Microsoft.EntityFrameworkCore;

namespace guinea_football_club_erp.Views;

public partial class BilletsPage : Page
{
    private List<Billet> _all = [];
    private List<Billet> _filtered = [];
    private int _page = 1;
    private const int PageSize = 15;

    public BilletsPage()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _all = App.Db.Billets.Include(b => b.Club).Include(b => b.Match).ToList();
        _filtered = _all;
        _page = 1;
        Refresh();
    }

    private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        var q = TbSearch.Text.Trim().ToLower();
        _filtered = string.IsNullOrEmpty(q)
            ? _all
            : _all.Where(b => (b.Club?.Acronyme?.ToLower().Contains(q) ?? false)
                            || (b.Match?.Adversaire?.ToLower().Contains(q) ?? false)
                            || b.TypePlace.ToString().ToLower().Contains(q)).ToList();
        _page = 1;
        Refresh();
    }

    private void Refresh()
    {
        int total = _filtered.Count;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        DgBillets.ItemsSource = _filtered.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
        TbPageInfo.Text = $"Page {_page} / {pages}  ({total} entrée{(total > 1 ? "s" : "")})";
        BtnPrev.IsEnabled = _page > 1;
        BtnNext.IsEnabled = _page < pages;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e) { _page--; Refresh(); }
    private void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; Refresh(); }

    private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DgBillets.SelectedItem is Billet b)
            NavigationService?.Navigate(new BilletFormPage(b));
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        => NavigationService?.Navigate(new BilletFormPage(null));
}
