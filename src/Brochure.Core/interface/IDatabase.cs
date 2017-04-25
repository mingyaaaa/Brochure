using System.Threading.Tasks;

namespace Brochure.Core
{
    public interface IDatabase
    {
        string ConnectStr { get; set; }
        IEntrity Insert(IEntrity entrity);
        long Delete(object obj);
        long Update(object obj);
        long GetInfoById(string id);
        long Search(string str);
        Task<IEntrity> InsertAsync(IEntrity entrity);
        Task<long> DeleteAsync(object obj);
        Task<long> UpdateaAsync(object obj);
        Task<long> GetInfoByIdAsync(string id);
        Task<long> SearchAsync(string str);
    }
}
