using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICalculator
    {
        [OperationContract]
        int Add(string fruit, int quantity);
        [OperationContract]
        int Subtract(string name, int quantity);
        [OperationContract]
        int Get(string name);

    }
}