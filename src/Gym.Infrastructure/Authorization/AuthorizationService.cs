using System.Net;
using Gym.Infrastructure.Abstractions.Interfaces;
using Gym.Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using Saritasa.Tools.Domain.Exceptions;

namespace Gym.Infrastructure.Authorization;

/// <summary>
/// Authorization service.
/// </summary>
public class AuthorizationService : IAuthorizationService, IDisposable
{
    private readonly RestClient client;
    private string? accessToken;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="apiSettingsOptions"></param>
    public AuthorizationService(IOptions<ApiSettings> apiSettingsOptions)
    {
        var options = new RestClientOptions(apiSettingsOptions.Value.BackendUrl);
        client = new RestClient(options);
    }

    /// <inheritdoc />
    public async Task<string> AuthenticateAsync(string login, string password)
    {
        var request = new RestRequest("auth");
        request.AddJsonBody(new
        {
            Email = login,
            Password = password
        });
        try
        {
            var response = await client.PostAsync<AuthorizationResponse>(request);
            accessToken = response!.Token;
            RestoreJwtToken(accessToken);
        }
        catch (HttpRequestException exception)
        {
            throw new InfrastructureException("Email or password is incorrect.", exception);
        }

        return accessToken;
    }

    /// <inheritdoc />
    public async Task<Guid?> GetMeAsync()
    {
        var request = new RestRequest("auth");
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        try
        {
            var response = await client.GetAsync<GetMeResponse>(request);
            return response.Id;
        }
        catch (HttpRequestException exception)
        {
            if (accessToken == null)
            {
                throw new InfrastructureException("You are not authorized", exception);
            }
            string newToken = await RefreshTokenAsync();
            if (string.IsNullOrEmpty(newToken))
            {
                throw new InfrastructureException("You are not authorized", exception);
            }
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<string> RefreshTokenAsync()
    {
        var request = new RestRequest("auth");
        request.AddJsonBody(new
        {
            Token = accessToken,
        });
        try
        {
            var response = await client.PutAsync<AuthorizationResponse>(request);
            accessToken = response!.Token;
            RestoreJwtToken(accessToken);
        }
        catch (HttpRequestException exception)
        {
            throw new InfrastructureException("Cannot update token", exception);
        }

        return accessToken;
    }

    /// <inheritdoc />
    public void RestoreJwtToken(string token)
    {
        accessToken = token;
        client.Authenticator = new JwtAuthenticator(token);
    }

    /// <inheritdoc />
    public void Dispose() => client.Dispose();
}
