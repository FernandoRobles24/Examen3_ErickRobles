using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen3_ErickRobles.Config;
using Examen3_ErickRobles.Models;

namespace Examen3_ErickRobles.Service
{
    public class NotasService
    {
        private readonly FirebaseClient _firebaseClient;

        public NotasService()
        {
            _firebaseClient = new FirebaseClient("https://examen3er-14cc0-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Nota>> GetAllNotas()
        {
            return (await _firebaseClient
            .Child("Notas")
                .OnceAsync<Nota>())
                .Select(item => new Nota
                {
                    Id_nota = item.Object.Id_nota,
                    Descripcion = item.Object.Descripcion,
                    Fecha = item.Object.Fecha,
                    Photo_Record = item.Object.Photo_Record
                }).ToList();
        }

        public async Task AddNota(Nota nota)
        {
            await _firebaseClient
                .Child("Notas")
                .PostAsync(nota);
        }

        public async Task UpdateNota(Nota nota)
        {
            var toUpdateNota = (await _firebaseClient
            .Child("Notas")
                .OnceAsync<Nota>())
                .FirstOrDefault(a => a.Object.Id_nota == nota.Id_nota);

            await _firebaseClient
                .Child("Notas")
                .Child(toUpdateNota.Key)
                .PutAsync(nota);
        }

        public async Task DeleteNota(int idNota)
        {
            var toDeleteNota = (await _firebaseClient
                .Child("Notas")
                .OnceAsync<Nota>())
                .FirstOrDefault(a => a.Object.Id_nota == idNota);

            await _firebaseClient
                .Child("Notas")
                .Child(toDeleteNota.Key)
                .DeleteAsync();
        }
    }
}
