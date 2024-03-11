using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

/// <summary>
/// BearerToken 验证处理
/// </summary>
public class BearerTokenHandler : TokenHandler
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    /// <summary>
    /// 异步验证程序
    /// </summary>
    /// <param name="token"></param>
    /// <param name="validationParameters"></param>
    /// <returns></returns>
    public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    {
        try
        {
            _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                return Task.FromResult(new TokenValidationResult() { IsValid = false });

            return Task.FromResult(new TokenValidationResult
            {
                IsValid = true,
                ClaimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, JwtBearerDefaults.AuthenticationScheme),

                // If you do not add SecurityToken to the result, then our validator will fire, return a positive result, 
                // but the authentication, in general, will fail.
                SecurityToken = jwtSecurityToken,
            });
        }

        catch (Exception e)
        {
            return Task.FromResult(new TokenValidationResult 
            {
                IsValid = false,
                Exception = e,
            });
        }
    }
}