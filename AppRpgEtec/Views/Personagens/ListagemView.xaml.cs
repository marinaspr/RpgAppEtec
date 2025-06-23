using AppRpgEtec.ViewModels.Personagens;
using System.Threading.Tasks;

namespace AppRpgEtec.Views.Personagens;

public partial class ListagemView : ContentPage
{
	ListagemPersonagemViewModel viewModel;

	public ListagemView()
	{
		InitializeComponent();

		viewModel = new ListagemPersonagemViewModel();
		BindingContext = viewModel;
		Title = "Personagens - App Rpg Etec";
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var viewModel = BindingContext as ListagemPersonagemViewModel;
            if (viewModel != null)
            {
                await viewModel.InicializarAsync();
                _ = viewModel.ObterPersonagens();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao carregar personagens: {ex.Message}", "OK");
        }
    }

}