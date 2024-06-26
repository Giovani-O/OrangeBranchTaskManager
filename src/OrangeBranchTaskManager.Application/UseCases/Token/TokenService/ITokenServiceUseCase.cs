﻿using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Application.UseCases.Token.TokenService;
public interface ITokenServiceUseCase
{
    JwtSecurityToken Execute(IEnumerable<Claim> claims, IConfiguration config);
}
