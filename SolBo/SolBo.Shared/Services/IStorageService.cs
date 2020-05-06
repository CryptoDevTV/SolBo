using System.Collections.Generic;

namespace SolBo.Shared.Services
{
    public interface IStorageService
    {
        void SaveValue(decimal val);
        ICollection<decimal> GetValues();
        void SetPath(string path);
    }
}