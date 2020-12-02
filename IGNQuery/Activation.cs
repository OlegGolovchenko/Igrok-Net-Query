using IGNActivation.Client;

namespace IGNQuery
{
    public static class Activation
    {
        private static ActivationClient _activationClient;

        public static void Activate(string email)
        {
            _activationClient = new ActivationClient(email);
#if NETFULL
            _activationClient.ActivateAsync();
#else
            var result = _activationClient.ActivateAsync();
            result.Wait();
#endif
        }

        public static bool IsActive 
        { 
            get 
            {
#if NETFULL
                return (_activationClient != null) && _activationClient.IsRegisteredAsync();
#else
                var result = _activationClient.IsRegisteredAsync();
                result.Wait();
                return (_activationClient != null) && result.Result;
#endif
            } 
        }
    }
}
