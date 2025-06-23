using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views.Usuarios;

public partial class LoginView : ContentPage
{
	UsuarioViewModel usuarioViewModel;
	public LoginView()
	{
		InitializeComponent();

		usuarioViewModel = new UsuarioViewModel();
		BindingContext = usuarioViewModel;
	}
    private void BtnLogin_Clicked(object sender, EventArgs e)
    {
        // Simula login e token recebido da API
        string tokenRecebido = "abc123def456";

        // Salva o token
        Preferences.Set("UsuarioToken", tokenRecebido);

        DisplayAlert("Sucesso", "Token salvo com sucesso!", "OK");
    }

    private void BtnVerificarToken_Clicked(object sender, EventArgs e)
    {
        string token = Preferences.Get("UsuarioToken", string.Empty);

        if (!string.IsNullOrEmpty(token))
            DisplayAlert("Token", $"Token atual: {token}", "OK");
        else
            DisplayAlert("Token", "Nenhum token encontrado.", "OK");
    }
}