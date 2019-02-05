namespace DotNETDevOps.Identity.AzureB2CUserService
{
    /// <summary>
    /// OData Result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ODataResult<T>
    {
        /// <summary>
        /// The odata result
        /// </summary>
        public T Value { get; set; }


    }

}
