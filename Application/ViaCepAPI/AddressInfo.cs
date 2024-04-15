using System.Text.Json.Serialization;

namespace Application.ViaCepAPI
{
    public class AddressInfo
    {
        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }
    }
}
