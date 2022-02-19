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

        public static void Activate(string email, string key = null)
        {
            if (_activationClient == null)
            {
                _activationClient = new ActivationClient();
            }
            if (!_activationClient.IsRegistered((ushort)ProductsEnum.IGNQuery))
            {
                _activationClient.Init(email, key);
            }
            _activationClient.Register((ushort)ProductsEnum.IGNQuery);
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
