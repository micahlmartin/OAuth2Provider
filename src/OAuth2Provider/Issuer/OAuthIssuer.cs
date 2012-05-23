using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Issuer
{
    public class OAuthIssuer : IOAuthIssuer
    {
        private readonly Encryption _encryptor = new Encryption();

        public string GenerateAccessToken(TokenData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var bytes = GetAccessTokenBytes(data);

            var encryptedBytes  = _encryptor.Encrypt(bytes);

            return encryptedBytes.ToBase65();
        }
        private static byte[] GetAccessTokenBytes(TokenData data)
        {
            var userBytes = BitConverter.GetBytes(data.ResourceOwnerId);
            var tickBytes = BitConverter.GetBytes(data.Timestamp);
            var consumerBytes = BitConverter.GetBytes(data.ConsumerId);

            var bytes = new byte[24];

            var index = 0;
            for (int i = 0; i < 8; i++)
            {
                bytes[index] = tickBytes[i];
                bytes[index + 1] = userBytes[i];
                bytes[index + 2] = consumerBytes[i];

                index += 3;
            }

            return bytes;
        }

        public string GenerateRefreshToken(TokenData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var bytes = GetRefreshTokenBytes(data);

            var encryptedBytes = _encryptor.Encrypt(bytes);

            return encryptedBytes.ToBase65();
        }
        private static byte[] GetRefreshTokenBytes(TokenData data)
        {
            var consumerBytes = BitConverter.GetBytes(data.ConsumerId);
            var userBytes = BitConverter.GetBytes(data.ResourceOwnerId);
            var tickBytes = BitConverter.GetBytes(data.Timestamp);
            var randomBytes = GetRandomBytes(8);

            var bytes = new byte[32];

            var index = 0;
            for (int i = 0; i < 8; i++)
            {
                bytes[index] = tickBytes[i];
                bytes[index + 1] = consumerBytes[i];
                bytes[index + 2] = userBytes[i];
                bytes[index + 3] = randomBytes[i];

                index += 4;
            }

            return bytes;
        }

        private static readonly Random Rand = new Random((int) DateTime.Now.Ticks);

        private static byte[] GetRandomBytes(int count)
        {
            var bytes = new byte[count];
            Rand.NextBytes(bytes);
            return bytes;
        }

        public string GenerateAuthorizationToken(TokenData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var bytes = GetAuthorizationTokenBytes(data);

            var encryptedBytes = _encryptor.Encrypt(bytes);

            var encryptedString = encryptedBytes.ToBase65();

            if (data.RedirectUri.HasValue())
                encryptedString += "|" + _encryptor.Encrypt(Encoding.ASCII.GetBytes(data.RedirectUri)).ToBase65();

            return encryptedString;
        }
        private static byte[] GetAuthorizationTokenBytes(TokenData data)
        {
            var consumerBytes = BitConverter.GetBytes(data.ConsumerId);
            var randomBytes1 = GetRandomBytes(8);
            var tickBytes = BitConverter.GetBytes(data.Timestamp);
            var userBytes = BitConverter.GetBytes(data.ResourceOwnerId);

            var bytes = new byte[32];

            var index = 0;
            for (int i = 0; i < 8; i++)
            {
                bytes[index] = tickBytes[i];
                bytes[index + 1] = consumerBytes[i];
                bytes[index + 2] = randomBytes1[i];
                bytes[index + 3] = userBytes[i];

                index += 4;
            }

            return bytes;
        }

        public TokenData DecodeAccessToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            var encryptedBytes = token.FromBase65();

            var decryptedBytes = _encryptor.Decrypt(encryptedBytes);

            return GetAccessTokenFromBytes(decryptedBytes);
        }
        private static TokenData GetAccessTokenFromBytes(byte[] bytes)
        {
            var userBytes = new byte[8];
            var tickBytes = new byte[8];
            var consumerBytes = new byte[8];

            var index = 0;
            for (var i = 0; i < 8; i++)
            {
                tickBytes[i] = bytes[index];
                userBytes[i] = bytes[index + 1];
                consumerBytes[i] = bytes[index + 2];

                index += 3;
            }

            return new TokenData
                       {
                           ResourceOwnerId = BitConverter.ToInt64(userBytes, 0),
                           Timestamp = BitConverter.ToInt64(tickBytes, 0),
                           ConsumerId = BitConverter.ToInt64(consumerBytes, 0)
                       };
        }

        public TokenData DecodeRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            var encryptedBytes = token.FromBase65();

            var decryptedBytes = _encryptor.Decrypt(encryptedBytes);

            return GetRefreshTokenFromBytes(decryptedBytes);
        }
        private static TokenData GetRefreshTokenFromBytes(byte[] bytes)
        {
            var consumerBytes = new byte[8];
            var userBytes = new byte[8];
            var tickBytes = new byte[8];

            var index = 0;
            for (int i = 0; i < 8; i++)
            {
                tickBytes[i] = bytes[index];
                consumerBytes[i] = bytes[index + 1];
                userBytes[i] = bytes[index + 2];

                //skip random byte

                index += 4;
            }

            return new TokenData
            {
                ConsumerId = BitConverter.ToInt64(consumerBytes, 0),
                ResourceOwnerId = BitConverter.ToInt64(userBytes, 0),
                Timestamp = BitConverter.ToInt64(tickBytes, 0)
            };
        }

        public TokenData DecodeAuthorizationToken(string token)
        {

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            var splitterIndex = token.IndexOf('|');
            string redirectUri = null;
            if (splitterIndex > -1)
            {
                redirectUri = token.Substring(splitterIndex + 1);
                token = token.Remove(splitterIndex);
            }

            var encryptedBytes = token.FromBase65();

            var decryptedBytes = _encryptor.Decrypt(encryptedBytes);

            var decodedToken = GetAuthorizationTokenFromBytes(decryptedBytes);
            if (redirectUri.HasValue())
                decodedToken.RedirectUri = Encoding.ASCII.GetString(_encryptor.Decrypt(redirectUri.FromBase65()));

            return decodedToken;
        }
        private static TokenData GetAuthorizationTokenFromBytes(byte[] bytes)
        {
            var consumerBytes = new byte[8];
            var tickBytes = new byte[8];
            var userBytes = new byte[8];

            var index = 0;
            for (int i = 0; i < 8; i++)
            {
                tickBytes[i] = bytes[index];
                consumerBytes[i] = bytes[index + 1];
                //skip random bytes
                userBytes[i] = bytes[index + 3];
                

                index += 4;
            }

            return new TokenData
            {
                ConsumerId = BitConverter.ToInt64(consumerBytes, 0),
                Timestamp = BitConverter.ToInt64(tickBytes, 0),
                ResourceOwnerId = BitConverter.ToInt64(userBytes, 0)
            };
        }
    }
}
