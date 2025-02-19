using System.Globalization;



namespace SistemaApoyo.API.Utilidad
{
    public class Response<T>
    {
        public bool status { get; set; }

        public string msg { get; set; }

        public T value { get; set; }

        public string token { get; set; }
    }
}

