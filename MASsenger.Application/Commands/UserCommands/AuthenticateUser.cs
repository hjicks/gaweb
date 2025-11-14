using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace MASsenger.Application.Commands.UserCommands
{
    public class AuthenticateUser : IAuthenticateHandler
    {
    }
}