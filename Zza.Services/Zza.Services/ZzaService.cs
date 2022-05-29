using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Zza.Data;
using Zza.Entities;

namespace Zza.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] //percall clean 
    //up the objct from memory as soon as a service call is complete
    public class ZzaService : IZzaService, IDisposable
    {
        readonly ZzaDbContext _Context = new ZzaDbContext();

        
        public List<Customer> GetCustomers()
        {
            return _Context.Customers.ToList();
        }

        public List<Product> GetProducts()
        {
            return _Context.Products.ToList();
        }

        [OperationBehavior(TransactionScopeRequired =true)]
        //it will roll back the transaction if exception occurs
        //will commit the transaction if transaction is completed succesfully or if you levae the method
        //normally

        public void SubmitOrder(Order order)
        {
            _Context.Orders.Add(order);
            order.OrderItems.ForEach(oi => _Context.OrderItems.Add(oi));
            _Context.SaveChanges();

        }

        public void Dispose()   //wcf will call this
        {
            _Context.Dispose();
        }

    }
}
