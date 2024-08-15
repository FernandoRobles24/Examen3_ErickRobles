using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Examen3_ErickRobles.Service;
using Examen3_ErickRobles.Models;
using Examen3_ErickRobles.Config;
using Examen3_ErickRobles.Converters;
namespace Examen3_ErickRobles
{
    public partial class MainPage : ContentPage
    {
        private readonly NotasService _notasService;
        private byte[] _currentPhoto;
        private Nota _selectedNota;

        public ObservableCollection<Nota> Notas { get; } = new ObservableCollection<Nota>();

        public MainPage()
        {
            InitializeComponent();
            _notasService = new NotasService();
            BindingContext = this;
            LoadNotas();
        }

        private async void LoadNotas()
        {
            var notas = await _notasService.GetAllNotas();
            foreach (var nota in notas.OrderByDescending(n => n.Fecha))
            {
                Notas.Add(nota);
            }
        }

        private async void OnAddNotaButtonClicked(object sender, EventArgs e)
        {
            if (_selectedNota == null)
            {
                var nota = new Nota
                {
                    Descripcion = descripcionEntry.Text,
                    Fecha = fechaPicker.Date.ToOADate(),
                    Photo_Record = _currentPhoto
                };

                await _notasService.AddNota(nota);
                Notas.Insert(0, nota);
            }
            else
            {
                _selectedNota.Descripcion = descripcionEntry.Text;
                _selectedNota.Fecha = fechaPicker.Date.ToOADate();
                _selectedNota.Photo_Record = _currentPhoto;

                await _notasService.UpdateNota(_selectedNota);
                var index = Notas.IndexOf(_selectedNota);
                Notas[index] = _selectedNota;
                _selectedNota = null;
            }

            ClearInputs();
        }

        private void ClearInputs()
        {
            descripcionEntry.Text = string.Empty;
            fechaPicker.Date = DateTime.Now;
            photoImage.Source = null;
            _currentPhoto = null;
        }

        private async Task<byte[]> CapturePhotoAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso denegado", "No se puede acceder a la cámara", "OK");
                    return null;
                }

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return null;

                using var stream = await photo.OpenReadAsync();
                using var memoryStream = new System.IO.MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }

        private async void OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            _currentPhoto = await CapturePhotoAsync();
            if (_currentPhoto != null)
            {
                photoImage.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(_currentPhoto));
                await DisplayAlert("Foto Capturada", "La foto ha sido capturada y almacenada.", "OK");
            }
        }

        private void OnEditNotaButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var nota = button?.CommandParameter as Nota;
            if (nota != null)
            {
                _selectedNota = nota;
                descripcionEntry.Text = nota.Descripcion;
                fechaPicker.Date = DateTime.FromOADate(nota.Fecha);
                _currentPhoto = nota.Photo_Record;
                photoImage.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(nota.Photo_Record));
            }
        }

        private async void OnDeleteNotaButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var nota = button?.CommandParameter as Nota;
            if (nota != null)
            {
                await _notasService.DeleteNota(nota.Id_nota);
                Notas.Remove(nota);
            }
        }
    }

}
