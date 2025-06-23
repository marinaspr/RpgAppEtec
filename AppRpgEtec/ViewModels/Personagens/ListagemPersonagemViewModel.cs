using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ObservableCollection<Personagem> Personagens { get; set; }
        //public Command NovoPersonagem { get; }

        private Personagem _personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get => _personagemSelecionado;
            set
            {
                if (_personagemSelecionado != value)
                {
                    _personagemSelecionado = value;
                    OnPropertyChanged();

                    if (_personagemSelecionado != null)
                    {
                        Shell.Current.GoToAsync($"cadPersonagemView?pId={_personagemSelecionado.Id}");
                    }
                }
            }
        }
        public ListagemPersonagemViewModel()
        {
            Personagens = new ObservableCollection<Personagem>();
            //NovoPersonagem = new Command(async () => { await ExibirCadastroPersonagem(); });
            NovoPersonagemCommand = new Command(async () => await ExibirCadastroPersonagem());
            RemoverPersonagemCommand = new Command<Personagem>(async (Personagem p) => { await RemoverPersonagem(p); });
        }
            public ICommand NovoPersonagemCommand { get; }
        public ICommand RemoverPersonagemCommand { get; set; }
        public async Task InicializarAsync()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            await ObterPersonagens();
        }
        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops!!", ex.Message + "Detalhes" + ex.InnerException, "Ok");
            }
        }
        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("cadPersonagemView");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ops", ex.Message + "detalhes: " + ex.InnerException, "OK");
            }
        } 
    public async Task RemoverPersonagem(Personagem p)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoçã0 de {p.Nome}?", "sim", "não"))
                {
                    await pService.DeletePersonagemAsync(p.Id);
                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Personagem removido com sucesso", "Ok");
                    _ = ObterPersonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " detalhes: " + ex.InnerException, "ok");
            }
        }
    }
}

