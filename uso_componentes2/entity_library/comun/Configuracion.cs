namespace entity_library.Comun
{
    public class Configuracion
    {
        private static Configuracion configuracion;

        private Configuracion()
        {

        }

        public static Configuracion Instance 
        {
            get 
            {
                if(configuracion == null)
                {
                    configuracion = new Configuracion();
                }

                return configuracion;
            }
        }

        public string DefaultStringConnection { get; set; }
    }
}