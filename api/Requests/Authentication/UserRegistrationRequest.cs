﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Requests.Authentication
{
    public class UserRegistrationRequest
    {
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        public string OAuthId { get; set; }
        public string PhotoUrl { get; set; }
    }
}