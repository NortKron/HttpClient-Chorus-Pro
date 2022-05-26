using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace TestChorusPro
{
    public class ChrorusProClient
    {
        // URL API to get the token
        private static string URL_TOKEN_REQUEST         = "https://sandbox-oauth.aife.economie.gouv.fr/api/oauth/token";

        // URL API to send the invoice in the XML format
        private static string URL_UBL_SEND              = "https://sandbox-api.piste.gouv.fr/cpro/factures/v1/deposer/flux";

        // URL API to request Flow status
        private static string URL_GET_STATUS_INVOICE    = "https://sandbox-api.piste.gouv.fr/cpro/transverses/v1/consulterCRDetaille";

        // CLIENT_ID and CLIENT_SECRET from the application from the https://piste.gouv.fr/
        private static string CLIENT_ID     = "b3295361-1234-4219-86d3-e71007c91644";
        private static string CLIENT_SECRET = "ab80b483-4321-4daf-8623-bdda2826c617";
        private static string GRANT_TYPE    = "client_credentials";
        private static string SCOPE         = "openid";

        // Login and password for technical account Chorus Pro
        private static string CPRO_TECH_LOGIN   = "TECH_1_1234567890@cpro.fr";
        private static string CPRO_TECH_PASS    = "password1234";

        private string CPRO_TECH_ACCOUNT_ENCODED = "";

        HttpClient client = new HttpClient();

        public Token GetToken()
        {
            var form = new Dictionary<string, string>
            {
                {"grant_type", GRANT_TYPE},
                {"client_id", CLIENT_ID},
                {"client_secret", CLIENT_SECRET},
                {"scope", SCOPE}
            };

            HttpResponseMessage tokenResponse = client.PostAsync(URL_TOKEN_REQUEST, new FormUrlEncodedContent(form)).Result;
            var jsonContent = tokenResponse.Content.ReadAsStringAsync().Result;
            Token ret = JsonConvert.DeserializeObject<Token>(jsonContent);
             
            return ret;
        }

        public FlowDeposit SubmitUBL(Token token, string pathFileXML)
        {
            string contentFile = File.ReadAllText(pathFileXML);
            string contentFileEncoding = GetEncodeStringInBase64(contentFile);

            CPRO_TECH_ACCOUNT_ENCODED = GetEncodeStringInBase64($"{CPRO_TECH_LOGIN}:{CPRO_TECH_PASS}");

            var dataJson = $"{{\"idUtilisateurCourant\":0,"
                + $"\"fichierFlux\":\"{contentFileEncoding}\","
                + $"\"nomFichier\":\"{Path.GetFileName(pathFileXML)}\","
                + $"\"syntaxeFlux\":\"IN_DP_E1_UBL_INVOICE\","
                + $"\"avecSignature\":false}}";

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(URL_UBL_SEND),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token.AccessToken}" },
                    { "cpro-account", CPRO_TECH_ACCOUNT_ENCODED }
                },
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };

            FlowDeposit ret = null;

            try
            {
                HttpResponseMessage resultContent = client.SendAsync(httpRequestMessage).Result;
                var jsonContent = resultContent.Content.ReadAsStringAsync().Result;
                ret = JsonConvert.DeserializeObject<FlowDeposit>(jsonContent);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }

            return ret;
        }

        public FlowDetail CheckUBL(FlowDeposit flow, Token token)
        {
            var dataJson = $"{{\"numeroFluxDepot\":\"{flow.NumeroFluxDepot}\","
                + $"\"syntaxeFlux\":\"{flow.SyntaxeFlux}\"}}";

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(URL_GET_STATUS_INVOICE),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token.AccessToken}" },
                    { "cpro-account", CPRO_TECH_ACCOUNT_ENCODED }
                },
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };

            FlowDetail ret = null;

            try
            {
                HttpResponseMessage resultContent = client.SendAsync(httpRequestMessage).Result;
                var jsonContent = resultContent.Content.ReadAsStringAsync().Result;
                ret = JsonConvert.DeserializeObject<FlowDetail>(jsonContent);
            } 
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }

            return ret;
        }

        private string GetEncodeStringInBase64(string data)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            string ret = Convert.ToBase64String(plainTextBytes);

            return ret;
        }
    }
}
