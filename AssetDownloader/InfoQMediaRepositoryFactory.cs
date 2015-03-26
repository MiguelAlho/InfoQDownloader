using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloader
{
    public class InfoQMediaRepositoryFactory : IMediaRepositoryFactory
    {
        string basePath;

        public InfoQMediaRepositoryFactory(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentNullException("base path must be provided");
            this.basePath = basePath;
        }

        public IMediaRepository Create(string presentationName)
        {
            return new InfoQWindowsRepository(basePath, presentationName);
        }
    }
}
