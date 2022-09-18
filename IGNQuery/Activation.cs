using IGNActivation.Client;
using IGNActivation.Client.Interfaces;

namespace IGNQuery
{
    public static class Activation
    {
        private static IActivationClient _activationClient;

        public static void Init(IActivationClient activationClient = null)
        {
            _activationClient = activationClient;
        }

        public static void Activate(string email, string key)
        {
            if (_activationClient == null)
            {
                _activationClient = new ActivationClient();
            }
            _activationClient.Init(email, key);
            if (!_activationClient.IsRegistered((ushort)ProductsEnum.IGNQuery))
            {
                _activationClient.Register((ushort)ProductsEnum.IGNQuery, key);
            }
        }

        public static bool IsActive 
        { 
            get 
            {
                return _activationClient.IsRegistered((ushort)ProductsEnum.IGNQuery);
            } 
        }
    }
}
