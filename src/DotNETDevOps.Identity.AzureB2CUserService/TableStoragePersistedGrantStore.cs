using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SInnovations.Azure.TableStorageRepository;
using SInnovations.Azure.TableStorageRepository.Queryable;
using SInnovations.Azure.TableStorageRepository.TableRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNETDevOps.Identity.AzureB2CUserService
{
    public class DeviceCodeWrapper
    {
        public DeviceCode Item { get;set;}
        public string UserCode { get; set; }
        public string DeviceCode { get; set; }
    }
    public class DeviceFlowStoreContext: TableStorageContext
    {
        private static JsonSerializerSettings jsonSerializer;
        static DeviceFlowStoreContext()
        {
            jsonSerializer = new JsonSerializerSettings();
            jsonSerializer.Converters.Add(new ClaimsPrincipalConverter());
        }
        public DeviceFlowStoreContext(
            ILoggerFactory loggerFactory,
            IEntityTypeConfigurationsContainer container,
            CloudStorageAccount account) :
            base(loggerFactory, container, account)
        {
            this.InsertionMode = InsertionMode.AddOrReplace;
        }

        protected override void OnModelCreating(TableStorageModelBuilder modelbuilder)
        {

            modelbuilder.Entity<DeviceCodeWrapper>()
               .HasKeys(k => new { k.UserCode, k.DeviceCode })
               .WithIndex(k => k.DeviceCode, true, "devicecodestore", "devicecodes")
               .WithPropertyOf(k=>k.Item,deserializer:Deserialize, serializer:Serialize)
               .ToTable("devicecodestore");

            base.OnModelCreating(modelbuilder);
        }

        private Task<EntityProperty> Serialize(DeviceCode arg)
        {
            return Task.FromResult(EntityProperty.GeneratePropertyForByteArray(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(arg, jsonSerializer))));
        }

        private Task<DeviceCode> Deserialize(EntityProperty arg)
        {
            return Task.FromResult(JsonConvert.DeserializeObject<DeviceCode>(Encoding.UTF8.GetString(arg.BinaryValue),jsonSerializer));
        }


        public ITableRepository<DeviceCodeWrapper> DeviceCodes { get; set; }
    }
    public class TableStorageDeviceFlowStore : IDeviceFlowStore
    {
        private readonly DeviceFlowStoreContext deviceFlowStoreContext;

        public TableStorageDeviceFlowStore(DeviceFlowStoreContext deviceFlowStoreContext)
        {
            this.deviceFlowStoreContext = deviceFlowStoreContext ?? throw new System.ArgumentNullException(nameof(deviceFlowStoreContext));
        }

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
           var wrap=await this.deviceFlowStoreContext.DeviceCodes.WithPrefix($"devicecodes__{deviceCode}").FirstOrDefaultAsync();
            return wrap.Item;
        }

        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var wrap=await this.deviceFlowStoreContext.DeviceCodes.Where(c=>c.UserCode==userCode) .FirstOrDefaultAsync();
            return wrap.Item;
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var wrap= await this.deviceFlowStoreContext.DeviceCodes.WithPrefix($"devicecodes__{deviceCode}").FirstOrDefaultAsync();
            await this.deviceFlowStoreContext.DeviceCodes.Table.ExecuteAsync(TableOperation.Delete(new TableEntity($"devicecodes__{deviceCode}", "") { ETag = "*" }));
            await this.deviceFlowStoreContext.DeviceCodes.Table.ExecuteAsync(TableOperation.Delete(new TableEntity($"{wrap.UserCode}", "") { ETag = "*" }));

        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            this.deviceFlowStoreContext.DeviceCodes.Add(new DeviceCodeWrapper { DeviceCode = deviceCode, UserCode = userCode, Item = data });
            await this.deviceFlowStoreContext.SaveChangesAsync();
        }

        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            var wrap = await this.deviceFlowStoreContext.DeviceCodes.Where(c => c.UserCode == userCode).FirstOrDefaultAsync();
            this.deviceFlowStoreContext.DeviceCodes.Add(new DeviceCodeWrapper { DeviceCode = wrap.DeviceCode, UserCode = userCode, Item = data });
            await this.deviceFlowStoreContext.SaveChangesAsync();
        }
    }
    public class TableStoragePersistedGrantStore : IPersistedGrantStore
    {
        private readonly PersistedGrantContext _context;
        public TableStoragePersistedGrantStore(PersistedGrantContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return await _context.PersistedGrants.Where(k => k.SubjectId == subjectId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await _context.PersistedGrants.FindByIndexAsync(key).ConfigureAwait(false);
        }
        public async Task StoreAsync(PersistedGrant grant)
        {

            this._context.PersistedGrants.Add(grant);

            await this._context.SaveChangesAsync().ConfigureAwait(false);
        }



        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            return Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {

            this._context.PersistedGrants.Delete(await _context.PersistedGrants.FindByIndexAsync(key).ConfigureAwait(false));
            await Task.WhenAll(
                this._context.PersistedGrants.DeleteByKey(PersistedGrantContext.FixPartitionKey(key), ""),
                this._context.SaveChangesAsync());

        }




    }

}
