using GalaSoft.MvvmLight.Command;
using MangasMVVM.Model;
using MangasMVVM.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangasMVVM.ViewModel
{
    public class MangasViewModel : INotifyPropertyChanged
    {
        private Manga? manga;

        

        public ObservableCollection<Manga> Lista { get; set; } = new ObservableCollection<Manga>();
       
        public Manga? Manga 
        {
            get { return manga; }
            set { manga = value; PropertyChange("Manga"); }
        }
        int PosMangaAEditar;

        public string Vista { get; set; } = "Ver";
        public string Error { get; set; } = "";
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public MangasViewModel()
        {
            CargarManga();
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            ShowCommand = new RelayCommand<string>(Show);
            GuardarCommand = new RelayCommand(Guardar);
        }

        private void Guardar()
        {
            if (Manga != null)
            {
                Lista[PosMangaAEditar] = Manga;
                GuardarManga();
                Cancelar();
            }
        }

        private void Show(string obj)
        {
            Vista = obj;
            if(Vista == "Agregar")
            {
                Manga = new Manga();
            }
            if(Vista == "Editar")
            {
                if(manga != null)
                {
                    Manga copia = new Manga()
                    {
                        Titulo = manga.Titulo,
                        Genero = manga.Genero,
                        Autor = manga.Autor,
                        NumTomos = manga.NumTomos,
                        Sinopsis = manga.Sinopsis,
                        Imagen = manga.Imagen
                    };
                    PosMangaAEditar = Lista.IndexOf(manga);
                    Manga = copia;
                }
            }
            
            PropertyChange();           
        }

        private void Cancelar()
        {
            Show("Ver");
            Manga = null;
        }

        private void Eliminar()
        {
            if(Manga != null)
            {
                Lista.Remove(Manga);
                GuardarManga();
                PropertyChange();
            }
        }

        public void Agregar()
        {
            if (Manga != null)
            {
                if (string.IsNullOrWhiteSpace(Manga.Titulo))
                {
                    Error = "Escribe el título del manga";
                    PropertyChange("Error"); return;
                }
                if (string.IsNullOrWhiteSpace(Manga.Genero))
                {
                    Error = "Escribe el género del manga";
                    PropertyChange("Error"); return;
                }
                if (string.IsNullOrWhiteSpace(Manga.Autor))
                {
                    Error = "Escribe el autor del manga";
                    PropertyChange("Error"); return;
                }

                if (string.IsNullOrWhiteSpace(Manga.Sinopsis))
                {
                    Error = "Escribe la sinopsis del manga";
                    PropertyChange("Error"); return;
                }
                if (string.IsNullOrWhiteSpace(Manga.Imagen))
                {
                    Error = "Escribe la url de la imágen del manga";
                    PropertyChange("Error"); return;
                }
                if (!Uri.TryCreate(Manga.Imagen, UriKind.Absolute, out var uri))
                {
                    Error = "Escribe una url valida para la imágen del manga";
                    PropertyChange("Error"); return;
                }
                Lista.Add(Manga);
                Show("Ver");
                GuardarManga();

            }
        }

      

        private void GuardarManga()
        {
            var json = JsonConvert.SerializeObject(Lista);
            File.WriteAllText("mangas.json", json);
        }

        public void CargarManga()
        {
            if(File.Exists("mangas.json"))
            {
                var json = File.ReadAllText("mangas.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Manga>?>(json);
                
                if(datos != null)
                {
                   Lista = datos;
                }
                else
                {
                   Lista = new ObservableCollection<Manga>();
                }
            }
 
        }
        void PropertyChange(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
