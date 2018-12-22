namespace Surging.Core.CPlatform.Runtime.Server
{
    public interface IServiceTokenGenerator
    {
        string GeneratorToken(string code);

        string GetToken();
    }
}