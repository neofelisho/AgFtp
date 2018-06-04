using System.Collections.Generic;

namespace JH.AgFtp
{
    public interface IAgSyncService
    {
        /// <summary>
        ///     List files in datetime range, compare size, and then download the necessary parts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetFiles();
    }
}