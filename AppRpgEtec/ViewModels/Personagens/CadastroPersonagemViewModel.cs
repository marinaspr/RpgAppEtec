﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty("PersonagemSelecionadoId", "pId")]
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; set; }
       
        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });
            CancelarCommand = new Command(async () => { await CancelarCadastro(); });

        }

        private async Task CancelarCadastro()
        { await Shell.Current.GoToAsync(".."); }

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;

        private PersonagemService pService;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public string Nome { get => nome; set { nome = value; OnPropertyChanged(); } }
        public int PontosVida { get => pontosVida; set { pontosVida = value; OnPropertyChanged(); } }
        public int Forca { get => forca; set { forca = value; OnPropertyChanged(); } }
        public int Defesa { get => defesa; set { defesa = value; OnPropertyChanged(); } }
        public int Inteligencia { get => inteligencia; set { inteligencia = value; OnPropertyChanged(); } }
        public int Disputas { get => disputas; set { disputas = value; OnPropertyChanged(); } }
        public int Vitorias { get => vitorias; set { vitorias = value; OnPropertyChanged(); } }
        public int Derrotas { get => derrotas; set { derrotas = value; OnPropertyChanged(); } }

        private ObservableCollection<TipoClasse> listaTipoClasse;
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTipoClasse; }
            set
            {
                if (value != null)
                {
                    listaTipoClasse = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task ObterClasses()
        {
            try
            {
                ListaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "Clerigo" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private TipoClasse tipoClasseSelecionado;
        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != tipoClasseSelecionado)
                {
                    tipoClasseSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PersonagemSelecionadoId { get; set; }

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = this.nome,
                    PontosVida = this.pontosVida,
                    Defesa = this.defesa,
                    Derrotas = this.derrotas,
                    Disputas = this.disputas,
                    Forca = this.forca,
                    Inteligencia = this.inteligencia,
                    Vitorias = this.vitorias,
                    Id = this.id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                    await Application.Current.MainPage
                            .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                        .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
        public async void CarregarPersonagem()
        {
            try
            {
                Personagem p = await
                    pService.GetPersonagemAsync(int.Parse(PersonagemSelecionadoId));
                this.Nome = p.Nome;
                this.PontosVida = p.PontosVida;
                this.Defesa = p.Defesa;
                this.Derrotas = p.Derrotas;
                this.Disputas = p.Disputas;
                this.Forca = p.Forca;
                this.Inteligencia = p.Inteligencia;
                this.Vitorias = p.Vitorias;
                this.Id = p.Id;

                TipoClasseSelecionado = this.ListaTiposClasse.FirstOrDefault(tClasse => tClasse.Id == (int)p.Classe);

            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "ok");
            }

        }
        public string personagemSelecionadoId
            
        {
            get => personagemSelecionadoId;
            set
            {
                personagemSelecionadoId = Uri.UnescapeDataString(value);
                CarregarPersonagem();
            }
        }

    }
}

