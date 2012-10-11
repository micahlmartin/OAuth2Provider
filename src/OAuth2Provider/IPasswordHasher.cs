namespace OAuth2Provider
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Compares the specified plaintext value with the hash supplied.
        /// </summary>
        /// <param name="plaintext">The plaintext password.</param>
        /// <param name="hashed">The hashed password.</param>
        /// <returns>True if the password is correct, otherwise false.</returns>
        bool CheckPassword(string plaintext, string hashed);
    }
}