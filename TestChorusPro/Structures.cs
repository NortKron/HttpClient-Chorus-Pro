using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestChorusPro
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class FlowDeposit
    {
        [JsonProperty("codeRetour")]
        public int CodeRetour { get; set; }

        [JsonProperty("libelle")]
        public string Libelle { get; set; }

        [JsonProperty("numeroFluxDepot")]
        public string NumeroFluxDepot { get; set; }

        [JsonProperty("dateDepot")]
        public string DateDepot { get; set; }

        [JsonProperty("syntaxeFlux")]
        public string SyntaxeFlux { get; set; }
    }

    public class FlowDetail
    {
        [JsonProperty("codeRetour")]
        public int? CodeRetour { get; set; }

        [JsonProperty("libelle")]
        public string? Libelle { get; set; }

        [JsonProperty("dateDepotFlux")]
        public string? DateDepotFlux { get; set; }

        [JsonProperty("etatCourantDepotFlux")]
        public string? EtatCourantDepotFlux { get; set; }

        [JsonProperty("numeroDP")]
        public string? NumeroDP { get; set; }

        [JsonProperty("listeErreurDP")]
        public List<ListErrorDP>? ListeErreurDP { get; set; }

        [JsonProperty("listeErreurTechnique")]
        public List<ListErrorTechnical>? ListeErreurTechnique { get; set; }
    }

    public class ListErrorDP
    {
        [JsonProperty("identifiantFournisseur")]
        public string IdentifiantFournisseur { get; set; }

        [JsonProperty("identifiantDestinataire")]
        public string IdentifiantDestinataire { get; set; }

        [JsonProperty("numeroDP")]
        public string NumeroDP { get; set; }

        [JsonProperty("libelleErreurDP")]
        public string LibelleErreurDP { get; set; }
    }

    public class ListErrorTechnical
    {
        [JsonProperty("natureErreur")]
        public string NatureErreur { get; set; }

        [JsonProperty("codeErreur")]
        public string CodeErreur { get; set; }

        [JsonProperty("libelleErreur")]
        public string LibelleErreur { get; set; }
    }
}
