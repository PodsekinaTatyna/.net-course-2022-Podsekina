using ModelsDb;

namespace Services.Storages
{
    public interface IClientStorage : IStorage<ClientDb>
    {
        public BankContext Data { get;}

        public void AddAccount(AccountDb account);
        public void UpdateAccount(AccountDb account);
        public void DeleteAccount(AccountDb account);

    }
}
