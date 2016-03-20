using System.IO;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace App
{
    [ExcludeFromCodeCoverage]
    [ContractClassFor(typeof(IFileContentProvider))]
    public abstract class IFileContentProviderContract : IFileContentProvider
    {
        public bool Exists(string relativePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(relativePath), "relativePath is null or empty.");

            return default(bool);
        }

        public Stream GetContent(string relativePath)
        {

            Contract.Requires(!string.IsNullOrEmpty(relativePath), "relativePath is null or empty.");
            Contract.Ensures(Contract.Result<Stream>() != null, "Return value is out of Range");

            return default(Stream);
        }
    }

    [ContractClass(typeof(IFileContentProviderContract))]
    public interface IFileContentProvider
    {
        Stream GetContent(string relativePath);
        bool Exists(string relativePath);
    }
}
