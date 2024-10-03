using Chromecaster.Datatypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chromecaster.Services
{
    public class ChromecastClient
    {
        private readonly ChromecastController _controller;
        private readonly ChromecastLocator _locator;
        private readonly string _applicationID;
        private readonly string _namespaceName;

        public ChromecastClient(string applicationID, string namespaceName)
        {
            _locator = new ChromecastLocator();
            _controller = new ChromecastController();
            _applicationID = applicationID;
            _namespaceName = namespaceName;
        }

        public async Task CastUrl(ChromecastDevice device, string url)
        {
            var message = new Message
            {
                ApplicationID = _applicationID,
                NamespaceName = _namespaceName,
                Url = url
            };
            
            await _controller.SendMessageAsync(device, message);        
        }

        public async Task<List<ChromecastDevice>> FindReceiversAsync() => await _locator.DiscoverDevicesAsync();
    }
}
