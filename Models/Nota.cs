using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen3_ErickRobles.Models
{
    public class Nota
    {
        public int Id_nota { get; set; }
        public string Descripcion { get; set; }
        public double Fecha { get; set; } // Fecha en formato OADate
        public byte[] Photo_Record { get; set; }

        public string FechaFormateada => DateTime.FromOADate(Fecha).ToString("MM/dd/yyyy");
    }
}
