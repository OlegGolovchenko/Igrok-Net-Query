using IGNActivation.Client;

namespace IGNQuery
{
    internal static class Activation
    {
        private static ActivationClient _activationClient;

        public static void Activate(string email)
        {
            _activationClient = new ActivationClient(email);
            _activationClient.ActivateAsync();
        }

        internal static bool IsActive 
        { 
            get 
            {
                return (_activationClient != null) && _activationClient.IsRegisteredAsync();
            } 
        }
    }
}
