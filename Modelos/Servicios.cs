namespace ManzanasDelCuidado.Modelos
{
    //creamos las clase Servicios 
    public class Servicios
    {
        //atributos de la clase Servicios con sus descriptores de acceso get y set
        public int codigoServ { get; set; }

        public string nombre { get; set; }

        public string localidad { get; set; }

        public string direccion { get; set; }

        public int fkCodigoMunc { get; set; }
    }
}
