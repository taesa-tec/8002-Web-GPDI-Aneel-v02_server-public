﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PeD.Auth
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }

        public SigningConfigurations(string basehash)
        {
            var sha256Hash = SHA256.Create();
            var data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(basehash));
            Key = new SymmetricSecurityKey(data);
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}