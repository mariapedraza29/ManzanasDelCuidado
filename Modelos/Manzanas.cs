namespace ManzanasDelCuidado.Modelos
{
    //creamos las clase Manzanas 
    public class Manzanas
    {
        //atributos de la clase Manzanas con sus descriptores de acceso get y set
        public int codigoManz{get; set;}

        public string nombre { get; set;}

        public string localidad { get; set;}

        public string direccion { get; set;}

        public int fkCodigoMunc { get; set;}
    }
}
