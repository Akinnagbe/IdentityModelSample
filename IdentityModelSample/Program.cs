using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModelSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AuthorizeUrl().Wait();
        }

        static async Task AuthorizeUrl()
        {
            try
            {
                
                var codeVerifier = CryptoRandom.CreateUniqueId(32);
                var stateVerifier = CryptoRandom.CreateUniqueId(32);
                var nonceVerifier = CryptoRandom.CreateUniqueId(32);
                // discover endpoints from metadata
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44303/");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

               

                var ru = new RequestUrl("https://localhost:44303/connect/authorize");
                var authorizeUrl = ru.CreateAuthorizeUrl(clientId: "d84d0a966e0b470facebd7a6dfa8b6b1",
                    responseType: "code",
                    scope: "openid profile offline_access awesomecareapi",
                    redirectUri: "https://localhost:44372/signin-oidc",
                    responseMode: "form_post",
                    codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256,
                    codeChallenge: codeVerifier.ToSha256(),
                    state: stateVerifier.ToSha256(),
                    nonce: nonceVerifier.ToSha256()
                    );
            }
            catch (Exception ex)
            {

                throw;
            }
        }



    }
}
