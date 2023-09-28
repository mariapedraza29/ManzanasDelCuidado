namespace ManzanasDelCuidado.Modelos
{
    //creamos las clase ServMuj 
    public class ServMuj
    {
        //atributos de la clase mujeres con sus descriptores de acceso get y set
        public DateTime fechaInicio { get; set; }

        public string fechaFinal { get; set; }

        public byte documentoPdf { get; set; }

        public int fkDocMujeres { get; set; }

        public int fkCodigoServ1 { get; set; }
    }
}
